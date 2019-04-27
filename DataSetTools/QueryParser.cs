// QueryAnything: QueryParser.cs
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
using System.Resources;
using System.Reflection;
using System.IO;
using crudwork.DataSetTools.Statements;
using System.Configuration;
using crudwork.Utilities;

namespace crudwork.DataSetTools
{
	/// <summary>
	/// QueryAnything Statement Type
	/// </summary>
	public enum QAStatementType
	{
		/// <summary>DML: Select</summary>
		Select,
		/// <summary>DML: Insert with VALUES clause</summary>
		Insert,
		/// <summary>DML: Update</summary>
		Update,

		/// <summary>DDL: Delete</summary>
		Delete,
		/// <summary>DDL: Create Table</summary>
		CreateTable,
		/// <summary>DDL: Drop Table</summary>
		DropTable,

		/// <summary>QA: Open File</summary>
		OpenFile,
		/// <summary>QA: Open Database</summary>
		OpenDatabase,
		/// <summary>QA: Save DataSet</summary>
		SaveDataSet,
		/// <summary>QA: Save DataTable</summary>
		SaveDataTable,
		/// <summary>QA: Commit</summary>
		Commit,
		/// <summary>QA: Rollback</summary>
		Rollback,
		/// <summary>QA: Clear</summary>
		Clear,
		/// <summary>QA: List</summary>
		List,
		/// <summary>QA: View DataSet</summary>
		ViewDataSet,
	}

	/// <summary>
	/// SQL Query parser, supporting major DML statements.
	/// </summary>
	public static class QueryParser
	{
		private static Dictionary<QAStatementType, string[]> Clauses;
		private static Dictionary<QAStatementType, string[]> RequiredClauses;

		static QueryParser()
		{
			RegisterStatements();
		}

		private static void RegisterStatements()
		{
			Clauses = new Dictionary<QAStatementType, string[]>();
			RequiredClauses = new Dictionary<QAStatementType, string[]>();

			//DataSet ds = GetStatementsDS("ReferenceDS");
			DataSet ds = GetStatementsDS("crudwork.DataSetTools.Resources.References.xml");

			if (ds == null)
				throw new ArgumentNullException("ds");

			DataTable dt = ds.Tables["Statement"];

			if (dt == null)
				throw new ArgumentNullException("dt");

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];

				QAStatementType key = (QAStatementType)Enum.Parse(typeof(QAStatementType), dr["Key"].ToString());
				string[] clauses = dr["Value"].ToString().Split('|');

				string[] required = null;
				string reqStr = dr["Required"].ToString();
				if (string.IsNullOrEmpty(reqStr) || reqStr == "*")
					required = clauses;
				else
					required = reqStr.Split('|');

				Clauses.Add(key, clauses);
				RequiredClauses.Add(key, required);
			}
		}

		/// <summary>
		/// Retrieve Statements DataSet from Resource
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static DataSet GetStatementsDS(string key)
		{
			//string value = ConfigurationManager.AppSettings[key];
			string value = key;

			using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(key))
			{
				DataSet ds = new DataSet();
				ds.ReadXml(s);
				return ds;
			}
		}

		/// <summary>
		/// Parse the SQL string and return a Statement
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static IStatement Parse(string value)
		{
			IStatement result = null;

			foreach (KeyValuePair<QAStatementType, string[]> kv in Clauses)
			{
				string[] splitters = kv.Value;

				if (!value.StartsWith(splitters[0], StringComparison.InvariantCultureIgnoreCase))
					continue;

				List<KeyValue> clauses = ParseStatement(value, splitters);

				CheckRequiredClause(clauses, kv.Key);

				#region Return strong-type statements
				switch (kv.Key)
				{
					#region DML Statements
					case QAStatementType.Select:
						result = new SelectStatement(clauses);
						break;
					case QAStatementType.Insert:
						result = new InsertStatement(clauses);
						break;
					case QAStatementType.Update:
						result = new UpdateStatement(clauses);
						break;
					#endregion

					#region DDL Statements
					case QAStatementType.Delete:
						result = new DeleteStatement(clauses);
						break;
					case QAStatementType.CreateTable:
						result = new CreateTableStatement(clauses);
						break;
					case QAStatementType.DropTable:
						result = new DropTableStatement(clauses);
						break;
					#endregion

					#region QueryAnything Statements
					case QAStatementType.OpenFile:
						result = new OpenStatement(clauses);
						break;
					case QAStatementType.OpenDatabase:
						result = new OpenDatabaseStatement(clauses);
						break;
					case QAStatementType.SaveDataSet:
						result = new SaveDataSetStatement(clauses);
						break;
					case QAStatementType.SaveDataTable:
						result = new SaveDataTableStatement(clauses);
						break;
					case QAStatementType.Commit:
						result = new CommitStatement(clauses);
						break;
					case QAStatementType.Rollback:
						result = new RollbackStatement(clauses);
						break;
					case QAStatementType.Clear:
						result = new ClearStatement(clauses);
						break;
					case QAStatementType.List:
						result = new ListStatement(clauses);
						break;
					case QAStatementType.ViewDataSet:
						result = new ViewDataSet(clauses);
						break;
					#endregion

					default:
						throw new ArgumentOutOfRangeException("key=" + kv.Key);
				}
				#endregion

				if (result != null)
					break;
			}
			return result;
		}

		/// <summary>
		/// Parse the Statement and return KeyValue list
		/// </summary>
		/// <param name="value"></param>
		/// <param name="splitters"></param>
		/// <returns></returns>
		private static List<KeyValue> ParseStatement(string value, string[] splitters)
		{
			List<KeyValue> results = new List<KeyValue>();
			string[] tokens = Common.SplitClause(value, splitters);

			for (int i = 0; i < tokens.Length; i++)
			{
				string token = tokens[i];
				int idx = Common.Search(splitters, token);

				if (idx == -1)
					continue;

				string key = splitters[idx];
				string val = string.Empty;

				#region Get Value
				if (idx == splitters.Length - 1)
				{
					// read to end of line.
					val = Common.Merge(tokens, i + 1);
				}
				else
				{
					int nextToken = -1;
					for (int j = idx + 1; nextToken == -1 && j < splitters.Length; j++)
					{
						nextToken = Common.Search(tokens, splitters[j]);
					}

					if (nextToken == -1)
					{
						val = Common.Merge(tokens, i + 1);
					}
					else if (i < nextToken)
					{
						val = Common.Merge(tokens, i + 1, nextToken - 1);
						i = nextToken - 1;
					}
				}
				#endregion

				results.Add(new KeyValue(key, val));
			}

			return results;
		}

		/// <summary>
		/// check for required clauses
		/// </summary>
		/// <param name="clauses"></param>
		/// <param name="type"></param>
		private static void CheckRequiredClause(List<KeyValue> clauses, QAStatementType type)
		{
			string[] clauseNames = RequiredClauses[type];

			for (int i = 0; i < clauseNames.Length; i++)
			{
				string clause = clauseNames[i];
				bool found = false;

				foreach (KeyValue kv in clauses)
				{
					if (clause.Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase))
					{
						found = true;
						break;
					}
				}

				if (found)
					continue;

				string err = string.Format("A required clause '{0}' not found", clause);
				throw new ArgumentException(err);
			}
		}

		/// <summary>
		/// Parse a SET clause
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static List<KeyValue> ParseSet(string value)
		{
			try
			{
				List<KeyValue> results = new List<KeyValue>();

				if (string.IsNullOrEmpty(value))
					return results;

				string[] tokens = Common.SplitTrim(value, StringSplitOptions.None, ",");

				for (int i = 0; i < tokens.Length; i++)
				{
					string token = tokens[i];
					string[] pair = Common.SplitTrim(token, StringSplitOptions.None, "=");
					if (pair.Length != 2)
						throw new ArgumentException("expected 2 tokens, but found " + pair.Length + "(token=" + token + ")");

					results.Add(new KeyValue(pair[0], Common.Unqoute(pair[1])));
				}

				return results;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "value", value);
				throw;
			}
		}

		/// <summary>
		/// Parse a CREATE TABLE clause
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DataTable ParseCreateTable(string value)
		{
			DataTable dt = new DataTable();
			if (string.IsNullOrEmpty(value))
				return dt;

			int idx = value.IndexOf(' ');
			if (idx == -1)
				throw new ArgumentException("space expected between table and column defintion");

			string table = value.Substring(0, idx).Trim();
			string[] defs = Common.SplitTrim(Common.EatParenthesis(value.Substring(idx + 1).Trim()), StringSplitOptions.None, ",");

			dt.TableName = table;

			for (int i = 0; i < defs.Length; i++)
			{
				dt.Columns.Add(CreateDataColumn(defs[i]));
			}

			return dt;
		}

		private static DataColumn CreateDataColumn(string value)
		{
			string[] tokens = Common.SplitTrim(value, StringSplitOptions.RemoveEmptyEntries, " ");
			string columnName = Common.Unqoute(tokens[0]);
			Type dataType = typeof(string);
			string expr = string.Empty;
			MappingType type = MappingType.Element;

			return new DataColumn(columnName, dataType, expr, type);
		}
	}
}
