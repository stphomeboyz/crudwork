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
using System.Text;

namespace crudwork.Utilities
{
	/// <summary>
	/// Date Utility
	/// </summary>
	public class DateUtil
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public DateUtil()
		{
		}

		/// <summary>
		/// Return a unique identifier based on system's full date (yyyymmdd) and time (hhmissms).
		/// 
		/// Output sample: 2006040212445099
		/// </summary>
		public static string UniqueID
		{
			get
			{
				DateTime date = DateTime.Now;

				string uniqueID = String.Format(
					"{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}",
					date.Year, date.Month, date.Day,
					date.Hour, date.Minute, date.Second, date.Millisecond
					);
				return uniqueID;
			}
		}

		/// <summary>
		/// return a elapsed time in formatted string. (hh:mm:ss:mi)
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static string ElapsedTime(long ticks)
		{
			TimeSpan ts = new TimeSpan(ticks);

			return String.Format("{0}:{1}:{2}:{3}",
				ts.Hours,
				ts.Minutes,
				ts.Seconds,
				ts.Milliseconds
			);
		}

		/// <summary>
		/// return a elapsed time in formatted string. (hh:mm:ss:mi)
		/// </summary>
		/// <param name="t1"></param>
		/// <returns></returns>
		public static string ElapsedTime(DateTime t1)
		{
			return ElapsedTime(t1, DateTime.Now);
		}

		/// <summary>
		/// return a elapsed time in formatted string. (hh:mm:ss:mi)
		/// </summary>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <returns></returns>
		public static string ElapsedTime(DateTime t1, DateTime t2)
		{
			if (t2 > t1)
				return ElapsedTime(t2.Ticks - t1.Ticks);
			else
				return ElapsedTime(t1.Ticks - t2.Ticks);
		}


		#region Next/Previous BusinessDay methods
		/// <summary>
		/// Return the previous or next business day of the date specified.
		/// </summary>
		/// <param name="today"></param>
		/// <param name="addValue"></param>
		/// <returns></returns>
		public static DateTime GetBusinessDay(DateTime today, int addValue)
		{
			#region Sanity Checks
			if ((addValue != -1) && (addValue != 1))
				throw new ArgumentOutOfRangeException("addValue must be -1 or 1");
			#endregion

			if (addValue > 0)
				return NextBusinessDay(today);
			else
				return DateUtil.PreviousBusinessDay(today);
		}

		/// <summary>
		/// return the previous business date of the date specified.
		/// </summary>
		/// <param name="today"></param>
		/// <returns></returns>
		public static DateTime PreviousBusinessDay(DateTime today)
		{
			DateTime result;
			switch (today.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					result = today.AddDays(-2);
					break;

				case DayOfWeek.Monday:
					result = today.AddDays(-3);
					break;

				case DayOfWeek.Tuesday:
				case DayOfWeek.Wednesday:
				case DayOfWeek.Thursday:
				case DayOfWeek.Friday:
					result = today.AddDays(-1);
					break;

				case DayOfWeek.Saturday:
					result = today.AddDays(-1);
					break;

				default:
					throw new ArgumentOutOfRangeException("DayOfWeek=" + today.DayOfWeek);
			}
			return ScreenHolidays(result, -1);
		}

		/// <summary>
		/// return the next business date of the date specified.
		/// </summary>
		/// <param name="today"></param>
		/// <returns></returns>
		public static DateTime NextBusinessDay(DateTime today)
		{
			DateTime result;
			switch (today.DayOfWeek)
			{
				case DayOfWeek.Sunday:
				case DayOfWeek.Monday:
				case DayOfWeek.Tuesday:
				case DayOfWeek.Wednesday:
				case DayOfWeek.Thursday:
					result = today.AddDays(1);
					break;

				case DayOfWeek.Friday:
					result = today.AddDays(3);
					break;

				case DayOfWeek.Saturday:
					result = today.AddDays(2);
					break;

				default:
					throw new ArgumentOutOfRangeException("DayOfWeek=" + today.DayOfWeek);
			}
			return ScreenHolidays(result, 1);
		}

		/// <summary>
		/// return the mm/dd string of the date specified.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string MonthDay(DateTime time)
		{
			return String.Format("{0:00}/{1:00}", time.Month, time.Day);
		}

		/// <summary>
		/// screen for holidays 
		/// (simple mode)
		/// </summary>
		/// <param name="result"></param>
		/// <param name="addValue"></param>
		/// <returns></returns>
		public static DateTime ScreenHolidays(DateTime result, int addValue)
		{
			#region Sanity Checks
			if ((addValue != -1) && (addValue != 1))
				throw new ArgumentOutOfRangeException("addValue must be -1 or 1");
			#endregion

			// holidays on fixed date
			switch (MonthDay(result))
			{
				case "01/01":	// Happy New Year
				case "07/04":	// Independent Day
				case "12/25":	// Christmas
					return GetBusinessDay(result, addValue);
				default:
					return result;
			}
		}
		#endregion

		#region HasElapsed method
		private DateTime? then = null;

		/// <summary>
		/// <para>return true if the number of seconds has elapsed since the last check; otherwise return false.</para>
		/// <para>Use this method to raise a status report event to the GUI (for example, every 1 second) to prevent
		/// clogging the message pump.</para>
		/// </summary>
		/// <param name="seconds">specify number of seconds to check</param>
		/// <returns></returns>
		public bool HasElapsed(double seconds)
		{
			return HasElapsed(seconds, true);
		}

		/// <summary>
		/// <para>return true if the number of seconds has elapsed since the last check; otherwise return false.</para>
		/// <para>Use this method to raise a status report event to the GUI (for example, every 1 second) to prevent
		/// clogging the message pump.</para>
		/// </summary>
		/// <param name="seconds">specify number of seconds to check</param>
		/// <param name="mark">set true to mark the time if time has elapsed; or set false to do a test only</param>
		/// <returns></returns>
		public bool HasElapsed(double seconds, bool mark)
		{
			var now = DateTime.Now;

			if (!then.HasValue)
			{
				then = now;
				return false;	// return false, because this is the very first check
			}

			if (then.Value.AddSeconds(seconds) >= now)
				return false;	// the # of seconds has NOT elapsed...

			// yes, it has. mark new time for next check
			if (mark)
				then = now;

			return true;
		}
		#endregion
	}
}
