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
#if !SILVERLIGHT
using System.Data;
#endif
using System.Reflection;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;
using crudwork.Utilities;
using System.Text.RegularExpressions;

namespace crudwork.DynamicRuntime
{
	/// <summary>
	/// DynamicCode - dynamically invoke a member of a class during run-time.
	/// </summary>
	public static class DynamicCode
	{
		#region Binding Flags

		/* FYI: Notes on Binding Flags
		 * Getter/setter flags is set to work with both public and private properties.
		 * Property name is not case-sensitived.
		 * */

		/// <summary>the binding flags for invoking a set property</summary>
		private static BindingFlags invokeSetPropertyFlag = BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
		/// <summary>the binding flags for invoking  a get property</summary>
		private static BindingFlags invokeGetPropertyFlag = BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty;

		/// <summary>the binding flags for invoking a set field</summary>
		private static BindingFlags invokeSetFieldFlag = BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;
		/// <summary>the binding flags for invoking  a get field</summary>
		private static BindingFlags invokeGetFieldFlag = BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;

		/// <summary>the binding flags for invoking a member</summary>
		/// <remarks>should allow invoking a public method</remarks>
		private static BindingFlags invokeMemberFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Static;

		private static BindingFlags getMethodFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		#endregion

		#region Invoke

		#region Property Getter/Setter

		#region Setter

		/// <summary>
		/// set a value of a given property name.  The name/value pair is specified.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		public static void SetProperty(object myObject, string propertyName, object propertyValue)
		{
			InternalSetter(myObject, propertyName, propertyValue, invokeSetPropertyFlag);
		}

#if !SILVERLIGHT
		/// <summary>
		/// set a value of a given property name, using an OrderedDictionary enumerator.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="dict"></param>
		public static void SetProperty(object myObject, OrderedDictionary dict)
		{
			if (myObject == null)
				throw new ArgumentNullException("thisObject");
			if (dict == null)
				throw new ArgumentNullException("dict");

			IDictionaryEnumerator en = dict.GetEnumerator();
			while (en.MoveNext())
			{
				DynamicCode.SetProperty(myObject, en.Key.ToString(), en.Value);
			}
		}
#endif

		/// <summary>
		/// set a value of a given property name, using a list of key/value pair.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="dict"></param>
		public static void SetProperty(object myObject, List<DictionaryEntry> dict)
		{
			if (myObject == null)
				throw new ArgumentNullException("thisObject");
			if (dict == null)
				throw new ArgumentNullException("dict");

			foreach (DictionaryEntry en in dict)
			{
				DynamicCode.SetProperty(myObject, en.Key.ToString(), en.Value);
			}
		}

		/// <summary>
		/// set a value of a given property name, using an list of key/value pair.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T SetProperty<T>(List<DictionaryEntry> list)
		{
			T theObject = Activator.CreateInstance<T>();
			SetProperty(theObject, list);
			return theObject;
		}

		#endregion

		#region Getter

		/// <summary>
		/// get a value of a given property name.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static object GetProperty(object myObject, string propertyName)
		{
			return InternalGetter(myObject, propertyName, invokeGetPropertyFlag);
		}

		/// <summary>
		/// get all public properties and return a list of key/value pair
		/// </summary>
		/// <param name="theObject"></param>
		/// <returns></returns>
		public static List<DictionaryEntry> GetProperty(object theObject)
		{
			var results = new List<DictionaryEntry>();
			var pis = theObject.GetType().GetProperties();
			for (int i = 0; i < pis.Length; i++)
			{
				var kv = new DictionaryEntry();
				var pi = pis[i];
				kv.Key = pi.Name;
				kv.Value = DynamicCode.GetProperty(theObject, pi.Name);
				results.Add(kv);
			}
			return results;
		}

		#endregion

		#endregion

		#region Field Getter/Setter

		/// <summary>
		/// set a value of a given field name.  The name/value pair is specified.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		public static void SetField(object myObject, string fieldName, object fieldValue)
		{
			InternalSetter(myObject, fieldName, fieldValue, invokeSetFieldFlag);
		}

		/// <summary>
		/// get a value of a given field name.
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public static object GetField(object myObject, string fieldName)
		{
			return InternalGetter(myObject, fieldName, invokeGetFieldFlag);
		}

		#endregion

		#region Method dynamic invocation

		/// <summary>
		/// Invoke a method with specified arguments
		/// </summary>
		/// <param name="myObject"></param>
		/// <param name="methodName"></param>
		/// <param name="aguments"></param>
		public static object InvokeMethod(object myObject, string methodName, params object[] aguments)
		{
			try
			{
				return myObject.GetType().InvokeMember(methodName, invokeMemberFlag, null, myObject, aguments);
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
					throw ex.InnerException;
				throw ex;
			}
		}

		#endregion

		private static void InternalSetter(object myObject, string name, object value, BindingFlags invokeAttr)
		{
			try
			{
				try
				{
					Type t = myObject.GetType();
					t.InvokeMember(name, invokeAttr, null, myObject, new object[] { value });
					return;
				}
				catch (MissingMemberException ex)
				{
					Debug.WriteLine("Try 1: " + ex.Message);
				}

				int count = 1;
				Type baseT = myObject.GetType().BaseType;
				while (baseT != null)
				{
					count++;
					try
					{
						Type t = baseT;
						t.InvokeMember(name, invokeAttr, null, myObject, new object[] { value });
						return;
					}
					catch (MissingMemberException ex)
					{
						Debug.WriteLine(string.Format("Try {0}: {1}", count, ex.Message));
					}

					baseT = baseT.BaseType;
				}
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
					throw ex.InnerException;
				throw ex;
			}

			// --------------------------------------------------------------
			// if all fails, throw an exception.
			// --------------------------------------------------------------
			{
				var ex = new MissingMemberException("Unable to set property/field name: " + name);
				DebuggerTool.AddData(ex, "name", name);
				DebuggerTool.AddData(ex, "value", value);
				DebuggerTool.AddData(ex, "value.GetType()", value.GetType().FullName);
				throw ex;
			}
		}
		private static object InternalGetter(object myObject, string name, BindingFlags invokeAttr)
		{
			try
			{
				try
				{
					Type t = myObject.GetType();
					return t.InvokeMember(name, invokeAttr, null, myObject, null);
				}
				catch (MissingMemberException ex)
				{
					Debug.WriteLine("Try 1: " + ex.Message);
				}

				int count = 1;
				Type baseT = myObject.GetType().BaseType;
				while (baseT != null)
				{
					count++;
					try
					{
						Type t = baseT;
						return t.InvokeMember(name, invokeAttr, null, myObject, null);
					}
					catch (MissingMemberException ex)
					{
						Debug.WriteLine(string.Format("Try {0}: {1}", count, ex.Message));
					}

					baseT = baseT.BaseType;
				}
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
					throw ex.InnerException;
				throw ex;
			}
			// --------------------------------------------------------------
			// if all fails, throw an exception.
			// --------------------------------------------------------------
			{
				var ex = new MissingMemberException("Unable to get property/field name: " + name);
				DebuggerTool.AddData(ex, "name", name);
				throw ex;
			}
		}

		#endregion

		#region Perform nested lookup

		/// <summary>
		/// GetProperties recursively.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static PropertyInfo[] GetNestedProperties(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var result = new List<PropertyInfo>();

			while (true)
			{
				result.AddRange(type.GetProperties());

				if (type.BaseType.FullName == "System.Object")
					break;

				type = type.BaseType;
			}

			return result.ToArray();
		}

		#endregion

		#region Convert List<T> to DataTable

#if !SILVERLIGHT
		/// <summary>
		/// convert the specified object into a DataTable
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		public static DataTable ToTable<T>(List<T> items)
		{
			var dt = new DataTable();
			var pis = typeof(T).GetProperties();

			foreach (var pi in pis)
			{
				Type type = pi.PropertyType;
				bool isNullableType = type.ToString().StartsWith("System.Nullable`", StringComparison.InvariantCultureIgnoreCase);

				if (isNullableType)
				{
					string typename = type.ToString();
					var m = Regex.Match(typename, @"^System.Nullable`\d+\[(?<BaseType>.+)\]$");
					if (!m.Success)
						throw new ArgumentException("invalid nullable type");

					string basetype = m.Groups["BaseType"].Value;
					type = Type.GetType(basetype);
				}

				var dc = dt.Columns.Add(pi.Name, type);

				dc.AllowDBNull = isNullableType || (type == typeof(string));
			}

			foreach (var item in items)
			{
				var dr = dt.NewRow();

				foreach (var pi in pis)
				{
					dr[pi.Name] = DynamicCode.GetProperty(item, pi.Name) ?? DBNull.Value;
				}

				dt.Rows.Add(dr);
			}

			return dt;
		}
#endif

		#endregion

		#region Retrieve info via reflection

		/// <summary>
		/// return a list of types defined in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <returns></returns>
		public static List<Type> GetTypes(string assemblyFile)
		{
			Assembly asm = Assembly.LoadFrom(assemblyFile);

			List<Type> results = new List<Type>();

			Type[] types = asm.GetTypes();
			results.AddRange(types);

			results.Sort((x, y) => string.Compare(x.FullName, y.FullName));
			return results;
		}

		/// <summary>
		/// return a list of classes defined in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <returns></returns>
		public static List<string> GetClasses(string assemblyFile)
		{
			return GetClasses(assemblyFile, false);
		}

		/// <summary>
		/// return a list of public (or all) classes defined in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="isPublic"></param>
		/// <returns></returns>
		public static List<string> GetClasses(string assemblyFile, bool isPublic)
		{
			List<Type> types = GetTypes(assemblyFile);

			List<string> results = new List<string>();
			foreach (var item in types)
			{
				// DBacon's advance formula: condition or !switch
				if (item.IsPublic || !isPublic)
					results.Add(item.FullName);
			}

			return results;
		}

		/// <summary>
		/// search for a specified class name in a assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		private static Type GetType(string assemblyFile, string className)
		{
			List<Type> types = GetTypes(assemblyFile);
			foreach (var item in types)
			{
				if (item.FullName.Equals(className))
					return item;
			}
			return null;
		}

		/// <summary>
		/// return a specified method if defined in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<MethodInfo> GetMethods(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<MethodInfo>(type.GetMethods(getMethodFlags));
		}

		/// <summary>
		/// return a list of defined constructor for a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<ConstructorInfo> GetConstructors(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<ConstructorInfo>(type.GetConstructors());
		}

		/// <summary>
		/// return a list of attributes defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<object> GetCustomAttributes(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<object>(type.GetCustomAttributes(true));
		}

		/// <summary>
		/// return a list of events defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<EventInfo> GetEvents(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<EventInfo>(type.GetEvents(getMethodFlags));
		}

		/// <summary>
		/// return a list of fields defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<FieldInfo> GetFields(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<FieldInfo>(type.GetFields(getMethodFlags));
		}

		/// <summary>
		/// return a list of generic arguments defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<Type> GetGenericArguments(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<Type>(type.GetGenericArguments());
		}

		/// <summary>
		/// return a list of generic parameter constraints defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<Type> GetGenericParameterConstraints(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<Type>(type.GetGenericParameterConstraints());
		}

		/// <summary>
		/// return a list of interfaces defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<Type> GetInterfaces(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<Type>(type.GetInterfaces());
		}

		/// <summary>
		/// return a list of public member defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<MemberInfo> GetMembers(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<MemberInfo>(type.GetMembers(getMethodFlags));
		}

		/// <summary>
		/// return a list of public nested types defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<Type> GetNestedTypes(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
#if !SILVERLIGHT
			return new List<Type>(type.GetNestedTypes());
#else
			throw new NotImplementedException();
#endif
		}

		/// <summary>
		/// return a list of public properties defined in a class in the assembly file
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static List<PropertyInfo> GetProperties(string assemblyFile, string className)
		{
			Type type = GetType(assemblyFile, className);
			return new List<PropertyInfo>(type.GetProperties(getMethodFlags));
		}

		#endregion

		#region Decompile methods

#if !SILVERLIGHT
		/// <summary>
		/// return the CIL for the method body of the method
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="className"></param>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static string[] GetCIL(string assemblyFile, string className, string methodName)
		{
			List<MethodInfo> methods = GetMethods(assemblyFile, className);
			foreach (var item in methods)
			{
				if (item.Name.Equals(methodName))
				{
					byte[] bytes = item.GetMethodBody().GetILAsByteArray();
					return OpcodeManager.ConvertToCIL(bytes);
				}
			}
			return null;
		}
#endif

		#endregion
	}
}

namespace System
{
	using crudwork.DynamicRuntime;

	/// <summary>
	/// Extensions for Type
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Extension: Same as GetProperties() but recurse into base class and return its properties too.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static PropertyInfo[] GetNestedProperties(this Type type)
		{
			return DynamicCode.GetNestedProperties(type);
		}
	}
}