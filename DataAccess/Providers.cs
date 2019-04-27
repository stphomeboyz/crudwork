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
using crudwork.Models.DataAccess;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Database Provider
	/// </summary>
	public static class Providers
	{
		#region Public methods
		/// <summary>
		/// Convert DatabaseProvider to .NET Provider assembler name.
		/// </summary>
		/// <param name="dataProviderType"></param>
		/// <returns></returns>
		public static string ToProvider(DatabaseProvider dataProviderType)
		{
			switch (dataProviderType)
			{
				case DatabaseProvider.Odbc:
					return "System.Data.Odbc";
				case DatabaseProvider.OleDb:
					return "System.Data.OleDb";
				case DatabaseProvider.OracleClient:
					return "System.Data.OracleClient";
				case DatabaseProvider.OracleDataProvider:
					return "Oracle.OracleDataProvider";
				case DatabaseProvider.SqlClient:
					return "System.Data.SqlClient";
				case DatabaseProvider.SQLite:
					return "System.Data.SQLite";
				default:
					throw new ArgumentOutOfRangeException("dataProviderType=" + dataProviderType);
			}
		}

		/// <summary>
		/// Convert string to DatabaseProvider.
		/// </summary>
		/// <param name="DataProviderName"></param>
		/// <returns></returns>
		public static DatabaseProvider Converter(string DataProviderName)
		{
			switch (DataProviderName.ToUpper())
			{
				case "ODBC":
				case "SYSTEM.DATA.ODBC":
					return DatabaseProvider.Odbc;
				case "OLEDB":
				case "SYSTEM.DATA.OLEDB":
					return DatabaseProvider.OleDb;
				case "ORACLE":
				case "ORACLECLIENT":
				case "SYSTEM.DATA.ORACLECLIENT":
					return DatabaseProvider.OracleClient;
				case "ODP":
				case "ORACLEDATAPROVIDER":
				case "ORACLE.ORACLEDATAPROVIDER":
					return DatabaseProvider.OracleDataProvider;
				case "SQL":
				case "SQLCLIENT":
				case "SYSTEM.DATA.SQLCLIENT":
					return DatabaseProvider.SqlClient;
				case "SQLITE":
				case "SYSTEM.DATA.SQLITE":
					return DatabaseProvider.SQLite;
				default:
					throw new ArgumentOutOfRangeException("DataProviderName=" + DataProviderName);
			}
		}
		#endregion
	}
}
