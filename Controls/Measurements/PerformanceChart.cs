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

namespace crudwork.Controls.Measurements
{
	/// <summary>
	/// A graphical chart to display performance measurement
	/// </summary>
	public partial class PerformanceChart : UserControl
	{
		private Timer timer = new Timer();

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public PerformanceChart()
		{
			InitializeComponent();
			timer.Tick += new EventHandler(timer_Tick);
		}

		/// <summary>
		/// Get or set a value to enable or dislable the timer
		/// </summary>
		public bool EnableTimer
		{
			get
			{
				return timer.Enabled;
			}
			set
			{
				timer.Enabled = value;
			}
		}

		/// <summary>
		/// Get or set the interval amount
		/// </summary>
		public int Interval
		{
			get
			{
				return timer.Interval;
			}
			set
			{
				timer.Interval = value;
			}
		}

		/// <summary>
		/// The timer tick main flow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void timer_Tick(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
