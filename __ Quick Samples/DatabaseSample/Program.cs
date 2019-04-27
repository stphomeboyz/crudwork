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
using System.Configuration;
using System.Data;

using crudwork.DataAccess;

namespace DatabaseSample
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 * PURPOSE: connect to database
			 * */

			DataFactory dataFactory = CreateDataFactory("testConnectionString");
			DataTable dt = dataFactory.FillTable("select * from Customers");
			Console.WriteLine("Number rows " + dt.Rows.Count);

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static DataFactory CreateDataFactory(string connectionStringName)
		{
			// retrieve the provider and connection string from app.config
			ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[connectionStringName];
			DataFactory df = new DataFactory(Providers.Converter(css.ProviderName), css.ConnectionString);
			return df;
		}
	}
}
