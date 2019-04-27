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
	/// A simple form to allow the user to select (or unselect) from a list of item.
	/// </summary>
	public partial class SelectListBox : Form
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public SelectListBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get or set the data source
		/// </summary>
		public object DataSource
		{
			get
			{
				return listBox1.DataSource;
			}
			set
			{
				listBox1.DataSource = value;
			}
		}

		/// <summary>
		/// Get or set the display member
		/// </summary>
		public string DisplayMember
		{
			get
			{
				return listBox1.DisplayMember;
			}
			set
			{
				listBox1.DisplayMember = value;
			}
		}

		/// <summary>
		/// Get or set the value member
		/// </summary>
		public string ValueMember
		{
			get
			{
				return listBox1.ValueMember;
			}
			set
			{
				listBox1.ValueMember = value;
			}
		}

		/// <summary>
		/// get or set the list of items
		/// </summary>
		public object[] Items
		{
			get
			{
				List<object> results = new List<object>();
				for (int i = 0; i < listBox1.Items.Count; i++)
				{
					results.Add(listBox1.Items[i]);
				}
				return results.ToArray();
			}
			set
			{
				listBox1.Items.AddRange(value);
			}
		}

		/// <summary>
		/// get or set the selected index
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				return listBox1.SelectedIndex;
			}
			set
			{
				listBox1.SelectedIndex = value;
			}
		}

		/// <summary>
		/// clear all selection
		/// </summary>
		public void Clear()
		{
			listBox1.Items.Clear();
		}

		private int IndexOf(string value)
		{
			value = value.ToUpper();
			for (int i = 0; i < listBox1.Items.Count; i++)
			{
				if (value.Equals(listBox1.Items[i].ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// get or set the select item
		/// </summary>
		public object SelectedItem
		{
			get
			{
				return listBox1.SelectedItem;
			}
			set
			{
				int index = IndexOf(value.ToString());

				if (index != -1)
					listBox1.SelectedIndex = index;
			}
		}
	}
}