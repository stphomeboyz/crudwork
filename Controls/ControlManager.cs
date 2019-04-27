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
using crudwork.Utilities;
using System.Data.Common;
using System.Data;
using crudwork.DataAccess;
using crudwork.Controls.DatabaseUC;
using crudwork.Models.DataAccess;
using crudwork.Controls.DataTools;
using crudwork.FileImporters;

namespace crudwork.Controls
{
	/// <summary>
	/// Common Utility to show dialog box.
	/// </summary>
	public static class ControlManager
	{
		/// <summary>
		/// display a TextDialog form
		/// </summary>
		/// <param name="message"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		public static DialogResult ShowTextDialog(StringBuilder message, string title)
		{
			var s = message.ToString();
			var dr = ShowTextDialog(ref s, title);
			if (dr == DialogResult.OK)
			{
				message.Length = 0;
				message.Append(s);
			}
			return dr;
		}

		/// <summary>
		/// display a TextDialog form
		/// </summary>
		/// <param name="message"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		public static DialogResult ShowTextDialog(ref string message, string title)
		{
			try
			{
				using (TextDialog d = new TextDialog())
				{
					d.DialogTitle = title;
					d.Message = message;
					d.StartPosition = FormStartPosition.CenterParent;
					var dr = d.ShowDialog();
					if (dr == DialogResult.OK)
						message = d.Message;
					return dr;
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
				return DialogResult.OK;
			}
		}

		/// <summary>
		/// Display an InputBox form
		/// </summary>
		/// <param name="message"></param>
		/// <param name="title"></param>
		/// <param name="defaultValue"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static DialogResult GetInput(string message, string title, string defaultValue, ref string text)
		{
			using (InputBox d = new InputBox())
			{
				d.DialogTitle = title;
				d.Message = message;
				d.Input = defaultValue;
				d.StartPosition = FormStartPosition.CenterParent;
				DialogResult dr = d.ShowDialog();
				if (dr == DialogResult.OK)
					text = d.Input;
				return dr;
			}
		}

		/// <summary>
		/// Display a confirmation prompt
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool Confirm(string message)
		{
			return Confirm(message, "Confirmation");
		}

		/// <summary>
		/// Display a confirmation prompt
		/// </summary>
		/// <param name="message"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		public static bool Confirm(string message, string title)
		{
			return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
		}

		#region DataSetViewer
		/// <summary>
		/// Display a SimpleDataSetViewer form
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static DialogResult ShowDataSetViewer(string connectionString, DbCommand command)
		{
			DataFactory df = new DataFactory(connectionString);
			return ShowDataSetViewer(df.FillTable(command));
		}

		/// <summary>
		/// Display a SimpleDataSetViewer form
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="queryString"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static DialogResult ShowDataSetViewer(string connectionString, string queryString, params DbParameter[] parameters)
		{
			DataFactory df = new DataFactory(connectionString);
			return ShowDataSetViewer(df.FillTable(queryString, parameters));
		}

		/// <summary>
		/// Display a SimpleDataSetViewer form
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public static DialogResult ShowDataSetViewer(DataSet ds)
		{
			using (SimpleDataSetViewerForm f = new SimpleDataSetViewerForm())
			{
				f.DataSource = ds;
				return f.ShowDialog();
			}
		}

		/// <summary>
		/// Display a SimpleDataSetViewer form
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DialogResult ShowDataSetViewer(DataTable dt)
		{
			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(dt);
				return ShowDataSetViewer(ds);
			}
		}
		#endregion

		/// <summary>
		/// Display the DataTableAnalyzeTool form
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DialogResult AnalyzeTable(DataTable dt)
		{
			using (DataTableAnalysisTool f = new DataTableAnalysisTool())
			{
				f.DataSource = dt;
				return f.ShowDialog();
			}
		}

		/// <summary>
		/// Show the task scheduler
		/// </summary>
		/// <returns></returns>
		public static DialogResult ShowTaskScheduler()
		{
			using (TaskScheduler.MainDaySchedule uc = new TaskScheduler.MainDaySchedule())
			{
				return ShowForm(uc, "Task Scheduler");
			}
		}

		#region Query Analyzer
		/// <summary>
		/// Show the query analyser
		/// </summary>
		/// <returns></returns>
		public static DialogResult ShowQueryAnalyzer()
		{
			var sqldbFile = FileUtil.CreateTempFile("db", new string[0]);
			var connStr = ConnectionStringManager.MakeSQLite(sqldbFile, "", false, true);
			return ShowQueryAnalyzer(connStr, DatabaseProvider.SQLite);
		}

		/// <summary>
		/// Show the query analyser
		/// </summary>
		/// <returns></returns>
		public static DialogResult ShowQueryAnalyzer(DataSet ds)
		{
			var tmpFile = FileUtil.CreateTempFile("db", new string[0]);

			var df = new DataFactory(DatabaseProvider.SQLite, ConnectionStringManager.MakeSQLite(tmpFile, "", false, true));
			df.TestConnection();

			foreach (DataTable dt in ds.Tables)
			{
				df.CreateTable(dt.TableName, dt);
			}

			return ShowQueryAnalyzer(df.ConnectionStringManager.ConnectionString, df.Provider);
		}

		/// <summary>
		/// Show the SQLite query analyzer
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static DialogResult ShowSQLiteQueryAnalyzer(string connectionString)
		{
			using (var d = new QueryAnalyzer())
			{
				//d.MinimizeBox = false;
				d.Query = "";
				d.ConnectionString = connectionString;
				d.AllowUserToChangeProvider = false;
				d.DatabaseProvider = DatabaseProvider.SQLite;
				return ShowForm(d, "SQLite Query Analyzer");
			}
		}

		/// <summary>
		/// /// <summary>
		/// Show the query analyzer
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="databaseProvider"></param>
		/// <returns></returns>
		public static DialogResult ShowQueryAnalyzer(string connectionString, DatabaseProvider databaseProvider)
		{
			using (var d = new QueryAnalyzer())
			{
				//d.MinimizeBox = false;
				d.Query = "";
				d.ConnectionString = connectionString;
				d.AllowUserToChangeProvider = true;
				d.DatabaseProvider = databaseProvider;
				return ShowForm(d, "Query Analyzer + +");
			}
		}
		#endregion

		/// <summary>
		/// Promp the user for information to build a connection string.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="selectedProvider"></param>
		/// <param name="allowUserToChangeProvider"></param>
		/// <returns></returns>
		public static DataConnectionInfo ShowConnectionStringBuilder(string connectionString, DatabaseProvider selectedProvider, bool allowUserToChangeProvider)
		{
			using (var d = new ConnectionStringBuilderDialogBox())
			{
				d.SelectedDatabaseProvider = selectedProvider;
				d.AllowUserToChangeProvider = allowUserToChangeProvider;
				d.ConnectionString = connectionString;

				if (d.ShowDialog() != DialogResult.OK)
					return new DataConnectionInfo(selectedProvider, connectionString);

				return d.DatabaseConnection;
			}
		}

		/// <summary>
		/// Open a form to contain the user control
		/// </summary>
		/// <param name="userControl"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		private static DialogResult ShowForm(Control userControl, string title)
		{
			using (Form f = new Form())
			{
				userControl.Dock = DockStyle.Fill;

				f.Text = title;
				f.Size = new System.Drawing.Size(600, 400);
				f.Controls.Add(userControl);

				return f.ShowDialog();
			}
		}

		private static Form CreateForm(Control userControl, string title)
		{
			Form f = new Form();
			userControl.Dock = DockStyle.Fill;

			f.Text = title;
			f.Size = new System.Drawing.Size(600, 400);
			f.Controls.Add(userControl);
			return f;
		}

		/// <summary>
		/// show the selectable list box form
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public static DialogResult ShowSelectableListBox(Dictionary<string, bool> options)
		{
			using (var d = new SelectableListBox())
			{
				d.Options = options;

				var dr = d.ShowDialog();

				if (dr == DialogResult.OK)
					options = d.Options;

				return dr;
			}
		}

		/// <summary>
		/// Show a Pivot Table Editor form
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DialogResult ShowPivotTableEditor(DataTable dt)
		{
			using (var uc = new PivotTableEditor())
			using (var f = CreateForm(uc, "Pivot Table Editor"))
			{
				uc.DataSource = dt;
				return f.ShowDialog();
			}
		}
	}
}
