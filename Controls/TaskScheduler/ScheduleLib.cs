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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace crudwork.Controls.TaskScheduler
{
	/// <summary>
	/// Schedule library
	/// </summary>
	internal static class ScheduleLib
	{
		#region Conversion methods
		/// <summary>
		/// Convert the month string to number value
		/// </summary>
		/// <param name="strMonth"></param>
		/// <returns></returns>
		public static int ConvertMonth(string strMonth)
		{
			switch (strMonth.ToUpper())
			{
				case "JANUARY": return 0;
				case "FEBRUARY": return 1;
				case "MARCH": return 2;
				case "APRIL": return 3;
				case "MAY": return 4;
				case "JUNE": return 5;
				case "JULY": return 6;
				case "AUGUST": return 7;
				case "SEPTEMBER": return 8;
				case "OCTOBER": return 9;
				case "NOVEMBER": return 10;
				case "DECEMBER": return 11;
				default: throw new ArgumentOutOfRangeException("strMonth=" + strMonth);
			}
		}

		/// <summary>
		/// Convert the day string to a DayOfWeek value
		/// </summary>
		/// <param name="strDayOfWeek"></param>
		/// <returns></returns>
		public static DayOfWeek ConvertDayOfWeek(string strDayOfWeek)
		{
			return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), strDayOfWeek);
		}
		#endregion

		#region GetDayOfWeek methods
		/// <summary>
		/// get days of the week
		/// </summary>
		/// <param name="strDayOfWeek"></param>
		/// <param name="mmyy"></param>
		/// <param name="frequencies"></param>
		/// <returns></returns>
		public static string[] GetDayOfWeek(string strDayOfWeek, string mmyy, decimal frequencies)
		{
			try
			{
				Regex re = new Regex(@"^(?<month>\d+)/(?<year>\d+)$");
				Match m = re.Match(mmyy);
				if (!m.Success)
				{
					throw new ArgumentException("mmyy=" + mmyy);
				}

				int year = int.Parse(m.Groups["year"].ToString());
				int month = int.Parse(m.Groups["month"].ToString());

				DayOfWeek dayOfWeek = ConvertDayOfWeek(strDayOfWeek);
				return GetDayOfWeek(dayOfWeek, year, month, frequencies);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Get day of week
		/// </summary>
		/// <param name="dayOfWeek"></param>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="frequencies"></param>
		/// <returns></returns>
		public static string[] GetDayOfWeek(DayOfWeek dayOfWeek, int year, int month, decimal frequencies)
		{
			ArrayList ls = new ArrayList();

			DateTime d;
			int day = 0;

			// look for month's first dayOfWeek
			do
			{
				day++;
				d = new DateTime(year, month, day);
			} while (d.DayOfWeek != dayOfWeek);

			// goodies...
			int daysInMonth = DateTime.DaysInMonth(d.Year, d.Month);
			for (int i = day; i <= daysInMonth; i += 7 * (int)frequencies)
			{
				string s = String.Format("{0}/{1}/{2}", month, i, year);
				try
				{
					string s2 = DateTime.Parse(s).ToLongDateString();
					ls.Add(s2);
				}
				catch (Exception ex)
				{
					string errmsg = String.Format("Error while parsing date '{0}': {1}", s, ex.ToString());
					Debug.Print(errmsg);
					//MessageBox.Show(errmsg);
					throw ex;
				}
			}

			return (string[])ls.ToArray(typeof(string));
		}
		#endregion
	}
}
