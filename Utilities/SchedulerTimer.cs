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
using System.Linq;
using System.Text;

namespace crudwork.Utilities
{
	/// <summary>
	/// Round off style
	/// </summary>
	public enum RoundOffStyle
	{
		/// <summary>Round off to the nearest quadrant (0, 15, 30, 45)</summary>
		Quad,
		/// <summary>Round off to zero</summary>
		Zero,
		/// <summary>Do not round off</summary>
		None,
	}

	/// <summary>
	/// Schedule Timer - a simple implement to start a scheduled task at an interval rounded to the nearest quadrant.
	/// </summary>
	public class SchedulerTimer
	{
		#region Properties / Fields
		/// <summary>Get or set the start time</summary>
		public DateTime StartTime
		{
			get;
			set;
		}

		/// <summary>Get or set the Day portion interval</summary>
		public int Day
		{
			get;
			set;
		}
		/// <summary>Get or set the Hour portion interval</summary>
		public int Hour
		{
			get;
			set;
		}
		/// <summary>Get or set the Minute portion interval</summary>
		public int Minute
		{
			get;
			set;
		}
		/// <summary>Get or set the Second portion interval</summary>
		public int Second
		{
			get;
			set;
		}

		/// <summary>Get or set the round-off style for hour</summary>
		public RoundOffStyle RoundOffHour
		{
			get;
			set;
		}
		/// <summary>Get or set the round-off style for minute</summary>
		public RoundOffStyle RoundOffMinute
		{
			get;
			set;
		}
		/// <summary>Get or set the round-off style for second</summary>
		public RoundOffStyle RoundOffSecond
		{
			get;
			set;
		}

		private const int ONE_SECOND = 1000 * 1;
		private const int ONE_MINUTE = ONE_SECOND * 60;
		private const int ONE_HOUR = ONE_MINUTE * 60;
		#endregion

		/* Sample usage:
		 * 
		 * schedule a task once an hour (1:00, 2:00, 3:00, etc...)
		 *		TimerEx t = new TimerEx(DateTime.Now, 0, 1, 0, 0, false, true, true)
		 *
		 * schedule a task once a day at 12:00am
		 *		TimerEx t = new TimerEx(DateTime.Now, 1, 0, 0, 0, true, true, true)
		 * 
		 * schedule a task once every 10 minutes
		 *		TimerEx t = new TimerEx(DateTime.Now, 0, 0, 10, 0, false, true, true)
		 * 
		 */

		#region Constructions
		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="day"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="roundOffHour"></param>
		/// <param name="roundOffMinute"></param>
		/// <param name="roundOffSecond"></param>
		public SchedulerTimer(DateTime dt, int day, int hour, int minute, int second, RoundOffStyle roundOffHour, RoundOffStyle roundOffMinute, RoundOffStyle roundOffSecond)
		{
			this.StartTime = dt;

			this.Day = day;
			this.Hour = hour;
			this.Minute = minute;
			this.Second = second;

			RoundOffHour = roundOffHour;
			RoundOffMinute = roundOffMinute;
			RoundOffSecond = roundOffSecond;
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="roundOffMinute"></param>
		/// <param name="roundOffSecond"></param>
		public SchedulerTimer(int hour, int minute, int second, RoundOffStyle roundOffMinute, RoundOffStyle roundOffSecond)
			: this(DateTime.Now, 0, hour, minute, second, RoundOffStyle.None, roundOffMinute, roundOffSecond)
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="day"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="roundOffHour"></param>
		/// <param name="roundOffMinute"></param>
		/// <param name="roundOffSecond"></param>
		public SchedulerTimer(int day, int hour, int minute, int second, RoundOffStyle roundOffHour, RoundOffStyle roundOffMinute, RoundOffStyle roundOffSecond)
			: this(DateTime.Now, day, hour, minute, second, roundOffHour, roundOffMinute, roundOffSecond)
		{
		}
		#endregion

		#region Helpers
		private DateTime AddHour(DateTime results, int addValue, RoundOffStyle style)
		{
			var dt = results;
			dt = dt.AddHours(addValue);
			dt = dt.AddHours(PadToQuad(dt.Hour, style, false));
			return dt;
		}

		private DateTime AddMinute(DateTime results, int addValue, RoundOffStyle style)
		{
			var dt = results;
			dt = dt.AddMinutes(addValue);
			dt = dt.AddMinutes(PadToQuad(dt.Minute, style, true));
			return dt;
		}

		private DateTime AddSecond(DateTime results, int addValue, RoundOffStyle style)
		{
			var dt = results;
			dt = dt.AddSeconds(addValue);
			dt = dt.AddSeconds(PadToQuad(dt.Second, style, true));
			return dt;
		}

		private int PadToQuad(int value, RoundOffStyle style, bool isSixty)
		{
			switch (style)
			{
				case RoundOffStyle.Quad:
					return (isSixty ? Quad60(value) : Quad12(value)) - value;

				case RoundOffStyle.Zero:
					return -value;

				case RoundOffStyle.None:
					return 0;

				default:
					throw new ArgumentOutOfRangeException("style=" + style);
			}
		}

		private int Quad12(int value)
		{
			if (value < 1 || value > 23)
				throw new ArgumentOutOfRangeException("value must be between 0 and 23");

			return (value == 1 ? 1 :
				value <= 2 ? 2 :
				value <= 3 ? 3 :
				value <= 4 ? 4 :
				value <= 6 ? 6 :
				value <= 8 ? 8 :
				value <= 12 ? 12 :
				value <= 14 ? 14 :
				value <= 15 ? 15 :
				value <= 16 ? 16 :
				value <= 18 ? 18 :
				value <= 20 ? 20 :
				24);
		}

		private int Quad60(int value)
		{
			if (value < 0 || value > 59)
				throw new ArgumentOutOfRangeException("value must be between 0 and 59");

			return (value == 0 ? 0 :
				value <= 15 ? 1 :
				value <= 30 ? 2 :
				value <= 45 ? 3 : 0) * 15;
		}
		#endregion

		#region Public
		/// <summary>
		/// get the next schedule time
		/// </summary>
		public DateTime NextTime
		{
			get
			{
				var results = StartTime;
				results = results.AddDays(this.Day);
				results = AddHour(results, this.Hour, RoundOffHour);
				results = AddMinute(results, this.Minute, RoundOffMinute);
				results = AddSecond(results, this.Second, RoundOffSecond);
				return results;
			}
		}

		/// <summary>
		/// return the interval (in ms) with the minute and second rounded off.
		/// </summary>
		public double RoundOffInterval
		{
			get
			{
				var now = StartTime;
				var next = NextTime;

				TimeSpan ts = new TimeSpan(next.Ticks - now.Ticks);

				if (ts.TotalMilliseconds < 0)
					throw new Exception("Total ms must be greater than 0");

				return ts.TotalMilliseconds;
			}
		}

		/// <summary>
		/// return the interval in milliseconds
		/// </summary>
		public double Interval
		{
			get
			{
				return (Hour * ONE_HOUR) + (Minute * ONE_MINUTE) + (Second * ONE_SECOND);
			}
		}

		/// <summary>
		/// get string presentation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Interval=[{10}d {0}h {1}m {2}s] \nstart={6} \n next={7} \ninterval={9} \nRoundOff[h={3} m={4} s={5} i={8}]",
				Hour, Minute, Second,
				RoundOffHour, RoundOffMinute, RoundOffSecond,
				StartTime.ToString("s"), NextTime.ToString("s"),
				RoundOffInterval, Interval,
				Day);
		}
		#endregion

		#region Static methods
		///// <summary>
		///// Create a new SchedulerTimer instance with given interval.
		///// The schedule task will be scheduled to run at 12AM daily.
		///// </summary>
		///// <param name="interval"></param>
		///// <returns></returns>
		//public static SchedulerTimer Daily(int interval)
		//{
		//    return new SchedulerTimer(DateTime.Now,
		//        interval, 0, 0, 0,
		//        RoundOffStyle.Zero, RoundOffStyle.Zero, RoundOffStyle.Zero);
		//}

		///// <summary>
		///// Create a new SchedulerTimer instance with given interval
		///// The schedule task will be scheduled to run hourly at 1:00, 2:00, 3:00, etc...
		///// </summary>
		///// <param name="interval"></param>
		///// <returns></returns>
		//public static SchedulerTimer Hourly(int interval, RoundOffStyle rounding)
		//{
		//    return new SchedulerTimer(DateTime.Now, 
		//        0, interval, 0, 0,
		//        rounding, RoundOffStyle.Zero, RoundOffStyle.Zero);
		//}
		#endregion
	}
}
