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

namespace crudwork.FileImporters
{
	/// <summary>
	/// List of supported extensions
	/// </summary>
	[Obsolete("no longer use this", true)]
	internal enum SupportedExtensions
	{
		/// <summary>Comma Separated Variable file</summary>
		CSV,
		/// <summary>DBF file</summary>
		DBF,
		/// <summary>Microsoft Access Database</summary>
		MDB,
		/// <summary>Tab delimited file</summary>
		//TXT,
		///// <summary>Tab delimited file</summary>
		TAB,
		/// <summary>Microsoft Excel spreadsheet</summary>
		XLS,
		/// <summary>XML file</summary>
		XML,
		/// <summary>Extension Is Not supported</summary>
		ExtensionNotSupported,
	}
}
