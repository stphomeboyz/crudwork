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
namespace crudwork.Concurrency
{
	/// <summary>
	/// Interface for ConcurrencyTask
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	public interface IConcurrencyTask<T,U>
	{
		/// <summary>
		/// Do custom processing here
		/// </summary>
		/// <param name="e"></param>
		void DoProcess(ConcurrencyTaskEventArgs<T, U> e);
	}
}
