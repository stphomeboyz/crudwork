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

namespace crudwork.Utilities
{
	/// <summary>
	/// Enum Utility
	/// </summary>
	public static class EnumUtil
	{
		#region ---- Enum ----
		/// <summary>
		/// Get a array of object from a enum datatype.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object[] EnumValues(Type type)
		{
			List<object> list = new List<object>();

#if !SILVERLIGHT
			Array array = Enum.GetValues(type);
#else
			object[] array = null;
			throw new NotImplementedException();
#endif
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array.GetValue(i));
			}

			return list.ToArray();
		}
		#endregion
	}
}
