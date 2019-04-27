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
using System.Data;

using crudwork.DataAccess;
using crudwork.Controls;
using crudwork.Models.DataAccess;

namespace DataSetSample
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			DataSet ds = PopulateDataSet();
			ControlManager.ShowDataSetViewer(ds);
			//Common.ShowQueryAnalyzer(ds);
		}

		private static DataSet PopulateDataSet()
		{
			DataSet ds = new DataSet("MyNorthwind");

			string[] tablenames = new string[] {
				"Customers",
				"Order Details",
				"Orders"
			};

			DataFactory df = new DataFactory(DatabaseProvider.SqlClient, "data source=(local); integrated security=true; initial catalog=Northwind");

			for (int i = 0; i < tablenames.Length; i++)
			{
				string tbl = tablenames[i];
				DataTable dt = df.FillTable("select * from [" + tbl + "]");
				dt.TableName = tbl;
				ds.Tables.Add(dt);
			}

			return ds;
		}
	}
}
