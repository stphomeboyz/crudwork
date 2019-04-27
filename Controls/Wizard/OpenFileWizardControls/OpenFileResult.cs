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
//using System.Linq;
using System.Text;
using crudwork.DataAccess;
using System.Data;
using System.Windows.Forms;
using crudwork.Models.DataAccess;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	/// <summary>
	/// The return result of a OpenFileWizard
	/// </summary>
	public class OpenFileResult
	{
		/// <summary>
		/// the form's dialog result
		/// </summary>
		[Obsolete("not being used", true)]
		public DialogResult DialogResult
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set source of the input data
		/// </summary>
		public InputDataSourceType InputDataSource
		{
			get;
			set;
		}
		/// <summary>
		/// Get or set the filename.  Use this property if the data input came from a File.
		/// </summary>
		public string Filename
		{
			get;
			set;
		}
		/// <summary>
		/// Get or se ta value indicating the file supports multiple tables.  Use this property if the data input came from a File.
		/// </summary>
		public bool FileSupportsTables
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the database provider id.  Use this property if the data input came from a Database.
		/// </summary>
		public DatabaseProvider Provider
		{
			get;
			set;
		}
		/// <summary>
		/// Get or set the connection string.  Use this property if the data input came from a Database.
		/// </summary>
		public string ConnectionString
		{
			get;
			set;
		}
		/// <summary>
		/// Get or set the tablename.  Use this property if the data input came from a Database.
		/// </summary>
		public string Tablename
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the data column mapping.
		/// </summary>
		public DataTable DataColumnMapping
		{
			get;
			set;
		}

		/// <summary>
		/// the client column.  This column is used to map to the destination column
		/// </summary>
		public string SourceColumn
		{
			get;
			set;
		}

		/// <summary>
		/// The field.
		/// </summary>
		public string DestinationColumn
		{
			get;
			set;
		}
	}
}
