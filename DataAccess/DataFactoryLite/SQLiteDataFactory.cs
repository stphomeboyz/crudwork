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
using System.Data.SQLite;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Data Factory (lite version) for the SQLite provider
	/// </summary>
	public class SQLiteDataFactory : DataFactoryLite<SQLiteConnection, SQLiteCommand, SQLiteDataAdapter, SQLiteParameter>
	{
		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public SQLiteDataFactory(string connectionString)
			: base(connectionString)
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="password"></param>
		/// <param name="readOnly"></param>
		/// <param name="fileMustExist"></param>
		public SQLiteDataFactory(string filename, string password, bool readOnly, bool fileMustExist)
			: base(string.Empty /* bogus value... */)
		{
			base.connectionString = ConnectionStringManager.MakeSQLite(filename, password, readOnly, fileMustExist);
		}
	}
}
