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
using crudwork.Models.DataAccess;
using System.Data;
using crudwork.DataAccess.DbCommon;
using System.Data.SQLite;

namespace crudwork.DataAccess.SQLiteClient
{
	internal class DatabaseManager:IDatabaseManager
	{
		#region Fields
		private DataFactory dataFactory = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance with specific attribute
		/// </summary>
		/// <param name="dataFactory"></param>
		public DatabaseManager(DataFactory dataFactory)
		{
			this.dataFactory = dataFactory;

			#region Use System.Data.SQLite assembly...
			// IMPORTANT: Do something here to force the compiler to add a reference to System.Data.SQLite assembly.
			SQLiteErrorCode x = SQLiteErrorCode.Ok;
			if (x == SQLiteErrorCode.Busy)
			{
				x = SQLiteErrorCode.Ok;
			}
			#endregion
		}
		#endregion

		private TableDefinitionList QueryMaster(ObjectType type, string name, QueryFilter filter)
		{
			//CREATE TABLE sqlite_master (
			//	type TEXT,
			//	name TEXT,
			//	tbl_name TEXT,
			//	rootpage INTEGER,
			//	sql TEXT
			//);

			StringBuilder query = new StringBuilder();
			query.Append("select type as TABLE_TYPE, name as TABLE_NAME, tbl_name, '' as TABLE_CATALOG, '' as TABLE_SCHEMA, rootpage, sql from sqlite_master where ");

			switch (type)
			{
				case ObjectType.Table:
					query.Append("type = 'table'");
					break;

				case ObjectType.Index:
					query.Append("type = 'index'");
					break;

				default:
					throw new ArgumentOutOfRangeException("type=" + type);
			}

			query.Append(" and ");

			switch (filter)
			{
				case QueryFilter.Contains:
					query.AppendFormat("upper(name) like upper('%{0}%')", name);
					break;
				case QueryFilter.EndsWith:
					query.AppendFormat("upper(name) like upper('{0}%')", name);
					break;
				case QueryFilter.Exact:
					query.AppendFormat("upper(name) = upper('{0}')", name);
					break;
				case QueryFilter.None:
					query.Append("1 = 1");
					break;
				case QueryFilter.StartsWith:
					query.AppendFormat("upper(name) like upper('%{0}')", name);
					break;
				default:
					throw new ArgumentOutOfRangeException("filter=" + filter);

			}

			return dataFactory.FillTable(query.ToString()).ToTableDefinitionList();
		}

		#region IDatabaseManager Members

		public crudwork.Models.DataAccess.ServiceDefinitionList GetServices()
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.DatabaseDefinitionList GetDatabases(bool showSystemDatabase)
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.TableDefinitionList GetTables(string tableName, QueryFilter tableFilter)
		{
			return QueryMaster(ObjectType.Table, tableName, tableFilter);
		}

		public crudwork.Models.DataAccess.TableDefinitionList GetTables()
		{
			return QueryMaster(ObjectType.Table, null, QueryFilter.None);
		}

		public crudwork.Models.DataAccess.TableDefinitionList GetObjects(ObjectType objectType)
		{
			return QueryMaster(objectType, null, QueryFilter.None);
		}

		public crudwork.Models.DataAccess.ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.ColumnDefinitionList GetColumns()
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.TableDependencyList GetTableDependencyList()
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.TableDependencyList GetTableDependencyList(string tableName)
		{
			throw new NotImplementedException();
		}

		public crudwork.Models.DataAccess.PrimaryColumnDefinitionList GetPrimaryKeys(string tableName)
		{
			throw new NotImplementedException();
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
			return QueryMaster(ObjectType.Index, null, QueryFilter.None);
		}

		/// <summary>
		/// Retrieve a index listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="indexFilter"></param>
		/// <returns></returns>
		public TableDefinitionList GetIndexes(string indexName, QueryFilter indexFilter)
		{
			return QueryMaster(ObjectType.Index, indexName, indexFilter);
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
