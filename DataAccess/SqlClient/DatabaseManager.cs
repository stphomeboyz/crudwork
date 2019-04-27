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
using crudwork.DataAccess.Common;
using System.Data.Common;
using crudwork.Models.DataAccess;
using crudwork.DataAccess.DbCommon;

namespace crudwork.DataAccess.SqlClient
{
	/// <summary>
	/// SQL Database Manager
	/// </summary>
	internal class DatabaseManager : IDatabaseManager
	{
		#region Enumerators
		#endregion

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
			DbDataSourceEnumerator e = dataFactory.ProviderFactory.CreateDataSourceEnumerator();

			/*
			 * ServerName
			 * InstanceName
			 * IsClustered
			 * Version
			 * FactoryName
			 * 
			 */
			return e.GetDataSources().ToServiceDefinitionList();
		}
		/// <summary>
		/// Retrieve a database listing.
		/// </summary>
		/// <returns></returns>
		public DatabaseDefinitionList GetDatabases(bool showSystemDatabase)
		{
			/*
			 * name, dbid, sid, mode, status, status2, crdate, reserved, category, cmptlevel, filename, version
			 * */

			string query = "select * from master..sysdatabases (nolock)";

			if (!showSystemDatabase)
			{
				query = query + " where sid <> 1";
			}

			query = query + " order by name";
			return dataFactory.FillTable(query).ToDatabaseDefinitionList();
		}
		/// <summary>
		/// Retrieve a table listing for a given tableName and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables(string tableName, QueryFilter tableFilter)
		{
			//string query = "select table_catalog, table_Schema, table_name, '[' + table_catalog + '].[' + table_Schema + '].[' + table_name + ']' as fullTableName from information_schema.tables (nolock)";
			string query = "select * from information_schema.tables (nolock)";
			string condition = Accessories.CreateQueryFilter(tableName, tableFilter, "table_name");

			if (!String.IsNullOrEmpty(condition))
				query = query + " where " + condition;

			query = query + "  order by 1,2,3";
			return dataFactory.FillTable(query).ToTableDefinitionList();
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetTables()
		{
			return GetTables(string.Empty, QueryFilter.None);
		}
		/// <summary>
		/// Retrieve a table listing.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetObjects(ObjectType objectType)
		{
			return GetObjects(string.Empty, QueryFilter.None, objectType);
		}
		/// <summary>
		/// Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public TableDefinitionList GetObjects(string objectName, QueryFilter queryFilter, ObjectType objectType)
		{
			StringBuilder query = new StringBuilder();

			switch (objectType)
			{
				case ObjectType.StoredProcedure:
					query.Append("select routine_catalog as CatalogName, routine_schema as SchemaName, routine_name as ObjectName, '[' + routine_catalog + '].[' + routine_schema + '].[' + routine_name + ']' as FullName from information_schema.routines (nolock)");
					break;

				case ObjectType.Function:
					query.Append("select routine_catalog as CatalogName, routine_schema as SchemaName, routine_name as ObjectName, '[' + routine_catalog + '].[' + routine_schema + '].[' + routine_name + ']' as FullName from information_schema.routines (nolock)");
					break;

				case ObjectType.Table:
					query.Append("select table_catalog as CatalogName, table_schema as SchemaName, table_name as ObjectName, '[' + table_catalog + '].[' + table_schema + '].[' + table_name + ']' as FullName from information_schema.tables (nolock)");
					break;

				case ObjectType.View:
					query.Append("select table_catalog as CatalogName, table_schema as SchemaName, table_name as ObjectName, '[' + table_catalog + '].[' + table_schema + '].[' + table_name + ']' as FullName  from information_schema.views (nolock)");
					break;

				case ObjectType.Index:
					//query.Append("select CatalogName, SchemaName, ObjectName, FullName from (select constraint_catalog as CatalogName, constraint_schema as SchemaName, constraint_name as ObjectName, '[' + constraint_catalog + '].[' + constraint_schema + '].[' + constraint_name + ']' as FullName from information_schema.table_constraints (nolock)) x");
					query.Append(
@"select CatalogName, SchemaName, ObjectName, FullName
	from (
		SELECT
			db_name() as CatalogName,
			SCHEMA_NAME(tbl.schema_id) as SchemaName,
			i.name AS ObjectName,
			'[' + db_name() + '].[' + SCHEMA_NAME(tbl.schema_id) + '].[' + i.name + ']' as FullName
		--	'Server[@Name=' + quotename(CAST(serverproperty(N'Servername') AS sysname),'''') + ']' + '/Database[@Name=' + quotename(db_name(),'''') + ']' + '/Table[@Name=' + quotename(tbl.name,'''') + ' and @Schema=' + quotename(SCHEMA_NAME(tbl.schema_id),'''') + ']' + '/Index[@Name=' + quotename(i.name,'''') + ']' AS [Urn],
		--	CAST(CASE i.index_id WHEN 1 THEN 1 ELSE 0 END AS bit) AS [IsClustered],
		--	i.is_unique AS [IsUnique],
		--	CAST(case when i.type=3 then 1 else 0 end AS bit) AS [IsXmlIndex],
		--	case UPPER(ISNULL(xi.secondary_type,'')) when 'P' then 1 when 'V' then 2 when 'R' then 3 else 0 end AS [SecondaryXmlIndexType]
		FROM
			sys.tables AS tbl
			INNER JOIN sys.indexes AS i ON (i.index_id > 0 and i.is_hypothetical = 0) AND (i.object_id=tbl.object_id)
			LEFT OUTER JOIN sys.xml_indexes AS xi ON xi.object_id = i.object_id AND xi.index_id = i.index_id
		--WHERE
		--	tbl.name = 'ClientRulesetRule'
		--	(tbl.name=@_msparam_2 and SCHEMA_NAME(tbl.schema_id)=@_msparam_3)
		) x");
					break;

				default:
					throw new ArgumentOutOfRangeException("objectType=" + objectType);
			}

			string condition = Accessories.CreateQueryFilter(objectName, queryFilter, "ObjectName");

			if (!String.IsNullOrEmpty(condition))
				query.Append(" where " + condition);

			switch (objectType)
			{
				case ObjectType.Function:
					query.AppendFormat("{0} routine_type='FUNCTION'", String.IsNullOrEmpty(condition) ? " where " : " and ");
					break;

				case ObjectType.StoredProcedure:
					query.AppendFormat("{0} routine_type='PROCEDURE'", String.IsNullOrEmpty(condition) ? " where " : " and ");
					break;
			}

			query.Append(" order by 1,2,3");
			return dataFactory.FillTable(query.ToString()).ToTableDefinitionList();
		}
		/// <summary>
		/// Retrieve a columns listing for all tables.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns(string tableName, QueryFilter tableFilter, string columnName, QueryFilter columnFilter)
		{
			StringBuilder query = new StringBuilder("select * from information_schema.columns (nolock)");

			string[] conditions = new string[] {
				Accessories.CreateQueryFilter(tableName, tableFilter, "table_name"),
				Accessories.CreateQueryFilter(columnName, columnFilter, "column_name"),
			};

			int len = 0;
			for (int i = 0; i < conditions.Length; i++)
			{
				len += conditions[i].Length;
			}

			if (len > 0)
			{
				int c = 0;

				for (int i = 0; i < conditions.Length; i++)
				{
					if (String.IsNullOrEmpty(conditions[i]))
						continue;

					if (c == 0)
						query.Append(" where ");
					else
						query.Append(" and ");

					c++;

					query.Append(conditions[i]);
				}
			}

			return dataFactory.FillTable(query.ToString()).ToColumnDefinitionList();
		}
		/// <summary>
		/// Retrieve a columns listing for a given table, column and query filter criteria.
		/// </summary>
		/// <returns></returns>
		public ColumnDefinitionList GetColumns()
		{
			return GetColumns(string.Empty, QueryFilter.None, string.Empty, QueryFilter.None);
		}
		/// <summary>
		/// Get the primary key(s) for the given table name
		/// </summary>
		/// <returns></returns>
		public PrimaryColumnDefinitionList GetPrimaryColumns(string tableName)
		{
			string query = string.Format(@"SELECT
    T.TABLE_NAME,
    T.CONSTRAINT_NAME,
    K.COLUMN_NAME,
    K.ORDINAL_POSITION
FROM
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS T
    INNER JOIN
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE K
    ON T.CONSTRAINT_NAME = K.CONSTRAINT_NAME
WHERE
    T.CONSTRAINT_TYPE = 'PRIMARY KEY'
    AND T.TABLE_NAME = '{0}'
ORDER BY
    T.TABLE_NAME,
    K.ORDINAL_POSITION", tableName);
			return dataFactory.FillTable(query).ToPrimaryColumnDefinitionList();
		}
		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList()
		{
			return GetTableDependencyList(string.Empty);
		}

		/// <summary>
		/// Retrieve a parent-child relation between tables.
		/// </summary>
		/// <returns></returns>
		public TableDependencyList GetTableDependencyList(string tableName)
		{
			StringBuilder query = new StringBuilder(@"
select
	 pk.TABLE_CATALOG					as P_TABLE_CATALOG
	,pk.TABLE_SCHEMA					as P_TABLE_SCHEMA
	,pk.TABLE_NAME						as P_TABLE_NAME
	,pk_kcu.COLUMN_NAME					as P_COLUMN_NAME
	--,'[' + pk.TABLE_CATALOG + '].' +
	-- '[' + pk.TABLE_SCHEMA + '].' +
	-- '[' + pk.TABLE_NAME + ']'			as pkFullTable


	,fk.TABLE_CATALOG					as F_TABLE_CATALOG
	,fk.TABLE_SCHEMA					as F_TABLE_SCHEMA
	,fk.TABLE_NAME						as F_TABLE_NAME
	,fk_kcu.COLUMN_NAME					as F_COLUMN_NAME
	--,'[' + fk.TABLE_CATALOG + '].' +
	-- '[' + fk.TABLE_SCHEMA + '].' +
	-- '[' + fk.TABLE_NAME + ']'			as fkFullTable
from
	INFORMATION_SCHEMA.TABLE_CONSTRAINTS as pk
	join INFORMATION_SCHEMA.KEY_COLUMN_USAGE as pk_kcu
		on (pk.TABLE_SCHEMA = pk_kcu.TABLE_SCHEMA
			and pk.TABLE_NAME = pk_kcu.TABLE_NAME
			and pk.CONSTRAINT_NAME = pk_kcu.CONSTRAINT_NAME)
	join INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as rc
		on (pk.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME)
	join INFORMATION_SCHEMA.TABLE_CONSTRAINTS as fk
		on (fk.CONSTRAINT_NAME = rc.CONSTRAINT_NAME)
	join INFORMATION_SCHEMA.KEY_COLUMN_USAGE as fk_kcu
		on (fk.TABLE_SCHEMA = fk_kcu.TABLE_SCHEMA
			and fk.TABLE_NAME = fk_kcu.TABLE_NAME
			and fk.CONSTRAINT_NAME = fk_kcu.CONSTRAINT_NAME)
where
	pk.CONSTRAINT_TYPE = 'PRIMARY KEY'
	and pk_kcu.ORDINAL_POSITION = fk_kcu.ORDINAL_POSITION
	{0}
order by 
	--pk_kcu.ORDINAL_POSITION
	1,2,3,4
");

			if (String.IsNullOrEmpty(tableName))
			{
				query.Replace("{0}", "");
			}
			else
			{
				query.Replace("{0}", "and pk.TABLE_NAME = '" + tableName + "'");
			}

			return dataFactory.FillTable(query.ToString()).ToTableDependencyList();
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
			return GetObjects(ObjectType.Index);
		}

		/// <summary>
		/// Retrieve a index listing for a given tableName and query filter criteria.
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="indexFilter"></param>
		/// <returns></returns>
		public TableDefinitionList GetIndexes(string indexName, QueryFilter indexFilter)
		{
			return GetObjects(indexName, indexFilter, ObjectType.Index);
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
