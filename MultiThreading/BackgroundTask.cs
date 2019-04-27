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
using System.Text;
using System.ComponentModel;
using crudwork.Utilities;
using System.Threading;
using System.Diagnostics;
#if !SILVERLIGHT
using System.Windows.Forms;
#else
using System.Windows.Controls;
#endif
using crudwork.Models;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// BackgroundWorker Utility
	/// </summary>
	public class BackgroundTask
	{
		#region Fields
		/// <summary>Main code block</summary>
		public event DoWorkEventHandler OnWork = null;
		/// <summary>Code for initializing prior to starting the main code</summary>
		public event EventHandler OnInitialize = null;
		/// <summary>Code for updating progress</summary>
		public event ProgressChangedEventHandler OnProgress = null;
		/// <summary>Code for cleaning up</summary>
		public event RunWorkerCompletedEventHandler OnComplete = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an empty instance with given attributes
		/// </summary>
		public BackgroundTask()
		{
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Start the backgroundWorker
		/// </summary>
		/// <param name="argument"></param>
		public BackgroundWorker Start(object argument)
		{
			//BackgroundTaskArgument arg = new BackgroundTaskArgument(argument);
			return Start(argument, this.OnWork, this.OnInitialize, this.OnProgress, this.OnComplete);
		}
		#endregion

		#region Public static methods
		/// <summary>
		/// Start a background task
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="OnWork">This code will be run under the context of the background thread</param>
		/// <param name="OnInit">This code will run on the owner's thread</param>
		/// <param name="OnProgress">This code will run on the owner's thread</param>
		/// <param name="OnComplete">This code will run on the owner's thread</param>
		/// <returns></returns>
		public static BackgroundWorker Start(
			object argument,
			DoWorkEventHandler OnWork,
			EventHandler OnInit,
			ProgressChangedEventHandler OnProgress,
			RunWorkerCompletedEventHandler OnComplete
			)
		{
			BackgroundWorker b = NewBackgroundWorker(OnWork, OnProgress, OnComplete);

			if (OnInit != null)
				OnInit(b, EventArgs.Empty);

			b.RunWorkerAsync(argument);
			return b;
		}

		/// <summary>
		/// Start a background task
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="app"></param>
		public static void Start(object argument, BackgroundWorkerToolStripInfo app)
		{
			#region Set your CancelButton, ProgressBar, and StatusLabel here...
			Form activeForm = app.ActiveForm;
			Button cancelButton = app.CancelButton;
			ToolStripProgressBar progressBar = app.ProgressBar;
			ToolStripLabel statusLabel = app.StatusLabel;
			// disable these controls when running...
			Control[] userControls = app.DisableControls;
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

					StatusReport sr = new StatusReport();
					sr.SetBackgroundWorker(w, e, 1);

					app.Callback(new BackgroundWorkerEventArgs(w, e, sr));

					#endregion
				},
				delegate(object sender, EventArgs e)
				//(object sender, DoWorkEventArgs e) =>
				{
					#region EventHandler - initialize code goes here...
					// It is safe to access the GUI controls here...
					if (progressBar != null)
					{
						progressBar.Visible = true;
						progressBar.Minimum = progressBar.Maximum = progressBar.Value = 0;
					}
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

					if (activeForm != null)
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

					if(progressBar != null)
						StatusReport.UpdateStatusReport(progressBar, sr);
					if (statusLabel != null)
						statusLabel.Text = sr.ToString();
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
						if (e.Cancelled)
							throw new ThreadInterruptedException("Thread was interrupted by the user.");
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), ex.Message);
					}
					finally
					{
						if (activeForm != null)
							FormUtil.Busy(activeForm, userControls, false);
						
						if (progressBar != null)
							progressBar.Visible = false;
						if (statusLabel != null)
							statusLabel.Text = "Ready";

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

					}
					#endregion
				});
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Create a new instance of BackgroundWorker
		/// </summary>
		/// <param name="OnWork"></param>
		/// <param name="OnProgress"></param>
		/// <param name="OnComplete"></param>
		/// <returns></returns>
		private static BackgroundWorker NewBackgroundWorker(
			DoWorkEventHandler OnWork,
			ProgressChangedEventHandler OnProgress,
			RunWorkerCompletedEventHandler OnComplete
			)
		{

			SanityCheck.IsNullOrEmpty(OnWork, "OnWork cannot be null.");

			BackgroundWorker worker = new BackgroundWorker();

			// hookup to DoWork event: the main process block
			if (OnWork != null)
				worker.DoWork += OnWork;

			// hookup to ProgressChanged: to receive progress updates
			if (OnProgress != null)
				worker.ProgressChanged += OnProgress;

			// hookup to RunWorkerCompleted: to receive completion status.
			if (OnComplete != null)
				worker.RunWorkerCompleted += OnComplete;

			worker.WorkerReportsProgress = OnProgress != null;
			worker.WorkerSupportsCancellation = true;
			return worker;
		}
		#endregion
	}

	/// <summary>
	/// Status Report class - status report object for passing to client application
	/// </summary>
	public class StatusReport
	{
		#region Properties
		/// <summary>The message to display</summary>
		public string Message
		{
			get;
			set;
		}

		/// <summary>The minimum value (to update the ProgressBar)</summary>
		public int Min
		{
			get;
			set;
		}
		/// <summary>The maximum value (to update the ProgressBar)</summary>
		public int Max
		{
			get;
			set;
		}
		/// <summary>The current value (to update the ProgressBar)</summary>
		public int Value
		{
			get;
			set;
		}

		/// <summary>Get or set a value to indicate that the progressbar has been initialized</summary>
		public bool IsProgressBarInitialized
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public StatusReport()
		{
			this.Message = string.Empty;
			this.Min = -1;
			this.Max = -1;
			this.Value = -1;
		}
		#endregion

		/// <summary>
		/// return a string presentation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0}... {1} of {2} ({3:P0})", Message, Value, Max, (decimal)Value / Max);
		}

		private BackgroundWorker worker = null;
		private DoWorkEventArgs workerArgs = null;
		private double secondInterval;
		private DateTime? then = null;

		/// <summary>
		/// Set the background worker
		/// </summary>
		/// <param name="worker"></param>
		/// <param name="workerArgs"></param>
		/// <param name="secondInterval"></param>
		public void SetBackgroundWorker(BackgroundWorker worker, DoWorkEventArgs workerArgs, double secondInterval)
		{
			if (worker == null)
				throw new ArgumentNullException("worker");
			if (workerArgs == null)
				throw new ArgumentNullException("workerArgs");
			if (secondInterval <= 0)
				throw new ArgumentException("secondInterval must be greater than 0");

			this.worker = worker;
			this.workerArgs = workerArgs;
			this.secondInterval = secondInterval;
		}

		/// <summary>
		/// return true if a status report is due; otherwise, return false.
		/// </summary>
		/// <returns></returns>
		public bool CanReportProgress()
		{
			return CanReportProgress(true);
		}

		/// <summary>
		/// return true if a status report is due; otherwise, return false.
		/// </summary>
		/// <param name="testMode">set true for test-mode only; or set false, to keep track of the last reported time</param>
		/// <returns></returns>
		private bool CanReportProgress(bool testMode)
		{
			if (worker == null)
				return false;

			DateTime now = DateTime.Now;

			if (!then.HasValue)
				then = now;

			//Debug.WriteLine(string.Format("Now={0} Then={1}", now.ToString("s"), then.Value.ToString("s")));
			if (then.Value.AddSeconds(secondInterval) >= now)
				return false;

			if (!testMode)
				then = now;
			return true;
		}

		/// <summary>
		/// Send a progress report to the application, with the min/max/value
		/// values to initialize (or finalize) the progress bar.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="value"></param>
		public bool ReportProgress(string message, int min, int max, int value)
		{
			this.Message = message;
			this.Min = min;
			this.Max = max;
			this.Value = value;
			return ReportProgress(this, true);
		}

		/// <summary>
		/// Send a progress report to the application at each interval.
		/// </summary>
		/// <returns></returns>
		public bool ReportProgress()
		{
			return ReportProgress(this, false);
		}

		/// <summary>
		/// Send a progress report to the application.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="alwaysReport">set true to always send this report; or, set false to send one report per interval</param>
		/// <returns></returns>
		private bool ReportProgress(StatusReport status, bool alwaysReport)
		{
			if (worker == null)
				return false;

			if (worker.WorkerSupportsCancellation && worker.CancellationPending)
			{
				workerArgs.Cancel = true;
				throw new TerminatedByUserException();
			}

			if (worker.WorkerReportsProgress && (alwaysReport || CanReportProgress(false)))
			{
				// need this "int->decimal->int" conversion to report the percentage...  very weird!
				decimal pct = (decimal)Value / Max * 100;
				worker.ReportProgress((int)pct, status);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Raise this StatusReport always
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public bool ReportProgress(string message)
		{
			if (worker == null)
				return false;

			var status = new StatusReport();
			status.Message = message;
			return ReportProgress(status, true);
		}

#if !SILVERLIGHT
		/// <summary>
		/// Update the ToolStripProgressBar with the status report
		/// </summary>
		/// <remarks>Run this method in the context of the GUI's thread</remarks>
		/// <param name="control"></param>
		/// <param name="sr"></param>
		public static void UpdateStatusReport(/*this*/ ToolStripProgressBar control, StatusReport sr)
		{
			if (control == null)
				return;

			if (!sr.IsProgressBarInitialized && sr.Min >= 0 && sr.Max >= 0)
			{
				sr.IsProgressBarInitialized = true;
				control.Minimum = sr.Min;
				control.Maximum = sr.Max;
				control.Value = sr.Value;
				control.Visible = true;
			}
			if (sr.Value >= 0)
				control.Value = sr.Value;
		}
#endif

		/// <summary>
		/// Update the ProgressBar with the status report
		/// </summary>
		/// <remarks>Run this method in the context of the GUI's thread</remarks>
		/// <param name="control"></param>
		/// <param name="sr"></param>
		public static void UpdateStatusReport(/*this*/ ProgressBar control, StatusReport sr)
		{
			if (control == null)
				return;

			if (!sr.IsProgressBarInitialized && sr.Min >= 0 && sr.Max >= 0)
			{
				sr.IsProgressBarInitialized = true;
				control.Minimum = sr.Min;
				control.Maximum = sr.Max;
				control.Value = sr.Value;
#if !SILVERLIGHT
				control.Visible = true;
#else
				control.Visibility = System.Windows.Visibility.Visible;
#endif
			}
			if (sr.Value >= 0)
				control.Value = sr.Value;
		}
	}

#if !SILVERLIGHT
	/// <summary>
	/// Samples for using BackgroundTask
	/// </summary>
	internal class BackgroundTaskSamples
	{
		#region Start a background worker - via anonymous online blocks (using delegates or lamda expressions)
		private void Start1(object argument)
		{
			#region Set your CancelButton, ProgressBar, and StatusLabel here...
			Form activeForm = null;	// set myForm=this; 
			Button cancelButton = new Button();
			ToolStripProgressBar progressBar = new ToolStripProgressBar();
			ToolStripLabel statusLabel = new ToolStripLabel();
			// disable these controls when running...
			Control[] userControls = null;
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

					if (false) // code sample
					{
						int max = (int)e.Argument;
						StatusReport sr = new StatusReport();
						sr.SetBackgroundWorker(w, e, 1);
						sr.ReportProgress("Processing...", 0, max, 0);

						for (int i = 0; i < max; i++)
						{
							sr.Value++;
							sr.ReportProgress();
						}

						sr.ReportProgress("Done");
					}

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
						if (e.Cancelled)
							throw new ThreadInterruptedException("Thread was interrupted by the user.");
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

		#region Start a background worker - via anonymous blocks (using delegates using lamda expressions)
		private void Start2(object argument)
		{
			/*
			 * Create anonymous (using delegates using lamda expressions)
			 * */

			#region Set your CancelButton, ProgressBar, and StatusLabel here...
			Form activeForm = null;	// set myForm=this; 
			Button cancelButton = new Button();
			ToolStripProgressBar progressBar = new ToolStripProgressBar();
			ToolStripLabel statusLabel = new ToolStripLabel();
			// disable these controls when running...
			Control[] userControls = null;
			#endregion

			bool isRunning = false;
			EventHandler cancel = null;
			FormClosingEventHandler formClosing = null;

			DoWorkEventHandler doWork =
				delegate(object sender, DoWorkEventArgs e)
				//(object sender, DoWorkEventArgs e) =>
				{
					#region DoWorkEventHandler - main code goes here...
					// IMPORTANT: Do not access any GUI controls here.  (This code is running in the background thread!)
					var w = (BackgroundWorker)sender;
					//Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

					if (false) // code sample
					{
						int max = (int)e.Argument;
						StatusReport sr = new StatusReport();
						sr.SetBackgroundWorker(w, e, 1);
						sr.ReportProgress("Processing...", 0, max, 0);

						for (int i = 0; i < max; i++)
						{
							sr.Value++;
							sr.ReportProgress();
						}

						sr.ReportProgress("Done");
					}
					#endregion
				};

			EventHandler doInit =
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
				};

			ProgressChangedEventHandler doProgress =
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
					#endregion
				};

			RunWorkerCompletedEventHandler doComplete =
				delegate(object sender, RunWorkerCompletedEventArgs e)
				//(object sender, RunWorkerCompletedEventArgs e) =>
				{
					#region RunWorkerCompletedEventHandler - cleanup code goes here...
					// It is safe to access the GUI controls here...
					try
					{
						if (e.Error != null)
							throw e.Error;
						if (e.Cancelled)
							throw new ThreadInterruptedException("Thread was interrupted by the user.");
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
				};

			BackgroundTask.Start(argument, doWork, doInit, doProgress, doComplete);
		}
		#endregion

		#region Using instance

		/* This is a bare structure (not ideal for use).  Need to revise to meet the followings:
		 * 
		 *		1) subscribe to CancelButton.Click event
		 *		2) subscribe to Form.FormClosing event
		 *		3) update ProgressBar
		 *		4) update StatusLabel
		 *		5) run FormUtil.Busy to disable/enable controls
		 *		...
		 * 
		 * */

		private void Sample3(object argument)
		{
			/*
			 * create an instance
			 * */

			BackgroundTask task = new BackgroundTask();
			task.OnInitialize += new EventHandler(task_OnInitialize);
			task.OnWork += new DoWorkEventHandler(task_OnWork);
			task.OnProgress += new ProgressChangedEventHandler(task_OnProgress);
			task.OnComplete += new RunWorkerCompletedEventHandler(task_OnComplete);

			task.Start(argument);
		}

		private void task_OnWork(object sender, DoWorkEventArgs e)
		{
			#region DoWorkEventHandler - main code goes here...
			// IMPORTANT: Do not access any GUI controls here.  (This code is running in the background thread!)
			var w = (BackgroundWorker)sender;
			//Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
			#endregion
		}

		private void task_OnInitialize(object sender, EventArgs e)
		{
			#region EventHandler - initialize code goes here...
			// It is safe to access the GUI controls here...
			#endregion
		}

		private void task_OnProgress(object sender, ProgressChangedEventArgs e)
		{
			#region ProgressChangedEventHandler - update progress code goes here...
			// It is safe to access the GUI controls here...
			#endregion
		}

		private void task_OnComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			#region RunWorkerCompletedEventHandler - cleanup code goes here...
			// It is safe to access the GUI controls here...
			#endregion
		}
		#endregion
	}
#endif

	/// <summary>
	/// Capture the necessary information to invoke a background task
	/// </summary>
	public abstract class BackgroundWorkerInfoBase
	{
		/// <summary>
		/// The active or current form
		/// </summary>
		public Form ActiveForm
		{
			get;
			set;
		}
		/// <summary>
		/// The cancel button - this button allows the user to cancel a long running process
		/// </summary>
		public Button CancelButton
		{
			get;
			set;
		}
		/// <summary>
		/// Disable these control(s) on start
		/// </summary>
		public Control[] DisableControls
		{
			get;
			set;
		}
		/// <summary>
		/// The process to run in the background thread.
		/// DO NOT ACCESS ANY WINDOWS CONTROL(S) IN THIS METHOD.
		/// </summary>
		public Action<BackgroundWorkerEventArgs> Callback
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public BackgroundWorkerInfoBase()
		{
		}
	}

	public class BackgroundWorkerInfo : BackgroundWorkerInfoBase
	{
		/// <summary>
		/// The progress bar indicator to show the current progress
		/// </summary>
		public ProgressBar ProgressBar
		{
			get;
			set;
		}
		/// <summary>
		/// The message to display to the user
		/// </summary>
		public Label StatusLabel
		{
			get;
			set;
		}
	}

	public class BackgroundWorkerToolStripInfo : BackgroundWorkerInfoBase
	{
		/// <summary>
		/// The progress bar indicator to show the current progress
		/// </summary>
		public ToolStripProgressBar ProgressBar
		{
			get;
			set;
		}
		/// <summary>
		/// The message to display to the user
		/// </summary>
		public ToolStripLabel StatusLabel
		{
			get;
			set;
		}
	}

	/// <summary>
	/// The BackgroundWorker EventArgs
	/// </summary>
	public class BackgroundWorkerEventArgs : EventArgs
	{
		/// <summary>
		/// the background worker
		/// </summary>
		public BackgroundWorker BackgroundWorker;
		/// <summary>
		/// the eventarg to the background worker
		/// </summary>
		public DoWorkEventArgs DoWorkEventArgs;
		/// <summary>
		/// the status report instance used to invoke the status report
		/// </summary>
		public StatusReport StatusReport;

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="w"></param>
		/// <param name="e"></param>
		/// <param name="sr"></param>
		public BackgroundWorkerEventArgs(BackgroundWorker w, DoWorkEventArgs e, StatusReport sr)
		{
			this.BackgroundWorker = w;
			this.DoWorkEventArgs = e;
			this.StatusReport = sr;
		}
	}

}
