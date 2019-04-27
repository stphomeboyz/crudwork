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
	/// Query Filter keywords
	/// </summary>
	public enum QueryFilter
	{
		/// <summary>No filter</summary>
		None = 0,
		/// <summary>Contains this value</summary>
		Contains = 1,
		/// <summary>Starts with value</summary>
		StartsWith = 2,
		/// <summary>Ends with value</summary>
		EndsWith = 3,
		/// <summary>Exact match</summary>
		Exact = 4,
	}
}
