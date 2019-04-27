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
using System.Text;
using System.Threading;
using crudwork.Utilities;
using System.Diagnostics;
using crudwork.Models;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// An abstract, generic, object container for multi-threading process where
	/// T represents the input type (the key), and U represents the output type
	/// (the value).  This process container allows duplicate input keys.
	/// </summary>
	/// <typeparam name="T">Input type T (the key)</typeparam>
	/// <typeparam name="U">Output type U (the value)</typeparam>
	public abstract class MultiThreadingBase<T, U> : IMultiThreading
	/* where T : blah, blah, blah */
	/* where T : U */
	{
		#region Enumerators
		#endregion

		#region Fields
		/* NOTE: inputList and outputList -vs- Dictionary<T,U>
		 * inputList and outputList are being used instead of Dictionary<T,U>
		 * because we want to allow duplicate keys to occur.  This design is
		 * more appropriate for data processing.
		 * 
		 * protected Dictionary<T,U> myList = default(Dictionary<T,U>);
		 */
		private List<T> inputList = new List<T>();
		private List<U> outputList = new List<U>();

		/*
		 * store row-level statistics.
		 * */
		private bool enableStatistics = true;

		/// <summary>
		/// array of MultiThreadingStatistics
		/// </summary>
		public Dictionary<int, MultiThreadingStatistics> statList = new Dictionary<int, MultiThreadingStatistics>();

		/*
		 * store runtime logging.
		 */
		private List<string> errorLog = new List<string>();

		/// <summary>
		/// the Thread
		/// </summary>
		private Thread currentThread = null;

		/// <summary>
		/// the relative offset of the parent's inputList used for merging input.
		/// </summary>
		public int Offset = 0;

		/// <summary>
		/// raise status report interval (in percent)
		/// </summary>
		private int raiseStatusInterval = 10;

		private int recordsProcessed = 0;

		private bool continueOnError = false;

		private bool checkForDefaultValues = true;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an empty object
		/// </summary>
		protected MultiThreadingBase()
		{
			this.outputList = new List<U>();
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="inputList"></param>
		public MultiThreadingBase(List<T> inputList)
			: this()
		{
			this.inputList = inputList;
		}
		#endregion

		#region Events
		/// <summary>
		/// Subscribe to receive workstatus events
		/// </summary>
		public WorkStatusEventHandler workStatusEvent = null;

		/// <summary>
		/// Indicate whether a workstatus can be raised
		/// </summary>
		/// <param name="idx"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		protected bool CanRaiseWorkStatus(decimal idx, decimal max)
		{
			int p = (int)MathUtil.Percentage(idx, max);
#if !SILVERLIGHT
			Trace.WriteLine("p=" + p);
#endif
			return (p % RaiseStatusInterval == 0);
		}

		/// <summary>
		/// Raise a workstatus event
		/// </summary>
		/// <param name="message"></param>
		/// <param name="index"></param>
		/// <param name="percentage"></param>
		protected void RaiseWorkStatus(string message, int index, decimal percentage)
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
					ev(this, new WorkStatusEventArgs(message, ThreadName, AbsoluteIndex(index), percentage, WorkerThreadType.Slave));
				}
				catch (Exception ex)
				{
					exlist.Add(ex);
				}
			}

			if (exlist.Count > 0)
				throw exlist;
		}
		#endregion

		#region Public Methods
#if !SILVERLIGHT
		/// <summary>
		/// Start a process asynchronously.
		/// </summary>
		/// <param name="threadName"></param>
		/// <param name="threadPriority"></param>
		public virtual void StartAsync(string threadName, ThreadPriority threadPriority)
#else
		/// <summary>
		/// Start a process asynchronously.
		/// </summary>
		/// <param name="threadName"></param>
		public virtual void StartAsync(string threadName)
#endif
		{
			currentThread = new Thread(new ThreadStart(Start));

			currentThread.Name = threadName;
#if !SILVERLIGHT
			currentThread.Priority = threadPriority;
#endif
			currentThread.Start();
		}

		/// <summary>
		/// Start a process synchronously.
		/// </summary>
		public virtual void Start()
		{
			try
			{
				RecordsProcessed = 0;

				for (int idx = 0; idx < inputList.Count; idx++)
				{
					// raise an event on every tenth of a percent (10, 20, 30, etc...)
					//if (CanRaiseWorkStatus(idx, inputList.Count))
					RaiseWorkStatus("Processing", idx, MathUtil.Percentage(idx, inputList.Count));

					// apply input constraints
					if (GetInput(idx).Equals(default(T)))
						throw new ApplicationException("the input for index " + idx + " cannot be set to default.");

					// mark the starting time.
					DateTime start = DateTime.Now;

					try
					{
						MultiThreadEventArgs<T, U> ev = new MultiThreadEventArgs<T, U>(GetInput(idx), idx);

						// run the actual method for processing input and generate output.
						ProcessInput(ev);

						// apply output constraints
						if (checkForDefaultValues && ev.Result.Equals(default(U)))
							throw new ApplicationException("the output for index " + idx + " must be changed.");

						SetOutput(idx, ev.Result);
					}
					catch (Exception ex)
					{
						LogException(idx, ex);
						if (!ContinueOnError)
							throw;
					}
					finally
					{
						// save row-level statistics.
						if (enableStatistics)
						{
							statList.Add(idx, new MultiThreadingStatistics(start, DateTime.Now));
						}
					}

					RecordsProcessed++;
				}

				// raise last event
				RaiseWorkStatus("Processing", inputList.Count, 100);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// return the absolute index (ie: index + Offset) of the
		/// parent's input/output list.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int AbsoluteIndex(int index)
		{
			return index + Offset;
		}

		/// <summary>
		/// Abort the current thread
		/// </summary>
		public void Abort()
		{
			if (currentThread.ThreadState == System.Threading.ThreadState.Stopped)
				return;

			currentThread.Abort();
		}
		#endregion

		#region Private Methods

		#region logging
		//private /*protected virtual*/ void AddLog(int index, string message)
		//{
		//    string s = String.Format("threadname={1} index={2} absolute_index={3} message={4}{0}",
		//        Environment.NewLine,
		//        ThreadName, index, AbsoluteIndex(index), message);
		//    log.Add(s);
		//}

		private /*protected virtual*/ void LogException(int index, Exception ex)
		{
			string s = string.Format("absolute_index={0} input={1} message={2} detail={3}",
					AbsoluteIndex(index), GetInput(index), ex.Message, ex.ToString());
			//string s = String.Format("threadname={1} index={2} absolute_index={3} message={4} input={5} detail={6}{0}",
			//    Environment.NewLine,
			//    ThreadName, index, AbsoluteIndex(index), ex.Message, 
			//    GetInput(index),
			//    ex.ToString());
			errorLog.Add(s);
		}
		#endregion

		#endregion

		#region Protected Methods
		/// <summary>
		/// Process the Input
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		protected abstract void ProcessInput(MultiThreadEventArgs<T, U> e);

		#region accessing input / output data
		/// <summary>
		/// Get the input value at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected T GetInput(int index)
		{
			return inputList[index];
		}

		/// <summary>
		/// Set the output value to the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="input"></param>
		protected void SetInput(int index, T input)
		{
			if (index < inputList.Count)
			{
				inputList[index] = input;
			}
			else
			{
				inputList.Add(input);
			}
		}

		/// <summary>
		/// Get the output value at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected U GetOutput(int index)
		{
			return outputList[index];
		}

		/// <summary>
		/// Set the output value to the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="output"></param>
		protected void SetOutput(int index, U output)
		{
			try
			{
				if (index < outputList.Count)
				{
					outputList[index] = output;
				}
				else
				{
					outputList.Add(output);
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "index", index);
				DebuggerTool.AddData(ex, "output", output);
				throw;
			}
		}
		#endregion

		#endregion

		#region Properties
		/// <summary>
		/// Get the current thread.  Set is protected
		/// </summary>
		[Description("Get the current thread.  Set is protected"), Category("MTObject")]
		public Thread CurrentThread
		{
			get
			{
				return this.currentThread;
			}
			protected set
			{
				this.currentThread = value;
			}
		}

		/// <summary>
		/// Get the thread name
		/// </summary>
		[Description("Get the thread name"), Category("MTObject")]
		public string ThreadName
		{
			get
			{
				if (this.currentThread == null)
					return "";
				else
					return this.currentThread.Name;
			}
		}

		/// <summary>
		/// Get or set the input list
		/// </summary>
		[Description("Get or set the input list"), Category("MTObject")]
		public List<T> InputList
		{
			get
			{
				return this.inputList;
			}
			set
			{
				this.inputList = value;
			}
		}

		/// <summary>
		/// Get the output list. Set is protected
		/// </summary>
		[Description("Get the output list.  Set is protected"), Category("MTObject")]
		public List<U> OutputList
		{
			get
			{
				return this.outputList;
			}
			protected set
			{
				this.outputList = value;
			}
		}

		/// <summary>
		/// Get the log information. Set is protected
		/// </summary>
		[Description("Get the log information.  Set is protected"), Category("MTObject")]
		public string[] ErrorLog
		{
			get
			{
				return errorLog.ToArray();
			}
			/*protected set
			{
			}*/
		}

		/// <summary>
		/// Enable or disable the recording of statistics
		/// </summary>
		[Description("Enable or disable the recording of statistics"), Category("MTObject")]
		public bool EnableStatistics
		{
			get
			{
				return this.enableStatistics;
			}
			set
			{
				this.enableStatistics = value;
			}
		}

		/// <summary>
		/// Enable or disable the recording of statistics
		/// </summary>
		[Description("Enable or disable the recording of statistics"), Category("MTObject")]
		public bool CheckForDefaultValues
		{
			get
			{
				return this.checkForDefaultValues;
			}
			set
			{
				this.checkForDefaultValues = value;
			}
		}


		/// <summary>
		/// Get or set the interval of raising status
		/// </summary>
		[Description("Get or set the interval of raising status"), Category("MTObject")]
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
		/// Get the total records processed
		/// </summary>
		public int RecordsProcessed
		{
			get
			{
				return recordsProcessed;
			}
			private set
			{
				recordsProcessed = value;
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
		#endregion

		#region Others
		#endregion

		#region IMultiThreading Members

#if !SILVERLIGHT
		void IMultiThreading.StartAsync(string threadName, ThreadPriority threadPriority)
		{
			StartAsync(threadName, threadPriority);
		}
#else
		void IMultiThreading.StartAsync(string threadName)
		{
			StartAsync(threadName);
		}
#endif

		void IMultiThreading.Start()
		{
			Start();
		}

		#endregion
	}
}
