using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using crudwork.DataAccess;
using crudwork.Parsers;

namespace crudwork.UnitTests
{
	[TestClass]
	public class ParsingTest
	{
		#region Junks
		public ParsingTest()
		{
		}

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get;
			set;
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion
		#endregion

		[TestMethod,Description("Build 45 - not able to read the connection string with '=' or ';' ")]
		public void TestConnectionStringWithEqualAndSemicolonChar()
		{
			// Arrange
			string connStr = ConnectionStringManager.MakeExcel(@"c:\f=oo;ba'r.xls", null, null, true, false);
			Assert.AreEqual(connStr, "Provider=\"Microsoft.Jet.OLEDB.4.0\"; Data Source=\"c:\\f=oo;ba'r.xls\"; user id=\"admin\"; password=\"\"; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=1\";");

			// Act
			var result = ConnectionStringParser.Parse(connStr);

			foreach (var item in result)
			{
				Console.WriteLine("[{0}] = [{1}]", item.Key, item.Value);
			}

			// Assert
			Assert.AreEqual(result["Provider"], @"Microsoft.Jet.OLEDB.4.0");
			Assert.AreEqual(result["Data Source"], @"c:\f=oo;ba'r.xls");
			Assert.AreEqual(result["user id"], @"admin");
			Assert.AreEqual(result["password"], @"");
			Assert.AreEqual(result["Extended Properties"], @"Excel 8.0; HDR=Yes; IMEX=1");
			Assert.AreEqual(result.Count, 5);
		}

		[TestMethod, Description("Build 45 - skipped the last entry ")]
		public void TestSQLiteConnectionString()
		{
			// Arrange
			string connStr = ConnectionStringManager.MakeSQLite(@"c:\f=oo;ba'r.xls", "mypassword!", false, false);

			// Act
			var result = ConnectionStringParser.Parse(connStr);

			// Assert
			Assert.AreEqual(result["data source"], @"c:\f=oo;ba'r.xls");
			Assert.AreEqual(result["password"], @"mypassword!");
			Assert.AreEqual(result["read only"], @"false");
			Assert.AreEqual(result["failifmissing"], @"false");
			Assert.AreEqual(result["compress"], @"true");
			Assert.AreEqual(result.Count, 5);
		}
	}
}
