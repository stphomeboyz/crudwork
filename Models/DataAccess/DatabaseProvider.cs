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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// The database provider enumerator.
	/// </summary>
	public enum DatabaseProvider
	{
		/// <summary>Unspecified</summary>
		Unspecified = -1,
		/// <summary>SqlClient</summary>
		SqlClient = 1,
		/// <summary>OracleClient</summary>
		OracleClient = 2,
		/// <summary>OracleDataProvider</summary>
		OracleDataProvider = 3,
		/// <summary>OleDb</summary>
		OleDb = 4,
		/// <summary>Odbc</summary>
		Odbc = 5,
		/// <summary>SQLite 3.6.3+</summary>
		SQLite = 6,
	}
}
