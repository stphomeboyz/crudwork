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
using System.Linq;
using System.Text;
using System.Collections;

namespace crudwork.Concurrency
{
	/// <summary>
	/// Concurrency Manager for handling objects types
	/// </summary>
	public class ConcurrencyManager : ConcurrencyManager<object, object>
	{
	}

	/// <summary>
	/// Concurrency Manager
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	public class ConcurrencyManager<T, U>
	{
		private List<IConcurrencyTask<T, U>> tasks;

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public ConcurrencyManager()
		{
			tasks = new List<IConcurrencyTask<T, U>>();
		}

		/// <summary>
		/// Add an assembly type (with optional arguments) to the task list
		/// </summary>
		/// <param name="type"></param>
		/// <param name="args"></param>
		public void Add(Type type, params object[] args)
		{
			// TODO: casting does not work...
			IConcurrencyTask<T, U> task = (IConcurrencyTask<T, U>)Activator.CreateInstance(type, args);
			tasks.Add(task);

		}

		/// <summary>
		/// Start the task process
		/// </summary>
		public void Start()
		{
			// establish the chain-of-command
			for (int i = tasks.Count - 1; i > -1; i--)
			{
				ConcurrencyTask<T, U> task = (ConcurrencyTask<T, U>)tasks[i];
				ConcurrencyTask<T, U> prevTask = (i > 0) ? (ConcurrencyTask<T, U>)tasks[i - 1] : null;

				if (prevTask != null)
				{
					task.Enumerator = (IEnumerable<T>)prevTask;
					//task.Enumerator = ((IEnumerable)prevTask).GetEnumerator();
				}
			}
		}
	}
}
