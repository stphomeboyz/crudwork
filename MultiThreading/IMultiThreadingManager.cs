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
namespace crudwork.MultiThreading
{
	/// <summary>
	/// MultiThreading Mananger Interface
	/// </summary>
	public interface IMultiThreadingManager
	{
		/// <summary>
		/// TEST PURPOSE ONLY: start the process in a single thread and merge output.
		/// </summary>
		void Start();

		/// <summary>
		/// start the process in multiple threads, wait for completion, and merge output.
		/// </summary>
		void StartAsync();

		/// <summary>
		/// subscribe or unsubscribe to the Stop event.
		/// </summary>
		event EventHandler Stop;

		/// <summary>
		/// subscribe or unsubscribe to the WorkStatus event
		/// </summary>
		event WorkStatusEventHandler WorkStatusEvent;

		/// <summary>
		/// Get or set a value to abort the task
		/// </summary>
		bool UserAbort
		{
			get;
			set;
		}

		/// <summary>
		/// Get the log information
		/// </summary>
		string[] Log
		{
			get;
		}
	}
}
