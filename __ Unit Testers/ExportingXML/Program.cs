using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ExportingXML
{
	class Program
	{
		static void Main(string[] args)
		{
			DataTable dt = new DataTable("Foobar");
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Name", typeof(string));


			for (int i = 0; i < 10; i++)
			{
				var dr = dt.NewRow();
				dr["ID"] = i;
				dr["Name"] = "Name " + i;
				dt.Rows.Add(dr);
			}

			ExportTable(dt, "Test1.xml");

			DataTable dt2 = new DataTable("Mungbar");
			dt2.Columns.Add("ID", typeof(int));
			dt2.Columns.Add("Phone", typeof(string));

			for (int i = 0; i < 10; i++)
			{
				var dr = dt2.NewRow();
				dr["ID"] = i;
				dr["Phone"] = "123-4567";
				dt2.Rows.Add(dr);
			}

			DataSet ds = new DataSet("MyBook");
			ds.Tables.Add(dt);
			ds.Tables.Add(dt2);

			var relation = new DataRelation("MyID", dt.Columns["ID"], dt2.Columns["ID"]);
			relation.Nested = true;

			ds.Relations.Add(relation);

			ExportDataSet(ds, "Test2.xml");
		}

		private static void ExportDataSet(DataSet ds, string filename)
		{
			ds.WriteXml(filename, XmlWriteMode.IgnoreSchema);
		}

		private static void ExportTable(DataTable dt, string filename)
		{
			dt.WriteXml(filename, false);
		}
	}
}
