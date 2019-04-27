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

namespace crudwork.Controls.Measurements
{
	/// <summary>
	/// Performance Counter object
	/// </summary>
	public class PerformanceCounter<T> // where T : class
	{
		private DateTime now;
		private T value;

		#region Constructors
		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		public PerformanceCounter()
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="now"></param>
		/// <param name="value"></param>
		public PerformanceCounter(DateTime now, T value)
		{
			this.now = now;
			this.value = value;
		}
		#endregion

		/// <summary>
		/// Get or set performance value
		/// </summary>
		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		/// <summary>
		/// Get or set the current date
		/// </summary>
		public DateTime Now
		{
			get
			{
				return this.now;
			}
			set
			{
				this.now = value;
			}
		}
	}

	/// <summary>
	/// Performance Counter collection
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PerformanceCounterCollection<T> : List<PerformanceCounter<T>>
	{
		private string name;

		#region Constructors
		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="name"></param>
		/// <param name="counters"></param>
		public PerformanceCounterCollection(string name, List<PerformanceCounter<T>> counters)
		{
			this.name = name;
			this.Clear();
			if (counters != null)
				this.AddRange(counters);
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="name"></param>
		public PerformanceCounterCollection(string name)
			: this(name, null)
		{
		}

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public PerformanceCounterCollection()
			: this(string.Empty, null)
		{
		}
		#endregion

		/// <summary>
		/// Get or set the descriptive name for the collection
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
	}
}
