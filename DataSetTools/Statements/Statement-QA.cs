// QueryAnything: Statements-QA.cs
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
using crudwork.DataSetTools.DataImporters;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;
using crudwork.FileImporters;

namespace crudwork.DataSetTools.Statements
{
	#region QueryAnything Statements
	/// <summary>
	/// Open File
	/// </summary>
	public class OpenStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public OpenStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.OpenFile)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the Filename
		/// </summary>
		private string Filename
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("open file"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("file name not specified");
				#endregion

				return value;
			}
		}

		/// <summary>
		/// Get the options to be pass to ImportManager
		/// </summary>
		private string Option
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("option"));

				#region Sanity Checking
				//if (string.IsNullOrEmpty(value))
				//	throw new ArgumentException("file name not specified");
				#endregion

				return value;
			}
		}

		/// <summary>
		/// Get the DataTable
		/// </summary>
		private string Table
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("as table"));

				#region Sanity Checking
				//if (string.IsNullOrEmpty(value))
				//	throw new ArgumentException("file name not specified");
				#endregion

				return value;
			}
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Load the DataSet with tables from filename.
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			//FileImporter fi = new FileImporter();
			//DataSet ds = fi.Import(Filename);
			var ds = ImportManager.Import(Filename, Option);

			if (ds == null || ds.Tables.Count == 0)
				throw new ArgumentException("No tables were imported");

			if (base.ContainsKey("as table"))
			{
				if (ds.Tables.Count > 1)
					throw new ArgumentException("Cannot use the 'AS TABLE' clause when input contains multiple tables");

				DataTable dt = ds.Tables[0].Copy();
				dt.TableName = Table;
				dt.AcceptChanges();
				base.DataSet.Tables.Add(dt);
			}
			else
			{
				base.DataSet = ds;
				base.DataSet.AcceptChanges();
			}

			return new QueryResult(ds.Tables.Count + " table(s) opened.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Open database connection string
	/// </summary>
	public class OpenDatabaseStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public OpenDatabaseStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.OpenDatabase)
		{
		}
		#endregion

		#region Properties
		// OPEN DATABASE|AS|PROVIDER|SELECT|AS TABLE
		private string ConnectionString
		{
			get
			{
				// open database sqadb1.msbocg.com/rct3_prd_nationwide as rct21/sqatest
				string[] database = SlashPair(base.GetValue("open database"));
				string[] userinfo = null;

				bool useIntegratedSecurity = string.IsNullOrEmpty(base.GetValue("as"));

				if (!useIntegratedSecurity)
					userinfo = SlashPair(base.GetValue("as"));
				else
					userinfo = new string[] { "", "" };

				ConnectionStringManager m = new ConnectionStringManager(
					database[0], database[1], 
					userinfo[0], userinfo[1], 
					useIntegratedSecurity);


				return m.ConnectionString;
			}
		}

		private string Query
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("select"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("select query not specified");
				#endregion

				return "select " + value;
			}
		}

		private string TableName
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("as table"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("table name not specified");
				#endregion

				return value;
			}
		}

		private DatabaseProvider Provider
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("provider"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("provider not specified");
				#endregion
				
				return Providers.Converter(value);
			}
		}

		#endregion

		#region Private methods
		private string[] SlashPair(string value)
		{
			string[] results = new string[2];

			if (!value.Contains("/"))
				throw new ArgumentException("SlashPair: a forward slash not found");

			int idx = value.IndexOf("/");

			results[0] = value.Substring(0, idx);
			results[1] = value.Substring(idx + 1);

			return results;
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Connect to the Database via connection string
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			DataFactory dataFactory = new DataFactory(Provider, ConnectionString);
			DataTable dt = dataFactory.FillTable(Query);
			dt.TableName = TableName;

			DataSet.Tables.Add(dt);

			return new QueryResult("1 table(s) opened.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Save File Statement
	/// </summary>
	public class SaveDataSetStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public SaveDataSetStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.SaveDataSet)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the Filename
		/// </summary>
		private string Filename
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("as file"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("file name not specified");
				#endregion

				return value;
			}
		}

		/// <summary>
		/// Get the options to be pass to ImportManager
		/// </summary>
		private string Option
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("option"));

				#region Sanity Checking
				//if (string.IsNullOrEmpty(value))
				//	throw new ArgumentException("file name not specified");
				#endregion

				return value;
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
			//FileImporter fi = new FileImporter();
			//fi.Export(base.DataSet, Filename);
			ImportManager.Export(DataSet, Filename, Option);
			base.DataSet.AcceptChanges();
			return new QueryResult("DataSet saved to disk.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Save File Statement
	/// </summary>
	public class SaveDataTableStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public SaveDataTableStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.SaveDataTable)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the Table
		/// </summary>
		private string Table
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("save table"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("table name not specified");
				#endregion

				return value;
			}
		}

		/// <summary>
		/// Get the Filename
		/// </summary>
		private string Filename
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("as file"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("file name not specified");
				#endregion

				return value;
			}
		}

		/// <summary>
		/// Get the options to be pass to ImportManager
		/// </summary>
		private string Option
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("option"));

				#region Sanity Checking
				//if (string.IsNullOrEmpty(value))
				//	throw new ArgumentException("file name not specified");
				#endregion

				return value;
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
			//FileImporter fi = new FileImporter();

			using (DataSet ds = new DataSet())
			using (DataTable dt = base.GetDataTable(Table).Copy())
			{
				ds.Tables.Add(dt);
				//fi.Export(ds, Filename);
				ImportManager.Export(ds, Filename, Option);
			}

			base.GetDataTable(Table).AcceptChanges();
			return new QueryResult("Table saved to disk.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Commit any changes
	/// </summary>
	public class CommitStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public CommitStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Commit)
		{
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			base.DataSet.AcceptChanges();

			return new QueryResult("DataSet committed.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Rollback any changes
	/// </summary>
	public class RollbackStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public RollbackStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Rollback)
		{
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			base.DataSet.RejectChanges();

			return new QueryResult("DataSet rolled back.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Clear all tables or clear the given table
	/// </summary>
	public class ClearStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public ClearStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.Clear)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the Table
		/// </summary>
		private string TableName
		{
			get
			{
				//return base.GetValue("table");
				string value = Common.Unqoute(base.GetValue("table"));

				#region Sanity Checking
				//if (string.IsNullOrEmpty(value))
				//	throw new ArgumentException("file name not specified");
				#endregion

				return value;
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
			DataSet ds = base.DataSet;
			int tablesAffected = 0;
			if (base.ContainsKey("table"))
			{
				ds.Tables.Remove(TableName);
				tablesAffected = 1;
			}
			else
			{
				tablesAffected = ds.Tables.Count;

				//while (ds.Relations.Count > 0)
				//	ds.Relations.RemoveAt(0);

				//while (ds.Tables.Count > 0)
				//	ds.Tables.RemoveAt(0);

				ds.Clear();
			}

			base.DataSet = new DataSet();

			// free up memory usage at this point.
			GC.Collect();
			GC.Collect();
			GC.Collect();

			return new QueryResult(tablesAffected + " tables cleared.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Rollback any changes
	/// </summary>
	public class ListStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public ListStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.List)
		{
		}
		#endregion

		#region Private methods

		private DataTable ListColumns()
		{
			DataTable result = new DataTable();
			result.Columns.Add("TableName", typeof(string));
			result.Columns.Add("ColumnName", typeof(string));
			result.Columns.Add("ColumnType", typeof(string));

			DataSet ds = base.DataSet;
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dt = ds.Tables[i];

				for (int j = 0; j < dt.Columns.Count; j++)
				{
					DataColumn dc = dt.Columns[j];
					result.Rows.Add(dt.TableName, dc.ColumnName, dc.DataType.Name);
				}
			}

			using (DataView dv = new DataView(result))
			{
				dv.Sort = "TableName, ColumnName";
				return dv.ToTable();
			}
		}

		private DataTable ListTables()
		{
			DataTable result = new DataTable();
			result.Columns.Add("TableName", typeof(string));
			result.Columns.Add("Rows", typeof(int));

			DataSet ds = base.DataSet;
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dt = ds.Tables[i];

				result.Rows.Add(dt.TableName, dt.Rows.Count);
			}

			using (DataView dv = new DataView(result))
			{
				dv.Sort = "TableName";
				return dv.ToTable();
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
			DataTable result = null;

			if (base.DataSet.Tables.Count == 0)
				throw new ArgumentException("DataSet is empty");

			if (base.ContainsKey("COLUMNS"))
				result = ListColumns();
			else // TABLES
				result = ListTables();

			return new QueryResult(result);
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// View the DataSet
	/// </summary>
	public class ViewDataSet : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public ViewDataSet(List<KeyValue> clauses)
			: base(clauses, QAStatementType.ViewDataSet)
		{
		}
		#endregion

		#region IStatement Members
		/// <summary>
		/// Execute the statement
		/// </summary>
		/// <returns></returns>
		public QueryResult Run()
		{
			return new QueryResult("View DataSet not yet implemented.");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}

		#endregion
	}
	#endregion
}
