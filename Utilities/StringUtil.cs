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
using System.Security;
using System.Runtime.InteropServices;

namespace crudwork.Utilities
{
	/// <summary>
	/// String Utility
	/// </summary>
	public static class StringUtil
	{
		#region ---- SecureString conversions methods ----
#if !SILVERLIGHT
		/// <summary>
		/// convert a string into a SecureString type.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static SecureString StringToSecureString(string p)
		{
			SecureString ss = new SecureString();
			for (int i = 0; i < p.Length; i++)
			{
				ss.AppendChar(p[i]);
			}

			// lock the password!
			ss.MakeReadOnly();
			return ss;
		}

		/// <summary>
		/// convert a SecureString type into a string
		/// </summary>
		/// <param name="ss"></param>
		/// <returns></returns>
		public static string SecureStringToString(SecureString ss)
		{
			IntPtr bstr = Marshal.SecureStringToBSTR(ss);

			try
			{
				// WARNING: this is too revealing.
				return bstr.ToString();
			}
			finally
			{
				Marshal.ZeroFreeBSTR(bstr);
			}
		} 
#endif
		#endregion

		#region ---- String conversions methods ----
		/// <summary>
		/// convert string array into a string
		/// </summary>
		/// <param name="values"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static string StringArrayToString(IEnumerable<string> values, string delimiter)
		{
			StringBuilder results = new StringBuilder();

			foreach (var item in values)
			{
				if (results.Length > 0)
					results.Append(delimiter);

				results.Append(item);
			}

			return results.ToString();
		}

		/// <summary>
		/// Convert a string array to a string
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static string StringArrayToString(IEnumerable<string> values)
		{
			return StringArrayToString(values, Environment.NewLine);
		}

		/// <summary>
		/// convert a string (with newline) into string array.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string[] StringToStringArray(string value)
		{
			return value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
		}

#if !SILVERLIGHT
		/// <summary>
		/// Convert a byte array to a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[Obsolete("Consider using Encoding.ASCII.GetString(bytes[])", false)]
		public static string BytesToString(byte[] value)
		{
			return Encoding.ASCII.GetString(value);
			//StringBuilder s = new StringBuilder(value.Length);

			//for (int i = 0; i < value.Length; i++)
			//{
			//    s.Append((char)value[i]);
			//}

			//return s.ToString();
		}

		/// <summary>
		/// Convert a string to a byte array
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[Obsolete("Consider using Encoding.ASCII.GetBytes(string)", false)]
		public static byte[] StringToBytes(string value)
		{
			return Encoding.ASCII.GetBytes(value);
			//byte[] results = new byte[value.Length];

			//for (int i = 0; i < value.Length; i++)
			//{
			//    results[i] = (byte)value[i];
			//}

			//return results;
		}
#endif
		#endregion

		#region Quote/Unquote String Methods
		/// <summary>
		/// Remove both single-quotes or double-quotes that surround the
		/// input entry.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Unquote(string input)
		{
			return Unquote(input, true, '"', '\'');
		}

		/// <summary>
		/// Remove the quotes that surround the input entry.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="exact"></param>
		/// <param name="delim"></param>
		/// <returns></returns>
		public static string Unquote(string input, bool exact, params char[] delim)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			char firstChar = input[0];
			char lastChar = input[input.Length - 1];

			if ((CharUtil.ContainsChar(firstChar, delim)) &&
				(CharUtil.ContainsChar(lastChar, delim)) &&
				(firstChar == lastChar))
				return input.Substring(1, input.Length - 2);
			else
				return input;
		}

		/// <summary>
		/// apply double quotes around the input value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Quote(string value)
		{
			return String.Format("\"{0}\"", value.Trim(' ', '\t'));
		}
		#endregion

		#region Escape / Unescape Methods
		/// <summary>
		/// Escape a given value
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Escape(string input)
		{
			StringBuilder s = new StringBuilder();

			for (int i = 0; i < input.Length; i++)
			{
				s.AppendFormat(@"\{0}", input[i]);
			}

			return s.ToString();
		}

		/// <summary>
		/// Unescape a given value
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Unescape(string input)
		{
			StringBuilder s = new StringBuilder();

			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];

				if (c == '\\')
				{
					if (i + 1 < input.Length)
					{
						char c2 = input[++i];

						#region Translate escape sequence
						// http://www.wilsonmar.com/1eschars.htm
						switch (c2)
						{
							case 'b':
								s.Append("\b");
								break;
							case 't':
								s.Append("\t");
								break;
							case 'n':
								s.Append("\n");
								break;
							case 'v':
								s.Append("\v");
								break;
							case 'f':
								s.Append("\f");
								break;
							case 'r':
								s.Append("\r");
								break;
							case '\\':
								s.Append("\\");
								break;
							case '\'':
								s.Append("\'");
								break;
							case '"':
								s.Append(@"\""");
								break;
							case '0':
								s.Append("\0");
								break;
							//case 'c':				// continuation (UNIX)
							//    s.Append("\c");
							//    break;
							case 'a':
								s.Append("\a");
								break;
							default:
								s.Append(c2);
								break;
						}
						#endregion
					}
					else
					{
						string s2 = String.Format("Bad escape char at pos {0}: {1}", i, input);
						throw new Exception(s2);
					}
				}
				else
				{
					s.Append(c);
				}
			}

			return s.ToString();
		}
		#endregion

		#region Simple String Parsers
		/// <summary>
		/// A simply parser.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="delimChar"></param>
		/// <returns></returns>
		public static string[] Parse(string input, params char[] delimChar)
		{
			#region Sanity Checks
			if (String.IsNullOrEmpty(input))
				return new string[0];
			#endregion

			return input.Split(delimChar);
		}

		/// <summary>
		/// Another simple parser, with observation of escape characters.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="delimChar"></param>
		/// <returns></returns>
		public static string[] Parse2(string input, char[] delimChar)
		{
			#region Sanity Checks
			if (String.IsNullOrEmpty(input))
				return new string[0];
			#endregion

			/* Smart Advice: Parsing CSV is horrible!
			 *
			 * This method can parse CSV lines that were created
			 * by the !!! 
			 * The data fields are assumed
			 */

			List<string> result = new List<string>();
			StringBuilder s = new StringBuilder();

			for (int pos = 0; pos < input.Length; pos++)
			{
				char c = input[pos];

				if (c == '\\')
				{
					// unescape char...
					if (pos + 1 < input.Length)
					{
						pos++;
						s.Append(input[pos]);
					}
					else
					{
						string s2 = String.Format("Bad escape char at pos {0}: {1}",
							pos, input);
						throw new Exception(s2);
					}
				}
				else if (CharUtil.ContainsChar(c, delimChar))
				{
					// break here
					result.Add(s.ToString());
					s.Length = 0;
				}
				else
				{
					// append char
					s.Append(c);
				}
			}

			if (s.Length > 0)
				result.Add(s.ToString());

			return result.ToArray();
		}
		#endregion

		#region Character Conversion Methods
#if !SILVERLIGHT
		private static ProperCasingVB.StringConversion stringConversion = null;

		/// <summary>
		/// Convert given value to proper case
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToProper(string value)
		{
			if (stringConversion == null)
				stringConversion = new ProperCasingVB.StringConversion();
			return stringConversion.ProperCase(value);
		} 
#endif

		/// <summary>
		/// Convert given string array to upper case
		/// </summary>
		/// <param name="array"></param>
		public static void ToUpper(string[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].ToUpper();
			}
		}
		#endregion

		#region Other methods
		/// <summary>
		/// split the string into array using delimiter tokens.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="delim"></param>
		/// <returns></returns>
		public static string[] SplitDelimiter(string value, params char[] delim)
		{
			return value.Split(delim);
		}

		/// <summary>
		/// merge prefix and suffix values with delim as the separater.
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="suffix"></param>
		/// <param name="delim"></param>
		/// <returns></returns>
		public static string Merge(string prefix, string suffix, string delim)
		{
			return prefix + delim + suffix;
		}

		/// <summary>
		/// Concatenate a list of string array and remove leading/trailing whitespaces.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public static string Concatenate(string delimiter, params string[] arguments)
		{
			if (arguments == null || arguments.Length == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < arguments.Length; i++)
			{
				if (sb.Length>0)
					sb.Append(delimiter);
				sb.Append(arguments[i].Trim(' ', '\t'));
			}

			return sb.ToString().Trim(' ', '\t');
		}

		/// <summary>
		/// Concatenate a list of string array with format string and remove leading/trailing whitespaces.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <param name="format"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public static string Concatenate(string delimiter, string format, params string[] arguments)
		{
			if (arguments == null || arguments.Length == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < arguments.Length; i++)
			{
				if (sb.Length > 0)
					sb.Append(delimiter);
				sb.AppendFormat(format, arguments[i].Trim(' ', '\t'));
			}

			return sb.ToString().Trim(' ', '\t');
		}

		/// <summary>
		/// fill a string with a char with the specified length.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string FillChar(char c, int length)
		{
			StringBuilder s = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				s.Append(c);
			}
			return s.ToString();
		}

		/// <summary>
		/// Return str padded with trailing blanks to the given length.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Pad(string str, int length)
		{
			return Pad(str, length, ' ');
		}

		/// <summary>
		/// return the input string with trailing char to the given length
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <param name="padChar"></param>
		/// <returns></returns>
		public static String Pad(String str, int length, char padChar)
		{
			if (padChar == '\0')
				throw new ArgumentException("padChar cannot be null");

			StringBuilder sb = new StringBuilder(length);
			sb.Append(str);
			for (int i = str.Length; i < length; ++i)
			{
				sb.Append(padChar);
			}
			return sb.ToString();
		}

		/// <summary>
		/// generate a random key.
		/// </summary>
		/// <returns></returns>
		public static string RandomKey()
		{
			Random r = new Random();
			return String.Format("{0}", r.Next(int.MaxValue).ToString());
		}

		/// <summary>
		/// copy a range of items from List to a array
		/// </summary>
		/// <param name="input"></param>
		/// <param name="fromIndex"></param>
		/// <param name="toIndex"></param>
		/// <returns></returns>
		public static string[] CopyList(List<string> input, int fromIndex, int toIndex)
		{
			#region Sanity Checks
			if (fromIndex < 0)
				throw new ArgumentOutOfRangeException("range is too low: fromIndex=" + fromIndex);

			if (toIndex >= input.Count)
				throw new ArgumentOutOfRangeException("range is too high: toIndex=" + toIndex);

			if (fromIndex > toIndex)
				throw new ArgumentOutOfRangeException("error: ! fromIndex < toIndex");
			#endregion

			List<string> results = new List<string>();

			for (int i = fromIndex; i <= toIndex; i++)
			{
				results.Add(input[i]);
			}

			return results.ToArray();
		}

		/// <summary>
		/// Remove the '\0' null characters
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string RemoveNulls(string value)
		{
			return value.Replace("\0", "");
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
			if (string.IsNullOrEmpty(value))
				return new string[0];

			string[] result = value.Split(delim, option);

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = result[i].Trim(' ', '\t');
			}

			return result;
		}

		/// <summary>
		/// Trim leading and trailing whitespaces
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string TrimWhitespace(string value)
		{
			return value.Trim(' ', '\t', '\n', '\r');
		}

		/// <summary>
		/// Return a string value in reverse order
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Reverse(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			var stack = new Stack<string>(value.Length);
			var sb = new StringBuilder(value.Length);

			for (int i = 0; i < value.Length; i++)
				stack.Push(value.Substring(i, 1));

			for (int i = 0; i < value.Length; i++)
				sb.Append(stack.Pop());

			return sb.ToString();
		}

		#endregion

		#region Search methods
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
		#endregion
	}
}
