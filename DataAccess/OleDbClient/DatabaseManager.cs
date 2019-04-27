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
using System.Data.OleDb;
using System.Data;
using crudwork.Utilities;
using crudwork.Models.DataAccess;
using crudwork.DataAccess.DbCommon;

// Helpful tips on using QuerySchema
// - http://support.microsoft.com/kb/309488 (samples)
// - http://msdn.microsoft.com/en-us/library/system.data.oledb.oledbschemaguid_members.aspx

namespace crudwork.DataAccess.OleDbClient
{
	/// <summary>
	/// OleDb Database Manager
	/// </summary>
	internal class DatabaseManager : IDatabaseManager
	{
		private string connectionString = null;

		/// <summary>
		/// create a new instance with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public DatabaseManager(string connectionString)
		{
			this.connectionString = connectionString;
		}

		/// <summary>
		/// fill a table
		/// </summary>
		/// <returns></returns>
		public DataSet FillTables(string tablename, QueryFilter tableFilter)
		{
			try
			{
				var tdl = GetTables(tablename, tableFilter);
				DataSet ds = new DataSet();

				foreach (var item in tdl)
				{
					try
					{
						DataTable dt = FillTable("select * from [" + item.TableName + "]", item.TableName);
						ds.Tables.Add(dt);
					}
					catch (Exception ex)
					{
						DebuggerTool.AddData(ex, "item", item);
						throw;
					}
				}

				return ds;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "tablename", tablename);
				DebuggerTool.AddData(ex, "tableFilter", tableFilter);
				throw;
			}
		}

		/// <summary>
		/// fill a table
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataTable FillTable(string query)
		{
			return FillTable(query, "Query1");
		}

		/// <summary>
		/// fill a table
		/// </summary>
		/// <param name="query"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable FillTable(string query, string tableName)
		{
			try
			{
				using (var conn = new OleDbConnection(this.connectionString))
				using (var cmd = new OleDbCommand(query, conn))
				using (var da = new OleDbDataAdapter(cmd))
				{
					var dt = new DataTable(tableName);
					da.Fill(dt);
					return dt;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "tableName", tableName);
				throw;
			}
		}

		#region Query Schema
		/// <summary>
		/// Return columns for given table
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable GetColumns(string tableName)
		{
			var tm = new TableManager(DatabaseProvider.OleDb, tableName, false);
			return QuerySchema(this.connectionString, OleDbSchemaGuid.Columns, tm.Database, tm.Owner, tm.Tablename, null);
		}

		/// <summary>
		/// Return the primary columns
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable GetPrimaryKeys(string tableName)
		{
			var tm = new TableManager(DatabaseProvider.OleDb, tableName, false);
			return QuerySchema(this.connectionString, OleDbSchemaGuid.Primary_Keys, tm.Database, tm.Owner, tm.Tablename, null);
		}

		/// <summary>
		/// Perform the GetOleDbSchemaTable operation and return a result as DataTable
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="schema"></param>
		/// <param name="restrictions"></param>
		/// <returns></returns>
		public static DataTable QuerySchema(string connectionString, Guid schema, params object[] restrictions)
		{
			if (restrictions.Length != 4)
				throw new ArgumentOutOfRangeException("Expected 4 restrictions; but found " + restrictions.Length);

			using (var conn = new OleDbConnection(connectionString))
			{
				conn.Open();

				try
				{
					return conn.GetOleDbSchemaTable(schema, restrictions);
				}
				catch (Exception ex)
				{
					DebuggerTool.AddData(ex, "connectionString", connectionString);
					DebuggerTool.AddData(ex, "schema", schema.ToString());
					DebuggerTool.AddData(ex, "restrictions", restrictions.ToString());
					throw;
				}
				finally
				{
					if (conn.State != System.Data.ConnectionState.Closed)
						conn.Close();
				}
			}
		}
		#endregion

		#region IDatabaseManager Members
		/// <summary>
		/// Retrieve a service listing.
		/// </summary>
		/// <returns></returns>
		public ServiceDefinitionList GetServices()
		{
			return new ServiceDefinitionList();
			//throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a database listing.
		/// </summary>
		/// <param name="showSystemDatabase"></param>
		/// <returns></returns>
		public DatabaseDefinitionList GetDatabases(bool showSystemDatabase)
		{
			return new DatabaseDefinitionList();
			//throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a table listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="tableFilter"></param>
		/// <returns></returns>
		public TableDefinitionList GetTables(string tableName, QueryFilter tableFilter)
		{
			switch (tableFilter)
			{
				case QueryFilter.Exact:
					if (string.IsNullOrEmpty(tableName))
						throw new ArgumentNullException("tableName");
					return QuerySchema(this.connectionString, OleDbSchemaGuid.Tables, null, null, tableName, "TABLE").ToTableDefinitionList();

				case QueryFilter.None:
					return QuerySchema(this.connectionString, OleDbSchemaGuid.Tables, null, null, null, "TABLE").ToTableDefinitionList();

				case QueryFilter.Contains:
				case QueryFilter.EndsWith:
				case QueryFilter.StartsWith:
					var result = QuerySchema(this.connectionString, OleDbSchemaGuid.Tables, null, null, null, "TABLE").ToTableDefinitionList();
					if (string.IsNullOrEmpty(tableName))
						throw new ArgumentNullException("tableName");

					string tableNameUC = tableName.ToString();

					#region Remove item(s) from collection if they do not match the filtering
					foreach (TableDefinition item in result)
					{
						bool remove = true;

						if (tableFilter == QueryFilter.Contains && item.TableName.ToUpper().Contains(tableNameUC))
							remove = false;
						if (tableFilter == QueryFilter.EndsWith && item.TableName.EndsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
							remove = false;
						if (tableFilter == QueryFilter.StartsWith && item.TableName.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
							remove = false;

						if (remove)
							result.Remove(item);
					}
					#endregion
					return result;

				default:
					throw new ArgumentOutOfRangeException("tableFilter=" + tableFilter);
			}
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables()
		{
			return QuerySchema(this.connectionString, OleDbSchemaGuid.Tables, null, null, null, "TABLE").ToTableDefinitionList();
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <param name="objectType"></param>
		/// <returns></returns>
		public TableDefinitionList GetObjects(ObjectType objectType)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		///  Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="tableFilter"></param>
		/// <param name="columnName"></param>
		/// <param name="columnFilter"></param>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			if (tableFilter != QueryFilter.Exact)
				throw new NotImplementedException("tableFilter supports only QueryFilter.Exact");
			if (columnFilter != QueryFilter.None)
				throw new NotImplementedException("tableFilter supports only QueryFilter.None");
			return GetColumns(tableName).ToColumnDefinitionList();
		}
		/// <summary>
		/// Retrieve a columns listing for all tables.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns()
		{
			return GetColumns(null).ToColumnDefinitionList();
		}
		/// <summary>
		/// Get the primary key(s) for the given table name
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public PrimaryColumnDefinitionList GetPrimaryColumns(string tableName)
		{
			return GetPrimaryKeys(tableName).ToPrimaryColumnDefinitionList();
		}
		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList()
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList(string tableName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region IDatabaseSchema Members

		ServiceDefinitionList IDatabaseManager.GetServices()
		{
			return GetServices();
		}

		DatabaseDefinitionList IDatabaseManager.GetDatabases(bool showSystemDatabase)
		{
			return GetDatabases(showSystemDatabase);
		}

		TableDefinitionList IDatabaseManager.GetTables(string tableName, QueryFilter tableFilter)
		{
			return GetTables(tableName, tableFilter);
		}

		TableDefinitionList IDatabaseManager.GetTables()
		{
			return GetTables();
		}

		TableDefinitionList IDatabaseManager.GetObjects(ObjectType objectType)
		{
			return GetObjects(objectType);
		}

		ColumnDefinitionList IDatabaseManager.GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			return GetColumns(tableName, tableFilter, columnName, columnFilter);
		}

		ColumnDefinitionList IDatabaseManager.GetColumns()
		{
			return GetColumns();
		}

		PrimaryColumnDefinitionList IDatabaseManager.GetPrimaryKeys(string tableName)
		{
			return GetPrimaryColumns(tableName);
		}

		TableDependencyList IDatabaseManager.GetTableDependencyList()
		{
			return GetTableDependencyList();
		}

		TableDependencyList IDatabaseManager.GetTableDependencyList(string tableName)
		{
			return GetTableDependencyList(tableName);
		}
		#endregion

		#region IDatabaseManager GetIndexes Members

		/// <summary>
		/// Retrieve a list of indexes
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetIndexes()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieve a index listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="indexFilter"></param>
		/// <returns></returns>
		public TableDefinitionList GetIndexes(string indexName, QueryFilter indexFilter)
		{
			throw new NotImplementedException();
		}

		TableDefinitionList IDatabaseManager.GetIndexes()
		{
			return GetIndexes();
		}

		TableDefinitionList IDatabaseManager.GetIndexes(string indexName, QueryFilter indexFilter)
		{
			return GetIndexes(indexName, indexFilter);
		}

		#endregion
	}
}
