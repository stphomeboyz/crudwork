using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.FileImporters;
using crudwork.Utilities;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace DelimiterFileImporter
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			csfSaveAs.DataConnectionInfo.Provider = DatabaseProvider.SQLite;
			csfSaveAs.DataConnectionInfo.ConnectionString = "data source=\"c:\foo.db\"; password=\"\"; read only=false; failifmissing=false; compress=false";
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			const string OUT_TABLENAME = @"CSVImportTest";
			const int MAX_BUFFER = 100;
			var start = DateTime.Now;

			try
			{
				var dci = csfSaveAs.DataConnectionInfo;

				if (dci.InputSource != InputSource.Database)
					throw new ArgumentException("Please choose a database");

				var df = new DataFactory(dci.Provider, dci.ConnectionString);
				df.TestConnection();

				if (df.Database.GetTables(OUT_TABLENAME, QueryFilter.Exact).Count >= 1)
					df.ExecuteNonQuery("drop table " + OUT_TABLENAME);

				int nr = 0;

				using (DataTable dt = new DataTable(OUT_TABLENAME))
				{
					var options = new DelimiterImportOptions(",");
					foreach (var item in ImportManager.ImportRow(txtFilename.Value, options))
					{
						nr++;

						#region Init Columns Definition
						if (nr == 1)
						{
							DataUtil.CopyColumns(item.Table.Columns, dt.Columns);
						}
						#endregion

						var dr = dt.NewRow();
						DataUtil.CopyRow(item, dr);
						dt.Rows.Add(dr);

						if (nr % MAX_BUFFER == 0)
						{
							df.AppendTable(OUT_TABLENAME, dt);
							dt.Rows.Clear();
						}
					}

					if (dt.Rows.Count > 0)
					{
						df.AppendTable(OUT_TABLENAME, dt);
						dt.Rows.Clear();
					}

				}

				MessageBox.Show("Done Elapsed = " + DateUtil.ElapsedTime(start, DateTime.Now));
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
	}
}
