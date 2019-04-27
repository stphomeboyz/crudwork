// QueryAnything: Statements-DDL.cs
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
	#region DDL Statements
	/// <summary>
	/// Create table statement
	/// </summary>
	public class CreateTableStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public CreateTableStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.CreateTable)
		{
		}
		#endregion

		#region Properties
		private string TableName
		{
			get
			{
				string value = Common.Unqoute(base.GetValue("create table"));

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("table name not specified");
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
			DataTable dt = QueryParser.ParseCreateTable(TableName);
			base.DataSet.Tables.Add(dt);
			return QueryResult.Empty;
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}

	/// <summary>
	/// Drop table statement
	/// </summary>
	public class DropTableStatement : Statement, IStatement
	{
		#region Constructors
		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="clauses"></param>
		public DropTableStatement(List<KeyValue> clauses)
			: base(clauses, QAStatementType.DropTable)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the tablename
		/// </summary>
		private string TableName
		{
			get
			{
				string value = base.GetValue("drop table");

				#region Sanity Checking
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("table name not specified");
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
			ds.Tables.Remove(TableName);

			return new QueryResult("Table dropped");
		}

		QueryResult IStatement.Run()
		{
			return Run();
		}
		#endregion
	}
	#endregion
}
