using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using crudwork.DataAccess;
using crudwork.Controls;
using crudwork.Models.DataAccess;
using crudwork.Utilities;

namespace QueryAnalyzerPlusPlus
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());

			string connectionString = string.Empty;
			string filename = (args.Length > 0) ? args[0] : FileUtil.CreateTempFile("db", new string[0]);
			connectionString = ConnectionStringManager.MakeSQLite(filename, string.Empty, false, true);
			ControlManager.ShowQueryAnalyzer(connectionString, DatabaseProvider.SQLite);
		}
	}
}
