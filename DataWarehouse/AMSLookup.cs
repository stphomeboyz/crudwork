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
using crudwork.DataAccess;
using System.Data;
using System.Data.Common;
using crudwork.Models.DataAccess;

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// Lookup Tool for AMS Data
	/// </summary>
	public static class AMSLookup
	{
		private static DataFactory dataFactory = null;

		static AMSLookup()
		{
			dataFactory = new DataFactory(DatabaseProvider.SqlClient, "data source=(local); integrated security=true; initial catalog=DataScrub");
		}

		/// <summary>
		/// Retrieve an AMS Reference AMSCityState_D for given zip code
		/// </summary>
		/// <param name="zip"></param>
		/// <returns></returns>
		public static DataTable GetCityState(string zip)
		{
			DbParameter[] parameters = new DbParameter[]{
				dataFactory.GetParameterIn("@Zip", DbType.String, 6, zip),
			};
			return dataFactory.FillTable("select city, stateabbr as state, zip from AMSCityState_D where (mailingid = 'Y') and (zip = @Zip) order by city, stateabbr, zip", parameters);
		}

		/// <summary>
		/// Retrieve an AMS Reference AMSCityState_D for given city, state and zip
		/// </summary>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="zip"></param>
		/// <returns></returns>
		public static DataTable LookupCSZ(string city, string state, string zip)
		{
			#region Sanity Checks
			if (TestRoutine.IsNullOrEmpty(city)
				&& TestRoutine.IsNullOrEmpty(state)
				&& TestRoutine.IsNullOrEmpty(zip)
				)
				throw new ArgumentNullException("All parameters cannot be null!");
			#endregion

			DbParameter[] parameters = new DbParameter[]{
				dataFactory.GetParameterIn("@City", DbType.String, 28, city),
				dataFactory.GetParameterIn("@State", DbType.String, 2, state),
				dataFactory.GetParameterIn("@Zip", DbType.String, 5, zip),
			};
			return dataFactory.FillTable("select * from AMSCityState_D where (city = @City or @City is null) and (state = @State or @State is null) and (zip = @Zip or @Zip is null)", parameters);
		}
	}
}
