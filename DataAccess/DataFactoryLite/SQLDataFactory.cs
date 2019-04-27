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
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Data Factory (lite version) for the Microsoft SQL provider
	/// </summary>
	public class SQLDataFactory : DataFactoryLite<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
	{
		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public SQLDataFactory(string connectionString)
			: base(connectionString)
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="datasource"></param>
		/// <param name="databasename"></param>
		/// <param name="integratedSecurity"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public SQLDataFactory(string datasource, string databasename, bool integratedSecurity, string username, string password)
			: base(string.Empty /* bogus value... */)
		{
			base.connectionString = string.Format("data source=\"{0}\"; {2}; initial catalog=\"{1}\"",
				datasource, databasename,
				integratedSecurity ? "integrated security=true" : string.Format("user id=\"{0}\"; password=\"{1}\"", username, password)
				);
		}

	}
}
