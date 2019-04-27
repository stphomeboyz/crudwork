using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Models.DataAccess;

namespace QueryDatabaseLinks
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void btnExecute_Click(object sender, EventArgs e)
		{
			/* USAGE:
			 * 
			 *		SET CONNECTION STRING @L1
			 *			DATABASE <ServiceName/DatabaseName>
			 *				[AS <Username/Password>]
			 *				PROVIDER <DatabaseProvider>
			 *		;
			 *
			 *		SET CONNECTION STRING @L2
			 *			FILE <Filename>
			 *		;
			 *		
			 *		SELECT *
			 *			FROM @L1.TableName1 a
			 *				INNER JOIN @L2.TableName2 b on b.key = a.key
			 * 
			 * 
			 * EXAMPLE:
			 * 
			 *		set connection string @db1 database local/northwind provider sql;
			 *		set connection string @db2 file "c:\myaccess.mdb";
			 *		
			 *		select a.*, b.keyfield
			 *			from @db1.table a
			 *				inner join @db2.foo b
			 *					on a.key = b.key;
			 * */
		}

		public LinkedDataTable LinkedTable
		{
			get
			{
				return new LinkedDataTable();
			}
			set
			{
				if (value == null)
					value = new LinkedDataTable();

			}
		}
	}
}
