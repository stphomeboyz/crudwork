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

namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	/// <summary>
	/// Wizard Step 3: (Final Step) Gather all data for output
	/// </summary>
	internal partial class Summary : UserControl, IWizardControl
	{
		#region Properties
		public ChooseSource Step1
		{
			get;
			private set;
		}
		public ChooseTables Step2
		{
			get;
			private set;
		}
		public ChooseDestination Step3
		{
			get;
			private set;
		}
		#endregion

		public Summary(ChooseSource step1, ChooseTables step2, ChooseDestination step3)
		{
			InitializeComponent();

			this.Step1 = step1;
			this.Step2 = step2;
			this.Step3 = step3;
		}

		private void WizardSummary_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "The wizard has gathered all the information needed to process the input data source.  Below is the summary of information collected by the wizard.  Click BACK to make any change(s); or, click FINISH to continue.";
		}

		#region IWizardControl Members
		void IWizardControl.ValidateContent()
		{
			if (Step1 as IWizardControl == null)
				throw new ApplicationException("control must implement the IWizardControl interface: step1");
			if (Step2 as IWizardControl == null)
				throw new ApplicationException("control must implement the IWizardControl interface: step2");
			if (Step3 as IWizardControl == null)
				throw new ApplicationException("control must implement the IWizardControl interface: step3");

			((IWizardControl)Step1).ValidateContent();
			((IWizardControl)Step2).ValidateContent();
			((IWizardControl)Step3).ValidateContent();
		}

		void IWizardControl.RefreshContent()
		{
			txtSummary.Text = "";
			txtSummary.Text += Step1.ToString() + Environment.NewLine;
			txtSummary.Text += Step3.ToString() + Environment.NewLine;
			txtSummary.Text += Step2.ToString() + Environment.NewLine;
		}
		#endregion
	}
}
