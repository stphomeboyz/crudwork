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
using System.Data;
using System.Text;
using crudwork.DataAccess.Common;
using crudwork.Utilities;

namespace crudwork.DataAccess.SqlClient
{
	/// <summary>
	/// Import Manager
	/// </summary>
	internal class MacroManager : IMacroManager
	{
		#region Enumerators
		#endregion Enumerators

		#region Fields
		private static string crlf = Environment.NewLine;
		#endregion Fields

		#region Constructors
		#endregion Constructors

		#region Event methods

		#region System Event methods
		#endregion System Event methods

		#region Application Event methods
		#endregion Application Event methods

		#region Custom Event methods
		#endregion Custom Event methods

		#endregion Event methods

		#region Public methods
		#endregion Public methods

		#region Private methods

		private string OneDataValue(string value, DataColumn c)
		{
			switch (c.DataType.ToString().Replace("System.", ""))
			{
				case "String":
				case "DateTime":
				case "Guid":
					return String.Format("'{0}'", Accessories.DbQuote(value));

				case "Byte":
				case "Int8":
				case "Int16":
				case "Int32":
				case "Int64":
				case "Single":
				case "Double":
				case "Decimal":
					{
						string v = value;
						if (String.IsNullOrEmpty(v))
							v = "null";
						return String.Format("{0}", v);
					}

				case "Boolean":
					return DataConvert.IsNull(value) ? "null" : DataConvert.ToBoolean(value) ? "1" : "0";

				default:
					throw new ArgumentOutOfRangeException("unsupport type: " + c.ToString());
			}
		}

		/// <summary>
		/// Convert or remove any character that are not supported in Column name.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string MakeSafeColumnName(string value)
		{
			return "[" + value + "]";
		}

		private string OneDataColumn(DataColumn c)
		{
			string columnName = MakeSafeColumnName(c.ColumnName);
			Type type = c.DataType;

			switch (type.ToString().Replace("System.", ""))
			{
				case "String":
				case "Guid":
					{
						string len = (c.MaxLength == -1) ? "MAX" : Math.Min(c.MaxLength, 8000).ToString();
						return String.Format("{0} varchar({1})", columnName, len);
					}

				case "DateTime":
					return String.Format("{0} datetime", columnName);

				case "Byte":
				case "Int8":
					return String.Format("{0} tinyint", columnName);

				case "Int16":
					return String.Format("{0} smallint", columnName);

				case "Int32":
					return String.Format("{0} int", columnName);

				case "Int64":
					return String.Format("{0} bigint", columnName);

				case "Single":
				case "Double":
				case "Decimal":
					// TODO: Need to implement precision.
					return String.Format("{0} decimal(18,6)", columnName);

				case "Boolean":
					return String.Format("{0} bit", columnName);

				default:
					throw new ArgumentOutOfRangeException("unsupported type=" + type + " ColumnName=" + c.ColumnName);
			}
		}

		private string CreatePrimaryColumn(string[] primaryColumns, string tablename)
		{
			StringBuilder s = new StringBuilder();
			TableManager tc = new TableManager(tablename, false);

			for (int i = 0; i < primaryColumns.Length; i++)
			{
				if (i > 0)
					s.Append(", ");

				s.AppendFormat("{0} ASC", primaryColumns[i]);
			}

			return String.Format(@"
 CONSTRAINT [PK_{0}]
 PRIMARY KEY CLUSTERED({1})
 WITH (PAD_INDEX=OFF, IGNORE_DUP_KEY=OFF)
 ON [PRIMARY]", tc.Tablename, s.ToString());
		}

		#endregion Private methods

		#region Protected methods
		#endregion Protected methods

		#region Property methods
		#endregion Property methods

		#region Others
		#endregion Others

		#region IQueryGenerator Members

		/// <summary>
		/// Generate a CREATE TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string CreateTableStatement(string tablename, System.Data.DataTable dt)
		{
			StringBuilder result = new StringBuilder();

			string[] primaryColumns = Accessories.GetPrimaryColumns(dt);
			result.AppendFormat("create table [{1}] ({0}", crlf, tablename);

			for (int i = 0; i < dt.Columns.Count; i++)
			{
				if (i > 0)
				{
					result.Append(crlf + ",");
				}

				DataColumn c = dt.Columns[i];
				result.AppendFormat("{0}", OneDataColumn(c));
				if (StringUtil.Search(primaryColumns, c.ColumnName) >= 0)
					result.AppendFormat(" identity(1,1)", tablename, c.ColumnName);
			}

			// TODO: insert primary key constraint code here...
			if (primaryColumns.Length > 0)
			{
				result.Append(CreatePrimaryColumn(primaryColumns, tablename));
				result.Append(") ON [PRIMARY];");
			}
			else
			{
				result.AppendFormat(");");
			}

			return result.ToString();
		}

		/// <summary>
		/// Generate the INSERT statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="columns"></param>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		public string CreateInsertStatement(string tablename, System.Data.DataColumnCollection columns, System.Data.DataRow dataRow)
		{
			StringBuilder col = new StringBuilder();
			StringBuilder val = new StringBuilder();

			for (int i = 0; i < columns.Count; i++)
			{
				// ignore primary keys
				if (columns[i].AutoIncrement)
					continue;

				string columnName = columns[i].ColumnName;
				string value = dataRow[columnName].ToString();

				if (i > 0)
				{
					col.Append(",");
					val.Append(",");
				}

				col.Append(MakeSafeColumnName(columnName));
				val.Append(OneDataValue(value, columns[columnName]));
			}

			return String.Format("insert into [{0}] ({1}) values ({2});", tablename, col.ToString(), val.ToString());
		}

		/// <summary>
		/// Generate the DROP TABLE statement
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string DropTableStatement(string tablename)
		{
			TableManager tc = new TableManager(tablename, false);

			return String.Format(@"
				IF EXISTS (
				  SELECT *
					FROM sys.tables
					JOIN sys.schemas
					  ON sys.tables.schema_id = sys.schemas.schema_id
				   WHERE sys.schemas.name = N'{0}'
					 AND sys.tables.name = N'{1}'
				)
				  DROP TABLE [{0}].[{1}];", tc.Owner, tc.Tablename);
		}

		#endregion IQueryGenerator Members

		#region IQueryGenerator Members

		string IMacroManager.CreateTableStatement(string tablename, System.Data.DataTable dt)
		{
			return CreateTableStatement(tablename, dt);
		}

		string IMacroManager.CreateInsertStatement(string tablename, System.Data.DataColumnCollection columns, System.Data.DataRow dataRow)
		{
			return CreateInsertStatement(tablename, columns, dataRow);
		}

		string IMacroManager.DropTableStatement(string tablename)
		{
			return DropTableStatement(tablename);
		}

		#endregion IQueryGenerator Members

		#region IDataImport Members

		public string CreateIndexStatement(string tablename, string columnname, string indexname)
		{
			return string.Format("CREATE INDEX {0} ON {1}({2})", indexname, tablename, columnname);
		}

		public string DropIndexStatement(string tablename, string indexname)
		{
			return string.Format("DROP INDEX {0} ON {1}", indexname, tablename);
		}

		#endregion IDataImport Members

		#region IDataImport Members

		string IMacroManager.CreateIndexStatement(string tablename, string columnname, string indexname)
		{
			return CreateIndexStatement(tablename, columnname, indexname);
		}

		string IMacroManager.DropIndexStatement(string tablename, string indexname)
		{
			return DropIndexStatement(tablename, indexname);
		}

		#endregion IDataImport Members

		#region CopyTable Members

		/// <summary>
		/// Generate a COPY TABLE statement
		/// </summary>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public string CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("select {2} into {0} from {1}", inputTable, outputTable, selectClause);
			if (!string.IsNullOrEmpty(whereClause))
				sb.Append(" where " + whereClause);
			return sb.ToString();
		}

		string IMacroManager.CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			return CopyTable(inputTable, outputTable, selectClause, whereClause);
		}

		#endregion CopyTable Members
	}
}