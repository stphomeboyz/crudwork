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
using crudwork.DataAccess;
using System.Diagnostics;

namespace crudwork.Controls
{
	/// <summary>
	/// Analysis tools for usage on DataTable
	/// </summary>
	public partial class DataTableAnalysisTool : Form
	{
		private DataTable dataTable = null;
		private int selectedColumnIndex = -1;
		private int selectedRowIndex = -1;

		/// <summary>
		/// Create and initialize a form
		/// </summary>
		public DataTableAnalysisTool()
		{
			InitializeComponent();
		}

		#region Foundation methods
		/// <summary>
		/// Get or set the DataSource for input.
		/// </summary>
		public DataTable DataSource
		{
			get
			{
				return this.dataTable;
			}
			set
			{
				try
				{
					this.dataTable = value;
					dgInput.DataSource = value;

					for (int i = 0; i < dgInput.Columns.Count; i++)
					{
						DataGridViewColumn c = dgInput.Columns[i];
						c.SortMode = DataGridViewColumnSortMode.NotSortable;
					}

					dgInput.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
					dgInput.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					throw;
				}
			}
		}

		/// <summary>
		/// return the current column index
		/// </summary>
		public int SelectedColumnIndex
		{
			get
			{
				return this.selectedColumnIndex;
			}
			private set
			{
				this.selectedColumnIndex = value;
			}
		}

		/// <summary>
		/// return the current row index
		/// </summary>
		public int SelectedRowIndex
		{
			get
			{
				return this.selectedRowIndex;
			}
			private set
			{
				this.selectedRowIndex = value;
			}
		}

		/// <summary>
		/// Return a list of selected column names.
		/// </summary>
		public string[] SelectedColumNames
		{
			get
			{
				SortedDictionary<int, string> results = new SortedDictionary<int, string>();

				int max = dgInput.SelectedColumns.Count;
				for (int i = 0; i < max; i++)
				{
					results.Add(dgInput.SelectedColumns[i].Index, dgInput.SelectedColumns[i].HeaderText);
				}

				string[] values = new string[results.Count];
				int idx = 0;
				foreach (KeyValuePair<int,string> kv in results)
				{
					values[idx++] = kv.Value;
				}
				return values;
			}
		}

		private void SetResults(object results)
		{
			switch (results.GetType().FullName)
			{
				case "System.Data.DataTable":
					TextResults = null;
					GridResults = (DataTable)results;
					break;

				case "System.String":
				default:
					TextResults = results.ToString();
					GridResults = null;
					break;
			}
		}

		private string TextResults
		{
			get
			{
				return txtResults.Text;
			}
			set
			{
				txtResults.Text = value;
				if (!string.IsNullOrEmpty(value))
				{
					tabControl1.SelectedTab = tabTextResult;
				}
			}
		}

		private DataTable gridResults = null;
		private DataTable GridResults
		{
			get
			{
				return this.gridResults;
			}
			set
			{
				this.gridResults = value;
				dgResults.DataSource = value;
				//dgResults.DataMember = value.TableName;
				if (value != null)
				{
					//dgResults.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
					dgResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
					tabControl1.SelectedTab = tabGridResult;
				}
			}
		}

		private string ColumnName(int index)
		{
			if (index < 0 || index > dataTable.Columns.Count - 1)
				throw new ArgumentOutOfRangeException("index=" + index);
			return dataTable.Columns[index].ColumnName;
		}

		private int ColumnIndex(string columnName)
		{
			columnName = columnName.ToUpper();
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				DataColumn dc = dataTable.Columns[i];
				if (dc.ColumnName.ToUpper() == columnName)
					return i + 1;
			}
			return -1;
		}

		private void ResizeColumns()
		{
			dgInput.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			dgResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		}
		#endregion

		#region Application events
		private void DataTableAnalysisTool_Load(object sender, EventArgs e)
		{
			ResizeColumns();
		}

		private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			SelectedColumnIndex = e.ColumnIndex;
			SelectedRowIndex = e.RowIndex;
		}

		private void DataTableAnalysisTool_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Alt)
				return;

			switch (e.KeyValue)
			{
				case 49:
					tabControl1.SelectedTab = tabDataTable;
					break;
				case 50:
					tabControl1.SelectedTab = tabTextResult;
					break;
				case 51:
					tabControl1.SelectedTab = tabGridResult;
					break;
				default:
					break;
			}
		}

		private void btnAutoResizeColumns_Click(object sender, EventArgs e)
		{
			ResizeColumns();
		}
		#endregion

		#region Analysis methods
		private void btnGroupBy_Click(object sender, EventArgs e)
		{
			try
			{
				string[] columns = SelectedColumNames;
				DataTable dt = DataFactory.FilterTable(dataTable, string.Join(",", columns), string.Empty, true, columns);
				SetResults(dt);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

	}
}