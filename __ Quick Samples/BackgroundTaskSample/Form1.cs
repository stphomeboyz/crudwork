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
using System.Threading;

using crudwork.MultiThreading;

namespace BackgroundTaskSample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			button2.Enabled = false;
			txtArgument.Text = "100";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				startTask(txtArgument.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void startTask(object argument)
		{
			BackgroundWorker worker = BackgroundTask.Start(
				argument,
				delegate(object sender, DoWorkEventArgs e)
				{
					DateTime start = DateTime.Now;
					BackgroundWorker w = (BackgroundWorker)sender;
					int max = int.Parse(e.Argument.ToString());
					Random random = new Random();

					// main code goes here...
					for (int i = 0; i < max; i++)
					{
						int sleepAmount = random.Next(100, 1000);
						Thread.Sleep(sleepAmount);

						w.ReportProgress((int)((1D + i) / max * 100), sleepAmount);

						if (w.CancellationPending)
						{
							e.Cancel = true;
							break;
						}
					}

					e.Result = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
				},
				delegate(object sender, EventArgs e)
				{
					// initialize code goes here...
					button1.Enabled = false;
					button2.Enabled = true;
					txtDetail.Clear();
				},
				delegate(object sender, ProgressChangedEventArgs e)
				{
					// update progress code goes here...
					progressBar1.Value = Math.Min(100, e.ProgressPercentage);

					string msg = string.Format("{1} percentage completed. [userStage={2}]{0}",
						Environment.NewLine, e.ProgressPercentage, e.UserState);

					txtDetail.AppendText(msg);
				},
				delegate(object sender, RunWorkerCompletedEventArgs e)
				{
					try
					{
						if (e.Error != null)
							throw new ApplicationException("BackgroundWorker thread threw an exception.", e.Error);
						if (e.Cancelled)
							throw new ThreadInterruptedException("Thread was interrupted by the user.");

						TimeSpan ts = (TimeSpan)e.Result;

						txtDetail.AppendText(string.Format("The elapsed time = {0}:{1}:{2}.{3} ",
							ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
					}
					catch (Exception ex)
					{
						txtDetail.AppendText(ex.ToString());
						//MessageBox.Show(ex.ToString());
					}
					finally
					{
						// cleanup code goes here...
						button1.Enabled = true;
						button2.Enabled = false;
						progressBar1.Value = 0;
					}
				});

			EventHandler h = delegate(object sender, EventArgs e)
			{
				// the Cancel delegate
				worker.CancelAsync();
			};

			button2.Click += h;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			// not being used... see the "Cancel delegate"
		}

	}
}