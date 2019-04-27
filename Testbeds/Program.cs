using System;
using crudwork.Controls;
using crudwork.DataAccess;
using crudwork.FileImporters;
using crudwork.Models.DataAccess;

namespace Testbeds
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			TestImportManager();
		}

		static void TestQueryAnalyzer()
		{
			var connStr = ConnectionStringManager.MakeSQLite(@"%TMP%\foo.sqlitedb", null, false, false);
			ControlManager.ShowQueryAnalyzer(connStr, DatabaseProvider.SQLite);
		}

		static void TestImportManager()
		{
			var x = ImportManager.GetConverter(typeof(DelimiterImportOptions));
			x.Extensions = new string[] { "pip" };
		}
	}
}