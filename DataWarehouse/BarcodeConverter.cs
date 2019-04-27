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

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// Barcode Converter
	/// </summary>
	public class BarcodeConverter
	{
		/// <summary>
		/// The Zipcode Barcode Lookup
		/// </summary>
		public static Dictionary<int, byte[]> ZipcodeBar
		{
			get;
			private set;
		}

		static BarcodeConverter()
		{
			// http://www.ams.org/featurecolumn/archive/barcodes3.html
			ZipcodeBar = new Dictionary<int, byte[]>();

			ZipcodeBar.Add(1, new byte[] { 0, 0, 0, 1, 1 });
			ZipcodeBar.Add(2, new byte[] { 0, 0, 1, 0, 1 });
			ZipcodeBar.Add(3, new byte[] { 0, 0, 1, 1, 0 });
			ZipcodeBar.Add(4, new byte[] { 0, 1, 0, 0, 1 });
			ZipcodeBar.Add(5, new byte[] { 0, 1, 0, 1, 0 });
			ZipcodeBar.Add(6, new byte[] { 0, 1, 1, 0, 0 });
			ZipcodeBar.Add(7, new byte[] { 1, 0, 0, 0, 1 });
			ZipcodeBar.Add(8, new byte[] { 1, 0, 0, 1, 0 });
			ZipcodeBar.Add(9, new byte[] { 1, 0, 1, 0, 0 });
			ZipcodeBar.Add(0, new byte[] { 1, 1, 0, 0, 0 });
		}
	}
}
