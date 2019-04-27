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

namespace crudwork.Controls
{
	/// <summary>
	/// Selectable List Box
	/// </summary>
	public partial class SelectableListBox : Form
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public SelectableListBox()
		{
			InitializeComponent();
		}


		private void SelectableListBox_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Get or set the list of options
		/// </summary>
		public Dictionary<string, bool> Options
		{
			get
			{
				return chooseListBox1.Options;
			}
			set
			{
				chooseListBox1.Options = value;
			}
		}

		/// <summary>
		/// Get or set the window title
		/// </summary>
		public string Caption
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
