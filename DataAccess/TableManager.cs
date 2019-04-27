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
using crudwork.Models.DataAccess;

namespace crudwork.DataAccess
{
	/// <summary>
	/// TableManager parses a fully-qualified tablename and return individual elements: Database, 
	/// Servicename, Owner, Tablename.
	/// </summary>
	public class TableManager : ITableManager
	{
		#region Enumerators
		#endregion

		#region Fields
		private DatabaseProvider provider;
		private ITableManager tableConvention = null;
		#endregion

		#region Constructors
		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="fullTablename"></param>
		/// <param name="useQuote"></param>
		public TableManager(DatabaseProvider provider, string fullTablename, bool useQuote)
		{
			this.provider = provider;

			switch (provider)
			{
				case DatabaseProvider.SqlClient:
				case DatabaseProvider.OleDb:
					tableConvention = new SqlClient.TableManager(string.Empty, useQuote);
					break;

				case DatabaseProvider.SQLite:
					tableConvention = new SQLiteClient.TableManager(string.Empty, useQuote);
					break;

				default:
					throw new ArgumentOutOfRangeException("unsupported : " + provider);
			}

			UseQuote = useQuote;
			FullTablename = fullTablename;
		}

		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="useQuote"></param>
		public TableManager(DatabaseProvider provider, bool useQuote)
			: this(provider, string.Empty, useQuote)
		{
		}

		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="fullTablename"></param>
		/// <param name="useQuote"></param>
		public TableManager(string fullTablename, bool useQuote)
			: this(DatabaseProvider.SqlClient, fullTablename, useQuote)
		{
		}
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		/// <summary>
		/// Parse the fully-qualified tablename into elements
		/// </summary>
		/// <param name="fullTablename"></param>
		/// <param name="useQuote"></param>
		/// <returns></returns>
		public TableManager Parse(string fullTablename, bool useQuote)
		{
			TableManager tableManager = new TableManager(this.provider, useQuote);
			tableManager.FullTablename = fullTablename;
			return tableManager;
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		#endregion

		#region Others
		#endregion

		#region ITableConvention Members

		/// <summary>
		/// Get or set the database element
		/// </summary>
		public string Database
		{
			get
			{
				return tableConvention.Database;
			}
			set
			{
				tableConvention.Database = value;
			}
		}

		/// <summary>
		/// Get or set the fully-qualified tablename
		/// </summary>
		public string FullTablename
		{
			get
			{
				return tableConvention.FullTablename;
			}
			set
			{
				tableConvention.FullTablename = value;
			}
		}

		/// <summary>
		/// Get or set the owner element
		/// </summary>
		public string Owner
		{
			get
			{
				return tableConvention.Owner;
			}
			set
			{
				tableConvention.Owner = value;
			}
		}

		/// <summary>
		/// Get or set the servicename element
		/// </summary>
		public string Servicename
		{
			get
			{
				return tableConvention.Servicename;
			}
			set
			{
				tableConvention.Servicename = value;
			}
		}

		/// <summary>
		/// Get or set the tablename element
		/// </summary>
		public string Tablename
		{
			get
			{
				return tableConvention.Tablename;
			}
			set
			{
				tableConvention.Tablename = value;
			}
		}

		/// <summary>
		/// Get or set a value indicating output should use quotes.
		/// </summary>
		public bool UseQuote
		{
			get
			{
				return tableConvention.UseQuote;
			}
			set
			{
				tableConvention.UseQuote = value;
			}
		}
		#endregion

		#region ITableConvention Members

		string ITableManager.Database
		{
			get
			{
				return Database;
			}
			set
			{
				Database = value;
			}
		}

		string ITableManager.FullTablename
		{
			get
			{
				return FullTablename;
			}
			set
			{
				FullTablename = value;
			}
		}

		string ITableManager.Owner
		{
			get
			{
				return Owner;
			}
			set
			{
				Owner = value;
			}
		}

		string ITableManager.Servicename
		{
			get
			{
				return Servicename;
			}
			set
			{
				Servicename = value;
			}
		}

		string ITableManager.Tablename
		{
			get
			{
				return Tablename;
			}
			set
			{
				Tablename = value;
			}
		}

		bool ITableManager.UseQuote
		{
			get
			{
				return UseQuote;
			}
			set
			{
				UseQuote = value;
			}
		}

		#endregion
	}
}
