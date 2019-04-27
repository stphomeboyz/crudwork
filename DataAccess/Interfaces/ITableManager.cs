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
namespace crudwork.DataAccess
{
	/// <summary>
	/// Interface for the Table Manager
	/// </summary>
	internal interface ITableManager
	{
		/// <summary>
		/// return the Database
		/// </summary>
		string Database
		{
			get;
			set;
		}

		/// <summary>
		/// return the fully-qualified table name
		/// </summary>
		string FullTablename
		{
			get;
			set;
		}

		/// <summary>
		/// return the owner
		/// </summary>
		string Owner
		{
			get;
			set;
		}

		/// <summary>
		/// return the service name
		/// </summary>
		string Servicename
		{
			get;
			set;
		}

		/// <summary>
		/// return the tablename
		/// </summary>
		string Tablename
		{
			get;
			set;
		}

		/// <summary>
		/// return the quote indicator
		/// </summary>
		bool UseQuote
		{
			get;
			set;
		}
	}
}
