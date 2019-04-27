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
#if !SILVERLIGHT
using System.Drawing;
#else
using System.Windows.Controls;
#endif

namespace crudwork.Utilities
{
	/// <summary>
	/// Db Field Utility
	/// </summary>
	[Obsolete("Consider using DataConvert class", true)]
	public static class DbField
	{
		/// <summary>
		/// Convert an object to Int32 (int in C#) type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Int32 ToInt32(object o)
		{
			return Int32.Parse(o.ToString());
		}

		/// <summary>
		/// Convert an object to Int64 (long in C#) type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Int64 ToInt64(object o)
		{
			return Int64.Parse(o.ToString());
		}

		/// <summary>
		/// Convert an object to Decimal (decimal in C#) type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Decimal ToDecimal(object o)
		{
			return Decimal.Parse(o.ToString());
		}

		/// <summary>
		/// Convert an object to DateTime type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o)
		{
			return DateTime.Parse(o.ToString());
		}

		/// <summary>
		/// Convert an object to String type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static String ToString(object o)
		{
			return o.ToString();
		}

		/// <summary>
		/// Convert an object to Image type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Image ToImage(object o)
		{
			return (Image)o;
		}
	}
}
