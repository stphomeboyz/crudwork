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
using crudwork.Models.DataAccess;

namespace crudwork.DataAccess.OracleClient
{
	/// <summary>
	/// Oracle database manager
	/// </summary>
	internal class DatabaseManager : IDatabaseManager
	{
		/// <summary>
		/// create new instance with given attribute
		/// </summary>
		/// <param name="dataFactory"></param>
		public DatabaseManager(DataFactory dataFactory)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#region IDatabaseSchema Members
		/// <summary>
		/// Retrieve a service listing.
		/// </summary>
		/// <returns></returns>
		public ServiceDefinitionList GetServices()
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a database listing.
		/// </summary>
		/// <returns></returns>
		public DatabaseDefinitionList GetDatabases(bool showSystemDatabase)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a table listing for a given tableName and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables(string tableName, QueryFilter tableFilter)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables()
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetObjects(ObjectType objectType)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Retrieve a columns listing for all tables.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns()
		{
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// Get the primary key(s) for the given table name
		/// </summary>
		/// <returns></returns>
		public PrimaryColumnDefinitionList GetPrimaryColumns(string tableName)
		{
			throw new Exception("The method or operation is not implemented.");
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
