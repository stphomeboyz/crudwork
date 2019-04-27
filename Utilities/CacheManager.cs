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
	/// Provide a memory caching mechanism to save and retrieve results based on the associated key names
	/// </summary>
	/// <typeparam name="T">the key</typeparam>
	/// <typeparam name="U">the result</typeparam>
	public class CacheManager<T, U>
	{
		private Dictionary<T, U> cache;

		/// <summary>
		/// Create an empty instance
		/// </summary>
		public CacheManager()
		{
			cache = new Dictionary<T, U>();
		}

		/// <summary>
		/// Generate a key based on the input provided.
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public string MakeKey(params string[] keys)
		{
			if (keys == null || keys.Length == 0)
				throw new ArgumentNullException("keys");

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < keys.Length; i++)
			{
				if (i > 0)
					sb.Append("ƒ");	// delimiter by ASCII 159

				sb.Append(keys[i].ToUpper().Trim(' ', '\t'));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Check whether or not the result exists in the cache by the associated key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Exists(T key)
		{
			return cache.ContainsKey(key);
		}

		/// <summary>
		/// Retrieve the result from cache by the associated key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public U Get(T key)
		{
			return cache[key];
		}

		/// <summary>
		/// Add the result/key pair to the cache
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(T key, U value)
		{
			if (cache.ContainsKey(key))
				cache[key] = value;
			else
				cache.Add(key, value);
		}

		/// <summary>
		/// Remove the result from cache by the associated key
		/// </summary>
		/// <param name="key"></param>
		public void Delete(T key)
		{
			cache.Remove(key);
		}

		/// <summary>
		/// Clear all cache from memory
		/// </summary>
		public void Clear()
		{
			cache.Clear();
		}

		/// <summary>
		/// Load cache entries from given file
		/// </summary>
		/// <param name="filename"></param>
		public void Load(string filename)
		{
			throw new NotImplementedException("not done");
		}

		/// <summary>
		/// Save all cache entries to given file
		/// </summary>
		/// <param name="filename"></param>
		public void Save(string filename)
		{
			throw new NotImplementedException("not done");
		}
	}
}
