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
using System.IO;
using System.Xml;

namespace crudwork.Utilities
{
	#region DataRelationHelper class
	/// <summary>
	/// Helper class for creating DataRelation
	/// </summary>
	public class DataRelationHelper
	{
		/// <summary>
		/// the parent's table name
		/// </summary>
		public string ParentTable;

		/// <summary>
		/// the parent's column name
		/// </summary>
		public string ParentColumn;

		/// <summary>
		/// the child's table name
		/// </summary>
		public string ChildTable;

		/// <summary>
		/// the child's column name
		/// </summary>
		public string ChildColumn;

		/// <summary>
		/// create nested DataRelation
		/// </summary>
		public bool Nested;
		private string relationName = string.Empty;

		/// <summary>
		/// Create a new object with the given relation
		/// </summary>
		/// <param name="relationCSVString"></param>
		/// <param name="nested"></param>
		public DataRelationHelper(string relationCSVString, bool nested)
		{
			string[] tokens = relationCSVString.Split(',', '.');
			if (tokens.Length != 4)
				throw new ArgumentOutOfRangeException("Expected four tokens; but, found " + tokens.Length);

			SetElement(tokens[0], tokens[1], tokens[2], tokens[3], nested);
		}

		/// <summary>
		/// Create a new object with the given relation elements
		/// </summary>
		/// <param name="parentTable"></param>
		/// <param name="parentColumn"></param>
		/// <param name="childTable"></param>
		/// <param name="childColumn"></param>
		/// <param name="nested"></param>
		public DataRelationHelper(string parentTable, string parentColumn, string childTable, string childColumn, bool nested)
		{
			SetElement(parentTable, parentColumn, childTable, childColumn, nested);
		}

		/// <summary>
		/// Return a string represent this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			base.ToString();
			return RelationName;
		}

		/// <summary>
		/// Set the elements
		/// </summary>
		/// <param name="parentTable"></param>
		/// <param name="parentColumn"></param>
		/// <param name="childTable"></param>
		/// <param name="childColumn"></param>
		/// <param name="nested"></param>
		public void SetElement(string parentTable, string parentColumn, string childTable, string childColumn, bool nested)
		{
			this.ParentTable = parentTable.Trim();
			this.ParentColumn = parentColumn.Trim();
			this.ChildTable = childTable.Trim();
			this.ChildColumn = childColumn.Trim();
			this.Nested = nested;
		}

		/// <summary>
		/// Get or set the relation name
		/// </summary>
		public string RelationName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.relationName))
					return this.relationName;
				else
					return string.Format("{0}_{1}_{2}_{3}",
						this.ParentTable,
						this.ParentColumn,
						this.ChildTable,
						this.ChildColumn
						);
			}
			set
			{
				this.relationName = value;
			}
		}

		/// <summary>
		/// Create DataRelation using the DataSet
		/// </summary>
		/// <param name="ds"></param>
		public void SetRelation(DataSet ds)
		{
			try
			{
				if (ds == null)
					throw new ArgumentNullException("ds");

				DataTable dtParent = ds.Tables[this.ParentTable];
				DataTable dtChild = ds.Tables[this.ChildTable];

				if (dtParent == null)
					throw new ArgumentNullException("dtParent");
				if (dtChild == null)
					throw new ArgumentNullException("dtChild");

				DataColumn dcParent = dtParent.Columns[this.ParentColumn];
				DataColumn dcChild = dtChild.Columns[this.ChildColumn];

				if (dcParent == null)
					throw new ArgumentNullException("dcParent");
				if (dcChild == null)
					throw new ArgumentNullException("dcChild");

				DataRelation dr = new DataRelation(RelationName, dcParent, dcChild);
				dr.Nested = this.Nested;
				ds.Relations.Add(dr);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "ds", DebuggerTool.Dump(ds));
				throw;
			}
		}
	}
	#endregion

	/// <summary>
	/// Data Utility
	/// </summary>
	public static class DataUtil
	{
		/// <summary>
		/// Apply nullification to values of given DataRow
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="useNull"></param>
		public static void NullifyValues(DataRow dr, bool useNull)
		{
			DataTable dt = dr.Table;

			for (int i = 0; i < dt.Columns.Count; i++)
			{
				string columnName = dt.Columns[i].ColumnName;
				object obj = dr[columnName];
				NullableObject nullableObject = obj as NullableObject ?? new NullableObject(obj);
				dr[columnName] = (useNull) ? nullableObject.value : nullableObject;
			}

		}

		/// <summary>
		/// Apply nullification to values of given DataTable
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="useNull"></param>
		public static void NullifyValues(DataTable dt, bool useNull)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				NullifyValues(dr, useNull);
			}
		}

		/// <summary>
		/// Apply nullification to values of given DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="useNull"></param>
		public static void NullifyValues(DataSet ds, bool useNull)
		{
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dt = ds.Tables[i];
				NullifyValues(dt, useNull);
			}
		}

		/// <summary>
		/// Apply nullification to values of given DataRow
		/// </summary>
		/// <param name="dr"></param>
		public static void NullifyValues(DataRow dr)
		{
			NullifyValues(dr, true);
		}

		/// <summary>
		/// Apply nullification to values of given DataTable
		/// </summary>
		/// <param name="dt"></param>
		public static void NullifyValues(DataTable dt)
		{
			NullifyValues(dt, true);
		}

		/// <summary>
		/// Apply nullification to values of given DataSet
		/// </summary>
		/// <param name="ds"></param>
		public static void NullifyValues(DataSet ds)
		{
			NullifyValues(ds, true);
		}

		/// <summary>
		/// Change the mapping type of given DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="mappingType"></param>
		public static void SetColumnMapping(DataSet ds, MappingType mappingType)
		{
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dt = ds.Tables[i];

				for (int y = 0; y < dt.Columns.Count; y++)
				{
					DataColumn dc = dt.Columns[y];
					dc.ColumnMapping = mappingType;
				}
			}
		}

		/// <summary>
		/// Create DataRelation to given DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="relations"></param>
		public static void SetRelation(DataSet ds, params DataRelationHelper[] relations)
		{
			try
			{
				if (ds == null)
					return;

				for (int i = 0; i < relations.Length; i++)
				{
					DataRelationHelper r = relations[i];
					r.SetRelation(ds);
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "ds", DebuggerTool.Dump(ds));
				DebuggerTool.AddData(ex, "relations", DebuggerTool.Dump(relations));
				throw;
			}
		}

		/// <summary>
		/// Set Nested to true to given DataSet
		/// </summary>
		/// <param name="ds"></param>
		public static void SetNested(DataSet ds)
		{
			if (ds.Relations.Count == 0)
				throw new ArgumentException("no DataRelations defined");

			for (int i = 0; i < ds.Relations.Count; i++)
			{
				DataRelation dr = ds.Relations[i];
				dr.Nested = true;
			}
		}

		/// <summary>
		/// Retrieve DataRelations of given DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public static DataTable GetRelation(DataSet ds)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("RelationID", typeof(int)).AutoIncrement = true;
			dt.Columns.Add("ParentTable", typeof(string));
			dt.Columns.Add("ChildTable", typeof(string));
			dt.Columns.Add("ParentColumns", typeof(string));
			dt.Columns.Add("ChildColumns", typeof(string));
			dt.Columns.Add("RelationName", typeof(string));
			dt.Columns.Add("Nested", typeof(bool));

			if (ds != null)
			{
				for (int i = 0; i < ds.Relations.Count; i++)
				{
					DataRelation relation = ds.Relations[i];
					DataRow row = dt.NewRow();

					row["RelationName"] = relation.RelationName;
					row["ParentTable"] = relation.ParentTable.TableName;
					row["ParentColumns"] = DumpConstraint(relation.ParentKeyConstraint);
					row["ChildTable"] = relation.ChildTable.TableName;
					row["ChildColumns"] = DumpConstraint(relation.ChildKeyConstraint);
					row["Nested"] = relation.Nested;

					dt.Rows.Add(row);
				}
			}

			return dt;
		}

		/// <summary>
		/// Dump the constraint
		/// </summary>
		/// <param name="constraint"></param>
		/// <returns></returns>
		private static string DumpConstraint(Constraint constraint)
		{
			StringBuilder s = new StringBuilder();
			DataColumn[] columns = null;

			if (constraint is UniqueConstraint)
			{
				columns = ((UniqueConstraint)constraint).Columns;
			}
			else if (constraint is ForeignKeyConstraint)
			{
				columns = ((ForeignKeyConstraint)constraint).Columns;
			}

			if (columns == null)
				return string.Empty;

			for (int i = 0; i < columns.Length; i++)
			{
				if (s.Length > 0)
					s.Append(", ");
				s.Append(columns[i].ColumnName);
			}

			return s.ToString();
		}

		/// <summary>
		/// Retrieve the Metadata of a given DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public static DataTable GetMetadata(DataSet ds)
		{
			DataTable dtMetadata = new DataTable();
			dtMetadata.Columns.Add("MetadataID", typeof(int)).AutoIncrement = true;
			dtMetadata.Columns.Add("table_name", typeof(string));
			dtMetadata.Columns.Add("column_name", typeof(string));
			dtMetadata.Columns.Add("data_type", typeof(string));
			dtMetadata.Columns.Add("is_nullable", typeof(bool));

			if (ds != null)
			{
				for (int i = 0; i < ds.Tables.Count; i++)
				{
					DataTable dt = ds.Tables[i];
					string tablename = dt.TableName;

					for (int c = 0; c < dt.Columns.Count; c++)
					{
						DataColumn dc = dt.Columns[c];

						DataRow row = dtMetadata.NewRow();

						row["table_name"] = tablename;
						row["column_name"] = dc.ColumnName;
						row["data_type"] = dc.DataType;
						row["is_nullable"] = dc.AllowDBNull;

						dtMetadata.Rows.Add(row);
					}
				}
			}

			return dtMetadata;
		}

		/// <summary>
		/// Converts DataSet to XML format
		/// </summary>
		/// <param name="ds">DataSet to be converted to Xml</param>
		/// <returns>XmlFormatted Data</returns>
		public static string DataSetToXml(DataSet ds)
		{
			using (StringWriter writer = new StringWriter())
			{
				ds.WriteXml(writer, XmlWriteMode.IgnoreSchema);
				return (writer.ToString());
			}
		}

		/// <summary>
		/// Converts a DataTable to XML format
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string DataTableToXml(DataTable dt)
		{
			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(dt.Copy());
				return DataSetToXml(ds);
			}
		}

		/// <summary>
		/// Convert an XML format to a DataSet
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DataSet XmlToDataSet(string value)
		{
			using (StringReader sr = new StringReader(value))
			{
				DataSet ds = new DataSet();
				ds.ReadXml(sr);
				return ds;
			}
		}

		/// <summary>
		/// Convert an XML format to a DataTable
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DataTable XmlToDataTable(string value)
		{
			using (DataSet ds = XmlToDataSet(value))
			{
				if (ds.Tables.Count == 0)
					throw new ArgumentException("no tables found");

				return ds.Tables[0];
			}
		}

		/// <summary>
		/// Add the list of DataTable into a DataSet and return the DataSet
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DataSet ToDataSet(params DataTable[] dt)
		{
			DataSet ds = new DataSet();
			ds.Tables.AddRange(dt);
			return ds;
		}

		/// <summary>
		/// Copy all columns from source row to target row.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void CopyRow(DataRow source, DataRow target)
		{
			var dt = source.Table;

			foreach (DataColumn dc in dt.Columns)
			{
				target[dc.ColumnName] = source[dc.ColumnName];
			}
		}

		/// <summary>
		/// Pivot a table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DataTable PivotTable(DataTable dt, string[] rows, string[] columns, string value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Copy all table metadata (columns) from source to target
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void CopyColumns(DataColumnCollection source, DataColumnCollection target)
		{
			foreach (DataColumn dc in source)
			{
				target.Add(dc.ColumnName, dc.DataType);
			}
		}

		/// <summary>
		/// reformat the xml string
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static string ReformatXML(string xml)
		{
			if (string.IsNullOrEmpty(xml))
				return string.Empty;

			var xmlInput = new StringBuilder(xml);

			// serializer hickup on utf-16... change the XML Declaration to use to utf-8
			xmlInput.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");

			var xd = new XmlDocument();
			xd.LoadXml(xmlInput.ToString());

			var sb = new StringBuilder();
			XmlTextWriter xtw = null;

			try
			{
				xtw = new XmlTextWriter(new StringWriter(sb));
				xtw.Indentation = 4;
				xtw.IndentChar = ' ';
				xtw.QuoteChar = '"';
				xtw.Formatting = Formatting.Indented;
				xd.WriteTo(xtw);
			}
			finally
			{
				if (xtw != null)
					xtw.Close();
			}

			return sb.ToString();
		}

		/// <summary>
		/// return the max length for the data column
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static int MaxLength(DataTable dt, string column)
		{
			int max = 0;

			foreach (DataRow dr in dt.Rows)
			{
				var s = dr[column].ToString();
				max = Math.Max(s.Length, max);
			}

			return max;
		}
	}
}
