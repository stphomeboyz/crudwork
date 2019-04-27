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
	internal partial class SimpleMetaDataViewer : UserControl
	{
		public event CellChangedEventHandler CellChanged = null;
		private DataSet ds = null;
		private DataTable metaDataDataTable = null;
		private int currentRow = -1;
		private int currentColumn = -1;
		private CurrencyManager currencyManager;

		public int CurrentRow
		{
			get
			{
				return currentRow;
			}
			set
			{
				currentRow = value;
			}
		}

		public int CurrentColumn
		{
			get
			{
				return currentColumn;
			}
			set
			{
				currentColumn = value;
			}
		}

		public SimpleMetaDataViewer()
		{
			InitializeComponent();
		}

		public DataSet DataSource
		{
			get
			{
				return this.ds;
			}
			set
			{
				this.ds = value;
				metaDataDataTable = DataUtil.GetMetadata(ds);
				currencyManager = (CurrencyManager)this.BindingContext[metaDataDataTable];
				metaDataDataTable.DefaultView.Sort = "table_name,column_name";
				dataGridView1.DataSource = metaDataDataTable.DefaultView;
			}
		}

		public int Count
		{
			get
			{
				if (metaDataDataTable == null)
					return -1;
				else
					return metaDataDataTable.Rows.Count;
			}
		}

		private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			currentRow = e.RowIndex;
			currentColumn = e.ColumnIndex;

			if (CellChanged == null)
				return;

			DataRow dr = metaDataDataTable.DefaultView[e.RowIndex].Row;

			CellChangedEventArgs e2 = new CellChangedEventArgs(
				dr["table_name"].ToString(),
				dr["column_name"].ToString(),
				-1, CurrentRow, CurrentColumn
				);
			CellChanged(sender, e2);
		}

		public void SetRow(TableColumn tc)
		{
			try
			{
				if (ds == null || currencyManager == null)
					return;
				if (string.IsNullOrEmpty(tc.TableName))
					throw new ArgumentNullException("TableName");
				if (string.IsNullOrEmpty(tc.ColumnName))
					throw new ArgumentNullException("ColumnName");

				DataRow dr = null;
				int idx = -1;

				using (DataView dv = new DataView(metaDataDataTable.DefaultView.Table, metaDataDataTable.DefaultView.RowFilter,
					metaDataDataTable.DefaultView.Sort, DataViewRowState.CurrentRows))
				{
					dv.RowFilter = string.Format("table_name='{0}' and column_name='{1}'",
						tc.TableName, tc.ColumnName);

					if (dv.Count == 0)
						throw new ArgumentOutOfRangeException("no rows found for: " + tc.TableName + "." + tc.ColumnName);

					dr = dv[0].Row;
				}

				for (int i = 0; i < metaDataDataTable.Rows.Count; i++)
				{
					if (dr == metaDataDataTable.Rows[i])
					{
						idx = i;
						break;
					}
				}

				Debug.WriteLine("Pos=" + currencyManager.Position + " idx=" + idx);
				currencyManager.Position = idx;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		public void SetBackColorByRows(Color backColor, Color selectionBackColor, params TableColumn[] tcList)
		{
			string[] columns = new string[tcList.Length];

			for (int i = 0; i < tcList.Length; i++)
			{
				columns[i] = tcList[i].FullName.ToUpper();
			}

			Array.Sort(columns);

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				TableColumn tc = new TableColumn(row.Cells["table_name"].Value.ToString().ToUpper(),
					row.Cells["column_name"].Value.ToString().ToUpper());
				bool found = Array.BinarySearch(columns, tc.FullName) >= 0;
				//bool found = TableColumn.Contains(columns, tc);

				row.DefaultCellStyle.BackColor = (found) ? backColor : Color.White;
				row.DefaultCellStyle.SelectionBackColor = (found) ? selectionBackColor : Color.Black;
				row.Tag = found;
			}
		}
	}
}
