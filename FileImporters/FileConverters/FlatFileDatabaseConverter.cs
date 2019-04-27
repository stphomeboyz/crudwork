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
using crudwork.FileImporters.Specialized;
using System.Data;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// File file database converter.
	/// </summary>
	internal abstract class FlatFileDatabaseConverter : FileConverterBase<FixedLengthImportOptions>, IFileConverter
	{
		private FixedLengthFileReader engine = new FixedLengthFileReader();

		public FlatFileDatabaseConverter(string newlineChar)
			: base()	
		{
			Options.NewlineChar = newlineChar;
		}

		#region IFileConverter Members

		public System.Data.DataSet Import(string filename)
		{
			var ds = new DataSet();
			ds.Tables.Add(engine.Read(filename, Options));
			return ds;
		}

		public void Export(System.Data.DataSet ds, string filename)
		{
			if (ds == null || ds.Tables.Count == 0)
				throw new ArgumentNullException("ds");

			engine.Write(ds.Tables[0], Options, filename);
		}

		#endregion
	}
}
