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
using System.Diagnostics;
using crudwork.Models;

namespace crudwork.Utilities
{
	/// <summary>
	/// Delegate Helper
	/// </summary>
	/// <typeparam name="T">derivative of EventHandler</typeparam>
	/// <typeparam name="U">derivative of EventArgs</typeparam>
	public class DelegateUtil<T, U>
	//where T : EventHandler, U : EventArgs
	//where T : System.MulticastDelegate
	{
		private T _eventHandle = default(T);
		private U _eventArgs = default(U);

		#region Constructors
		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="eventHandle"></param>
		/// <param name="eventArgs"></param>
		public DelegateUtil(T eventHandle, U eventArgs)
		{
			this._eventHandle = eventHandle;
			this._eventArgs = eventArgs;
		}

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public DelegateUtil()
			: this(default(T), default(U))
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the event handler
		/// </summary>
		public T EventHandler2
		{
			get
			{
				return this._eventHandle;
			}
			set
			{
				this._eventHandle = value;
			}
		}

		///// <summary>
		///// Get or set the event handler
		///// </summary>
		//public event T EventHandler
		//{
		//    add
		//    {
		//        this._eventHandle += value;
		//    }
		//    remove
		//    {
		//        this._eventHandle -= value;
		//    }
		//}

		/// <summary>
		/// Get or set the event args
		/// </summary>
		public U EventArgs
		{
			get
			{
				return this._eventArgs;
			}
			set
			{
				this._eventArgs = value;
			}
		}
		#endregion

		/// <summary>
		/// Invoke the event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Invoke(object sender, EventArgs e)
		{
			//if (this._eventHandle == default(T))
			//    throw new ArgumentException("EventHandler is unassigned");
			//if (this._eventArgs == default(U))
			//    throw new ArgumentException("EventArgs is unassigned");

			var handlers = _eventHandle as EventHandler;
			if (handlers == null)
				return;

			var exlist = new AggregatedException();

			foreach (EventHandler ev in handlers.GetInvocationList())
			{
				try
				{
					ev(sender, e);
				}
				catch (Exception ex)
				{
					exlist.Add(ex);
				}
			}

			if (exlist.Count > 0)
				throw exlist;
		}
	}
}
