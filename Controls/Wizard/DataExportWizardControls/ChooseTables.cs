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
using crudwork.DataAccess;
using crudwork.Models.DataAccess;
using crudwork.Utilities;
using crudwork.FileImporters;

namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	/// <summary>
	/// Choose the table using the Choose ListBox
	/// </summary>
	public partial class ChooseTables : UserControl, IWizardControl
	{
		private ChooseSource chooseSource = null;
		private DataConnectionInfo lastDbConn = null;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ChooseTables()
		{
			InitializeComponent();
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="source"></param>
		public ChooseTables(ChooseSource source)
			: this()
		{
			this.chooseSource = source;
		}

		private void ChooseTables_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "Choose the table(s) to export";
		}

		/// <summary>
		/// Get or set the options
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
		/// Get the non-selected options
		/// </summary>
		public string[] AvailableOptions
		{
			get
			{
				return chooseListBox1.AvailableOptions;
			}
		}

		/// <summary>
		/// return a string presentation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Table(s)    = " + StringUtil.StringArrayToString(SelectedOptions, ", "));
		}

		#region IWizardControl Members

		/// <summary>
		/// Refresh the content
		/// </summary>
		public void RefreshContent()
		{
			if (chooseSource == null || chooseSource.DatabaseConnection == lastDbConn)
				return;

			lastDbConn = chooseSource.DatabaseConnection;
			var options = new Dictionary<string, bool>();


			if (lastDbConn.InputSource == InputSource.Database)
			{
				var df = new DataFactory(lastDbConn.Provider, lastDbConn.ConnectionString);
				foreach (var item in df.Database.GetTables())
				{
					options.Add(item.TableName, false);
				}
			}
			else
			{
				var opt = lastDbConn.Options; // + "; ImportMaxRows=1";
				using (var ds = ImportManager.Import(lastDbConn.Filename, opt))
				{
					foreach (DataTable dt in ds.Tables)
					{
						options.Add(dt.TableName, false);
					}
				}
			}

			Options = options;
		}

		/// <summary>
		/// Validate the content
		/// </summary>
		public void ValidateContent()
		{
			if (SelectedOptions.Length == 0)
				throw new ArgumentException("One or more table(s) must be selected.");
		}

		#endregion
	}
}
