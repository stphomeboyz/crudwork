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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Controls;
using crudwork.DataAccess;
using crudwork.Utilities;
using crudwork.Models.DataAccess;
using crudwork.FileImporters;
using System.IO;

namespace crudwork.Controls.FormControls
{
	/// <summary>
	/// Connection String TextBox (Form-style)
	/// </summary>
	public partial class ConnectionStringForm : UserControl
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ConnectionStringForm()
		{
			InitializeComponent();
			ControlUtil.PopulateControl(cboSourceType, typeof(InputSource));
			ControlUtil.PopulateControl(cboDatabaseProvider, typeof(DatabaseProvider));
			cboDatabaseProvider.Items.Remove(DatabaseProvider.Unspecified);
		}

		private void ConnectionStringUC_Load(object sender, EventArgs e)
		{
		}

		private void cboSourceType_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selected = (InputSource)cboSourceType.SelectedItem;

			switch (selected)
			{
				case InputSource.Database:
					cboDatabaseProvider.Enabled = true;
					txtConnectionString.Enabled = true;
					txtFilename.Enabled = false;
					txtOptions.Enabled = false;
					break;
	
				//case SourceType.ComplexDataFile:
				//case SourceType.SimpleDataFile:
				case InputSource.File:
					cboDatabaseProvider.Enabled = false;
					txtConnectionString.Enabled = false;
					txtFilename.Enabled = true;
					txtOptions.Enabled = true;
					break;

				default:
					break;
			}


		}

		private void txtConnectionString_ButtonClicked(object sender, EventArgs e)
		{
			DataConnectionInfo = ControlManager.ShowConnectionStringBuilder(ConnectionString, Provider, true);
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

		#region Properties
		/// <summary>
		/// Get or set the field name
		/// </summary>
		public string Fieldname
		{
			get
			{
				return lblFieldname.Text;
			}
			set
			{
				lblFieldname.Text = value;
			}
		}

		/// <summary>
		/// Get or set the filename
		/// </summary>
		private string Filename
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
		private string Options
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
		/// Get or set the connection string value
		/// </summary>
		private string ConnectionString
		{
			get
			{
				return txtConnectionString.Value;
			}
			set
			{
				txtConnectionString.Value = value;
			}
		}
		/// <summary>
		/// Get or set the database provider
		/// </summary>
		private DatabaseProvider Provider
		{
			get
			{
				if (cboDatabaseProvider.Items.Count == 0)
					return DatabaseProvider.Unspecified;
				return (DatabaseProvider)cboDatabaseProvider.SelectedItem;
			}
			set
			{
				ControlUtil.SelectItem<DatabaseProvider>(cboDatabaseProvider, value);
			}
		}
		/// <summary>
		/// Get or set the source type
		/// </summary>
		private InputSource SourceType
		{
			get
			{
				if (cboSourceType.Items.Count == 0)
					return InputSource.File;
				return (InputSource)cboSourceType.SelectedItem;
			}
			set
			{
				ControlUtil.SelectItem<InputSource>(cboSourceType, value);
				cboSourceType_SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// get or set the database connection result
		/// </summary>
		public DataConnectionInfo DataConnectionInfo
		{
			get
			{
				if (SourceType == InputSource.Database)
					return new DataConnectionInfo(Provider, ConnectionString);
				else
					return new DataConnectionInfo(Filename, Options);
			}
			set
			{
				if (value == null)
					value = new DataConnectionInfo();

				if ((object)value == null)
					value = new DataConnectionInfo();

				ConnectionString = value.ConnectionString;
				Provider = value.Provider;
			}
		}
		#endregion
	}
}
