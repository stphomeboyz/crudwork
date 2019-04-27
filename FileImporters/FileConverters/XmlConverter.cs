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
using System.Data;
using crudwork.Utilities;

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// Xml File Converter
	/// </summary>
	internal class XmlConverter : FileConverterBase<XmlImportOptions>, IFileConverter
	{
		public XmlConverter()
			: base()
		{
		}

		private DataSet ImportXML(string filename, bool handleMultipleRoots)
		{
			try
			{
				#region Handle XML files with Multiple Roots
				if (handleMultipleRoots)
				{
					// TODO: Need to be improved.  This approach could cause performance issues.
					var s = new StringBuilder();
					s.Append(File.ReadAllText(filename));
					s.Insert(0, "<root>");
					s.Append("</root>");

					filename = FileUtil.CreateTempFile("xml", s.ToString());
				}
				#endregion

				var ds = new DataSet();
				ds.ReadXml(filename, XmlReadMode.Auto);
				return ds;
			}
			catch (Exception ex)
			{
				// special handle for dealing with XML with multiple roots.
				if (ex.Message.Contains("There are multiple root elements") && !handleMultipleRoots)
					return ImportXML(filename, true);
				throw;
			}
		}
		private void ExportXML(string filename, DataSet ds)
		{
			if (Options.UseMappingType != MappingType.Element)
			{
				// create a new copy to avoid changing the original data set.
				ds = ds.Copy();
				DataUtil.SetColumnMapping(ds, Options.UseMappingType);
			}

			ds.WriteXml(filename, Options.UseXmlWriteMode);
		}

		#region IDataSetConverter Members

		System.Data.DataSet IFileConverter.Import(string filename)
		{
			return ImportXML(filename, false);
		}

		void IFileConverter.Export(System.Data.DataSet ds, string filename)
		{
			ExportXML(filename, ds);
		}

		#endregion
	}
}
