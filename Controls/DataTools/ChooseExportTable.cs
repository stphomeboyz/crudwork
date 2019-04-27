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
using crudwork.Utilities;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	/// <summary>
	/// SaveDataSetAs - allows a dataset to be saved to a specified connection string
	/// </summary>
	[Obsolete("consider using DataExportWizard", true)]
	public partial class ChooseExportTable : Form
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ChooseExportTable()
		{
			InitializeComponent();
		}

		private void SaveDataSetAs_Load(object sender, EventArgs e)
		{
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				if (SelectedOptions.Length <= 0)
					throw new ArgumentException("No tables were selected for export.");

				var r = ConnectionStringBuilderResult;

				if (r.Provider == DatabaseProvider.Unspecified)
					throw new ArgumentException("Database Provider must be specified");

				if (string.IsNullOrEmpty(r.ConnectionString))
					throw new ArgumentException("Connection String must be specified");

				var df = new DataFactory(r.Provider, r.ConnectionString);
				df.TestConnection();

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

		/// <summary>
		/// Get or set the Options
		/// </summary>
		public Dictionary<string, bool> Options
		{
			get
			{
				return chooseListBox1.Options;
			}
			set
			{
				chooseListBox1.Options = value;
			}
		}

		/// <summary>
		/// Get the selected options
		/// </summary>
		public string[] SelectedOptions
		{
			get
			{
				return chooseListBox1.SelectedOptions;
			}
		}

		/// <summary>
		/// Get the available options
		/// </summary>
		public string[] AvailableOptions
		{
			get
			{
				return chooseListBox1.AvailableOptions;
			}
		}

		/// <summary>
		/// Get or set the Connection String Builder Result
		/// </summary>
		public DataConnectionInfo ConnectionStringBuilderResult
		{
			get
			{
				var result = connectionStringForm1.DataConnectionInfo;
				result["TableStem"] = txtTableStem.Text;
				return result;
			}
			set
			{
				connectionStringForm1.DataConnectionInfo = value ?? new DataConnectionInfo();
			}
		}
	}
}
