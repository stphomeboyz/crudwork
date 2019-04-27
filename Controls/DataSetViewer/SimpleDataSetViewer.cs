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
using System.Text;
using System.Windows.Forms;
using crudwork.Utilities;
using crudwork.DataAccess;
using System.Diagnostics;

namespace crudwork.Controls
{
	/// <summary>
	/// Simple DataSet viewer
	/// </summary>
	internal partial class SimpleDataSetViewer : UserControl
	{
		public event CellChangedEventHandler CellChanged = null;
		public event TablenameChangedEventHandler TablenameChanged = null;

		private DataSet ds = null;
		private int currentRow = -1;
		private int currentColumn = -1;
		private DataGridViewSelectionMode selectedMode = DataGridViewSelectionMode.RowHeaderSelect;

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public SimpleDataSetViewer()
		{
			InitializeComponent();
		}

		#region Events
		private void SimpleDataSetViewer_Load(object sender, EventArgs e)
		{
			lblStatus.Text = "";
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TableName = comboBox1.SelectedItem.ToString();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TableName = listBox1.SelectedItem.ToString();
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			showTableListToolStripMenuItem.Checked = !splitContainer1.Panel1Collapsed;
		}

		private void showTableListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HideTablePanel = !HideTablePanel;
		}

		private void lblTable_Click(object sender, EventArgs e)
		{
			HideTablePanel = !HideTablePanel;
		}

		private void UpdateStatus(int rowIndex, int columnIndex)
		{
			lblStatus.Text = string.Format("Row: {0} of {1} Col: {2} of {3}",
				1 + rowIndex, ds.Tables[dataGridView1.DataMember].Rows.Count, //dataGridView1.Rows.Count,
				1 + columnIndex, ds.Tables[dataGridView1.DataMember].Columns.Count //dataGridView1.Columns.Count
				);
		}

		private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				currentRow = e.RowIndex;
				currentColumn = e.ColumnIndex;

				UpdateStatus(e.RowIndex, e.ColumnIndex);

				if (CellChanged != null)
				{
					CellChangedEventArgs e2 = new CellChangedEventArgs(
						TableName, ColumnName, this.SelectedTableIndex,
						this.SelectedRowIndex, this.SelectedColumnIndex);
					CellChanged(sender, e2);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void pivotTableEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				ControlManager.ShowPivotTableEditor(ds.Tables[TableName]);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void viewTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				var s = SelectedCell.Value.ToString();
				ControlManager.ShowTextDialog(ref s, "View Text");
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		#region Private methods
		private void PopulateTables()
		{
			if (ds == null)
				return;

			comboBox1.Items.Clear();
			listBox1.Items.Clear();

			for (int i = 0; i < ds.Tables.Count; i++)
			{
				comboBox1.Items.Add(ds.Tables[i].TableName);
				listBox1.Items.Add(ds.Tables[i].TableName);
			}

			comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;
			listBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;

			HideTablePanel = ds.Tables.Count == 1;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Initialize the DataSet
		/// </summary>
		public void Initialize()
		{
			if (ds == null || ds.Tables.Count == 0 || !string.IsNullOrEmpty(TableName))
				return;

			TableName = ds.Tables[0].TableName;

			UpdateStatus(0, 0);
		}

		/// <summary>
		/// Set the background color of the columns
		/// </summary>
		/// <param name="backColor"></param>
		/// <param name="selectionBackColor"></param>
		/// <param name="tcList"></param>
		public void SetBackColorByColumns(Color backColor, Color selectionBackColor, params TableColumn[] tcList)
		{
			if (tcList == null)
				return;

			// get only the columns for processing
			//string[] columns = new string[tcList.Length];
			List<string> columns = new List<string>();

			for (int i = 0; i < tcList.Length; i++)
			{
				TableColumn tc = tcList[i];

				if (!TableName.Equals(tc.TableName, StringComparison.InvariantCultureIgnoreCase))
					continue;

				columns.Add(tcList[i].ColumnName.ToUpper());
			}

			columns.Sort();
			string[] colArray = columns.ToArray();
			//Array.Sort(columns);

			foreach (DataGridViewColumn col in dataGridView1.Columns)
			{
				string headerText = col.HeaderText.ToUpper();
				bool found = Array.BinarySearch(colArray, headerText) >= 0;
				col.DefaultCellStyle.BackColor = (found) ? backColor : Color.White;
				col.DefaultCellStyle.SelectionBackColor = (found) ? selectionBackColor : Color.Black;
				col.Tag = found;
			}
		}
		#endregion

		#region Public events
		/// <summary>
		/// The DataMemberChanged event
		/// </summary>
		public event EventHandler DataMemberChanged
		{
			add
			{
				dataGridView1.DataMemberChanged += value;
			}
			remove
			{
				dataGridView1.DataMemberChanged -= value;
			}
		}

		/// <summary>
		/// The Cell double-click event
		/// </summary>
		public event EventHandler CellDoubleClick
		{
			add
			{
				dataGridView1.DoubleClick += value;
			}
			remove
			{
				dataGridView1.DoubleClick -= value;
			}
		}
		#endregion

		#region Public properties
		/// <summary>
		/// Get or set the data source
		/// </summary>
		public DataSet DataSource
		{
			get
			{
				return ds;
			}
			set
			{
				ds = value;

				if (dataGridView1 != null)
				{
					// set to RowHeaderSelect to avoid this exception: Column's SortMode cannot be set to Automatic while the DataGridView control's SelectionMode is set to FullColumnSelect.
					dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
					//dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

					//foreach (DataGridViewColumn c in dataGridView1.Columns)
					//{
					//    c.SortMode = DataGridViewColumnSortMode.NotSortable;
					//    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
					//}
				}

				dataGridView1.DataSource = ds;
				PopulateTables();
				Initialize();
			}
		}

		/// <summary>
		/// Get or set the table name
		/// </summary>
		public string TableName
		{
			get
			{
				return dataGridView1.DataMember;
			}
			set
			{
				string previousMember = dataGridView1.DataMember;

				#region Change DataMember (with support for SelectionMode)
				{
					#region Change DataMember with rollback
					DataGridViewSelectionMode previousMode = dataGridView1.SelectionMode;

					try
					{
						dataGridView1.DataMember = value;
					}
					catch (Exception ex)
					{
						// BUGGY: DataMember does not restore previous value of DataMember...  we must do it here.
						try
						{
							dataGridView1.DataMember = previousMember;
						}
						catch //(Exception ex2)
						{
							// ignore exception.
						}

						if (ex.Message != @"Column's SortMode cannot be set to Automatic while the DataGridView control's SelectionMode is set to FullColumnSelect.")
							throw;

						try
						{
							// try changing the SelectionMode to default, first
							dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
							dataGridView1.DataMember = value;
						}
						catch (Exception ex2)
						{
							try
							{
								// restore previous value of SelectionMode.
								dataGridView1.SelectionMode = previousMode;
							}
							catch
							{
								// ignore exception.
							}
							throw ex2;
						}
					}
					#endregion

					if (SelectionMode != DataGridViewSelectionMode.RowHeaderSelect)
					{
						for (int i = 0; i < dataGridView1.Columns.Count; i++)
						{
							DataGridViewColumn c = dataGridView1.Columns[i];
							c.SortMode = DataGridViewColumnSortMode.NotSortable;
						};
					}
					dataGridView1.SelectionMode = SelectionMode;
				}
				#endregion

				#region Change ComboBox
				if (!string.IsNullOrEmpty(value) && comboBox1.SelectedItem != null && !value.Equals(comboBox1.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					string v2 = value.ToUpper();
					for (int i = 0; i < comboBox1.Items.Count; i++)
					{
						string v = comboBox1.Items[i].ToString().ToUpper();

						if (v == v2)
						{
							comboBox1.SelectedIndex = i;
							break;
						}
					}
				}
				#endregion

				#region Change ListBox
				if (!string.IsNullOrEmpty(value) && listBox1.SelectedItem != null && !value.Equals(listBox1.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					string v2 = value.ToUpper();
					for (int i = 0; i < listBox1.Items.Count; i++)
					{
						string v = listBox1.Items[i].ToString().ToUpper();

						if (v == v2)
						{
							listBox1.SelectedIndex = i;
							break;
						}
					}
				}
				#endregion

				// 04/14/2010: doing this will stretch the column on columns with long data content...
				//dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

				if (previousMember != dataGridView1.DataMember)
				{
					if (TablenameChanged != null)
						TablenameChanged(this, new TablenameChangedEventArgs(dataGridView1.DataMember));
				}

				// reorder the table's columns.  Make sure it does not follow the order of the previously selected table.
				for (int i = 0; i < dataGridView1.Columns.Count; i++)
				{
					dataGridView1.Columns[i].DisplayIndex = i;
				}
			}
		}

		/// <summary>
		/// Set the column's auto resize mode
		/// </summary>
		/// <param name="mode"></param>
		public void AutoResizeColumns(DataGridViewAutoSizeColumnsMode mode)
		{
			dataGridView1.AutoResizeColumns(mode);
		}

		/// <summary>
		/// Get or set the column name
		/// </summary>
		public string ColumnName
		{
			get
			{
				#region Sanity Checks
				if (dataGridView1 == null)
					return string.Empty;
				#endregion

				//string s = dataGridView1.Columns[currentColumn].HeaderText;

				for (int i = 0; i < dataGridView1.Columns.Count; i++)
				{
					DataGridViewColumn c = dataGridView1.Columns[i];
					if (c.Selected)
						return c.HeaderText;
				}

				if (currentColumn == -1)
					return string.Empty;
				else
					return dataGridView1.Columns[currentColumn].HeaderText;
			}
			set
			{
				for (int i = 0; i < dataGridView1.Columns.Count; i++)
				{
					DataGridViewColumn c = dataGridView1.Columns[i];
					c.Selected = false;
				}

				if (string.IsNullOrEmpty(value))
					return;
				for (int i = 0; i < dataGridView1.Columns.Count; i++)
				{
					DataGridViewColumn c = dataGridView1.Columns[i];
					c.Selected = value.Equals(c.HeaderText, StringComparison.InvariantCultureIgnoreCase);
					if (c.Selected)
						currentColumn = i;
				}
			}
		}

		/// <summary>
		/// Get the count
		/// </summary>
		public int Count
		{
			get
			{
				if (ds == null)
					return -1;
				else
					return ds.Tables.Count;
			}
		}

		/// <summary>
		/// Get the selected table index
		/// </summary>
		public int SelectedTableIndex
		{
			get
			{
				return comboBox1.SelectedIndex;
			}
		}

		/// <summary>
		/// Get the selected row index
		/// </summary>
		public int SelectedRowIndex
		{
			get
			{
				return currentRow;
			}
		}

		/// <summary>
		/// Get the selected column index
		/// </summary>
		public int SelectedColumnIndex
		{
			get
			{
				return currentColumn;
			}
		}

		/// <summary>
		/// Get the datagrid
		/// </summary>
		public DataGridView DataGridView
		{
			get
			{
				return dataGridView1;
			}
		}

		/// <summary>
		/// Get or set the selection mode
		/// </summary>
		public DataGridViewSelectionMode SelectionMode
		{
			get
			{
				return selectedMode;
			}
			set
			{
				selectedMode = value;
			}
		}

		/// <summary>
		/// Show or hide the table panel
		/// </summary>
		public bool HideTablePanel
		{
			get
			{
				return splitContainer1.Panel1Collapsed;
			}
			set
			{
				splitContainer1.Panel1Collapsed = value;
			}
		}

		/// <summary>
		/// Get the selected cell
		/// </summary>
		public DataGridViewCell SelectedCell
		{
			get
			{
				return dataGridView1.Rows[SelectedRowIndex].Cells[SelectedColumnIndex];
			}
			//set
			//{
			//}
		}

		/// <summary>
		/// get a list of internal controls used for making table selection
		/// </summary>
		/// <remarks>
		/// this internal property is used to enable/disable the controls while a process is busy running
		/// </remarks>
		internal Control[] SelectionControls
		{
			get
			{
				return new Control[] { comboBox1, listBox1 };
			}
		}
		#endregion
	}

	#region Delegate
	/// <summary>
	/// The CellChanged event handler
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void CellChangedEventHandler(object sender, CellChangedEventArgs e);

	/// <summary>
	/// The CellChanged event args
	/// </summary>
	public class CellChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Get the table name
		/// </summary>
		public readonly string TableName;

		/// <summary>
		/// Get the column name
		/// </summary>
		public readonly string ColumnName;

		/// <summary>
		/// Get the table index
		/// </summary>
		public readonly int TableIndex;

		/// <summary>
		/// Get the row index
		/// </summary>
		public readonly int RowIndex;

		/// <summary>
		/// Get the column index
		/// </summary>
		public readonly int ColumnIndex;
		//public readonly string DataType;
		//public readonly bool IsNullable;

		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public CellChangedEventArgs()
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		/// <param name="tableIndex"></param>
		/// <param name="rowIndex"></param>
		/// <param name="columnIndex"></param>
		public CellChangedEventArgs(string tableName, string columnName,
			int tableIndex, int rowIndex, int columnIndex)
		{
			this.TableName = tableName;
			this.ColumnName = columnName;
			this.TableIndex = tableIndex;
			this.RowIndex = rowIndex;
			this.ColumnIndex = columnIndex;
		}
	}

	/// <summary>
	/// The TablenameChanged event handler
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void TablenameChangedEventHandler(object sender, TablenameChangedEventArgs e);

	/// <summary>
	/// the TablenameChanged event args
	/// </summary>
	public class TablenameChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Get the table name
		/// </summary>
		public readonly string TableName;

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="tablename"></param>
		public TablenameChangedEventArgs(string tablename)
		{
			this.TableName = tablename;
		}
	}
	#endregion

}
