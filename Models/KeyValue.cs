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

namespace crudwork.Models
{
	/// <summary>
	/// KeyValue - where Key is a string and Value is an object
	/// </summary>
	public class KeyValue<K,V>
	{
		#region Properties
		/// <summary>
		/// Get or set the key string value
		/// </summary>
		public K Key
		{
			get;
			set;
		}
		/// <summary>
		/// Get or set the value
		/// </summary>
		public V Value
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with default attributes
		/// </summary>
		public KeyValue()
		{
		}
		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public KeyValue(K key, V value)
		{
			this.Key = key;
			this.Value = value;
		}
		#endregion
	}

	/// <summary>
	/// List of KeyValue - where Key is a string and Value is an object
	/// </summary>
	public class KeyValueList<K,V> : List<KeyValue<K,V>>
	{
		/// <summary>
		/// Add item to list
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(K key, V value)
		{
			this.Add(new KeyValue<K,V>(key, value));
		}

		/// <summary>
		/// string-based index of the element to get or set
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public V this[K key]
		{
			get
			{
				if (key == null)
					throw new ArgumentNullException("key");

				var result = Find(key);
				if (result == null)
					throw new ArgumentException("key not found: " + key);

				return result.Value;
			}
			set
			{
				var result = Find(key);

				if (result != null)
					result.Value = value;
				else
					this.Add(key, value);
			}
		}

		/// <summary>
		/// Return item from list; otherwise, return null 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private KeyValue<K,V> Find(K key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			string keyStr = key.ToString();
			string tmpStr = null;

			foreach (var item in this)
			{
				tmpStr = item.Key == null ? string.Empty : item.Key.ToString();
				if (keyStr.Equals(tmpStr, StringComparison.InvariantCultureIgnoreCase))
					return item;
			}
	
			return null;
		}
	}
}
