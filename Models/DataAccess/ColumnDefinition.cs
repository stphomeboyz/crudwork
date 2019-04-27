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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Definition for a database column
	/// </summary>
	public class ColumnDefinition
	{
		#region Fields/Properties
		/// <summary>Catalog</summary>
		public string Catalog
		{
			get;
			set;
		}
		/// <summary>xxxxxxxxxxxxxx</summary>
		public string Schema
		{
			get;
			set;
		}
		/// <summary>TableName</summary>
		public string TableName
		{
			get;
			set;
		}
		/// <summary>ColumnName</summary>
		public string ColumnName
		{
			get;
			set;
		}
		/// <summary>Position</summary>
		public int Position
		{
			get;
			set;
		}
		/// <summary>DefaultValue</summary>
		public string DefaultValue
		{
			get;
			set;
		}
		/// <summary>IsNullable</summary>
		public string IsNullable
		{
			get;
			set;
		}
		/// <summary>DataType</summary>
		public string DataType
		{
			get;
			set;
		}
		/// <summary>CharMaxLength</summary>
		public int? CharMaxLength
		{
			get;
			set;
		}
		/// <summary>CharOctetLength</summary>
		public int? CharOctetLength
		{
			get;
			set;
		}
		/// <summary>NumPrecision</summary>
		public int? NumPrecision
		{
			get;
			set;
		}
		/// <summary>NumPrecisionRadix</summary>
		public int? NumPrecisionRadix
		{
			get;
			set;
		}
		/// <summary>NumScale</summary>
		public int? NumScale
		{
			get;
			set;
		}
		/// <summary>DatePrecision</summary>
		public int? DatePrecision
		{
			get;
			set;
		}
		/// <summary>CharSetCatalog</summary>
		public string CharSetCatalog
		{
			get;
			set;
		}
		/// <summary>CharSetSchema</summary>
		public string CharSetSchema
		{
			get;
			set;
		}
		/// <summary>CharSetName</summary>
		public string CharSetName
		{
			get;
			set;
		}
		/// <summary>CollationCatalog</summary>
		public string CollationCatalog
		{
			get;
			set;
		}
		/// <summary>CollationSchema</summary>
		public string CollationSchema
		{
			get;
			set;
		}
		/// <summary>CollationName</summary>
		public string CollationName
		{
			get;
			set;
		}
		/// <summary>DomainCatalog</summary>
		public string DomainCatalog
		{
			get;
			set;
		}
		/// <summary>DomainSchema</summary>
		public string DomainSchema
		{
			get;
			set;
		}
		/// <summary>DomainName</summary>
		public string DomainName
		{
			get;
			set;
		}

		#region Used by Access Database
		/// <summary>ColumnGuid</summary>
		public Guid ColumnGuid
		{
			get;
			set;
		}
		/// <summary>ColumnPropertyId</summary>
		public Int64 ColumnPropertyId
		{
			get;
			set;
		}
		/// <summary>HasDefault</summary>
		public bool HasDefault
		{
			get;
			set;
		}
		/// <summary>ColumnFlags</summary>
		public Int64 ColumnFlags
		{
			get;
			set;
		}
		/// <summary>TypeGuid</summary>
		public Guid TypeGuid
		{
			get;
			set;
		}
		/// <summary>Description</summary>
		public string Description
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public ColumnDefinition()
		{
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("catalog={0} schema={1} tableName={2} columnName={3} position={4} defaultValue={5} isNullable={6} dataType={7} charMaxLength={8} charOctetLength={9} numPrecision={10} numPrecisionRadix={11} numScale={12} datePrecision={13} charSetCatalog={14} charSetSchema={15} charSetName={16} collationCatalog={17} collationSchema={18} collationName={19} domainCatalog={20} domainSchema={21} domainName={22}",
				Catalog, Schema, TableName, ColumnName, Position, DefaultValue, IsNullable, DataType, CharMaxLength, CharOctetLength, NumPrecision, NumPrecisionRadix, NumScale, DatePrecision, CharSetCatalog, CharSetSchema, CharSetName, CollationCatalog, CollationSchema, CollationName, DomainCatalog, DomainSchema, DomainName);
		}
	}

	/// <summary>
	/// List of Column definition
	/// </summary>
	public class ColumnDefinitionList : List<ColumnDefinition>
	{
		/// <summary>
		/// Add a new entry to list
		/// </summary>
		/// <param name="catalog"></param>
		/// <param name="schema"></param>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		/// <param name="position"></param>
		/// <param name="defaultValue"></param>
		/// <param name="isNullable"></param>
		/// <param name="dataType"></param>
		/// <param name="charMaxLength"></param>
		/// <param name="charOctetLength"></param>
		/// <param name="numPrecision"></param>
		/// <param name="numPrecisionRadix"></param>
		/// <param name="numScale"></param>
		/// <param name="datePrecision"></param>
		/// <param name="charSetCatalog"></param>
		/// <param name="charSetSchema"></param>
		/// <param name="charSetName"></param>
		/// <param name="collationCatalog"></param>
		/// <param name="collationSchema"></param>
		/// <param name="collationName"></param>
		/// <param name="domainCatalog"></param>
		/// <param name="domainSchema"></param>
		/// <param name="domainName"></param>
		public void Add(
			string catalog, string schema, string tableName, string columnName,
			int position, string defaultValue, string isNullable, string dataType,
			int? charMaxLength, int? charOctetLength,
			int? numPrecision, int? numPrecisionRadix, int? numScale,
			int datePrecision,
			string charSetCatalog, string charSetSchema, string charSetName,
			string collationCatalog, string collationSchema, string collationName,
			string domainCatalog, string domainSchema, string domainName)
		{
			this.Add(new ColumnDefinition()
			{
				Catalog = catalog,
				Schema = schema,
				TableName = tableName,
				ColumnName = columnName,
				Position = position,
				DefaultValue = defaultValue,
				IsNullable = isNullable,
				DataType = dataType,
				CharMaxLength = charMaxLength,
				CharOctetLength = charOctetLength,
				NumPrecision = numPrecision,
				NumPrecisionRadix = numPrecisionRadix,
				NumScale = numScale,
				DatePrecision = datePrecision,
				CharSetCatalog = charSetCatalog,
				CharSetSchema = charSetSchema,
				CharSetName = charSetName,
				CollationCatalog = collationCatalog,
				CollationSchema = collationSchema,
				CollationName = collationName,
				DomainCatalog = domainCatalog,
				DomainSchema = domainSchema,
				DomainName = domainName,
			});
		}
	}
}
