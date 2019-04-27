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
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using crudwork.DataAccess;
using System.Text.RegularExpressions;
using System.Text;
using crudwork.Utilities;
using crudwork.DynamicRuntime;
//using crudwork.DataWarehouse.Models;

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// Olap tools
	/// </summary>
	[Obsolete("This is the old code", true)]
	internal class OlapX
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public OlapX()
		{
		}

		/// <summary>
		/// pivot the table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="rowName"></param>
		/// <param name="colName"></param>
		/// <param name="valName"></param>
		/// <returns></returns>
		public static DataTable PivotTable(DataTable dt, string rowName, string colName, string valName)
		{
			return PivotTable(dt, rowName, colName, valName, "PivotKey");
		}

		/// <summary>
		/// pivot the table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="rowName"></param>
		/// <param name="colName"></param>
		/// <param name="valName"></param>
		/// <param name="pivotKey"></param>
		/// <returns></returns>
		public static DataTable PivotTable(DataTable dt,
			string rowName, string colName, string valName,
			string pivotKey)
		{
			var pivot = new DataTable("pivot");
			string filterFormat = pivotKey + " = '{0}'";

			pivot.Columns.Add("pivotId", typeof(int)).AutoIncrement = true;
			pivot.Columns.Add(pivotKey, typeof(string));

			foreach (DataRow dr in dt.Rows)
			{
				string r = dr[rowName].ToString();
				string c = dr[colName].ToString();
				string v = dr[valName].ToString();

				var drNew = Filter(pivot, string.Format(filterFormat, r)) ?? pivot.NewRow();

				drNew[pivotKey] = r;

				if (!pivot.Columns.Contains(c))
					pivot.Columns.Add(SafeColumn(c), typeof(string));

				drNew[c] = v;

				if (drNew.RowState == DataRowState.Detached)
					pivot.Rows.Add(drNew);
			}

			return pivot;
		}

		private static DataRow Filter(DataTable dt, string filterExpression)
		{
			using (var dv = new DataView(dt))
			{
				dv.RowFilter = filterExpression;
				return (dv.Count > 0) ? dv[0].Row : (DataRow)null;
			}
		}
		private static string SafeColumn(string c)
		{
			return c;
		}
	}

	/// <summary>
	/// Grouping data using Right-Most and Left-Most significant.
	/// </summary>
	public static class Olap
	{
		#region Constants
		/// <summary>The name of the dataset returned by the Pivot() method</summary>
		public const string PIVOT_DATASET_NAME = "PivotResult";

		/// <summary>The table name containing the pivot information</summary>
		public const string MASTER_TABLE_NAME = "Pivot_Master";

		/// <summary>The name of the input source table</summary>
		public const string SOURCE_TABLE_NAME = "Input";

		/// <summary>The table name containing the information in regard to the input columns and the group it is assocated to.</summary>
		public const string DEF_TABLE_NAME = "Pivot_Definition";
		/// <summary>The primary column name for the pivot_definition</summary>
		public const string DEF_ID_COLUMN = "PivotDefinitionID";
		/// <summary>The column for storing the group name</summary>
		public const string DEF_GROUP_COLUMN = "Group";
		/// <summary>The column for storing the column name.  This has one-to-many relationship with group name</summary>
		public const string DEF_ENTRY_COLUMN = "Entry";
		/// <summary>The column (used internally) for identifying/removing subgroups</summary>
		public const string DEF_DELETE_COLUMN = "Deleted";

		/// <summary>The table name containing the pivot result</summary>
		public const string PIVOT_TABLE_NAME = "Pivot";
		/// <summary>The primary column name of the pivot table</summary>
		public const string PIVOT_ID_COLUMN = "PivotTableID";
		/// <summary>The sub key component of the pivoted value</summary>
		public const string PIVOT_ENTRY_COLUMN = "Entry";
		/// <summary>The column signature for data columns in the pivot table</summary>
		public const string PIVOT_GROUP_FIELD_STEM = "Value_";
		#endregion

		#region Method and helpers for creating definition
		private static DataTable CreateNewDefinitionTable()
		{
			var result = new DataTable(DEF_TABLE_NAME);
			result.Columns.Add(DEF_ID_COLUMN, typeof(int)).AutoIncrement = true;
			result.Columns.Add(DEF_GROUP_COLUMN, typeof(string));
			result.Columns.Add(DEF_ENTRY_COLUMN, typeof(string));
			return result;
		}

		/// <summary>
		/// Create Pivot Definition data table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="columnName"></param>
		/// <param name="gs"></param>
		/// <returns></returns>
		private static DataTable CreateDefinition(DataTable dt, string columnName, GroupSignificant gs)
		{
			var result = CreateNewDefinitionTable();

			dt = DataFactory.FilterTable(dt, "", "", true, columnName);

			#region Perform Grouping
			using (var dv = new DataView(dt))
			{
				foreach (DataRow dr in dt.Rows)
				{
					string v = TrimWhitespace(dr[columnName].ToString());

					if (string.IsNullOrEmpty(v))
						continue;

					string group = string.Empty;

					#region Set the Stem using Right-Most or Left-Most
					switch (gs)
					{
						case GroupSignificant.RightMost:
							{
								int startPos = v.Length - 1;
								do
								{
									string testStem = v.Substring(startPos);
									dv.RowFilter = string.Format("{0} like '%{1}'", columnName, testStem);
									if (dv.Count <= 1)
										break;
									group = testStem; // NoTrailingNumber(testStem);
									startPos--;
								} while (true);
								// use the last good stem
								dv.RowFilter = string.Format("{0} like '%{1}'", columnName, group);
							}
							break;

						case GroupSignificant.LeftMost:
							{
								int stopPos = 0;
								while (++stopPos < v.Length)
								{
									string testStem = v.Substring(0, stopPos);
									dv.RowFilter = string.Format("{0} like '{1}%'", columnName, testStem);
									if (dv.Count <= 1)
										break;
									group = testStem; // NoTrailingNumber(testStem);
								}
								// use the last good stem
								dv.RowFilter = string.Format("{0} like '{1}%'", columnName, group);
							}
							break;

						default:
							throw new ArgumentOutOfRangeException("gs=" + gs);
					}
					#endregion

					if (string.IsNullOrEmpty(group) || group.Length == 1)
					{
						var dr2 = result.NewRow();
						dr2[DEF_GROUP_COLUMN] = v;
						dr2[DEF_ENTRY_COLUMN] = v;
						result.Rows.Add(dr2);
						continue;
					}

					if (DataViewFilter(result, string.Format("{0} = '{1}'", DEF_GROUP_COLUMN, group)).Count > 0)
						continue;

					#region Add to result
					for (int i = 0; i < dv.Count; i++)
					{
						var dr2 = result.NewRow();
						dr2[DEF_GROUP_COLUMN] = group;
						dr2[DEF_ENTRY_COLUMN] = dv[i][columnName].ToString();
						result.Rows.Add(dr2);
					}
					#endregion
				}
			}
			#endregion

			RemoveSubGroups(ref result, gs);

			#region Protect these columns
			result.Columns[DEF_ID_COLUMN].ReadOnly = true;
			result.Columns[DEF_GROUP_COLUMN].ReadOnly = true;
			result.Columns[DEF_ENTRY_COLUMN].ReadOnly = true;
			result.Columns[DEF_DELETE_COLUMN].ReadOnly = true;
			#endregion

			return result;
		}

		private static DataTable CreateDefinition(DataTable dt, string entryColumn, string baseColumn)
		{
			// y becomes the group
			// x + y becomes the entry
			var result = CreateNewDefinitionTable();
			result.Columns.Add(DEF_DELETE_COLUMN, typeof(bool));


			using (var dv = new DataView(dt))
			{
				var dt2 = dv.ToTable(true, baseColumn, entryColumn);

				foreach (DataRow dr in dt2.Rows)
				{
					var dr2 = result.NewRow();
					dr2[DEF_GROUP_COLUMN] = dr[baseColumn];
					dr2[DEF_ENTRY_COLUMN] = string.Format("{0}{1}", dr[entryColumn], dr[baseColumn]);
					dr2[DEF_DELETE_COLUMN] = false;
					result.Rows.Add(dr2);
				}
			}

			/*
			using (var dv = new DataView(result))
			{
				foreach (DataRow dr in dt.Rows)
				{
					var group = dr[baseColumn].ToString();
					var stem = dr[entryColumn].ToString();
					var entry = stem + group;
					System.Diagnostics.Debug.WriteLine(string.Format("group={0} stem={1} entry={2}",
						group, stem, entry));

					dv.RowFilter = string.Format("{0} = '{1}'", DEF_ENTRY_COLUMN, entry);
					if (dv.Count > 0)
						continue;

					var dr2 = result.NewRow();
					dr2[DEF_GROUP_COLUMN] = group;
					dr2[DEF_ENTRY_COLUMN] = entry;
					dr2[DEF_DELETE_COLUMN] = false;
					result.Rows.Add(dr2);
				}
			}*/

			return result;
		}

		private static string NoTrailingNumber(string value)
		{
			while (Regex.IsMatch(value, "[0-9]$"))
			{
				value = value.Substring(0, value.Length - 1);
				if (value.Length == 0)
					break;
			}
			return value;
		}

		/// <summary>
		/// remove any subgroup(s).  A group is considered a subgroup if all of its entries are contained in another group.
		/// </summary>
		/// <param name="definition"></param>
		/// <param name="gs"></param>
		/// <returns></returns>
		private static void RemoveSubGroups(ref DataTable definition, GroupSignificant gs)
		{
			definition.Columns.Add(DEF_DELETE_COLUMN, typeof(bool));

			foreach (DataRow dr in definition.Rows)
			{
				dr[DEF_DELETE_COLUMN] = false;
			}

			using (var dv = new DataView(definition))
			{
				dv.Sort = DEF_GROUP_COLUMN + " asc";

				foreach (DataRowView baseRow in dv)
				{
					var baseId = Convert.ToInt32(baseRow[DEF_ID_COLUMN]);
					var baseGroup = FormatGroup(baseRow[DEF_GROUP_COLUMN].ToString(), gs);

					foreach (DataRowView keyRow in dv)
					{
						var keyId = Convert.ToInt32(keyRow[DEF_ID_COLUMN]);
						var keyGroup = FormatGroup(keyRow[DEF_GROUP_COLUMN].ToString(), gs);

						// don't compare to self
						if (keyId == baseId)
							continue;

						if (baseGroup == keyGroup)
							continue;

						if (Convert.ToBoolean(keyRow[DEF_DELETE_COLUMN]))
							continue;

						if (keyGroup.StartsWith(baseGroup))
							keyRow[DEF_DELETE_COLUMN] = true;
					}
				}

				dv.RowFilter = DEF_DELETE_COLUMN + " = false";
				var newTable = dv.ToTable();
				//newTable.Columns.Remove(DEF_DELETE_COLUMN);
				definition = newTable;
			}
		}

		/// <summary>
		/// Return a string value in reverse order
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string Reverse(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			var stack = new Stack<string>(value.Length);
			var sb = new StringBuilder(value.Length);

			for (int i = 0; i < value.Length; i++)
				stack.Push(value.Substring(i, 1));

			for (int i = 0; i < value.Length; i++)
				sb.Append(stack.Pop());

			return sb.ToString();
		}

		/// <summary>
		/// Format the group name based on the group significant enum
		/// </summary>
		/// <param name="value"></param>
		/// <param name="gs"></param>
		/// <returns></returns>
		private static string FormatGroup(string value, GroupSignificant gs)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			switch (gs)
			{
				case GroupSignificant.RightMost:
					return Reverse(value);

				case GroupSignificant.LeftMost:
					return value;

				default:
					throw new ArgumentOutOfRangeException("gs=" + gs);
			}
		}

		/// <summary>
		/// Trim leading and trailing whitespaces
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string TrimWhitespace(string value)
		{
			return value.Trim(' ', '\t', '\n');
		}

		/// <summary>
		/// return a DataView instance with the row filter for the given data table.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="rowFilter"></param>
		/// <returns></returns>
		private static DataView DataViewFilter(DataTable dt, string rowFilter)
		{
			var dv = new DataView(dt);
			dv.RowFilter = rowFilter;
			return dv;
		}
		#endregion

		#region Method and helpers for pivot table
		private static DataTable CreateNewPivotTable()
		{
			var result = new DataTable(PIVOT_TABLE_NAME);
			result.Columns.Add(PIVOT_ID_COLUMN, typeof(int)).AutoIncrement = true;
			result.Columns.Add(PIVOT_ENTRY_COLUMN, typeof(string));
			return result;
		}

		private static DataSet CreateNewPivotDataSet(DataTable input, DataTable definition, DataTable master, DataTable pivot)
		{
			var ds = new DataSet(PIVOT_DATASET_NAME);
			ds.Tables.Add(master);
			ds.Tables.Add(definition);

			var temp = input.Copy();
			temp.TableName = SOURCE_TABLE_NAME;

			#region Protect the input table by enabling ReadOnly=true to all columns
			foreach (DataColumn dc in temp.Columns)
			{
				dc.ReadOnly = true;
			}
			#endregion

			ds.Tables.Add(temp);
			ds.Tables.Add(pivot);

			return ds;
		}

		/// <summary>
		/// Pivot a table using the X and Y column as the horizontal and vertical column, respectively.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="valueColumn">Value column</param>
		/// <param name="entryColumn">Set the column whose values appear on the ENTRY column</param>
		/// <param name="baseColumn">Set the column whose values appear as the name of the columns</param>
		/// <returns></returns>
		public static DataSet Pivot(DataTable dt, string valueColumn, string entryColumn, string baseColumn)
		{
			var start = DateTime.Now;
			var result = CreateNewPivotTable();
			var definition = CreateDefinition(dt, entryColumn, baseColumn);

			PivotInternal(dt, definition, result, new string[] { entryColumn, baseColumn }, valueColumn);

			var master = CreatePivotMaster(dt, result, entryColumn, baseColumn, valueColumn, start);
			return CreateNewPivotDataSet(dt, definition, master, result);
		}

		/// <summary>
		/// Pivot a table using key column, by splitting its value using the right-most or left-most significant algorithm
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="valueColumn"></param>
		/// <param name="keyColumn"></param>
		/// <param name="gs"></param>
		/// <returns></returns>
		public static DataSet Pivot(DataTable dt, string valueColumn, string keyColumn, GroupSignificant gs)
		{
			var start = DateTime.Now;
			var result = CreateNewPivotTable();
			var definition = CreateDefinition(dt, keyColumn, gs);

			PivotInternal(dt, definition, result, new string[] { keyColumn }, valueColumn);

			var master = CreatePivotMaster(dt, result, keyColumn, valueColumn, gs, start);
			return CreateNewPivotDataSet(dt, definition, master, result);
		}

		private static void PivotInternal(DataTable dt, DataTable definition, DataTable result, string[] keyColumns, string valueColumn)
		{
			#region Create Output Pivot Table
			using (var dv = new DataView(definition))
			{
				dv.Sort = "[" + DEF_ENTRY_COLUMN + "] asc";
				dv.RowFilter = DEF_DELETE_COLUMN + " = false";
				using (var def = dv.ToTable(true, DEF_GROUP_COLUMN))
				{
					foreach (DataRow dr in def.Rows)
					{
						string colName = PIVOT_GROUP_FIELD_STEM + dr[DEF_GROUP_COLUMN];
						var dc = result.Columns.Add(colName, typeof(string));
					}
				}
			}
			#endregion

			#region Populate pivot table
			using (var dvDefinition = new DataView(definition))
			using (var dvResult = new DataView(result))
			using (var dvInput = new DataView(dt))
			{
				{
					var sb = new StringBuilder();
					foreach (var s in keyColumns)
					{
						if (sb.Length > 0)
							sb.Append(", ");
						sb.AppendFormat("[{0}]", s);
					}
					dvInput.Sort = sb.ToString() + " asc";
				}
				string keyValue = null;

				foreach (DataRowView drv in dvInput)
				{
					var dr = drv.Row;

					#region create key
					if (keyColumns.Length == 1)
					{
						keyValue = dr[keyColumns[0]].ToString();
					}
					else
					{
						keyValue = string.Format("{0}{1}", dr[keyColumns[0]], dr[keyColumns[1]]);
					}
					#endregion

					var def = GetDefinition(dvDefinition, keyValue);
					string value = dr[valueColumn].ToString();

					DataRow drNew = GetPivotEntry(dvResult, def.GetStemOnly()) ?? result.NewRow();

					drNew[PIVOT_ENTRY_COLUMN] = def.GetStemOnly();
					drNew[def.GetSafeGroup()] = value;

					if (drNew.RowState == DataRowState.Detached)
					{
						result.Rows.Add(drNew);
						System.Diagnostics.Debug.WriteLine("new row added " + result.Rows.Count);
					}

					System.Diagnostics.Debug.WriteLine(def.Group + " " + def.Entry);
				}
			}
			#endregion

			#region Protect these columns
			result.Columns[PIVOT_ID_COLUMN].ReadOnly = true;
			result.Columns[PIVOT_ENTRY_COLUMN].ReadOnly = true;
			#endregion
		}

		private static Definition GetDefinition(DataView dv, string key)
		{
			dv.RowFilter = string.Format("{0} = false and {1} = '{2}'", DEF_DELETE_COLUMN, DEF_ENTRY_COLUMN, key);
			if (dv.Count != 1)
				throw new ArgumentException(string.Format("expected 1 for key '{0}'; but found {1}", key, dv.Count));
			return new Definition(dv[0].Row);
		}

		private static DataRow GetPivotEntry(DataView dv, string entry)
		{
			dv.RowFilter = PIVOT_ENTRY_COLUMN + " = '" + entry + "'";
			if (dv.Count != 1)
			{
				return null;
				throw new ArgumentException(string.Format("expected 1 for entry '{0}'; but found {1}", entry, dv.Count));
			}
			return dv[0].Row;
		}

		private static void PivotInternal2(DataTable dt, DataTable definition, DataTable result, string[] keyColumns, string valueColumn)
		{

		}

		#endregion

		#region Method and helpers for creating master table
		private static DataTable CreateNewMasterTable()
		{
			var dt = new DataTable(MASTER_TABLE_NAME);
			dt.Columns.Add("ID", typeof(string));
			dt.Columns.Add("Value", typeof(string));
			return dt;
		}

		private static DataTable CreatePivotMaster(DataTable input,
			DataTable result, string entryColumn, string baseColumn, string valueColumn,
			DateTime start)
		{
			var dt = CreateNewMasterTable();

			dt.Rows.Add("Pivot", "PivotXY");

			dt.Rows.Add("entryColumn", entryColumn);
			dt.Rows.Add("baseColumn", baseColumn);
			dt.Rows.Add("valueColumn", valueColumn);
			dt.Rows.Add("valueType", input.Columns[valueColumn].DataType.ToString());
			dt.Rows.Add("start", start.ToString("s"));
			dt.Rows.Add("elapsed", new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());

			dt.Rows.Add("inputTablename", input.TableName);
			dt.Rows.Add("inputRowCount", input.Rows.Count);
			dt.Rows.Add("inputColumnCount", input.Columns.Count);

			dt.Rows.Add("resultRowCount", result.Rows.Count);
			dt.Rows.Add("resultColumnCount", result.Columns.Count);

			#region Protect these columns
			dt.Columns["ID"].ReadOnly = true;
			dt.Columns["Value"].ReadOnly = true;
			#endregion

			return dt;
		}

		private static DataTable CreatePivotMaster(DataTable input,
			DataTable result, string keyColumn, string valueColumn,
			GroupSignificant gs, DateTime start)
		{
			var dt = CreateNewMasterTable();

			dt.Rows.Add("Pivot", "PivotGS");
			dt.Rows.Add("gs", gs);
			dt.Rows.Add("keyColumn", keyColumn);
			dt.Rows.Add("valueColumn", valueColumn);
			dt.Rows.Add("valueType", input.Columns[valueColumn].DataType.ToString());
			dt.Rows.Add("start", start.ToString("s"));
			dt.Rows.Add("elapsed", new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());

			dt.Rows.Add("inputTablename", input.TableName);
			dt.Rows.Add("inputRowCount", input.Rows.Count);
			dt.Rows.Add("inputColumnCount", input.Columns.Count);

			dt.Rows.Add("resultRowCount", result.Rows.Count);
			dt.Rows.Add("resultColumnCount", result.Columns.Count);

			#region Protect these columns
			dt.Columns["ID"].ReadOnly = true;
			dt.Columns["Value"].ReadOnly = true;
			#endregion

			return dt;
		}
		#endregion

		#region Method and helpers for saving pivot table
		/// <summary>
		/// Create a copy of the input data table, update the value column with the data
		/// from the pivot table, and return the table to the calling method.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public static DataTable SaveOutput(DataSet result)
		{
			var master = GetPivotMaster(result);

			var pivot = result.Tables[PIVOT_TABLE_NAME];
			var output = result.Tables[SOURCE_TABLE_NAME].Copy();

			output.Columns[master.ValueColumn].ReadOnly = false;

			foreach (DataRow dr in pivot.Rows)
			{
				string entry = dr[PIVOT_ENTRY_COLUMN].ToString();

				foreach (DataColumn dc in pivot.Columns)
				{
					// ignore these columns...
					if (dc.ColumnName == PIVOT_ID_COLUMN)
						continue;
					if (dc.ColumnName == PIVOT_ENTRY_COLUMN)
						continue;

					// ignore all other columns that do not start with the signature
					if (!dc.ColumnName.StartsWith(PIVOT_GROUP_FIELD_STEM))
						continue;

					if (dr[dc] == null || dr[dc] == DBNull.Value)
						continue;

					var key = new Dictionary<string, string>();

					switch (master.Pivot.ToUpper())
					{
						case "PIVOTXY":
							key.Add(master.BaseColumn, dc.ColumnName.Replace(PIVOT_GROUP_FIELD_STEM, ""));
							key.Add(master.EntryColumn, entry);
							break;

						case "PIVOTGS":
							#region Create Key base on the processed group significant
							{
								string s = null;
								switch (master.GS)
								{
									case GroupSignificant.RightMost:
										s = entry + dc.ColumnName.Replace(PIVOT_GROUP_FIELD_STEM, "");
										break;
									case GroupSignificant.LeftMost:
										s = dc.ColumnName.Replace(PIVOT_GROUP_FIELD_STEM, "") + entry;
										break;
									default:
										throw new ArgumentOutOfRangeException("master.GS=" + master.GS);
								}
								key.Add(master.KeyColumn, s);
							}
							#endregion
							break;

						default:
							throw new ArgumentOutOfRangeException("master.Pivot=" + master.Pivot);
					}

					object value = Convert.ChangeType(dr[dc], master.ValueType);

					UpdateSource(output, master, key, value);
				}
			}

			#region Unprotect the output file
			foreach (DataColumn dc in output.Columns)
			{
				dc.ReadOnly = false;
			}
			#endregion

			return output;
		}

		private static bool UpdateSource(DataTable source, PivotMaster master, Dictionary<string, string> key, object value)
		{
			using (var dv = new DataView(source))
			{
				var sb = new StringBuilder();
				foreach (KeyValuePair<string, string> kv in key)
				{
					if (sb.Length > 0)
						sb.Append(" and ");
					sb.AppendFormat("[{0}] = '{1}'", kv.Key, kv.Value);
				}
				dv.RowFilter = sb.ToString();
				if (dv.Count != 1)
					throw new ArgumentException(string.Format("expected 1 for column '{0}={1}'; but found {2}", master.KeyColumn, key, dv.Count));

				string oldValue = dv[0][master.ValueColumn].ToString();
				if (oldValue == value.ToString())
					return false;

				dv[0][master.ValueColumn] = value;
				return true;
			}
		}

		private static PivotMaster GetPivotMaster(DataSet result)
		{
			#region Sanity Checks
			if (!result.Tables.Contains(MASTER_TABLE_NAME))
				throw new ArgumentException("Invalid Pivot data set.  Table not found: " + MASTER_TABLE_NAME);
			if (!result.Tables.Contains(DEF_TABLE_NAME))
				throw new ArgumentException("Invalid Pivot data set.  Table not found: " + DEF_TABLE_NAME);
			if (!result.Tables.Contains(SOURCE_TABLE_NAME))
				throw new ArgumentException("Invalid Pivot data set.  Table not found: " + SOURCE_TABLE_NAME);
			if (!result.Tables.Contains(PIVOT_TABLE_NAME))
				throw new ArgumentException("Invalid Pivot data set.  Table not found: " + PIVOT_TABLE_NAME);
			#endregion

			var master = new PivotMaster(result.Tables[MASTER_TABLE_NAME]);

			// TODO: Need more sanity checks here...

			return master;
		}
		#endregion

		#region Enum
		/// <summary>
		/// The group significant enumerator
		/// </summary>
		public enum GroupSignificant
		{
			/// <summary>Use the Right-Most-Significant approach</summary>
			RightMost,
			/// <summary>Use the Left-Most-Significant approach</summary>
			LeftMost,
		}

		/// <summary>
		/// Class representation of the pivot_master table
		/// </summary>
		public class PivotMaster
		{
			#region Properties
			/// <summary>Get or set the pivot id</summary>
			public string Pivot
			{
				get;
				private set;
			}
			/// <summary>Get or set the group column</summary>
			public string GroupColumn
			{
				get;
				private set;
			}
			/// <summary>Get or set the base X column</summary>
			public string EntryColumn
			{
				get;
				private set;
			}
			/// <summary>Get or set the base Y column</summary>
			public string BaseColumn
			{
				get;
				private set;
			}
			/// <summary>Get or set the source key column</summary>
			public string KeyColumn
			{
				get;
				private set;
			}
			/// <summary>Get or set the source value column</summary>
			public string ValueColumn
			{
				get;
				private set;
			}
			/// <summary>Get or set the source value type</summary>
			public Type ValueType
			{
				get;
				private set;
			}
			/// <summary>Get or set the Group Signficant that was used to generate the pivot table</summary>
			public GroupSignificant GS
			{
				get;
				private set;
			}
			/// <summary>Get or set the creation date</summary>
			public DateTime Start
			{
				get;
				private set;
			}
			/// <summary>Get or set the time elapsed</summary>
			public string Elapsed
			{
				get;
				private set;
			}
			/// <summary>Get or set the input table name</summary>
			public string InputTablename
			{
				get;
				private set;
			}
			/// <summary>Get or set the input row count</summary>
			public int InputRowCount
			{
				get;
				private set;
			}
			/// <summary>Get or set the input column count</summary>
			public int InputColumnCount
			{
				get;
				private set;
			}
			/// <summary>Get or set the pivot row count</summary>
			public int ResultRowCount
			{
				get;
				private set;
			}
			/// <summary>Get or set the pivot column count</summary>
			public int ResultColumnCount
			{
				get;
				private set;
			}
			#endregion

			/// <summary>
			/// Create a new instance with given attributes
			/// </summary>
			/// <param name="dt"></param>
			public PivotMaster(DataTable dt)
			{

				foreach (DataRow dr in dt.Rows)
				{
					string id = dr["ID"].ToString();
					string value = dr["Value"].ToString();

					object value2 = null;

					#region Convert to proper data type
					switch (id.ToUpper())
					{
						case "PIVOT":
						case "ENTRYCOLUMN":
						case "BASECOLUMN":
						case "KEYCOLUMN":
						case "VALUECOLUMN":
						case "INPUTTABLENAME":
						case "ELAPSED":
						case "GROUPCOLUMN":
							value2 = value;
							break;

						case "VALUETYPE":
							value2 = Type.GetType(value);
							break;

						case "GS":
							value2 = (GroupSignificant)Enum.Parse(typeof(GroupSignificant), value);
							break;

						case "START":
							value2 = DateTime.Parse(value);
							break;

						case "INPUTROWCOUNT":
						case "INPUTCOLUMNCOUNT":
						case "RESULTROWCOUNT":
						case "RESULTCOLUMNCOUNT":
							value2 = DataConvert.ToInt32(value);
							break;

						default:
							throw new ArgumentOutOfRangeException("id=" + id);
					}
					#endregion

					try
					{
						DynamicCode.SetProperty(this, id, value2);
					}
					catch (Exception ex)
					{
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Class representation of the pivot_definition table
		/// </summary>
		public class Definition
		{
			#region Properties
			/// <summary>Get or set the Definition ID</summary>
			public int ID
			{
				get;
				set;
			}
			/// <summary>Get or set the Group name</summary>
			public string Group
			{
				get;
				set;
			}
			/// <summary>Get or set the Entry name</summary>
			public string Entry
			{
				get;
				set;
			}
			/// <summary>Get or set the Delete flag</summary>
			public bool Deleted
			{
				get;
				set;
			}
			#endregion

			/// <summary>
			/// create new instance with default attributes
			/// </summary>
			public Definition()
			{
			}

			/// <summary>
			/// create new instance with given attributes
			/// </summary>
			/// <param name="dr"></param>
			public Definition(DataRow dr)
			{
				this.ID = Convert.ToInt32(dr[Olap.DEF_ID_COLUMN]);
				this.Group = Convert.ToString(dr[Olap.DEF_GROUP_COLUMN]);
				this.Entry = Convert.ToString(dr[Olap.DEF_ENTRY_COLUMN]);
				this.Deleted = Convert.ToBoolean(dr[Olap.DEF_DELETE_COLUMN]);
			}

			/// <summary>Get the Entry stem -- without the Group</summary>
			public string GetStemOnly()
			{
				return Entry.Replace(Group, string.Empty);
			}
			/// <summary>Get the safe data column name for group</summary>
			public string GetSafeGroup()
			{
				return Olap.PIVOT_GROUP_FIELD_STEM + Group;
			}
		}
		#endregion
	}
}
