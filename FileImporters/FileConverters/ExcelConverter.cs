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
using crudwork.FileImporters.Tool;
using System.IO;
using crudwork.Utilities;
using crudwork.DataAccess;
using System.Runtime.InteropServices;
using System.Data;
using crudwork.Models.DataAccess;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// Microsoft Excel Converter
	/// </summary>
	internal class ExcelConverter : FileConverterBase<ExcelImportOptions>, IFileConverter
	{
		public ExcelConverter()
			: base()
		{
		}

		/// <summary>
		/// always return NAME or POSITION (uppercase)
		/// </summary>
		[Obsolete("Feature is not yet implemented", true)]
		public string OpenTableBy
		{
			get
			{
				string value = Options.OpenTableBy.ToUpper();
				if (value == "POSITION")
					return value;
				return "NAME";
			}
		}

		/// <summary>
		/// get the worksheet name (with a trailing dollar sign)
		/// </summary>
		public string Tablename
		{
			get
			{
				string value = Options.Tablename;
				return value + (value.EndsWith("$") ? "" : "$");
			}
		}

		[Obsolete("Feature is not yet implemented", true)]
		private void OpenTableByPosition(string connectionString)
		{
			int pos = Options.TablePosition;
			if (pos == -1)
				throw new ArgumentOutOfRangeException("TablePosition must be greater than or equal to 0", "TablePosition");

			DataFactory df = new DataFactory(DatabaseProvider.OleDb, connectionString);
			var tdl = df.Database.GetTables();

			if (tdl.Count < pos)
				throw new ArgumentException("TablePosition exceeded the number of tables found", "TablePosition");

			// import this table name
			Options.Tablename = tdl[pos].TableName;
			// and import exactly one table
			Options.TableFilter = QueryFilter.Exact;
		}

		#region IDataSetConverter Members

		System.Data.DataSet IFileConverter.Import(string filename)
		{
			string connectionString = null;

			if (Options.Provider == "Microsoft.ACE.OLEDB.12.0")
				connectionString = ConnectionStringManager.MakeExcel2007(filename, Options.Username, Options.Password, Options.UseHeader, true);
			else
				connectionString = ConnectionStringManager.MakeExcel(filename, Options.Username, Options.Password, Options.UseHeader, true);

			//if (OpenTableBy == "POSITION")
			//{
			//    OpenTableByPosition(connectionString);
			//}

			return Common.OleDbTables(connectionString, Tablename, Options.TableFilter);
		}

		void IFileConverter.Export(System.Data.DataSet ds, string filename)
		{
			// consider using CarlosAg Excel Xml Writer Library --> http://www.carlosag.net/Tools/ExcelXmlWriter
			throw new NotImplementedException();

			//string connectionString = ConnectionStringManager.MakeExcel(filename, Username, Password, UseHeader, false);

			#region A possible way to export  an Excel
			//#region Create DB if not already exist
			//if (!File.Exists(filename))
			//{
			//    FileUtil.WriteFile(filename, FileImportResource.EmptyExcel);

			//    if (!File.Exists(filename))
			//        throw new FileNotFoundException("unable to create file: " + filename);
			//}
			//#endregion

			//DataFactory df = new DataFactory(DatabaseProvider.OleDb, connectionString);
			//df.TestConnection();

			//foreach (DataTable dt in ds.Tables)
			//{
			//    df.Import.CreateNewTable(dt.TableName, dt);
			//}
			#endregion
		}

		#endregion
	}
}
