// crudwork
// Copyright 2004 by Steve T. Pham (http://www.crudwork.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with This program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace ConcurrencyDataReader
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Purpose of this code
			/*
			 * PURPOSE: Process independent tasks concurrency, in a single thread, to achieve better
			 * perceived performance. This technique can efficiently process a large amount of data.  It is
			 * very simple to use, easy to maintain, does not take up resources, and best of all relies heavily
			 * on the standard C# foreach and yield statement and Microsoft .NET IEnumerable<T> generic class.
			 * 
			 * Without concurrency, a simple application could use an unnecessary amount of system memory and
			 * resources, and could take lot of time to complete -- if at all.
			 * 
			 * Imagine the following application that (a) retrieve data from the database, (b) perform some
			 * tasks, and (c) save the results to file or database.
			 * 
			 * ----- sample code -----
			 * private static DataTable dt;
			 * 
			 * // ...
			 * 
			 * static void Main(string[] args)
			 * {
			 *		dt = LoadData();
			 *		DoTask1(dt);
			 *		DoTask2(dt);
			 *		SaveResults(dt);
			 * }
			 * 
			 * // ...
			 * ----- end -----
			 * 
			 * Step A:		Retrieve data from database and create a DataTable.
			 * Step B.1:	Loop through the table, and perform some task here.
			 * Step B.2:	Loop through the table, and perform some more tasks here.
			 * Step C:		Loop through the table, and save the results.
			 * 
			 * This technique described above works fine if the application were to process several hundreds
			 * or several thousands of rows.  But what would happen you feed it several millions or billions?
			 * 
			 * You've guess it!  The system would run out of memory somewhere along the line.  And, if memory is
			 * not the case, the first step alone would take several hours to build the DataTable in memory.
			 * Also, the steps will be executed consecutively, further increasing the total processing time.
			 * 
			 * One would argument, why not merge all the code logic into one main loop?  This will work too, right?
			 * Sure, the only problem is: you have mixed the business layer and the data layer together.  Furthermore,
			 * this method is harder to maintain because it does too much.
			 * 
			 * {
			 *		SqlCommand cmd = new SqlCommand(...);
			 *		SqlDataReader reader = cmd.ExecuteReader();
			 *		
			 *		while (reader.Read())
			 *		{
			 *			// (a) read the data fields...
			 *			
			 *			// (b.1) do some task here...
			 *			
			 *			// (b.2) do some more tasks here...
			 *			
			 *			// (c) save the results.
			 *		}
			 * }
			 * 
			 * Use the concurrency technique to overcome this issue.  Literally all of steps will take turn to
			 * process the row as it is being read from the database.  That's right... the complete processing
			 * cycle is satisfied on a row-by-row basis.  You don't have to wait for hours and hours before
			 * seeing the new results.
			 * 
			 * */
			#endregion

			MainLoop();

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static string connectionString = @"data source=(local); integrated security=true; initial catalog=Northwind";
		private static string query = @"select ContactName as Name, Phone as Telephone, Country from customers";

		/// <summary>
		/// Task 1: Display the phone list on the screen
		/// </summary>
		private static void MainLoop()
		{
			foreach (var item in FilterList())
			{
				Console.WriteLine("MainLoop: processing a row from GetList()");
				Console.WriteLine("---- Name = {0} Telephone = {1}", item.Name, item.Phone);
			}
		}

		/// <summary>
		/// Task 2: Filter the list
		/// 
		/// In order to participate, this method does a "yield return" and return an IEnumerable(T) object
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<PhoneEntry> FilterList()
		{
			foreach (var item in GetList())
			{
				Console.WriteLine("FilterList: Filter the list by USA phone");
				if (item.Country != "USA")
					continue;

				yield return item;
			}

			yield break;
		}

		/// <summary>
		/// Task 3: Retrieve the list from database.
		/// 
		/// In order to participate, this method does a "yield return" and return an IEnumerable(T) object
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<PhoneEntry> GetList()
		{
			/*
			 * Use DataFactory.ExecuteReader to iterate through the results.  It returns a DataRow for
			 * each iteration.
			 * 
			 * The user can use row.Table.Columns collection to iterate through the columns collection.
			 * 
			 * Returning a DataReader places some burdens on the user.  First, the user must know exactly
			 * the data type and the position to retrieve the data.  Secondly, the user must remember to
			 * dispose the DataReader afterward, to close the connection and free up the resources.
			 * 
			 * */

			DataFactory dataFactory = new DataFactory(DatabaseProvider.SqlClient, connectionString);
			foreach (var row in dataFactory.ExecuteReader(query))
			{
				Console.WriteLine("GetList: processing a row from ExecuteReader");
				yield return new PhoneEntry(row["Name"].ToString(), row["Telephone"].ToString(), row["Country"].ToString());
			}
			yield break;
		}
	}

	/// <summary>
	/// Phone entry with Name and Phone fields
	/// </summary>
	class PhoneEntry
	{
		public string Name;
		public string Phone;
		public string Country;

		public PhoneEntry(string name, string phone, string country)
		{
			this.Name = name;
			this.Phone = phone;
			this.Country = country;
		}

		public override string ToString()
		{
			return string.Format("Name={0} Phone={1} Country={2}", Name, Phone, Country);
		}
	}
}


/*
--------------------------------------------------------------------------
RESULTS:
--------------------------------------------------------------------------
GetList: processing a row from ExecuteReader
MainLoop: processing a row from GetList()
---- Name = Maria Anders Telephone = 030-0074321
GetList: processing a row from ExecuteReader
MainLoop: processing a row from GetList()
---- Name = Ana Trujillo Telephone = (5) 555-4729
...
...
...
---- Name = Matti Karttunen Telephone = 90-224 8858
GetList: processing a row from ExecuteReader
MainLoop: processing a row from GetList()
---- Name = Zbyszek Piestrzeniewicz Telephone = (26) 642-7012
Press ENTER to continue...

*/
