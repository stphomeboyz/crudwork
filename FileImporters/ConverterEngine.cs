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
using crudwork.Utilities;

namespace crudwork.FileImporters
{
	/// <summary>
	/// Converter Information
	/// </summary>
	public class ConverterEngine
	{
		#region Fields / Properties
		/// <summary>
		/// Get the assembly type
		/// </summary>
		public Type Type
		{
			get;
			private set;
		}

		/// <summary>
		/// Get the ImportOptions type
		/// </summary>
		public Type ImportOptionsType
		{
			get;
			private set;
		}

		/// <summary>
		/// Get a list of extensions associated to this type
		/// </summary>
		public string[] Extensions
		{
			get;
			private set;
		}

		/// <summary>
		/// The file description
		/// </summary>
		public string Description
		{
			get;
			private set;
		}
		#endregion

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="description"></param>
		/// <param name="type"></param>
		/// <param name="importOptionsType"></param>
		/// <param name="extensions"></param>
		public ConverterEngine(string description, Type type, Type importOptionsType, params string[] extensions)
		{
			this.Description = description;
			this.Type = type;
			this.ImportOptionsType = importOptionsType;

			if (extensions == null || extensions.Length == 0)
				throw new ArgumentNullException("extensions");

			this.Extensions = new string[extensions.Length];
			for (int i = 0; i < extensions.Length; i++)
			{
				this.Extensions[i] = extensions[i].ToUpper();
			}
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Description={0} Type={1} Extensions={2}",
				Description, Type, Extensions);
		}

		/// <summary>
		/// Return a Filter expression for OpenFileDialog() / SaveFileDialog()
		/// </summary>
		/// <returns></returns>
		public string ToFilter()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Extensions.Length; i++)
			{
				if (i > 0)
					sb.Append("|");
				sb.AppendFormat("{0} (*.{1})|*.{1}", Description, Extensions[i].ToLower());
			}
			return sb.ToString();
		}
	}

	/// <summary>
	/// List of Converter Information
	/// </summary>
	public class ConverterEngineList : List<ConverterEngine>
	{
		/// <summary>
		/// Add a converter to list
		/// </summary>
		/// <param name="description"></param>
		/// <param name="type"></param>
		/// <param name="importOptionsType"></param>
		/// <param name="extensions"></param>
		public void Add(string description, Type type, Type importOptionsType, params string[] extensions)
		{
			this.Add(new ConverterEngine(description, type, importOptionsType, extensions));
		}

		/// <summary>
		/// remove item from the collection, if exists
		/// </summary>
		/// <param name="type"></param>
		public void Remove(Type type)
		{
			foreach (var item in this)
			{
				if (item.Type != type)
					continue;

				this.Remove(item);
				break;
			}
		}

		/// <summary>
		/// Return type for extension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public Type GetType(string extension)
		{
			extension = extension.ToUpper();

			foreach (var item in this)
			{
				if (item.Extensions.Contains<string>(extension))
					return item.Type;
			}
			return null;
		}

		/// <summary>
		/// create a filter string array
		/// </summary>
		/// <returns></returns>
		public string[] CreateFilter()
		{
			List<string> result = new List<string>();
			StringBuilder allExtensions = new StringBuilder("All Supported File Types|");
			int c = 0;

			foreach (var item in this)
			{
				foreach (string ext in item.Extensions)
				{
					if (c++ > 0)
						allExtensions.Append("; ");
					allExtensions.Append("*." + ext);
					string entry = string.Format("{0} (*.{1})|*.{1}", item.Description, item.Extensions);
					result.Add(entry);
				}
			}
			result.Insert(0, allExtensions.ToString());

			return result.ToArray();
		}

		/// <summary>
		/// Return a Filter expression for OpenFileDialog() / SaveFileDialog()
		/// </summary>
		/// <returns></returns>
		public string ToFilter()
		{
			StringBuilder sb = new StringBuilder();

			#region Create an entry for "All Supported Files"
			sb.Append("All Supported Files|");
			for (int i = 0; i < this.Count; i++)
			{
				if (i > 0)
					sb.Append(";");

				string[] extensions = this[i].Extensions;

				for (int j = 0; j < extensions.Length; j++)
				{
					if (j > 0)
						sb.Append(";");
					sb.Append("*." + extensions[j].ToLower());
				}
			}
			sb.Append("|");
			#endregion

			#region Create an entry for each supported file
			for (int i = 0; i < this.Count; i++)
			{
				if (i > 0)
					sb.Append("|");
				sb.Append(this[i].ToFilter());
			}
			#endregion

			return sb.ToString();
		}

		/// <summary>
		/// Return the ImportOptions type for extension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public Type GetImportOptionsType(string extension)
		{
			extension = extension.ToUpper();

			foreach (var item in this)
			{
				if (item.Extensions.Contains<string>(extension))
					return item.ImportOptionsType;
			}
			return null;
		}
	}
}
