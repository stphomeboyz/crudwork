using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;
using System.Data;

namespace DataFactoryTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				ImportTableWithPrimaryKeyToSQLiteDB();
				//ListDatabases();
				//TestDatabaseConnection();
				//Test2();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static void ImportTableWithPrimaryKeyToSQLiteDB()
		{
			var df = new DataFactory(DatabaseProvider.SQLite, ConnectionStringManager.MakeSQLite("foo.db", "", false, false));
			df.TestConnection();

			var dt = new DataTable("MyTable");
			dt.Columns.Add("ID", typeof(int)).AutoIncrement = true;
			dt.Columns.Add("Name", typeof(string));

			dt.Rows.Add(1, "Abc");
			dt.Rows.Add(2, "Abc");
			dt.Rows.Add(3, "Abc");

			df.CreateTable(dt.TableName, dt);
		}

		private static void Test2()
		{
			var df = new DataFactory(DatabaseProvider.SqlClient, "data source=\"sql.vnesoft.com\"; user id=\"Va!dO!#!167892g\"; password=\"H@#d@Tag#6\"; initial catalog=\"\"");
			df.ExecuteNonQuery("select 1");
		}

		private static void TestDatabaseConnection()
		{
			//var df = new DataFactory(DatabaseProvider.SqlClient, "data source=\"sql.vnesoft.com\"; user id=\"Va!dO!#!167892g\"; password=\"H@#d@Tag#6\"; initial catalog=\"\"");
			//df.TestConnection();

			var df = new DataFactory(DatabaseProvider.SqlClient, "data source=\"sql.vnesoft.com\"; user id=\"Va!dO!#!167892g\"; password=\"H@#d@Tag#6\"; initial catalog=\"ARMLoyaltyMainDB\"");
			var tdl = df.Database.GetTables();
		}

		private static void ListDatabases()
		{
			string connStr = ConnectionStringManager.MakeSQLClient("sql.vnesoft.com", "", false, "Va!dO!#!167892g", "H@#d@Tag#6");
			var df = new DataFactory(DatabaseProvider.SqlClient, connStr);
			df.TestConnection();
			var ddl = df.Database.GetDatabases(true);
			Console.WriteLine(ddl.Count);
			var tdl = df.Database.GetTables();
		}
	}
}
