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
	/// Schedule a weekly task
	/// </summary>
	internal partial class WeeklyScheduleFreqOccur : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public WeeklyScheduleFreqOccur()
		{
			InitializeComponent();
			this.AutoSize = true;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		}
		#endregion

		#region Custom Event methods
		#endregion

		#region Event methods
		private void WeeklyScheduleFreqOccur_Load(object sender, EventArgs e)
		{
			InitComboBoxes(DateTime.Now);
		}

		private void cboFromMonth_SelectedIndexChanged(object sender, EventArgs e)
		{
			// cboFromMonth must be <= than cboToMonth, always ...
			// otherwise change the cboToMonth in this case.
			if (cboFromMonth.SelectedIndex > cboToMonth.SelectedIndex)
			{
				cboToMonth.SelectedIndex = cboFromMonth.SelectedIndex;
			}
			else
			{
				base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
			}
		}

		private void cboToDate_SelectedIndexChanged(object sender, EventArgs e)
		{
			// cboToMonth must be >= cboFromMonth, otherwise change cboFrom in this case...
			if (cboFromMonth.SelectedIndex > cboToMonth.SelectedIndex)
			{
				cboFromMonth.SelectedIndex = cboToMonth.SelectedIndex;
			}
			else
			{
				base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
			}
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private void InitComboBoxes(DateTime date)
		{
			DateTime curDate = date;

			cboFromMonth.Items.Clear();
			cboToMonth.Items.Clear();

			for (int i = 0; i < 36; i++)
			{
				string s = String.Format("{0}/{1}", curDate.Month, curDate.Year);

				cboFromMonth.Items.Add(s);
				cboToMonth.Items.Add(s);

				curDate = curDate.AddMonths(1);
			}

			cboFromMonth.SelectedIndex = 0;
			cboToMonth.SelectedIndex = 0;
		}
		#endregion

		#region Protected methods
		#endregion

		#region Override methods
		#endregion

		#region Property methods
		/// <summary>
		/// Get or set the ending date
		/// </summary>
		[Description("From Date"), Category("Schedule")]
		public string FromDate
		{
			get
			{
				return this.cboFromMonth.Text;
			}
			set
			{
				this.cboFromMonth.Text = value;
			}
		}

		/// <summary>
		/// Get or set the starting date
		/// </summary>
		[Description("To Date"), Category("Schedule")]
		public string ToDate
		{
			get
			{
				return this.cboToMonth.Text;
			}
			set
			{
				this.cboToMonth.Text = value;
			}
		}
		#endregion

		#region Indexer methods
		#endregion

		#region ISchedule Members

		string ISchedule.Value()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		string[] ISchedule.Values()
		{
			int fromIdx = this.cboFromMonth.SelectedIndex;
			int toIdx = this.cboToMonth.SelectedIndex;
			int max = toIdx - fromIdx + 1;
			string[] retVal = new string[max];

			for (int i = 0; i < max; i++)
			{
				retVal[i] = this.cboFromMonth.Items[fromIdx + i].ToString();
			}

			return retVal;
		}

		#endregion
	}
}
