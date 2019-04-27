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
using System.Drawing;

namespace crudwork.Utilities
{
	/// <summary>
	/// Color Utility
	/// </summary>
	public static class ColorUtil
	{
		private static Random random = new Random();
		private static int r;
		private static int g;
		private static int b;

		/// <summary>
		/// Return a randomly-generated color
		/// </summary>
		/// <returns></returns>
		public static Color GetRandomColor()
		{
			r = random.Next(0, 256);
			g = random.Next(0, 256);
			b = random.Next(0, 256);
			return Color.FromArgb(r, g, b);
		}
	}
}
