using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;
using crudwork.DataWarehouse;
using crudwork.DataAccess.SQLiteClient;
using System.Diagnostics;

namespace crudwork.UnitTests
{
	/// <summary>
	/// Summary description for DatawarehouseTest
	/// </summary>
	[TestClass]
	public class DatawarehouseTest
	{
		public DatawarehouseTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
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

		[TestMethod]
		public void DoPivotTest()
		{
			try
			{
				// data source="C:\twork\Realign\comp.db"; password=""; read only=false; failifmissing=false; compress=false
				var df = new DataFactory(DatabaseProvider.SQLite, ConnectionStringManager.MakeSQLite(@"c:\twork\realign\comp.db", "", false, true));
				df.MacroManager.Execute("@pivot b1_Zip_Comp_random1000 pivot sumCount zip foo");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
