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
using crudwork.Models.DataAccess;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Database Manager
	/// </summary>
	public class DatabaseManager : IDatabaseManager
	{
		#region Enumerators
		#endregion

		#region Fields
		private IDatabaseManager databaseSchema = null;
		private DatabaseProvider databaseProvider = DatabaseProvider.Unspecified;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="databaseProvider"></param>
		/// <param name="dataFactory"></param>
		public DatabaseManager(DatabaseProvider databaseProvider, DataFactory dataFactory)
		{
			this.databaseProvider = databaseProvider;

			switch (databaseProvider)
			{
				case DatabaseProvider.SqlClient:
					databaseSchema = new SqlClient.DatabaseManager(dataFactory);
					break;

				case DatabaseProvider.OracleClient:
					databaseSchema = new OracleClient.DatabaseManager(dataFactory);
					break;

				case DatabaseProvider.OleDb:
					databaseSchema = new OleDbClient.DatabaseManager(dataFactory.ConnectionStringManager.ConnectionString);
					break;

				case DatabaseProvider.SQLite:
					databaseSchema = new SQLiteClient.DatabaseManager(dataFactory);
					break;

				default:
					throw new ArgumentOutOfRangeException("databaseProvider=" + databaseProvider);
			}
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="databaseProvider"></param>
		/// <param name="connectionString"></param>
		public DatabaseManager(DatabaseProvider databaseProvider, string connectionString)
			: this(databaseProvider, new DataFactory(databaseProvider, connectionString))
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="dataFactory"></param>
		public DatabaseManager(DataFactory dataFactory)
			: this(dataFactory.Provider, dataFactory)
		{
		}
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		/// <summary>
		/// retrieve a list of primary tables.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable GetPrimaryTables(string tableName)
		{
			DataTable dt = new DataTable("PrimaryTables");
			dt.Columns.Add("P_TABLE_NAME", typeof(string));

			var list = GetTableDependencyList(tableName);
			foreach (var item in list)
			{
				dt.Rows.Add(item.PrimaryTableName);
			}

			return dt;
		}

		/// <summary>
		/// retrieve a list of primary tables.
		/// </summary>
		/// <returns></returns>
		public DataTable GetPrimaryTables()
		{
			return GetPrimaryTables(string.Empty);
		}

		/// <summary>
		/// get a list of tables that depend on a given parent tablename.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable GetForeignTables(string tableName)
		{
			DataTable dt = new DataTable("ForeignTables");
			dt.Columns.Add("P_TABLE_NAME", typeof(string));
			dt.Columns.Add("F_TABLE_NAME", typeof(string));

			var list = GetTableDependencyList(tableName);
			foreach (var item in list)
			{
				dt.Rows.Add(item.PrimaryTableName, item.ForeignTableName);
			}

			return dt;
		}

		/// <summary>
		/// Get a list of primary columns for a given tablename.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable GetPrimaryKeys2(string tableName)
		{
			DataTable dt = new DataTable("PrimaryKeys");
			dt.Columns.Add("P_TABLE_NAME", typeof(string));
			dt.Columns.Add("P_COLUMN_NAME", typeof(string));

			var list = GetTableDependencyList(tableName);
			foreach (var item in list)
			{
				dt.Rows.Add(item.PrimaryTableName, item.PrimaryColumnName);
			}

			return dt;
		}

		/// <summary>
		/// Get the key columns used between a given parent table and a given child table.
		/// </summary>
		/// <param name="primaryTable"></param>
		/// <param name="foreignTable"></param>
		/// <returns></returns>
		public DataTable GetForeignKeys(string primaryTable, string foreignTable)
		{
			DataTable dt = new DataTable("ForeignKey");
			dt.Columns.Add("P_TABLE_NAME", typeof(string));
			dt.Columns.Add("P_COLUMN_NAME", typeof(string));
			dt.Columns.Add("F_TABLE_NAME", typeof(string));
			dt.Columns.Add("F_COLUMN_NAME", typeof(string));

			var list = GetTableDependencyList(primaryTable);
			foreach (var item in list)
			{
				dt.Rows.Add(item.PrimaryTableName, item.PrimaryColumnName, item.ForeignTableName, item.ForeignColumnName);
			}

			return dt;
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		#endregion

		#region Others
		#endregion

		#region IDatabaseSchema Members
		/// <summary>
		/// Retrieve a service listing.
		/// </summary>
		/// <returns></returns>
		public ServiceDefinitionList GetServices()
		{
			return databaseSchema.GetServices();
		}
		/// <summary>
		/// Retrieve a database listing.
		/// </summary>
		/// <returns></returns>
		public DatabaseDefinitionList GetDatabases(bool showSystemDatabase)
		{
			return databaseSchema.GetDatabases(showSystemDatabase);
		}
		/// <summary>
		/// Retrieve a table listing for a given tableName and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables(string tableName, QueryFilter tableFilter)
		{
			TableManager tm = new TableManager(databaseProvider, tableName, false);
			return databaseSchema.GetTables(tm.Tablename, tableFilter);
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables()
		{
			return databaseSchema.GetTables();
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetObjects(ObjectType objectType)
		{
			return databaseSchema.GetObjects(objectType);
		}
		/// <summary>
		/// Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			TableManager tm = new TableManager(databaseProvider, tableName, false);
			return databaseSchema.GetColumns(tm.Tablename, tableFilter, columnName, columnFilter);
		}
		/// <summary>
		/// Retrieve a columns listing for all tables.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns()
		{
			return databaseSchema.GetColumns();
		}
		/// <summary>
		/// Get the primary key(s) for the given table name
		/// </summary>
		/// <returns></returns>
		public PrimaryColumnDefinitionList GetPrimaryKeys(string tableName)
		{
			return databaseSchema.GetPrimaryKeys(tableName);
		}
		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList()
		{
			return databaseSchema.GetTableDependencyList();
		}
		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList(string tableName)
		{
			TableManager tm = new TableManager(databaseProvider, tableName, false);
			return databaseSchema.GetTableDependencyList(tm.Tablename);
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
			return GetPrimaryKeys(tableName);
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
			return databaseSchema.GetIndexes();
		}

		/// <summary>
		/// Retrieve a index listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="indexFilter"></param>
		/// <returns></returns>
		public TableDefinitionList GetIndexes(string indexName, QueryFilter indexFilter)
		{
			return databaseSchema.GetIndexes(indexName, indexFilter);
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
