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
using System.Data.Common;

namespace crudwork.DataAccess.SqlClient
{
	/// <summary>
	/// SQL ScriptCommand
	/// </summary>
	internal class ScriptCommand
	{
		private string query;
		private string command;
		private string remark;

		/// <summary>
		/// Get or set the query string
		/// </summary>
		public string Query
		{
			get
			{
				return this.query;
			}
			private set
			{
				this.query = value;
			}
		}

		/// <summary>
		/// Get or set the command string
		/// </summary>
		public string Command
		{
			get
			{
				return this.command;
			}
			private set
			{
				this.command = value;
			}
		}

		/// <summary>
		/// Get or set the remark string
		/// </summary>
		public string Remark
		{
			get
			{
				return this.remark;
			}
			private set
			{
				this.remark = value;
			}
		}

		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="value"></param>
		public ScriptCommand(string value)
		{
			Query = value;
			Command = string.Empty;
			Remark = string.Empty;
		}

		/// <summary>
		/// return string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Query=[{0}] Command=[{1}] Remark=[{2}]",
				Query, Command, Remark);
		}

		/// <summary>
		/// return hash code for this string.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			//return base.GetHashCode();
			return ToString().GetHashCode();
		}
	}
}
