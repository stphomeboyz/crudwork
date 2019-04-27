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
using System.Windows.Forms;

namespace crudwork.Controls.TaskScheduler
{
	/// <summary>
	/// Schedule a monthly task
	/// </summary>
	internal partial class MonthlySchedule : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		private CheckBox[] chkMonths;
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public MonthlySchedule()
		{
			InitializeComponent();
			this.AutoSize = false;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			chkMonths = new CheckBox[] {
				chkJanuary,
				chkFebruary,
				chkMarch,
				chkApril,
				chkMay,
				chkJune,
				chkJuly,
				chkAugust,
				chkSeptember,
				chkOctober,
				chkNovember,
				chkDecember
			};

			for (int i = 0; i < chkMonths.Length; i++)
			{

				chkMonths[i].CheckedChanged += new EventHandler(chkMonths_CheckedChanged);
			}

			cboWeekIndex.Items.Clear();
			cboWeekIndex.Items.Add("first");
			cboWeekIndex.Items.Add("second");
			cboWeekIndex.Items.Add("third");
			cboWeekIndex.Items.Add("fourth");
			cboWeekIndex.Items.Add("last");
			cboWeekIndex.SelectedIndex = 0;

			cboWeekIndexDay.Items.Clear();
			cboWeekIndexDay.Items.Add("Monday");
			cboWeekIndexDay.Items.Add("Tuesday");
			cboWeekIndexDay.Items.Add("Wednesday");
			cboWeekIndexDay.Items.Add("Thursday");
			cboWeekIndexDay.Items.Add("Friday");
			cboWeekIndexDay.Items.Add("Saturday");
			cboWeekIndexDay.Items.Add("Sunday");
			cboWeekIndexDay.SelectedIndex = 0;

		}
		#endregion

		#region Custom Event methods
		private void chkMonths_CheckedChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Event methods
		private void MonthlySchedule_Load(object sender, EventArgs e)
		{
			rdoExactDayOfMonth.Checked = true;
			Flip(rdoExactDayOfMonth.Checked);
		}

		private void rdoExactDayOfMonth_Click(object sender, EventArgs e)
		{
			Flip(true);
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void rdoIndexDayOfMonth_Click(object sender, EventArgs e)
		{
			Flip(false);
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void cboWeekIndex_SelectedIndexChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void cboWeekIndexDay_SelectedIndexChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void numExactDay_ValueChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private void Flip(bool ExactDay)
		{
			numExactDay.Enabled = ExactDay;

			cboWeekIndex.Enabled = !ExactDay;
			cboWeekIndexDay.Enabled = !ExactDay;
		}

		private string[] MonthsChecked()
		{
			string[] v = new string[NumberMonthsChecked];
			int c = 0;

			for (int i = 0; i < chkMonths.Length; i++)
			{
				if (chkMonths[i].Checked)
				{
					v[c++] = chkMonths[i].Text;
				}
			}

			return v;
		}

		private string Day()
		{
			if (numExactDay.Enabled)
			{
				return String.Format("{0}", numExactDay.Value);
			}
			else
			{
				return String.Format("The {0} {1}", cboWeekIndex.Text, cboWeekIndexDay.Text);
			}
		}

		private string ExactDate(string strMonth)
		{
			int year = DateTime.Now.Year;
			int month = ScheduleLib.ConvertMonth(strMonth) + 1;
			decimal day;

			if (numExactDay.Enabled)
			{
				day = numExactDay.Value;
				string strDate = String.Format("{0}/{1}/{2}", month, day, year);
				return DateTime.Parse(strDate).ToLongDateString();
			}
			else
			{
				DayOfWeek dayOfWeek = ScheduleLib.ConvertDayOfWeek(cboWeekIndexDay.Text);
				string[] foo = ScheduleLib.GetDayOfWeek(dayOfWeek, year, month, 1);
				int idx = -1;

				switch (cboWeekIndex.Text)
				{
					case "first":
						idx = 0;
						break;
					case "second":
						idx = 1;
						break;
					case "third":
						idx = 2;
						break;
					case "fourth":
						idx = 3;
						break;
					case "last":
						idx = foo.Length - 1;
						break;
					default:
						throw new ArgumentOutOfRangeException("cboWeekIndex.Text=" + cboWeekIndex.Text);
				}

				if (idx < foo.Length)
				{
					return foo[idx];
				}
				else
				{
					string s = String.Format("not idx<foo.Length: idx={0} foo.Length={1} cboWeekIndex.Text={2}", idx, foo.Length, cboWeekIndex.Text);
					throw new ArgumentOutOfRangeException(s);
				}
			}
		}
		#endregion

		#region Protected methods
		#endregion

		#region Override methods
		#endregion

		#region Property methods
		private int NumberMonthsChecked
		{
			get
			{
				int cnt = 0;
				for (int i = 0; i < chkMonths.Length; i++)
				{
					cnt += (chkMonths[i].Checked) ? 1 : 0;
				}
				return cnt;
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
			string[] v = MonthsChecked();
			for (int i = 0; i < v.Length; i++)
			{
				//v[i] = String.Format("Monthly: {0} of {1}", Day(), v[i]);
				v[i] = String.Format("{0}", ExactDate(v[i]));
			}
			return v;
		}
		#endregion
	}
}
