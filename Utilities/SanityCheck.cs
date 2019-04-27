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

namespace crudwork.Utilities
{
	/// <summary>
	/// Routines for performing sanity checks
	/// </summary>
	public static class SanityCheck
	{
		#region Throw methods
		/// <summary>
		/// Throw a SanityCheckException with given message and inner exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		/// <param name="data"></param>
		private static void Throw(string message, Exception innerException, params object[] data)
		{
			throw new SanityCheckException(message, innerException);
		}

		/// <summary>
		/// Throw a SanityCheckException with given message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="data"></param>
		private static void Throw(string message, params object[] data)
		{
			throw new SanityCheckException(message);
		}

		private static void AddData(SanityCheckException ex, params object[] data)
		{
			if (data == null)
				return;

			for (int i = 0; i < data.Length; i++)
			{
				DebuggerTool.AddData(ex, "idx" + i, data[i]);
			}
		}
		#endregion

		/// <summary>
		/// Throw exception if the expression evaluates to false.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="message"></param>
		public static void Assert(bool expression, string message)
		{
			if (!expression)
				Throw(message, new object[] { expression });
		}

		/// <summary>
		/// Throw exception if the object is set to null, set to DbNull, or an empty string.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		public static void IsNullOrEmpty(object value, string message)
		{
			if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
				Throw(message, new object[] { value });
		}

		/// <summary>
		/// Throw exception if the value is not within the min/max range.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="message"></param>
		public static void IsWithRange(int value, int min, int max, string message)
		{
			if (value < min || value > max)
				Throw(message, new object[] { value, min, max });
		}
	}

	/// <summary>
	/// Represent a SanityCheck exception
	/// </summary>
	public class SanityCheckException : Exception
	{
		#region Constructors
		/// <summary>
		/// Create new instance with given attribute
		/// </summary>
		public SanityCheckException()
			: base()
		{
		}

		/// <summary>
		/// Create new instance with given attribute
		/// </summary>
		/// <param name="message"></param>
		public SanityCheckException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Create new instance with given attribute
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public SanityCheckException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		#endregion
	}
}
