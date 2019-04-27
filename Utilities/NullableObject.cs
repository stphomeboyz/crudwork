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
using System.Data.SqlTypes;

namespace crudwork.Utilities
{
	/// <summary>
	/// create a Nullable object for associating with DataColumns.  This is used
	/// to change the column's MappingType.  See DataUtil.SetColumnMapping() for usage.
	/// </summary>
	public class NullableObject : INullable, IConvertible
	{
		/// <summary>
		/// the data object
		/// </summary>
		public object value;

		/// <summary>
		/// Create a new object with given attributes
		/// </summary>
		/// <param name="value"></param>
		public NullableObject(object value)
		{
			this.value = value;
		}

		/// <summary>
		/// Return a string representation of object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (((INullable)this).IsNull)
				return string.Empty;

			return this.value.ToString();
		}

		#region INullable Members

		bool INullable.IsNull
		{
			get
			{
				return value == null || value == DBNull.Value;
			}
		}

		#endregion

		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return DataConvert.ToBoolean(this.value);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return DataConvert.ToByte(this.value);
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return DataConvert.ToChar(this.value);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return DataConvert.ToDateTime(this.value);
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return DataConvert.ToDecimal(this.value);
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return DataConvert.ToDouble(this.value);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return DataConvert.ToInt16(this.value);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return DataConvert.ToInt32(this.value);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return DataConvert.ToInt64(this.value);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return DataConvert.ToSByte(this.value);
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return DataConvert.ToSingle(this.value);
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return DataConvert.ToString(this.value);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return this.value;
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return DataConvert.ToUInt16(this.value);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return DataConvert.ToUInt32(this.value);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return DataConvert.ToUInt64(this.value);
		}

		#endregion
	}

}
