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

namespace crudwork.DataWarehouse
{
	/// <summary>
	/// Collection of matcher methods
	/// </summary>
	public static class Matcher
	{
		/// <summary>
		/// match two strings using fuzzy-logic algorithm, return value of 0 - 66535
		/// where lowest is non-match and highest is exact match.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int FuzzyMatch(string a, string b)
		{
			if (a == b)
				return int.MaxValue;

			//return 1;
			return Scorer.Match(a, b);
		}

		/// <summary>
		/// match two strings using soundex algorithm.
		/// generate soundex value of a given string
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Soundex(string a, string b)
		{
			if (a == b)
				return true;

			string resulta = crudwork.DataWarehouse.Soundex.ToSoundexCode(a);
			string resultb = crudwork.DataWarehouse.Soundex.ToSoundexCode(b);
			return resulta == resultb;
		}
	}
}
