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
using System.Drawing;
using System.Data;

namespace crudwork.Controls
{
	class ExtendedDataGridView : DataGridView
	{
		private int offset1 = 15;
		private ContextMenuStrip contextMenuStrip1;
		private System.ComponentModel.IContainer components;
		private ToolStripMenuItem mnuOpenPivotTableEditor;
		private ToolStripMenuItem mnuResize;
		private int offset2 = 20;

		private object dataSource = null;

		public ExtendedDataGridView()
			: base()
		{
			InitializeComponent();
			base.ContextMenuStrip = contextMenuStrip1;
			base.EditMode = DataGridViewEditMode.EditOnEnter;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuOpenPivotTableEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResize = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenPivotTableEditor,
            this.mnuResize});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(170, 70);
			this.contextMenuStrip1.Click += new System.EventHandler(this.contextMenuStrip1_Click);
			// 
			// mnuOpenPivotTableEditor
			// 
			this.mnuOpenPivotTableEditor.Name = "mnuOpenPivotTableEditor";
			this.mnuOpenPivotTableEditor.Size = new System.Drawing.Size(169, 22);
			this.mnuOpenPivotTableEditor.Text = "Pivot Table Editor";
			this.mnuOpenPivotTableEditor.Click += new System.EventHandler(this.mnuOpenPivotTableEditor_Click);
			// 
			// mnuResize
			// 
			this.mnuResize.Name = "mnuResize";
			this.mnuResize.Size = new System.Drawing.Size(169, 22);
			this.mnuResize.Text = "Resize Columns";
			this.mnuResize.Click += new System.EventHandler(this.mnuResize_Click);
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		private int LengthOf(object value)
		{
			return value == null ? 0 : value.ToString().Length;
		}

		protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
		{
			string format = "{0:D" + LengthOf(base.RowCount) + "}";
			string value = string.Format(format, e.RowIndex + 1);

			// adjust the width to fit the row number
			SizeF size = e.Graphics.MeasureString(value, base.Font);
			base.RowHeadersWidth = Math.Max(base.RowHeadersWidth, (int)size.Width + offset2);

			// draw the row number
			Rectangle r = e.RowBounds;
			PointF p = new PointF(r.Location.X + offset1, r.Top + ((r.Height - size.Height) / 2));
			e.Graphics.DrawString(value, base.Font, SystemBrushes.ControlText, p);

			base.OnRowPostPaint(e);
		}

		protected override void OnDataSourceChanged(EventArgs e)
		{
			if (base.DataSource is DataSet)
				this.dataSource = base.DataSource;

			base.OnDataSourceChanged(e);
		}

		private void contextMenuStrip1_Click(object sender, EventArgs e)
		{
		}

		private void mnuResize_Click(object sender, EventArgs e)
		{
			base.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		}

		private void mnuOpenPivotTableEditor_Click(object sender, EventArgs e)
		{
			DataTable dt = null;
			if (this.dataSource is DataSet)
				dt = ((DataSet)this.dataSource).Tables[this.DataMember];
			else
				dt = this.dataSource as DataTable;

			if (dt == null)
			{
				MessageBox.Show("Input data source must be a data table");
				return;
			}

			ControlManager.ShowPivotTableEditor(dt);
		}
	}
}