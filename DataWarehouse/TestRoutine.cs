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
using System.Data;

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// Common routines for testing
	/// </summary>
	public static class TestRoutine
	{

	private static Dictionary<string,string> stateList;

		static TestRoutine()
		{
			stateList = new Dictionary<string,string>();

			#region Populate the StateList
			stateList.Add("ALABAMA", "AL");
			stateList.Add("ALASKA", "AK");
			stateList.Add("AMERICAN SAMOA", "AS");
			stateList.Add("ARIZONA", "AZ");
			stateList.Add("ARKANSAS", "AR");
			stateList.Add("CALIFORNIA", "CA");
			stateList.Add("COLORADO", "CO");
			stateList.Add("CONNECTICUT", "CT");
			stateList.Add("DELAWARE", "DE");
			stateList.Add("DISTRICT OF COLUMBIA", "DC");
			stateList.Add("FEDERATED STATES OF MICRONESIA", "FM");
			stateList.Add("FLORIDA", "FL");
			stateList.Add("GEORGIA", "GA");
			stateList.Add("GUAM", "GU");
			stateList.Add("HAWAII", "HI");
			stateList.Add("IDAHO", "ID");
			stateList.Add("ILLINOIS", "IL");
			stateList.Add("INDIANA", "IN");
			stateList.Add("IOWA", "IA");
			stateList.Add("KANSAS", "KS");
			stateList.Add("KENTUCKY", "KY");
			stateList.Add("LOUISIANA", "LA");
			stateList.Add("MAINE", "ME");
			stateList.Add("MARSHALL ISLANDS", "MH");
			stateList.Add("MARYLAND", "MD");
			stateList.Add("MASSACHUSETTS", "MA");
			stateList.Add("MICHIGAN", "MI");
			stateList.Add("MINNESOTA", "MN");
			stateList.Add("MISSISSIPPI", "MS");
			stateList.Add("MISSOURI", "MO");
			stateList.Add("MONTANA", "MT");
			stateList.Add("NEBRASKA", "NE");
			stateList.Add("NEVADA", "NV");
			stateList.Add("NEW HAMPSHIRE", "NH");
			stateList.Add("NEW JERSEY", "NJ");
			stateList.Add("NEW MEXICO", "NM");
			stateList.Add("NEW YORK", "NY");
			stateList.Add("NORTH CAROLINA", "NC");
			stateList.Add("NORTH DAKOTA", "ND");
			stateList.Add("NORTHERN MARIANA ISLANDS", "MP");
			stateList.Add("OHIO", "OH");
			stateList.Add("OKLAHOMA", "OK");
			stateList.Add("OREGON", "OR");
			stateList.Add("PALAU", "PW");
			stateList.Add("PENNSYLVANIA", "PA");
			stateList.Add("PUERTO RICO", "PR");
			stateList.Add("RHODE ISLAND", "RI");
			stateList.Add("SOUTH CAROLINA", "SC");
			stateList.Add("SOUTH DAKOTA", "SD");
			stateList.Add("TENNESSEE", "TN");
			stateList.Add("TEXAS", "TX");
			stateList.Add("UTAH", "UT");
			stateList.Add("VERMONT", "VT");
			stateList.Add("VIRGIN ISLANDS", "VI");
			stateList.Add("VIRGINIA", "VA");
			stateList.Add("WASHINGTON", "WA");
			stateList.Add("WEST VIRGINIA", "WV");
			stateList.Add("WISCONSIN", "WI");
			stateList.Add("WYOMING", "WY");
			#endregion
		}

		/// <summary>
		/// Test an object for null-ness.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(object value)
		{
			if ((value == null) || (value == DBNull.Value) || (value.ToString().Trim().Length == 0))
				return false;
			else
				return true;
		}

		/// <summary>
		/// Verify CSZ - all input fields supplied.  Return true/false truth table.
		/// </summary>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="zip"></param>
		/// <returns></returns>
		public static bool VerifyCSZ(string city, string state, string zip)
		{
			DataTable dt = AMSLookup.LookupCSZ(city, state, zip);
			return dt.Rows.Count > 0;
		}

		/// <summary>
		/// Return true if the given state is valid; otherwise, return false.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static bool IsValidState(string state)
		{
			if (string.IsNullOrEmpty(state))
				return false;

			foreach(KeyValuePair<string,string> kv in stateList)
			{
				if (state.Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase) || state.Equals(kv.Value, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Return the abbreviate state for the given state.  Throw exception if invalid or unspecified.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static string GetStateAbbreviation(string state)
		{
			if (string.IsNullOrEmpty(state))
				throw new ArgumentNullException("state");

			foreach(KeyValuePair<string,string> kv in stateList)
			{
				if (state.Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase))
					return kv.Value;
			}

			throw new ArgumentException("invalid state: " + state);
		}
	}
}
