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
using crudwork.Controls;
using crudwork.FileImporters;

namespace OpenSchemaGenerator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			//txtParseCSV_Click(this, EventArgs.Empty);
		}

		private void txtParseCSV_Click(object sender, EventArgs e)
		{
			//FileConverter converter = new FileConverter();
			//DataSet ds = converter.Convert(txtFilename.Text);

#pragma warning disable 0618
			DataSet ds = ImportManager.Import(txtFilename.Text);
#pragma warning restore 0618
			dataSetViewer1.DataSource = ds;

			//Common.AnalyzeTable(ds.Tables[0]);
		}
	}
}