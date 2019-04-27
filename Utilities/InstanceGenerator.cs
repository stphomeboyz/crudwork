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
using System.ComponentModel;

namespace crudwork.Utilities
{
	/// <summary>
	/// Instance generator
	/// </summary>
	public static class InstanceGenerator
	{
		/// <summary>
		/// Create an instance of given type and arguments
		/// </summary>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static object Create(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, args);
		}

		/// <summary>
		/// Create an array of instances of given type and arguments
		/// </summary>
		/// <param name="numberOfInstances"></param>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static object[] Create(int numberOfInstances, Type type, params object[] args)
		{
			var results = new object[numberOfInstances];
			for (int i = 0; i < numberOfInstances; i++)
			{
				results[i] = Create(type, args);
			}
			return results;
		}

		/// <summary>
		/// Create an instance of given type and arguments
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public static T Create<T>(params object[] args)
		{
			return (T)Activator.CreateInstance(typeof(T), args);
		}

		/// <summary>
		/// Create an array of instances of given type and arguments
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="numberOfInstances"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static T[] Create<T>(int numberOfInstances, params object[] args)
		{
			var results = new T[numberOfInstances];
			for (int i = 0; i < numberOfInstances; i++)
			{
				results[i] = Create<T>(args);
			}
			return results;
		}
	}

	/// <summary>
	/// Generate instances for generic type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class InstanceGenerator<T> : TypeConverter
	{
		/// <summary>
		/// create a number of instances of given generic type.
		/// </summary>
		/// <param name="totalElements"></param>
		/// <returns></returns>
		public static T[] CreateArray(int totalElements)
		{
			try
			{
				CheckAttribute();

				List<T> list = new List<T>();
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));


				for (int i = 0; i < totalElements; i++)
				{
					list.Add((T)tc.ConvertTo(i, typeof(T)));
				}

				return list.ToArray();
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "totalElements", totalElements);
				throw;
			}
		}

		/// <summary>
		/// Make sure the generic-type T specified a TypeConverterAttribute attribute.
		/// throw an exception otherwise.
		/// </summary>
		private static void CheckAttribute()
		{
			// Gets the attributes for the instance.
			AttributeCollection ac = TypeDescriptor.GetAttributes(typeof(T));

			/* Prints the name of the type converter by retrieving the 
			 * TypeConverterAttribute from the AttributeCollection. */
			TypeConverterAttribute tca = (TypeConverterAttribute)ac[typeof(TypeConverterAttribute)];

			if (String.IsNullOrEmpty(tca.ConverterTypeName))
			{
				string s = String.Format("Type <T> '{0}' is not bounded to a TypeConverter attribute",
					TypeDescriptor.GetClassName(typeof(T)));
				throw new ArgumentNullException(s);
			}

			//Console.WriteLine("The type conveter for this class is: " + tca.ConverterTypeName);
		}
	}
}
