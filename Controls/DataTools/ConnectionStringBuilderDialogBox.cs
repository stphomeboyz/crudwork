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
using System.Text;
using System.Windows.Forms;
using crudwork.Utilities;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	/// <summary>
	/// The connection string builder dialog box
	/// </summary>
	public partial class ConnectionStringBuilderDialogBox : Form
	{
		private IConnectionStringBuilder csb = null;

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public ConnectionStringBuilderDialogBox()
		{
			InitializeComponent();
			ControlUtil.PopulateControl(cboDatabaseProvider, typeof(DatabaseProvider));
			cboDatabaseProvider.Items.Remove(DatabaseProvider.Unspecified);
		}

		private void ConnectionStringBuilderDialogBox_Load(object sender, EventArgs e)
		{
			rdbmsConnectionStringBuilder1.Dock = DockStyle.Fill;
			sqLiteConnectionStringBuilder1.Dock = DockStyle.Fill;
		}

		private void connectionStringBuilder1_OnChange(object sender, EventArgs e)
		{
			//btnOkay.Enabled = false;
		}

		#region Application Events
		private void btnTest_Click(object sender, EventArgs e)
		{
			if (csb == null)
				return;

			try
			{
				FormUtil.Busy(this, true);
				csb.TestConnection();
				MessageBox.Show("Connection was established successfully.");
				//btnOkay.Enabled = true;
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

		private void btnOkay_Click(object sender, EventArgs e)
		{
			if (csb == null)
				return;

			try
			{
				FormUtil.Busy(this, true);
				csb.TestConnection();
				this.DialogResult = DialogResult.OK;
				this.Close();
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

		private void btnCancel_Click(object sender, EventArgs e)
		{
		}

		private void cboDataProvider_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				csb = null;
				var provider = (DatabaseProvider)cboDatabaseProvider.SelectedItem;

				switch (provider)
				{
					case DatabaseProvider.Odbc:
					case DatabaseProvider.OleDb:
					case DatabaseProvider.OracleClient:
					case DatabaseProvider.OracleDataProvider:
					case DatabaseProvider.SqlClient:
						sqLiteConnectionStringBuilder1.Visible = false;
						rdbmsConnectionStringBuilder1.Visible = true;
						csb = rdbmsConnectionStringBuilder1;
						csb.DatabaseProvider = provider;
						break;

					case DatabaseProvider.SQLite:
						sqLiteConnectionStringBuilder1.Visible = true;
						rdbmsConnectionStringBuilder1.Visible = false;
						csb = sqLiteConnectionStringBuilder1;
						csb.DatabaseProvider = provider;
						break;

					case DatabaseProvider.Unspecified:
						throw new ArgumentException("Provider cannot be Unspecified!");

					default:
						throw new ArgumentOutOfRangeException("provider=" + provider);
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		/// <summary>
		/// Get or set the connection strig
		/// </summary>
		public string ConnectionString
		{
			get
			{
				if (csb == null)
					return string.Empty;

				return csb.ConnectionString;
			}
			set
			{
				if (csb == null)
					return;

				csb.ConnectionString = value;
			}
		}

		/// <summary>
		/// Show or hide the password using mask characters
		/// </summary>
		public bool MaskPassword
		{
			get
			{
				if (csb == null)
					return false;

				return csb.MaskPassword;
			}
			set
			{
				if (csb == null)
					return;

				csb.MaskPassword = value;
			}
		}

		/// <summary>
		/// Get or set the selected database provider
		/// </summary>
		public DatabaseProvider SelectedDatabaseProvider
		{
			get
			{
				if (cboDatabaseProvider.SelectedItem == null)
					return DatabaseProvider.Unspecified;

				return (DatabaseProvider)cboDatabaseProvider.SelectedItem;
			}
			set
			{
				for (int i = 0; i < cboDatabaseProvider.Items.Count; i++)
				{
					if (value != (DatabaseProvider)cboDatabaseProvider.Items[i])
						continue;

					cboDatabaseProvider.SelectedIndex = i;
					break;
				}
			}
		}

		/// <summary>
		/// FALSE - do not allow user to change the provider; TRUE - allow user to change the provider
		/// </summary>
		public bool AllowUserToChangeProvider
		{
			get
			{
				return cboDatabaseProvider.Enabled;
			}
			set
			{
				cboDatabaseProvider.Enabled = value;
			}
		}

		/// <summary>
		/// Get or set the database connection instance
		/// </summary>
		public DataConnectionInfo DatabaseConnection
		{
			get
			{
				return new DataConnectionInfo(SelectedDatabaseProvider, ConnectionString);
			}
			set
			{
				if (value == null)
					value = new DataConnectionInfo();

				SelectedDatabaseProvider = value.Provider;
				ConnectionString = value.ConnectionString;
			}
		}
	}
}