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
using crudwork.Models.DataAccess;

namespace crudwork.Controls
{
	internal interface IConnectionStringBuilder
	{
		/// <summary>
		/// Get or set the database provider name
		/// </summary>
		DatabaseProvider DatabaseProvider
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the connection string
		/// </summary>
		string ConnectionString
		{
			get;
			set;
		}

		/// <summary>
		/// Show or hide the password value (affects the GUI only)
		/// </summary>
		bool MaskPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Test the connection
		/// </summary>
		void TestConnection();
	}
}
