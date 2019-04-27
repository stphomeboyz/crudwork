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

/* TODO: The enhanced features are not really finish!
 * 
 *		Nice to have but is too complex...
 *		
 *		1) constant(C) where C can be string, int, datetime, etc...
 *		2) compute(rownum|date|time|datetime)
 * 
 * * */
#undef SUPPORT_ENHANCE_FEATURES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using crudwork.Utilities;
using System.Text.RegularExpressions;

namespace crudwork.FileImporters.Specialized
{
	internal class FixedLengthFileReader
	{
		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		public FixedLengthFileReader()
		{
		}

		private string FixedValue(string value, int length)
		{
			if (value.Length > length)
				return value.Substring(0, length);
			else
				return value.PadRight(length);
		}

		/// <summary>
		/// Read the filename and return a DataTable
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public DataTable Read(string filename, FixedLengthImportOptions options)
		{
			#region Sanity Checks
			if (!File.Exists(filename))
				throw new FileNotFoundException("File not found: " + filename);

			if (options == null)
				throw new ArgumentNullException("options");
			if (options.Definition == null)
				throw new ArgumentNullException("options.Definition");
			if (options.Definition.Count == 0)
				throw new ArgumentException("The list of field spec is empty");
			if (options.Definition.RecordLength <= 0)
				throw new ArgumentException("The record length must be greater than zero");
			#endregion

			var def = options.Definition;
			int reclen = def.RecordLength;
			var fi = new FileInfo(filename);

			// make sure that file size is evenly divisible by the record length.
			// If not, we will assume that the definition is not valid for this file.
			if (fi.Length % reclen != 0)
				throw new ArgumentException("File size does not break evenly by record length of " + reclen);

			var dt = new DataTable();
			dt.TableName = Path.GetFileNameWithoutExtension(filename);

			#region Generate Data Columns
			foreach (var item in def)
			{
				DataColumn dc = new DataColumn(item.Name);
				dc.DataType = item.Type;
				if (item.Type == typeof(string))
					dc.MaxLength = item.Length;
				dt.Columns.Add(dc);
			}

			#endregion

			#region Populate Data Rows
			int rownum = 0;
			foreach (var buffer in FileUtil.ReadFile(filename, reclen, 0))
			{
				if (buffer.Length == 0)
					continue;

				rownum++;

				var dr = dt.NewRow();

				#region Convert data to column
				foreach (var item in def)
				{
					string val;

#if SUPPORT_ENHANCE_FEATURES
					if (item.Compute)
					{
						switch (item.Value.ToString().ToUpper())
						{
							case "COUNTER":
								val = rownum.ToString();
								break;
							case "DATE":
								val = DateTime.Now.ToShortDateString();
								break;
							case "TIME":
								val = DateTime.Now.ToShortTimeString();
								break;
							case "DATETIME":
								val = DateTime.Now.ToString("s");
								break;
							default:
								throw new ArgumentException("unknown auto-compute key value = " + item.Value);
						}
					}
					else
					{
						if (item.Start == -1)
							val = StringUtil.Unescape(item.Value.ToString());
						else
#else
					{
#endif
						val = ASCIIEncoding.ASCII.GetString(buffer, item.Start - 1, item.Length).Trim();
					}

					if (string.IsNullOrEmpty(val))
						continue;

					object value = DataConvert.ChangeType(val, null, item.Type);
					if (value == null)
						throw new ArgumentException(string.Format("Converting '{0}' to {1} failed: ", val, item.Type.FullName));

					dr[item.Name] = value;
				}
				#endregion

				dt.Rows.Add(dr);
			}
			#endregion

			return dt;
		}

		/// <summary>
		/// Create a fixed-length data file using the provided data table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="options"></param>
		/// <param name="filename"></param>
		public void Write(DataTable dt, FixedLengthImportOptions options, string filename)
		{
			#region Sanity Checks
			if (dt == null)
				throw new ArgumentNullException("dt");
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("filename");
			#endregion

			//ResizeTableMaxLength(dt);

			var buffer = new byte[options.RecordLength];

			using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
			using (var w = new BinaryWriter(fs))
			{

				foreach (DataRow dr in dt.Rows)
				{
					#region initialize the buffer with spaces
					for (int i = 0; i < buffer.Length; i++)
					{
						buffer[i] = 32;
					}
					#endregion

					foreach (DataColumn dc in dt.Columns)
					{
						int start = options.Definition[dc.ColumnName].Start - 1;
						int len = options.Definition[dc.ColumnName].Length;
						var value = Encoding.ASCII.GetBytes((dr[dc] ?? string.Empty).ToString());

						Array.Copy(value, 0, buffer, start, len);
					}

					w.Write(buffer);

					if (!string.IsNullOrEmpty(options.NewlineChar))
						w.Write(options.NewlineChar);	// ends the row with the specified newline delimiter
				}

				w.Flush();
				w.Close();
			}
		}

		private void ResizeTableMaxLength(DataTable dt)
		{
			foreach (DataColumn dc in dt.Columns)
			{
				int maxLength = 0;

				foreach (DataRow dr in dt.Rows)
				{
					string v = dr[dc].ToString();
					if (v.Length > maxLength)
						maxLength = v.Length;
				}

				dc.ExtendedProperties["FixedLength"] = maxLength;
			}
		}

		///// <summary>
		///// Write the data schema to a file
		///// </summary>
		///// <param name="dt"></param>
		///// <param name="filename"></param>
		//public void WriteSchema(DataTable dt, string filename)
		//{
		//    #region Sanity Checks
		//    if (dt == null)
		//        throw new ArgumentNullException("dt");
		//    if (string.IsNullOrEmpty(filename))
		//        throw new ArgumentNullException("filename");
		//    #endregion
		//    dt.WriteXmlSchema(filename);
		//    //var sb = new StringBuilder();
		//}

		#region Static methods to read schema
		public static FieldSpecList ReadSchema(string[] content, DefinitionFormat format)
		{
			switch (format)
			{
				case DefinitionFormat.Text:
					return ReadTextSchema(content);
				//case DefinitionFormat.XML:
				//	return ReadXMLSchema(filename);
				default:
					throw new ArgumentOutOfRangeException("format=" + format);
			}
		}

		public static FieldSpecList ReadSchema(string filename, DefinitionFormat format)
		{
			switch (format)
			{
				case DefinitionFormat.Text:
					return ReadTextSchema(FileUtil.ReadFile(filename));
				case DefinitionFormat.XML:
					return ReadXMLSchema(filename);
				default:
					throw new ArgumentOutOfRangeException("format=" + format);
			}
		}

		private static FieldSpecList ReadXMLSchema(string filename)
		{
			string value = File.ReadAllText(filename);
			return Serializer.Deserialize<FieldSpecList>(value, Serializer.SerializeMethods.Xml);
		}

		private static FieldSpecList ReadTextSchema(IEnumerable<string> enumerator)
		{
			var result = new FieldSpecList();

			foreach (var item in enumerator)
			{
				string line = RemoveComments(item).ToLower().Trim(' ', '\t');
				if (string.IsNullOrEmpty(line))
					continue;

				var len = FieldSpecList.ReadRecordLength(line);
				if (len.HasValue)
				{
					result.RecordLength = len.Value;
					continue;
				}

				var spec = FieldSpec.ToFieldSpec(line);
				result.Add(spec);
			}

			return result;
		}

		private static string RemoveComments(string value)
		{
			int idx = value.IndexOf('#');
			return (idx == -1) ? value : value.Substring(0, idx);
		}

		public static void WriteSchema(FieldSpecList definition, string filename, DefinitionFormat format)
		{
			switch (format)
			{
				case DefinitionFormat.Text:
					using (var w = new StreamWriter(filename))
					{
						foreach (var item in definition)
						{
							w.WriteLine("{0,-20} {1,-20} {2}", item.Type, item.Name, item.ToSpec());
						}

						w.WriteLine(definition.ToRecordLength());

						w.Flush();
						w.Close();
					}
					break;

				case DefinitionFormat.XML:
					{
						var s = Serializer.Serialize(definition, Serializer.SerializeMethods.Xml);
						File.WriteAllText(filename, s);
					}
					break;

				default:
					throw new ArgumentException("format=" + format);
			}
		}
		#endregion
	}
}
