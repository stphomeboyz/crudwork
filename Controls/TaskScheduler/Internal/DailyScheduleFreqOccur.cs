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
	/// Schedule a daily task
	/// </summary>
	internal partial class DailyScheduleFreqOccur : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		/// <summary>
		/// specify the frequencies (every day, every two days, etc...)
		/// </summary>
		private decimal frequencies;
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public DailyScheduleFreqOccur()
		{
			InitializeComponent();
			this.AutoSize = true;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			// make sure these are exactly the same down to the milliseconds.
			this.dtpFrom.Value = DateTime.Now;
			this.dtpTo.Value = this.dtpFrom.Value;
		}
		#endregion

		#region Custom Event methods
		#endregion

		#region Event methods
		private void DayScheduleFrequenciesOccurencies_Load(object sender, EventArgs e)
		{
			this.rdoOccurrences.Checked = true;
			Flip(this.rdoOccurrences.Checked);
		}

		private void numFrequencies_ValueChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Event methods (Occurrency)
		private void rdoOccurrences_Click(object sender, EventArgs e)
		{
			Flip(true);
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void numOccurrences_ValueChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void dtpStart_ValueChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Event methods (DateRange)
		private void rdoDateRange_Click(object sender, EventArgs e)
		{
			Flip(false);
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void dtpFrom_ValueChanged(object sender, EventArgs e)
		{
			dtpTo.MinDate = dtpFrom.Value;
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void dtpTo_ValueChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private void Flip(bool UseOccurrences)
		{
			//this.rdoOccurrences.Enabled = !UseDateRange;
			this.numOccurrences.Enabled = UseOccurrences;
			this.dtpStart.Enabled = UseOccurrences;
			
			//this.rdoDateRange.Enabled = UseDateRange;
			this.dtpFrom.Enabled = !UseOccurrences;
			this.dtpTo.Enabled = !UseOccurrences;
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
				return this.frequencies;
			}
			set
			{
				this.frequencies = value;
			}
		}

		/// <summary>
		/// The number of occurrences
		/// </summary>
		[Description("The number of occurrences"), Category("Schedule")]
		public decimal Occurrences
		{
			get
			{
				if (this.rdoOccurrences.Checked)
				{
					return this.numOccurrences.Value;
				}
				else
				{
					// calculate the number of occurrences
					// based on the two dates: dtpFrom and dtpTo
					if (dtpTo.Value < dtpFrom.Value)
					{
						return 0;
					}

					TimeSpan ts = new TimeSpan(dtpTo.Value.Ticks - dtpFrom.Value.Ticks);

					decimal totalDays;
					if (!decimal.TryParse(ts.TotalDays.ToString(), out totalDays))
					{
						return 0;
					}
					totalDays = Math.Round(totalDays);
					return 1 + Math.Floor(totalDays / Frequencies);
				}
			}
			protected set
			{
				throw new NotImplementedException("The set property is not yet implemented.");
			}
		}

		/// <summary>
		/// The starting date
		/// </summary>
		[Description("The starting date"), Category("Schedule")]
		public DateTime StartDate
		{
			get
			{
				if (this.rdoOccurrences.Checked)
				{
					return this.dtpStart.Value;
				}
				else
				{
					return this.dtpFrom.Value;
				}
			}
			protected set
			{
				throw new NotImplementedException("The set property is not yet implemented.");
			}
		}

		/// <summary>
		/// The ending date
		/// </summary>
		[Description("The ending date"), Category("Schedule")]
		public DateTime EndDate
		{
			get
			{
				if (this.rdoOccurrences.Checked)
				{
					// calculate the ending date based on the
					// date, frequencies, and occurrencies.
					double days = (double)Frequencies * ((double)Occurrences-1);
					return this.dtpStart.Value.AddDays(days);
				}
				else
				{
					return this.dtpTo.Value;
				}
			}
			protected set
			{
				throw new NotImplementedException("The set property is not yet implemented.");
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
			int max = (int)Occurrences;

			string[] retVal = new string[max];

			if (max == 0)
				return retVal;

			
			int curIdx = 0;

			DateTime curDate = StartDate;
			DateTime endDate = EndDate;

			while (curDate <= endDate)
			{
				if (curIdx == max)
				{
					MessageBox.Show("This is not right!");
					break;
				}
				
				retVal[curIdx] = curDate.ToLongDateString();
				curIdx++;

				curDate = curDate.AddDays((double)Frequencies);
			}

			return retVal;
		}

		#endregion
	}
}
