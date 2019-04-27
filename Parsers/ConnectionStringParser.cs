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
using System.Linq;
using System.Text;
using crudwork.Utilities;

namespace crudwork.Parsers
{
	/// <summary>
	/// Connection String parser
	/// </summary>
	public static class ConnectionStringParser
	{
		private static string CleanKey(StringBuilder value)
		{
			return StringUtil.Unquote(value.ToString().Trim(' ', '\t'));
		}

		/// <summary>
		/// Parse a connection string value
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static Dictionary<string, string> Parse(string connectionString)
		{
			if (DataConvert.IsNull(connectionString))
				throw new ArgumentNullException("connectionString");

			// trim the excess leading/trailing whitespaces
			connectionString = connectionString.Trim();

			// Provider="Microsoft.Jet.OLEDB.4.0"; Data Source="c:\foo;bar.xls"; user id="admin"; password=""; Extended Properties="Excel 8.0; HDR=Yes; IMEX=1";
			var results = new Dictionary<string, string>();

			int pos = -1;
			int max = connectionString.Length;

			var key = new StringBuilder();
			var value = new StringBuilder();
			bool isKey = true;
			bool isQuote = false;

			while (++pos < max)
			{
				char c = connectionString[pos];

				// a key may be enclosed in quotes.
				if (c == '"')
					isQuote = !isQuote;

				#region handle the delimiters
				// we do not want to handle delimiters that are inside a quoted string
				if (!isQuote)
				{
					if (c == ';')
					{
						results.Add(CleanKey(key), CleanKey(value));
						key.Length = value.Length = 0;
						isKey = true;
						continue;
					}

					if (c == '=')
					{
						isKey = false;
						continue;
					}
				}
				#endregion

				if (isKey)
					key.Append(c);
				else
					value.Append(c);
			}

			// record the last entry.
			if (key.Length + value.Length > 0)
				results.Add(CleanKey(key), CleanKey(value));

			return results;
		}
	}
}
