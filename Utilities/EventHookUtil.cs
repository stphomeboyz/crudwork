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

namespace crudwork.Utilities
{
	/// <summary>
	/// Event Hook utility
	/// </summary>
	public class EventHookUtil
	{
		/// <summary>
		/// The EventHook handler
		/// </summary>
		public event EventHookEventHandler eventHook = null;

		/// <summary>
		/// Raise an EventHook event
		/// </summary>
		/// <param name="message"></param>
		public void RaiseEvent(string message)
		{
			RaiseEvent(this, new EventHookEventArgs(message));
		}

		/// <summary>
		/// Raise an EventHook event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void RaiseEvent(object sender, EventHookEventArgs e)
		{
			// thread-safe.
			EventHookEventHandler t = eventHook;

			if (t == null)
				return;

			foreach (EventHookEventHandler ev in t.GetInvocationList())
			{
				try
				{
					ev(sender, e);
				}
				catch (Exception ex)
				{
					// absorb error!
#if !SILVERLIGHT
					Debug.Print(ex.ToString());
#else
					Debug.WriteLine(ex.ToString());
#endif
				}
			}
		}
	}

	/// <summary>
	/// The EventHook delegate
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void EventHookEventHandler(object sender, EventHookEventArgs e);

	/// <summary>
	/// The EventHook argument
	/// </summary>
	public class EventHookEventArgs : EventArgs
	{
		/// <summary>
		/// Get the message
		/// </summary>
		public readonly string Message = string.Empty;

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="message"></param>
		public EventHookEventArgs(string message)
		{
			this.Message = message;
		}
	}
}
