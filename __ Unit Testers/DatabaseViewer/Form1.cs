using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Controls;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace DatabaseViewer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			dbo.SetStatusReportControls(this, toolStripSplitButton1, toolStripProgressBar1, toolStripStatusLabel1, null);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var result = new DataConnectionInfo();
			result.ConnectionString = "data source=(local); integrated security=true; initial catalog=ReAlignWorkDB";
			result.Provider = DatabaseProvider.SqlClient;
			OpenDatabase(result);
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			var result = ControlManager.ShowConnectionStringBuilder(string.Empty, DatabaseProvider.Unspecified, true);
			OpenDatabase(result);
		}

		private void OpenDatabase(DataConnectionInfo result)
		{
			dbo.ConnectionString = result.ConnectionString;
			dbo.Provider = result.Provider;
			dbo.RefreshTableList();		
		}
	}
}
