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
	/// The input and output for a currency task
	/// </summary>
	public class ConcurrencyTaskEventArgs<T,U> : EventArgs
	{
		/// <summary>
		/// The input
		/// </summary>
		public T Input;

		/// <summary>
		/// The results
		/// </summary>
		public U Output;

		#region Constructors
		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public ConcurrencyTaskEventArgs()
			: this(default(T), default(U))
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="input"></param>
		public ConcurrencyTaskEventArgs(T input)
			: this(input, default(U))
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public ConcurrencyTaskEventArgs(T input, U output)
			: base()
		{
			this.Input = input;
			this.Output = output;
		}
		#endregion


		/// <summary>
		/// Display string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Input={0} Output={1}", Input, Output);
		}
	}
}
