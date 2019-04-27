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
using crudwork.Models;

namespace crudwork.Controls.TaskScheduler
{
	/// <summary>
	/// Schedule a daily task
	/// </summary>
	public partial class MainDaySchedule : UserControl
	{
		#region Fields
		private UserControl[] controls;
		/// <summary>
		/// the event handler for day schedule change
		/// </summary>
		public DayScheduleChangeEventHandle dayScheduleChangeEventHandle;
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public MainDaySchedule()
		{
			InitializeComponent();

			controls = new UserControl[] {
				dailySchedule1,
				weeklySchedule1,
				monthlySchedule1,
				onceSchedule1,
				holidaySchedule1
			};

			dailySchedule1.Tag = "Daily";
			weeklySchedule1.Tag = "Weekly";
			monthlySchedule1.Tag = "Monthly";
			onceSchedule1.Tag = "Once";
			holidaySchedule1.Tag = "Holidays";

			dailySchedule1.OnChanged += new Base.ChangeEventHandler(controls_Changed);
			weeklySchedule1.OnChanged += new Base.ChangeEventHandler(controls_Changed);
			monthlySchedule1.OnChanged += new Base.ChangeEventHandler(controls_Changed);
			onceSchedule1.OnChanged += new Base.ChangeEventHandler(controls_Changed);
			holidaySchedule1.OnChanged += new Base.ChangeEventHandler(controls_Changed);

			if (controls.Length > 0)
			{
				cboScheduleType.Items.Clear();
				for (int i = 0; i < controls.Length; i++)
				{
					cboScheduleType.Items.Add(controls[i].Tag);
				}
				cboScheduleType.SelectedIndex = 0;
			}
		}
		#endregion

		#region Custom Event methods
		/// <summary>
		/// Raise event for DaySchedule change event
		/// </summary>
		/// <param name="c"></param>
		protected void RaiseDayScheduleChangeEvent(object c)
		{
			DayScheduleChangeEventHandle t = dayScheduleChangeEventHandle;

			if (t == null)
				return;

			var exlist = new AggregatedException();

			foreach (DayScheduleChangeEventHandle ev in t.GetInvocationList())
			{
				try
				{
					ev(this, new DayScheduleChangedEventArgs(c));
				}
				catch (Exception ex)
				{
					exlist.Add(ex);
				}
			}

			if (exlist.Count > 0)
				throw exlist;
		}

		private void controls_Changed(object sender, Base.ChangeEventArgs e)
		{
			ISchedule isc = sender as ISchedule;
			if (isc == null)
			{
				return;
			}

			RaiseDayScheduleChangeEvent(sender);

			//txtResults.Text = "";
			//string[] s = isc.Values();
			//for (int i = 0; i < s.Length; i++)
			//{
			//    txtResults.Text += s[i] + Environment.NewLine;
			//}
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private void EnableObject(UserControl c, bool enableValue)
		{
			//UserControl c = (UserControl)cx;
			c.Visible = enableValue;
			c.Enabled = enableValue;
			c.Dock = DockStyle.Fill;
			c.AutoSize = true;
			c.AutoSizeMode = AutoSizeMode.GrowOnly;
		}
		#endregion

		#region Protected methods
		#endregion

		#region Override methods
		#endregion

		#region Property methods
		/// <summary>
		/// Get or set the Title
		/// </summary>
		[Description("Get or set the scheduler title"), Category("Day Schedule")]
		public string Title1
		{
			get
			{
				return this.groupBox1.Text;
			}
			set
			{
				this.groupBox1.Text = value;
			}
		}

		//[Description("Get or set the scheduler title 2"), Category("Day Schedule")]
		//public string Title2
		//{
		//    get
		//    {
		//        return lblTitle2.Text;
		//    }
		//    set
		//    {
		//        lblTitle2.Text = value;
		//    }
		//}
		#endregion

		#region Event methods
		private void MainDaySchedule_Load(object sender, EventArgs e)
		{
			if (cboScheduleType.Items.Count > 0)
			{
				cboScheduleType.SelectedIndex = 0;
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*
			 * Daily
			 * Weekly
			 * Monthly
			 * Once
			 * Holidays
			 * Custom
			 * */

			string key = cboScheduleType.Text; //.ToUpper();
			bool setValue;

			// disable all first...
			for (int i = 0; i < controls.Length; i++)
			{
				EnableObject(controls[i], false);
			}

			for (int i = 0; i < controls.Length; i++)
			{
				setValue = (key == controls[i].Tag.ToString());
				if (setValue)
				{
					EnableObject(controls[i], setValue);
					RaiseDayScheduleChangeEvent(controls[i]);
				}
			}

		}
		#endregion

		#region Indexer methods
		#endregion
	}

	#region DayScheduleChanged EventHandler/EventArgs class
	/// <summary>
	/// Event handle for day schedule change event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void DayScheduleChangeEventHandle(object sender, DayScheduleChangedEventArgs e);

	/// <summary>
	/// EventArgs for DayScheduleChange
	/// </summary>
	public class DayScheduleChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The control that changed
		/// </summary>
		public readonly object Control;

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="c"></param>
		public DayScheduleChangedEventArgs(object c)
		{
			this.Control = c;
		}
	}
	#endregion
}
