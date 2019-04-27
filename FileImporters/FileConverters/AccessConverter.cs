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
using System.Runtime.InteropServices;
using crudwork.DataAccess;
using System.Data;
using crudwork.FileImporters.Specialized;
using crudwork.Models.DataAccess;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// Microsoft Access Converter
	/// </summary>
	internal class AccessConverter : FileConverterBase<AccessImportOptions>, IFileConverter
	{
		public AccessConverter()
			: base()
		{
		}

		#region IDataSetConverter Members

		System.Data.DataSet IFileConverter.Import(string filename)
		{
			string connectionString = ConnectionStringManager.MakeAccess(filename, Options.Username, Options.Password, false);
			return Common.OleDbTables(connectionString, Options.Tablename, Options.TableFilter);
		}

		void IFileConverter.Export(System.Data.DataSet ds, string filename)
		{
			string connectionString = ConnectionStringManager.MakeAccess(filename, Options.Username, Options.Password, false);

			#region Create DB if not already exist
			if (!File.Exists(filename))
			{
				using (ADOX_CatalogClass adox = new ADOX_CatalogClass())
				{
					// http://support.microsoft.com/kb/317881
					adox.CatalogClass.Create(connectionString);
				}

				if (!File.Exists(filename))
					throw new FileNotFoundException("unable to create file: " + filename);
			}
			#endregion

			DataFactory df = new DataFactory(DatabaseProvider.OleDb, connectionString);
			df.TestConnection();

			foreach (DataTable dt in ds.Tables)
			{
				df.CreateTable(dt.TableName, dt);
			}

		}

		#endregion
	}
}
