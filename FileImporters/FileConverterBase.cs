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
using crudwork.DynamicRuntime;
using System.Reflection;

namespace crudwork.FileImporters
{
	/// <summary>
	/// base for all file converter classes
	/// </summary>
	public abstract class FileConverterBase<T>
		where T : ImportOptions
	{
		/// <summary>
		/// Get or set the options
		/// </summary>
		protected T Options
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public FileConverterBase()
		{
			Options = Activator.CreateInstance<T>();
		}

		/// <summary>
		/// Set the user-defined options (or replace the default settings with the user-defined values)
		/// </summary>
		/// <param name="options"></param>
		public virtual void SetOptions(ConverterOptionList options)
		{
			if (options == null || options.Count == 0)
				return;

			#region Cast the value element into its underlying type
			var pis = new List<PropertyInfo>(this.Options.GetType().GetProperties());

			foreach (ConverterOption item in options)
			{
				var mypis = pis.FindAll(pi => pi.Name == item.Key);
				if (mypis.Count != 1)
					throw new ArgumentException(string.Format("Expected one property to match key of '{0}'; but, found {2}", item.Key, item.Value, mypis.Count));
				Type t = mypis[0].PropertyType.UnderlyingSystemType;
				if (item.Value.GetType() != t)
					item.Value = Convert.ChangeType(item.Value, t);
			}
			#endregion


			DynamicCode.SetProperty(this.Options, options.ToHashTable());
		}
	}
}