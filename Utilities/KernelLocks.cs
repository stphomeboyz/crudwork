using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace crudwork.Utilities
{
	/// <summary>
	/// Thread Synchronization using kernel objects (Mutex, Semaphore, and Event)
	/// </summary>
	public static class KernelLocks
	{
		/// <summary>
		/// Open an existing mutex object by the given name.  Or create a new object, if none found.
		/// </summary>
		/// <param name="mutexName"></param>
		/// <returns></returns>
		private static Mutex GetMutex(string mutexName)
		{
			Mutex m = null;

			try
			{
				m = Mutex.OpenExisting(mutexName);
			}
			catch (WaitHandleCannotBeOpenedException ex)
			{
				Debug.WriteLine("Mutex name does not exist: " + ex.Message);
			}

			if (m == null)
				m = new Mutex(false, mutexName);

			return m;
		}

		/// <summary>
		/// locks via a mutex object, run the execution block, and unlock the object.  If wait occurs, execute the wait block.
		/// </summary>
		/// <param name="mutexName"></param>
		/// <param name="executeBlock"></param>
		/// <param name="waitBlock"></param>
		public static void MutexLock(string mutexName, EventHandler executeBlock, EventHandler waitBlock)
		{
			Mutex m = GetMutex(mutexName);

			while (!m.WaitOne(100))
			{
				if (waitBlock != null)
					waitBlock(m, EventArgs.Empty);
			}

			try
			{
				executeBlock(m, EventArgs.Empty);
			}
			finally
			{
				m.ReleaseMutex();
			}
		}

		/// <summary>
		/// Lock via a Monitor, run the execute block, and unlcok the object.  This method is similiar to the
		/// conventional lock (C#) or SynLock (VB.NET).  If wait occurs, execute the wait block.
		/// </summary>
		/// <param name="lockObj"></param>
		/// <param name="executeBlock"></param>
		/// <param name="waitBlock"></param>
		public static void MonitorLock(object lockObj, EventHandler executeBlock, EventHandler waitBlock)
		{
			object thisObj = new object();

			while (!Monitor.TryEnter(lockObj, 100))
			{
				if (waitBlock != null)
					waitBlock(thisObj, EventArgs.Empty);
			}

			try
			{
				executeBlock(thisObj, EventArgs.Empty);
			}
			finally
			{
				Monitor.Exit(lockObj);
			}
		}

		private static ReaderWriterLock rwLock = new ReaderWriterLock();

		/// <summary>
		/// Create a reader lock
		/// </summary>
		/// <param name="executeBlock"></param>
		/// <param name="waitBlock"></param>
		public static void ReaderLock(EventHandler executeBlock, EventHandler waitBlock)
		{
			//var rwLock = new ReaderWriterLock();

			do
			{
				try
				{
					rwLock.AcquireReaderLock(100);
					break;
				}
				catch (ApplicationException ex)
				{
					Debug.WriteLine(ex.Message);
					if (waitBlock != null)
						waitBlock(rwLock, EventArgs.Empty);
				}
			}
			while (true /* infinite loop */);

			try
			{
				executeBlock(rwLock, EventArgs.Empty);
			}
			finally
			{
				rwLock.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Create a writer lock
		/// </summary>
		/// <param name="executeBlock"></param>
		/// <param name="waitBlock"></param>
		public static void WriterLock(EventHandler executeBlock, EventHandler waitBlock)
		{
			//var rwLock = new ReaderWriterLock();

			do
			{
				try
				{
					rwLock.AcquireWriterLock(100);
					break;
				}
				catch (ApplicationException ex)
				{
					Debug.WriteLine(ex.Message);
					if (waitBlock != null)
						waitBlock(rwLock, EventArgs.Empty);
				}
			}
			while (true /* infinite loop */);

			try
			{
				executeBlock(rwLock, EventArgs.Empty);
			}
			finally
			{
				rwLock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Upgrade a reader lock to a writeer lock
		/// </summary>
		/// <param name="rwLock"></param>
		/// <param name="executeBlock"></param>
		/// <param name="waitBlock"></param>
		public static void UpgradeReadLock(ReaderWriterLock rwLock, EventHandler executeBlock, EventHandler waitBlock)
		{
			//var rwLock = new ReaderWriterLock();
			LockCookie lockCookie;
			do
			{
				try
				{
					lockCookie = rwLock.UpgradeToWriterLock(100);
					break;
				}
				catch (ApplicationException ex)
				{
					Debug.WriteLine(ex.Message);
					if (waitBlock != null)
						waitBlock(rwLock, EventArgs.Empty);
				}
			}
			while (true /* infinite loop */);

			try
			{
				executeBlock(rwLock, EventArgs.Empty);
			}
			finally
			{
				rwLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
	}
}
