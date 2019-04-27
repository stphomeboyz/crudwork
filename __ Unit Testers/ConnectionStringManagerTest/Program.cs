using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.DataAccess;
using crudwork.Parsers;

namespace ConnectionStringManagerTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				//ParseConnectionStringTest();
				UseFileWithSemicolon();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static void ParseConnectionStringTest()
		{
			string connStr = ConnectionStringManager.MakeExcel(@"c:\f=oo;ba'r.xls", null, null, true, false);
			var result = ConnectionStringParser.Parse(connStr);

			foreach (var item in result)
			{
				Console.WriteLine("[{0}] = [{1}]", item.Key, item.Value);
			}

		}

		private static void UseFileWithSemicolon()
		{
			string connStr = ConnectionStringManager.MakeExcel(@"C:\twork\ReAlign\Sample Test Files\invalid file with;semicolon.xml", null, null, true, false);
			ConnectionStringManager csm = new ConnectionStringManager();
			csm.ConnectionString = connStr;
			Console.WriteLine("cs=" + csm.ConnectionString);
		}
	}
}
