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
using System.Diagnostics;
using System.Data;
using crudwork.Utilities;
using crudwork.Models.DataAccess;
using crudwork.FileImporters.Tool;

namespace crudwork.FileImporters
{
	/// <summary>
	/// File Import / Export Manager
	/// </summary>
	public static class ImportManager
	{
		private static ConverterEngineList converters;

		#region Constructors
		static ImportManager()
		{
			converters = new ConverterEngineList();

			AddConverter("Microsoft Access databases", typeof(FileConverters.AccessConverter), typeof(FileImporters.AccessImportOptions), "MDB");
			AddConverter("Comma-delimited files", typeof(FileConverters.CSVConverter), typeof(FileImporters.DelimiterImportOptions), "CSV");
			AddConverter("DBF files", typeof(FileConverters.DBFConverter), typeof(FileImporters.DBFImportOptions), "DBF");
			AddConverter("Microsoft Excel files", typeof(FileConverters.ExcelConverter), typeof(FileImporters.ExcelImportOptions), "XLS");
			AddConverter("Microsoft Excel-2007 files", typeof(FileConverters.Excel2007Converter), typeof(FileImporters.ExcelImportOptions), "XLSX");
			AddConverter("Tab-delimited files", typeof(FileConverters.TabConverter), typeof(FileImporters.DelimiterImportOptions), "TAB");
			AddConverter("XML files", typeof(FileConverters.XmlConverter), typeof(FileImporters.XmlImportOptions), "XML");

			AddConverter("Fixed-length file", typeof(FileConverters.FixedFileConverter), typeof(FileImporters.FixedLengthImportOptions), "FIX");
			AddConverter("Flat file", typeof(FileConverters.FlatFileConverter), typeof(FileImporters.FixedLengthImportOptions), "TXT");
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Get the converter engine for filename
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		private static IFileConverter GetConverter(string filename, ConverterOptionList options)
		{
			try
			{
				// return the extension name (w/o the dot)
				string ext = Path.GetExtension(filename).Substring(1);
				if (string.IsNullOrEmpty(ext))
					throw new ArgumentException("File does not have an extension: " + filename);

				var type = converters.GetType(ext);
				if (type == null)
					throw new ArgumentException("Extension not supported: " + ext);

				var importOptionsType = converters.GetImportOptionsType(ext);

				var converterBase = Activator.CreateInstance(type);
				if (converterBase == null)
					throw new ApplicationException("Cannot create a new instance of this type: " + type.FullName);

				var converter = (IFileConverter)converterBase;
				if (converter == null)
					throw new ApplicationException("This converter does not implement IFileConverter: " + type.FullName);

				converter.SetOptions(options);

				return converter;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw;
			}
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Get a list of supported extensions
		/// </summary>
		/// <returns></returns>
		public static string[] GetSupportedExtensions()
		{
			List<string> result = new List<string>();

			foreach (ConverterEngine item in converters)
			{
				result.AddRange(item.Extensions);
			}

			result.Sort();
			return result.ToArray();
		}

		/// <summary>
		/// Return a Filter expression for OpenFileDialog() and SaveFileDialog()
		/// </summary>
		/// <returns></returns>
		public static string GetSupportedFilters()
		{
			return converters.ToFilter();
		}

		/// <summary>
		/// Return true if the extension is registered as supported type
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static bool IsSupportedExtension(string extension)
		{
			var supportedExtensions = GetSupportedExtensions();
			return Array.BinarySearch<string>(supportedExtensions, extension.ToUpper()) >= 0;
		}

		///// <summary>
		///// Get the file's source type
		///// </summary>
		///// <param name="filename"></param>
		///// <returns></returns>
		//public static SourceType GetSourceType(string filename)
		//{
		//    if (string.IsNullOrEmpty(filename))
		//        throw new ArgumentNullException("filename");

		//    string ext = Path.GetExtension(filename).Substring(1).ToUpper();

		//    // TODO: Hard-coded these for now... Need to improve this later!
		//    return "MDB|XLS|XLSX".Contains(ext) ? SourceType.ComplexDataFile : SourceType.SimpleDataFile;
		//}

		/// <summary>
		/// Return the FileConverter registered for the given extension
		/// </summary>
		/// <param name="ext"></param>
		/// <returns></returns>
		public static Type GetOptionType(string ext)
		{
			return converters.GetImportOptionsType(ext);
		}

		#region Register / Unregister converters
		/// <summary>
		/// add new file converter to the converter list
		/// </summary>
		/// <param name="description"></param>
		/// <param name="type"></param>
		/// <param name="importOptionsType"></param>
		/// <param name="extensions"></param>
		public static void AddConverter(string description, Type type, Type importOptionsType, params string[] extensions)
		{
			#region Make sure type specified implements the IFileConverter interface
			var interfaces = type.GetInterfaces();
			bool found = false;
			foreach (var item in interfaces)
			{
				if (item != typeof(IFileConverter))
					continue;
				found = true;
				break;
			}
			if (!found)
				throw new ArgumentException("type must implement the crudwork.FileImporters.IFileConverter interface");
			#endregion

			converters.Add(description, type, importOptionsType, extensions);
		}

		/// <summary>
		/// remove an existing converter from the converter list
		/// </summary>
		/// <param name="type"></param>
		public static void RemoveConverter(Type type)
		{
			converters.Remove(type);
		}
		#endregion

		#region Import / Export Methods
		#region Import - via obsoleted ConverterOptionList
		/// <summary>
		/// Import the file and return a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		[Obsolete("Consider using the strongly-typed version: Import(string, ImportOptions)", false)]
		public static DataSet Import(string filename)
		{
#pragma warning disable 0618
			return Import(filename, new ConverterOptionList());
#pragma warning restore 0618
		}

		/// <summary>
		/// Import the file and return a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		[Obsolete("Consider using the strongly-typed version: Import(string, ImportOptions)", false)]
		public static DataSet Import(string filename, ConverterOptionList options)
		{
			try
			{
				IFileConverter converter = GetConverter(filename, options);
				return converter.Import(filename);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				DebuggerTool.AddData(ex, "filename", filename);
				DebuggerTool.AddData(ex, "options", options);
				throw;
			}
		}
		#endregion

		/// <summary>
		/// Import the file and return a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static DataSet Import(string filename, ImportOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");

#pragma warning disable 0618
			return Import(filename, options.ToConverterOptionList());
#pragma warning restore 0618
		}

		/// <summary>
		/// Import the file and return a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static DataSet Import(string filename, string options)
		{
			//if (string.IsNullOrEmpty(options))
			//	throw new ArgumentNullException("options");

#pragma warning disable 0618
			return Import(filename, new ConverterOptionList(options));
#pragma warning restore 0618
		}

		/// <summary>
		/// Import the file and return a DataSet
		/// </summary>
		/// <param name="connectionSpec"></param>
		/// <returns></returns>
		public static DataSet Import(DataConnectionInfo connectionSpec)
		{
			return Import(connectionSpec.Filename, connectionSpec.Options);
		}

		#region Export - via obsoleted ConverterOptionList
		/// <summary>
		/// Export a DataSet to a file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		[Obsolete("Consider using the strongly-typed version: Export(string, ImportOptions)", false)]
		public static void Export(DataSet ds, string filename)
		{
#pragma warning disable 0618
			Export(ds, filename);
#pragma warning restore 0618
		}

		/// <summary>
		/// Export a DataSet to a file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		[Obsolete("Consider using the strongly-typed version: Export(string, ImportOptions)", false)]
		public static void Export(DataSet ds, string filename, ConverterOptionList options)
		{
			try
			{
				IFileConverter converter = GetConverter(filename, options);
				converter.Export(ds, filename);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				DebuggerTool.AddData(ex, "filename", filename);
				DebuggerTool.AddData(ex, "options", options);
				throw;
			}
		}
		#endregion

		/// <summary>
		/// Export a DataSet to a file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		public static void Export(DataSet ds, string filename, ImportOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");

#pragma warning disable 0618
			Export(ds, filename, options.ToConverterOptionList());
#pragma warning restore 0618
		}

		/// <summary>
		/// Export a DataSet to a file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		public static void Export(DataSet ds, string filename, string options)
		{
			//if (options == null)
			//	throw new ArgumentNullException("options");

#pragma warning disable 0618
			Export(ds, filename, new ConverterOptionList(options));
#pragma warning restore 0618
		}

		/// <summary>
		/// Export a DataSet to a file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="connectionSpec"></param>
		public static void Export(DataSet ds, DataConnectionInfo connectionSpec)
		{
			Export(ds, connectionSpec.Filename, connectionSpec.Options);
		}
		#endregion

		#region ImportRow methods using IEnumerable<DataRow>

		/// <summary>
		/// Import file, yielding a DataRow for each iteration using IEnumerable type.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static IEnumerable<DataRow> ImportRow(string filename, DelimiterImportOptions options)
		{
			return Common.ImportCSV2(filename, options);
		}

		#endregion
		#endregion
	}
}
