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
	internal interface IDatabaseManager
	{
		/// <summary>
		/// Retrieve a service listing.
		/// </summary>
		/// <returns></returns>
		ServiceDefinitionList GetServices();

		/// <summary>
		/// Retrieve a database listing.
		/// </summary>
		/// <returns></returns>
		DatabaseDefinitionList GetDatabases(bool showSystemDatabase);

		/// <summary>
		/// Retrieve a table listing for a given tableName and query filter criteria.
		/// </summary>
		/// <returns></returns>
		TableDefinitionList GetTables(string tableName, QueryFilter tableFilter);

		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		TableDefinitionList GetTables();

		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		TableDefinitionList GetObjects(ObjectType objectType);

		/// <summary>
		/// Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <returns></returns>
		ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter);

		/// <summary>
		/// Retrieve a columns listing for all tables.
		/// </summary>
		/// <returns></returns>
		ColumnDefinitionList GetColumns();

		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		TableDependencyList GetTableDependencyList();

		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		TableDependencyList GetTableDependencyList(string tableName);

		/// <summary>
		/// Get the primary key(s) for the given table name
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		PrimaryColumnDefinitionList GetPrimaryKeys(string tableName);

		/// <summary>
		/// Retrieve a list of indexes
		/// </summary>
		/// <returns></returns>
		TableDefinitionList GetIndexes();

		/// <summary>
		/// Retrieve a index listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="indexFilter"></param>
		/// <returns></returns>
		TableDefinitionList GetIndexes(string indexName, QueryFilter indexFilter);
	}
}
