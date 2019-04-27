using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using crudwork.MultiThreading;

namespace crudwork.UnitTests
{
	/// <summary>
	/// Summary description for BackgroundTests
	/// </summary>
	[TestClass]
	public class BackgroundTests
	{
		public BackgroundTests()
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
		public void RunBGWTest()
		{
			var ai = new BackgroundWorkerApplicationInfo()
			{
				ActiveForm = null,
				CancelButton = null,
				ProgressBar = null,
				StatusLabel = null,
				DisableControls = null,
			};

			ai.Callback += new Action<BackgroundWorkerEventArgs>(e2 =>
			{
				// ----------------------------------------------------
				// IMPORTANT: Do not access any GUI controls here.
				// This code is running in the background thread!
				// ----------------------------------------------------

				var sr = e2.StatusReport;
				var max = (int)e2.DoWorkEventArgs.Argument;

				sr.ReportProgress("Processing", 0, max, 0);

				for (int i = 0; i < max; i++)
				{
					sr.Value++;
					sr.ReportProgress();

					/* do something here... */
				}

				sr.ReportProgress("Done"); 
			});


			BackgroundTask.Start(100, ai);
		}
	}
}
