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

namespace crudwork.Concurrency
{
	/// <summary>
	/// Define a task for concurrency processing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	public abstract class ConcurrencyTask<T, U> : IEnumerable<U>, crudwork.Concurrency.IConcurrencyTask<T,U>
	{
		/*
		 * Step to initialize this instance:
		 * 
		 * 1) Create new class and inherit this base class.
		 * 2) Implement or override the DoProcess method.
		 * 
		 * 3) Instance up the object
		 * 4) Set the_object.Enumerator to an IEnumerable object
		 * 5) do foreach(var item in the_object) { ... }
		 * 
		 * */

		private IEnumerable<T> enumerator;

		#region Constructors
		/// <summary>
		/// Create a new instance with default attribute
		/// </summary>
		public ConcurrencyTask()
			: this(null)
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="enumerator"></param>
		public ConcurrencyTask(IEnumerable<T> enumerator)
		{
			this.enumerator = enumerator;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the Enumerator
		/// </summary>
		public IEnumerable<T> Enumerator
		{
			get
			{
				return this.enumerator;
			}
			set
			{
				this.enumerator = value;
			}
		}
		#endregion

		/// <summary>
		/// Do custom processing here
		/// </summary>
		/// <param name="e"></param>
		public abstract void DoProcess(ConcurrencyTaskEventArgs<T, U> e);

		#region IEnumerable<U> Members

		IEnumerator<U> IEnumerable<U>.GetEnumerator()
		{
			if (this.enumerator == null)
				throw new ArgumentNullException("Enumerator is null");
			
			foreach (var input in this.enumerator)
			{
				ConcurrencyTaskEventArgs<T, U> e = new ConcurrencyTaskEventArgs<T, U>(input);
				DoProcess(e);
				yield return e.Output;
			}

			yield break;
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException("not yet implemented: Concurrency.GetEnumerator");
		}

		#endregion
	}
}
