using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using crudwork.Utilities;

namespace crudwork.DataAccess.SQLiteClient
{
	[Obsolete("consider using MacroManager", true)]
	internal class SQLiteMacroManager
	{
		private DataFactory df;

		public SQLiteMacroManager(DataFactory df)
		{
			this.df = df;
		}

		public DataTable PivotTable(string inputTable, string outputTable,
			string valueColumn, string entryColumn, string groupColumn)
		{
			CreateIndex(inputTable, entryColumn);
			CreateIndex(inputTable, groupColumn);

			var groups = new DataView(Groupby(inputTable, groupColumn));
			groups.Sort = groupColumn;

			/*
			 *	select b.Zip [Zip from B1_Zip_Comp], k1.Value_2009Q4 [sumCount where Foo = '2009Q4'], k2.Value_2010Q2 [sumCount where Foo = '2010Q2']
			 *	from       (select distinct zip from B1_Zip_Comp) b
			 *	inner join (select zip, sumCount as Value_2009Q4 from B1_Zip_Comp where foo = '2009Q4') k1 on k1.zip = b.zip
			 *	inner join (select zip, sumCount as Value_2010Q2 from B1_Zip_Comp where foo = '2010Q2') k2 on k2.zip = b.zip;
			 *	
			 * */

			var sb = new StringBuilder();

			#region SELECT clause
			sb.AppendLine("select");
			sb.AppendLine("    b.<E> [<V> from <T>]");

			for (int i = 0; i < groups.Count; i++)
			{
				var dr = groups[i];
				sb.AppendFormat("    ,k{0}.v{0} [<V> where <G> = '{1}']", i, dr[groupColumn]);
				sb.AppendLine();
			}
			#endregion

			#region FROM clause
			sb.AppendLine("from");
			sb.AppendLine("    (select distinct <E> from <T>) b");

			for (int i = 0; i < groups.Count; i++)
			{
				var dr = groups[i];
				sb.AppendFormat("    inner join (select <E>, <V> as v{0} from <T> where <G> = '{1}') k{0} on k{0}.<E> = b.<E>",
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

			return df.FillTable(sb.ToString());
		}

		public DataSet Execute(string query)
		{
			query = RegexUtil.Sub(query, "^[ \t]*@[ \t]*", "");
			var tokens = query.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			DataTable dt = null;

			switch (tokens[0].ToUpper())
			{
				case "PIVOT":
					dt = PivotTable(tokens[1], tokens[2], tokens[3], tokens[4], tokens[5]);
					break;
				default:
					throw new ArgumentOutOfRangeException("unknown macro: " + query);
			}

			var ds = new DataSet();
			ds.Tables.Add(dt);
			return ds;
		}


		public bool ExistsIndex(string indexName)
		{
			var result = df.ExecuteScalar("select 1 from sqlite_master where type='index' and upper(name)='" + DataConvert.Quoted(indexName).ToUpper() + "'");
			return result != null;
		}
		public int DropIndex(string indexName)
		{
			if (!ExistsIndex(indexName))
				return 0;
			return df.ExecuteNonQuery("drop index " + indexName);
		}
		public int CreateIndex(string tableName, string column)
		{
			var idxName = "idx_" + tableName + "_" + column;
			return CreateIndex(tableName, column, idxName);
		}
		public int CreateIndex(string tableName, string column, string indexName)
		{
			if (ExistsIndex(indexName))
				return 0;
			var q = string.Format("create index {0} on {1}({2})", indexName, tableName, column);
			return df.ExecuteNonQuery(q);
		}

		public DataTable Groupby(string tableName, string columnName)
		{
			var q = string.Format("select {0}, count(*) from {1} group by {0} order by {0}", columnName, tableName);
			return df.FillTable(q);
		}
	}
}
