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

namespace crudwork.DataAccess
{
	/// <summary>
	/// Data column identity manager
	/// </summary>
	public class TableColumn : IComparable
	{
		private string tableName;
		private string columnName;
		private bool useQuote;
		private string quoteBeginIdentifier;
		private string quoteEndIdentifier;

		#region Constructors
		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="quoteTokens"></param>
		public TableColumn(string fullName, params string[] quoteTokens)
			: this(fullName, true, "[", "]")
		{
		}

		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="useQuote"></param>
		/// <param name="quoteTokens"></param>
		public TableColumn(string fullName, bool useQuote, params string[] quoteTokens)
		{
			SetQuote(quoteTokens);
			this.useQuote = useQuote;

			if (string.IsNullOrEmpty(fullName))
				return;

			string[] tokens = Parse(fullName);
			this.TableName = tokens[0];
			this.ColumnName = tokens[1];
		}

		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		public TableColumn(string tableName, string columnName)
			: this(tableName, columnName, true, "[", "]")
		{
		}

		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		/// <param name="useQuote"></param>
		/// <param name="quoteTokens"></param>
		public TableColumn(string tableName, string columnName, bool useQuote, params string[] quoteTokens)
		{
			SetQuote(quoteTokens);
			this.useQuote = useQuote;
			this.TableName = tableName;
			this.ColumnName = columnName;
		}
		#endregion

		private void SetQuote(string[] quoteTokens)
		{
			if (quoteTokens == null || quoteTokens.Length == 0)
				return;

			if (quoteTokens.Length == 1)
			{
				quoteBeginIdentifier = quoteEndIdentifier = quoteTokens[0];
			}
			else if (quoteTokens.Length == 2)
			{
				quoteBeginIdentifier = quoteTokens[0];
				quoteEndIdentifier = quoteTokens[1];
			}
			else
			{
				throw new ArgumentOutOfRangeException("expected 2 values as quoteTokens; but, found " + quoteTokens.Length);
			}
		}

		#region Public Properties
		/// <summary>
		/// Get the column full name
		/// </summary>
		public string FullName
		{
			get
			{
				return string.Format("{0}.{1}",
					TableName, ColumnName);
			}
		}

		/// <summary>
		/// Get the table name
		/// </summary>
		public string TableName
		{
			get
			{
				//if (useQuote)
				//    return this.quoteBeginIdentifier + this.tableName + this.quoteEndIdentifier;
				//else
				return this.tableName;
			}
			private set
			{
				this.tableName = value;
			}
		}

		/// <summary>
		/// Get the column name
		/// </summary>
		public string ColumnName
		{
			get
			{
				//if (useQuote)
				//    return this.quoteBeginIdentifier + this.columnName + this.quoteEndIdentifier;
				//else
				return this.columnName;
			}
			private set
			{
				this.columnName = value;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Parse a string
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public string[] Parse(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullException("fullName");
				//return new string[] { string.Empty, string.Empty };
			}

			string[] tokens = fullName.Split('.');
			if (tokens.Length != 2)
				throw new ArgumentOutOfRangeException("expected 2 tokens for '" + fullName + "'; but found " + tokens.Length);

			for (int i = 0; i < tokens.Length; i++)
			{
				string s = tokens[i];

				if (s.StartsWith(quoteBeginIdentifier))
					s = s.Substring(2);

				if (s.EndsWith(quoteEndIdentifier))
					s = s.Substring(1, s.Length - 1);

				tokens[i] = s;
			}

			return tokens;
		}

		/// <summary>
		/// Return a string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0}.{1}", this.TableName, this.ColumnName);
		}

		/// <summary>
		/// Convert to uppercase
		/// </summary>
		public void ToUpper()
		{
			this.tableName = this.tableName.ToUpper();
			this.columnName = this.columnName.ToUpper();
		}
		#endregion

		#region Operator overloaders
		/// <summary>
		/// Compare equality
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return this == obj as TableColumn;
		}

		/// <summary>
		/// Get the hash code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Perform comparison between two TableColumn object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator ==(TableColumn l, TableColumn r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return string.Compare(l.FullName, r.FullName) == 0;
		}

		/// <summary>
		/// Perform comparison between two TableColumn object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator !=(TableColumn l, TableColumn r)
		{
			return !(l == r);
		}

		/// <summary>
		/// Perform comparison between two TableColumn object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator <(TableColumn l, TableColumn r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return string.Compare(l.FullName, r.FullName) < 0;
		}

		/// <summary>
		/// Perform comparison between two TableColumn object
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator >(TableColumn l, TableColumn r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return string.Compare(l.FullName, r.FullName) > 0;
		}
		#endregion

		#region IComparable Members

		int IComparable.CompareTo(object obj)
		{
			TableColumn tc = obj as TableColumn;
			if (this == tc)
				return 0;
			else if (this > tc)
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
