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

namespace crudwork.Controls
{
	/// <summary>
	/// Display a dialog box with a textbox and Cancel/OK buttons.  The user can toggle the text wrapping using the Wrap checkbox.
	/// </summary>
	public partial class TextDialog : Form
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public TextDialog()
		{
			InitializeComponent();
		}

		private void TextDialog_Load(object sender, EventArgs e)
		{
		}

		private void checkBox1_Click(object sender, EventArgs e)
		{
			textBox1.WordWrap = !textBox1.WordWrap;
		}

		/// <summary>
		/// Get or set the message to display on the textbox
		/// </summary>
		public string Message
		{
			get
			{
				return textBox1.Text;
			}
			set
			{
				textBox1.Text = value;
			}
		}

		/// <summary>
		/// Get or set the form's title
		/// </summary>
		public string DialogTitle
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

	}
}