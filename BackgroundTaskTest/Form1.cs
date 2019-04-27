using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.MultiThreading;
using System.Threading;
using crudwork.Utilities;

namespace BackgroundTaskTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			btnStart.Enabled = true;
			btnCancel.Enabled = false;

			textBox1.Text = (42 * 1000).ToString();
		}

		private void btnStart_Click(object senderX, EventArgs eX)
		{
			int loop;
			if (!int.TryParse(textBox1.Text, out loop))
				loop = 42 * 1000;
			Start1(loop);
		}

		#region Start a background worker - via anonymous online blocks (using delegates or lamda expressions)
		private void Start1(object argument)
		{
			#region Set your CancelButton, ProgressBar, and StatusLabel here...
			Form activeForm = this;	// set myForm=this;
			Button cancelButton = btnCancel;
			ToolStripProgressBar progressBar = toolStripProgressBar1;
			ToolStripLabel statusLabel = toolStripStatusLabel1;
			// disable these controls when running...
			Control[] userControls = new Control[] { textBox1, btnStart };
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
					//Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

					#region Do your work here...
					int maxValue = Convert.ToInt32(e.Argument);
					var sr = new StatusReport();
					sr.SetBackgroundWorker(w, e, 1);

					var r = new Random(DateTime.Now.Millisecond);

					sr.ReportProgress("Processing", 0, maxValue, 0);

					for (int i = 0; i < maxValue; i++)
					{
						sr.Value++;
						sr.ReportProgress();

						Thread.Sleep(r.Next(2));
					}

					sr.ReportProgress("Done", 0, maxValue, maxValue);
					#endregion
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
					txtLog.AppendText(string.Format("{0} : {1:N0}" + Environment.NewLine, sr, sr.Value));
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
	}
}
