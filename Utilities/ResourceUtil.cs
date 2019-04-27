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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.IO;
using System.Data;

namespace crudwork.Utilities
{
	/// <summary>
	/// Resource Utility
	/// </summary>
	public class ResourceUtil
	{
		#region Enumerators
		#endregion

		#region Fields
		private ResourceManager resourceManager;
		private CultureInfo cultureInfo;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an object with given attributes
		/// </summary>
		/// <param name="baseName"></param>
		/// <param name="assembly"></param>
		public ResourceUtil(string baseName, Assembly assembly)
		{
			resourceManager = new ResourceManager(baseName, assembly);
			cultureInfo = Thread.CurrentThread.CurrentCulture;
		}

		/// <summary>
		/// Create an object with given attributes
		/// </summary>
		/// <param name="resourceFilename"></param>
		public ResourceUtil(string resourceFilename)
			: this(Assembly.GetEntryAssembly().GetName().Name + "." + Path.GetFileNameWithoutExtension(resourceFilename), Assembly.GetEntryAssembly())
		{
			//if (!File.Exists(resourceFilename))
			//{
			//    throw new FileNotFoundException(resourceFilename);
			//}
		}
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		/// <summary>
		/// Get a list of culture codes
		/// </summary>
		/// <returns></returns>
		public static string[] GetCultureList()
		{
			List<string> results = new List<string>();

			foreach (CultureInfo c in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
			{
				results.Add(c.Name);
			}

			string[] a = results.ToArray();
			Array.Sort(a);
			return a;
		}

		/// <summary>
		/// get the value associated with key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetString(string key)
		{
			try
			{
				return resourceManager.GetString(key, cultureInfo);
			}
			catch (Exception ex)
			{
				Debug.Print(ex.ToString());
				return "<<" + key + ">>";
			}
		}
		#endregion

		#region Public Static Methods
		/// <summary>
		/// Read the resource key (from the calling assembly) and return a Stream
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <returns></returns>
		public static Stream ToStream(string resourceKey)
		{
			return Assembly.GetCallingAssembly().GetManifestResourceStream(resourceKey);
		}

		/// <summary>
		/// Read the resource key (from the calling assembly) and return a DataSet
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <returns></returns>
		public static DataSet ToDataSet(string resourceKey)
		{
			using (var s = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceKey))
			{
				var ds = new DataSet();
				ds.ReadXml(s);
				return ds;
			}
		}

		/// <summary>
		/// Read the resource key (from the calling assembly) and return a String
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <returns></returns>
		public static string ToString(string resourceKey)
		{
			using (var s = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceKey))
			using (var sr = new StreamReader(s))
			{
				return sr.ReadToEnd();
			}
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		#endregion

		#region Others
		#endregion
	}
}
