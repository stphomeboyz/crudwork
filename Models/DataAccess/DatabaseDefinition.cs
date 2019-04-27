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

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// Definition for a database
	/// </summary>
	public class DatabaseDefinition
	{
		//name, dbid, sid, mode, status, status2, crdate, reserved, category, cmptlevel, filename, version
		#region Fields/Properties
		/// <summary>Database Name</summary>
		public string Name
		{
			get;
			set;
		}
		/// <summary>Database Id</summary>
		public int DatabaseId
		{
			get;
			set;
		}
		/// <summary>Server ID</summary>
		public byte[] ServerId
		{
			get;
			set;
		}
		/// <summary>Mode</summary>
		public int Mode
		{
			get;
			set;
		}
		/// <summary>Status</summary>
		public int Status
		{
			get;
			set;
		}
		/// <summary>Status 2</summary>
		public int Status2
		{
			get;
			set;
		}
		/// <summary>Creation Date</summary>
		public DateTime CreationDate
		{
			get;
			set;
		}
		/// <summary>Reserved</summary>
		public object Reserved
		{
			get;
			set;
		}
		/// <summary>Category</summary>
		public int Category
		{
			get;
			set;
		}
		/// <summary>Compatibility level</summary>
		public int CompatibilityLevel
		{
			get;
			set;
		}
		/// <summary>Filename</summary>
		public string Filename
		{
			get;
			set;
		}
		/// <summary>Version</summary>
		public int Version
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public DatabaseDefinition()
		{
		}

		/// <summary>
		/// return a string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Name={0} DatabaseId={1} ServerId={2} Mode={3} Status={4} Status2={5} CreationDate={6} Reserved={7} Category={8} CompatibilityLevel={9} Filename={10} Version={11}",
				Name, DatabaseId, ServerId, Mode, Status, Status2, CreationDate, Reserved, Category, CompatibilityLevel, Filename, Version);
		}
	}

	/// <summary>
	/// List of database defintion
	/// </summary>
	public class DatabaseDefinitionList : List<DatabaseDefinition>
	{
		/// <summary>
		/// Add a new entry to list
		/// </summary>
		/// <param name="name"></param>
		/// <param name="databaseId"></param>
		/// <param name="serverId"></param>
		/// <param name="mode"></param>
		/// <param name="status"></param>
		/// <param name="status2"></param>
		/// <param name="creationDate"></param>
		/// <param name="reserved"></param>
		/// <param name="category"></param>
		/// <param name="compatibilityLevel"></param>
		/// <param name="filename"></param>
		/// <param name="version"></param>
		public void Add(string name, int databaseId, byte[] serverId, int mode, int status, int status2,
			DateTime creationDate, object reserved, int category, int compatibilityLevel, string filename, int version)
		{
			this.Add(new DatabaseDefinition()
			{
				Name = name,
				DatabaseId = databaseId,
				ServerId = serverId,
				Mode = mode,
				Status = status,
				Status2 = status2,
				CreationDate = creationDate,
				Reserved = reserved,
				Category = category,
				CompatibilityLevel = compatibilityLevel,
				Filename = filename,
				Version = version,
			});
		}
	}
}
