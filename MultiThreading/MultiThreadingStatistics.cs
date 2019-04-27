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
using System.Text;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Record statistics
	/// </summary>
	public class MultiThreadingStatistics
	{
		#region Enumerators
		#endregion

		#region Fields
		private DateTime start;
		private DateTime stop;
		private TimeSpan ts;
		#endregion

		#region Constructors
		/// <summary>
		/// create new object with given attributes
		/// </summary>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		public MultiThreadingStatistics(DateTime start, DateTime stop)
		{
			this.start = start;
			this.stop = stop;
			this.ts = new TimeSpan(stop.Ticks - start.Ticks);
		}
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		/// <summary>
		/// return the average ticks
		/// </summary>
		/// <param name="statList"></param>
		/// <returns></returns>
		public static long AverageTicks(Dictionary<int, MultiThreadingStatistics> statList)
		{
			long? avg = null;

			for (int i = 0; i < statList.Count; i++)
			{
				if (!avg.HasValue)
				{
					avg = statList[i].ts.Ticks;
				}
				else
				{
					avg += statList[i].ts.Ticks;
				}
			}

			return (!avg.HasValue) ? 0 : avg.Value / statList.Count;
		}

		/// <summary>
		/// return the average milliseconds
		/// </summary>
		/// <param name="statList"></param>
		/// <returns></returns>
		public static int AverageMilliseconds(Dictionary<int,MultiThreadingStatistics> statList)
		{
			int? avg = null;

			for (int i = 0; i < statList.Count; i++)
			{
				if (!avg.HasValue)
				{
					avg = statList[i].ElapsedTime;
				}
				else
				{
					avg += statList[i].ElapsedTime;
				}
			}

			return (!avg.HasValue) ? 0 : avg.Value / statList.Count;
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the start time
		/// </summary>
		[Description("Get or set the start time"), Category("Multi-Threading Statistic")]
		public DateTime Start
		{
			get
			{
				return start;
			}
			set
			{
				start = value;
			}
		}

		/// <summary>
		/// Get or set the stop time
		/// </summary>
		[Description("Get or set the stop time"), Category("Multi-Threading Statistic")]
		public DateTime Stop
		{
			get
			{
				return stop;
			}
			set
			{
				stop = value;
			}
		}

		/// <summary>
		/// Get the elapsed time, in milliseconds
		/// </summary>
		[Description("Get the elapsed time, in milliseconds"), Category("Multi-Threading Statistic")]
		public int ElapsedTime
		{
			get
			{
				return this.ts.Milliseconds;
			}
		}
		#endregion

		#region Others
		#endregion




	}
}
