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

namespace crudwork.FileImporters.FileConverters
{
	/// <summary>
	/// Microsoft Excel Converter
	/// </summary>
	internal class Excel2007Converter : ExcelConverter, IFileConverter
	{
		public Excel2007Converter()
			:base()
		{
			/*
			 * System.InvalidOperationException: The 'Microsoft.ACE.OLEDB.12.0' provider is not registered on the local machine.
			 * 
			 * need to install this driver, if you don't have Office 2007
			 * 2007 Office System Driver: Data Connectivity Components
			 * 
			 * http://www.microsoft.com/downloads/en/confirmation.aspx?familyId=7554f536-8c28-4598-9b72-ef94e038c891&displayLang=en
			 */
			Options.Provider = "Microsoft.ACE.OLEDB.12.0";
		}
	}
}
