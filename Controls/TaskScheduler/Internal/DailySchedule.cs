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
	internal partial class DailySchedule : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public DailySchedule()
		{
			InitializeComponent();
			this.AutoSize = false;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			this.dsfo1.OnChanged += new Base.ChangeEventHandler(dsfo1_Changed);
			this.dsfo1.Frequencies = this.numFrequencies.Value;
		}
		#endregion

		#region Custom Event methods
		protected void dsfo1_Changed(object sender, Base.ChangeEventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Event methods
		private void DailySchedule_Load(object sender, EventArgs e)
		{
		}

		private void numFrequencies_ValueChanged(object sender, EventArgs e)
		{
			dsfo1.Frequencies = this.numFrequencies.Value;
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
		[Description("The frequency number (eg. Every XXX days)"), Category("Schedule")]
		public decimal Frequencies
		{
			get
			{
				return numFrequencies.Value;
			}
			set
			{
				numFrequencies.Value = value;
			}
		}
		#endregion

		#region Indexer methods
		#endregion

		#region ISchedule Members

		string ISchedule.Value()
		{
			return ((ISchedule)this.dsfo1).Value();
			/*
			return String.Format("Daily: Every {0} day(s) for {1} time(s) from {2} to {3}.",
				this.dsfo1.Frequencies,
				this.dsfo1.Occurrences,
				this.dsfo1.StartDate.ToShortDateString(),
				this.dsfo1.EndDate.ToShortDateString());

			 */
		}

		string[] ISchedule.Values()
		{
			return ((ISchedule)this.dsfo1).Values();
		}

		#endregion
	}
}
