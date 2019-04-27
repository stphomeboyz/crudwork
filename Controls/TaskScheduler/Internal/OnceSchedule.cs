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
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace crudwork.Controls.TaskScheduler
{
	/// <summary>
	/// Schedule a one-time event
	/// </summary>
	internal partial class OnceSchedule : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public OnceSchedule()
		{
			InitializeComponent();
			this.AutoSize = false;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			this.monthCalendar1.MinDate = DateTime.Now;
		}
		#endregion

		#region Custom Event methods
		#endregion

		#region Event methods
		private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		#endregion

		#region Protected methods
		#endregion

		#region Override methods
		#endregion

		#region Property methods
		#endregion

		#region Indexer methods
		#endregion

		#region ISchedule Members

		string ISchedule.Value()
		{
			return String.Format("{0}", monthCalendar1.SelectionStart.ToLongDateString());
		}

		string[] ISchedule.Values()
		{
			// See Value for actual work...
			return new string[] { ((ISchedule)this).Value() };
		}

		#endregion
	}
}
