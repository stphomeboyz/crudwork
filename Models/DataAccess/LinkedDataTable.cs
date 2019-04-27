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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Defines a linked data table
	/// </summary>
	public class LinkedDataTable
	{
		/// <summary>
		/// Get or set the datbase connection of the input source
		/// </summary>
		public DataConnectionInfo Source
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the query
		/// </summary>
		public string Query
		{
			get;
			set;
		}

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public LinkedDataTable()
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="query"></param>
		public LinkedDataTable(DataConnectionInfo source, string query)
		{
			this.Source = source;
			this.Query = query;
		}
	}
}
