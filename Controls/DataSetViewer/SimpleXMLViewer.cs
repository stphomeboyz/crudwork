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
using System.IO;
using System.Diagnostics;

namespace crudwork.Controls
{
	/// <summary>
	/// Simple XML Viewer control
	/// </summary>
	internal partial class SimpleXMLViewer : UserControl
	{
		private DataSet ds = null;
		private string xmlFile = string.Empty;

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public SimpleXMLViewer()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get or set the default DataSource
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
				UpdateXML();
			}
		}

		/// <summary>
		/// Set the column mapping type
		/// </summary>
		/// <param name="mappingType"></param>
		public void SetColumnMapping(MappingType mappingType)
		{
			DataUtil.SetColumnMapping(ds, mappingType);
		}

		/// <summary>
		/// Refresh the XML viewer
		/// </summary>
		private void UpdateXML()
		{
			try
			{
				this.Enabled = false;

				Uri uri;
				if (ds == null)
				{
					uri = new Uri("about:blank");
				}
				else
				{
					xmlFile = FileUtil.CreateTempFile("xml", new string[] { string.Empty });

					DataUtil.SetColumnMapping(ds, chkMapAsAttribute.Checked ? MappingType.Attribute : MappingType.Element);
					//DataUtil.NullifyValues(ds, chkIncludeNulls.Checked);

					ds.WriteXml(xmlFile);
					uri = new Uri(xmlFile);
				}
				webBrowser1.Url = uri;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
			finally
			{
				this.Enabled = true;
			}
		}

		private void chkMapAsAttribute_Click(object sender, EventArgs e)
		{
			UpdateXML();
		}

		private void chkIncludeNulls_Click(object sender, EventArgs e)
		{
			UpdateXML();
		}

		private void btnSaveAs_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				using (SaveFileDialog d = new SaveFileDialog())
				{
					d.Filter = "XML Files (*.xml)|*.xml";
					d.OverwritePrompt = true;
					if (d.ShowDialog() != DialogResult.OK)
						return;

					File.Copy(xmlFile, d.FileName, true);

					MessageBox.Show("XML File saved successfully to " + d.FileName);
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
	}
}
