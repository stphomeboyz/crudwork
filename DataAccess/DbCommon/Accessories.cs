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
using crudwork.DataAccess;
using System.Data;
using crudwork.Models.DataAccess;
using crudwork.DynamicRuntime;

namespace crudwork.DataAccess.Common
{
	/// <summary>
	/// Utility common to any database
	/// </summary>
	internal static class Accessories
	{
		/// <summary>
		/// escape single quotes
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string DbQuote(string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Replace("'", "''");
		}

		/// <summary>
		/// create filtered query
		/// </summary>
		/// <param name="value"></param>
		/// <param name="queryFilter"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public static string CreateQueryFilter(string value, QueryFilter queryFilter, string columnName)
		{
			string condition = string.Empty;

			if (!string.IsNullOrEmpty(value))
			{
				switch (queryFilter)
				{
					case QueryFilter.Contains:
						condition = columnName + " like '%{0}%'";
						break;
					case QueryFilter.EndsWith:
						condition = columnName + " like '%{0}'";
						break;
					case QueryFilter.StartsWith:
						condition = columnName + " like '{0}%'";
						break;
					case QueryFilter.Exact:
						condition = columnName + " = '{0}'";
						break;
					case QueryFilter.None:
						// bypass normal procedure, and return empty string
						return string.Empty;
					default:
						throw new ArgumentOutOfRangeException("queryFilter=" + queryFilter);
				}
			}

			return String.Format(condition, DbQuote(value));
		}

		/// <summary>
		/// Convert or remove any character that are not supported in Column name.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string MakeSafeColumnName(string value)
		{
			return value;
		}

		/// <summary>
		///  return a list of columns considered as primary column from the DataTable instance
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string[] GetPrimaryColumns(DataTable dt)
		{
			List<string> results = new List<string>();

			for (int i = 0; i < dt.Columns.Count; i++)
			{
				// AutoIncrement or Unique will be considered as Primary Column
				if (dt.Columns[i].AutoIncrement || dt.Columns[i].Unique)
					results.Add(dt.Columns[i].ColumnName);
			}

			results.Sort();
			return results.ToArray();
		}
	}
}
