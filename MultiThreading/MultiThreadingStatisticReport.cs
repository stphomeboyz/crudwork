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

namespace crudwork.MultiThreading
{
	/// <summary>
	/// The statistic reporting class for MultiThreading class
	/// </summary>
	public class MultiThreadingStatisticReport
	{
		/// <summary>
		/// number of operations per interval
		/// </summary>
		private int opi;
		private int minOpi;
		private int maxOpi;
		private int averageOpi;
		private int lastOpi;

		private DateTime lastUpdated;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public MultiThreadingStatisticReport()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="minOpi"></param>
		/// <param name="maxOpi"></param>
		/// <param name="averageOpi"></param>
		/// <param name="lastOpi"></param>
		/// <param name="opi"></param>
		public MultiThreadingStatisticReport(int minOpi, int maxOpi, int averageOpi, int lastOpi, int opi)
		{
			this.minOpi = minOpi;
			this.maxOpi = maxOpi;
			this.averageOpi = averageOpi;
			this.lastOpi = lastOpi;
			this.opi = opi;
		}
	
		/// <summary>
		/// add or accumulate the total number of operations since last interval.
		/// </summary>
		/// <param name="e"></param>
		public void Add(WorkStatusEventArgs e)
		{
			if (opi < minOpi)
				minOpi = opi;
	
			if (opi > maxOpi)
				maxOpi = opi;

			lastOpi = opi;
			averageOpi = (averageOpi + opi) / 2;
			lastUpdated = DateTime.Now;
		}
	}
}
