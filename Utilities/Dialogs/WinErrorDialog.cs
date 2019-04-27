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
using System.Text;
using System.Windows.Forms;

namespace crudwork.Utilities.Dialogs
{
	internal partial class WinErrorDialog : Form
	{
        #region Enumerators
        #endregion

        #region Fields
        #endregion

        #region Constructors
		public WinErrorDialog()
		{
			InitializeComponent();
		}
		#endregion

        #region Event Methods

		#region System Event Methods
		private void WinErrorDialog_Load(object sender, EventArgs e)
		{
			chkWrapText.Checked = true;
			chkWrapText_Click(sender, e);
			btnOkay.Focus();
		}
		#endregion

		#region Application Event Methods
		private void btnCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(txtDetails.Text, TextDataFormat.Text);
			//MessageBox.Show("Exception details is copy to the clipboard");
		}
		
		private void chkWrapText_Click(object sender, EventArgs e)
		{
			txtDetails.WordWrap = chkWrapText.Checked;
		}
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		#endregion

		#region Private Methods
		#endregion

        #region Protected Methods
        #endregion

        #region Properties
		/// <summary>
		/// Get or set the window title
		/// </summary>
		public string WindowTitle
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text = value;
			}
		}

		/// <summary>
		/// Get or set the brief message
		/// </summary>
		public string Message
		{
			get
			{
				return this.lblMessage.Text;
			}
			set
			{
				this.lblMessage.Text = value;
			}
		}

		/// <summary>
		/// Get or set the full details
		/// </summary>
		public string Details
		{
			get
			{
				return this.txtDetails.Text;
			}
			set
			{
				this.txtDetails.Text = value;
			}
		}

		#endregion

		#region Others
		#endregion
	}
}