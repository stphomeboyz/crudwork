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
using System.Threading;

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Interface for processing in MultiThreadings mode
	/// </summary>
	public interface IMultiThreading
	{
#if !SILVERLIGHT
		/// <summary>
		/// Start asynchronously
		/// </summary>
		/// <param name="threadName"></param>
		/// <param name="threadPriority"></param>
		void StartAsync(string threadName, ThreadPriority threadPriority);
#else
		/// <summary>
		/// Start asynchronously
		/// </summary>
		/// <param name="threadName"></param>
		void StartAsync(string threadName);
#endif

		/// <summary>
		/// Start synchronously
		/// </summary>
		void Start();
	}
}
