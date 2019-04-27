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
using System.Text;
using System.Data;
using System.ComponentModel;

namespace crudwork.Utilities
{
	/// <summary>
	/// DataRow Utility
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DataRowUtil<T>
	{
		#region Enumerators
		#endregion

		#region Fields
		private DataTable dataTable;
		private DataColumn dataColumn;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an empty instance
		/// </summary>
		public DataRowUtil()
		{
			this.dataTable = null;
			this.dataColumn = null;
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="columnName"></param>
		public DataRowUtil(DataTable dt, string columnName)
			: this(dt, dt.Columns[columnName])
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="dataColumn"></param>
		public DataRowUtil(DataTable dt, DataColumn dataColumn)
		{
			this.dataTable = dt;
			this.dataColumn = dataColumn;
		}
		#endregion

		#region Event methods

		#region System Event methods
		#endregion

		#region Application Event methods
		#endregion

		#region Custom Event methods
		#endregion

		#endregion

		#region Public methods
		/// <summary>
		/// Return a list of T from table's column
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="dataColumn"></param>
		/// <returns></returns>
		public T[] ToArray(DataTable dataTable, DataColumn dataColumn)
		{
			List<T> list = new List<T>();
			list.Clear();

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));

			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow dr = dataTable.Rows[i];
				string data = dr[dataColumn.ColumnName].ToString();

				T result = default(T);

				if (!String.IsNullOrEmpty(data))
				{
					result = (T)tc.ConvertTo(data, typeof(T));
				}

				list.Add(result);

			}

			return list.ToArray();
		}

		/// <summary>
		/// Return a list of T from given table's column
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			#region Sanity Checks
			if (this.dataTable == null)
				throw new ArgumentNullException("dataTable");
			if (this.dataColumn == null)
				throw new ArgumentNullException("dataColumn");
			#endregion

			return ToArray(this.dataTable, this.dataColumn);
		}
		#endregion

		#region Private methods
		#endregion

		#region Protected methods
		#endregion

		#region Property methods
		#endregion

		#region Others
		#endregion
	}
}
