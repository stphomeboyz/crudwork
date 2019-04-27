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

namespace crudwork.Parsers
{
	/// <summary>
	/// Parser for the post method
	/// </summary>
	public static class PostBackParser
	{
		/// <summary>
		/// the delimiter
		/// </summary>
		public const char Delimiter = '|';

		/// <summary>
		/// parse a string into DataBlock used in Postback, similar to Ajax's post back.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string[] Parse(string value)
		{
			List<string> results = new List<string>();

			string token = string.Empty;
			int pos = 0;

			while (pos < value.Length)
			{
				int len = Convert.ToInt32(TestRoutine.GetNextToken(value, ref pos, Delimiter));

				// advance past the delimiter
				pos++;

				token = value.Substring(pos, len);
				pos += len;

				// advance past the delimiter
				pos++;

				#region Sanity Checks
				if (token.Length != len)
					throw new ArgumentOutOfRangeException("incorrect length");
				#endregion

				results.Add(token);
			}

			return results.ToArray();
		}
	}
}
