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
using System.Text;
using System.Windows.Forms;
using crudwork.DataAccess;
using System.Diagnostics;
using crudwork.DataSetTools;
using crudwork.Utilities;
using crudwork.Controls.Properties;

namespace crudwork.Controls.DatabaseUC
{
	/// <summary>
	/// The query analyzer command interface
	/// </summary>
	public partial class QueryAnalyzer : UserControl
	{
		private QueryManager qm;
		private DataSet ds;
		private bool queryModified;
		private string queryFilename;

		#region Constructors
		/// <summary>
		/// Create a new instance
		/// </summary>
		public QueryAnalyzer()
		{
			InitializeComponent();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Get or set the DataSource; and create a new QueryManager based on this.
		/// </summary>
		public DataSet DataSource
		{
			get
			{
				return this.ds;
			}
			set
			{
				this.ds = value;
				simpleDataSetViewer1.DataSource = value;

				if (qm == null)
					qm = new QueryManager(value);

				qm.DataSet = value;
			}
		}

		/// <summary>
		/// Get or set the query statement
		/// </summary>
		public string Query
		{
			get
			{
				if (!string.IsNullOrEmpty(txtQuery.SelectedText))
					return txtQuery.SelectedText;
				else
					return txtQuery.Text;
			}
			set
			{
				txtQuery.Text = value;
			}
		}

		/// <summary>
		/// Run the Query
		/// </summary>
		public void RunQuery()
		{
			try
			{
				FormUtil.Busy(this, true);
				string query = Query;

				if (string.IsNullOrEmpty(query))
					throw new ArgumentNullException("query");

				if (this.qm == null)
					this.qm = new QueryManager(this.ds);

				QueryResultSet results = qm.RunBatch(query);
				this.DataSource = qm.DataSet;

				ShowQueryResult(results);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		/// <summary>
		/// Commit any changes on the dataset object
		/// </summary>
		public void Commit()
		{
			if (qm == null)
				return;
			qm.Commit();
		}

		/// <summary>
		/// Roll back any changes on the dataset object
		/// </summary>
		public void Rollback()
		{
			if (qm == null)
				return;
			qm.Rollback();
		}

		/// <summary>
		/// Show or hide the Result panel
		/// </summary>
		public void ToggleResultPanel()
		{
			SetResultPanel(!splitContainer1.Panel2Collapsed);
		}
		#endregion

		#region Private methods
		private void SetResultPanel(bool value)
		{
			splitContainer1.Panel2Collapsed = value;

			if (value)
			{
				tsbToggleResults.Image = Resources.Collapse_large;
				tsbToggleResults.Text = "Show Results";
			}
			else
			{
				tsbToggleResults.Image = Resources.Expand_large;
				tsbToggleResults.Text = "Hide Results";
			}
		}

		/// <summary>
		/// Show the results on the result panel
		/// </summary>
		/// <param name="results"></param>
		private void ShowQueryResult(QueryResultSet results)
		{
			flpResults.Controls.Clear();
			for (int i = 0; i < results.Count; i++)
			{
				QueryResult result = results[i];
				QueryResultsViewer v = new QueryResultsViewer(result);
				flpResults.Controls.Add(v);
			}

			flpResults_SizeChanged(this, EventArgs.Empty);
		}
		#endregion

		#region Application Events

		private void QueryAnalyzer_Load(object sender, EventArgs e)
		{
			SetResultPanel(false);
		}

		private void tsbNewFile_Click(object sender, EventArgs e)
		{
			try
			{
				if (queryModified && ControlManager.Confirm("Do you want to save change?"))
				{
					tsbSaveFile_Click(sender, e);
					if (queryModified)
						return; // TODO: need to confirm action: user click abort??
				}

				txtQuery.Clear();
				queryFilename = string.Empty;
				queryModified = false;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void tsbOpenFile_Click(object sender, EventArgs e)
		{
			try
			{
				using (OpenFileDialog f = new OpenFileDialog())
				{
					f.Filter = "SQL Query (*.SQL)|*.SQL|All Files|*.*";
					f.Multiselect = false;

					if (f.ShowDialog() != DialogResult.OK)
						return;

					txtQuery.LoadFile(f.FileName, RichTextBoxStreamType.PlainText);
					queryFilename = f.FileName;
					queryModified = false;
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void tsbSaveFile_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(queryFilename))
				{
					using (SaveFileDialog f = new SaveFileDialog())
					{
						f.Filter = "SQL Query (*.SQL)|*.SQL";
						if (f.ShowDialog() != DialogResult.OK)
							return;

						queryFilename = f.FileName;
					}
				}

				txtQuery.SaveFile(queryFilename, RichTextBoxStreamType.PlainText);
				queryModified = false;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void tsbRun_Click(object sender, EventArgs e)
		{
			try
			{
				RunQuery();
				SetResultPanel(false);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void tsbToggleResults_Click(object sender, EventArgs e)
		{
			ToggleResultPanel();
		}

		private void tsbHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Help");
		}

		private void flpResults_SizeChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < flpResults.Controls.Count; i++)
			{
				Control c = flpResults.Controls[i];
				c.Width = flpResults.ClientRectangle.Width;
			}
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Debug.WriteLine("Splitter: " + e.SplitX + " " + e.SplitY);
		}
		#endregion

		private void txtQuery_TextChanged(object sender, EventArgs e)
		{
			queryModified = true;
		}
	}
}
