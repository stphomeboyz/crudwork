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
using crudwork.Utilities;

namespace crudwork.Parsers
{
	/// <summary>
	/// Interface for parsing telephone
	/// </summary>
	public interface ITelephoneParser
	{
		/// <summary>
		/// Format the telephone
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		string Format(string format);

		/// <summary>
		/// Parse the telephone
		/// </summary>
		/// <param name="value"></param>
		void Parse(string value);
	}

	/// <summary>
	/// Parse international telephone number
	/// </summary>
	public class InternationalTelephone : ITelephoneParser
	{
		// International telephone: 011 84 61 883 003

		#region Fields
		/// <summary>
		/// the country code
		/// </summary>
		public string CountryCode;

		/// <summary>
		/// the local / city / province code
		/// </summary>
		public string ProvinceCode;

		/// <summary>
		/// the telephone number
		/// </summary>
		public string Phone;
		#endregion


		#region IFormatter Members

		string ITelephoneParser.Format(string format)
		{
			if (string.IsNullOrEmpty(format))
			{
				format = "011-{0}-{1}-{2}";
			}

			return string.Format(format,
				CountryCode,
				ProvinceCode,
				Phone);
		}

		void ITelephoneParser.Parse(string value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}

	/// <summary>
	/// Parse US telephone number
	/// </summary>
	public class Telephone : ITelephoneParser
	{
		private static string[] delimiters = new string[] { "(", ")", "-", ".", "/", "\\", " " };
		private static string[] extensionTokens = new string[] { "x", "ext", "extension" };

		#region Fields
		/// <summary>
		/// Area code
		/// </summary>
		public string Areacode;

		/// <summary>
		/// Prefix: 3 digits
		/// </summary>
		public string Prefix;

		/// <summary>
		/// Suffix: 4 digits
		/// </summary>
		public string Suffix;

		/// <summary>
		/// Extension number
		/// </summary>
		public string Extension;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="areacode"></param>
		/// <param name="prefix"></param>
		/// <param name="suffix"></param>
		/// <param name="extension"></param>
		public Telephone(string areacode, string prefix, string suffix, string extension)
		{
			this.Areacode = areacode;
			this.Prefix = prefix;
			this.Suffix = suffix;
			this.Extension = extension;
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="areacode"></param>
		/// <param name="prefix"></param>
		/// <param name="suffix"></param>
		public Telephone(string areacode, string prefix, string suffix)
			: this(areacode, prefix, suffix, string.Empty)
		{
		}

		/// <summary>
		/// Create an empty object
		/// </summary>
		public Telephone()
			: this(string.Empty, string.Empty, string.Empty, string.Empty)
		{
		}
		#endregion

		#region IFormatter Members

		string ITelephoneParser.Format(string format)
		{
			if (string.IsNullOrEmpty(format))
			{
				if (!string.IsNullOrEmpty(Areacode) &&
					!string.IsNullOrEmpty(Prefix) &&
					!string.IsNullOrEmpty(Suffix) &&
					!string.IsNullOrEmpty(Extension))
				{
					// got area + prefix + suffix + extension (everything)
					format = "({0:3}) {1:3}-{2:4} ext {3}";
				}
				else if (!string.IsNullOrEmpty(Areacode) &&
					!string.IsNullOrEmpty(Prefix) &&
					!string.IsNullOrEmpty(Suffix))
				{
					// got area + prefix + suffix.
					format = "({0:3}) {1:3}-{2:4}";
				}

				else if (!string.IsNullOrEmpty(Prefix) &&
					!string.IsNullOrEmpty(Suffix))
				{
					// got prefix + suffix
					format = "({0:3}) {1:3}-{2:4}";
				}
			}

			return string.Format(format,
				Areacode,
				Prefix,
				Suffix,
				Extension);
		}

		void ITelephoneParser.Parse(string value)
		{
			try
			{
				StringBuilder buffer = new StringBuilder(value);

				// remove format chars...
				TestRoutine.RemoveFormatChar(buffer, delimiters);

				if (!TestRoutine.IsDigits(buffer.ToString()))
				{
					// look for possible extension
					int pos = -1, len = -1;
					if (TestRoutine.FindToken(buffer, extensionTokens, out pos, out len))
					{
						// get the extension number.
						int pos2 = pos + len;
						string extension = buffer.ToString().Substring(pos2);
						this.Extension = extension;

						// remove everything past the extension token.
						buffer.Remove(pos, buffer.Length);
					}
				}

				string buf = buffer.ToString();
				switch (buf.Length)
				{
					case 11:
						if (buf[0] != '1')
							goto default;
						buf.Remove(0, 1);
						goto case 10;

					case 10:
						this.Areacode = buf.Substring(0, 3);
						this.Prefix = buf.Substring(3, 3);
						this.Suffix = buf.Substring(6, 4);
						break;

					case 7:
						this.Prefix = buf.Substring(0, 3);
						this.Suffix = buf.Substring(3, 4);
						break;

					default:
						throw new ArgumentOutOfRangeException("invalid phone: " + buf);
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "value", value);
				throw;
			}
		}

		#endregion
	}

	/// <summary>
	/// Telephone parser manager
	/// </summary>
	public class TelephoneParser<T> where T : ITelephoneParser
	{
		private T parser;

		/// <summary>
		/// Create new object with given attribute
		/// </summary>
		/// <param name="parser"></param>
		public TelephoneParser(T parser)
		{
			this.parser = parser;
		}

		/// <summary>
		/// Parse the telephone
		/// </summary>
		/// <param name="value"></param>
		public void Parse(string value)
		{
			parser.Parse(value);
		}

		/// <summary>
		/// Format the telephone
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string Format(string format)
		{
			return parser.Format(format);
		}
	}
}
