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
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using crudwork.Controls.Wizard;
using crudwork.Utilities;
using crudwork.Controls;
using crudwork.DataAccess;
using crudwork.FileImporters;
using crudwork.Models.DataAccess;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	/// <summary>
	/// Wizard Step 1: Choose Input Data Source
	/// </summary>
	internal partial class ChooseInputSource : UserControl, IWizardControl
	{
		public ChooseInputSource()
		{
			InitializeComponent();
			ControlUtil.PopulateControl(cboDataSource, typeof(InputDataSourceType));
		}

		#region Events
		private void WizardStep1_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "Step 1: Choose Input Data Source";
		}

		private void cboDataSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (InputDataSource)
			{
				case InputDataSourceType.File:
					EnableGroup("Filename");
					break;
				case InputDataSourceType.Database:
					EnableGroup("ConnectionString", "Tablename");
					break;
				default:
					throw new ArgumentOutOfRangeException("InputDataSource=" + InputDataSource);
			}
		}

		private void btnBrowseFile_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				using (var d = new OpenFileDialog())
				{
					d.Filter = ImportManager.GetSupportedFilters();
					d.FileName = Filename;
					d.Title = "Open Data File...";
					//d.Filter = "";

					if (d.ShowDialog() != DialogResult.OK)
						return;

					Filename = d.FileName;
					if (FileSupportsTables(Filename))
					{
						EnableGroup("Filename", "Tablename");
						UpdateTableList();
					}
					else
					{
						EnableGroup("Filename");
					}
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		private void btnBuildConnectionString_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				var result = ControlManager.ShowConnectionStringBuilder(ConnectionString, Provider, true);
				Provider = result.Provider;
				ConnectionString = result.ConnectionString;
				UpdateTableList();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		private void cboTablename_Enter(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				UpdateTableList();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		#endregion

		#region Properties
		public InputDataSourceType InputDataSource
		{
			get
			{
				return (InputDataSourceType)cboDataSource.SelectedItem;
			}
			set
			{
				ControlUtil.SelectItem<InputDataSourceType>(cboDataSource, value);
			}
		}

		public string Filename
		{
			get
			{
				return txtFilename.Text;
			}
			set
			{
				txtFilename.Text = value;
			}
		}

		public DatabaseProvider Provider
		{
			get;
			set;
		}

		public string ConnectionString
		{
			get
			{
				return txtConnectionString.Text;
			}
			set
			{
				txtConnectionString.Text = value;
			}
		}

		public string Tablename
		{
			get
			{
				if (cboTablename.SelectedItem == null)
					return string.Empty;
				else return cboTablename.SelectedItem.ToString();
				//return txtTablename.Text;
			}
			set
			{
				if (cboTablename.Items.Count == 0)
					UpdateTableList();
				ControlUtil.SelectItem<string>(cboTablename, value);
			}
		}

		public override string ToString()
		{
			return string.Format("InputDataSource={0} Filename={1} Provider={2} ConnectionString={3} Tablename={4}",
				InputDataSource, Filename, Provider, ConnectionString, Tablename);
		}
		#endregion

		private void UpdateTableList()
		{
			switch (InputDataSource)
			{
				case InputDataSourceType.File:
					if (FileSupportsTables(Filename))
					{
						switch (GetFileExtension(Filename))
						{
							case "MDB":
								Provider = DatabaseProvider.OleDb;
								ConnectionString = ConnectionStringManager.MakeAccess(Filename, null, null, true);
								break;
							case "XLS":
								Provider = DatabaseProvider.OleDb;
								ConnectionString = ConnectionStringManager.MakeExcel(Filename, null, null, true, true);
								break;
							case "XLSX":
								Provider = DatabaseProvider.OleDb;
								ConnectionString = ConnectionStringManager.MakeExcel2007(Filename, null, null, true, true);
								break;
							default:
								throw new ArgumentException("File extension not supported: " + Filename);
						}
						GetTableListing(Provider, ConnectionString);
					}
					break;

				case InputDataSourceType.Database:
					GetTableListing(Provider, ConnectionString);
					break;

				default:
					throw new ArgumentOutOfRangeException("InputDataSource=" + InputDataSource);
			}
		}

		private string GetFileExtension(string filename)
		{
			return Path.GetExtension(filename).Substring(1).ToUpper();
		}

		private void GetTableListing(DatabaseProvider provider, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				return;

			var df = new DataFactory(provider, connectionString);
			var tdl = df.Database.GetTables();

			int selected = cboTablename.SelectedIndex;

			cboTablename.Items.Clear();
			foreach (var item in tdl)
			{
				cboTablename.Items.Add(item.TableName);
			}

			if (cboTablename.Items.Count > 0)
				cboTablename.SelectedIndex = selected == -1 ? 0 : Math.Min(selected, cboTablename.Items.Count - 1);
		}

		private void EnableGroup(params string[] group)
		{
			if (group == null || group.Length == 0)
				throw new ArgumentNullException("group");

			Array.Sort(group);

			foreach (Control c in this.Controls)
			{
				string g = DataConvert.ToString(c.Tag, string.Empty);
				if (string.IsNullOrEmpty(g))
					continue;
				c.Enabled = StringUtil.Search(group, g) != -1;
			}
		}

		public bool FileSupportsTables(string filename)
		{
			return "MDB|XLS|XLSX".Contains(GetFileExtension(Filename));
		}

		#region IWizardControl Members

		void IWizardControl.ValidateContent()
		{
			switch (InputDataSource)
			{
				case InputDataSourceType.File:
					if (string.IsNullOrEmpty(Filename))
						throw new ArgumentNullException("Filename", "File name is required");
					if (!File.Exists(Filename))
						throw new FileNotFoundException("File not found", Filename);
					break;

				case InputDataSourceType.Database:
					var df = new DataFactory(Provider, ConnectionString);
					df.TestConnection();

					var tdl = df.Database.GetTables(Tablename, QueryFilter.Exact);
					if (tdl.Count != 1)
						throw new ArgumentException("Table not found: " + Tablename);
					break;

				default:
					throw new ArgumentOutOfRangeException("InputDataSource=" + InputDataSource);
			}
		}

		void IWizardControl.RefreshContent()
		{
			//throw new NotImplementedException();
		}

		#endregion
	}
}
