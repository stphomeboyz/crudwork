using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;
using System.Data;
using System.IO;

namespace DumpTabletoText
{
	class Program
	{
		static void Main(string[] args)
		{
			Dump("select * from pdf_out1 order by id", "pdf1.idx");
			Dump("select * from pdf_out2 order by id", "pdf2.idx");
			Dump("select * from pdf_out3 order by id", "pdf3.idx");
		}

		private static void Dump(string query, string file)
		{
			Console.WriteLine("doing " + file);
			var df = new DataFactory(DatabaseProvider.SqlClient, ConnectionStringManager.MakeSQLClient("la1db2-dev-vm.mscorp.com", "testDb", true, null, null));
			df.TestConnection();

			var dt = df.FillTable(query);

			using (var w = new StreamWriter(file, false, Encoding.ASCII))
			{
				foreach (DataRow dr in dt.Rows)
				{
					w.WriteLine(dr["foo"].ToString());
				}

				w.Flush();
				w.Close();
			}
		}
	}
}
