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
#if !SILVERLIGHT
using System.Data;
#endif

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// CityStateZip cleanser tool
	/// </summary>
	public class CityStateZip : ICleanser
	{
		/// <summary>
		/// container indicating which data parts are supplied
		/// </summary>
		public enum Container
		{
			/// <summary>Empty</summary>
			Empty = 0,

			/// <summary>Contains city, state, zip</summary>
			CityStateZip = 1,

			/// <summary>Contains city and state</summary>
			CityState = 2,

			/// <summary>Contains zip</summary>
			Zip = 3,

			/// <summary>Contains city</summary>
			City = 4,

			/// <summary>Contains state</summary>
			State = 5,
		}

		private string city;
		private string state;
		private string zip;
		private bool validate;

		/// <summary>
		/// Create an object with given city, state, zip and validate flag.
		/// </summary>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="zip"></param>
		/// <param name="validate"></param>
		public CityStateZip(string city, string state, string zip, bool validate)
		{
			City = city;
			State = state;
			Zip = zip;
			this.validate = validate;
			Validate();
		}

		/// <summary>
		/// return a container indicating which data parts are supplied
		/// </summary>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="zip"></param>
		/// <returns></returns>
		public static Container GetContainer(string city, string state, string zip)
		{
			bool hasCity = !TestRoutine.IsNullOrEmpty(city);
			bool hasState = !TestRoutine.IsNullOrEmpty(state);
			bool hasZip = !TestRoutine.IsNullOrEmpty(zip);

			// test when all fields are specified.
			if (hasCity && hasState && hasZip)
			{
				return Container.CityStateZip;
			}

			if (hasCity && hasState)
			{
				return Container.CityState;
			}

			if (hasCity)
			{
				return Container.City;
			}

			if (hasState)
			{
				return Container.State;
			}

			if (hasZip)
			{
				return Container.Zip;
			}

			return Container.Empty;
		}

		#region Properties
		/// <summary>
		/// Get or set city
		/// </summary>
		public string City
		{
			get
			{
				return this.city;
			}
			set
			{
				this.city = value;
			}
		}

		/// <summary>
		/// Get or set state
		/// </summary>
		public string State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		/// <summary>
		/// Get or set zip code
		/// </summary>
		public string Zip
		{
			get
			{
				return this.zip;
			}
			set
			{
				this.zip = value;
			}
		}
		#endregion

		#region ICleanser Members
		/// <summary>
		/// Validate data integrity
		/// </summary>
		/// <returns></returns>
		public bool Validate()
		{
			if (!this.validate)
				return false;

			bool val = false;

			switch (GetContainer(City, State, Zip))
			{
				case Container.Empty:
					val = false;
					break;

				case Container.CityStateZip:
					val = TestRoutine.VerifyCSZ(City, State, Zip);
					break;

				case Container.CityState:
					val = TestRoutine.VerifyCSZ(City, State, null);
					break;

				case Container.City:
					val = TestRoutine.VerifyCSZ(City, null, null);
					break;

				case Container.State:
					val = TestRoutine.VerifyCSZ(null, State, null);
					break;

				case Container.Zip:
					val = TestRoutine.VerifyCSZ(null, null, Zip);
					break;

				default:
					val = false;
					break;
			}

			return val;
		}

		/// <summary>
		/// Clean data
		/// </summary>
		/// <returns></returns>
		public bool Cleanse()
		{
			bool cleansed = false;
			switch (GetContainer(City, State, Zip))
			{
				case Container.Empty:
					break;
				case Container.CityStateZip:
					break;
				case Container.CityState:
					break;
				case Container.City:
					break;
				case Container.State:
					break;
				case Container.Zip:
					break;
				default:
					break;
			}
			return cleansed;
		}

		bool ICleanser.Cleanse()
		{
			return Cleanse();
		}

		bool ICleanser.Validate()
		{
			return Validate();
		}

		#endregion
	}
}
