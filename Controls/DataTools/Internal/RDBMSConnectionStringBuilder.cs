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
using System.Text;
using System.Windows.Forms;
using crudwork.DataAccess;
using System.Diagnostics;
using crudwork.Utilities;
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	internal partial class RDBMSConnectionStringBuilder : UserControl, IConnectionStringBuilder
	{
		public event EventHandler OnChange = null;
		private ConnectionStringManager csm = new ConnectionStringManager();
		private DatabaseDefinitionList dtDatabase = null;

		public RDBMSConnectionStringBuilder()
		{
			InitializeComponent();
			MaskPassword = true;
			IntegratedSecurity = true;
		}

		#region Event Handlers
		private void chkIntegratedSecurity_Click(object sender, EventArgs e)
		{
			txtUserID.Enabled = !chkIntegratedSecurity.Checked;
			txtPassword.Enabled = !chkIntegratedSecurity.Checked;
			//chkMaskPassword.Enabled = !chkIntegratedSecurity.Checked;
		}

		private void chkMaskPassword_Click(object sender, EventArgs e)
		{
			txtPassword.PasswordChar = chkMaskPassword.Checked ? '*' : '\0';
		}

		private void cboDatabase_Enter(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				int selectedIndex = cboDatabase.SelectedIndex;

				csm.Service = txtService.Text;
				csm.Userid = txtUserID.Text;
				csm.Password = txtPassword.Text;
				csm.IsIntegratedSecurity = chkIntegratedSecurity.Checked;

				//connectionStringManager.OtherPairs = string.Empty;
				PopulateDatabaseList();

				cboDatabase.SelectedIndex = selectedIndex;
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

		private void txtService_TextChanged(object sender, EventArgs e)
		{
			RaiseOnChangeEvent(sender);
		}

		private void txtUserID_TextChanged(object sender, EventArgs e)
		{
			RaiseOnChangeEvent(sender);
		}

		private void txtPassword_TextChanged(object sender, EventArgs e)
		{
			RaiseOnChangeEvent(sender);
		}

		private void cboDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaiseOnChangeEvent(sender);
		}

		private void txtOthers_TextChanged(object sender, EventArgs e)
		{
			RaiseOnChangeEvent(sender);
		}

		private void btnBrowserService_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				using (SelectListBox d = new SelectListBox())
				{
					var sdl = DataFactory.GetService(DatabaseProvider.SqlClient);

					if (sdl == null || sdl.Count == 0)
					{
						MessageBox.Show("No services were found.");
						return;
					}

					string[] items = new string[sdl.Count];
					for (int i = 0; i < sdl.Count; i++)
					{
						items[i] = sdl[i].ServiceName;
					}
					d.Clear();
					d.Items = items;

					d.SelectedItem = txtService.Text;
					d.StartPosition = FormStartPosition.CenterParent;
					if (d.ShowDialog() != DialogResult.OK)
						return;

					//DataRowView drv = (DataRowView)d.SelectedItem;
					//txtService.Text = drv[d.ValueMember].ToString();
					txtService.Text = d.SelectedItem.ToString();
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
		#endregion

		#region Helpers
		private void RaiseOnChangeEvent(object sender)
		{
			if (OnChange == null)
				return;

			OnChange(sender, EventArgs.Empty);
		}

		private void PopulateDatabaseList()
		{
			try
			{
				string connectionString = csm.ConnectionString;
				if (string.IsNullOrEmpty(connectionString))
					return;

				DataFactory dataFactory = new DataFactory(connectionString);
				dtDatabase = dataFactory.Database.GetDatabases(true);
				cboDatabase.DataSource = dtDatabase;
				cboDatabase.DisplayMember = "name";
				cboDatabase.ValueMember = "name";
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				cboDatabase.DataSource = null;
				cboDatabase.Items.Clear();
				throw;
			}
		}

		private string Database
		{
			get
			{
				DatabaseDefinition dd = cboDatabase.SelectedItem as DatabaseDefinition;
				if (dd == null)
					throw new ArgumentException("selected item is not a DatabaseDefinition type");
				return dd.Name;
			}
			set
			{
				SelectComboBoxItem(cboDatabase, value);
			}
		}

		private bool IntegratedSecurity
		{
			get
			{
				return chkIntegratedSecurity.Checked;
			}
			set
			{
				// chkIntegratedSecurity.Checked = connectionStringManager.IsIntegratedSecurity;
				chkIntegratedSecurity.Checked = value;
				chkIntegratedSecurity_Click(this, EventArgs.Empty);
			}
		}

		private void SelectComboBoxItem(ComboBox comboBox, string value)
		{
			if (dtDatabase == null)
				return;

			value = value.ToUpper();
			for (int i = 0; i < comboBox.Items.Count; i++)
			{
				DatabaseDefinition dd = comboBox.Items[i] as DatabaseDefinition;
				if (!value.Equals(dd.Name, StringComparison.InvariantCultureIgnoreCase))
					continue;

				comboBox.SelectedIndex = i;
				return;
			}

			comboBox.SelectedIndex = -1;
		}
		#endregion

		#region IConnectionStringBuilder Members
		public string ConnectionString
		{
			get
			{
				return csm.ConnectionString;
			}
			set
			{
				try
				{
					csm.ConnectionString = value;
					if (string.IsNullOrEmpty(value))
						return;

					txtService.Text = csm.Service;
					txtUserID.Text = csm.Userid;
					txtPassword.Text = csm.Password;
					IntegratedSecurity = csm.IsIntegratedSecurity;
					txtOthers.Text = csm.OtherPairsFormatted();

					PopulateDatabaseList();
					SelectComboBoxItem(cboDatabase, csm.Database);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					throw;
				}
			}
		}

		public bool MaskPassword
		{
			get
			{
				return chkMaskPassword.Checked;
			}
			set
			{
				chkMaskPassword.Checked = value;
				chkMaskPassword_Click(this, EventArgs.Empty);
			}
		}

		public void TestConnection()
		{
			try
			{
				csm.Service = txtService.Text;
				csm.Database = Database;
				csm.Userid = txtUserID.Text;
				csm.Password = txtPassword.Text;
				csm.IsIntegratedSecurity = chkIntegratedSecurity.Checked;

				DataFactory dataFactory = new DataFactory(DatabaseProvider, csm.ConnectionString);
				dataFactory.TestConnection();
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "Service", txtService.Text);
				DebuggerTool.AddData(ex, "Database", Database);
				DebuggerTool.AddData(ex, "Userid", txtUserID.Text);
				DebuggerTool.AddData(ex, "Password", txtPassword.Text);
				DebuggerTool.AddData(ex, "IsIntegratedSecurity", chkIntegratedSecurity.Checked);
				throw;
			}
		}

		public DatabaseProvider DatabaseProvider
		{
			get;
			set;
		}
		#endregion

		#region IConnectionStringBuilder Members

		string IConnectionStringBuilder.ConnectionString
		{
			get
			{
				return ConnectionString;
			}
			set
			{
				ConnectionString = value;
			}
		}

		bool IConnectionStringBuilder.MaskPassword
		{
			get
			{
				return MaskPassword;
			}
			set
			{
				MaskPassword = value;
			}
		}

		void IConnectionStringBuilder.TestConnection()
		{
			TestConnection();
		}

		DatabaseProvider IConnectionStringBuilder.DatabaseProvider
		{
			get
			{
				return DatabaseProvider;
			}
			set
			{
				DatabaseProvider = value;
			}
		}

		#endregion
	}
}
