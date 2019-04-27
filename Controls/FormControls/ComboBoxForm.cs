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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Utilities;
using crudwork.Controls.Base;

namespace crudwork.Controls.FormControls
{
	/// <summary>
	/// Form-Style ComboBox
	/// </summary>
	public partial class ComboBoxForm : Base.ChangeEventUserControl
	{
		/// <summary>
		/// Event for the ellipse button click
		/// </summary>
		public event EventHandler ButtonClicked = null;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ComboBoxForm()
		{
			InitializeComponent();
		}

		#region Application Events
		private void FileTextBox_Load(object sender, EventArgs e)
		{
		}
		private void button1_Click(object sender, EventArgs e)
		{
			if (ButtonClicked != null)
				ButtonClicked(sender, e);
		}
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaiseChangeEvent(this, new ChangeEventArgs(sender));
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the label
		/// </summary>
		[Description("Get or set the label"), Category("Custom"), DefaultValue("Label Name")]
		public string Label
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
		/// Get or set the value of the text box
		/// </summary>
		[Description("Get or set the value of the text box"), Category("Custom"), DefaultValue("")]
		public string Value
		{
			get
			{
				return ControlUtil.GetSelectedItem(comboBox1);
			}
			set
			{
				ControlUtil.SelectItem<string>(comboBox1, value);
			}
		}
		/// <summary>
		/// Get or set the data source of the combobox
		/// </summary>
		[Description("Get or set the data source of the combobox"), Category("Custom")]
		public object DataSource
		{
			get
			{
				return comboBox1.DataSource;
			}
			set
			{
				comboBox1.DataSource = value;
			}
		}
		/// <summary>
		/// Show or hide the ellipse button
		/// </summary>
		[Description("Show or hide the ellipse button"), Category("Custom"), DefaultValue(false)]
		public bool ShowEllipseButton
		{
			get
			{
				return button1.Visible;
			}
			set
			{
				button1.Visible = value;
			}
		}
		/// <summary>
		/// Get the combobox control
		/// </summary>
		[Description("Get the combobox control"), Category("Custom")]
		public ComboBox Control
		{
			get
			{
				return this.comboBox1;
			}
		}
		#endregion

	}
}
