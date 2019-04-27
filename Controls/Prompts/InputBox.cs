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
	/// The input box
	/// </summary>
	public partial class InputBox : Form
	{
		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public InputBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get or set the message
		/// </summary>
		public string Message
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text = value;
			}
		}

		/// <summary>
		/// Get or set the title of the dialog box
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
		
		/// <summary>
		/// Get or set the input value
		/// </summary>
		public string Input
		{
			get
			{
				return txtInput.Text;
			}
			set
			{
				txtInput.Text = value;
			}
		}
	}
}