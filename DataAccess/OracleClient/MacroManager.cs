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

namespace crudwork.DataAccess.OracleClient
{
	internal class MacroManager : IMacroManager
	{
		#region IQueryGenerator Members
		/// <summary>
		/// Generate a CREATE TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string CreateTableStatement(string tablename, System.Data.DataTable dt)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Generate the INSERT statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="columns"></param>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		public string CreateInsertStatement(string tablename, System.Data.DataColumnCollection columns, System.Data.DataRow dataRow)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Generate the DROP TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string DropTableStatement(string tablename)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		#endregion

		#region IQueryGenerator Members

		string IMacroManager.CreateTableStatement(string tablename, System.Data.DataTable dt)
		{
			return CreateTableStatement(tablename, dt);
		}

		string IMacroManager.CreateInsertStatement(string tablename, System.Data.DataColumnCollection columns, System.Data.DataRow dataRow)
		{
			return CreateInsertStatement(tablename, columns, dataRow);
		}

		string IMacroManager.DropTableStatement(string tablename)
		{
			return DropTableStatement(tablename);
		}
		#endregion

		#region IDataImport Members


		public string CreateIndexStatement(string tablename, string columnname, string indexname)
		{
			throw new NotImplementedException();
		}

		public string DropIndexStatement(string tablename, string indexname)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDataImport Members


		string IMacroManager.CreateIndexStatement(string tablename, string columnname, string indexname)
		{
			return CreateIndexStatement(tablename, columnname, indexname);
		}

		string IMacroManager.DropIndexStatement(string tablename, string indexname)
		{
			return DropIndexStatement(tablename, indexname);
		}

		#endregion

		#region CopyTable Members
		/// <summary>
		/// Generate a COPY TABLE statement
		/// </summary>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public string CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("create table {1} as select {2} from {0}", inputTable, outputTable, selectClause);
			if (!string.IsNullOrEmpty(whereClause))
				sb.Append(" where " + whereClause);
			return sb.ToString();
		}

		string IMacroManager.CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			return CopyTable(inputTable, outputTable, selectClause, whereClause);
		}
		#endregion
	}
}
