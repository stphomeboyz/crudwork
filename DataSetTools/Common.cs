// QueryAnything: Common.cs
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

namespace crudwork.DataSetTools
{
	/// <summary>
	/// Common parsing methods
	/// </summary>
	public static class Common
	{
		/// <summary>
		/// Split a string by given clause tokens
		/// </summary>
		/// <param name="value"></param>
		/// <param name="splitters"></param>
		/// <returns></returns>
		public static string[] SplitClause(string value, string[] splitters)
		{
			string[] a = value.Split(new string[] { " ", "\t", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			return FixSpacesInToken(a, splitters);
		}

		/// <summary>
		/// Attempt to fix clauses with embedding spaces, such as "GROUP BY".
		/// </summary>
		/// <param name="a"></param>
		/// <param name="splitters"></param>
		/// <returns></returns>
		private static string[] FixSpacesInToken(string[] a, string[] splitters)
		{
			List<string[]> fixSpace = new List<string[]>();

			#region Build a list of splitters (with whitespaces) to fix...
			for (int i = 0; i < splitters.Length; i++)
			{
				string s = splitters[i];

				if (!Common.ContainsWhitespace(s))
					continue;

				fixSpace.Add(s.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries));
			}

			if (fixSpace.Count == 0)
				return a;
			#endregion

			bool fix = false;
			string[][] fixSpace2 = fixSpace.ToArray();

			for (int i = 0; i < a.Length; i++)
			{
				for (int j = 0; j < fixSpace2.Length; j++)
				{
					string[] fs = fixSpace2[j];

					if (!ContainsArray(a, fs, i))
						continue;

					string newValue = string.Empty;
					for (int k = 0; k < fs.Length; k++)
					{
						if (k > 0)
							newValue += " ";
						newValue += a[i + k];
						a[i + k] = string.Empty;
					}

					a[i] = newValue;
					fix = true;
				}
			}

			if (!fix)
				return a;

			List<string> b = new List<string>();
			for (int i = 0; i < a.Length; i++)
			{
				if (string.IsNullOrEmpty(a[i]))
					continue;
				b.Add(a[i]);
			}

			return b.ToArray();
		}

		/// <summary>
		/// return true if the values of a given array can be found within another array
		/// </summary>
		/// <param name="array"></param>
		/// <param name="lookFor"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public static bool ContainsArray(string[] array, string[] lookFor, int startIndex)
		{
			int c = 0;
			for (int i = startIndex; i < array.Length && c < lookFor.Length; i++)
			{
				if (!array[i].Equals(lookFor[c], StringComparison.InvariantCultureIgnoreCase))
					break;
				c++;
			}
			return c == lookFor.Length;
		}

		/// <summary>
		/// Merge an array of value starting from index
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="fromIndex"></param>
		/// <returns></returns>
		public static string Merge(string[] tokens, int fromIndex)
		{
			return Merge(tokens, fromIndex, tokens.Length - 1);
		}

		/// <summary>
		/// Merge an array of values starting from index to a given end-index
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="fromIndex"></param>
		/// <param name="toIndex"></param>
		/// <returns></returns>
		public static string Merge(string[] tokens, int fromIndex, int toIndex)
		{
			StringBuilder s = new StringBuilder();

			#region Sanity Checking
			if (fromIndex < 0)
				throw new ArgumentOutOfRangeException("fromIndex=" + fromIndex);
			if (toIndex >= tokens.Length)
				throw new ArgumentOutOfRangeException("toIndex=" + toIndex);
			#endregion

			for (int i = fromIndex; i <= toIndex; i++)
			{
				if (s.Length > 0)
					s.Append(" ");
				s.Append(tokens[i]);
			}

			return s.ToString();
		}

		private static string[] quotes = null;
		/// <summary>
		/// Remove the single quotes, double quotes, or square quotes surrounding the value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Unqoute(string value)
		{
			if (quotes == null)
			{
				quotes = new string[] {
					"'", "'",
					"\"", "\"",
					"[", "]",
				};
			}

			StringBuilder sb = new StringBuilder(value);
			for (int i = 0; i < quotes.Length; i += 2)
			{
				string s1 = quotes[i];
				string s2 = quotes[i + 1];

				if (value.StartsWith(s1) && value.EndsWith(s2))
					value = value.Substring(1, value.Length - 2);
			}

			return value;
		}

		/// <summary>
		/// Get the surrounding parenthesis
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string EatParenthesis(string value)
		{
			if (!value.StartsWith("(") || !value.EndsWith(")"))
				throw new ArgumentException("Surrounding parenthesis not found: " + value);

			// remove the parenthesis and trim whitespaces...
			return value.Substring(1, value.Length - 2).Trim(' ', '\t');
		}

		/// <summary>
		/// Split a string by given delimiters
		/// </summary>
		/// <param name="value"></param>
		/// <param name="option"></param>
		/// <param name="delimiters"></param>
		/// <returns></returns>
		public static string[] Split(string value, StringSplitOptions option, params string[] delimiters)
		{
			List<string> result = new List<string>(value.Split(delimiters, option));

			for (int i = 0; i < result.Count; i++)
			{
				result[i] = result[i].Trim(' ', '\t');
			}

			return result.ToArray();
		}

		/// <summary>
		/// Split the string by the given delimiter, trim whitespaces, and return array.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="option"></param>
		/// <param name="delim"></param>
		/// <returns></returns>
		public static string[] SplitTrim(string value, StringSplitOptions option, params string[] delim)
		{
			string[] result = value.Split(delim, option);

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = result[i].Trim(' ', '\t');
			}

			return result;
		}

		/// <summary>
		/// Search an array for a given value
		/// </summary>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		public static int Search(string[] array, string value, StringComparison compare)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value.Equals(array[i], compare))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Search an array for a given value
		/// </summary>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int Search(string[] array, string value)
		{
			return Search(array, value, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Return true if value contains one or more whitespaces
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ContainsWhitespace(string value)
		{
			return value.Contains(" ") || value.Contains("\t");
		}

		/// <summary>
		/// Remove comments from input buffer
		/// </summary>
		/// <param name="buffer"></param>
		public static void RemoveComments(List<string> buffer)
		{
			for (int i = 0; i < buffer.Count; i++)
			{
				string newValue = RemoveComments(buffer[i]);

				if (string.IsNullOrEmpty(newValue))
				{
					buffer.RemoveAt(i);
					i--;
					continue;
				}

				buffer[i] = newValue;
			}
		}

		private static string RemoveComments(string query)
		{
			if (!query.Contains("--"))
				return query;

			int idx = query.IndexOf("--");
			return query.Substring(0, idx);
		}
	}

}
