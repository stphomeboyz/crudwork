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
using System.Diagnostics;
using System.Threading;
using crudwork.Utilities;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif
using crudwork.Models;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Distribution types
	/// </summary>
	public enum DistributionTypes
	{
		/// <summary>
		/// distribute evenly using odd/even approach
		/// </summary>
		Evenly,
		/// <summary>
		/// distribute the tasks via the round-robin approach
		/// </summary>
		RoundRobin,
	}

	/// <summary>
	/// Multi-thread object manager
	/// </summary>
	/// <typeparam name="T">Input type T (the key)</typeparam>
	/// <typeparam name="U">Output type U (the value)</typeparam>
	public class MultiThreadingManager<T, U> : crudwork.MultiThreading.IMultiThreadingManager
	//public class MultiThreadingManager<S, T, U>
	//where S : MultiThreadingBase<T,U>
	{
		#region Enumerators
		#endregion Enumerators

		#region Fields
		/// <summary>
		/// an arbitrary value for limiting the maximum
		/// number of threads for a process.
		/// </summary>
		private const int MAX_THREADS = 128;

		/// <summary>
		/// The thread name id for MultiThreadingManager
		/// </summary>
		public const string THREADNAME = "Main";

		/* NOTE: inputList and outputList -vs- Dictionary<T,U>
		 * inputList and outputList are being used instead of Dictionary<T,U>
		 * because we want to allow duplicate keys to occur.  This design is
		 * more appropriate for data processing.
		 *
		 * private Dictionary<T,U> myList = default(Dictionary<T,U>);
		 */

		private List<T> mainInputList;
		private List<U> mainOutputList;

		/// <summary>
		/// a list of multi-threaded objects, derived from MTObject class.
		/// </summary>
		private List<MultiThreadingBase<T, U>> mtObjects;

		/// <summary>
		/// simple check on input distribution.
		/// </summary>
		private bool inputDistributed = false;

		/// <summary>
		/// The prefix name of a thread name.
		/// </summary>
		private const string ThreadNamePrefix = "Thread";

		/// <summary>
		/// the amount of time taken for the process to complete.
		/// </summary>
		private TimeSpan elapsedTime;
		private List<string> log;

		private int raiseStatusInterval = 10;
		private int waitInterval = 1000;

		/// <summary>
		/// subscribe to this event to receive work status
		/// </summary>
		private WorkStatusEventHandler workStatusEvent = null;

		/// <summary>
		/// subscribe to this event to received stop status
		/// </summary>
		private EventHandler stopEvent = null;

		/// <summary>
		/// lock mechanism for raising events to workStatusEvent
		/// </summary>
		private static object lockObjWork = new object();

		/// <summary>
		/// lock mechanism for raising events to stopEvent
		/// </summary>
		private static object lockObjStop = new object();

		private bool continueOnError = false;

		private bool userAbort = false;
		private bool checkForDefaultValues = false;

		private bool isMasterCanRaiseWorkStatus = true;
		private bool isSlaveCanRaiseWorkStatus = false;

#if !SILVERLIGHT
		/// <summary>
		/// the slave's thread priority (default = Normal)
		/// </summary>
		private ThreadPriority threadPriority = ThreadPriority.Normal;
#endif
		#endregion Fields

		#region Constructors

		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="mtObjects"></param>
		/// <param name="mainInputList"></param>
		public MultiThreadingManager(List<MultiThreadingBase<T, U>> mtObjects, List<T> mainInputList)
		{
			#region Sanity Checks
			if (mtObjects == null)
				throw new ArgumentNullException("myObjects");

			if (mainInputList == null)
				throw new ArgumentNullException("mainInputList");
			#endregion Sanity Checks

			this.log = new List<string>();
			this.mtObjects = mtObjects;
			MainInputList = mainInputList;
		}

		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="mainInputList"></param>
		public MultiThreadingManager(List<T> mainInputList)
			: this(new List<MultiThreadingBase<T, U>>(), mainInputList)
		{
		}

		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		public MultiThreadingManager()
			: this(new List<MultiThreadingBase<T, U>>(), new List<T>())
		{
		}

		#endregion Constructors

		#region Custom Events

		/// <summary>
		/// Indicate whether a workstatus can be raised
		/// </summary>
		/// <param name="idx"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		protected bool CanRaiseWorkStatus(decimal idx, decimal max)
		{
			decimal p = MathUtil.Percentage(idx, max);

			return (p % RaiseStatusInterval == 0);
		}

		/// <summary>
		/// Raise the workstatus event by the Master thread
		/// </summary>
		/// <param name="message"></param>
		/// <param name="threadName"></param>
		/// <param name="index"></param>
		/// <param name="percentage"></param>
		protected void RaiseMasterWorkStatus(string message, string threadName, int index, decimal percentage)
		{
			if (!IsMasterCanRaiseWorkStatus)
				return;
			RaiseWorkStatus(message, threadName, index, percentage, WorkerThreadType.Master);
		}

		/// <summary>
		/// Raise the workstatus event by the Slave thread
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RaiseSlaveWorkerStatus(object sender, WorkStatusEventArgs e)
		{
			if (!isSlaveCanRaiseWorkStatus)
				return;
			RaiseWorkStatus(e.Message, e.ThreadName, e.Index, e.Percentage, e.WorkerThreadType);
		}

		/// <summary>
		/// Raise the workstatus event
		/// </summary>
		/// <param name="message"></param>
		/// <param name="threadName"></param>
		/// <param name="index"></param>
		/// <param name="percentage"></param>
		/// <param name="workerThreadType"></param>
		protected void RaiseWorkStatus(string message, string threadName, int index, decimal percentage, WorkerThreadType workerThreadType)
		{
			// make a local copy for thread-safe
			WorkStatusEventHandler t = workStatusEvent;

			if (t == null)
				return;

			var exlist = new AggregatedException();

			foreach (WorkStatusEventHandler ev in t.GetInvocationList())
			{
				try
				{
					lock (lockObjWork)
					{
						ev(this, new WorkStatusEventArgs(message, threadName, index, percentage, workerThreadType));
					}
				}
				catch (Exception ex)
				{
					exlist.Add(ex);
				}
			}

			if (exlist.Count > 0)
				throw exlist;
		}

		/// <summary>
		/// Raise the Stop event
		/// </summary>
		protected void RaiseStopEvent()
		{
			EventHandler t = stopEvent;

			if (t == null)
				return;

			var exlist = new AggregatedException();

			foreach (EventHandler ev in t.GetInvocationList())
			{
				try
				{
					lock (lockObjStop)
					{
						ev(this, EventArgs.Empty);
					}
				}
				catch (Exception ex)
				{
					exlist.Add(ex);
				}
			}

			if (exlist.Count > 0)
				throw exlist;
		}

		/// <summary>
		/// subscribe or unsubscribe to the Stop event.
		/// </summary>
		public event EventHandler Stop
		{
			add
			{
				stopEvent += value;
			}
			remove
			{
				stopEvent -= value;
			}
		}

		/// <summary>
		/// subscribe or unsubscribe to the WorkStatus event
		/// </summary>
		public event WorkStatusEventHandler WorkStatusEvent
		{
			add
			{
				workStatusEvent += value;
			}
			remove
			{
				workStatusEvent -= value;
			}
		}
		#endregion Custom Events

		#region Public Methods

		#region ShowProgress Window
		//private MultiThreadingProcessForm progressWindow = null;
		///// <summary>
		///// Show or hide the progress bar
		///// </summary>
		///// <param name="flag"></param>
		//public void ShowProgress(bool flag)
		//{
		//    if (flag)
		//    {
		//        progressWindow = new MultiThreadingProcessForm(this);
		//        progressWindow.ConfirmExit = true;
		//        StopEventHandler += delegate(object sender, EventArgs e)
		//        {
		//            ShowProgress(false);
		//        };
		//        progressWindow.Show();
		//        //this.workStatusEvent += new WorkStatusEventHandler(MultiThreadingManager_workStatusEvent);
		//        this.RaiseStatusInterval = 1;
		//    }
		//    else
		//    {
		//        progressWindow.ConfirmExit = false;
		//        progressWindow.Close();
		//        progressWindow.Dispose();
		//    }
		//}

		//void MultiThreadingManager_workStatusEvent(object sender, WorkStatusEventArgs e)
		//{
		//    try
		//    {
		//        progressWindow.Percentage = e.Percentage;
		//        progressWindow.Detail = e.ToString();
		//    }
		//    catch (Exception ex)
		//    {
		//        Debug.WriteLine(ex.Message);
		//        throw;
		//    }
		//}
		#endregion ShowProgress Window

		#region mtObjects Configuration

		/// <summary>
		/// add a MTObject-derived instance to the list.
		/// </summary>
		/// <param name="mtObject"></param>
		public void Add(MultiThreadingBase<T, U> mtObject)
		{
			#region Sanity Checks
			if (mtObject == null)
				throw new ArgumentNullException("myObject");

			if (this.mtObjects.Count >= MAX_THREADS)
				throw new ArgumentOutOfRangeException("Maximum thread occurred");

			if (this.mtObjects.Count > 0)
			{
				// for consistency purpose: all MultiThreading objects
				// must be of the same type.

				if (this.mtObjects[0].GetType() != mtObject.GetType())
				{
					throw new ArgumentException("All mtObjects must be of same type for consistency");
				}
			}

			if (this.mtObjects.Contains(mtObject))
				throw new ArgumentException("myObject already added.");
			#endregion Sanity Checks

			this.mtObjects.Add(mtObject);
		}

		/// <summary>
		/// remove a MTObject-derived instance from the list.
		/// </summary>
		/// <param name="mtObject"></param>
		public void Remove(MultiThreadingBase<T, U> mtObject)
		{
			#region Sanity Checks
			if (mtObject == null)
				throw new ArgumentNullException("myObject");

			//// TODO: do i want to throw exception for object that does not exist?
			//if (!this.mtObjects.Contains(mtObject))
			//    throw new ArgumentOutOfRangeException("mtObject not found");
			#endregion Sanity Checks

			if (this.mtObjects.Contains(mtObject))
				this.mtObjects.Remove(mtObject);
		}

		/// <summary>
		/// clear all MTObject-derived instances.
		/// </summary>
		public void Clear()
		{
			this.mtObjects.Clear();
		}

		/// <summary>
		/// Create a number of given type
		/// </summary>
		/// <param name="numberThreads"></param>
		/// <param name="type"></param>
		/// <param name="args"></param>
		public void InitializeThread(int numberThreads, Type type, params object[] args)
		{
			try
			{
				Clear();

				for (int i = 0; i < numberThreads; i++)
				{
					MultiThreadingBase<T, U> mtObject = (MultiThreadingBase<T, U>)Activator.CreateInstance(type, args);
					Add(mtObject);
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "numberThreads", numberThreads);
				DebuggerTool.AddData(ex, "type", type.GetType());
				DebuggerTool.AddData(ex, "args", args);
				throw;
			}
		}

		#endregion mtObjects Configuration

		#region Start

		/// <summary>
		/// TEST PURPOSE ONLY: start the process in a single thread and merge output.
		/// </summary>
		public void Start()
		{
			#region Sanity Checks
			if ((this.mtObjects == null) || (this.mtObjects.Count == 0))
				throw new ArgumentNullException("mtObjects is null or empty");
			#endregion Sanity Checks

			if (!inputDistributed)
			{
				DistributeInput();
				//throw new ApplicationException("input not distributed.");
			}

			InitializeMTContainers();

			RaiseMasterWorkStatus("Starting Process", THREADNAME, 0, 0);
			DateTime start = DateTime.Now;

			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				string threadName = ThreadNamePrefix + i;
				MultiThreadingBase<T, U> thread = this.mtObjects[i];

#if !SILVERLIGHT
				Trace.WriteLine("Starting", "Start");
#endif
				thread.Start();
#if !SILVERLIGHT
				Trace.WriteLine("Completed", "Start");
#endif
				MergeLog(thread);
			}

			RaiseMasterWorkStatus("Merging Output", THREADNAME, 0, 0);
			MergeOutput();

			elapsedTime = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
			RaiseMasterWorkStatus("Stopped", THREADNAME, 0, 0);
		}

		private void InitializeMTContainers()
		{
			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				this.mtObjects[i].ContinueOnError = this.continueOnError;
				this.mtObjects[i].workStatusEvent += new WorkStatusEventHandler(RaiseSlaveWorkerStatus);
				this.mtObjects[i].RaiseStatusInterval = RaiseStatusInterval;
				this.mtObjects[i].EnableStatistics = true;
				this.mtObjects[i].CheckForDefaultValues = CheckForDefaultValues;
			}
		}

		#endregion Start

		#region StartAsync

		/// <summary>
		/// Start the process using the given number threads
		/// </summary>
		/// <param name="numThreads"></param>
		/// <param name="type"></param>
		/// <param name="args"></param>
		[Obsolete("Not yet implemented", true)]
		public void StartAsync(int numThreads, Type type, params object[] args)
		{
			//for (int i = 0; i < numThreads; i++)
			//{
			//    IMultiThreading mt = (IMultiThreading)InstanceGenerator.Create(type, args);
			//    Add(mt);
			//}
			throw new ApplicationException("Not done!");
		}

#if !SILVERLIGHT

		/// <summary>
		/// Start the process with progress
		/// </summary>
		/// <param name="showProgress"></param>
		/// <param name="owner"></param>
		/// <param name="closeOnCompletion"></param>
		public DialogResult StartAsync(bool showProgress, IWin32Window owner, bool closeOnCompletion)
		{
			if (!showProgress)
			{
				// switch to the default start (w/o progress bar)
				StartAsync();
				return DialogResult.OK;
			}

			MultiThreadingStatusDialog f = new MultiThreadingStatusDialog(this, closeOnCompletion);
			return f.ShowDialog(owner);
		}

#endif

		/// <summary>
		/// start the process in multiple threads, wait for completion, and merge output.
		/// </summary>
		public void StartAsync()
		{
			// make local copy for thread-safe
			List<MultiThreadingBase<T, U>> mtObjects = this.mtObjects;

			DateTime start = DateTime.Now;
			RaiseMasterWorkStatus("Task Start : " + start.ToString("s"), THREADNAME, 0, 100);

			#region Sanity Checks
			if ((mtObjects == null) || (mtObjects.Count == 0))
				throw new ArgumentNullException("mtObjects is null or empty");
			#endregion Sanity Checks

			if (!inputDistributed)
				DistributeInput();

			InitializeMTContainers();

			#region Start Threads
			MultiThreadingBase<T, U>[] threads = new MultiThreadingBase<T, U>[mtObjects.Count];

			try
			{
				for (int i = 0; i < mtObjects.Count; i++)
				//for (int i = mtObjects.Count - 1; i >= 0; i--)
				{
					string threadName = ThreadNamePrefix + i;
					threads[i] = mtObjects[i];

					string s = String.Format("Starting thread {0} with {1} records.",
						threadName,
						mtObjects[i].InputList.Count);
					RaiseMasterWorkStatus(s, THREADNAME, 0, 0);

#if !SILVERLIGHT
					threads[i].StartAsync(threadName, ThreadPriority);
#else
					threads[i].StartAsync(threadName);
#endif
				}

				WaitForProcess(WaitInterval);

				MergeOutput();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Thread Execution Failed: " + ex.ToString());
				// abort all threads
				for (int i = 0; i < threads.Length; i++)
				{
					threads[i].Abort();
				}
			}
			#endregion Start Threads

			// *********************************************************************
			// all threads should be completed (or aborted) with at this point
			// *********************************************************************

			MergeLog(threads);

			ElapsedTime = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
			RaiseMasterWorkStatus("Task Completed : " + DateTime.Now.ToString("s") + " Elapsed: " + ElapsedTime.ToString(), THREADNAME, 0, 100);

			RaiseStopEvent();
		}

		#region MergeLog methods

		private void MergeLog(MultiThreadingBase<T, U>[] threads)
		{
			RaiseMasterWorkStatus("Merging Log", THREADNAME, 0, 100);

			for (int i = 0; i < threads.Length; i++)
			{
				this.log.AddRange(threads[i].ErrorLog);
			}
		}

		private void MergeLog(MultiThreadingBase<T, U> thread)
		{
			this.log.AddRange(thread.ErrorLog);
		}

		#endregion MergeLog methods

		/// <summary>
		/// wait for all process to complete, sleep at the specified
		/// amount of milliseconds on each interval.
		/// </summary>
		/// <param name="intervalMilliseconds"></param>
		private void WaitForProcess(int intervalMilliseconds)
		{
			#region Sanity Checks
			if (intervalMilliseconds <= 0)
				throw new ArgumentOutOfRangeException("intervalSeconds=" + intervalMilliseconds);
			#endregion Sanity Checks

			int waitcount = 0;

			// wait indefinitely until all threads have completed.
			while (true)
			{
				waitcount++;
				bool active = false;
				int counter = 0;
				int overallRecordsProcessed = 0;

				// check state of all threads.
				for (int i = 0; i < this.mtObjects.Count; i++)
				{
					Thread t = this.mtObjects[i].CurrentThread;

					if (t != null && t.IsAlive)
					{
						active = true;
						counter++;
					}

					overallRecordsProcessed += this.mtObjects[i].RecordsProcessed;
				}

				if (!active)
					break;

				//if (CanRaiseWorkStatus(overallRecordsProcessed, this.mainInputList.Count))
				RaiseMasterWorkStatus("Overall Completion", THREADNAME, overallRecordsProcessed, MathUtil.Percentage(overallRecordsProcessed, this.mainInputList.Count));

#if !SILVERLIGHT
				// wait 1 sec on each iterval.
				Trace.WriteLine(overallRecordsProcessed + " of " + this.mainInputList.Count + " completed.  Waiting another " + intervalMilliseconds + " ms for " + counter + " threads to finish... ", "WaitForProcess");
#endif
				Thread.Sleep(intervalMilliseconds);

				if (UserAbort)
					throw new ApplicationException("User Abort");
			}
		}

		#endregion StartAsync

		#region Distribute

		/// <summary>
		/// distribute the input based on the distribution type specified.
		/// </summary>
		/// <param name="distributionType"></param>
		public void DistributeInput(DistributionTypes distributionType)
		{
			#region Sanity Checks
			if ((this.mtObjects == null) || (this.mtObjects.Count == 0))
				throw new ArgumentNullException("mtObjects is null or empty");

			if ((this.mainInputList == null) || (this.mainInputList.Count == 0))
				throw new ArgumentNullException("mainInputList is null or empty");
			#endregion Sanity Checks

			int numRecords = this.MainInputList.Count;
			int numThreads = (numRecords < this.mtObjects.Count) ? numRecords : this.mtObjects.Count;

			string s = string.Format("Distributing {0} records among {1} threads using {2} distribution type.",
				numRecords, numThreads, distributionType);
			RaiseMasterWorkStatus(s, THREADNAME, 0, 0);

			switch (distributionType)
			{
				case DistributionTypes.RoundRobin:
					for (int i = 0; i < numThreads; i++)
					{
						this.mtObjects[i].InputList = new List<T>();
					}

					for (int i = 0; i < numRecords; i++)
					{
						int idx = i % numThreads;
						this.mtObjects[idx].InputList.Add(this.mainInputList[i]);
					}
					break;

				case DistributionTypes.Evenly:
					#region Codes to distribute input evenly
					/*
					 * distribute records evenly amount the total number of threads.
					 * The remainder is spread out even among the threads.
					 * */
					{
						int recordPerGroup = numRecords / numThreads;
						int remainder = numRecords % numThreads;
						int firstIndex = 0;

						for (int i = 0; i < numThreads; i++)
						{
							int totalRecords = recordPerGroup;

							// spread the remainders to the top threads
							if (remainder > 0)
							{
								totalRecords++;
								remainder--;
							}

							this.mtObjects[i].InputList = this.mainInputList.GetRange(firstIndex, totalRecords);
							this.mtObjects[i].Offset = firstIndex;
							firstIndex += totalRecords;
						}
					}

					/*{
						// calculate the record per group.
						int recordPerGroup = numRecords / numThreads;
						int reminder = numRecords % numThreads;

						for (int i = 0; i < numThreads; i++)
						{
							int totalRecords;
							int firstIndex;

							firstIndex = i * recordPerGroup;
							totalRecords = recordPerGroup;

							// last thread process the remainder too.
							if (i + 1 == numThreads)
								totalRecords += reminder;

							this.mtObjects[i].InputList = this.mainInputList.GetRange(firstIndex, totalRecords);

							// store this offset for MergeInput()
							this.mtObjects[i].Offset = firstIndex;
						}
					}*/
					#endregion Codes to distribute input evenly
					break;

				default:
					throw new ArgumentOutOfRangeException("distributeType=" + distributionType.ToString());
			}

			inputDistributed = true;
		}

		/// <summary>
		/// distribute the input using the default distribution type, Evenly.
		/// </summary>
		public void DistributeInput()
		{
			DistributeInput(DistributionTypes.Evenly);
		}

		#endregion Distribute

		#region Merge

		/// <summary>
		/// merge the output processed by the processes.
		/// </summary>
		public void MergeOutput()
		{
			#region Sanity Checks
			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				// ------------------------------------------------------------
				// make sure that the total input/total output are the same.
				// ------------------------------------------------------------
				{
					int inputCount = this.mtObjects[i].InputList.Count;
					int outputCount = this.mtObjects[i].OutputList.Count;

					if (inputCount != outputCount)
						throw new ArgumentOutOfRangeException("total input/output not the same!");
				}

				if (!CheckForDefaultValues)
					continue;

				// insure output is not the initial default value.
				// This mean an unforseened error occurred.
				for (int j = 0; j < this.mtObjects[i].OutputList.Count; j++)
				{
					U value = this.mtObjects[i].OutputList[j];
					if (value.Equals(default(U)))
					{
						string s = String.Format("Output value for thread #{0} input #{1} value=\"{2}\" was not assigned!",
							i, j, this.mtObjects[i].InputList[j]);
						throw new ArgumentNullException(s);
					}
				}
			}
			#endregion Sanity Checks

			RaiseMasterWorkStatus("Merging Output", THREADNAME, 0, 0);

			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				int offset = this.mtObjects[i].Offset;
				List<U> results = this.mtObjects[i].OutputList;

				// loop works like AddRange() but with offset.
				//this.mainInputList.AddRange(results.ToArray());
				for (int j = 0; j < results.Count; j++)
				{
					int idx = j + offset;
					this.mainOutputList[idx] = results[j];
				}
			}
		}

		#endregion Merge

		#region Statistics

		/// <summary>
		/// return the average time spent
		/// </summary>
		/// <returns></returns>
		public int AverageTime()
		{
			int? avg = null;

			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				if (!avg.HasValue)
				{
					avg = MultiThreadingStatistics.AverageMilliseconds(this.mtObjects[i].statList);
				}
				else
				{
					avg += MultiThreadingStatistics.AverageMilliseconds(this.mtObjects[i].statList);
				}
			}

			if (!avg.HasValue)
				return 0;

			return avg.Value / this.mtObjects.Count;
		}

		/// <summary>
		/// return the time spent in ticks
		/// </summary>
		/// <returns></returns>
		public long AverageTicks()
		{
			long? avg = null;

			for (int i = 0; i < this.mtObjects.Count; i++)
			{
				if (!avg.HasValue)
				{
					avg = MultiThreadingStatistics.AverageTicks(this.mtObjects[i].statList);
				}
				else
				{
					avg += MultiThreadingStatistics.AverageTicks(this.mtObjects[i].statList);
				}
			}

			if (!avg.HasValue)
				return 0;

			return avg.Value / this.mtObjects.Count;
		}

		#endregion Statistics

		#endregion Public Methods

		#region Private Methods
		#endregion Private Methods

		#region Protected Methods
		#endregion Protected Methods

		#region Properties

		/// <summary>
		/// Get or set the main input list
		/// </summary>
		[Description("Get or set the main input list"), Category("MTObjectManager")]
		public List<T> MainInputList
		{
			get
			{
				return this.mainInputList;
			}
			set
			{
				if (value == null)
				{
					this.mainInputList = null;
					this.mainOutputList = null;
					return;
				}

				this.mainInputList = value;

				// initialize mainOutput.
				if (this.mainOutputList == null)
					this.mainOutputList = new List<U>();
				else
					this.mainOutputList.Clear();

				for (int i = 0; i < value.Count; i++)
				{
					this.mainOutputList.Add(default(U));
				}

				#region Sanity Checks
				if (this.mainInputList.Count != this.mainOutputList.Count)
					throw new ApplicationException("initialization error!");
				#endregion Sanity Checks
			}
		}

		/// <summary>
		/// Get or set the main output list
		/// </summary>
		[Description("Get the main output list.  Set is private."), Category("MTObjectManager")]
		public List<U> MainOutputList
		{
			get
			{
				if (this.mainInputList.Count != this.mainOutputList.Count)
					throw new ApplicationException("input/output counts are different!");

				return this.mainOutputList;
			}
			private set
			{
				this.mainOutputList = value;
			}
		}

		/// <summary>
		/// Get or set the elapsed time
		/// </summary>
		[Description("Get the main elapsed time.  Set is private."), Category("MTObjectManager")]
		public TimeSpan ElapsedTime
		{
			get
			{
				return this.elapsedTime;
			}
			private set
			{
				this.elapsedTime = value;
			}
		}

		/// <summary>
		/// return the elapsed time in ticks
		/// </summary>
		public long ElapsedTicks
		{
			get
			{
				return this.elapsedTime.Ticks;
			}
		}

		/// <summary>
		/// Get the log information
		/// </summary>
		[Description("Get the log information.  Set is private."), Category("MTObjectManager")]
		public String[] Log
		{
			get
			{
				return this.log.ToArray();
			}
			/*private set
			{
			}*/
		}

		/// <summary>
		/// Get or set the interval of raising status
		/// </summary>
		[Description("Get or set the interval of raising status"), Category("MTObjectManager")]
		public int RaiseStatusInterval
		{
			get
			{
				return raiseStatusInterval;
			}
			set
			{
				raiseStatusInterval = value;
			}
		}

		/// <summary>
		/// Get or set the waiting interval
		/// </summary>
		[Description("Get or set the waiting interval"), Category("MTObjectManager")]
		public int WaitInterval
		{
			get
			{
				return waitInterval;
			}
			set
			{
				waitInterval = value;
			}
		}

		/// <summary>
		/// Get or set a value indicating whether or not to continue if error is encountered
		/// </summary>
		public bool ContinueOnError
		{
			get
			{
				return this.continueOnError;
			}
			set
			{
				this.continueOnError = value;
			}
		}

		private static object lockObj = new object();

		/// <summary>
		/// Get or set a value to abort the task
		/// </summary>
		public bool UserAbort
		{
			get
			{
				lock (lockObj)
				{
					return this.userAbort;
				}
			}
			set
			{
				lock (lockObj)
				{
					this.userAbort = value;
				}
			}
		}

		/// <summary>
		/// Indicate whether or not the master thread is allowed to raise workstatus events.
		/// </summary>
		public bool IsMasterCanRaiseWorkStatus
		{
			get
			{
				return this.isMasterCanRaiseWorkStatus;
			}
			set
			{
				this.isMasterCanRaiseWorkStatus = value;
			}
		}

		/// <summary>
		/// Indicate whether or not the slave thread is allowed to raise workstatus events.
		/// </summary>
		public bool IsSlaveCanRaiseWorkStatus
		{
			get
			{
				return this.isSlaveCanRaiseWorkStatus;
			}
			set
			{
				this.isSlaveCanRaiseWorkStatus = value;
			}
		}

		public bool CheckForDefaultValues
		{
			get
			{
				return checkForDefaultValues;
			}
			set
			{
				checkForDefaultValues = value;
			}
		}

#if !SILVERLIGHT

		/// <summary>
		/// Get or set the slave's thread priority.  Default is Normal.
		/// </summary>
		public ThreadPriority ThreadPriority
		{
			get
			{
				return this.threadPriority;
			}
			set
			{
				this.threadPriority = value;
			}
		}

#endif
		#endregion Properties

		#region Others
		#endregion Others
	}
}