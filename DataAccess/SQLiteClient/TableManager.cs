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
using System.ComponentModel;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace crudwork.DataAccess.SQLiteClient
{
	/// <summary>
	/// TableManager parses a fully-qualified tablename and return individual elements: Database, 
	/// Servicename, Owner, Tablename.
	/// </summary>
	internal class TableManager : ITableManager
	{
		#region Enumerators
		#endregion

		#region Fields
		private bool useQuote;
		private string tablename;

		private string[] quoteChar = new string[] { "\"", "\"" };
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance of object, with a tablename and a flag indication to use quote.
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="quoteEntry"></param>
		public TableManager(string tablename, bool quoteEntry)
		{
			Clear();
			this.useQuote = quoteEntry;
			Parse(tablename);
		}
		#endregion

		#region Event methods

		#region System Event methods
		#endregion

		#region Application Event methods
		#endregion

		#region Custom Event methods
		#endregion

		#endregion

		#region Public methods
		#endregion

		#region Private methods
		/// <summary>
		/// Clear all instance fields, setting all fields to default values.
		/// </summary>
		private void Clear()
		{
			useQuote = false;
			tablename = string.Empty;
		}

		/// <summary>
		/// Parse the tablename and store the service name, database name,
		/// owner, and tablename in its own elements.  Use the properties
		/// (Servicename, Database, Owner, Tablename, or FullTablename) to
		/// get the value.
		/// </summary>
		/// <param name="tablename"></param>
		private void Parse(string tablename)
		{
			// There nothing to parse... the specified tablename in is the tablename itself
			this.tablename = tablename;
		}

		/// <summary>
		/// Quote the keyword.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private string Quote(string value)
		{
			if (String.IsNullOrEmpty(value))
				return string.Empty;

			if (this.useQuote)
			{
				// double-quotes works here too, but Microsoft recommends using square bracklets.
				return quoteChar[0] + value + quoteChar[1];
			}
			else
			{
				return value;
			}
		}

		private void DoSomething()
		{
			// we want to do something to reference the SQLite provider, so that the compiler
			// is aware that we need it.  (Otherwise, the client must install SQLite.NET manually)

			using (SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
			using (SQLiteDataAdapter da = new SQLiteDataAdapter("select * from SQLite_master", conn))
			{
				var dt = new DataTable();
				da.Fill(dt);
				//return dt;
			}

			
		}
		#endregion

		#region Protected methods
		#endregion

		#region Property methods
		/// <summary>
		/// Get or set the fully-qualified table name
		/// </summary>
		[Description("Get or set the fully-qualified table name"), Category("TableConvention")]
		public string FullTablename
		{
			get
			{
				return Tablename;
			}
			set
			{
				Parse(value);
			}
		}

		/// <summary>
		/// Get or set the service name
		/// </summary>
		[Description("Get or set the service name"), Category("TableConvention")]
		public string Servicename
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		/// <summary>
		/// Get or set the database name
		/// </summary>
		[Description("Get or set the database name"), Category("TableConvention")]
		public string Database
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		/// <summary>
		/// Get or set the owner name
		/// </summary>
		[Description("Get or set the owner name"), Category("TableConvention")]
		public string Owner
		{
			get
			{
				return string.Empty;

			}
			set
			{
			}
		}

		/// <summary>
		/// Get or set the table name
		/// </summary>
		[Description("Get or set the table name"), Category("TableConvention")]
		public string Tablename
		{
			get
			{
				return Quote(this.tablename);
			}
			set
			{
				this.tablename = value;
			}
		}

		/// <summary>
		/// Get or set a value indicating output should use quotes.
		/// </summary>
		public bool UseQuote
		{
			get
			{
				return this.useQuote;
			}
			set
			{
				this.useQuote = value;
			}
		}

		#endregion

		#region Others
		#endregion

		#region ITableConvention Members
		/// <summary>
		/// return the Database
		/// </summary>
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
		/// <summary>
		/// return the fully-qualified table name
		/// </summary>
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
		/// <summary>
		/// return the owner
		/// </summary>
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
		/// <summary>
		/// return the service name
		/// </summary>
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
		/// <summary>
		/// return the tablename
		/// </summary>
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
		/// <summary>
		/// return the quote indicator
		/// </summary>
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
