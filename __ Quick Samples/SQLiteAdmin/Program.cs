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
using System.Windows.Forms;
using crudwork.Controls;
using crudwork.DataAccess;
using crudwork.Utilities;

namespace SQLiteQueryAnalyzer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());

			string connectionString = string.Empty;
			string filename = (args.Length > 0) ? args[0] : FileUtil.CreateTempFile("db", new string[0]);
			connectionString = ConnectionStringManager.MakeSQLite(filename, string.Empty, false, true);
			ControlManager.ShowSQLiteQueryAnalyzer(connectionString);
		}
	}
}
