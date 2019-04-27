using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using crudwork.MultiThreading;
using System.Data;

namespace crudwork.UnitTests
{
	/// <summary>
	/// Summary description for MultiThreadingTest
	/// </summary>
	[TestClass]
	public class MultiThreadingTest
	{
		public MultiThreadingTest()
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
		public void MTStartCallback()
		{
			var input = new List<int>();
			input.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
			var output = MT.Start<int, int>(input, 16, (e) =>
			{
				e.Result = e.Input * 10;
			});
		}

		[TestMethod]
		public void MTStartContainer()
		{
			var input = new List<int>();
			input.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
			var output = MT.Start<int, int>(input, 16, typeof(MTContainer), null);
		}

		internal class MTContainer : MultiThreadingBase<int, int>
		{
			protected override void ProcessInput(MultiThreadEventArgs<int, int> e)
			{
				e.Result = e.Input * 10;
			}
		}
	}
}
