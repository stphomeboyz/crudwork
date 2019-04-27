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
using crudwork.Utilities;

namespace crudwork.DataWarehouse
{
	/*
	**
	** 1. Use loadjava to load Scorer.class into Oracle.
	** 2. Run the command in SETUP.
	** 3. You now have a fuzzy-match function -- see the documentation in Scorer.java.
	** 
	** It seems like you can't call a Java stored procedure directly from
	** SQL, weirdly enough -- I get `unknown column error' unless I call it
	** from PL/SQL instead.  Annoying.  Is there a trick to it?
	**
	** (by Darius Bacon on 05/27/2003)
	**
	**
	**
	**
	** Notes:
	**
	**	06-24-2004	stp		fixed cast (int) overflow bug on score().
	**
	**	01-13-2006	stp		ported to Visual Studio J#  :-)
	**
	**	02-11-2008	stp		ported to C# because J# is defunc.
	*/

	/// <summary>
	/// Port of SCORE.C. 
	/// </summary>
	internal static class Scorer
	{
		///<summary>Range of valid indices in a result from encode().</summary>
		private static int VALS = 38;


		/// <summary>
		/// points[i][j] is the `goodness of match' between encoded indices
		/// i and j.  We initialize with the values from a POINTS.DAT that
		/// seems standard, but it might be safest to always run loadPoints()
		/// to be sure you're always getting the measure you want.
		/// </summary>
		private static int[][] points = new int[][] {
		/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
		/*  1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29   30   31   32   33   34   35   36   37   38 */
		/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
		#region Static numbers
		new int[] {   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,  16,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,  64 },
		new int[] {   0, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  32,  16,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0, 255,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,  32,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,  32,   0 },
		new int[] {   0,   0,   0,   0, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0, 255,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,  16, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  32,  16,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0, 255,  16,   0,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0 },
		new int[] {   0,   0,  32,   0,   0,   0,   0,  16, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 255,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0 },
		new int[] {  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,  16,   0,   0,   0,   0,   0,   0,  96,   0,   0,   0,   0,   0,   0,   0,  96,   0,   0,   0,   0,   0,   0,  64,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16, 128,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,  32,   0,   0,  16, 128,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,  16,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,  16,   0,   0,   0,  16, 128,   0,   0,  64,  32,   0,   0,   0,   0,   0,  32,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,  48,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0 },
		new int[] {   0,   0,  32,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,  32,   0,   0,   0, 128,  32,  16,  64,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  64,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  64,  48,  32, 128,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  96,   0,   0,   0,  32,   0,  16,   0, 128,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16 },
		new int[] {   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  64,   0,   0, 128,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16 },
		new int[] {  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0, 128,  48,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,  48, 128,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0, 128,   0,  64,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16 },
		new int[] {   0,  32,  16,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,  32,   0,   0,   0,  32,   0,   0,   0,  64,   0, 128,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,  16,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,  96,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,   0,   0,   0,  16,  16,   0,  96,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,  32,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0, 128,   0,   0,   0,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 128,  64,  32,   0,  16,   0,   0 },
		new int[] {  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  64, 128,   0,   0,  32,   0,   0 },
		new int[] {   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,  32,   0, 128,   0,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0, 128,   0,   0,   0 },
		new int[] {   0,   0,   0,   0,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,  64,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,  32,   0,   0, 128,   0,   0 },
		new int[] {   0,   0,   0,  32,   0,   0,   0,   0,   0,   0,   0,  16,   0,  64,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  96,   0,   0,   0,   0,   0,   0, 128,   0 },
		new int[] {  64,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  16,  16,   0,   0,   0,  16,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 127 }
		#endregion
	    };

		/////<summary>Load the matching table from POINTS.DAT.</summary>
		//public static void loadPoints() throws IOException
		//{
		//    InputStream in = new FileInputStream("POINTS.DAT");
		//    int[][] result = new int[VALS][VALS];
		//    for (int i = 0; i < VALS; ++i)
		//    {
		//        for (int j = 0; j < VALS; ++j)
		//        {
		//            int b = in.read();
		//            if (1 == -b)
		//            {
		//                throw new IOException("Unexpected EOF in POINTS.DAT");
		//            }
		//            result[i][j] = 0xFF & b;
		//        }
		//    }
		//    in.close();
		//    points = result;
		//}


		/// <summary>
		/// Compare a and b, and return a score that is higher for closer
		/// matches, and 0 for total mismatches.  The shorter of the two
		/// is padded out to the longer length.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int Match(String a, String b)
		{
			// I think Oracle should never call us with a null value,
			// since nullness is supposed to propagate.  Let's see what
			// we get when we try this.
			if (null == a || null == b)
			{
				return 0;
			}

			// Pad out a and b to the same length.
			int alen = a.Length;
			int blen = b.Length;
			int length;
			if (alen < blen)
			{
				length = blen;
				a = StringUtil.Pad(a, length);
			}
			else if (blen < alen)
			{
				length = alen;
				b = StringUtil.Pad(b, length);
			}
			else
			{
				length = alen;
			}

			String ea = Encode(a.ToUpper());
			String eb = Encode(b.ToUpper());
			return Score(ea, eb, length);
		}

		/// <summary>
		/// Return an encoding of string that can be used by score().
		/// Its characters correspond one-to-one with the characters of
		/// the input string (so the length will be the same).
		/// TODO: return a byte[]?  Can Oracle handle that?
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String Encode(String value)
		{
			int length = value.Length;
			StringBuilder code = new StringBuilder(length);
			for (int i = 0; i < length; ++i)
			{
				char c = value[i];
				char encoded =
				(char)((c == ' ') ? 0 : (c >= '0' && c <= '9') ? c + (1 - '0') :
					   (c >= 'A' && c <= 'Z') ? c + (11 - 'A') : VALS - 1);
				code.Append(encoded);
			}
			return code.ToString();
		}

		/// <summary>
		/// Compare the first `length' characters of a and b, and return
		/// a score that is higher for closer matches, and 0 for total
		/// mismatches.  a and b must be the results of calls to encode();
		/// it's an error to supply any other type of string.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static int Score(String a, String b, int length)
		{
			if (0 == length)
				return 0;

			//for (int i = 0; i < length; ++i)
			//{
			//    int c = (int)a.charAt(i);
			//    int d = (int)b.charAt(i);
			//}

			{
				int maxfact = (length * length + 514) / (2 * length);
				int scale = 0, temp = 0;
				int factor;

				factor = maxfact;
				for (int i = 0; i < length; ++i)
				{
					int c = (int)a[i];
					scale += points[c][c] * factor--;
				}

				factor = maxfact;
				for (int i = 0; i < length; ++i)
				{
					int c = (int)b[i];
					temp += points[c][c] * factor--;
				}

				if (scale < temp)
					scale = temp;

				{
					int p = ComputePoints((int)length, a, 0, b, 0, maxfact, 0);
					long expr1 = (long)p * 65535;
					return (int)(expr1 / scale);
					//return (int)((p * 65535) / scale);
				}
			}
		}

		/// <summary>
		/// compute points
		/// </summary>
		/// <param name="length"></param>
		/// <param name="a"></param>
		/// <param name="ai"></param>
		/// <param name="b"></param>
		/// <param name="bi"></param>
		/// <param name="factor"></param>
		/// <param name="slides"></param>
		/// <returns></returns>
		private static int ComputePoints(int length,
				   String a, int ai,
				   String b, int bi,
				   int factor, int slides)
		{
			int ret = 0, best;
			for (;
				 ai < length && bi < length && a[ai] == b[bi];
				 ++ai, ++bi)
			{
				int c = (int)a[ai];
				ret += points[c][c] * factor--;
			}
			if (ai >= length || bi >= length)
				return ret;

			/* Require digits to match in place. */
			{
				int c = (int)a[ai];
				int d = (int)b[bi];
				if (((0 != c && c < 11) || (0 != d && d < 11))
				&& 0 == points[c][d])
					return 0;
			}
			{
				int c = (int)a[ai];
				int d = (int)b[bi];
				best = points[c][d] * factor--;
			}
			if (slides < 2)
			{
				int temp;
				int aj = ai + 1;
				int bj = bi + 1;
				slides++;
				if (aj < length && bj < length)
				{	/* Transposition. */
					if (a[ai] == b[bj] &&
						  a[aj] == b[bi])
					{
						int c = (int)a[ai];
						ret += points[c][c] * 2 * factor;
						return ret +
						((++aj < length && ++bj < length) ?
						 ComputePoints(length, a, aj, b, bj, --factor, slides) : 0);
					}
					best += ComputePoints(length, a, aj, b, bj, factor, slides);
				}
				if (aj < length)
				{
					temp = ComputePoints(length, a, aj, b, bi, factor, slides);
					if (temp > best)
						best = temp;
				}
				if (bj < length)
				{
					temp = ComputePoints(length, a, ai, b, bj, factor, slides);
					if (temp > best)
						best = temp;
				}
			}
			return ret + best;
		}
	}
}
