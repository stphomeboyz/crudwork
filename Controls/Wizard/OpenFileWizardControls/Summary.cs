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

using crudwork.FileImporters;
using crudwork.Controls.Wizard;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	/// <summary>
	/// Wizard Step 3: (Final Step) Gather all data for output
	/// </summary>
	internal partial class OpenFileWizardSummary : UserControl, IWizardControl
	{
		public ChooseInputSource Step1
		{
			get;
			private set;
		}

		public MapDataField Step2
		{
			get;
			private set;
		}

		public OpenFileWizardSummary(ChooseInputSource step1, MapDataField step2)
		{
			InitializeComponent();

			this.Step1 = step1;
			this.Step2 = step2;
		}

		private void WizardSummary_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "The wizard has gathered all the information needed to process the input data source.  Below is the summary of information collected by the wizard.  Click BACK to make any change(s); or, click FINISH to continue.";
		}

		private string GetField(object theObject)
		{
			if (theObject == null || theObject == DBNull.Value)
				return string.Empty;

			string value = theObject.ToString();

			if (!value.Contains("."))
				return value;

			string[] tokens = value.Split('.');
			return tokens[tokens.Length - 1];
		}

		private void CreateSummary()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Data Source:");
			sb.AppendLine("  InputDataSource      = " + Step1.InputDataSource);

			switch (Step1.InputDataSource)
			{
				case InputDataSourceType.File:
					sb.AppendLine("  Filename             = " + Step1.Filename);

					if (Step1.InputDataSource == InputDataSourceType.File && Step1.FileSupportsTables(Step1.Filename))
						sb.AppendLine("  Tablename            = " + Step1.Tablename);
					break;

				case InputDataSourceType.Database:
					sb.AppendLine("  Provider             = " + Step1.Provider);
					sb.AppendLine("  ConnectionString     = " + Step1.ConnectionString);
					sb.AppendLine("  Tablename            = " + Step1.Tablename);
					break;

				default:
					break;
			}

			sb.AppendLine();

			var relation = Step2.Editor.RelationshipTable;
			sb.AppendLine("Data Column Mapping Definition:");
			foreach (DataRow dr in relation.Rows)
			{
				sb.Append("  " + GetField(dr["Field"]).PadRight(20) + " = ");

				if (dr["Column"] == DBNull.Value || string.IsNullOrEmpty(dr["Column"].ToString()))
				{
					sb.AppendLine("N/A");
				}
				else
				{
					sb.AppendLine(dr["Column"].ToString());
				}
			}

			txtSummary.Text = sb.ToString();
		}

		#region IWizardControl Members
		void IWizardControl.ValidateContent()
		{
			if (Step1 as IWizardControl == null)
				throw new ApplicationException("control must implement the IWizardControl interface: step1");
			if (Step2 as IWizardControl == null)
				throw new ApplicationException("control must implement the IWizardControl interface: step2");

			((IWizardControl)Step1).ValidateContent();
			((IWizardControl)Step2).ValidateContent();
		}

		void IWizardControl.RefreshContent()
		{
			CreateSummary();
		}
		#endregion
	}
}
