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
using crudwork.Utilities;
using System.Diagnostics;

namespace crudwork.Controls
{
	/// <summary>
	/// Choose list box
	/// </summary>
	public partial class ChooseListBox : UserControl
	{
		private List<string> optionNames = new List<string>();
		private List<bool> optionFlags = new List<bool>();

		/// <summary>
		/// The selected change event handler
		/// </summary>
		public event EventHandler SelectedChanged = null;

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public ChooseListBox()
		{
			InitializeComponent();
		}

		#region Application Events
		private void btnSelect_Click(object sender, EventArgs e)
		{
			SetOption(lstAvailable.SelectedItems, true);
			UpdateListBox();
			lstAvailable.Focus();
		}

		private void lstSelected_DoubleClick(object sender, EventArgs e)
		{
			SetOption(lstSelected.SelectedItem.ToString(), false);
			UpdateListBox();
		}

		private void btnDeselect_Click(object sender, EventArgs e)
		{
			SetOption(lstSelected.SelectedItems, false);
			UpdateListBox();
			lstSelected.Focus();
		}

		private void lstAvailable_DoubleClick(object sender, EventArgs e)
		{
			SetOption(lstAvailable.SelectedItem.ToString(), true);
			UpdateListBox();
		}

		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			try
			{
				ControlUtil.MoveItems(lstSelected, -1);
				RaiseSelectedChanged();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			try
			{
				ControlUtil.MoveItems(lstSelected, +1);
				RaiseSelectedChanged();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		/// <summary>
		/// Raise a selected changed event
		/// </summary>
		protected void RaiseSelectedChanged()
		{
			if (SelectedChanged == null)
				return;
			SelectedChanged(this, EventArgs.Empty);
		}

		private void SetOption(ListBox.SelectedObjectCollection list, bool active)
		{
			for (int i = 0; i < list.Count; i++)
			{
				SetOption(list[i].ToString(), active, false);
			}

			RaiseSelectedChanged();
		}

		private void SetOption(string value, bool active)
		{
			SetOption(value, active, true);
		}

		private void SetOption(string value, bool active, bool raiseEvent)
		{
			for (int i = 0; i < optionNames.Count; i++)
			{
				if (optionNames[i] != value)
					continue;

				optionFlags[i] = active;

				if (raiseEvent)
					RaiseSelectedChanged();

				break;
			}
		}

		private string[] GetOptions(ListBox listBox)
		{
			List<string> results = new List<string>();
			for (int i = 0; i < listBox.Items.Count; i++)
			{
				results.Add(listBox.Items[i].ToString());
			}
			return results.ToArray();
		}

		/// <summary>
		/// Refresh the list box
		/// </summary>
		public void UpdateListBox()
		{
			try
			{
				int idxAvailable = lstAvailable.SelectedIndex;
				int idxSelected = lstSelected.SelectedIndex;
				lstAvailable.Items.Clear();
				lstSelected.Items.Clear();

				if (Options == null || Options.Count == 0)
				{
					return;
				}

				foreach (KeyValuePair<string, bool> kv in Options)
				{
					if (kv.Value)
					{
						lstSelected.Items.Add(kv.Key);
					}
					else
					{
						lstAvailable.Items.Add(kv.Key);
					}
				}

				lstAvailable.SelectedIndex = Math.Min(lstAvailable.Items.Count - 1, idxAvailable);
				lstSelected.SelectedIndex = Math.Min(lstSelected.Items.Count - 1, idxSelected);

				RaiseSelectedChanged();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Get or set the options
		/// </summary>
		public Dictionary<string, bool> Options
		{
			get
			{
				Dictionary<string, bool> results = new Dictionary<string, bool>();

				for (int i = 0; i < optionNames.Count; i++)
				{
					results.Add(optionNames[i], optionFlags[i]);
				}
				return results;
			}
			set
			{
				optionFlags.Clear();
				optionNames.Clear();

				if (value != null)
				{
					foreach (KeyValuePair<string, bool> kv in value)
					{
						optionNames.Add(kv.Key);
						optionFlags.Add(kv.Value);
					}
				}

				UpdateListBox();
			}
		}

		/// <summary>
		/// Get the selected options
		/// </summary>
		public string[] SelectedOptions
		{
			get
			{
				return GetOptions(lstSelected);
			}
		}

		/// <summary>
		/// Get the non-selected options
		/// </summary>
		public string[] AvailableOptions
		{
			get
			{
				return GetOptions(lstAvailable);
			}
		}
	}
}