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
using crudwork.DataAccess;
using crudwork.Utilities;
using System.Diagnostics;

namespace crudwork.Controls
{
	/// <summary>
	/// DataSet Viewer
	/// </summary>
	public partial class DataSetViewer : UserControl
	{
		private DataSet ds = null;
		private List<TabPage> tabPages = new List<TabPage>();

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public DataSetViewer()
		{
			InitializeComponent();

			#region Associate a TabName to each tabs
			tabDataSet.Tag = TabName.DataSet;
			tabMetadata.Tag = TabName.Metadata;
			tabRelation.Tag = TabName.Relation;
			tabXML.Tag = TabName.XML;
			tabQueryAnalyzer.Tag = TabName.QueryAnalyzer;
			#endregion

			tabControl1.SelectedIndex = 0;

			#region Save the list for show/hide tabs
			tabPages.Add(tabDataSet);
			tabPages.Add(tabMetadata);
			tabPages.Add(tabRelation);
			tabPages.Add(tabXML);
			tabPages.Add(tabQueryAnalyzer);
			#endregion
		}

		/// <summary>
		/// Show or hide the specified TabName
		/// </summary>
		/// <param name="tabName"></param>
		/// <param name="isVisible"></param>
		public void ShowTab(TabName tabName, bool isVisible)
		{
			// the TabPage does not have a Visible property; therefore, we need
			// to permanently remove it from the TabControl.  Because of this,
			// we will maintain a list of tabs with 'tabPages' to add them back
			// to the control later.
			if (!isVisible)
			{
				foreach (TabPage item in tabControl1.TabPages)
				{
					if (tabName != (TabName)item.Tag)
						continue;

					tabControl1.TabPages.Remove(item);
					break;
				}
			}
			else
			{
				foreach (TabPage item in tabPages)
				{
					if (tabName != (TabName)item.Tag)
						continue;

					tabControl1.TabPages.Add(item);
					break;
				}
			}
		}

		#region Events
		private void DataSetViewer_Load(object sender, EventArgs e)
		{
			//tabControl1_SelectedIndexChanged(sender, e);
		}

		private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
		{
			try
			{
				TabName tab = (TabName)e.TabPage.Tag;

				switch (tab)
				{
					case TabName.QueryAnalyzer:
						// query analyzer command(s) may create new instance of DataSet:
						// 1) clear;
						if (queryAnalyzer1.DataSource != null)
							ds = queryAnalyzer1.DataSource;
						break;

					default:
						// nothing special need to do ...
						break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
			}
		}

		private void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				TabName tab = (TabName)e.TabPage.Tag;
				switch (tab)
				{
					case TabName.DataSet:
						//if (simpleDataSetViewer1.DataSource == null)
						simpleDataSetViewer1.DataSource = ds;
						tabControl1.SelectedTab.Text = tab + " - " + simpleDataSetViewer1.Count;
						break;
					case TabName.Metadata:
						//if (simpleMetaDataViewer1.DataSource == null)
						simpleMetaDataViewer1.DataSource = ds;
						tabControl1.SelectedTab.Text = tab + " - " + simpleMetaDataViewer1.Count;
						break;
					case TabName.Relation:
						//if (simpleDataRelationViewer1.DataSource == null)
						simpleDataRelationViewer1.DataSource = ds;
						tabControl1.SelectedTab.Text = tab + " - " + simpleDataRelationViewer1.Count;
						break;
					case TabName.XML:
						//if (simpleXMLViewer1.DataSource == null)
						simpleXMLViewer1.DataSource = ds;
						break;
					case TabName.QueryAnalyzer:
						//if (queryAnalyzer1.DataSource == null)
						queryAnalyzer1.DataSource = ds;
						break;
					default:
						throw new ArgumentOutOfRangeException("tab=" + tab);
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		#endregion

		/// <summary>
		/// Set the table column
		/// </summary>
		/// <param name="tc"></param>
		public void SetTableColumn(TableColumn tc)
		{
			TableName = tc.TableName;
			ColumnName = tc.ColumnName;
			MDV.SetRow(tc);
		}

		#region Properties for SimpleDataSetViewer
		/// <summary>
		/// Get the SimpleDataSetViewer object
		/// </summary>
		//[Obsolete("this is for internal uses only", false)]
		private SimpleDataSetViewer DSV
		{
			get
			{
				return simpleDataSetViewer1;
			}
		}

		/// <summary>
		/// Set the column's auto resize mode
		/// </summary>
		/// <param name="mode"></param>
		public void AutoResizeColumns(DataGridViewAutoSizeColumnsMode mode)
		{
			DSV.AutoResizeColumns(mode);
		}

		/// <summary>
		/// Get or set the DataSource
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
				simpleDataSetViewer1.DataSource = null;
				simpleMetaDataViewer1.DataSource = null;
				simpleDataRelationViewer1.DataSource = null;
				simpleXMLViewer1.DataSource = null;
				//tabControl1_SelectedIndexChanged(this, EventArgs.Empty);
				tabControl1_Selected(this, new TabControlEventArgs(tabDataSet, 0, TabControlAction.Selected));
			}
		}

		/// <summary>
		/// Get or set the tablename
		/// </summary>
		public string TableName
		{
			get
			{
				return DSV.TableName;
			}
			set
			{
				DSV.TableName = value;
			}
		}

		/// <summary>
		/// Get or set the column name
		/// </summary>
		public string ColumnName
		{
			get
			{
				return DSV.ColumnName;
			}
			set
			{
				DSV.ColumnName = value;
			}
		}

		/// <summary>
		/// Show or hide the table panel
		/// </summary>
		public bool HideTablePanel
		{
			get
			{
				return DSV.HideTablePanel;
			}
			set
			{
				DSV.HideTablePanel = value;
			}
		}

		/// <summary>
		/// The DataMemberChange event handler
		/// </summary>
		public event EventHandler DataMemberChanged
		{
			add
			{
				DSV.DataMemberChanged += value;
			}
			remove
			{
				DSV.DataMemberChanged -= value;
			}
		}

		/// <summary>
		/// The Cell Double-click event handler
		/// </summary>
		public event EventHandler CellDoubleClick
		{
			add
			{
				DSV.CellDoubleClick += value;
			}
			remove
			{
				DSV.CellDoubleClick -= value;
			}
		}

		/// <summary>
		/// The Cell changed event handler
		/// </summary>
		public event CellChangedEventHandler CellChanged
		{
			add
			{
				DSV.CellChanged += value;
			}
			remove
			{
				DSV.CellChanged -= value;
			}
		}

		/// <summary>
		/// Set the background color for the columns
		/// </summary>
		/// <param name="backColor"></param>
		/// <param name="selectionBackColor"></param>
		/// <param name="columns"></param>
		public void SetBackColorByColumns(Color backColor, Color selectionBackColor, params TableColumn[] columns)
		{
			DSV.SetBackColorByColumns(backColor, selectionBackColor, columns);
		}

		/// <summary>
		/// Get or set  the selection mode
		/// </summary>
		public DataGridViewSelectionMode SelectionMode
		{
			get
			{
				return DSV.SelectionMode;
			}
			set
			{
				DSV.SelectionMode = value;
			}
		}
		#endregion

		#region Properties for SimpleMetaDataViewer
		private SimpleMetaDataViewer MDV
		{
			get
			{
				return simpleMetaDataViewer1;
			}
		}

		/// <summary>
		/// The Cell Change event handler
		/// </summary>
		public event CellChangedEventHandler MetadataCellChanged
		{
			add
			{
				MDV.CellChanged += value;
			}
			remove
			{
				MDV.CellChanged -= value;
			}
		}
		#endregion
	}

	/// <summary>
	/// List of tab name
	/// </summary>
	public enum TabName
	{
		/// <summary>DataSet tab</summary>
		DataSet,
		/// <summary>Metadata tab</summary>
		Metadata,
		/// <summary>Relation tab</summary>
		Relation,
		/// <summary>XML tab</summary>
		XML,
		/// <summary>QueryAnalyzer tab</summary>
		QueryAnalyzer,
	}
}
