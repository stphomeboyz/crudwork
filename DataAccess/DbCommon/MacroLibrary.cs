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
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using crudwork.Models.DataAccess;
using System.Diagnostics;
using crudwork.Utilities;

namespace crudwork.DataAccess.DbCommon
{
	[MacroService("crudwork")]
	internal class MacroLibrary
	{
		private DataFactory df;
		internal MacroLibrary(DataFactory df)
		{
			this.df = df;
		}

		#region Handy macros that execute query
		[MacroMethod(Name = "DropTable", Description = "Drop a table")]
		public void DropTable(string tablename)
		{
			if (df.Database.GetTables(tablename, crudwork.Models.DataAccess.QueryFilter.Exact).Count == 0)
				return;
			df.ExecuteNonQuery("drop table [" + tablename + "]");
		}

		[MacroMethod(Name = "DropIndex", Description = "Drop an index")]
		public void DropIndex(string tablename, string indexname)
		{
			if (df.Database.GetIndexes(indexname, crudwork.Models.DataAccess.QueryFilter.Exact).Count == 0)
				return;
			var q = df.MacroManager.DropIndexStatement(tablename, indexname);
			df.ExecuteNonQuery(q);
		}

		[MacroMethod(Name = "CreateIndex", Description = "Create an index")]
		public void CreateIndex(string tablename, string column, string indexname)
		{
			if (string.IsNullOrEmpty(indexname))
				indexname = string.Format("ix_{0}_{1}", tablename, column);

			if (df.Database.GetIndexes(indexname, crudwork.Models.DataAccess.QueryFilter.Exact).Count > 0)
				return;
			var q = df.MacroManager.CreateIndexStatement(tablename, column, indexname);
			df.ExecuteNonQuery(q);
		}

		[MacroMethod(Name = "CopyTable", Description = "Copy a source table to a destination table")]
		public void CopyTable(string inputTable, string outputTable)
		{
			CopyTable(inputTable, outputTable, null, null);
		}

		private void CopyTable(string inputTable, string outputTable, string selectClause, string whereClause)
		{
			var q = df.MacroManager.CopyTable(inputTable, outputTable, selectClause, whereClause);
			df.ExecuteNonQuery(q);
		}
		
		//private void AddColumn(string tablename, DataColumn newColumn)
		//{
		//    throw new NotImplementedException();
		//}

		[MacroMethod(Name = "DropColumn", Description = "Drop a column")]
		public void DropColumn(string tablename, string column)
		{
			df.ExecuteNonQuery("alter table [" + tablename + "] drop column [" + column + "]");
		}

		[MacroMethod(Name = "AssignRowNumber", Description = "Create ROWNUM column")]
		public void AssignRowNumber(string inputTable, string outputTable, string rownum)
		{
			string q = null;

			switch (df.Provider)
			{
				case DatabaseProvider.SQLite:
				case DatabaseProvider.OleDb:
				case DatabaseProvider.Odbc:
				case DatabaseProvider.SqlClient:
					// 1. Create an empty table
					CopyTable(inputTable, outputTable, null, "1 = 2");

					// 2. Add a rownum column with auto increment
					//AddColumn(outputTable, rownum, false, true);
					q = string.Format("alter table {0} add {1} int not null identity(1,1)", outputTable, rownum);
					df.ExecuteNonQuery(q);

					// 3. insert rows...
					q = string.Format("insert into {0} select b.*, null as {2} from {1} b", outputTable, inputTable, rownum);
					df.ExecuteNonQuery(q);
					break;

				case DatabaseProvider.OracleClient:
				case DatabaseProvider.OracleDataProvider:
					CopyTable(inputTable, outputTable + " b", "b.*, rownum as " + rownum, null);
					break;

				case DatabaseProvider.Unspecified:
				default:
					throw new ArgumentOutOfRangeException("df.Provider= " + df.Provider);
			}
		}
		#endregion

		#region MacroHelp
		[MacroMethod(Name = "Help", Description = "List help information for all known macros")]
		public DataTable MacroHelp(MacroList macros, string[] args)
		{
			var dt = new DataTable("MacroHelp");
			dt.Columns.Add("Name", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("Help", typeof(string));

			foreach (var item in macros)
			{
				var dr = dt.NewRow();
				dt.Rows.Add(dr);

				var methodAttr = MacroList.GetAttribute<MacroMethodAttribute>(item);

				dr["Name"] = methodAttr.Name ?? item.MethodInfo.Name;
				dr["Description"] = methodAttr.Description;
				dr["Help"] = MacroList.CreateHelp(item.MethodInfo);
			}

			return dt;
		}
		#endregion

		#region Groupby
		/// <summary>
		/// Perform a GROUP BY on a table column and return number of occurrence for each entry
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="groupByColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "Groupby", Description = "Perform a GROUP BY on a table column and return number of occurrence for each entry")]
		public DataTable GroupBy(string tablename, string groupByColumn)
		{
			var query = string.Format("select {0}, count(*) Count from {1} group by {0} order by {0}",
				groupByColumn, tablename);
			return df.FillTable(query);
		}
		#endregion

		#region Grouping
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
		/// Split a column into two based on the value using the given grouping significant
		/// </summary>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="column"></param>
		/// <param name="groupSignificant">LeftMost or RightMost</param>
		/// <param name="newLeftColumn"></param>
		/// <param name="newRightColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "SplitColumn", Description = "Split a column into two based on the value using the given grouping significant")]
		public DataTable SplitColumn(string inputTable, string outputTable,
			string column, string groupSignificant, string newGroupColumn, string newEntryColumn)
		{
			try
			{
				var gs = (GroupSignificant)Enum.Parse(typeof(GroupSignificant), groupSignificant);

				// 1. Get a list of unique values from input column.
				var q = string.Format("select distinct {1} from {0}", inputTable, column);
				var dt = df.FillTable(q);

				// 2. Create definition file
				var def = CreateDefinition(dt, column, gs);
				int maxlen = DataUtil.MaxLength(def, DEF_ENTRY_COLUMN);

				// 3. Create output table
				DropTable(outputTable);
				CopyTable(inputTable, outputTable, null, null);

				// 4. create new fields
				var s = "alter table [" + outputTable + "] add <C> varchar(" + maxlen + ")";
				df.ExecuteNonQuery(s.Replace("<C>", newGroupColumn));
				df.ExecuteNonQuery(s.Replace("<C>", newEntryColumn));

				// 5. update each entries
				var sb = new StringBuilder();
				foreach (DataRow item in def.Rows)
				{
					var group = item[DEF_GROUP_COLUMN].ToString();
					var entry = item[DEF_ENTRY_COLUMN].ToString();
					var stem = entry.Replace(group, "");

					sb.Length = 0;
					sb.AppendFormat("update {0} set", outputTable);
					sb.AppendFormat(" {0}='{2}', {1}='{3}'", newGroupColumn, newEntryColumn, group, stem);
					sb.AppendFormat(" where {0}='{1}'", column, entry);
					var result = df.ExecuteNonQuery(sb.ToString());
				}

				return df.FillTable("select * from [" + outputTable + "]");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region Helpers
		/// <summary>The table name containing the information in regard to the input columns and the group it is assocated to.</summary>
		private const string DEF_TABLE_NAME = "Pivot_Definition";
		/// <summary>The primary column name for the pivot_definition</summary>
		private const string DEF_ID_COLUMN = "PivotDefinitionID";
		/// <summary>The column for storing the group name</summary>
		private const string DEF_GROUP_COLUMN = "Group";
		/// <summary>The column for storing the column name.  This has one-to-many relationship with group name</summary>
		private const string DEF_ENTRY_COLUMN = "Entry";
		/// <summary>The column (used internally) for identifying/removing subgroups</summary>
		private const string DEF_DELETE_COLUMN = "Deleted";

		private static DataTable CreateNewDefinitionTable()
		{
			var result = new DataTable(DEF_TABLE_NAME);
			result.Columns.Add(DEF_ID_COLUMN, typeof(int)).AutoIncrement = true;
			result.Columns.Add(DEF_GROUP_COLUMN, typeof(string));
			result.Columns.Add(DEF_ENTRY_COLUMN, typeof(string));
			//result.Columns.Add(DEF_VALUE_COLUMN, typeof(string));
			return result;
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

		private DataTable CreateDefinition(DataTable dt, string columnName, GroupSignificant gs)
		{
			var result = CreateNewDefinitionTable();

			#region Perform Grouping
			using (var dv = new DataView(dt))
			{
				foreach (DataRow dr in dt.Rows)
				{
					string v = StringUtil.TrimWhitespace(dr[columnName].ToString());

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
						dr2[DEF_ENTRY_COLUMN] = StringUtil.TrimWhitespace(dv[i][columnName].ToString());
						result.Rows.Add(dr2);
					}
					#endregion
				}
			}
			#endregion

			RemoveSubGroups(ref result, gs);

			return result;
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
					return StringUtil.Reverse(value);

				case GroupSignificant.LeftMost:
					return value;

				default:
					throw new ArgumentOutOfRangeException("gs=" + gs);
			}
		}
		#endregion

		#endregion

		#region Pivot macro
		/// <summary>
		/// Pivot a table
		/// </summary>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="valueColumn"></param>
		/// <param name="entryColumn"></param>
		/// <param name="groupColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "Pivot", Description = "Pivot a table")]
		public DataTable PivotTable(string inputTable, string outputTable, string valueColumn, string entryColumn, string groupColumn)
		{
			var sb = new StringBuilder();

			CreateIndex(inputTable, entryColumn, null);
			CreateIndex(inputTable, groupColumn, null);

			var groups = new DataView(GroupBy(inputTable, groupColumn));
			groups.Sort = groupColumn;

			#region SELECT clause
			sb.AppendLine("select");
			sb.AppendLine("    b.<E> [<V> of <E> from <T>]");

			for (int i = 0; i < groups.Count; i++)
			{
				var dr = groups[i];
				sb.AppendFormat("    ,k{0}.v{0} [<G> = '{1}']", i, dr[groupColumn]);
				sb.AppendLine();
			}
			#endregion

			#region FROM clause
			sb.AppendLine("from");
			sb.AppendLine("    (select distinct <E> from <T>) b");

			for (int i = 0; i < groups.Count; i++)
			{
				var dr = groups[i];
				sb.AppendFormat("    left join (select <E>, <V> as v{0} from <T> where <G> = '{1}') k{0} on k{0}.<E> = b.<E>",
					i, dr[groupColumn]);
				sb.AppendLine();
			}
			#endregion

			#region ORDER BY clause
			sb.AppendLine("order by b.<E>");
			#endregion

			#region replace variables
			sb.Replace("<V>", valueColumn);
			sb.Replace("<E>", entryColumn);
			sb.Replace("<G>", groupColumn);
			sb.Replace("<T>", inputTable);
			#endregion

			Debug.WriteLine(sb.ToString());
			var dt = df.FillTable(sb.ToString());

			df.CreateTable(outputTable, dt);
			return dt;
		}

		[MacroMethod(Name = "Pivot2", Description = "Pivot a table")]
		public DataTable Pivot2(string inputTable, string outputTable, string valueColumn, string entryColumn, string groupColumn)
		{
			var sb = new StringBuilder();

			CreateIndex(inputTable, entryColumn, null);
			CreateIndex(inputTable, groupColumn, null);

			var groups = new DataView(GroupBy(inputTable, groupColumn));
			groups.Sort = groupColumn;

			for (int i = groups.Count - 1; i >= 0; i--)
			{
				var drv = groups[i];
				sb.AppendFormat("select b{0}.*, k{0}.v{0} [<G> = '{1}'] from (", i, drv[groupColumn]);
				sb.AppendLine();
			}

			sb.AppendFormat("select distinct <E> [<V> of <E> from <T>] from <T>");
			sb.AppendLine();

			for (int i = 0; i < groups.Count; i++)
			{
				var drv = groups[i];
				sb.AppendFormat(") b{0} left join (select <E>, <V> as v{0} from <T> where <G> = '{1}') k{0} on k{0}.<E> = b{0}.[<V> of <E> from <T>]",
					i, drv[groupColumn]);
				sb.AppendLine();
			}

			#region replace variables
			sb.Replace("<V>", valueColumn);
			sb.Replace("<E>", entryColumn);
			sb.Replace("<G>", groupColumn);
			sb.Replace("<T>", inputTable);
			#endregion

			Debug.WriteLine(sb.ToString());
			var dt = df.FillTable(sb.ToString());

			df.CreateTable(outputTable, dt);
			return dt;
		}
		#endregion

		#region Dupi/Dupe/Dupo macros
		private string MakeDupQuery(string inputTable, string outputTable, string primaryKey, string having)
		{
			var where = new StringBuilder(@"<PRIKEY> in (
		select distinct <PRIKEY>
			from <INPUT>
			group by <PRIKEY>
			having <HAVINGCLAUSE>
	)
	order by <PRIKEY>)");
			where.Replace("<INPUT>", inputTable);
			where.Replace("<PRIKEY>", primaryKey);
			where.Replace("<HAVINGCLAUSE>", having);
			return df.MacroManager.CopyTable(inputTable, outputTable, "", where.ToString());
		}

		/// <summary>
		/// Return only the rows without any duplicated key.
		/// <para>.</para>
		/// <para>.</para>
		/// <para>Sample Input: APPLE, APPLE, PEACH</para>
		/// <para>DUPE returns PEACH; DUPI returns APPLE, APPLE; and DUPO returns APPLE, PEACH.</para>
		/// </summary>
		/// <remarks>
		/// The DUPE, DUPI, DUPO scripts work similarly like the SORTN command.
		/// Please refer to the Opt-Tech Sortn manual for more information.
		/// 
		/// DUPE: (dup exclude) removes all non-unique records.
		/// DUPI: (dup include) includes all non-unique records.
		/// DUPO: (dup out)     keeps one unique records for each value.
		/// </remarks>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="primaryColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "Dupe", Description = "Return only the rows without any duplicated key")]
		public DataTable Dupe(string inputTable, string outputTable, string primaryColumn)
		{
			var q = MakeDupQuery(inputTable, outputTable, primaryColumn, "count(*) = 1");
			df.ExecuteNonQuery(q);
			return df.FillTable("select * from [" + outputTable + "]");
		}

		/// <summary>
		/// Return only the rows with duplicated key.
		/// <para>.</para>
		/// <para>.</para>
		/// <para>Sample Input: APPLE, APPLE, PEACH</para>
		/// <para>DUPE returns PEACH; DUPI returns APPLE, APPLE; and DUPO returns APPLE, PEACH.</para>
		/// </summary>
		/// <remarks>
		/// The DUPE, DUPI, DUPO scripts work similarly like the SORTN command.
		/// Please refer to the Opt-Tech Sortn manual for more information.
		/// 
		/// DUPE: (dup exclude) removes all non-unique records.
		/// DUPI: (dup include) includes all non-unique records.
		/// DUPO: (dup out)     keeps one unique records for each value.
		/// </remarks>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="primaryColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "Dupi", Description = "Return only the rows with duplicated key")]
		public DataTable Dupi(string inputTable, string outputTable, string primaryColumn)
		{
			var q = MakeDupQuery(inputTable, outputTable, primaryColumn, "count(*) > 1");
			df.ExecuteNonQuery(q);
			return df.FillTable("select * from [" + outputTable + "]");
		}

		/// <summary>
		/// Return only the unique rows by the key
		/// <para>.</para>
		/// <para>.</para>
		/// <para>Sample Input: APPLE, APPLE, PEACH</para>
		/// <para>DUPE returns PEACH; DUPI returns APPLE, APPLE; and DUPO returns APPLE, PEACH.</para>
		/// </summary>
		/// <remarks>
		/// The DUPE, DUPI, DUPO scripts work similarly like the SORTN command.
		/// Please refer to the Opt-Tech Sortn manual for more information.
		/// 
		/// DUPE: (dup exclude) removes all non-unique records.
		/// DUPI: (dup include) includes all non-unique records.
		/// DUPO: (dup out)     keeps one unique records for each value.
		/// </remarks>
		/// <param name="inputTable"></param>
		/// <param name="outputTable"></param>
		/// <param name="primaryColumn"></param>
		/// <param name="secondaryColumn"></param>
		/// <returns></returns>
		[MacroMethod(Name = "Dupo", Description = "Return only the unique rows by the key")]
		public DataTable Dupo(string inputTable, string outputTable, string primaryColumn, string secondaryColumn)
		{
			var random = Path.GetRandomFileName();
			var dupe = "tmp_dupo_" + random + "_1";
			var dupi = "tmp_dupo_" + random + "_2";

			var sb = new StringBuilder();

			try
			{
				// Pre-steps:
				// a) create new table.  (This is the output table)
				// b) create index on primary column
				CopyTable(inputTable, outputTable, null, null);
				CreateIndex(outputTable, primaryColumn, null);

				// ----------------------------------------------------------------------------------------
				// IMPORTANT: at this point, all CRUD actions should be performed against the outputTable!
				// ----------------------------------------------------------------------------------------

				// 1. Select ALL entries with duplicated primary key
				sb.Length = 0;
				sb.Append(MakeDupQuery(outputTable, dupe, primaryColumn, "count(*) > 1"));
				df.ExecuteNonQuery(sb.ToString());

				// 2. Dedup only the duplicated list
				DupoInternal(dupe, dupi, primaryColumn, secondaryColumn);

				// 3. Remove ALL entries with the duplicated entries
				sb.Length = 0;
				sb.Append("delete from <BASE> where <PRIKEY> in (select k.<PRIKEY> from <KEY> k)");
				sb.Replace("<BASE>", outputTable);
				sb.Replace("<KEY>", dupi);
				sb.Replace("<PRIKEY>", primaryColumn);
				df.ExecuteNonQuery(sb.ToString());

				// 4. insert the unique entries.
				sb.Length = 0;
				sb.Append("insert into <BASE> select * from <KEY>");
				sb.Replace("<BASE>", outputTable);
				sb.Replace("<KEY>", dupi);
				df.ExecuteNonQuery(sb.ToString());

				return df.FillTable("select * from [" + outputTable + "]");
			}
			finally
			{
				// clean up...
				DropTable(dupe);
				DropTable(dupi);
			}
		}

		private void DupoInternal(string inputTable, string outputTable, string primaryColumn, string secondaryColumn)
		{
			var random = Path.GetRandomFileName();
			var sb = new StringBuilder();

			var tmp1 = "tmp_dupo_" + random + "_1";
			var rownum = "dupoIntern_rownum";

			try
			{
				// 1. create temp table with ROWNUM column
				sb.Length = 0;
				sb.Append("(select * from <INPUT> order by <PRIKEY>) a");
				sb.Replace("<INPUT>", inputTable);
				sb.Replace("<PRIKEY>", primaryColumn);
				AssignRowNumber(sb.ToString(), tmp1, rownum);

				// 2. create index on ROWNUM column
				CreateIndex(tmp1, rownum, null);

				// 3. select the first of each 
				sb.Length = 0;
				sb.Append("a.<ROWNUM> = (select min(k.<ROWNUM>) from <INPUT> k where k.<PRIKEY> = a.<PRIKEY>)");
				sb.Replace("<INPUT>", inputTable);
				sb.Replace("<PRIKEY>", primaryColumn);
				sb.Replace("<ROWNUM>", rownum);
				CopyTable(tmp1 + " a", outputTable, null, sb.ToString());

				// 4. drop the ROWNUM column
				DropColumn(outputTable, rownum);
			}
			finally
			{
				// clean up...
				DropTable(tmp1);
			}
		}
		#endregion
	}
}
