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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using crudwork.Utilities;
using crudwork.FileImporters;
using crudwork.DynamicRuntime;
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	/// <summary>
	/// A FileImport Builder
	/// </summary>
	public partial class FileImportBuilderDialogBox : Form
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public FileImportBuilderDialogBox()
		{
			InitializeComponent();
		}

		#region Application Events
		private void FileImportBuilderDialogBox_Load(object sender, EventArgs e)
		{
		}

		private void txtFilename_OnChanged(object sender, crudwork.Controls.Base.ChangeEventArgs e)
		{
			if (!File.Exists(Filename))
				return;
		}

		private void txtFilename_ButtonClicked(object sender, EventArgs e)
		{
			using (var d = new OpenFileDialog())
			{
				d.FileName = Filename;
				d.Filter = ImportManager.GetSupportedFilters();
				if (d.ShowDialog() != DialogResult.OK)
					return;
				if (Filename == d.FileName)
					return;
				Filename = d.FileName;
				Options = string.Empty;
			}
		}

		private void txtOptions_ButtonClicked(object sender, EventArgs e)
		{
			try
			{
				#region Sanity Checks
				if (string.IsNullOrEmpty(Filename))
					throw new ArgumentNullException("Filename");
				string ext = Path.GetExtension(Filename).Substring(1).ToUpper();
				if (!ImportManager.IsSupportedExtension(ext))
					throw new ArgumentException("File extension not supported: " + ext);
				#endregion

				var t = ImportManager.GetOptionType(ext);
				if (t == null)
					throw new ArgumentException("Option type not found for extension: " + ext);

				#region Create DataTable
				var dt = new DataTable("Options");
				dt.Columns.Add("Key", typeof(string)).ReadOnly = true;
				dt.Columns.Add("Value", typeof(string));
				dt.Columns.Add("DataType", typeof(string)).ReadOnly = true;

				var piList = t.GetProperties();
				foreach (var item in piList)
				{
					var dr = dt.NewRow();
					dr["Key"] = item.Name;
					dr["Value"] = DBNull.Value;
					dr["DataType"] = item.PropertyType.Name;
					dt.Rows.Add(dr);
				}
				#endregion

				using (var d = new DynamicEditorForm())
				{
					d.Caption = "Import Option(s) Editor";
					d.DataSource = dt;
					if (d.ShowDialog() != DialogResult.OK)
						return;
				}

				var sb = new StringBuilder();

				foreach (DataRow dr in dt.Rows)
				{
					if (DataConvert.IsNull(dr["Value"]))
						continue;

					var key = dr["Key"].ToString();
					var val = dr["Value"].ToString();
					var type = dr["DataType"].ToString();

					if (type.Equals("String", StringComparison.InvariantCultureIgnoreCase))
						val = string.Format("\"{0}\"", val);

					if (sb.Length > 0)
						sb.Append("; ");

					sb.AppendFormat("{0}={1}", key, val);
				}

				Options = sb.ToString();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnOkay_Click(object sender, EventArgs e)
		{
			try
			{
				if (!File.Exists(Filename))
					throw new FileNotFoundException("File not found: " + Filename);

				string ext = Path.GetExtension(Filename).Substring(1).ToUpper();
				if (!ImportManager.IsSupportedExtension(ext))
					throw new NotSupportedException("File extension not supported: " + ext);

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the Filename
		/// </summary>
		public string Filename
		{
			get
			{
				return txtFilename.Value;
			}
			set
			{
				txtFilename.Value = value;
			}
		}
		/// <summary>
		/// Get or set the Options
		/// </summary>
		public string Options
		{
			get
			{
				return txtOptions.Value;
			}
			set
			{
				txtOptions.Value = value;
			}
		}
		/// <summary>
		/// Get the database connection
		/// </summary>
		public DataConnectionInfo DatabaseConnection
		{
			get
			{
				if (!File.Exists(Filename))
					return new DataConnectionInfo();

				//var sourceType = ImportManager.GetSourceType(Filename);
				return new DataConnectionInfo(Filename, Options);
			}
		}
		#endregion
	}
}
