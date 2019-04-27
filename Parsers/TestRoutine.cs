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
	/// Test Routines
	/// </summary>
	public static class TestRoutine
	{
		/// <summary>
		/// Check all values are digits
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsDigits(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (Char.IsDigit(value[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Remove formatting characters
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		public static void RemoveFormatChar(StringBuilder s, string[] delimiter)
		{
			for (int i = 0; i < delimiter.Length; i++)
			{
				s.Replace(delimiter[i], "");
			}
		}

		/// <summary>
		/// Find next token
		/// </summary>
		/// <param name="s"></param>
		/// <param name="tokens"></param>
		/// <param name="pos"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static bool FindToken(StringBuilder s, string[] tokens, out int pos, out int len)
		{
			string buf = s.ToString();
			pos = -1;
			len = -1;

			for (int i = 0; i < tokens.Length; i++)
			{
				string token = tokens[i];
				int p = buf.IndexOf(token);
				if (p != -1)
				{
					pos = p;
					len = token.Length;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Get next token
		/// </summary>
		/// <param name="value"></param>
		/// <param name="pos"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static string GetNextToken(string value, ref int pos, char delimiter)
		{
			int pos2 = value.IndexOf(delimiter, pos);
			int len = pos2 - pos;

			string result;

			if (len == -1)
				result = value.Substring(pos);
			else
				result = value.Substring(pos, len);

			pos += len;

			return result;

		}
	}
}
