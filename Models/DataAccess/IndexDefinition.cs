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
	/// Definition for a database table
	/// </summary>
	public class IndexDefinition
	{
		#region Properties
		/// <summary>The catalog name</summary>
		public string Catalog
		{
			get;
			set;
		}
		/// <summary>The schema or owner name</summary>
		public string Schema
		{
			get;
			set;
		}
		/// <summary>The table name</summary>
		public string TableName
		{
			get;
			set;
		}
		/// <summary>The type of table</summary>
		public string TableType
		{
			get;
			set;
		}

		#region Used By Access Database
		/// <summary>Unique Guid</summary>
		public Guid TableGuid
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
		/// <summary>PropertyID</summary>
		public object PropertyId
		{
			get;
			set;
		}
		/// <summary>Creation Date</summary>
		public DateTime Created
		{
			get;
			set;
		}
		/// <summary>Modification date</summary>
		public DateTime Modified
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public IndexDefinition()
		{
		}

		/// <summary>
		/// Return a fully qualified table name, optionally with the surrounding quotes.  Specify one quote
		/// to use it as both BEGIN and END quotes.  Or specify two: one for the BEGIN quote and second for the END quote.
		/// </summary>
		/// <param name="quotes">set 1 or 2 elements; set null (or empty) for no quotes</param>
		/// <returns></returns>
		public string FullName(params char[] quotes)
		{
			char beginQuote, endQuote;

			if (quotes == null || quotes.Length == 0)
			{
				beginQuote = endQuote = '\0';
			}
			else if (quotes.Length == 1)
			{
				beginQuote = endQuote = quotes[0];
			}
			else if (quotes.Length == 2)
			{
				beginQuote = quotes[0];
				endQuote = quotes[1];
			}
			else
			{
				throw new ArgumentException("Too many quotes specified.  Expected 0, 1, or 2 quotes only.");
			}

			return string.Format("{0}{2}{1}.{0}{3}{1}.{0}{4}{1}.{0}{5}{1}",
				beginQuote, endQuote,
				Catalog, Schema, Schema, TableName);
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FullName('[', ']');
		}
	}

	/// <summary>
	/// List of Table definition
	/// </summary>
	public class IndexDefinitionList : List<IndexDefinition>
	{
		/// <summary>
		/// Add a new entry to the list
		/// </summary>
		/// <param name="catalog"></param>
		/// <param name="schema"></param>
		/// <param name="tableName"></param>
		/// <param name="tableType"></param>
		public void Add(string catalog, string schema, string tableName, string tableType)
		{
			this.Add(new IndexDefinition()
			{
				Catalog = catalog,
				Schema = schema,
				TableName = tableName,
				TableType = tableType,
			});
		}

		/// <summary>
		/// Add a new entry to list
		/// </summary>
		/// <param name="catalog"></param>
		/// <param name="schema"></param>
		/// <param name="tableName"></param>
		/// <param name="tableType"></param>
		/// <param name="guid"></param>
		/// <param name="description"></param>
		/// <param name="propertyId"></param>
		/// <param name="created"></param>
		/// <param name="modified"></param>
		public void Add(string catalog, string schema, string tableName, string tableType,
			Guid guid, string description, object propertyId, DateTime created, DateTime modified)
		{
			this.Add(new IndexDefinition()
			{
				Catalog = catalog,
				Schema = schema,
				TableName = tableName,
				TableType = tableType,
				TableGuid = guid,
				Description = description,
				PropertyId = propertyId,
				Created = created,
				Modified = modified,
			});
		}
	}
}
