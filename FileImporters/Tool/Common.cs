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
using System.IO;
using System.Data;
using crudwork.DataAccess;
using crudwork.Utilities;
using System.Text.RegularExpressions;
using System.Diagnostics;
using crudwork.Models.DataAccess;

namespace crudwork.FileImporters.Tool
{
	/// <summary>
	/// Common utilities among converters
	/// </summary>
	internal static class Common
	{
		private static bool IsValidHeader(string value)
		{
			if (!Regex.IsMatch(value, "^[A-Z]", RegexOptions.IgnoreCase | RegexOptions.Singleline))
				return false;

			// all is well...
			return true;
		}
		private static void ValidateHeaderField(List<string> value)
		{
			List<string> duplicates = new List<string>();

			for (int i = 0; i < value.Count; i++)
			{
				string v = value[i];

				if (duplicates.Contains(v))
					throw new ArgumentException("duplicate column name: " + v);

				if (!IsValidHeader(v))
					throw new ArgumentException("not a valid column name: " + v);

				duplicates.Add(v);
			}
		}

		public static IEnumerable<DataRow> ImportCSV2(string filename, DelimiterImportOptions options)
		{
			bool hasHeader = options.UseHeader;
			string delimiter = options.Delimiter;
			bool useTextQualifier = options.UseTextQualifier;
			string qualifier = options.UseTextQualifier ? options.TextQualifierId : string.Empty;
			int maxRows = options.ImportMaxRows;

			if (!File.Exists(filename))
				throw new FileNotFoundException(filename);

			int nr = 0;
			int nf = 0;
			DataRow dr = null;

			using (DataTable dt = new DataTable(Path.GetFileNameWithoutExtension(filename)))
			using (StreamReader r = new StreamReader(filename))
			{
				while (!r.EndOfStream)
				{
					string buffer = r.ReadLine();

					if (string.IsNullOrEmpty(buffer))
						continue;

					List<string> fields = Split(buffer, delimiter, qualifier);
					nr++;

					#region Create Columns
					if (nr == 1)
					{
						nf = fields.Count;

						if (hasHeader)
						{
							ValidateHeaderField(fields);

							for (int i = 0; i < fields.Count; i++)
							{
								string name = MakeSafeColumnName(fields[i]);
								dt.Columns.Add(name);
							}
						}
						else
						{
							// make up a generic field names.
							for (int i = 0; i < fields.Count; i++)
							{
								dt.Columns.Add("Field" + (i + 1));
							}
						}

						dr = dt.NewRow();

						// skip this line if, and only if, the first row has header info
						if (hasHeader)
							continue;
					}
					#endregion

					#region attempt to fix splitted lines.
					if (fields.Count != nf)
					{
						while (fields.Count < nf && !r.EndOfStream)
						{
							string buf2 = r.ReadLine();
							buffer += "\\n" + buf2;
							fields = Split(buffer, delimiter, qualifier);
						}

						if (fields.Count != nf)
							throw new ApplicationException("inconsistent field count.  (nf=[" + nf + "], fields.Count=[" + fields.Count + "], nr=[" + nr + "])");
					}
					#endregion

					#region Insert Data into columns
					for (int i = 0; i < fields.Count; i++)
					{
						dr[i] = fields[i];
					}
					#endregion

					yield return dr;

					if (maxRows > 0 && nr > maxRows)
						break;
				}

				r.Close();
			}

			yield break;
		}

		/// <summary>
		/// Convert a CSV file into a DataTable
		/// </summary>
		/// <param name="filename">CSV filename</param>
		/// <param name="options">converter options</param>
		/// <returns></returns>
		public static DataTable ImportCSV(string filename, DelimiterImportOptions options)
		{
			bool hasHeader = options.UseHeader;
			string delimiter = options.Delimiter;
			bool useTextQualifier = options.UseTextQualifier;
			string qualifier = options.UseTextQualifier ? options.TextQualifierId : string.Empty;
			int maxRows = options.ImportMaxRows;

			if (!File.Exists(filename))
				throw new FileNotFoundException(filename);

			int nr = 0;
			int nf = 0;

			DataTable dt = new DataTable(Path.GetFileNameWithoutExtension(filename));

			using (StreamReader r = new StreamReader(filename))
			{
				while (!r.EndOfStream)
				{
					string buffer = r.ReadLine();

					if (string.IsNullOrEmpty(buffer))
						continue;

					List<string> fields = Split(buffer, delimiter, qualifier);
					nr++;

					#region Create Columns
					if (nr == 1)
					{
						nf = fields.Count;

						if (hasHeader)
						{
							ValidateHeaderField(fields);

							for (int i = 0; i < fields.Count; i++)
							{
								string name = MakeSafeColumnName(fields[i]);
								dt.Columns.Add(name);
							}
							// skip this line.
							continue;
						}
						else
						{
							// make up a generic field names.
							for (int i = 0; i < fields.Count; i++)
							{
								dt.Columns.Add("Field" + (i + 1));
							}
						}
					}
					#endregion

					#region attempt to fix splitted lines.
					if (fields.Count != nf)
					{
						while (fields.Count < nf && !r.EndOfStream)
						{
							string buf2 = r.ReadLine();
							buffer += "\\n" + buf2;
							fields = Split(buffer, delimiter, qualifier);
						}

						if (fields.Count != nf)
							throw new ApplicationException("inconsistent field count.  (nf=[" + nf + "], fields.Count=[" + fields.Count + "], nr=[" + nr + "])");
					}
					#endregion

					#region Insert Data into columns
					DataRow dr = dt.NewRow();
					for (int i = 0; i < fields.Count; i++)
					{
						dr[i] = fields[i];
					}
					dt.Rows.Add(dr);
					#endregion

					if (maxRows > 0 && nr > maxRows)
						break;
				}

				r.Close();
			}

			return dt;
		}

		/// <summary>
		/// Split a string by a delimiter and return a string list.
		/// </summary>
		/// <param name="buffer">input string</param>
		/// <param name="delimiter">delimiter string</param>
		/// <param name="qualifier">text qualifier: double-quote, single-quote or string.Empty</param>
		/// <returns></returns>
		private static List<string> Split(string buffer, string delimiter, string qualifier)
		{
			List<string> fields = new List<string>();
			fields.AddRange(buffer.Split(new string[] { delimiter }, StringSplitOptions.None));
			int qlen = qualifier.Length;

			for (int i = 0; i < fields.Count; i++)
			{
				string data = fields[i];

				if (data.StartsWith(qualifier) && !data.EndsWith(qualifier))
				{
					// fix broken field
					while (!data.EndsWith(qualifier) && i < fields.Count - 1)
					{
						fields[i] = null;
						i++;
						data += delimiter + fields[i];
					}
				}

				if (data.StartsWith(qualifier) && data.EndsWith(qualifier))
				{
					fields[i] = data.Substring(qlen, data.Length - (qlen * 2));
				}
			}

			// ignore nulls...
			List<string> result = new List<string>();
			for (int i = 0; i < fields.Count; i++)
			{
				if (fields[i] == null)
					continue;
				result.Add(fields[i]);
			}

			return result;
		}

		/// <summary>
		/// Convert or remove any character that are not supported in Column name.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string MakeSafeColumnName(string value)
		{
			return value;
		}

		/// <summary>
		/// Convert or remove any character that are not supported in data value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string MakeSafeColumnValue(object value)
		{
			if (DataConvert.IsNull(value))
				return string.Empty;

			StringBuilder sb = new StringBuilder(value.ToString());
			sb.Replace("\n", "\\n");
			return sb.ToString();
		}



		/// <summary>
		/// Save the DataTable as a CSV file
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		public static void ExportCSV(DataTable dt, string filename, DelimiterImportOptions options)
		{
			bool writeHeader = options.UseHeader;
			string delimiter = options.Delimiter;
			bool useTextQualifier = options.UseTextQualifier;
			string qualifier = options.UseTextQualifier ? options.TextQualifierId : string.Empty;
			string newlineChar = options.NewlineChar;

			using (StreamWriter s = new StreamWriter(filename))
			{
				if (writeHeader)
				{
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						DataColumn dc = dt.Columns[i];
						if (i > 0)
							s.Write(delimiter);
						s.Write("{0}{1}{0}", qualifier, MakeSafeColumnName(dc.ColumnName));
					}
					s.Write(newlineChar);
				}

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dr = dt.Rows[i];

					for (int j = 0; j < dt.Columns.Count; j++)
					{
						DataColumn dc = dt.Columns[j];
						if (j > 0)
							s.Write(delimiter);
						s.Write("{0}{1}{0}", qualifier, MakeSafeColumnValue(dr[dc.ColumnName]));
					}
					s.Write(newlineChar);
				}

				s.Flush();
				s.Close();
			}
		}

		///// <summary>
		///// Retrieve table from OleDb provider
		///// </summary>
		///// <param name="connectionString"></param>
		///// <param name="query"></param>
		///// <returns></returns>
		//public static DataSet OleDbTable(string connectionString, string query)
		//{
		//    DataFactory df = new DataFactory(DatabaseProvider.OleDb, connectionString);
		//    return df.Fill(query);
		//}

		/// <summary>
		/// Retrieve tables from OleDb provider
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="tablename"></param>
		/// <param name="tableFilter"></param>
		/// <returns></returns>
		public static DataSet OleDbTables(string connectionString, string tablename, QueryFilter tableFilter)
		{
			DataFactory df = new DataFactory(DatabaseProvider.OleDb, connectionString);
			DataSet ds = new DataSet();

			var tdl = df.Database.GetTables(tablename, tableFilter);

			foreach (var entry in tdl)
			{
				DataTable dt = df.FillTable("select * from [" + entry.TableName + "]");
				dt.TableName = entry.TableName;
				ds.Tables.Add(dt);
			}

			return ds;
		}

		#region Methods for DBF conversion
		/// <summary>
		/// Copy portion of a byte array into a fixed byte array.
		/// </summary>
		/// <param name="source">source byte array</param>
		/// <param name="start">the starting position</param>
		/// <param name="stop">the ending position</param>
		/// <param name="target">the fixed byte array</param>
		/// <param name="maxCapacity">the array's maximum capacity</param>
		public static unsafe void UnsafeCopy(byte[] source, int start, int stop, byte* target, int? maxCapacity)
		{
			#region Sanity Checks
			if (start < 0)
				throw new ArgumentException("start position is out of range");
			if (stop > source.Length)
				throw new ArgumentException("stop position is out of range");
			#endregion

			int c = 0;

			for (int i = start; i < stop; i++)
			{
				#region Break on these conditions
				if (i < 0 || i >= source.Length)
				{
					Debug.Write("WARNING: index is out of range");
					break;
				}

				if (maxCapacity.HasValue && c >= maxCapacity.Value)
				{
					Debug.Write("WARNING: maximum capacity reached!!");
					break;
				}
				#endregion

				*target = source[i];
				target++;
				c++;
			}

			// fill the target array with null to max length
			if (maxCapacity.HasValue)
			{
				while (c < maxCapacity.Value)
				{
					*target = 0;
					target++;
					c++;
				}
			}
		}

		/// <summary>
		/// Copy portion of a byte array into a fixed byte array.
		/// </summary>
		/// <param name="source">source byte array</param>
		/// <param name="start">the starting position</param>
		/// <param name="stop">the ending position</param>
		/// <param name="target">the fixed byte array</param>
		public static unsafe void UnsafeCopy(byte[] source, int start, int stop, byte* target)
		{
			UnsafeCopy(source, start, stop, target, null);
		}

		/// <summary>
		/// Convert portion of a byte array into a string
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <returns></returns>
		public static string ToString(byte[] buffer, int start, int stop)
		{
			int len = stop - start + 1;
			//return BitConverter.ToString(buffer, start, len);
			return ASCIIEncoding.ASCII.GetString(buffer, start, len).Trim();
		}

		/// <summary>
		/// Convert portion of a byte array into an Int16-type
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		public static Int16 ToInt16(byte[] buffer, int start)
		{
			return BitConverter.ToInt16(buffer, start);
			//if (stop - start + 1 != 2)
			//    throw new ArgumentException("incorrect length.  Expected 2 bytes");

			//Int16 result = 0;
			//byte shift = 0;

			//for (int i = start; i <= stop; i++)
			//{
			//    byte b = buffer[i];
			//    result += (Int16)(b << (shift * 8));
			//    shift++;
			//}

			//return result;
		}

		/// <summary>
		/// Convert portion of a byte array into an Int32-type
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		public static Int32 ToInt32(byte[] buffer, int start)
		{
			return BitConverter.ToInt32(buffer, start);
			//if (stop - start + 1 != 4)
			//    throw new ArgumentException("incorrect length.  Expected 4 bytes");

			//Int32 result = 0;
			//byte shift = 0;

			//for (int i = start; i <= stop; i++)
			//{
			//    byte b = buffer[i];
			//    result += (Int32)(b << (shift * 8));
			//    shift++;
			//}

			//return result;
		}
		#endregion

		/// <summary>
		/// Convert a string-type to a byte array
		/// </summary>
		/// <param name="value"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <returns></returns>
		public static byte[] StringToByteArray(string value, int start, int stop)
		{
			int max = Math.Min(stop, value.Length);
			byte[] results = new byte[max];
			ASCIIEncoding.ASCII.GetBytes(value, start, max, results, 0);
			return results;
		}

		/// <summary>
		/// Convert a byte array (at the start/stop position) into a string-type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <returns></returns>
		public static unsafe string ByteArrayToString(byte* source, int start, int stop)
		{
			// seek to the starting position
			for (int i = 0; i < start; i++)
			{
				byte b = *source++;
				if (b == 0)
					return string.Empty;	// EOL encountered while seeking to start position
			}

			StringBuilder sb = new StringBuilder();
			for (int i = start; i < stop; i++)
			{
				byte b = *source++;
				if (b == 0)
					break;
				sb.Append((char)b);
			}
			return sb.ToString();
		}

		public static unsafe byte[] ToBytes(byte* source, int readBytes)
		{
			List<byte> results = new List<byte>();

			for (int i = 0; i < readBytes; i++)
			{
				results.Add(*source++);
			}

			return results.ToArray();
		}
		public static byte[] ToBytes(Int32 value)
		{
			return BitConverter.GetBytes(value);
		}
		public static byte[] ToBytes(Int16 value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// convert string to byte array
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <param name="filler"></param>
		/// <returns></returns>
		public static byte[] ToBytes(string value, int maxLength, byte filler)
		{
			byte[] results = new byte[maxLength];

			for (int i = 0; i < value.Length && i < maxLength; i++)
			{
				results[i] = (byte)value[i];
			}

			int c = value.Length;
			while (c < maxLength)
			{
				results[c++] = filler;
			}

			return results;
		}

		/// <summary>
		/// Convert a date or datetime type to byte array: YYYYMMDD and HHMMSS if time is included
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="includeTime"></param>
		/// <returns></returns>
		public static byte[] ToBytes(DateTime dt, bool includeTime)
		{
			List<byte> results = new List<byte>();

			results.AddRange(ToBytes(dt.Year.ToString(), 4, 0));
			results.AddRange(ToBytes(dt.Month.ToString(), 2, 0));
			results.AddRange(ToBytes(dt.Day.ToString(), 2, 0));

			if (includeTime)
			{
				results.AddRange(ToBytes(dt.Hour.ToString(), 2, 0));
				results.AddRange(ToBytes(dt.Minute.ToString(), 2, 0));
				results.AddRange(ToBytes(dt.Second.ToString(), 2, 0));
			}

			return results.ToArray();
		}

		/// <summary>
		/// return a 4-digit year for the given two-digit.
		/// If the year &lt;= 30, then return 20xx; otherwise return 19xx.
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public static int Y2K(int year)
		{
			if (year >= 100)
				throw new ArgumentException("year must be a two-digit number");

			if (year <= 30)
				return year + 2000;
			else
				return year + 1900;
		}
	}
}
