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
	/// Data column mapper
	/// </summary>
	public partial class DataColumnMapper : UserControl
	{
		private CurrencyManager cmRelationship = null;

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public DataColumnMapper()
		{
			InitializeComponent();
		}

		#region Events
		private void DataColumnMapper_Load(object sender, EventArgs e)
		{
			dgRelationship.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		}

		#region Events for DataGrid controls
		private void dgRelationship_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (string.IsNullOrEmpty(ParentColumn) || string.IsNullOrEmpty(ChildColumn))
				return;

			DataRow dr = RelationshipTable.Rows[e.RowIndex];
			if (cmRelationship != null)
				cmRelationship.Position = e.RowIndex;
			ShowRelation(dr);
		}

		private void dgRelationship_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}
		#endregion

		#region Buttons and Controls
		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				DataRow dr = RelationshipTable.NewRow();
				MapRelation(dr);
				RelationshipTable.Rows.Add(dr);

				// select new row.
				cmRelationship.Position = RelationshipTable.Rows.Count;
				RefreshParentChildTables();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (!ControlManager.Confirm("Delete relation.  Are you sure?"))
					return;
				int idx = cmRelationship.Position;
				if (idx == -1)
					return;

				DataRow dr = RelationshipTable.Rows[idx];

				if (idx == cmRelationship.Count - 1)
					idx--;

				dr.Delete();
				RefreshParentChildTables();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnSet_Click(object sender, EventArgs e)
		{
			try
			{
				int idx = cmRelationship.Position;
				if (idx == -1)
					return;

				MapRelation(RelationshipTable.Rows[idx]);

				// advance to next field, if avaiable.
				if (idx + 1 < cmRelationship.Count)
				{
					cmRelationship.Position++;
					dgRelationship.Rows[cmRelationship.Position].Selected = true;
					ShowRelation(RelationshipTable.Rows[cmRelationship.Position]);
				}

				RefreshParentChildTables();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		private void btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				int idx = cmRelationship.Position;
				if (idx == -1)
					return;

				DataRow dr = RelationshipTable.Rows[idx];
				dr[ChildColumn] = string.Empty;

				RefreshParentChildTables();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnLoadDefinition_Click(object sender, EventArgs e)
		{
			try
			{
				using (OpenFileDialog d = new OpenFileDialog())
				{
					d.Title = "Load Definition from ...";
					d.DefaultExt = "xml";
					d.CheckFileExists = true;
					d.Filter = "All XML Files (*.xml)|*.xml";
					if (d.ShowDialog() != DialogResult.OK)
						return;

					RelationshipTable.Clear();
					RelationshipTable.ReadXml(d.FileName);

					RefreshParentChildTables();
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		private void btnSaveDefinition_Click(object sender, EventArgs e)
		{
			try
			{
				using (SaveFileDialog d = new SaveFileDialog())
				{
					d.Title = "Save Definition to ...";
					d.DefaultExt = "xml";
					d.OverwritePrompt = true;
					d.Filter = "All XML Files (*.xml)|*.xml";
					if (d.ShowDialog() != DialogResult.OK)
						return;

					if (string.IsNullOrEmpty(RelationshipTable.TableName))
						RelationshipTable.TableName = "Relation";

					using (var ds = new DataSet("DataColumnMapper"))
					{
						ds.Tables.Add(RelationshipTable.Copy());
						ds.WriteXml(d.FileName, XmlWriteMode.IgnoreSchema);
					}

				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}

		}
		#endregion

		#region dgParent / dgChild controls
		private void dsvParent_DataMemberChanged(object sender, EventArgs e)
		{
			//ShowRelation(parentColumn, new TableColumn(dsvParent.TableName, dsvParent.ColumnName));
			MarkUsedColumns(dsvParent, ParentColumn);
		}
		private void dsvParent_CellChanged(object sender, CellChangedEventArgs e)
		{
		}
		private void dsvParent_CellDoubleClick(object sender, EventArgs e)
		{
			dsvParent_DataMemberChanged(sender, e);
		}

		private void dsvChild_DataMemberChanged(object sender, EventArgs e)
		{
			//ShowRelation(childColumn, new TableColumn(dsvChild.TableName, dsvChild.ColumnName));
			MarkUsedColumns(dsvChild, ChildColumn);
		}
		private void dsvChild_CellChanged(object sender, CellChangedEventArgs e)
		{
		}
		private void dsvChild_CellDoubleClick(object sender, EventArgs e)
		{
			dsvChild_DataMemberChanged(sender, e);
		}
		#endregion
		#endregion

		#region Private methods
		private void MapRelation(DataRow dr)
		{
			try
			{
				//if ((new TableColumn(dsvParent.TableName, dsvParent.ColumnName)).FullName != dr[ParentColumn].ToString())
				//{
				//	int i = 1;
				//}
				//dr[ParentColumn] = new TableColumn(dsvParent.TableName, dsvParent.ColumnName).FullName;
				dr[ChildColumn] = new TableColumn(dsvChild.TableName, dsvChild.ColumnName).FullName;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		[Obsolete("not being used at all", true)]
		private void ShowRelation(string useColumnName, TableColumn tc)
		{
			try
			{
				DataView dv = GetRelation(useColumnName, tc);
				if (dv == null || dv.Count == 0)
				{
					SelectRelation(null);
					ShowRelation(null);
					return;
				}

				ShowRelation(dv[0].Row);
				SelectRelation(dv[0].Row);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		private DataView GetRelation(string useColumnName, TableColumn tc)
		{
			DataView dv = new DataView(RelationshipTable);
			dv.RowFilter = string.Format("{0} = '{1}'", useColumnName, tc);
			return dv.Count == 0 ? null : dv;
		}

		private void ShowRelation(DataRow dr)
		{
			if (dr == null || dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached)
			{
				SelectColumn(dsvParent, string.Empty);
				SelectColumn(dsvChild, string.Empty);
				return;
			}
			SelectColumn(dsvParent, dr[ParentColumn].ToString());
			SelectColumn(dsvChild, dr[ChildColumn].ToString());
		}

		private void SelectRelation(DataRow dataRow)
		{
			if (dataRow == null)
			{
				if (cmRelationship != null)
					cmRelationship.Position = -1;
				return;
			}

			// update position
			for (int i = 0; i < RelationshipTable.Rows.Count; i++)
			{
				DataRow dr = RelationshipTable.Rows[i];
				if (dr == dataRow)
				{
					cmRelationship.Position = i;
					break;
				}
			}
		}

		private void RefreshParentChildTables()
		{
			if (string.IsNullOrEmpty(ParentColumn) || string.IsNullOrEmpty(ChildColumn))
				return;
			MarkUsedColumns(dsvParent, ParentColumn);
			MarkUsedColumns(dsvChild, ChildColumn);
		}

		private void SelectColumn(SimpleDataSetViewer grid, string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
			{
				grid.ColumnName = null;
				return;
			}
			TableColumn tc = new TableColumn(columnName);
			//grid.SetTableColumn(tc);
			grid.TableName = tc.TableName;
			grid.ColumnName = tc.ColumnName;
		}

		private string GetColumns(DataGridViewSelectedColumnCollection selectedColumns)
		{
			return selectedColumns[0].HeaderText;
		}

		private TableColumn[] GetRelationColumns(TableColumn relation)
		{
			try
			{
				if (cmRelationship == null)
					return null;

				List<TableColumn> columns = new List<TableColumn>();
				for (int i = 0; i < RelationshipTable.Rows.Count; i++)
				{
					DataRow dr = RelationshipTable.Rows[i];

					if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Detached)
						continue;

					if (DataConvert.IsNull(dr[relation.ColumnName]))
						continue;

					string fullName = dr[relation.ColumnName].ToString();

					if (!fullName.Contains("."))
					{
						fullName = string.Format("{0}.{1}", relation.TableName, dr[relation.ColumnName]);
						dr[relation.ColumnName] = fullName;
					}

					TableColumn tc = new TableColumn(fullName);

					if (columns.Contains(tc))
						continue;

					columns.Add(tc);
				}
				return columns.ToArray();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		private void MarkUsedColumns(SimpleDataSetViewer grid, string useColumnName)
		{
			try
			{
				TableColumn tc = new TableColumn(grid.TableName, useColumnName);
				grid.SetBackColorByColumns(Color.LightGray, Color.Red, GetRelationColumns(tc));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get the column name associated to the parent data set
		/// </summary>
		public string ParentColumn
		{
			get;
			private set;
		}

		/// <summary>
		/// Get the column name associated to the child data set
		/// </summary>
		public string ChildColumn
		{
			get;
			private set;
		}

		private DataTable relationshipTable = null;
		/// <summary>
		/// Get the relationship data source
		/// </summary>
		public DataTable RelationshipTable
		{
			get
			{
				return relationshipTable;
			}
			private set
			{
				relationshipTable = value;
				dgRelationship.DataSource = value;

				if (value != null)
					cmRelationship = (CurrencyManager)this.BindingContext[RelationshipTable];

			}
		}

		/// <summary>
		/// Get the parent DataSet
		/// </summary>
		public DataSet ParentDataSet
		{
			get;
			private set;
		}

		/// <summary>
		/// Get the child's DataSet
		/// </summary>
		public DataSet ChildDataSet
		{
			get;
			private set;
		}

		/// <summary>
		/// show or hide the parent grid
		/// </summary>
		public bool ShowParentGrid
		{
			get
			{
				return !scPreview.Panel1Collapsed;
			}
			set
			{
				scPreview.Panel1Collapsed = !value;
			}
		}

		/// <summary>
		/// show or hide the child grid
		/// </summary>
		public bool ShowChildGrid
		{
			get
			{
				return !scPreview.Panel2Collapsed;
			}
			set
			{
				scPreview.Panel2Collapsed = !value;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Clear all relationship entry
		/// </summary>
		public void ClearRelations()
		{
			if (RelationshipTable == null)
				return;
			foreach (DataRow dr in RelationshipTable.Rows)
			{
				dr[this.ChildColumn] = string.Empty;
			}
			RefreshParentChildTables();
		}

		/// <summary>
		/// set the relation table and parent/child column information
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="parentColumn"></param>
		/// <param name="childColumn"></param>
		public void SetRelation(DataTable dt, string parentColumn, string childColumn)
		{
			this.ParentColumn = parentColumn;
			this.ChildColumn = childColumn;

			RelationshipTable = dt;
		}

		/// <summary>
		/// Set the parent's column name
		/// </summary>
		/// <param name="dataSet"></param>
		public void SetParent(DataSet dataSet)
		{
			//this.parentColumn = columnName;
			ParentDataSet = dataSet;
			//dsvParent.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			dsvParent.DataSource = dataSet;
			dsvParent.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
			//dsvParent.HideTablePanel = true;
		}

		/// <summary>
		/// Set the child's column name
		/// </summary>
		/// <param name="dataSet"></param>
		public void SetChild(DataSet dataSet)
		{
			//this.childColumn = columnName;
			ChildDataSet = dataSet;
			//dsvChild.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			dsvChild.DataSource = dataSet;
			dsvChild.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
			//dsvChild.HideTablePanel = true;
		}

		/// <summary>
		/// Show or hide the Add / Delete buttons
		/// </summary>
		/// <param name="flag"></param>
		public void ShowAddDeleteButtons(bool flag)
		{
			btnAdd.Enabled = btnAdd.Visible = flag;
			btnDelete.Enabled = btnDelete.Visible = flag;
		}

		/// <summary>
		/// Show or hide the Load / Save buttons
		/// </summary>
		/// <param name="flag"></param>
		public void ShowLoadSaveButtons(bool flag)
		{
			btnLoadDefinition.Enabled = btnLoadDefinition.Visible = flag;
			btnSaveDefinition.Enabled = btnSaveDefinition.Visible = flag;
		}
		#endregion
	}
}