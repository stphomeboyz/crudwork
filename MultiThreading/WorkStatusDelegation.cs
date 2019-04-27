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

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Event for reporting work status
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WorkStatusEventHandler(object sender, WorkStatusEventArgs e);

	/// <summary>
	/// The types of worker thread
	/// </summary>
	public enum WorkerThreadType
	{
		/// <summary>Master thread</summary>
		Master = 1,
		/// <summary>Slave thread</summary>
		Slave = 2,
	}

	/// <summary>
	/// EventArgs for reporting work status
	/// </summary>
	public class WorkStatusEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// The message status
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// the thread name
		/// </summary>
		public readonly string ThreadName;

		/// <summary>
		/// the row index
		/// </summary>
		public readonly int Index;

		/// <summary>
		/// the percentage complete
		/// </summary>
		public readonly decimal Percentage;

		/// <summary>
		/// The worker thread type
		/// </summary>
		public readonly WorkerThreadType WorkerThreadType;
		#endregion

		#region Constructors
		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="message"></param>
		/// <param name="threadName"></param>
		/// <param name="index"></param>
		/// <param name="percentage"></param>
		/// <param name="workerThreadType"></param>
		public WorkStatusEventArgs(string message, string threadName, int index, decimal percentage, WorkerThreadType workerThreadType)
		{
			this.Message = message;
			this.ThreadName = threadName;
			this.Index = index;
			this.Percentage = percentage;
			this.WorkerThreadType = workerThreadType;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// return string presentation of the object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("WorkerThreadType={0} ThreadName={1} - Message={2} - Index={3} - Percentage={4:F0}",
				this.WorkerThreadType,
				this.ThreadName,
				this.Message,
				this.Index,
				this.Percentage
				);
		}
		#endregion
	}
}
