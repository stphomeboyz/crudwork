// StpLibrary - by Steve T. Pham
// http://www.stpLibrary.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StpLibrary.DataAccess;
using System.Data;
using System.Data.SQLite;

namespace SQLiteTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//SQLiteConnection conn = new SQLiteConnection();
			DataFactory df = new DataFactory(DatabaseProvider.SQLite, "Data Source=foo.db3; Pooling=true; Read Only=false; FailIfMissing=false");

			DataSet ds = new DataSet();
			ds.ReadXml(@"c:\test.xml");

			df.CreateTable("z", ds.Tables[0]);
			df.AppendTable("z", ds.Tables[0]);

			//df.ExecuteNonQuery("create table foobar(id int, name varchar(100), phone varchar(10))");


			//for (int i = 0; i < 10; i++)
			//{
			//    string q = string.Format("insert into foobar(id, name, phone) values ({0}, '{1}', '{2}')", i, "Name" + i, "Phone" + i);
			//    df.ExecuteNonQuery(q);
			//}

			//DataSet ds = new DataSet();
			//ds = df.Fill("select * from foobar");
		}
	}
}
