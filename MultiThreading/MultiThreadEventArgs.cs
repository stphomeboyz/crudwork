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
	/// EventArgs used by the MultiThread class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	public class MultiThreadEventArgs<T,U> : EventArgs
	{
		#region Fields
		/// <summary>
		/// The input used for processing
		/// </summary>
		private T input;

		/// <summary>
		/// The output
		/// </summary>
		private U result;

		/// <summary>
		/// The relative index
		/// </summary>
		private int index;
		#endregion

		#region Constructors
		/// <summary>
		/// create new object with given attributes
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		public MultiThreadEventArgs(T input, int index)
		{
			this.input = input;
			this.index = index;
			this.result = default(U);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the input value for process
		/// </summary>
		public T Input
		{
			get
			{
				return this.input;
			}
		}

		/// <summary>
		/// Get the relative index offset
		/// </summary>
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>
		/// Get or set the returning result.
		/// </summary>
		public U Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
			}
		}
		#endregion
	}
}
