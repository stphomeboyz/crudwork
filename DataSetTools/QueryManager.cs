// QueryAnything: QueryManager.cs
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

namespace crudwork.DataSetTools
{
	/// <summary>
	/// Query manager for parsing and executing SQL statements against file-based data, such as XML file, CSV file, etc...
	/// </summary>
	public class QueryManager
	{
		//private string filename;
		private DataSet ds;

		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="ds"></param>
		public QueryManager(DataSet ds)
		{
			this.ds = ds;
		}

		/// <summary>
		/// Create an empty instance
		/// </summary>
		public QueryManager()
			: this(new DataSet())
		{
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Parse the query statement and return the instance implementing the IStatement interface
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public IStatement Parse(string query)
		{
			return QueryParser.Parse(query);
		}

		/// <summary>
		/// Run the SQL statement and return the results
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public QueryResult Run(string query)
		{
			#region Statement Samples
			/*
			 * SELECT * FROM table WHERE var=value;
			 * 
			 * INSERT INTO table (variable) values (values);
			 * 
			 * UPDATE table set var=value WHERE var=value;
			 * 
			 * DELETE table WHERE var=value;
			 * 
			 * */
			#endregion

			QueryResult results = new QueryResult();
			IStatement s = null;

			try
			{
				s = QueryParser.Parse(query);
				if (s == null)
					throw new ArgumentException("Invalid statement: " + query);

				s.DataSet = this.ds;
				results = s.Run();
				this.ds = s.DataSet;
			}
			catch (Exception ex)
			{
#if DEBUG
				results.ErrorText = ex.Message + "\r\n\r\n" + ex.ToString();
#else
				results.ErrorText = ex.Message;
#endif
			}

			results.Statement = s == null ? string.Empty : s.ToString();
			return results;
		}

		/// <summary>
		/// Run a list of SQL statements and return the list of results
		/// </summary>
		/// <param name="queries"></param>
		/// <returns></returns>
		public QueryResultSet Run(string[] queries)
		{
			QueryResultSet results = new QueryResultSet();

			for (int i = 0; i < queries.Length; i++)
			{
				QueryResult result = Run(queries[i]);
				results.Add(result);

				// should stop on error...
				if (!string.IsNullOrEmpty(result.ErrorText))
					break;
			}

			return results;
		}

		/// <summary>
		/// Run a batch of SQL statements and return the list of results
		/// </summary>
		/// <param name="queryBatch"></param>
		/// <returns></returns>
		public QueryResultSet RunBatch(string queryBatch)
		{
			List<string> q = new List<string>(Common.SplitTrim(queryBatch, StringSplitOptions.RemoveEmptyEntries, "\n", "\r\n"));
			Common.RemoveComments(q);

			// now parse the statements
			string a = String.Join(" ", q.ToArray());
			string[] queries = Common.SplitTrim(a, StringSplitOptions.RemoveEmptyEntries, ";");

			return Run(queries);
		}

		/// <summary>
		/// Undo changes
		/// </summary>
		public QueryResultSet Rollback()
		{
			ds.RejectChanges();

			QueryResultSet results = new QueryResultSet();
			results.Add(new QueryResult("Rollback"));
			return results;
		}

		/// <summary>
		/// Commit changes
		/// </summary>
		public QueryResultSet Commit()
		{
			ds.AcceptChanges();

			QueryResultSet results = new QueryResultSet();
			results.Add(new QueryResult("Commit"));
			return results;
		}
		#endregion

		#region Public properties
		/// <summary>
		/// Get or set the dataset for querying
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return this.ds;
			}
			set
			{
				this.ds = value;
			}
		}

		/// <summary>
		/// Get a bool value indicating the DataSet has been changed
		/// </summary>
		public bool HasChanges
		{
			get
			{
				return ds.HasChanges();
			}
		}
		#endregion
	}
}
