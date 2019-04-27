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
using System.Text.RegularExpressions;

namespace crudwork.DataSetTools
{
	/// <summary>
	/// This class contains a collection of reusable methods for processing repetitive tasks.
	/// </summary>
	public class Repetitive
	{
		private string fieldSeparator = " ";
		private string recordSeparator = "\r\n";

		#region Constructors
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public Repetitive()
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="fieldSeparator"></param>
		/// <param name="recordSeparator"></param>
		public Repetitive(string fieldSeparator, string recordSeparator)
		{
			this.fieldSeparator = fieldSeparator;
			this.recordSeparator = recordSeparator;
		}
		#endregion

		/// <summary>
		/// Get or set the field separator
		/// </summary>
		public string FieldSeparator
		{
			get
			{
				return fieldSeparator;
			}
			set
			{
				fieldSeparator = value;
			}
		}

		/// <summary>
		/// Get or set the record separator
		/// </summary>
		public string RecordSeparator
		{
			get
			{
				return recordSeparator;
			}
			set
			{
				recordSeparator = value;
			}
		}

		/// <summary>
		/// Convert data into the format line using the DataTable
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		public string Convert(DataTable dt, string line)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				sb.Append(Convert(dr, line) + recordSeparator);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Convert data into the format line using the DataRow
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		public string Convert(DataRow dr, string line)
		{
			StringBuilder sb = new StringBuilder(line);
			MatchCollection mc = Regex.Matches(line, @"\${1,2}[^ ]+", RegexOptions.None);

			for (int i = 0; i < mc.Count; i++)
			{
				Match m = mc[i];
				string var = line.Substring(m.Index, m.Length);
				string value = string.Empty;

				if (var.StartsWith("$$"))
					continue;

				int idx = -1;

				if (int.TryParse(var.Substring(1), out idx))
				{
					value = Arg(dr, idx);
				}
				else
				{
					value = Arg(dr, var.Substring(1));
				}

				sb.Replace(var, value);
			}
			return sb.ToString();
		}

		private string Arg(DataRow dr, string columnName)
		{
			return dr[columnName].ToString();
		}

		private string Arg(DataRow dr, int columnIndex)
		{
			StringBuilder sb = new StringBuilder();

			if (columnIndex == 0)
			{
				for (int i = 0; i < dr.Table.Columns.Count; i++)
				{
					if (sb.Length > 0)
						sb.Append(fieldSeparator);
					sb.Append(dr[i]);
				}
			}
			else
			{
				sb.Append(dr[columnIndex - 1]);
			}

			return sb.ToString();
		}
	}
}
