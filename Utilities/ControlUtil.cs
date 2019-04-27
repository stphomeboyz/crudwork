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
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace crudwork.Utilities
{
	/// <summary>
	/// Control Utility
	/// </summary>
	public static class ControlUtil
	{
		#region ListBox methods
		/// <summary>
		/// Move selected items from ListBox up or down specified by direction.
		/// </summary>
		/// <param name="listBox"></param>
		/// <param name="direction"></param>
		public static void MoveItems(ListBox listBox, int direction)
		{
			// save the items
			Dictionary<int, object> values = new Dictionary<int, object>();
			ListBox.SelectedIndexCollection indices = listBox.SelectedIndices;
			int startidx = -1;

			if (indices.Count == 0)
				return;

			for (int i = 0; i < indices.Count; i++)
			{
				int idx = indices[i];

				if (i == 0)
					startidx = idx;
				values.Add(idx, listBox.Items[idx]);
			}

			#region Sanity Checks
			if (direction == -1)		// MoveUp
			{
				if (indices[0] == 0)
					return;				// first entry is already at top!
			}
			else if (direction == 1)	// MoveDown
			{
				if (indices[indices.Count - 1] == listBox.Items.Count - 1)
					return;				// last entry is already at bottom!
			}
			else
			{
				throw new ArgumentOutOfRangeException("direction=" + direction);
			}
			#endregion

			RemoveItems(listBox, indices);
			InsertItems(listBox, values, startidx + direction);

			// re-select items.
			SelectItems(listBox, values, direction);
		}

		private static void RemoveItems(ListBox listBox, ListBox.SelectedIndexCollection indices)
		{
			// must order descendingly, for deletion.
			int[] orderIndices = new int[indices.Count];

			for (int i = 0; i < indices.Count; i++)
			{
				orderIndices[i] = indices[i];
			}

			Array.Sort(orderIndices);

			// remove them
			for (int i = orderIndices.Length - 1; i > -1; i--)
			{
				listBox.Items.RemoveAt(orderIndices[i]);
			}
		}

		private static void InsertItems(ListBox listBox, Dictionary<int, object> values, int offset)
		{
			int counter = -1;
			foreach (KeyValuePair<int, object> kv in values)
			{
				counter++;
				int idx = counter + offset;
				listBox.Items.Insert(idx, kv.Value);
			}
		}

		private static void SelectItems(ListBox listBox, Dictionary<int, object> values, int offset)
		{
			foreach (KeyValuePair<int, object> kv in values)
			{
				int idx = kv.Key + offset;
				listBox.SetSelected(idx, true);
			}
		}

		/// <summary>
		/// Convert ListBox to string array
		/// </summary>
		/// <param name="listBox"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		public static string[] ListBoxToArray(ListBox listBox, string prefix)
		{
			string[] results = new string[listBox.Items.Count];
			for (int i = 0; i < listBox.Items.Count; i++)
			{
				results[i] = prefix + listBox.Items[i].ToString();
			}
			return results;
		}
		#endregion

		#region ---- PopulateControl() methods for ComboBox ----
		/// <summary>
		/// Populate a ComboBox control with values in an Enum type, with first item.
		/// </summary>
		/// <param name="comboBox"></param>
		/// <param name="type"></param>
		/// <param name="firstItem"></param>
		public static void PopulateControl(ComboBox comboBox, Type type, string firstItem)
		{
			//comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox.BeginUpdate();
			comboBox.Items.Clear();
			if (!String.IsNullOrEmpty(firstItem))
				comboBox.Items.Add(firstItem);
			comboBox.Items.AddRange(EnumUtil.EnumValues(type));
			comboBox.EndUpdate();
			comboBox.SelectedIndex = 0;
		}

		/// <summary>
		/// Populate a ComboBox control with values in an Enum type.
		/// </summary>
		/// <param name="comboBox"></param>
		/// <param name="type"></param>
		public static void PopulateControl(ComboBox comboBox, Type type)
		{
			PopulateControl(comboBox, type, String.Empty);
		}

		/// <summary>
		/// Populate a ComboBox control with values in a DataTable.
		/// </summary>
		/// <param name="comboBox"></param>
		/// <param name="table"></param>
		/// <param name="column"></param>
		/// <param name="firstItem"></param>
		public static void PopulateControl(ComboBox comboBox, DataTable table, DataColumn column, string firstItem)
		{
			PopulateControl(comboBox, table, column.ColumnName, firstItem);
		}

		/// <summary>
		/// Populate a ComboBox control with values in a DataTable.
		/// </summary>
		/// <param name="comboBox"></param>
		/// <param name="table"></param>
		/// <param name="columnName"></param>
		/// <param name="firstItem"></param>
		public static void PopulateControl(ComboBox comboBox, DataTable table, string columnName, string firstItem)
		{
			////comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			//comboBox.DataSource = table;
			//comboBox.DisplayMember = columnName;

			comboBox.BeginUpdate();
			comboBox.Items.Clear();

			if (!String.IsNullOrEmpty(firstItem))
				comboBox.Items.Add(firstItem);

			for (int i = 0; i < table.Rows.Count; i++)
			{
				string value = table.Rows[i][columnName].ToString();
				comboBox.Items.Add(value);
			}
			comboBox.EndUpdate();
			comboBox.SelectedIndex = 0;
		}
		#endregion

		#region ---- PopulateControl() methods for ToolStripComboBox ----
		/// <summary>
		/// Populate a ToolStripComboBox control with values in an Enum type, with first item.
		/// </summary>
		/// <param name="toolStripComboBox"></param>
		/// <param name="type"></param>
		/// <param name="firstItem"></param>
		public static void PopulateControl(ToolStripComboBox toolStripComboBox, Type type, string firstItem)
		{
			toolStripComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			toolStripComboBox.BeginUpdate();
			toolStripComboBox.Items.Clear();
			if (!String.IsNullOrEmpty(firstItem))
				toolStripComboBox.Items.Add(firstItem);
			toolStripComboBox.Items.AddRange(EnumUtil.EnumValues(type));
			toolStripComboBox.EndUpdate();
			toolStripComboBox.SelectedIndex = 0;
		}

		/// <summary>
		/// Populate a ToolStripComboBox control with values in an Enum type.
		/// </summary>
		/// <param name="toolStripComboBox"></param>
		/// <param name="type"></param>
		public static void PopulateControl(ToolStripComboBox toolStripComboBox, Type type)
		{
			PopulateControl(toolStripComboBox, type, String.Empty);
		}
		#endregion

		#region ---- ListBox ----
		/// <summary>
		/// Remove selected items in a ListBox control.
		/// </summary>
		/// <param name="listBox"></param>
		public static void RemoveListBoxItems(ListBox listBox)
		{
			switch (listBox.SelectionMode)
			{
				case SelectionMode.MultiExtended:
					{
						// in order to remove items, we must remove items by order, descendly.

						ListBox.SelectedIndexCollection ic = listBox.SelectedIndices;

						int[] indices = new int[ic.Count];
						for (int i = 0; i < ic.Count; i++)
						{
							indices[i] = ic[i];
						}
						Array.Sort(indices);
						Array.Reverse(indices);

						listBox.BeginUpdate();
						for (int i = 0; i < indices.Length; i++)
						{
							int idx = indices[i];
							object val = listBox.Items[idx];
							listBox.Items.Remove(val);
						}
						listBox.EndUpdate();

						listBox.SelectedIndex = -1;
					}
					break;

				case SelectionMode.MultiSimple:
					throw new NotImplementedException("MultiSimple not yet implemented.");
					//break;

				case SelectionMode.One:
					listBox.Items.RemoveAt(listBox.SelectedIndex);
					break;

				default:
					throw new ArgumentOutOfRangeException(listBox.SelectionMode.ToString());
			}
		}
		#endregion

		/// <summary>
		/// Enable the control
		/// </summary>
		/// <param name="c"></param>
		/// <param name="value"></param>
		public static void EnableControl(Control c, bool value)
		{
			c.SuspendLayout();
			c.Enabled = value;
			c.Visible = value;
			c.ResumeLayout();
		}

		/// <summary>
		/// re-adjust the client size
		/// </summary>
		/// <param name="clientSize"></param>
		/// <returns></returns>
		public static Size AdjustedClientSize(Size clientSize)
		{
			clientSize.Width -= 5;
			clientSize.Height -= 5;
			return clientSize;
		}

		#region Select item from ComboBox
		/// <summary>
		/// select the item of the combobox, and return the item's index.  Otherwise return a -1 to indicate not found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="combo"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int SelectItem<T>(ComboBox combo, T value)
		{
			if (value == null)
				return -1;
			for (int i = 0; i < combo.Items.Count; i++)
			{
				if (value.Equals((T)combo.Items[i]))
				{
					combo.SelectedIndex = i;
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Get the selected item
		/// </summary>
		/// <param name="combo"></param>
		/// <returns></returns>
		public static string GetSelectedItem(ComboBox combo)
		{
			return (combo.SelectedItem ?? string.Empty).ToString();
		}
		#endregion
	}
}
