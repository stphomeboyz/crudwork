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
	/// Schedule for Holidays
	/// </summary>
	internal partial class HolidaySchedule : Base.ChangeEventUserControl, ISchedule
	{
		#region Fields
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public HolidaySchedule()
		{
			InitializeComponent();
			this.AutoSize = false;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		}
		#endregion

		#region Custom Event methods
		#endregion

		#region Event methods
		private void HolidaySchedule_Load(object sender, EventArgs e)
		{
			cboType.Items.Clear();
			cboType.Items.Add("Federal Holidays");
			cboType.Items.Add("All USA+Canadian Holidays");
			cboType.SelectedIndex = 0;
		}

		private void cboType_SelectedIndexChanged(object sender, EventArgs e)
		{
			string key = cboType.Text.ToUpper();
			switch (key)
			{
				case "FEDERAL HOLIDAYS":
					/*
					 * New Year's Day 
					 * Birthday of Martin Luther King, Jr.
					 * Washington's Birthday
					 * Memorial Day
					 * Independence Day
					 * Labor Day
					 * Columbus Day
					 * Veterans Day
					 * Thanksgiving Day
					 * Christmas Day
					 * */
					GetHolidays(true);
					break;
				case "ALL USA+CANADIAN HOLIDAYS":
					GetHolidays(false);
					break;
			}
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}

		private void lstHolidays_SelectedIndexChanged(object sender, EventArgs e)
		{
			base.RaiseChangeEvent(this, new Base.ChangeEventArgs(sender));
		}
		#endregion

		#region Public methods
		#endregion

		#region Private methods
		private void GetHolidays(bool FederalOnly)
		{
			string query = "select HolidayName, HolidayDate from All_Holidays where (DatePart('yyyy',HolidayDate) between {0} and {1}) {2}";
			int year = DateTime.Now.Year;
			if (FederalOnly)
			{
				query = String.Format(query, year, year + 1, " and Federal='Yes'");
			}
			else
			{
				query = String.Format(query, year, year + 1, "");
			}

			try
			{
				DataTable dt = new DataTable();
				//RealEstateLib.Settings.RunQuery(dt, query);

				lstHolidays.Items.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string s = String.Format("{0:d}: {1}", dt.Rows[i]["HolidayDate"], dt.Rows[i]["HolidayName"]);
					lstHolidays.Items.Add(s);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}
		}

		private string ExactDate(string strHolidayDate)
		{
			try
			{
				string strDate = strHolidayDate.Substring(0, 10);
				return DateTime.Parse(strDate).ToLongDateString();
			}
			catch (Exception ex)
			{
				throw ex;
				//MessageBox.Show(ex.ToString());
			}
		}
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
			int c = lstHolidays.SelectedIndices.Count;
			if (c == 0)
			{
				return new string[0];
			}

			string[] v = new string[c];
			for (int i = 0; i < lstHolidays.SelectedIndices.Count; i++)
			{
				int idx = lstHolidays.SelectedIndices[i];
				//v[i] = "Holiday: " + lstHolidays.Items[idx].ToString();
				v[i] = ExactDate(lstHolidays.Items[idx].ToString());
			}
			return v;
		}
		#endregion
	}
}
