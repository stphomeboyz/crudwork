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
using System.Text;
using System.Configuration;
using System.IO;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif
using System.Reflection;
using System.Collections.Specialized;
using crudwork.Models;

namespace crudwork.Utilities
{
	/// <summary>
	/// Configuration Manager allows code to save and retrieve settings
	/// </summary>
	public class ConfigManager : DisposableObject
	{
		/// <summary>
		/// Get the current configuration
		/// </summary>
		public Configuration Configuration
		{
			get;
			private set;
		}

		/// <summary>
		/// The type of configuration (assembly) file to be open
		/// </summary>
		public enum AssemblyType
		{
			/// <summary>the calling assembly</summary>
			CallingAssembly,
			/// <summary>the entry assembly</summary>
			EntryAssembly,
			/// <summary>the executing assembly</summary>
			ExecutingAssembly,
		}

		/// <summary>
		/// Create an instance to save and retrieve settings from the executable config file.
		/// </summary>
		public ConfigManager()
		{
			Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		}

		/// <summary>
		/// Create an instance to save and retrieve settings from a named config file.
		/// </summary>
		/// <param name="name"></param>
		public ConfigManager(string name)
		{
			//throw new NotImplementedException("not implemented");
			ExeConfigurationFileMap m = new ExeConfigurationFileMap();
			m.ExeConfigFilename = name;
			Configuration = ConfigurationManager.OpenMappedExeConfiguration(m, ConfigurationUserLevel.None);
		}

		/// <summary>
		/// <para>Create a new instance to save and retrieve settings from the specified assembly type:</para>
		/// <para>AssemblyType.CallingAssembly - the caller assembly</para>
		/// <para>AssemblyType.EntryAssembly - the entry assembly</para>
		/// <para>AssemblyType.ExecutingAssembly - the executing assembly</para>
		/// </summary>
		/// <param name="entry"></param>
		public ConfigManager(AssemblyType entry)
		{
			string exePath;

			switch (entry)
			{
				case AssemblyType.CallingAssembly:
					exePath = Assembly.GetCallingAssembly().Location;
					break;
				case AssemblyType.EntryAssembly:
					exePath = Assembly.GetEntryAssembly().Location;
					break;
				case AssemblyType.ExecutingAssembly:
					exePath = Assembly.GetExecutingAssembly().Location;
					break;
				default:
					throw new ArgumentOutOfRangeException("entry=" + entry);
			}

			Configuration = ConfigurationManager.OpenExeConfiguration(exePath);
		}

		/// <summary>
		/// Get value associated to the given key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Get(string key)
		{
			try
			{
				return Configuration.AppSettings.Settings[key].Value;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "key", key);
				DebuggerTool.AddData(ex, "AllKeys", string.Concat(Configuration.AppSettings.Settings.AllKeys));
				DebuggerTool.AddData(ex, "config.FilePath", Configuration.FilePath);
				throw;
			}
		}

		/// <summary>
		/// Get the value associated with key.  If not found return the given default value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public string Get(string key, string defaultValue)
		{
			try
			{
				return Get(key);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return defaultValue;
			}
		}

		/// <summary>
		/// Set value associated to the given key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(string key, string value)
		{
			try
			{
				string[] allKeys = Configuration.AppSettings.Settings.AllKeys;
				StringUtil.ToUpper(allKeys);
				Array.Sort(allKeys);

				if (Array.BinarySearch<string>(allKeys, key.ToUpper()) < 0)
				{
					// key not found, create new entry
					Configuration.AppSettings.Settings.Add(key, value);
				}
				else
				{
					// key found, update existing entry
					Configuration.AppSettings.Settings[key].Value = value;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "key", key);
				DebuggerTool.AddData(ex, "value", value);
				DebuggerTool.AddData(ex, "config.FilePath", Configuration.FilePath);
				throw;
			}
		}

		/// <summary>
		/// Save the configuration file
		/// </summary>
		public void Save()
		{
			try
			{
				Configuration.Save(ConfigurationSaveMode.Modified);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "config.FilePath", Configuration.FilePath);
				throw;
			}
		}

		/// <summary>
		/// Get the ConnectionString associated to the given key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public ConnectionStringSettings GetConnectionString(string key)
		{
			try
			{
				ConnectionStringSettings results = Configuration.ConnectionStrings.ConnectionStrings[key];
				return results;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "key", key);
				DebuggerTool.AddData(ex, "config.FilePath", Configuration.FilePath);
				throw;
			}
		}

		/// <summary>
		/// Set the ConnectionString/ProviderName associated to the given key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="newcss"></param>
		public void SetConnectionString(string key, ConnectionStringSettings newcss)
		{
			var css = Configuration.ConnectionStrings.ConnectionStrings[key];
			css.ConnectionString = newcss.ConnectionString;
			css.ProviderName = newcss.ProviderName;
			//css.Name = newcss.Name;
		}


		/// <summary>
		/// properly dispose the property
		/// </summary>
		/// <param name="disposeManagedResources"></param>
		protected override void Dispose(bool disposeManagedResources)
		{
			base.Dispose(disposeManagedResources);

			if (disposeManagedResources)
			{
				// save the configuration
				Configuration.Save(ConfigurationSaveMode.Modified);
			}
		}
	}

	/// <summary>
	/// Config Utility
	/// </summary>
	[Obsolete("consider using ConfigManager", true)]
	public static class Config
	{
		/// <summary>
		/// get the value associated with key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Value(string key)
		{
			try
			{
				if (ConfigurationManager.AppSettings == null)
					throw new ArgumentNullException("AppSettings");

				if (String.IsNullOrEmpty(key))
					throw new ArgumentNullException("key");

				string s = String.Empty;

				try
				{
					s = ConfigurationManager.AppSettings[key].ToString();
				}
				catch (Exception ex)
				{
					Debug.Print(ex.ToString());
					// absorb error!
				}

				if (String.IsNullOrEmpty(s))
					throw new ArgumentNullException("key=" + key);

				if (s.Contains("{TEMP_FOLDER}"))
				{
					s = s.Replace("{TEMP_FOLDER}", Path.GetTempPath());
				}

				return s;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "key", key);
				throw;
			}
		}

		/// <summary>
		/// Get the value associated with key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Get(string key)
		{
			return Value(key);
		}

		/// <summary>
		/// Get the value associated with key.  If not found return the given default value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string Get(string key, string defaultValue)
		{
			try
			{
				return Value(key);
			}
			catch (Exception ex)
			{
				Debug.Print(ex.ToString());
				return defaultValue;
			}
		}

		/// <summary>
		/// Save the key and value pair to config file.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Save(string key, string value)
		{
			try
			{
				Assembly assembly = Assembly.GetCallingAssembly();
				string appConfig = assembly.Location;
				Configuration config = ConfigurationManager.OpenExeConfiguration(appConfig);


				string[] names = ConfigurationManager.AppSettings.AllKeys;
				NameValueCollection appSettings = ConfigurationManager.AppSettings;
				for (int i = 0; i < appSettings.Count; i++)
				{
					Console.WriteLine("#{0} Name: {1} Value: {2}",
					  i, names[i], appSettings[i]);
				}


				config.Save();
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "key", key);
				DebuggerTool.AddData(ex, "value", value);
				throw;
			}
		}
	}
}
