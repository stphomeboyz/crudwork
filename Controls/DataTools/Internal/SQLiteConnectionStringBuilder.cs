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
using crudwork.Utilities;
using crudwork.Models.DataAccess;

namespace crudwork.Controls.DataTools.Internal
{
	internal partial class SQLiteConnectionStringBuilder : UserControl, IConnectionStringBuilder
	{
		ConnectionStringManager csm = new ConnectionStringManager();
		public SQLiteConnectionStringBuilder()
		{
			InitializeComponent();
		}

		#region Application Events
		private void btnBrowseFile_Click(object sender, EventArgs e)
		{
			using (var d = new OpenFileDialog())
			{
				d.FileName = Filename;

				d.CheckFileExists = true;
				if (d.ShowDialog() != DialogResult.OK)
					return;

				Filename = d.FileName;
			}
		}

		private void chkMaskPassword_CheckedChanged(object sender, EventArgs e)
		{
			txtPassword.PasswordChar = chkMaskPassword.Checked ? '*' : '\0';
		}
		#endregion

		#region Properties
		private string Filename
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
		private string Password
		{
			get
			{
				return txtPassword.Text;
			}
			set
			{
				txtPassword.Text = value;
			}
		}
		private bool ReadOnly
		{
			get
			{
				return chkReadOnly.Checked;
			}
			set
			{
				chkReadOnly.Checked = value;
			}
		}
		private bool FileMustExist
		{
			get
			{
				return chkFileMustExist.Checked;
			}
			set
			{
				chkFileMustExist.Checked = value;
			}
		}
		private bool Compress
		{
			get
			{
				return chkCompress.Checked;
			}
			set
			{
				chkCompress.Checked = value;
			}
		}
		#endregion

		#region IConnectionStringBuilder Members

		public string ConnectionString
		{
			get
			{
				return ConnectionStringManager.MakeSQLite(Filename, Password, ReadOnly, FileMustExist, Compress);
			}
			set
			{
				csm.ConnectionString = value;
				Filename = csm.Service;
				Password = csm.Password;
				ReadOnly = DataConvert.ToBoolean(csm["read only"]);
				FileMustExist = DataConvert.ToBoolean(csm["failifmissing"]);
				Compress = DataConvert.ToBoolean(csm["compress"]);
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
				chkMaskPassword_CheckedChanged(this, EventArgs.Empty);
			}
		}

		public void TestConnection()
		{
			try
			{
				/*
				csm.Service = Filename;

				csm.IsIntegratedSecurity = false;
				csm.Password = Password;

				csm.SetOtherPair("read only", ReadOnly ? "True" : "False");
				csm.SetOtherPair("failifmissing", FileMustExist ? "True" : "False");
				*/
				DataFactory dataFactory = new DataFactory(DatabaseProvider, ConnectionString);
				dataFactory.TestConnection();
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "Filename", Filename);
				DebuggerTool.AddData(ex, "Password", Password);
				DebuggerTool.AddData(ex, "ReadOnly", ReadOnly);
				DebuggerTool.AddData(ex, "FileMustExist", FileMustExist);
				DebuggerTool.AddData(ex, "Compress", Compress);
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
