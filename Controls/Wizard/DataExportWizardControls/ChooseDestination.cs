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
using crudwork.Models.DataAccess;
using crudwork.DataAccess;
using crudwork.FileImporters;

namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	/// <summary>
	/// Choose the output destination
	/// </summary>
	public partial class ChooseDestination : UserControl, IWizardControl
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ChooseDestination()
		{
			InitializeComponent();
		}

		private void ChooseDestination_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "Choose the Destination";
		}

		/// <summary>
		/// get or set the database connection result
		/// </summary>
		public DataConnectionInfo DatabaseConnection
		{
			get
			{
				return connectionStringForm1.DataConnectionInfo;
			}
			set
			{
				connectionStringForm1.DataConnectionInfo = value;
			}
		}

		/// <summary>
		/// return a string presentation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Destination = " + DatabaseConnection);
		}

		#region IWizardControl Members

		/// <summary>
		/// Refresh the content
		/// </summary>
		public void RefreshContent()
		{
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Validate the content
		/// </summary>
		public void ValidateContent()
		{
			if (DatabaseConnection.InputSource == InputSource.Database)
			{
				var df = new DataFactory(DatabaseConnection.Provider, DatabaseConnection.ConnectionString);
				df.TestConnection();
			}
			else
			{
				var opt = DatabaseConnection.Options; // + "; ImportMaxRows=1";
				using (var ds = ImportManager.Import(DatabaseConnection.Filename, opt))
				{
				}
			}
		}

		#endregion
	}
}
