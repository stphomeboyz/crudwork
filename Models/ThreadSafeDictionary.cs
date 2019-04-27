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

namespace crudwork.Models
{
	/// <summary>
	/// A thread-safe container to wrap a non thread-safe object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ThreadSafeContainer<T>
	{
		#region Fields
		private object lockObj = new object();
		private T _inner = default(T);
		#endregion

		#region Constructors
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ThreadSafeContainer()
		{
		}
		#endregion

		/// <summary>
		/// Get or set the thread-safe object.
		/// </summary>
		public T Inner
		{
			get
			{
				lock (lockObj)
				{
					return _inner;
				}
			}
			set
			{
				lock (lockObj)
				{
					_inner = value;
				}
			}
		}
	}
}
