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
	/// Definition for a database service
	/// </summary>
	public class ServiceDefinition
	{
		#region Properties
		/// <summary>The Service name</summary>
		public string ServiceName
		{
			get;
			set;
		}
		/// <summary>The instance name</summary>
		public string InstanceName
		{
			get;
			set;
		}
		/// <summary>Clustered environment</summary>
		public string IsClustered
		{
			get;
			set;
		}
		/// <summary>The version number</summary>
		public string Version
		{
			get;
			set;
		}
		/// <summary>The factory name or company name</summary>
		public string FactoryName
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public ServiceDefinition()
		{
		}
	}

	/// <summary>
	/// List of Service definition
	/// </summary>
	public class ServiceDefinitionList : List<ServiceDefinition>
	{
		/// <summary>
		/// Add a new entry to the list
		/// </summary>
		/// <param name="serviceName"></param>
		/// <param name="instanceName"></param>
		/// <param name="isClustered"></param>
		/// <param name="version"></param>
		/// <param name="factoryName"></param>
		public void Add(string serviceName, string instanceName, string isClustered, string version, string factoryName)
		{
			this.Add(new ServiceDefinition()
			{
				ServiceName = serviceName,
				InstanceName = instanceName,
				IsClustered = isClustered,
				Version = version,
				FactoryName = factoryName,
			});
		}

	}
}
