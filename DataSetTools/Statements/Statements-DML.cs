// QueryAnything: Statements-DML.cs
//
// Copyright 2007 Steve T. Pham
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// Linking this library statically or dynamically with other modules is
// making a combined work based on this library.  Thus, the terms and
// conditions of the GNU General Public License cover the whole
// combination.
// 
// As a special exception, the copyright holders of this library give you
// permission to link this library with independent modules to produce an
// executable, regardless of the license terms of these independent
// modules, and to copy and distribute the resulting executable under
// terms of your choice, provided that you also meet, for each linked
// independent module, the terms and conditions of the license of that
// module.  An independent module is a module which is not derived from
// or based on this library.  If you modify this library, you may extend
// this exception to your version of the library, but you are not
// obligated to do so.  If you do not wish to do so, delete this
// exception statement from your version.using System;

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace crudwork.DataSetTools.Statements
{
	#region DML Statements
	/// <summary>
	/// Select Statement
	/// </summary>
	public class SelectStatement : Statement, IStatement
	{
		private bool isDistinct;
		//private int topResults;
		private string[] columns;

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public SelectStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Select)
		{
			/*
			 * SELECT distinct [columns]
			 * FROM	[tables]
			 * WHERE [filter]
			 * GROUP BY [columns]
			 * HAVING [filter]
			 * ORDER BY [columns]
			 * */

			ParseSelectClause();
		}
		#endregion

		#region Private methods
		private void ParseSelectClause()
		{
			string value = base.GetValue("select");
			IsDistinct = false;

			if (value.StartsWith("distinct ", StringComparison.InvariantCultureIgnoreCase))
			{
				IsDistinct = true;
				value = value.Remove(0, "distinct ".Length);
			}

			//if (value.StartsWith("top ", StringComparison.InvariantCultureIgnoreCase))
			//{
			//    topResults = 10;
			//    value = value.Remove(0, "top ".Length);
			//}


			Columns = Common.SplitTrim(value, StringSplitOptions.RemoveEmptyEntries, ",");
		}
		#endregion

		#region Properties
		/// <summary>
		/// Indicate whether the data in the resultset should be distinctive
		/// </summary>
		private bool IsDistinct
		{
			get
			{
				return this.isDistinct;
			}
			set
			{
				this.isDistinct = value;
			}
		}

		/// <summary>
		/// Create a resultset with these columns
		/// </summary>
		private string[] Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				this.columns = value;
			}
		}

		/// <summary>
		/// Create a resultset based on these tables
		/// </summary>
		private string[] Tables
		{
			get
			{
				string value = base.GetValue("from");
				return Common.SplitTrim(value, StringSplitOptions.RemoveEmptyEntries, ",");
			}
		}

		/// <summary>
		/// The row filter criteria
		/// </summary>
		private string RowFilter
		{
			get
			{
				return base.GetValue("where");
			}
		}

		/// <summary>
		/// The sort criteria
		/// </summary>
		private string SortCriteria
		{
			get
			{
				return base.GetValue("order by");
			}
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			string tablename = Tables[0];

			using (DataView dv = Filter(tablename, RowFilter, SortCriteria))
			{
				DataTable dt;
				if (Columns[0] == "*")
				{
					dt = dv.ToTable(tablename, IsDistinct);
				}
				else
				{
					dt = dv.ToTable(tablename, IsDistinct, Columns);
				}
				return new QueryResult(dt);
			}
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Insert Statement via INSERT INTO table (Field1, Field2) VALUES (Value1, Value2)
	/// </summary>
	public class InsertStatement : Statement, IStatement
	{
		private string table;
		private string[] columns;
		private List<KeyValue> keyValues;

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public InsertStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Insert)
		{
			ParseInsertIntoClause();
		}
		#endregion

		#region Private methods
		private void ParseInsertIntoClause()
		{
			// insert into Table1 (Field1, Field2) select f1, f2 from Table2
			string value = base.GetValue("insert into");
			int idx = value.IndexOfAny(new char[] { ' ', '\t' });

			// store the tablename
			Table = value.Substring(0, idx);

			//string[] columns = null;
			string[] values = null;

			// store the columns
			if (idx > 0)
			{
				value = Common.EatParenthesis(value.Substring(idx + 1).Trim(' ', '\t'));
				columns = Common.SplitTrim(value, StringSplitOptions.None, ",");
			}
			else
			{
				DataTable dt = base.GetDataTable(Table);
				columns = DataSetUtil.GetColumns(dt);
			}

			if (base.ContainsKey("values"))
			{
				value = Common.EatParenthesis(base.GetValue("values"));
				values = Common.SplitTrim(value, StringSplitOptions.None, ",");

				if (columns.Length != values.Length)
					throw new ArgumentException("incorrect number of items: missing columns or values");

				keyValues = new List<KeyValue>();
				for (int i = 0; i < columns.Length; i++)
				{
					KeyValue kv = new KeyValue(columns[i], Common.Unqoute(values[i]));
					keyValues.Add(kv);
				}
			}
		}

		private List<KeyValue> MakePair(DataRow dr, string[] columns)
		{
			List<KeyValue> results = new List<KeyValue>();

			for (int i = 0; i < columns.Length; i++)
			{
				string c = columns[i];
				KeyValue kv = new KeyValue(c, Common.Unqoute(dr[i].ToString()));
				results.Add(kv);
			}

			return results;
		}
		#endregion

		#region Properties
		/// <summary>
		/// insert data to this table
		/// </summary>
		private string Table
		{
			get
			{
				return this.table;
			}
			set
			{
				this.table = value;
			}
		}

		/// <summary>
		/// Get the KeyValue pairs of columns and values.
		/// </summary>
		private List<KeyValue> KeyValues
		{
			get
			{
				return this.keyValues;
			}
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			int rowsAffected = 0;

			if (base.ContainsKey("values"))
			{
				DataTable dt = GetDataTable(Table);
				DataRow dr = dt.NewRow();

				UpdateRow(dr, KeyValues);
				dt.Rows.Add(dr);
				rowsAffected = 1;
			}
			else if (base.ContainsKey("select"))
			{
				string subquery = "select " + base.GetValue("select");
				QueryManager qm = new QueryManager(base.DataSet);
				QueryResult qr = qm.Run(subquery);

				if (!string.IsNullOrEmpty(qr.ErrorText))
					throw new ArgumentException(qr.ErrorText);

				DataTable dt = GetDataTable(Table);

				for (int i = 0; i < qr.DataResult.Rows.Count; i++)
				{
					List<KeyValue> keyValueList = MakePair(qr.DataResult.Rows[i], columns);

					DataRow dr = dt.NewRow();
					UpdateRow(dr, keyValueList);
					dt.Rows.Add(dr);
				}

				rowsAffected = dt.Rows.Count;
			}

			return new QueryResult(null, null, null, 1);
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	///// <summary>
	///// Insert Statement via INSERT INTO table (Field1, Field2) SELECT f1, f2 FROM table2
	///// </summary>
	//public class InsertQueryStatement : Statement, IStatement
	//{
	//    #region Constructors
	//    /// <summary>
	//    /// create new object with given attributes
	//    /// </summary>
	//    /// <param name="clauses"></param>
	//    public InsertQueryStatement(List<KeyValue> clauses)
	//        : base(clauses)
	//    {
	//    }
	//    #endregion

	//    #region IStatement Members
	//    /// <summary>
	//    /// Execute the statement
	//    /// </summary>
	//    /// <returns></returns>
	//    public QueryResults Run()
	//    {
	//    }

	//    QueryResults IStatement.Run()
	//    {
	//        return Run();
	//    }
	//    #endregion
	//}

	/// <summary>
	/// Update Statement
	/// </summary>
	public class UpdateStatement : Statement, IStatement
	{
		private List<KeyValue> keyValues;

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public UpdateStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Update)
		{
			/*
			 * UPDATE table
			 * SET KeyValues
			 * WHERE filter
			 * */
			keyValues = QueryParser.ParseSet(base.GetValue("set"));
		}
		#endregion

		#region Properties
		/// <summary>
		/// The table to update
		/// </summary>
		private string Table
		{
			get
			{
				return base.GetValue("update");
			}
		}

		/// <summary>
		/// The row filter criteria
		/// </summary>
		private string RowFilter
		{
			get
			{
				return base.GetValue("where");
			}
		}

		/// <summary>
		/// Get a list of key value pairs for update
		/// </summary>
		private List<KeyValue> KeyValues
		{
			get
			{
				return this.keyValues;
			}
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			string tablename = Table;
			string where = RowFilter;
			List<KeyValue> setValues = KeyValues;

			using (DataView dv = Filter(tablename, where, null))
			{
				int rowAffected = dv.Count;
				foreach (DataRowView drv in dv)
				{
					UpdateRow(drv.Row, setValues);
				}
				return new QueryResult(null, null, null, rowAffected);
			}
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Delete Statement
	/// </summary>
	public class DeleteStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public DeleteStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Delete)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// delete data from this table
		/// </summary>
		private string Table
		{
			get
			{
				return base.GetValue("delete");
			}
		}

		/// <summary>
		/// The row filter criteria
		/// </summary>
		private string RowFilter
		{
			get
			{
				return base.GetValue("where");
			}
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			string tablename = Table;
			string where = RowFilter;
			using (DataView dv = Filter(tablename, where, null))
			{
				int rowAffected = dv.Count;
				foreach (DataRowView drv in dv)
				{
					drv.Delete();
				}

				return new QueryResult(null, null, null, rowAffected);
			}
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}
	#endregion
}
