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
using System.IO;
using crudwork.Utilities;
using crudwork.Parsers;

namespace crudwork.DataAccess
{
	/// <summary>
	/// ConnectionStringManager parses a ConnectionString into individual elements: Servicename,
	/// Databasename, UserID, Password, IntegratedSecurity.  Manager also merges elements into 
	/// fully-qualified ConnectionString.
	/// </summary>
	public class ConnectionStringManager
	{
		#region Enumerators
		#endregion

		#region Fields
		private string service;
		private string database;
		private string userid;
		private string password;
		private bool isIntegratedSecurity;

		private Dictionary<string, string> otherPairs = null;
		#endregion

		#region Constructors
		/// <summary>
		/// create an empty manager.  The caller is responsible for specifying the 
		/// </summary>
		public ConnectionStringManager()
		{
			Reset();
		}

		/// <summary>
		/// create a manager with a ConnectionString value.
		/// </summary>
		/// <param name="connectionString"></param>
		public ConnectionStringManager(string connectionString)
		{
			ConnectionString = connectionString;
		}

		/// <summary>
		/// create a manager with the individual elements.
		/// </summary>
		/// <param name="service"></param>
		/// <param name="database"></param>
		/// <param name="userid"></param>
		/// <param name="password"></param>
		/// <param name="integratedSecurity"></param>
		public ConnectionStringManager(string service, string database, string userid, string password, bool integratedSecurity)
		{
			this.service = service;
			this.database = database;
			this.userid = userid;
			this.password = password;
			this.isIntegratedSecurity = integratedSecurity;
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
		/// Reset all elements.
		/// </summary>
		public void Reset()
		{
			service = string.Empty;
			database = string.Empty;
			userid = string.Empty;
			password = string.Empty;
			isIntegratedSecurity = false;
			OtherPairs.Clear();
		}

		/// <summary>
		/// return any other key=value pair(s) that the connection string manager does not understand.
		/// </summary>
		public string OtherPairsFormatted()
		{
			StringBuilder s = new StringBuilder();

			foreach (KeyValuePair<string, string> kv in OtherPairs)
			{
				if (s.Length > 0)
					s.Append("; ");
				s.AppendFormat("{0}=\"{1}\"", kv.Key, kv.Value);
			}

			return s.ToString();
		}

		/// <summary>
		/// Set a key value pair to the OtherPairs list
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		[Obsolete("Consider using the OtherPairs property", true)]
		public void SetOtherPair(string key, string value)
		{
			key = key.ToLower();
			if (OtherPairs.ContainsKey(key))
			{
				OtherPairs[key] = value;
			}
			else
			{
				OtherPairs.Add(key, value);
			}
		}

		/// <summary>
		/// Get a key value from the OtherPairs list
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[Obsolete("Consider using the OtherPairs property", true)]
		public string GetOtherPair(string key)
		{
			key = key.ToLower();
			if (!OtherPairs.ContainsKey(key))
				return string.Empty;
			return OtherPairs[key];
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Static Methods
		/// <summary>
		/// return a connection string for accessing a Microsoft Access database file
		/// </summary>
		/// <param name="filename">the Access database filename</param>
		/// <param name="username">the username (optional)</param>
		/// <param name="password">the password (optional)</param>
		/// <param name="fileMustExist">specify whether the file must exist</param>
		/// <returns></returns>
		public static string MakeAccess(string filename, string username, string password, bool fileMustExist)
		{
			if (string.IsNullOrEmpty(username))
				username = "admin";
			if (fileMustExist && !File.Exists(filename))
				throw new FileNotFoundException("File not found", filename);
			return string.Format("Provider=\"Microsoft.Jet.OLEDB.4.0\"; Data Source=\"{0}\"; user id=\"{1}\"; password=\"{2}\"; Jet OLEDB:Engine Type=5",
				filename, username, password);
		}
		/// <summary>
		/// return a connection string for accessing a DBF file
		/// </summary>
		/// <param name="filename">the DBF filename</param>
		/// <param name="fileMustExist">specify whether the file must exist</param>
		/// <returns></returns>
		public static string MakeDBF(string filename, bool fileMustExist)
		{
			if (fileMustExist && !File.Exists(filename))
				throw new FileNotFoundException("File not found", filename);
			return string.Format("Provider=\"Microsoft.Jet.OLEDB.4.0\"; Data Source=\"{0}\"; user id=\"admin\"; password=\"\"; Extended Properties=dBASE IV",
				Path.GetDirectoryName(filename));
		}
		/// <summary>
		/// return a connection string for accessing a Microsoft Excel spreadsheet file
		/// </summary>
		/// <param name="filename">the Excel filename</param>
		/// <param name="username">the username (optional)</param>
		/// <param name="password">the password (optional)</param>
		/// <param name="useHeader">use the header name on first row</param>
		/// <param name="fileMustExist">specify whether the file must exist</param>
		/// <returns></returns>
		public static string MakeExcel(string filename, string username, string password, bool useHeader, bool fileMustExist)
		{
			if (string.IsNullOrEmpty(username))
				username = "admin";
			if (fileMustExist && !File.Exists(filename))
				throw new FileNotFoundException("File not found", filename);
			return string.Format("Provider=\"Microsoft.Jet.OLEDB.4.0\"; Data Source=\"{0}\"; user id=\"{1}\"; password=\"{2}\"; Extended Properties=\"Excel 8.0; HDR={3}; IMEX=1\";",
				filename, username, password, useHeader ? "Yes" : "No");
		}
		/// <summary>
		/// return a connection string for accessing a Microsoft Excel-2007 spreadsheet file
		/// </summary>
		/// <param name="filename">the Excel filename</param>
		/// <param name="username">the username (optional)</param>
		/// <param name="password">the password (optional)</param>
		/// <param name="useHeader">use the header name on first row</param>
		/// <param name="fileMustExist">specify whether the file must exist</param>
		/// <returns></returns>
		public static string MakeExcel2007(string filename, string username, string password, bool useHeader, bool fileMustExist)
		{
			// http://www.connectionstrings.com/excel-2007
			// Use IMEX=1 to always treat data as text, overriding Excel's column type "General" to guess what type of data is in the column
			if (string.IsNullOrEmpty(username))
				username = "admin";
			if (fileMustExist && !File.Exists(filename))
				throw new FileNotFoundException("File not found", filename);
			return string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"{0}\"; user id=\"{1}\"; password=\"{2}\"; Extended Properties=\"Excel 12.0 Xml; HDR={3}; IMEX=1\";",
				filename, username, password, useHeader ? "Yes" : "No");
		}
		/// <summary>
		/// return a connection string for accessing a Microsoft SQL server.  For use with System.Data.SqlClient class
		/// </summary>
		/// <param name="datasource">the data source name</param>
		/// <param name="databasename">the database name or the initial catalog</param>
		/// <param name="integratedSecurity">if true, use integrated security; otherwise, use the username/password parameters</param>
		/// <param name="username">specify a username (required if integratedSecurity is false)</param>
		/// <param name="password">specify a password (required if integratedSecurity is false)</param>
		/// <returns></returns>
		public static string MakeSQLClient(string datasource, string databasename, bool integratedSecurity, string username, string password)
		{
			return string.Format("data source=\"{0}\"; {2}; initial catalog=\"{1}\"",
				datasource, databasename,
				integratedSecurity ? "integrated security=true" : string.Format("user id=\"{0}\"; password=\"{1}\"", username, password)
				);
		}
		/// <summary>
		/// return a connection string for accessing a Microsoft SQL Express server.  For use with System.Data.SqlClient class
		/// </summary>
		/// <param name="datasource">the data source (specify null for .\SQLExpress)</param>
		/// <param name="filename">attach the database file, use "|DataDirectory|" to translate to the website's app_data folder</param>
		/// <param name="databasename">the database name or the initial catalog</param>
		/// <param name="fileMustExist">specify whether the file must exist</param>
		/// <returns></returns>
		public static string MakeSQLExpressClient(string datasource, string filename, string databasename, bool fileMustExist)
		{
			// http://www.connectionstrings.com/sql-server-2005
			//
			// Server=.\SQLExpress;
			// AttachDbFilename=|DataDirectory|mydbfile.mdf;
			// Database=dbname;
			// Trusted_Connection=Yes;

			if (string.IsNullOrEmpty(datasource))
				datasource = null;

			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("filename");

			if (fileMustExist && !filename.StartsWith("|"))
			{
				if (!File.Exists(filename))
					throw new FileNotFoundException("File not found", filename);
			}

			return string.Format("Server=\"{0}\"; AttachDbFilename=\"{1}\"; Database=\"{2}\"; Trusted_Connection=\"Yes\"",
				datasource ?? ".\\SQLExpress", filename, databasename);
		}
		/// <summary>
		/// return a connection string for accessing an Oracle 8i (or later) server.  For use with System.Data.OracleClient class
		/// </summary>
		/// <param name="datasource">the data source name</param>
		/// <param name="databasename">the database name or the initial catalog</param>
		/// <param name="integratedSecurity">if true, use integrated security; otherwise, use the username/password parameters</param>
		/// <param name="username">specify a username (required if integratedSecurity is false)</param>
		/// <param name="password">specify a password (required if integratedSecurity is false)</param>
		/// <returns></returns>
		public static string MakeOracleClient(string datasource, string databasename, bool integratedSecurity, string username, string password)
		{
			return string.Format("data source=\"{0}\"; {2}; database=\"{1}\"",
				datasource, databasename,
				integratedSecurity ? "integrated security=true" : string.Format("user id=\"{0}\"; password=\"{1}\"", username, password)
				);
		}
		/// <summary>
		/// return a connection string for accessing a SQLite database file.  For use with System.Data.SQLite
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="password"></param>
		/// <param name="readOnly"></param>
		/// <param name="fileMustExist"></param>
		/// <returns></returns>
		public static string MakeSQLite(string filename, string password, bool readOnly, bool fileMustExist)
		{
			return MakeSQLite(filename, password, readOnly, fileMustExist, true);
		}
		/// <summary>
		/// return a connection string for accessing a SQLite database file.  For use with System.Data.SQLite
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="password"></param>
		/// <param name="readOnly"></param>
		/// <param name="fileMustExist"></param>
		/// <param name="compress"></param>
		/// <returns></returns>
		public static string MakeSQLite(string filename, string password, bool readOnly, bool fileMustExist, bool compress)
		{
			if (fileMustExist && !File.Exists(filename))
				throw new FileNotFoundException("File not found", filename);

			// Pooling=False; Max Pool Size=100
			// UseUTF16Encoding=True
			// Legacy Format=True
			// Version=3
			return string.Format("data source=\"{0}\"; password=\"{1}\"; read only={2}; failifmissing={3}; compress={4}",
				filename,
				password,
				readOnly ? "true" : "false",
				fileMustExist ? "true" : "false",
				compress ? "true" : "false");
		}
		/// <summary>
		/// return a connection string for accessing a MySQL database.  For use with System.Data.OleDb
		/// </summary>
		/// <param name="datasource"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string MakeMySQL(string datasource, string username, string password)
		{
			// Provider=MySQLProv;Data Source=mydb;User Id=myUsername;Password=myPassword
			return string.Format("provider=MySqlProv; data source=\"{0}\"; user id=\"{1}\"; password=\"{2}\"",
				datasource, username, password);
		}
		#endregion

		#region Indexer
		/// <summary>
		/// Get or set the KEY=VALUE component of the connection string
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string this[string index]
		{
			get
			{
				index = index.ToLower();
				switch (index)
				{
					case "server":
					case "data source":
						return this.Service;

					case "integrated security":
						return this.IsIntegratedSecurity ? "true" : "false";
		
					case "database":
					case "initial catalog":
						return this.database;

					case "userid":	// this is not a real key!
					case "uid":
					case "user id":
						return this.Userid;
					
					case "pwd":
					case "password":
						return this.Password;

					default:
						if (OtherPairs.ContainsKey(index))
							return OtherPairs[index];
						return "";
				}
			}
			set
			{
				index = index.ToLower();
				switch (index)
				{
					case "server":
					case "data source":
						this.Service = value;
						break;

					case "integrated security":
						this.IsIntegratedSecurity = DataConvert.ToBoolean(value);
						break;

					case "database":
					case "initial catalog":
						this.database = value;
						break;

					case "userid":	// this is not a real key!
					case "uid":
					case "user id":
						this.Userid = value;
						break;

					case "pwd":
					case "password":
						this.Password = value;
						break;

					default:
						if (OtherPairs.ContainsKey(index))
							OtherPairs[index] = value;
						else
							OtherPairs.Add(index, value);
						break;

				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the ConnectionString
		/// </summary>
		public string ConnectionString
		{
			get
			{
				if (IsInitialized)
					return string.Empty;

				StringBuilder s = new StringBuilder();

				s.AppendFormat("data source=\"{0}\"", Service);

				if (IsIntegratedSecurity)
				{
					s.Append("; integrated security=true");
				}
				else
				{
					if (!string.IsNullOrEmpty(userid))
						s.AppendFormat("; user id=\"{0}\"", Userid);
					if (!string.IsNullOrEmpty(Password))
						s.AppendFormat("; password=\"{0}\"", Password);
				}

				if (!string.IsNullOrEmpty(database))
					s.AppendFormat("; initial catalog=\"{0}\"", Database);

				if (OtherPairs != null && OtherPairs.Count > 0)
				{
					s.Append("; " + OtherPairsFormatted());
				}

				return s.ToString();

			}
			set
			{
				Reset();

				if (String.IsNullOrEmpty(value))
					return;

				var pairs = ConnectionStringParser.Parse(value);

				foreach (var item in pairs)
				{
					if (string.IsNullOrEmpty(item.Key))
						continue;

					string var = item.Key.ToLower();
					string val = item.Value;

					switch (var)
					{
						case "server":
						case "data source":
							service = val;
							break;

						case "integrated security":
							switch (val.ToLower())
							{
								case "false":
									isIntegratedSecurity = false;
									break;

								case "true":
								case "sspi":
									isIntegratedSecurity = true;
									break;

								default:
									throw new ArgumentException("unknown value: " + val);
							}
							break;

						case "database":
						case "initial catalog":
							database = val;
							break;

						case "uid":
						case "user id":
							userid = val;
							break;

						case "pwd":
						case "password":
							password = val;
							break;

						default:
							OtherPairs.Add(var, val);
							break;
					}
				}
			}
		}

		/// <summary>
		/// Get a value whether all elements are specified.
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				// DBacon algorithm: condition && !switch
				return string.IsNullOrEmpty(service) &&
					string.IsNullOrEmpty(database) &&
					((string.IsNullOrEmpty(userid) && string.IsNullOrEmpty(password)) && !isIntegratedSecurity);
			}
		}

		/// <summary>
		/// Get or set the Service element.
		/// </summary>
		public string Service
		{
			get
			{
				return service;
			}
			set
			{
				service = StringUtil.Unquote(value);
			}
		}

		/// <summary>
		/// Get or set the Database element.
		/// </summary>
		public string Database
		{
			get
			{
				return database;
			}
			set
			{
				database = StringUtil.Unquote(value);
			}
		}

		/// <summary>
		/// Get or set the UserID element.
		/// </summary>
		public string Userid
		{
			get
			{
				return userid;
			}
			set
			{
				userid = StringUtil.Unquote(value);
			}
		}

		/// <summary>
		/// Get or set the Password element.
		/// </summary>
		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = StringUtil.Unquote(value);
			}
		}

		/// <summary>
		/// Get or set the IntegratedSecurity element.
		/// </summary>
		public bool IsIntegratedSecurity
		{
			get
			{
				return isIntegratedSecurity;
			}
			set
			{
				isIntegratedSecurity = value;
			}
		}

		/// <summary>
		/// Get the list of OtherPairs items.
		/// </summary>
		public Dictionary<string, string> OtherPairs
		{
			get
			{
				if (this.otherPairs == null)
					this.otherPairs = new Dictionary<string, string>();
				return this.otherPairs;
			}
			private set
			{
				this.otherPairs = value;
			}
		}
		#endregion

		#region Others
		#endregion
	}
}
