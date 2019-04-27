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

namespace crudwork.Controls
{
	internal partial class SimpleDataRelationViewer : UserControl
	{
		private DataSet ds = null;
		private DataTable dataRelationDataTable = null;

		public SimpleDataRelationViewer()
		{
			InitializeComponent();
		}

		public DataSet DataSource
		{
			get
			{
				return ds;
			}
			set
			{
				ds = value;
				dataRelationDataTable = DataUtil.GetRelation(ds);
				dataRelationDataTable.DefaultView.Sort = "ParentTable,ChildTable,ParentColumns,ChildColumns,Nested";
				dataGridView1.DataSource = dataRelationDataTable.DefaultView;
			}
		}

		public int Count
		{
			get
			{
				if (dataRelationDataTable == null)
					return -1;
				else
					return dataRelationDataTable.Rows.Count;
			}
		}
	}
}
