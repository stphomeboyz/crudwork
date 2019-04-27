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
using System.Collections;
using crudwork.Utilities;

namespace crudwork.FileImporters
{
	/// <summary>
	/// key value pair
	/// </summary>
	public class ConverterOption
	{
		/// <summary>
		/// the key
		/// </summary>
		public string Key
		{
			get;
			set;
		}

		/// <summary>
		/// the value
		/// </summary>
		public object Value
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ConverterOption()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public ConverterOption(string key, object value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Key={0} Value={1}", Key, Value);
		}
	}

	/// <summary>
	/// list of key value pairs
	/// </summary>
	[Serializable]
	public class ConverterOptionList : List<ConverterOption>
	{
		#region Constructors
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ConverterOptionList()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="hashTable"></param>
		public ConverterOptionList(List<DictionaryEntry> hashTable)
		{
			if (hashTable == null)
				throw new ArgumentNullException("hashTable");

			foreach (var item in hashTable)
			{
				#region Sanity Checks
				if (DataConvert.IsNull(item.Key))
					throw new ArgumentException(string.Format("Key cannot be null or empty.  Key={0} Value={1}", item.Key, item.Value));
				#endregion

				Add(item.Key.ToString(), item.Value);
			}
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="optionString">a list of semicolon-separated key=value tokens (separated with an equal char).  For example: "foo=x;bar=y;none=z"</param>
		public ConverterOptionList(string optionString)
		{
			if (string.IsNullOrEmpty(optionString))
				return;

			// foo=xxx;bar=yyy;none=zzz
			string[] tokens = StringUtil.SplitTrim(optionString, StringSplitOptions.RemoveEmptyEntries, ";");

			if (tokens == null || tokens.Length == 0)
				return;

			foreach (var item in tokens)
			{
				string[] kv = StringUtil.SplitTrim(item, StringSplitOptions.RemoveEmptyEntries, "=");

				#region Sanity Checks
				if (kv.Length != 2)
					throw new ArgumentException("Invalid entry '" + item + "'.  Expected a KEY=VALUE expression");
				if (DataConvert.IsNull(kv[0]))
					throw new ArgumentException("Invalid entry '" + item + "'.  The Key element cannot be null");
				#endregion

				Add(kv[0], kv[1]);
			}

		}
		#endregion

		/// <summary>
		/// Convert this instance into a key/value hashtable.
		/// </summary>
		/// <returns></returns>
		public List<DictionaryEntry> ToHashTable()
		{
			var results = new List<DictionaryEntry>();
			foreach (var item in this)
			{
				if (item.Value == null)
					continue;
				if (string.IsNullOrEmpty(item.Value.ToString()))
					continue;
				results.Add(new DictionaryEntry(item.Key, item.Value));
			}
			return results;
		}

		/// <summary>
		/// add a new key value pair to the list
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, object value)
		{
			this.Add(new ConverterOption(key, value));
		}

		/// <summary>
		/// The string-based index of the element to get or set
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public object this[string index]
		{
			get
			{
				foreach (ConverterOption kv in this)
				{
					if (kv.Key.Equals(index, StringComparison.InvariantCultureIgnoreCase))
						return kv.Value;
				}

				throw new IndexOutOfRangeException("key not found: " + index);
			}
			set
			{
				bool found = false;

				foreach (ConverterOption kv in this)
				{
					if (kv.Key.Equals(index, StringComparison.InvariantCultureIgnoreCase))
					{
						kv.Value = value;
						found = true;
						break;
					}
				}

				if (!found)
				{
					Add(index, value);
				}
			}
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var item in this)
			{
				if (sb.Length > 0)
					sb.Append(", ");
				sb.AppendFormat("{0}=[{1}]", item.Key, item.Value);
			}

			return sb.ToString();
		}
	}
}
