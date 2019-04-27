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
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using crudwork.DataAccess;
using crudwork.Utilities;
using crudwork.Controls;
using crudwork.Models.DataAccess;
using crudwork.DataSetTools;
using System.Diagnostics;
using crudwork.Controls.Wizard;
using crudwork.FileImporters;
using crudwork.DataWarehouse;
using crudwork.DataAccess.SQLiteClient;

namespace crudwork.Controls
{
	/// <summary>
	/// The SQLite query analyzer
	/// </summary>
	[Obsolete("consider using QueryAnalyzer", true)]
	public partial class SQLiteQueryAnalyzer : Form
	{
		private DataFactory df = null;
		private string oldConnectionString = string.Empty;
		private DataConnectionInfo _csbResult = null;

		#region Constructors
		/// <summary>
		/// create new instance with default attributes.
		/// </summary>
		public SQLiteQueryAnalyzer()
		{
			InitializeComponent();
			dsvResult.ShowTab(TabName.QueryAnalyzer, false);
		}
		#endregion

		#region Application Events
		private void Form1_Load(object sender, EventArgs e)
		{
			txtQuery.Focus();
		}

		private void btnBuildConnectString_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				ConnectionString = ControlManager.ShowConnectionStringBuilder(ConnectionString, DatabaseProvider.SQLite, false).ConnectionString;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
				txtQuery.Focus();
			}
		}

		private void tsbRunQuery_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				string query = !string.IsNullOrEmpty(SelectedQuery) ? SelectedQuery : Query;

				if (IsQAStatement(query))
				{
					ShowResult(ExecuteQA(query));
				}
				else if (query.StartsWith("@"))	// ie, Is a macro statement
				{
					ShowResult(ExecuteMacro(query));
				}
				else if (query.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
				{
					ShowResult(FillDataSet(query));
				}
				else
				{
					ExecuteNonQuery(query);
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
				txtQuery.Focus();
			}
		}

		private void DataExportWizard_OnExecute(object sender, WizardEventArgs e2)
		{
			var d = sender as DataExportWizard;

			var dfExport = new DataFactory(d.Destination);
			dfExport.TestConnection();

			var selectedTables = d.SelectedTables;

			if (d.Source.Provider == DatabaseProvider.Unspecified)
			{
				// use ImportManager to import a file via the connection string
				var ds = ImportManager.Import(d.Source.Filename, d.Source.Options);

				foreach (DataTable dt in ds.Tables)
				{
					if (Array.IndexOf<string>(selectedTables, dt.TableName) < 0)
						continue;
					dfExport.CreateTable(dt.TableName, dt);
				}
			}
			else
			{
				var dfImport = new DataFactory(d.Source);
				dfImport.TestConnection();

				foreach (var item in selectedTables)
				{
					var dt = dfImport.FillTable("select * from [" + item + "]");
					dfExport.CreateTable(item, dt);
				}
			}
		}

		private void tsbImport_Click(object sender, EventArgs e)
		{
			try
			{
				using (var d = new DataExportWizard())
				{
					d.Source = null;
					if (DataFactory != null)
						d.Destination = new DataConnectionInfo(DatabaseProvider.SQLite, DataFactory.ConnectionStringManager.ConnectionString);

					d.OnExecute += new WizardEventHandler(DataExportWizard_OnExecute);
					d.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
				txtQuery.Focus();
			}
		}

		private void tsbExport_Click(object sender, EventArgs e)
		{
			try
			{
				using (var d = new DataExportWizard())
				{
					if (DataFactory != null)
						d.Source = new DataConnectionInfo(DatabaseProvider.SQLite, DataFactory.ConnectionStringManager.ConnectionString);
					d.Destination = null;

					d.OnExecute += new WizardEventHandler(DataExportWizard_OnExecute);
					d.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
				txtQuery.Focus();
			}
		}

		private void tsbListTables_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				ShowResult(FillDataSet("select * from sqlite_master order by name"));
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
				txtQuery.Focus();
			}
		}

		private void tsbGetTableCounts_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				StringBuilder sb = new StringBuilder();
				var dt = FillTable("select * from sqlite_master where type='table' order by name");
				foreach (DataRow dr in dt.Rows)
				{
					if (sb.Length > 0)
						sb.AppendLine("union all");
					sb.AppendFormat("select '{0}' as Tablename, count(*) as RowCount from [{0}]" + Environment.NewLine,
						dr["name"]);
				}
				ShowResult(FillDataSet(sb.ToString()));
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
				txtQuery.Focus();
			}
		}

		private void tsbGetSamples_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);
				DataSet ds = new DataSet("Preview");
				var dt = FillTable("select * from sqlite_master where type='table' order by name");
				foreach (DataRow dr in dt.Rows)
				{
					var dtNew = FillTable("select * from [" + dr["name"] + "] limit 100");
					dtNew.TableName = dr["name"].ToString();
					ds.Tables.Add(dtNew);
				}
				ShowResult(ds);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
				txtQuery.Focus();
			}

		}

		private void txtQuery_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
				tsbRunQuery_Click(sender, EventArgs.Empty);
		}
		#endregion

		#region Helpers
		private void TestConnection()
		{
			DataFactory.TestConnection();
		}

		private DataSet FillDataSet(string query)
		{
			return DataFactory.Fill(query);
		}

		private DataTable FillTable(string query)
		{
			return DataFactory.FillTable(query);
		}

		private void ExecuteNonQuery(string query)
		{
			int result = DataFactory.ExecuteNonQuery(query);
			MessageBox.Show("Query returned a result of " + result);
		}

		private string TrimWS(string value)
		{
			return value.Trim(' ', '\t', '\n');
		}

		/// <summary>
		/// set the DataSetViewer's Datasource
		/// </summary>
		/// <param name="dataSource"></param>
		private void ShowResult(DataSet dataSource)
		{
			dsvResult.DataSource = dataSource;
			dsvResult.HideTablePanel = dataSource.Tables.Count <= 1;
		}

		private bool IsQAStatement(string query)
		{
			try
			{
				var statement = QueryParser.Parse(query);

				if (statement == null)
					return false;

				switch (statement.StatementType)
				{
					case QAStatementType.OpenFile:
					case QAStatementType.OpenDatabase:
						//case QAStatementType.SaveDataSet:
						//case QAStatementType.SaveDataTable:
						return true;

					default:
						return false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return false;
			}
		}

		private DataSet ExecuteQA(string query)
		{
			var qm = new QueryManager();
			var result = qm.RunBatch(query);

			foreach (var item in result)
			{
				if (!string.IsNullOrEmpty(item.ErrorText))
					throw new ApplicationException(item.ErrorText);
			}

			return qm.DataSet;
		}

		private DataSet ExecuteMacro(string query)
		{
			return DataFactory.MacroManager.Execute(query);
		}
		#endregion

		#region Properties
		private DataFactory DataFactory
		{
			get
			{
				if (df == null || ConnectionString != oldConnectionString)
				{
					df = new DataFactory(DatabaseProvider.SQLite, ConnectionString);
					oldConnectionString = ConnectionString;
				}
				return df;
			}
		}

		/// <summary>
		/// Get or set the connection string
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return txtConnectionString.Text;
			}
			set
			{
				txtConnectionString.Text = value;
			}
		}

		/// <summary>
		/// Get or set the query statement
		/// </summary>
		public string Query
		{
			get
			{
				return string.IsNullOrEmpty(txtQuery.Text) ? string.Empty : TrimWS(txtQuery.Text);
			}
			set
			{
				txtQuery.Text = value;
			}
		}

		/// <summary>
		/// Get the selected text, or empty if not selected.
		/// </summary>
		private string SelectedQuery
		{
			get
			{
				return string.IsNullOrEmpty(txtQuery.SelectedText) ? string.Empty : TrimWS(txtQuery.SelectedText);
			}
		}
		#endregion
	}
}
