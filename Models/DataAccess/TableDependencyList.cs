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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Definition of Table Column
	/// </summary>
	public class TableDependency
	{
		/// <summary>The catalog name</summary>
		public string PrimaryCatalog
		{
			get;
			set;
		}
		/// <summary>The schema or owner name</summary>
		public string PrimarySchema
		{
			get;
			set;
		}
		/// <summary>The table name</summary>
		public string PrimaryTableName
		{
			get;
			set;
		}
		/// <summary>The column name</summary>
		public string PrimaryColumnName
		{
			get;
			set;
		}

		/// <summary>The catalog name</summary>
		public string ForeignCatalog
		{
			get;
			set;
		}
		/// <summary>The schema or owner name</summary>
		public string ForeignSchema
		{
			get;
			set;
		}
		/// <summary>The table name</summary>
		public string ForeignTableName
		{
			get;
			set;
		}
		/// <summary>The column name</summary>
		public string ForeignColumnName
		{
			get;
			set;
		}

		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public TableDependency()
		{
		}
	}

	/// <summary>
	/// List of Primary / Foreign Tables Dependencies
	/// </summary>
	public class TableDependencyList : List<TableDependency>
	{
		/// <summary>
		/// Add a new entry to list
		/// </summary>
		/// <param name="priCatalog"></param>
		/// <param name="priSchema"></param>
		/// <param name="priTableName"></param>
		/// <param name="priColumnName"></param>
		/// <param name="forCatalog"></param>
		/// <param name="forSchema"></param>
		/// <param name="forTableName"></param>
		/// <param name="forColumnName"></param>
		public void Add(string priCatalog, string priSchema, string priTableName, string priColumnName,
			string forCatalog, string forSchema, string forTableName, string forColumnName)
		{
			this.Add(new TableDependency()
			{
				PrimaryCatalog = priCatalog,
				PrimarySchema = priSchema,
				PrimaryTableName = priTableName,
				PrimaryColumnName = priColumnName,
				ForeignCatalog = forCatalog,
				ForeignSchema = forSchema,
				ForeignTableName = forTableName,
				ForeignColumnName = forColumnName,
			});
		}
	}
}
