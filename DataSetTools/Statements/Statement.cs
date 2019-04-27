// QueryAnything: Statement.cs
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
	#region Statement Base Class

	/// <summary>
	/// Common functionality of a SQL Statement
	/// </summary>
	public abstract class Statement : IStatement
	{
		private DataSet ds;

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="items"></param>
		/// <param name="statementType"></param>
		public Statement(List<KeyValue> items, QAStatementType statementType)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			this.Items = items;
			this.StatementType = statementType;
		}
		#endregion

		#region Indexer
		/// <summary>
		/// Get or set a KeyValue item
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public KeyValue this[int index]
		{
			get
			{
				return Items[index];
			}
			set
			{
				Items[index] = value;
			}
		}

		/// <summary>
		/// Get the number of items
		/// </summary>
		public int Count
		{
			get
			{
				return Items.Count;
			}
		}
		#endregion

		/// <summary>
		/// Get the list of KeyValues
		/// </summary>
		public List<KeyValue> Items
		{
			get;
			private set;
		}

		/// <summary>
		/// Get the QueryAnything statement type
		/// </summary>
		public QAStatementType StatementType
		{
			get;
			private set;
		}

		#region Querying Statement
		/// <summary>
		/// Return the Value of the given Key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetValue(string key)
		{
			foreach (KeyValue kv in Items)
			{
				if (kv.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
					return kv.Value;
			}
			return string.Empty;
		}

		/// <summary>
		/// Return true if the key specify existed
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			foreach (KeyValue kv in Items)
			{
				if (kv.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}
			return false;
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Get the DataTable from the DataSet
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		protected DataTable GetDataTable(string tablename)
		{
			if (!ds.Tables.Contains(tablename))
				throw new ArgumentException("Tablename not exist: " + tablename);
			return ds.Tables[tablename];
		}

		/// <summary>
		/// Create a new DataView for the tablename
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		protected DataView GetDataView(string tablename)
		{
			int found = 0;

			if (ds == null)
				throw new ArgumentException("DataSet is empty");

			for (int i = 0; i < ds.Tables.Count; i++)
			{
				string tblName = ds.Tables[i].TableName;
				if (tablename.Equals(tblName, StringComparison.InvariantCultureIgnoreCase))
					found++;
			}

			if (found == 0)
				throw new ArgumentException("Tablename not exist: " + tablename);

			return new DataView(ds.Tables[tablename]);
		}

		/// <summary>
		/// Return a DataView with the given criteria
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="where"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		protected DataView Filter(string tablename, string where, string orderBy)
		{
			DataView dv = GetDataView(tablename);

			dv.Sort = orderBy;
			dv.RowFilter = where;

			//dv.RowStateFilter = DataViewRowState.CurrentRows;

			return dv;
		}

		/// <summary>
		/// Update the DataRow with the given values.
		/// </summary>
		/// <param name="dataRow"></param>
		/// <param name="dataValues"></param>
		protected void UpdateRow(DataRow dataRow, List<KeyValue> dataValues)
		{
			foreach (KeyValue kv in dataValues)
			{
				dataRow[kv.Key] = kv.Value;
			}
		}
		#endregion

		/// <summary>
		/// Return a string representation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder s = new StringBuilder();

			if (Items != null)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (s.Length > 0)
						s.Append(" ");
					s.Append(Items[i].ToString());
				}
			}

			return s.ToString();
		}

		#region IStatement Members
		/// <summary>
		/// Get or set the DataSet associated with the statement
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				if (this.ds == null)
					this.ds = new DataSet();
				return this.ds;
			}
			set
			{
				this.ds = value;
			}
		}
		#endregion

		#region IStatement Members

		QueryResult IStatement.Run()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		DataSet IStatement.DataSet
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

		QAStatementType IStatement.StatementType
		{
			get
			{
				return StatementType;
			}
		}

		#endregion
	}
	#endregion
}
