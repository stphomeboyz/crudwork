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

using crudwork.DataSetTools;

namespace CreateDataSetUsingQueryAnything
{
	class Program
	{
		static void Main(string[] args)
		{
			DataSet ds = new DataSet("phonebook");
			QueryManager qm = new QueryManager(ds);
			QueryResult qr = null;

			qr = qm.Run("create table foobar (name varchar(50), telephone varchar(50))");

			qm.Run("insert into foobar (name, telephone) values ('steve1', '123-4561')");
			qm.Run("insert into foobar (name, telephone) values ('steve2', '123-4562')");
			qm.Run("insert into foobar (name, telephone) values ('steve3', '123-4563')");
			qm.Run("insert into foobar (name, telephone) values ('steve4', '123-4564')");
			qm.Run("insert into foobar (name, telephone) values ('steve5', '123-4565')");
			qm.Run("insert into foobar (name, telephone) values ('steve6', '123-4566')");
			qm.Run("insert into foobar (name, telephone) values ('steve7', '123-4567')");
			qm.Run("insert into foobar (name, telephone) values ('steve8', '123-4568')");
			qm.Run("insert into foobar (name, telephone) values ('steve9', '123-4569')");
			qm.Run("insert into foobar (name, telephone) values ('steve0', '123-4560')");

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}
	}
}
