// QueryAnything: QueryResults.cs
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
	/// Object containing the query result, such as DataTable, TextResult and Error Text
	/// </summary>
	public class QueryResult
	{
		private string statement;
		private DataTable dataResult;
		private string textResult;
		private string errorText;
		private int rowAffected;

		#region Constructors
		/// <summary>
		/// Create an empty object
		/// </summary>
		public QueryResult()
			: this(null, null, null, 0)
		{
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="result"></param>
		public QueryResult(string result)
			: this(null, result, null, 0)
		{
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="result"></param>
		public QueryResult(DataTable result)
			: this(result, null, null, result.Rows.Count)
		{
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="dataResult"></param>
		/// <param name="textResult"></param>
		/// <param name="errorText"></param>
		/// <param name="rowAffected"></param>
		public QueryResult(DataTable dataResult, string textResult, string errorText, int rowAffected)
		{
			DataResult = dataResult;
			TextResult = textResult;
			ErrorText = errorText;
			RowAffected = rowAffected;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Return an empty QueryResult object
		/// </summary>
		public static QueryResult Empty
		{
			get
			{
				return new QueryResult();
			}
		}

		/// <summary>
		/// Get or set the row affected
		/// </summary>
		public int RowAffected
		{
			get
			{
				return this.rowAffected;
			}
			set
			{
				this.rowAffected = value;
			}
		}

		/// <summary>
		/// Get or set the DataTable result
		/// </summary>
		public DataTable DataResult
		{
			get
			{
				return this.dataResult;
			}
			set
			{
				this.dataResult = value;
			}
		}

		/// <summary>
		/// Get or set the text result
		/// </summary>
		public string TextResult
		{
			get
			{
				return this.textResult;
			}
			set
			{
				this.textResult = value;
			}
		}

		/// <summary>
		/// Get or set the error text
		/// </summary>
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		/// <summary>
		/// Get or set the statement that produce the result
		/// </summary>
		public string Statement
		{
			get
			{
				return this.statement;
			}
			set
			{
				this.statement = value;
			}
		}
		#endregion
	}

	/// <summary>
	/// List of query results
	/// </summary>
	public class QueryResultSet : List<QueryResult>
	{
	}
}

