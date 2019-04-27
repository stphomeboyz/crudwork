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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using crudwork.MultiThreading;
using System.Threading;
using crudwork.Utilities;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Progress bar for Multi Threading
	/// </summary>
	public partial class MultiThreadingStatusDialog : Form, IMultiThreadingManager
	{
		#region Fields
		private bool confirmExit = false;
		private IMultiThreadingManager manager;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an empty instance with given attributes
		/// </summary>
		public MultiThreadingStatusDialog()
			: this(null, false)
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="closeOnCompletion"></param>
		public MultiThreadingStatusDialog(IMultiThreadingManager manager, bool closeOnCompletion)
		{
			InitializeComponent();

			this.manager = manager;
			if (this.manager != null)
				this.manager.WorkStatusEvent += new WorkStatusEventHandler(manager_WorkStatusEvent);

			this.CloseOnCompletion = closeOnCompletion;
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="manager"></param>
		public MultiThreadingStatusDialog(IMultiThreadingManager manager)
			: this(manager, false)
		{
		}
		#endregion

		#region Custom Events
		// 2008-06-18: implemented lock mechanism in the MT Manager to relieve the consumers of this responsibility.
		// 2008-10-16: re-enable the lock on host.  The win host _may_ needs to manage its own message pump.
		private static object lockObj = new object();
		void manager_WorkStatusEvent(object sender, WorkStatusEventArgs e)
		{
			// 2008-10-16: too many messages can cause application to lock up.  We need to
			// apply a filter to make sure we don't cram the message pump.  Explicitly leave
			// this here to prevent this from happen -- in the case if the user set
			// IsSlaveCanRaiseWorkStatus = true.
			if (e.ThreadName != "Main")
				return;

			if (InvokeRequired)
			{
				BeginInvoke(new WorkStatusEventHandler(manager_WorkStatusEvent), sender, e);
				return;
			}

			lock (lockObj)
			{
				//Random r = new Random();
				//Thread.Sleep(r.Next(0, 100));

				Percentage = e.Percentage;
				txtDetails.AppendText(string.Format("{0} - {1}{2}",
							DateTime.Now.ToString("HH:mm:ss.fff"), e.ToString(), Environment.NewLine));
			}
		}
		#endregion

		#region Application Events
		private void MultiThreadingStatusDialog_Load(object sender, EventArgs e)
		{
			MultiThreadingStatusDialog_Resize(sender, e);

			// show no details on start up
			btnShowDetail.Tag = false;
			btnShowDetail_Click(sender, e);

			// start MultiTask now.
			StartAsync();
		}

		private void MultiThreadingStatusDialog_Resize(object sender, EventArgs e)
		{
			// keep original height for more/less view
			tabControl1.Tag = tabControl1.Height;
		}

		/// <summary>
		/// Do special action before closing the form
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (!confirmExit)
				return;

			// if no tasks are running, don't nag.

			DialogResult dr = MessageBox.Show("Do you wish to cancel?", "Warning", MessageBoxButtons.OKCancel);
			switch (dr)
			{
				case DialogResult.OK:
					//e.Cancel = false;
					this.confirmExit = false;
					this.manager.UserAbort = true;
					//btnCancel_Click(this, EventArgs.Empty);
					break;

				case DialogResult.Cancel:
				default:
					e.Cancel = true;
					break;
			}
		}

		private void btnShowDetail_Click(object sender, EventArgs e)
		{
			bool showMore = (bool)btnShowDetail.Tag;
			int height = (int)tabControl1.Tag;

			if (height == 0)
				height = 200;

			tabControl1.Visible = showMore;
			this.Height += height * (showMore ? 1 : -1);
			btnShowDetail.Tag = !showMore;

			btnShowDetail.Text = "" + (showMore ? "Less" : "More") + " ...";

			System.Diagnostics.Debug.Print("showMore={0} Height={1}", showMore, this.Height);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOkay_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void chkCloseOnCompletion_Click(object sender, EventArgs e)
		{
			this.CloseOnCompletion = chkCloseOnCompletion.Checked;
		}
		#endregion

		#region Public Properties
		private string oldTitle = string.Empty;

		/// <summary>
		/// Get or set the percent complete
		/// </summary>
		public decimal Percentage
		{
			get
			{
				return pbOverall.Value;
			}
			set
			{
				pbOverall.Value = (int)value;
				if (string.IsNullOrEmpty(oldTitle))
					oldTitle = Title;
				Title = string.Format("{0:F0}% Completed - {1}", value, oldTitle);
			}
		}

		/// <summary>
		/// Get or set the detail log
		/// </summary>
		public string Detail
		{
			get
			{
				return txtDetails.Text;
			}
			set
			{
				//txtDetails.SuspendLayout();
				txtDetails.Text = value;
				//txtDetails.SelectionStart = txtDetails.Text.Length;
				//txtDetails.ScrollToCaret();
				//txtDetails.ResumeLayout();
			}
		}

		/// <summary>
		/// Get or set the error log
		/// </summary>
		public string Error
		{
			get
			{
				return this.txtErrors.Text;
			}
			set
			{
				//txtErrors.SuspendLayout();
				txtErrors.Text = value;
				//txtErrors.SelectionStart = txtErrors.Text.Length;
				//txtErrors.ScrollToCaret();
				//txtErrors.ResumeLayout();
			}
		}

		/// <summary>
		/// Get or set a value that indicate whether or not to close form upon completion
		/// </summary>
		public bool ConfirmExit
		{
			get
			{
				return this.confirmExit;
			}
			set
			{
				this.confirmExit = value;
			}
		}

		/// <summary>
		/// Get or set the message title
		/// </summary>
		public string Message
		{
			get
			{
				return lblMessage.Text;
			}
			set
			{
				lblMessage.Text = value;
			}
		}

		/// <summary>
		/// Get or set the window title
		/// </summary>
		public string Title
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text = value;
			}
		}

		/// <summary>
		/// Get or set a value indicating to close dialog box if the task completes successfully
		/// </summary>
		public bool CloseOnCompletion
		{
			get
			{
				return chkCloseOnCompletion.Checked;
			}
			set
			{
				chkCloseOnCompletion.Checked = value;
			}
		}
		#endregion

		#region IMultiThreading Members
		/// <summary>
		/// start the process in multiple threads, wait for completion, and merge output.
		/// </summary>
		public void StartAsync()
		{
			BackgroundTask.Start(
				null,
				delegate(object sender, DoWorkEventArgs e)
				{
					// main code goes here...
					try
					{
						manager.StartAsync();
					}
					catch (Exception ex)
					{
						throw ex;
					}
				},
				delegate(object sender, EventArgs e)
				{
					// initialize code goes here...
					this.confirmExit = true;
					this.btnOkay.Enabled = false;
					this.btnCancel.Enabled = true;
					Message = "Please wait while processing...";
					Title = "Task Running...";
				},
				delegate(object sender, ProgressChangedEventArgs e)
				{
					// update progress code goes here...
				},
				delegate(object sender, RunWorkerCompletedEventArgs e)
				{
					// cleanup code goes here...
					this.confirmExit = false;
					this.btnOkay.Enabled = true;
					this.btnCancel.Enabled = false;

					int errors = manager.Log.Length;

					if (errors == 0 && this.CloseOnCompletion)
					{
						btnOkay_Click(sender, EventArgs.Empty);
					}


					if (errors == 0)
					{
						Title = "Task Completed";
						Message = "The task completed successfully.  Click OK to continue.";
					}
					else
					{
						Title = "Task Completed With Errors";
						Message = string.Format("There were {0} errors.  Click the ERROR tab for more detail.", errors);
						btnShowDetail.Tag = true;
						btnShowDetail_Click(this, EventArgs.Empty);
					}

					Error = StringUtil.StringArrayToString(manager.Log,
						string.Format("{0}{0}{1}{0}", Environment.NewLine, "".PadRight(40, '-')));
				});
		}

		/// <summary>
		/// TEST PURPOSE ONLY: start the process in a single thread and merge output.
		/// </summary>
		public void Start()
		{
			BackgroundTask.Start(
				null,
				delegate(object sender, DoWorkEventArgs e)
				{
					// main code goes here...
					try
					{
						manager.Start();
					}
					catch (Exception ex)
					{
						throw ex;
					}
				},
				delegate(object sender, EventArgs e)
				{
					// initialize code goes here...
					this.confirmExit = true;
				},
				delegate(object sender, ProgressChangedEventArgs e)
				{
					// update progress code goes here...
				},
				delegate(object sender, RunWorkerCompletedEventArgs e)
				{
					// cleanup code goes here...
					this.confirmExit = false;
					//this.Close();
				});
		}

		///// <summary>
		///// subscribe or unsubscribe to the Stop event.
		///// </summary>
		//public event EventHandler Stop;

		/// <summary>
		/// start the process in multiple threads, wait for completion, and merge output.
		/// </summary>
		void IMultiThreadingManager.StartAsync()
		{
			StartAsync();
		}

		/// <summary>
		/// TEST PURPOSE ONLY: start the process in a single thread and merge output.
		/// </summary>
		void IMultiThreadingManager.Start()
		{
			Start();
		}

		event EventHandler IMultiThreadingManager.Stop
		{
			add
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
			remove
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
		}

		/// <summary>
		/// subscribe or unsubscribe to the WorkStatus event
		/// </summary>
		event WorkStatusEventHandler IMultiThreadingManager.WorkStatusEvent
		{
			add
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
			remove
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
		}

		/// <summary>
		/// Get or set a value to abort the task
		/// </summary>
		bool IMultiThreadingManager.UserAbort
		{
			get
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
			set
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
		}

		/// <summary>
		/// Get the log information
		/// </summary>
		string[] IMultiThreadingManager.Log
		{
			get
			{
				throw new NotImplementedException("this feature is not supported or not yet implemented");
			}
		}

		#endregion
	}
}