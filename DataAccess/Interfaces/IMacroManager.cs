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
using System.Data;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Interface for Import Manager
	/// </summary>
	internal interface IMacroManager
	{
		/// <summary>
		/// Generate a CREATE TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		string CreateTableStatement(string tablename, DataTable dt);

		/// <summary>
		/// Generate the INSERT statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="columns"></param>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		string CreateInsertStatement(string tablename, DataColumnCollection columns, DataRow dataRow);

		/// <summary>
		/// Generate the DROP TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		string DropTableStatement(string tablename);

		/// <summary>
		/// Generate the CREATE INDEX statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		string CreateIndexStatement(string tablename, string columnname, string indexname);

		/// <summary>
		/// Generate the DROP INDEX statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		string DropIndexStatement(string tablename, string indexname);

		/// <summary>
		/// Generate a COPY TABLE statement
		/// </summary>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		string CopyTable(string inputTable, string outputTable, string selectClause, string whereClause);
	}
}
