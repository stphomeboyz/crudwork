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
using System.Text;

namespace crudwork.Controls.Wizard
{
	/// <summary>
	/// delegate for the Wizard Form
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WizardEventHandler(object sender, WizardEventArgs e);

	/// <summary>
	/// Event argument for the WizardEventHandler
	/// </summary>
	public class WizardEventArgs : EventArgs
	{
		/// <summary>
		/// Get the current step
		/// </summary>
		public int SelectedStep
		{
			get;
			private set;
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="selectedStep"></param>
		public WizardEventArgs(int selectedStep)
		{
			this.SelectedStep = selectedStep;
		}
	}
}
