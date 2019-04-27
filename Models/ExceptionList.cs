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
	/// List of Exceptions - use to store all exceptions thrown while raising events 
	/// </summary>
	public class AggregatedException : Exception
	{
		private List<Exception> list = new List<Exception>();

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public AggregatedException()
			: base()
		{
		}

		/// <summary>
		/// add new entry to the list
		/// </summary>
		/// <param name="ex"></param>
		public void Add(Exception ex)
		{
			list.Add(ex);
		}

		/// <summary>
		/// Get the inner exception
		/// </summary>
		public new Exception InnerException
		{
			get
			{
				return list.Count == 0 ? null : list[0];
			}
		}

		/// <summary>
		/// Get the total exceptions on list
		/// </summary>
		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			int c = 0;

			foreach (var item in list)
			{
				c++;
				sb.AppendFormat("{0} = {1}" + Environment.NewLine, c, item.ToString());
			}

			return sb.ToString();
		}
	}

	/// <summary>
	/// Throw this exception if the user clicks on the Cancel button
	/// </summary>
	public class TerminatedByUserException : ApplicationException
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public TerminatedByUserException()
		{
		}

		/// <summary>
		/// return a string presentation of this instance
		/// </summary>
		public override string Message
		{
			get
			{
				return "The background process was terminated by the user";
			}
		}
	}
}

#if SILVERLIGHT
namespace System
{
	/// <summary>
	/// Silverlight V3 core CLR no longer implements the ApplicationException class.
	/// We will create a dummy one -- for backward compatible.
	/// </summary>
	public class ApplicationException : Exception
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ApplicationException()
			: base()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		public ApplicationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		public ApplicationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
#endif
