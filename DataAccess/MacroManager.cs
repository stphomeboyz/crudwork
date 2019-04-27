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
using System.Diagnostics;
using crudwork.Utilities;
using crudwork.Models.DataAccess;
using crudwork.DataAccess.DbCommon;
using crudwork.DynamicRuntime;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Data Import Manager
	/// </summary>
	public class MacroManager : IMacroManager
	{
		#region Fields
		private string connectionString;
		private DataFactory df;
		private IMacroManager queryGenerator;
		private MacroLibrary macroLib;
		private MacroList macros = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="dataFactory"></param>
		public MacroManager(DataFactory dataFactory)
		{
			this.df = dataFactory;
			this.connectionString = this.df.ConnectionStringManager.ConnectionString;

			macroLib = new MacroLibrary(dataFactory);

			switch (this.df.Provider)
			{
				case DatabaseProvider.SqlClient:
					queryGenerator = new SqlClient.MacroManager();
					break;

				case DatabaseProvider.OracleClient:
					queryGenerator = new OracleClient.MacroManager();
					break;

				case DatabaseProvider.OleDb:
					queryGenerator = new OleDbClient.MacroManager();
					break;

				case DatabaseProvider.SQLite:
					queryGenerator = new SQLiteClient.MacroManager();
					break;

				default:
					throw new ArgumentOutOfRangeException("providerType=" + this.df.Provider);
			}
		}
		#endregion

		#region CreateNewTable
		/// <summary>
		/// Create a new table (or append to table, if exist) using the given DataTable.
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		public void CreateNewTable(string tablename, DataTable dt)
		{
			CreateNewTable(tablename, dt, false);
		}

		/// <summary>
		/// Create a new table using the given DataTable
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		/// <param name="dropTable">set true to drop table (if one already exists); or set false to append to table</param>
		public void CreateNewTable(string tablename, DataTable dt, bool dropTable)
		{
			DbConnection conn = null;
			DbTransaction tran = null;

			try
			{
				bool dropped = false;	// possibly a bug in SQLite?

				conn = df.GetConnection();
				conn.Open();

				tran = conn.BeginTransaction();

				if (dropTable && TableExists(tablename))
				{
					// ---- drop table, if exists ----
					string query = DropTableStatement(tablename);
					df.ExecuteNonQuery(query, conn, tran);
					dropped = true;
				}

				if (!TableExists(tablename) || dropped)
				{
					// ---- create table ----
					string query = CreateTableStatement(tablename, dt);
					df.ExecuteNonQuery(query, conn, tran);
				}

				// ---- insert data into table ----
				for (int nr = 0; nr < dt.Rows.Count; nr++)
				{
					string query = CreateInsertStatement(tablename, dt.Columns, dt.Rows[nr]);
					//Debug.Print("Query: " + query);
					df.ExecuteNonQuery(query, conn, tran);
				}

				tran.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "tablename", tablename);
				DebuggerTool.AddData(ex, "dt", dt);

				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (conn != null)
					conn.Dispose();
			}
		}

		/// <summary>
		/// Return true if a table name exists; otherwise return false.
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		private bool TableExists(string tablename)
		{
			return df.Database.GetTables(tablename, QueryFilter.Exact).Count >= 1;
		}

		/// <summary>
		/// Return true if an index exists; otherwise return false.
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		private bool IndexExists(string tablename)
		{
			return df.Database.GetObjects(ObjectType.Index).Count >= 1;
		}
		#endregion

		/// <summary>
		/// Execute macro statement
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataSet Execute(string query)
		{
			query = RegexUtil.Sub(query, "^[ \t]*@[ \t]*", "");
			var tokens = query.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			#region Invoke Macro
			if (macros == null)
			{
				macros = new MacroList();
				macros.AddInstance(macroLib);
			}

			var mx = macros.Find(p =>
			{
				var m = MacroList.GetAttribute<MacroMethodAttribute>(p).Name ?? p.MethodInfo.Name;
				return m.Equals(tokens[0], StringComparison.InvariantCultureIgnoreCase);
			});

			if (mx == null)
				throw new ArgumentException("macro not found: " + tokens[0]);


			object result = null;

			try
			{
				if (mx.MethodInfo.Name.Equals("MacroHelp", StringComparison.InvariantCultureIgnoreCase))
				{
					result = DynamicCode.InvokeMethod(mx.Instance, "MacroHelp", macros, tokens);
				}
				else
				{
					var parameters = MacroList.CreateParameter(mx.MethodInfo, tokens);
					result = DynamicCode.InvokeMethod(mx.Instance, mx.MethodInfo.Name, parameters);
				}
			}
			catch (Exception ex)
			{
				throw new ArgumentException(ex.Message + Environment.NewLine + MacroList.CreateHelp(mx.MethodInfo), ex);
			}
			#endregion

			#region Return DataSet
			var ds = new DataSet("MacroResults");

			if (result is DataTable)
			{
				ds.Tables.Add(result as DataTable);
			}
			else
			{
				var dt = new DataTable();
				dt.Columns.Add("Value", mx.MethodInfo.ReturnType);
				dt.Rows.Add(result);
				ds.Tables.Add(dt);
			}
			#endregion

			return ds;
		}

		#region IQueryGenerator Members
		/// <summary>
		/// Generate a CREATE TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string CreateTableStatement(string tablename, DataTable dt)
		{
			return queryGenerator.CreateTableStatement(tablename, dt);
		}

		/// <summary>
		/// Generate the INSERT statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="columns"></param>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		public string CreateInsertStatement(string tablename, DataColumnCollection columns, DataRow dataRow)
		{
			return queryGenerator.CreateInsertStatement(tablename, columns, dataRow);
		}

		/// <summary>
		/// Generate the DROP TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string DropTableStatement(string tablename)
		{
			return queryGenerator.DropTableStatement(tablename);
		}

		/// <summary>
		/// Generate the CREATE INDEX statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string CreateIndexStatement(string tablename, string columnname, string indexname)
		{
			if (string.IsNullOrEmpty(indexname))
				indexname = string.Format("[ix_{0}_{1}]", tablename, columnname);

			return queryGenerator.CreateIndexStatement(tablename, columnname, indexname);
		}

		/// <summary>
		/// Generate the CREATE TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string DropIndexStatement(string tablename, string indexname)
		{
			return queryGenerator.DropIndexStatement(tablename, indexname);
		}
		#endregion

		#region IQueryGenerator Members

		string IMacroManager.CreateTableStatement(string tablename, DataTable dt)
		{
			return CreateTableStatement(tablename, dt);
		}

		string IMacroManager.CreateInsertStatement(string tablename, DataColumnCollection columns, DataRow dataRow)
		{
			return CreateInsertStatement(tablename, columns, dataRow);
		}

		string IMacroManager.DropTableStatement(string tablename)
		{
			return DropTableStatement(tablename);
		}

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
			if (string.IsNullOrEmpty(selectClause))
				selectClause = "*";
			return queryGenerator.CopyTable(inputTable, outputTable, selectClause, whereClause);
		}

		string IMacroManager.CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			return CopyTable(inputTable, outputTable, selectClause, whereClause);
		}
		#endregion
	}
}
