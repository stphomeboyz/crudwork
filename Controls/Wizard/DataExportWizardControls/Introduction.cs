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
using crudwork.Controls.Wizard;

namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	internal partial class Introduction : UserControl, IWizardControl
	{
		public Introduction()
		{
			InitializeComponent();
		}

		private void WizardStart_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "";
			lblIntroduction.Text = @"This wizard helps you choose the source and destination data sources, and the tables(s) for export.";
		}

		#region IWizardControl Members

		void IWizardControl.RefreshContent()
		{
			//throw new NotImplementedException();
		}

		void IWizardControl.ValidateContent()
		{
			//throw new NotImplementedException();
		}

		#endregion
	}
}
