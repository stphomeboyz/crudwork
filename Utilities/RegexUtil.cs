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
using System.Text.RegularExpressions;

namespace crudwork.Utilities
{
	/// <summary>
	/// Regular Expression Utility
	/// </summary>
	public static class RegexUtil
	{
		/// <summary>
		/// Global substitution every match of regular expression
		/// in input is replaced by replacement string.
		/// </summary>
		/// <param name="input">input string</param>
		/// <param name="expression">search for regular expression</param>
		/// <param name="replacement">replace with this text</param>
		/// <returns></returns>
		public static string Sub(string input, string expression, string replacement)
		{
			var sb = new StringBuilder(input);
			MatchCollection mc = Regex.Matches(input, expression);

			for (int i = 0; i < mc.Count; i++)
			{
				Match m = mc[i];
				sb.Replace(m.Value, replacement);
			}

			return sb.ToString();
		}
	}
}
