// QueryAnything: FileImporter.cs
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
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using System.Xml;

namespace crudwork.DataSetTools.DataImporters
{
	/// <summary>
	/// List of Supported Extension
	/// </summary>
	public enum FileExtension
	{
		/// <summary>Comma Separated Variable file</summary>
		CSV,
		/// <summary>DBF file</summary>
		DBF,
		/// <summary>Microsoft Access Database</summary>
		MDB,
		/// <summary>Tab delimited file</summary>
		TXT,
		/// <summary>Tab delimited file</summary>
		TAB,
		/// <summary>Microsoft Excel spreadsheet</summary>
		XLS,
		/// <summary>XML file</summary>
		XML,
	}

	/// <summary>
	/// File Importer / Exporter
	/// </summary>
	[Obsolete("consider using FileImports.ImportManager class", true)]
	public class FileImporter
	{
		private bool useHeaderRow = false;

		#region Comment
		/*
		 * look for other "Edit Me" section if you intend to add more extension to the list.
		 * */
		#endregion

		/// <summary>
		/// Import file into a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public DataSet Import(string filename)
		{
			FileExtension extension = ConvertExtension(Path.GetExtension(filename));
			switch (extension)	// Edit Me
			{
				case FileExtension.CSV:
					return ToDataSet(ImportCSV(filename, UseHeaderRow, ",", "\""));

				case FileExtension.DBF:
					return ImportDBF(filename);

				case FileExtension.MDB:
					return ImportAccess(filename);

				case FileExtension.TXT:
				case FileExtension.TAB:
					return ToDataSet(ImportCSV(filename, UseHeaderRow, "\t", "\""));

				case FileExtension.XLS:
					return ImportExcel(filename);

				case FileExtension.XML:
					return ImportXML(filename);

				default:
					throw new ArgumentOutOfRangeException("extension=" + extension);
			}
		}

		#region File Importers
		/// <summary>
		/// Convert a CSV file into a DataTable
		/// </summary>
		/// <param name="filename">CSV filename</param>
		/// <param name="hasHeader">Specify true if columns header is on line 1; false otherwise.</param>
		/// <returns></returns>
		public DataTable ImportCSV(string filename, bool hasHeader)
		{
			return ImportCSV(filename, hasHeader, ",", "\"");
		}

		/// <summary>
		/// Convert a CSV file into a DataTable
		/// </summary>
		/// <param name="filename">CSV filename</param>
		/// <param name="hasHeader">true if line one contains the columns header; false otherwise.</param>
		/// <param name="delimiter">delimiter string</param>
		/// <param name="qualifier">text qualifier: double-quote, single-quote or string.Empty</param>
		/// <returns></returns>
		public DataTable ImportCSV(string filename, bool hasHeader, string delimiter, string qualifier)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException(filename);

			int nr = 0;
			int nf = 0;

			DataTable dt = new DataTable(Path.GetFileNameWithoutExtension(filename));

			using (StreamReader r = new StreamReader(filename))
			{
				while (!r.EndOfStream)
				{
					nr++;
					string buffer = r.ReadLine();
					List<string> fields = Split(buffer, delimiter, qualifier);

					#region Create Columns
					if (nr == 1)
					{
						nf = fields.Count;

						if (hasHeader)
						{
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
				}

				r.Close();
			}

			return dt;
		}

		/// <summary>
		/// Convert a DBF File into a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private DataSet ImportDBF(string filename)
		{
			string folder = Path.GetDirectoryName(filename);
			string fname = Path.GetFileNameWithoutExtension(filename);

			string connectionString = string.Format(
				@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;User ID=Admin;Password=;",
				folder);
			OleDbManager m = new OleDbManager(connectionString);
			return ToDataSet(m.FillTable("select * from " + fname, fname));
		}

		/// <summary>
		/// Convert a Microsoft Excel into a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public DataSet ImportExcel(string filename)
		{
			string connectionString = string.Format(
				@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR={1};IMEX=1"";",
				filename,
				useHeaderRow ? "Yes" : "No");

			OleDbManager m = new OleDbManager(connectionString);
			return m.FillTables();
		}

		/// <summary>
		/// Convert a Microsoft Access database into a DataSet
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public DataSet ImportAccess(string filename)
		{
			//    Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password=MyDbPassword;
			string connectionString = string.Format(
				@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User Id=admin;Password=;",
				filename);

			OleDbManager m = new OleDbManager(connectionString);
			return m.FillTables();
		}

		private DataSet ImportXML(string filename)
		{
			return ImportXML(filename, false);
		}

		private DataSet ImportXML(string filename, bool handleMultipleRoots)
		{
			try
			{
				if (handleMultipleRoots)
				{
					// TODO: Need to be improved.  This approach could cause performance issues.
					StringBuilder s = new StringBuilder();
					s.Append(File.ReadAllText(filename));
					s.Insert(0, "<root>");
					s.Append("</root>");

					string newFilename = Environment.ExpandEnvironmentVariables(@"%TMP%\@multi-roots.xml");
					using (StreamWriter wr = new StreamWriter(newFilename))
					{
						wr.Write(s.ToString());
						wr.Close();
					}
					filename = newFilename;
				}

				DataSet ds = new DataSet();
				ds.ReadXml(filename, XmlReadMode.Auto);
				return ds;
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("There are multiple root elements"))
					return ImportXML(filename, true);
				throw;
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Convert string extension to a FileExtension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static FileExtension ConvertExtension(string extension)
		{
			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			switch (extension.ToUpper())	// Edit Me
			{
				case "CSV":
					return FileExtension.CSV;
				case "DBF":
					return FileExtension.DBF;
				case "MDB":
					return FileExtension.MDB;
				case "TXT":
					return FileExtension.TXT;
				case "TAB":
					return FileExtension.TAB;
				case "XLS":
					return FileExtension.XLS;
				case "XML":
					return FileExtension.XML;
				default:
					throw new ArgumentOutOfRangeException("unsupported extension=" + extension);
			}
		}

		/// <summary>
		/// return a description of a given file extension.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static string ExtensionDescription(string extension)
		{
			FileExtension ext = ConvertExtension(extension);
			return ExtensionDescription(ext);
		}

		/// <summary>
		/// return a description of a given file extension.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static string ExtensionDescription(FileExtension extension)
		{
			switch (extension)	// Edit Me
			{
				case FileExtension.CSV:
					return "Comma-Separated Variable";
				case FileExtension.DBF:
					return "DBF";
				case FileExtension.MDB:
					return "Microsoft Access";
				case FileExtension.TAB:
					return "Tab-Delimited";
				case FileExtension.TXT:
					return "Tab-Delimited";
				case FileExtension.XLS:
					return "Microsoft Excel";
				case FileExtension.XML:
					return "XML";
				default:
					throw new ArgumentOutOfRangeException("unsupported extension=" + extension);
			}
		}

		/// <summary>
		/// Return true, if file extension is supported
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool Supports(string filename)
		{
			try
			{
				FileExtension extension = ConvertExtension(Path.GetExtension(filename));
				return true;
			}
			catch //(Exception ex)
			{
				return false;
			}
		}

		private string MakeSafeColumnName(string value)
		{
			return value;
		}

		/// <summary>
		/// Split a string by a delimiter and return a string list.
		/// </summary>
		/// <param name="buffer">input string</param>
		/// <param name="delimiter">delimiter string</param>
		/// <param name="qualifier">text qualifier: double-quote, single-quote or string.Empty</param>
		/// <returns></returns>
		private List<string> Split(string buffer, string delimiter, string qualifier)
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

		private DataSet ToDataSet(DataTable dataTable)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(dataTable);
			return ds;
		}

		/// <summary>
		/// Get or set an indicator to read the first row as header.
		/// </summary>
		public bool UseHeaderRow
		{
			get
			{
				return useHeaderRow;
			}
			set
			{
				useHeaderRow = value;
			}
		}
		#endregion

		/// <summary>
		/// Export DataSet to file
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		public void Export(DataSet ds, string filename)
		{
			FileExtension extension = ConvertExtension(Path.GetExtension(filename));
			switch (extension)	// Edit Me
			{
				case FileExtension.CSV:
					ExportCSV(ds.Tables[0], filename, true, ",");
					//return ToDataSet(ImportCSV(filename, UseHeaderRow, ",", "\""));
					break;

				case FileExtension.DBF:
					throw new NotImplementedException("Export function is not yet supported");
				//break;

				case FileExtension.MDB:
					throw new NotImplementedException("Export function is not yet supported");
				//break;

				case FileExtension.TXT:
				case FileExtension.TAB:
					ExportCSV(ds.Tables[0], filename, true, "\t");
					break;

				case FileExtension.XLS:
					throw new NotImplementedException("Export function is not yet supported");
				//break;

				case FileExtension.XML:
					ds.WriteXml(filename);
					break;

				default:
					throw new ArgumentOutOfRangeException("extension=" + extension);
			}
		}

		#region File Exporters
		/// <summary>
		/// Save the DataTable as a CSV file
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="filename"></param>
		/// <param name="writeHeader"></param>
		/// <param name="delimiter"></param>
		public void ExportCSV(DataTable dt, string filename, bool writeHeader, string delimiter)
		{
			using (StreamWriter s = new StreamWriter(filename))
			{

				if (writeHeader)
				{
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						DataColumn dc = dt.Columns[i];
						if (i > 0)
							s.Write(delimiter);
						s.Write("\"{0}\"", dc.ColumnName);
					}
					s.WriteLine();
				}

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dr = dt.Rows[i];

					for (int j = 0; j < dt.Columns.Count; j++)
					{
						string columnName = dt.Columns[j].ColumnName;
						if (j > 0)
							s.Write(",");

						s.Write("\"{0}\"", dr[columnName]);
					}
					s.WriteLine();
				}

				s.Flush();
				s.Close();
			}
		}
		#endregion
	}
}
