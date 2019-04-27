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

namespace crudwork.DataAccess
{
	/// <summary>
	/// Return a list of non-stored proc database commands
	/// </summary>
	public class DBTextCommandList : List<string>
	{
		private bool changed = false;
		private string _regexString = string.Empty;

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="delimitedListOfTextCommands"></param>
		public DBTextCommandList(string delimitedListOfTextCommands)
		{
			AddList(delimitedListOfTextCommands);
		}

		/// <summary>
		/// Add a list of text commands separated by a pipe '|' character
		/// </summary>
		/// <param name="delimitedListOfTextCommands"></param>
		public void AddList(string delimitedListOfTextCommands)
		{
			string[] tokens = delimitedListOfTextCommands.Split('|');
			for (int i = 0; i < tokens.Length; i++)
			{
				Add(tokens[i]);
			}
		}

		/// <summary>
		/// Add new item to list
		/// </summary>
		/// <param name="item"></param>
		public new void Add(string item)
		{
			base.Add(item);
			if (!changed)
				changed = true;
		}

		private string CreateRegexString()
		{
			if (!changed && !string.IsNullOrEmpty(_regexString))
				return _regexString;

			StringBuilder sb = new StringBuilder();
			sb.Append("^(");

			for (int i = 0; i < base.Count; i++)
			{
				if (i > 0)
					sb.Append("|");
				sb.Append(base[i]);
			}

			sb.Append(")\\s");
			return _regexString = sb.ToString();
		}

		/// <summary>
		/// Return the CommandType
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public CommandType GetCommandType(string value)
		{
			value = value.Trim(' ', '\t', '\r', '\n');
			string regex = CreateRegexString();
			return Regex.IsMatch(value, regex, RegexOptions.IgnoreCase) ? CommandType.Text : CommandType.StoredProcedure;
		}
	}
}
