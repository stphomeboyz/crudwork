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
using crudwork.Utilities;

namespace crudwork.Controls
{
	/// <summary>
	/// A form allowing the user to specify the properties for the data source
	/// </summary>
	public partial class DynamicEditorForm : Form
	{
		/// <summary>
		/// create a new instance with default attributes
		/// </summary>
		public DynamicEditorForm()
		{
			InitializeComponent();
		}

		#region Application Events
		private void DynamicEditorForm_Load(object sender, EventArgs e)
		{

		}

		private void btnOkay_Click(object sender, EventArgs e)
		{
			try
			{
				// validate here...

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		/// <summary>
		/// Get or set the data source to display on the grid
		/// </summary>
		public object DataSource
		{
			get
			{
				return dataGridView1.DataSource;
			}
			set
			{
				dataGridView1.DataSource = value;
			}
		}

		/// <summary>
		/// Get or set the title caption
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
