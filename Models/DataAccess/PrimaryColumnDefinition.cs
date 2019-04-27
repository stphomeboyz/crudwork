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
//using System.Linq;
using System.Text;

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Definition for primary column
	/// </summary>
	public class PrimaryColumnDefinition
	{
		#region Fields / Properties
		/// <summary>The table name</summary>
		public string TableName
		{
			get;
			set;
		}
		/// <summary>The column name</summary>
		public string ColumnName
		{
			get;
			set;
		}
		/// <summary>the contraint name</summary>
		public string ConstraintName
		{
			get;
			set;
		}
		/// <summary>the position</summary>
		public int OrdinalPosition
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// create new instance with default attribute
		/// </summary>
		public PrimaryColumnDefinition()
		{
		}

		public override string ToString()
		{
			return string.Format("TableName={0} ColumnName={1} ConstraintName={2} OrdinalPosition={3}", TableName, ColumnName, ConstraintName, OrdinalPosition);
		}
	}

	/// <summary>
	/// List of Primary Column Definition
	/// </summary>
	public class PrimaryColumnDefinitionList : List<PrimaryColumnDefinition>
	{
	}
}
