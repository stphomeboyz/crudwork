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
using System.IO;

namespace crudwork.Utilities
{
	/// <summary>
	/// Math Utility
	/// </summary>
	public static class MathUtil
	{
		/// <summary>
		/// return the percentage of a given value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="total"></param>
		/// <returns></returns>
		public static decimal Percentage(decimal value, decimal total)
		{
			return value / total * 100;
		}

		/// <summary>
		/// return true if percent is equal to the tenths of a percent, such as 10%, 20%, 30%, ..., 90%, and 100%.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="total"></param>
		/// <returns></returns>
		public static bool IsTenthPercent(decimal value, decimal total)
		{
			return Percentage(value, total) % 10 == 0;
		}

		/// <summary>
		/// return the min value in the list of double
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static double Min(params double[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentNullException("values");

			double result = double.MaxValue;

			for (int i = 0; i < values.Length; i++)
			{
				result = Math.Min(result, values[i]);
			}

			return result;
		}

		/// <summary>
		/// return the max value in the list of double
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static double Max(params double[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentNullException("values");

			double result = double.MinValue;

			for (int i = 0; i < values.Length; i++)
			{
				result = Math.Max(result, values[i]);
			}

			return result;
		}

		/// <summary>
		/// Return the average of the given values
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static decimal Average(decimal[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentNullException("values");

			decimal acc = 0;
			for (int i = 0; i < values.Length; i++)
			{
				acc += values[i];
			}

			return acc / values.Length;
		}

		/// <summary>
		/// Return the checksum of the buffer
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static long ComputeChecksum(byte[] buffer)
		{
			return ComputeChecksum(buffer, 0);
		}

		/// <summary>
		/// Return the checksum of the buffer at the given offset
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public static long ComputeChecksum(byte[] buffer, int offset)
		{
			long sum = 0;
			for (int i = 0; i < buffer.Length; i++)
			{
				sum += buffer[i] * (i + 1 + offset);
			}
			return sum;
		}

		/// <summary>
		/// Return the checksum value of the given filename
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static long ComputeChecksum(string filename)
		{
			//byte[] buffer = FileUtil.ReadFile(filename, 4096);
			//return ComputeChecksum(buffer);

			long sum = 0;
			int bufSize = 4096;

			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			using (BinaryReader r = new BinaryReader(fs))
			{
				byte[] readChar = null;
				int offset = 0;
				
				do
				{
					readChar = r.ReadBytes(bufSize);
					sum += ComputeChecksum(readChar, offset);
					offset += readChar.Length;
				} while ((readChar != null) && (readChar.Length > 0));

				r.Close();
				//fs.Close();
			}

			return sum;
		}
	}
}
