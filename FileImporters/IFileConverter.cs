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
using System.Data;

namespace crudwork.FileImporters
{
	/// <summary>
	/// Interface for importing a filename to a DataSet, and exporting a DataSet to a filename.
	/// </summary>
	internal interface IFileConverter
	{
		/// <summary>
		/// Import the filename to a DataSet
		/// </summary>
		/// <returns></returns>
		/// <param name="filename"></param>
		DataSet Import(string filename);

		/// <summary>
		/// Export a DataSet to a filename
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="filename"></param>
		void Export(DataSet ds, string filename);

		/// <summary>
		/// Set the user-defined options (or replace the default settings with the user-defined values)
		/// </summary>
		/// <remarks>
		/// This method is not designed to be consumed by the caller.  This method will be used
		/// exclusively by the ImportManager to pass-over the options to the implementing engine.
		/// </remarks>
		/// <param name="options"></param>
		void SetOptions(ConverterOptionList options);
	}
}
