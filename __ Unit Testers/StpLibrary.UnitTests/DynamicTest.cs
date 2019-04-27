using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using crudwork.DataAccess;
using crudwork.Parsers;
using crudwork.DynamicRuntime;

namespace crudwork.UnitTests
{
	[TestClass]
	public class DynamicTest
	{
		#region Junks
		public DynamicTest()
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

		private string PropertyThatThrowsEx
		{
			get
			{
				throw new NotImplementedException("I throw this exception on purpose");
			}
		}

		[TestMethod,Description("Build 50 - DynamicCode.Get/Set Property/Method should rethrow ex if an exception was thrown by the invoked method/property")]
		public void InvokeAMemberThatThrowEx()
		{
			// Arrange

			// Act
			try
			{
				object result = DynamicCode.GetProperty(this, "PropertyThatThrowsEx");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is NotImplementedException, "Should throw NotImplementedException on purpose");
			}
			// Assert
		}

		[TestMethod, Description("Build 50 - DynamicCode.Get/Set Property/Method should rethrow ex if an exception was thrown by the invoked method/property")]
		public void InvokeAMemberThatDoesNotExist()
		{
			// Arrange

			// Act
			try
			{
				object result = DynamicCode.GetProperty(this, "APropertyThatDoesNotExist");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is MissingMethodException, "Should throw NotImplementedException on purpose");
			}
			// Assert
		}
	}
}
