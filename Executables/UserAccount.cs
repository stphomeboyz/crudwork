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
using System.Security;
using crudwork.Utilities;

namespace crudwork.Executables
{
	/// <summary>
	/// Class for storing username, password and domain
	/// for the ExeRunner class.
	/// </summary>
	public class UserAccount
	{
		#region Fields
		private string username;
		private SecureString password;
		private string domain;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="domain"></param>
		public UserAccount(string username, SecureString password, string domain)
		{
			this.Username = username;
			this.Password = password;
			this.Domain = domain;
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="domain"></param>
		public UserAccount(string username, string password, string domain)
			: this(username, StringUtil.StringToSecureString(password), domain)
		{
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public UserAccount(string username, SecureString password)
			: this(username, password, string.Empty)
		{
		}

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public UserAccount(string username, string password)
			: this(username, StringUtil.StringToSecureString(password), string.Empty)
		{
		}

		/// <summary>
		/// Create an empty object
		/// </summary>
		public UserAccount()
			: this(string.Empty, string.Empty, string.Empty)
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the username
		/// </summary>
		[Description("Get the username"), Category("ExeUser")]
		public string Username
		{
			get
			{
				return username;
			}
			private set
			{
				this.username = value;
			}
		}

		/// <summary>
		/// Get the password
		/// </summary>
		[Description("Get the password"), Category("ExeUser")]
		public SecureString Password
		{
			get
			{
				return this.password;
			}
			private set
			{
				this.password = value;
			}
		}

		/// <summary>
		/// Get the domain
		/// </summary>
		[Description("Get the domain"), Category("ExeUser")]
		public string Domain
		{
			get
			{
				return domain;
			}
			private set
			{
				this.domain = value;
			}
		}

		/// <summary>
		/// Get the fullname (i.e: name\domain)
		/// </summary>
		[Description("Get the fullname"), Category("ExeUser")]
		public string Fullname
		{
			get
			{
				return String.Format("{1}\\{0}", this.username, this.domain);
			}
		}
		#endregion
	}
}
