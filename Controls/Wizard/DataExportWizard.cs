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
using crudwork.Controls.Wizard.DataExportWizardControls;
using crudwork.Models.DataAccess;

namespace crudwork.Controls.Wizard
{
	/// <summary>
	/// Pivot Table Wizard
	/// </summary>
	public partial class DataExportWizard : Wizard
	{
		private Introduction intro = null;
		private ChooseSource chooseSource = null;
		private ChooseTables chooseTables = null;
		private ChooseDestination chooseDestination = null;
		private Summary summary = null;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public DataExportWizard()
		{
			InitializeComponent();
			InitializeWizard();
		}

		private void InitializeWizard()
		{
			this.Title = "Data Export Wizard";

			intro = new Introduction();
			chooseSource = new ChooseSource();
			chooseTables = new ChooseTables(chooseSource);
			chooseDestination = new ChooseDestination();
			summary = new Summary(chooseSource, chooseTables, chooseDestination);

			this.WizardControls.Add(intro);
			this.WizardControls.Add(chooseSource);
			this.WizardControls.Add(chooseTables);
			this.WizardControls.Add(chooseDestination);
			this.WizardControls.Add(summary);

			// re-using the controls from OpenFileWizardControls
			this.HeaderPane.Controls.Add(new crudwork.Controls.Wizard.OpenFileWizardControls.HeaderPane());
			this.LHSPane.Controls.Add(new crudwork.Controls.Wizard.OpenFileWizardControls.LHSPane());
		}

		#region Properties
		/// <summary>
		/// Get or set the Source database connection
		/// </summary>
		public DataConnectionInfo Source
		{
			get
			{
				return chooseSource.DatabaseConnection;
			}
			set
			{
				chooseSource.DatabaseConnection = value;
			}
		}
		/// <summary>
		/// Get or set the Destination database connection
		/// </summary>
		public DataConnectionInfo Destination
		{
			get
			{
				return chooseDestination.DatabaseConnection;
			}
			set
			{
				chooseDestination.DatabaseConnection = value;
			}
		}
		/// <summary>
		/// Get the selected tables for Import/Export procedure
		/// </summary>
		public string[] SelectedTables
		{
			get
			{
				var arr = chooseTables.SelectedOptions;
				Array.Sort(arr);
				return arr;
				//return chooseTables.SelectedOptions;
			}
		}
		/// <summary>
		/// Get the list of non-selected tables
		/// </summary>
		public string[] AvailableTables
		{
			get
			{
				return chooseTables.AvailableOptions;
			}
		}
		/// <summary>
		/// Get or set the list of tables
		/// </summary>
		public Dictionary<string, bool> AllTables
		{
			get
			{
				return chooseTables.Options;
			}
			set
			{
				chooseTables.Options = value;
			}
		}
		#endregion
	}
}
