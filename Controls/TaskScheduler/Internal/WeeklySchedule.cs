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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;

namespace crudwork.Controls.TaskScheduler
{
	/// <summary>
	/// Schedule a weekly task
	/// </summary>
	internal partial class WeeklySchedule : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		Control[] chkDays;
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public WeeklySchedule()
		{
			InitializeComponent();
			this.AutoSize = false;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			chkDays = new Control[] {
				chkSunday,
				chkMonday,
				chkTuesday,
				chkWednesday,
				chkThursday,
				chkFriday,
				chkSaturday
			};

			for (int i = 0; i < chkDays.Length; i++)
			{
				chkDays[i].Click += new EventHandler(chkDays_Click);
			}

			this.wsfo1.OnChanged += new Base.ChangeEventHandler(wdso1_Changed);
		}
		#endregion

		#region Custom Event methods
		/// <summary>
		/// raise the change event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void wdso1_Changed(object sender, Base.ChangeEventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Event methods
		void chkDays_Click(object sender, EventArgs e)
		{
			RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void numFrequencies_ValueChanged(object sender, EventArgs e)
		{
			RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private string[] DaysChecked()
		{
			string[] v = new string[NumberDaysChecked];
			int c = 0;

			for (int i = 0; i < chkDays.Length; i++)
			{
				CheckBox cb = (CheckBox)chkDays[i];
				if (cb.Checked)
				{
					v[c++] = cb.Text;
				}
			}

			return v;
		}

		#endregion

		#region Protected methods
		#endregion

		#region Override methods
		#endregion

		#region Property methods
		/// <summary>
		/// The frequency number (eg. Every XXX days)
		/// </summary>
		[Description("The frequency number (eg. Every XXX days)"), Category("Schedule")]
		public decimal Frequencies
		{
			get
			{
				// TODO: Needs some more work...
				return this.numFrequencies.Value;
			}
			set
			{
				this.numFrequencies.Value = value;
			}
		}

		/// <summary>
		/// Get the number of days checked
		/// </summary>
		[Description("Get the number of days checked"), Category("Schedule")]
		private int NumberDaysChecked
		{
			get
			{
				int c = 0;
				for (int i = 0; i < chkDays.Length; i++)
				{
					CheckBox cb = (CheckBox)chkDays[i];
					c += (cb.Checked) ? 1 : 0;
				}
				return c;
			}
		}
		#endregion

		#region Indexer methods
		#endregion

		#region ISchedule Members

		string ISchedule.Value()
		{
			// See Values for actual work...
			StringBuilder v = new StringBuilder();

			string[] v2 = ((ISchedule)this).Values();
			for (int i = 0; i < v2.Length; i++)
			{
				v.AppendFormat("{0}\n", v2[i]);
			}

			return v.ToString();
		}

		string[] ISchedule.Values()
		{
			ArrayList l = new ArrayList();

			string[] daysChecked  = DaysChecked();
			string[] monthsChecked = ((ISchedule)this.wsfo1).Values();

			for (int i = 0; i < daysChecked.Length; i++)
			{
				/*
				string s = String.Format("Weekly: Every {0} week(s) on {1} from {2} to {3}.",
					Frequencies,
					v[i],
					wsfo1.FromDate,
					wsfo1.ToDate);
				v[i] = s;
				*/

				for (int y = 0; y < monthsChecked.Length; y++)
				{
					string[] retValue = ScheduleLib.GetDayOfWeek(daysChecked[i], monthsChecked[y], this.numFrequencies.Value);

					for (int z = 0; z < retValue.Length; z++)
					{
						l.Add(retValue[z]);
					}
				}
			}

			return (string[])l.ToArray(typeof(string));
		}

		#endregion
	}
}
