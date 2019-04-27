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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Defines a database connection, such as a database provider and a connection string.
	/// 
	/// <para>If the InputSource is Database, use the Provider and ConnectionString property to connect to a relational database (via DataFactory)</para>
	/// <para>If the InputSource is File, use the Filename and Options property to import data from a data file (via ImportManager)</para>
	/// </summary>
	public class DataConnectionInfo
	{
		#region Fields
		/// <summary>
		/// Get or set the database provider type
		/// </summary>
		public DatabaseProvider Provider
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the connection string
		/// </summary>
		public string ConnectionString
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the input source type
		/// </summary>
		public InputSource InputSource
		{
			get;
			set;
		}

		/// <summary>
		/// Get other options
		/// </summary>
		private Dictionary<string, string> OtherKeys
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the filename
		/// </summary>
		public string Filename
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the options
		/// </summary>
		public string Options
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public DataConnectionInfo()
		{
			OtherKeys = new Dictionary<string, string>();
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="connectionString"></param>
		public DataConnectionInfo(DatabaseProvider provider, string connectionString)
			: this()
		{
			this.InputSource = InputSource.Database;	// indicates that data is from a relation database

			this.Provider = provider;
			this.ConnectionString = connectionString;

			this.Filename = null;
			this.Options = null;
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="connectionString"></param>
		public DataConnectionInfo(string filename, string options)
			: this()
		{
			this.InputSource = InputSource.File;		// indicates that data is from a file (simple or complex type)

			this.Provider = DatabaseProvider.Unspecified;
			this.ConnectionString = null;
			this.Filename = filename;
			this.Options = options;
		}
		#endregion

		/// <summary>
		/// Gets or sets the value associated with the specified key
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string this[string index]
		{
			get
			{
				if (!OtherKeys.ContainsKey(index))
					throw new KeyNotFoundException();
				return OtherKeys[index];
			}
			set
			{
				if (OtherKeys.ContainsKey(index))
					OtherKeys[index] = value;
				else
					OtherKeys.Add(index, value);
			}
		}

		public void Validate()
		{
			switch (InputSource)
			{
				case InputSource.Database:
					if (Provider == DatabaseProvider.Unspecified)
						throw new ArgumentException("Provider must be specified");
					if (string.IsNullOrEmpty(ConnectionString))
						throw new ArgumentException("ConnectionString must be specified");
					break;

				case InputSource.File:
					if (string.IsNullOrEmpty(Filename))
						throw new ArgumentException("Filename must be specified");
					//if (string.IsNullOrEmpty(Options))
					//    throw new ArgumentException("Options must be specified");
					break;

				default:
					throw new ArgumentOutOfRangeException("InputSource=" + InputSource);
			}
		}

		#region Operator overloaders
		/// <summary>
		/// Perform comparison between two DatabaseConnection object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator ==(DataConnectionInfo l, DataConnectionInfo r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return l.ToString() == r.ToString();
		}

		/// <summary>
		/// Perform comparison between two DatabaseConnection object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator !=(DataConnectionInfo l, DataConnectionInfo r)
		{
			return !(l == r);
		}

		/// <summary>
		/// Return the hash code for this string
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Return a string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (KeyValuePair<string, string> item in OtherKeys)
			{
				if (sb.Length > 0)
					sb.Append("; ");
				sb.AppendFormat("{0}={1}", item.Key, item.Value);
			}

			return string.Format("Provider=[{0}] ConnectionString=[{1}] Options=[{2}]",
				Provider, ConnectionString, sb.ToString());
		}

		/// <summary>
		/// Compare equality
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return this == obj as DataConnectionInfo;
		}
		#endregion
	}
}
