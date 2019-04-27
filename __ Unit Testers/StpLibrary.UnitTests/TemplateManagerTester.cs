using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using crudwork.Utilities;

namespace crudwork.UnitTests
{
	/// <summary>
	/// Summary description for DatawarehouseTest
	/// </summary>
	[TestClass]
	public class TemplateManagerTester
	{
		public TemplateManagerTester()
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
		public void Expand()
		{
			var tpl = @"Hello [FIRST], my name is [SALESPERSON] at [COMPANY].  May I interest you in a new product called '[PRODUCT]'?";

			#region Table
			var dt = new DataTable();
			dt.Columns.Add("FIRST", typeof(string));
			dt.Columns.Add("SALESPERSON", typeof(string));
			dt.Columns.Add("PRODUCT", typeof(string));

			dt.Rows.Add("Bob", "James", "Acme Drink");
			dt.Rows.Add("Jim", "Erin", "Shoe Polish");
			dt.Rows.Add("Simon", "Jack", "Slim Green Jean");
			dt.Rows.Add("Erica", "Bob", "Turbo Vacuum");
			#endregion

			#region Using various delimiters
			{
				Console.WriteLine("Using delimiter: [...]");
				var mytpl = tpl;
				var tm = new TemplateManager("\\[", "\\]", "[^\\]]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: <...>");
				var mytpl = tpl.Replace("[", "<").Replace("]", ">");
				var tm = new TemplateManager("<", ">", "[^>]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: {...}");
				var mytpl = tpl.Replace("[", "{").Replace("]", "}");
				var tm = new TemplateManager("{", "}", "[^}]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: [@...]");
				var mytpl = tpl.Replace("[", "[@").Replace("]", "]");
				var tm = new TemplateManager("\\[@", "\\]", "[^\\]]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: <<...>>");
				var mytpl = tpl.Replace("[", "<<").Replace("]", ">>");
				var tm = new TemplateManager("<<", ">>", "[^\\>]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}
			#endregion
		}
	}
}
