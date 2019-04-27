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
	/// Field Mapper is used to dynamically invoke a property set and assign a value from a data source.
	/// </summary>
	public class FieldMapper
	{
		public string PropertyName
		{
			get;
			set;
		}
		public string ColumnName
		{
			get;
			set;
		}
		public bool IsRequired
		{
			get;
			set;
		}


		public FieldMapper()
		{
		}

		public override string ToString()
		{
			return string.Format("PropertyName={0} ColumnName={1} IsRequired={2}", PropertyName, ColumnName, IsRequired);
		}
	}
	public class FieldMapperList : List<FieldMapper>
	{
		public void Add(string propertyName, string columnName)
		{
			Add(propertyName, columnName, false);
		}

		public void Add(string propertyName, string columnName, bool isRequired)
		{
			this.Add(new FieldMapper()
			{
				PropertyName = propertyName,
				ColumnName = columnName,
				IsRequired = isRequired,
			});
		}
	}
}
