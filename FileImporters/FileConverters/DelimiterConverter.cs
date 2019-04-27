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
using crudwork.Utilities;
using crudwork.FileImporters.Tool;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// Base class for handling text delimited data
	/// </summary>
	internal abstract class DelimiterConverter : FileConverterBase<DelimiterImportOptions>, IFileConverter
	{
		public DelimiterConverter(string delimiter)
			: base()
		{
			Options.Delimiter = delimiter;
		}

		#region IDataSetConverter Members

		System.Data.DataSet IFileConverter.Import(string filename)
		{
			return DataUtil.ToDataSet(Common.ImportCSV(filename, Options));
		}

		void IFileConverter.Export(System.Data.DataSet ds, string filename)
		{
			// TODO: Can create multiple files --> filename_{Tablename}.csv

			#region Sanity Checks
			if (ds == null || ds.Tables.Count == 0)
				throw new ArgumentNullException("ds is null or contains zero tables");

			if (ds.Tables.Count != 1)
				throw new ArgumentException("DataSet must have one DataTable");
			#endregion

			Common.ExportCSV(ds.Tables[0], filename, Options);
		}

		#endregion
	}
}
