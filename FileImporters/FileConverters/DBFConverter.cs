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
using crudwork.DataAccess;
using crudwork.FileImporters.Specialized;
using crudwork.Utilities;
using System.Data;
using crudwork.Models.DataAccess;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// DBF File Converter
	/// </summary>
	internal class DBFConverter : FileConverterBase<DBFImportOptions>, IFileConverter
	{
		public DBFConverter()
			: base()
		{
		}

		#region IDataSetConverter Members

		System.Data.DataSet IFileConverter.Import(string filename)
		{
			switch (Options.ImportEngine)
			{
				case DBFEngine.OLEDB:
					{
						string connectionString = ConnectionStringManager.MakeDBF(filename, true);
						string tablename = Path.GetFileNameWithoutExtension(filename);
						return Common.OleDbTables(connectionString, tablename, QueryFilter.Exact);
					}

				case DBFEngine.DBF4:
					{
						var dbf = new dBase4(Options);
						dbf.OpenFile(filename);
						return DataUtil.ToDataSet(dbf.DataTable);
					}

				default:
					throw new ArgumentOutOfRangeException("ImportEngine=" + Options.ImportEngine);
			}
		}

		void IFileConverter.Export(System.Data.DataSet ds, string filename)
		{
			if (ds == null || ds.Tables.Count == 0)
				throw new ArgumentNullException("ds");

			switch (Options.ExportEngine)
			{
				case DBFEngine.OLEDB:
					throw new NotImplementedException("OLEDB currently does not support Export");

				case DBFEngine.DBF4:
					{
						var dbf = new dBase4();
						dbf.ImportDataTable(ds.Tables[0]);
						dbf.SaveFile(filename);

						/*
						foreach (DataTable dt in ds.Tables)
						{
							dbf.ImportDataTable(dt);
							dbf.SaveFile(filename);
							break;
						}
						 * */
						break;
					}

				default:
					throw new ArgumentOutOfRangeException("ExportEngine=" + Options.ExportEngine);
			}
		}

		#endregion
	}
}
