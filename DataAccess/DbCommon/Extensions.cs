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
using System.Data;
using crudwork.DynamicRuntime;
using crudwork.Utilities;
using System.Diagnostics;
using System.Reflection;

namespace crudwork.DataAccess.DbCommon
{
	/// <summary>
	/// Extensions for DataTable class
	/// </summary>
	public static class DataTableExtension
	{
		private static TList MapFieldToProperty<TList, TItem>(DataTable dt, FieldMapperList map)
		{
			try
			{
				var result = (TList)InstanceGenerator.Create(typeof(TList));

				foreach (DataRow dr in dt.Rows)
				{
					var innerItem = (TItem)InstanceGenerator.Create(typeof(TItem));

					#region Invoke property setter calls
					foreach (var item in map)
					{
						try
						{
							object value = dr[item.ColumnName];
							if (Convert.IsDBNull(value))
								value = null;
							DynamicCode.SetProperty(innerItem, item.PropertyName, value);
						}
						catch (Exception ex)
						{
							if (item.IsRequired)
							{
								// set breakpoint here to find out why something fails
								Debug.WriteLine(ex.Message);
								Debug.WriteLine(DebuggerTool.Dump(ex));
								throw;
							}
						}
					}
					#endregion

					#region Invoke the Add() method to add the inner item to list
					try
					{
						DynamicCode.InvokeMethod(result, "Add", innerItem);
					}
					catch (Exception ex)
					{
						// set breakpoint here to find out why something fails
						Debug.WriteLine(ex.Message);
						throw;
					}
					#endregion
				}

				return result;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt(XML)", DataUtil.DataTableToXml(dt));
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		private static BindingFlags bindingAttr = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		
		/// <summary>
		/// Convert the definition list to table
		/// </summary>
		/// <typeparam name="TList"></typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DataTable ToTable<TList, TItem>(TList obj)
			where TList : List<TItem>
		{
			DataTable dt = null;
			PropertyInfo[] piList = null;

			foreach (TItem item in obj)
			{
				if (dt == null)
				{
					piList = item.GetType().GetProperties(bindingAttr);
					dt = new DataTable("Definition");

					dt.Columns.Add("DefinitionUniqueID", typeof(int)).AutoIncrement = true;
					foreach (var pi in piList)
					{
						dt.Columns.Add(pi.Name, pi.PropertyType);
					}
				}

				var dr = dt.NewRow();
				foreach (var pi in piList)
				{
					dr[pi.Name] = DynamicCode.GetProperty(item, pi.Name);
				}
				dt.Rows.Add(dr);
			}

			return dt;
		}

		/// <summary>
		/// Convert DataTable to ServiceDefinitionList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static ServiceDefinitionList ToServiceDefinitionList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("ServiceName", "ServerName");
			map.Add("InstanceName", "InstanceName");
			map.Add("IsClustered", "IsClustered");
			map.Add("Version", "Version");
			map.Add("FactoryName", "FactoryName", false);		// where did FactoryName comes from??????
			#endregion
			return MapFieldToProperty<ServiceDefinitionList, ServiceDefinition>(dt, map);
		}

		/// <summary>
		/// Convert DataTable to DatabaseDefinitionList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DatabaseDefinitionList ToDatabaseDefinitionList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("Name", "NAME");
			map.Add("DatabaseId", "DBID");
			map.Add("ServerId", "SID");
			map.Add("Mode", "MODE");
			map.Add("Status", "STATUS");
			map.Add("Status2", "STATUS2");
			map.Add("CreationDate", "CRDATE");
			map.Add("Reserved", "RESERVED");
			map.Add("Category", "CATEGORY");
			map.Add("CompatibilityLevel", "CMPTLEVEL");
			map.Add("Filename", "FILENAME");
			map.Add("Version", "VERSION");

			#endregion
			return MapFieldToProperty<DatabaseDefinitionList, DatabaseDefinition>(dt, map);
		}

		/// <summary>
		/// Convert DataTable to TableDefinitionList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static TableDefinitionList ToTableDefinitionList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("Catalog", "TABLE_CATALOG");
			map.Add("Schema", "TABLE_SCHEMA");
			map.Add("TableName", "TABLE_NAME");
			map.Add("TableType", "TABLE_TYPE");

			// for Access database
			map.Add("TableGuid", "TABLE_GUID", false);
			map.Add("Description", "DESCRIPTION", false);
			map.Add("PropertyId", "TABLE_PROPID", false);
			map.Add("Created", "DATE_CREATED", false);
			map.Add("Modified", "DATE_MODIFIED", false);
			#endregion
			return MapFieldToProperty<TableDefinitionList, TableDefinition>(dt, map);
		}

		/// <summary>
		/// Convert DataTable to ColumnDefinitionList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static ColumnDefinitionList ToColumnDefinitionList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("Catalog", "TABLE_CATALOG");
			map.Add("Schema", "TABLE_SCHEMA");
			map.Add("TableName", "TABLE_NAME");
			map.Add("ColumnName", "COLUMN_NAME");
			map.Add("Position", "ORDINAL_POSITION");
			map.Add("DefaultValue", "COLUMN_DEFAULT");
			map.Add("IsNullable", "IS_NULLABLE");
			map.Add("DataType", "DATA_TYPE");
			map.Add("CharMaxLength", "CHARACTER_MAXIMUM_LENGTH");
			map.Add("CharOctetLength", "CHARACTER_OCTET_LENGTH");
			map.Add("NumPrecision", "NUMERIC_PRECISION");
			map.Add("NumPrecisionRadix", "NUMERIC_PRECISION_RADIX");
			map.Add("NumScale", "NUMERIC_SCALE");
			map.Add("DatePrecision", "DATETIME_PRECISION");
			map.Add("CharSetCatalog", "CHARACTER_SET_CATALOG");
			map.Add("CharSetSchema", "CHARACTER_SET_SCHEMA");
			map.Add("CharSetName", "CHARACTER_SET_NAME");
			map.Add("CollationCatalog", "COLLATION_CATALOG");
			map.Add("CollationSchema", "COLLATION_SCHEMA");
			map.Add("CollationName", "COLLATION_NAME");
			map.Add("DomainCatalog", "DOMAIN_CATALOG");
			map.Add("DomainSchema", "DOMAIN_SCHEMA");
			map.Add("DomainName", "DOMAIN_NAME");

			// used by Access DB file
			map.Add("ColumnGuid", "COLUMN_GUID", false);
			map.Add("ColumnPropertyId", "COLUMN_PROPID", false);
			map.Add("HasDefault", "COLUMN_HASDEFAULT", false);
			map.Add("ColumnFlags", "COLUMN_FLAGS", false);
			map.Add("TypeGuid", "TYPE_GUID", false);
			map.Add("Description", "DESCRIPTION", false);
			#endregion
			return MapFieldToProperty<ColumnDefinitionList, ColumnDefinition>(dt, map);
		}

		/// <summary>
		/// Convert DataTable to TableDependencyList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static TableDependencyList ToTableDependencyList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("PrimaryCatalog", "P_TABLE_CATALOG");
			map.Add("PrimarySchema", "P_TABLE_SCHEMA");
			map.Add("PrimaryTableName", "P_TABLE_NAME");
			map.Add("PrimaryColumnName", "P_COLUMN_NAME");
			map.Add("ForeignCatalog", "F_TABLE_CATALOG");
			map.Add("ForeignSchema", "F_TABLE_SCHEMA");
			map.Add("ForeignTableName", "F_TABLE_NAME");
			map.Add("ForeignColumnName", "F_COLUMN_NAME");
			#endregion
			return MapFieldToProperty<TableDependencyList, TableDependency>(dt, map);
		}

		/// <summary>
		/// Convert DataTable to TableDependencyList object
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static PrimaryColumnDefinitionList ToPrimaryColumnDefinitionList(this DataTable dt)
		{
			var map = new FieldMapperList();
			#region Map Data Column to Property Name
			map.Add("TableName", "TABLE_NAME");
			map.Add("ColumnName", "COLUMN_NAME");
			map.Add("ConstraintName", "CONSTRAINT_NAME");
			map.Add("OrdinalPosition", "ORDINAL_POSITION");
			#endregion
			return MapFieldToProperty<PrimaryColumnDefinitionList, PrimaryColumnDefinition>(dt, map);
		}
	}
}
