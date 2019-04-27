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
using System.Data.Common;

namespace crudwork.DataAccess.DbCommon
{
	/// <summary>
	/// SchemaManager return schema information.
	/// </summary>
	internal class SchemaManagement
	{
		private DbConnection conn = null;

		/// <summary>
		/// create a manager for the given database connection
		/// </summary>
		/// <param name="conn"></param>
		public SchemaManagement(DbConnection conn)
		{
			this.conn = conn;
		}

		/// <summary>
		/// return a list of tables.
		/// </summary>
		/// <returns></returns>
		public DataTable GetTables()
		{
			return null;
			//return QuerySchema(this.conn, DbGuid);
		}

		/// <summary>
		/// query Schema
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="schema"></param>
		/// <param name="restrictions"></param>
		/// <returns></returns>
		public static DataTable QuerySchema(DbConnection conn, Guid schema, params object[] restrictions)
		{
			return conn.GetSchema(schema.ToString(), (string[])restrictions);
		}
	}
}
