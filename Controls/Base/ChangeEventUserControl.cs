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
using System.Windows.Forms;
using crudwork.Models;

namespace crudwork.Controls.Base
{
	/// <summary>
	/// The base class for controls with ChangeEvent functionality
	/// </summary>
	public class ChangeEventUserControl : UserControl
	{
		/// <summary>
		/// subscribe to this delegate to receive change events
		/// </summary>
		public event ChangeEventHandler OnChanged;

		#region Event handlers
		///// <summary>
		///// subscribe to the ChangeEventHandler.
		///// </summary>
		///// <param name="ev">The method name</param>
		//[Obsolete("",true)]
		//public void AddChangeEventHandler(ChangeEventHandler ev)
		//{
		//    OnChanged += ev;
		//}

		///// <summary>
		///// Remove the subscriber handler.
		///// </summary>
		///// <param name="ev">The method name</param>
		//[Obsolete("", true)]
		//public void ClearChangeEventHandler(ChangeEventHandler ev)
		//{
		//    if (OnChanged == null)
		//        return;

		//    OnChanged -= ev;
		//}

		/// <summary>
		/// Remove all subscriber handlers.
		/// </summary>
		public void ClearChangeEventHandler()
		{
			var t = OnChanged;

			if (t == null)
				return;

			foreach (ChangeEventHandler ev in t.GetInvocationList())
			{
				OnChanged -= ev;
			}
		}

		/// <summary>
		/// Publish or raise the event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RaiseChangeEvent(object sender, ChangeEventArgs e)
		{
			// make a local copy for thread-safe.
			var t = OnChanged;
			var exlist = new AggregatedException();

			if (t == null)
				return;

			foreach (ChangeEventHandler ev in t.GetInvocationList())
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
		#endregion
	}

	#region ChangeEventHandler and ChangEventArgs class
	/// <summary>
	/// The ChangeEventHandler delegate is raised by the generic ChangeEventUserControl.
	/// </summary>
	/// <param name="sender">the sender object</param>
	/// <param name="e">the ChangeEventArgs class, inherited from EventArgs</param>
	public delegate void ChangeEventHandler(object sender, ChangeEventArgs e);

	/// <summary>
	/// The ChangeEventArgs, inherited from EventArgs, contains information of
	/// the control that raises the event.
	/// </summary>
	public class ChangeEventArgs : EventArgs
	{
		/// <summary>
		/// the triggered object
		/// </summary>
		public object triggerObject;
	
		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="triggerObject"></param>
		public ChangeEventArgs(object triggerObject)
		{
			this.triggerObject = triggerObject;
		}
	}
	#endregion
}
