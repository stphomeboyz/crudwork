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
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Controls.Wizard;
using crudwork.DataAccess;
using System.Diagnostics;

using crudwork.Controls.Wizard.OpenFileWizardControls;


namespace crudwork.Controls.Wizard
{
	/// <summary>
	/// User friendly wizard to help user open a data file, and map the fields required for process
	/// </summary>
	public partial class OpenFileWizard : Wizard
	{
		private OpenFileResult _result = null;

		/// <summary>
		/// Subscribe to this event to validate the data column mapper event
		/// </summary>
		public event ValidateDataColumnMapperEventHandler ValidateDataColumnMapper = null;

		#region Wizard Controls
		private Wizard wizard = null;
		private HeaderPane wizardHeaderPane = null;
		private LHSPane wizardLHSPane = null;
		private OpenFileWizardIntroduction intro = null;
		private ChooseInputSource step1 = null;
		private MapDataField step2 = null;
		private OpenFileWizardSummary summary = null;
		#endregion

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public OpenFileWizard()
		{
			InitializeComponent();
			InitializeWizard();
		}

		private void MyWizard_Load(object sender, EventArgs e)
		{

		}

		private void InitializeWizard()
		{
			// initialize custom controls
			wizardHeaderPane = new HeaderPane();
			wizardLHSPane = new LHSPane();

			wizardHeaderPane.Dock = DockStyle.Fill;
			wizardLHSPane.Dock = DockStyle.Fill;

			intro = new OpenFileWizardIntroduction();
			step1 = new ChooseInputSource();
			step2 = new MapDataField(step1);
			summary = new OpenFileWizardSummary(step1, step2);

			// subscribe event handlers
			step2.ValidateDataColumnMapper += new ValidateDataColumnMapperEventHandler(step2_OnValidateDataColumnMapper);

			// setup the wizard
			wizard = this;
			wizard.Size = new Size(605, 505);
			wizard.MinimumSize = new Size(605, 505);
			wizard.Title = "Open File Wizard";

			wizard.WizardControls.Add(intro);
			wizard.WizardControls.Add(step1);
			wizard.WizardControls.Add(step2);
			wizard.WizardControls.Add(summary);

			wizard.HeaderPane.Controls.Add(wizardHeaderPane);
			wizard.LHSPane.Controls.Add(wizardLHSPane);

			wizard.ShowHeader = true;
			wizard.ShowLHS = true;

			wizard.EnableHelp = true;
			wizard.OnHelpClick += new WizardEventHandler(wizard_OnHelpClick);

			wizard.EnableCustom = false;
			wizard.CustomButtonText = "View Spec";
			wizard.OnCustomClick += new WizardEventHandler(wizard_OnCustomClick);
		}

		void step2_OnValidateDataColumnMapper(object sender, ValidateDataColumnMapperEventArgs e)
		{
			var t = ValidateDataColumnMapper;
			if (t != null)
			{
				// NOTE: Do not absorb the exception thrown by the subscriber!
				// If validation failed, allow the error to bubble up to the wizard
				t(sender, e);
			}
		}

		void wizard_OnHelpClick(object sender, WizardEventArgs e)
		{
			MessageBox.Show("Help Topic for Step #" + e.SelectedStep);
		}

		void wizard_OnCustomClick(object sender, WizardEventArgs e)
		{
			MessageBox.Show("Custom for Step #" + e.SelectedStep);
		}

		/// <summary>
		/// Get or set the OpenFileResult
		/// </summary>
		public OpenFileResult Result
		{
			get
			{
				var r = _result = _result ?? new OpenFileResult();

				r.InputDataSource = step1.InputDataSource;
				r.Filename = step1.Filename;
				r.Provider = step1.Provider;
				r.ConnectionString = step1.ConnectionString;
				r.Tablename = step1.Tablename;

				r.DataColumnMapping = step2.Editor.RelationshipTable;
				r.DestinationColumn = step2.Editor.ParentColumn;
				r.SourceColumn = step2.Editor.ChildColumn;

				return r;
			}
			set
			{
				var r = _result = value ?? new OpenFileResult();

				// prefill these entries...
				step1.InputDataSource = r.InputDataSource;
				step1.Filename = r.Filename;
				step1.Provider = r.Provider;
				step1.ConnectionString = r.ConnectionString;
				step1.Tablename = r.Tablename;

				step2.Editor.SetRelation(r.DataColumnMapping, r.DestinationColumn, r.SourceColumn);
			}
		}
	}
}
