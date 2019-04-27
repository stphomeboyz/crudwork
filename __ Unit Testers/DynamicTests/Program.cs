using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.DataAccess;
using crudwork.DynamicRuntime;
using crudwork.Models.DataAccess;
using System.Diagnostics;

namespace DynamicTests
{
	class Program
	{
		static void Main(string[] args)
		{
			ShouldThrowEx();
			MispelledProperty();

			//ToTableTest();
		}

		private static void MispelledProperty()
		{
			try
			{
				var c = new ClassThatThrow();
				DynamicCode.GetProperty(c, "IDoNotExist");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private static void ShouldThrowEx()
		{
			try
			{
				var c = new ClassThatThrow();
				DynamicCode.GetProperty(c, "IThrowAnException");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private static void ToTableTest()
		{
			var df = new DataFactory(crudwork.Models.DataAccess.DatabaseProvider.SqlClient, "data source=\"sql.vnesoft.com\"; user id=\"Va!dO!#!167892g\"; password=\"H@#d@Tag#6\"; initial catalog=\"ARMLoyaltyUsers\"");
			df.TestConnection();

			var cdl = df.Database.GetColumns("EbmContactAddress", QueryFilter.Exact, null, QueryFilter.None);

			var dt = DynamicCode.ToTable<ColumnDefinition>(cdl);
		}
	}

	internal class ClassThatThrow
	{
		private string IThrowAnException
		{
			get
			{
				throw new ArgumentException("I throw this on purpose -- for testing.");
			}
		}
	}
}
