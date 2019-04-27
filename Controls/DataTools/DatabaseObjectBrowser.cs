// crudwork
// Copyright 2004 by Steve T. Pham (http://www.crudwork.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with This program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.DataAccess;
using crudwork.Utilities;
using crudwork.MultiThreading;
using System.Threading;
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	/// <summary>
	/// Database object browser
	/// </summary>
	public partial class DatabaseObjectBrowser : UserControl
	{
		private DataSet ds = new DataSet();

		private Form parentControl;
		private ToolStripSplitButton cancelButton;
		private ToolStripProgressBar progressBar;
		private ToolStripStatusLabel progressLabel;
		private Control[] lockControls;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public DatabaseObjectBrowser()
		{
			InitializeComponent();
		}

		private void DatabaseObjectsBrowser_Load(object sender, EventArgs e)
		{
			dsv.TablenameChanged += new TablenameChangedEventHandler(dsv_TablenameChanged);
			lockControls = dsv.SelectionControls;
		}

		void dsv_TablenameChanged(object sender, TablenameChangedEventArgs e)
		{
			Start1(e.TableName);
		}

		#region Start a background worker - via anonymous online blocks (using delegates or lamda expressions)
		private void Start1(object argument)
		{
			#region Set your CancelButton, ProgressBar, and StatusLabel here...
			Form activeForm = this.parentControl;	// set myForm=this; 
			ToolStripSplitButton cancelButton = this.cancelButton;
			ToolStripProgressBar progressBar = this.progressBar;
			ToolStripStatusLabel statusLabel = this.progressLabel;
			// disable these controls when running...
			Control[] userControls = lockControls;
			#endregion

			bool isRunning = false;
			EventHandler cancel = null;
			FormClosingEventHandler formClosing = null;

			BackgroundTask.Start(
				argument,
				delegate(object sender, DoWorkEventArgs e)
				//(object sender, DoWorkEventArgs e) =>
				{
					#region DoWorkEventHandler - main code goes here...
					// IMPORTANT: Do not access any GUI controls here.  (This code is running in the background thread!)
					var w = (BackgroundWorker)sender;
					Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

					string tablename = e.Argument.ToString();
					var dt = ds.Tables[tablename];

					var df = new DataFactory(Provider, ConnectionString);
					df.TestConnection();

					int maxRows = (int)df.ExecuteScalar("select count(*) from " + tablename);
					int nr = 0;

					var sr = new StatusReport();
					sr.SetBackgroundWorker(w, e, 1);
					sr.ReportProgress("Loading", 0, maxRows, 0);

					string query = "select * from " + tablename + " (nolock)";
					foreach (var item in df.ExecuteReader(query))
					{
						nr++;

						if (sr.CanReportProgress())
						{
							sr.Value = nr;
							sr.ReportProgress();
						}

						var dr = dt.NewRow();
						DataUtil.CopyRow(item, dr);
						dt.Rows.Add(dr);
					}

					sr.ReportProgress("Complete", 0, maxRows, maxRows);

					#endregion
				},
				delegate(object sender, EventArgs e)
				//(object sender, DoWorkEventArgs e) =>
				{
					#region EventHandler - initialize code goes here...
					// It is safe to access the GUI controls here...
					progressBar.Visible = true;
					progressBar.Minimum = progressBar.Maximum = progressBar.Value = 0;
					isRunning = true;

					#region Hookup event handlers for Button.Canceland Form.FormClosing
					var w = sender as BackgroundWorker;
					if (w != null && w.WorkerSupportsCancellation)
					{
						if (cancelButton != null)
						{
							cancel = (object sender2, EventArgs e2) =>
							{
								//var w = sender2 as BackgroundWorker;
								string msg = "The application is still busy processing.  Do you want to stop it now?";
								if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
									w.CancelAsync();
							};
							cancelButton.Click += cancel;
							cancelButton.Enabled = true;
							cancelButton.Visible = true;
						}
					}

					// also, confirm the user if the user clicks the close button.
					if (activeForm != null)
					{
						formClosing = (object sender2, FormClosingEventArgs e2) =>
						{
							if (!isRunning)
								return;

							string msg = "The application is still busy processing.  Do you want to close the application anyway?";
							e2.Cancel = MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
								MessageBoxDefaultButton.Button2) != DialogResult.Yes;
						};
						activeForm.FormClosing += formClosing;
					}
					#endregion

					FormUtil.Busy(activeForm, userControls, true);
					#endregion
				},
				delegate(object sender, ProgressChangedEventArgs e)
				//(object sender, ProgressChangedEventArgs e) =>
				{
					#region ProgressChangedEventHandler - update progress code goes here...
					// It is safe to access the GUI controls here...
					var sr = e.UserState as StatusReport;
					if (sr == null)
						return;

					StatusReport.UpdateStatusReport(progressBar, sr);
					statusLabel.Text = sr.ToString();
					activeForm.Refresh();
					#endregion
				},
				delegate(object sender, RunWorkerCompletedEventArgs e)
				//(object sender, RunWorkerCompletedEventArgs e) =>
				{
					#region RunWorkerCompletedEventHandler - cleanup code goes here...
					// It is safe to access the GUI controls here...
					try
					{
						if (e.Error != null)
							throw e.Error;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), ex.Message);
					}
					finally
					{
						FormUtil.Busy(activeForm, userControls, false);
						progressBar.Visible = false;

						if (cancel != null)
						{
							cancelButton.Click -= cancel;
							cancelButton.Enabled = false;
							cancelButton.Visible = false;
						}
						if (formClosing != null)
						{
							activeForm.FormClosing -= formClosing;
						}

						isRunning = false;
						statusLabel.Text = "Ready";
					}
					#endregion
				});
		}
		#endregion

		/// <summary>
		/// Refresh the table list
		/// </summary>
		public void RefreshTableList()
		{
			if (string.IsNullOrEmpty(ConnectionString))
				throw new ArgumentNullException("ConnectionString");
			if (Provider == DatabaseProvider.Unspecified)
				throw new ArgumentException("Provider must be specified");

			var df = new DataFactory(Provider, ConnectionString);
			df.TestConnection();

			ds.Clear();

			var tdl = df.Database.GetTables();
			foreach (var item in tdl)
			{
				string query = string.Format("select * from {0} where 1 = 0", item.TableName);
				var dt = df.FillTable(query);
				dt.TableName = item.TableName;
				ds.Tables.Add(dt);
			}

			dsv.DataSource = ds;
		}

		/// <summary>
		/// Get or set the connection string
		/// </summary>
		public string ConnectionString
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the database provider type
		/// </summary>
		[DefaultValue(typeof(DatabaseProvider), "Unspecified")]
		public DatabaseProvider Provider
		{
			get;
			set;
		}

		/// <summary>
		/// Set the controls of the parent's progress bar and progress label
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="cancelButton"></param>
		/// <param name="progressBar"></param>
		/// <param name="progressLabel"></param>
		/// <param name="lockControls"></param>
		public void SetStatusReportControls(Form parent,
			ToolStripSplitButton cancelButton, ToolStripProgressBar progressBar, ToolStripStatusLabel progressLabel,
			Control[] lockControls)
		{
			this.parentControl = parent;
			this.cancelButton = cancelButton;
			this.progressBar = progressBar;
			this.progressLabel = progressLabel;
			this.lockControls = lockControls;
		}
	}
}
