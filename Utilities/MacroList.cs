using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using crudwork.Models.DataAccess;
using System.Reflection;

namespace crudwork.Utilities
{
	/// <summary>
	/// List of Database Macro
	/// </summary>
	public class MacroList : List<MethodInfoEx>
	{
		/// <summary>
		/// Create a new instance with default attributes
		/// </summary>
		public MacroList()
		{
		}

		private static T GetAttribute<T>(object[] objects)
		{
			if (objects == null)
				return default(T);

			var attributeName = typeof(T).UnderlyingSystemType.FullName;
			foreach (var t in objects)
			{
				if (t.GetType().FullName == attributeName)
					return (T)t;
			}

			return default(T);
		}

		/// <summary>
		/// Get the attribute type from a given MethodInfo object.  If not found, return null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(MethodInfoEx obj)
		{
			return GetAttribute<T>(obj.MethodInfo);
		}

		/// <summary>
		/// Get the attribute type from a given MethodInfo object.  If not found, return null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(MethodInfo obj)
		{
			var types = obj.GetCustomAttributes(typeof(T), true);
			return GetAttribute<T>(types);
		}

		/// <summary>
		/// Get the attribute type from the given Type.  If not found, return null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(Type type)
		{
			var types = type.UnderlyingSystemType.GetCustomAttributes(typeof(T), true);
			return GetAttribute<T>(types);
		}

		/// <summary>
		/// Create a parameter for the given method
		/// </summary>
		/// <param name="method"></param>
		/// <param name="tokens"></param>
		/// <returns></returns>
		public static object[] CreateParameter(MethodInfo method, string[] tokens)
		{
			var result = new List<object>();

			var piList = method.GetParameters();

			if (piList.Length != (tokens.Length - 1))
			{
				var s = string.Format("Method {0} expected {1} parameters; but the query has {2}",
					method.Name, piList.Length, tokens.Length - 1);
				throw new ArgumentException(s);
			}

			foreach (var pi in piList)
			{
				var idx = pi.Position + 1;
				Type type = pi.ParameterType;

				if (pi.ParameterType.IsEnum)
					type = Enum.GetUnderlyingType(pi.ParameterType);

				var p = DataConvert.ChangeType(tokens[idx], pi.DefaultValue, type);
				result.Add(p);
			}

			return result.ToArray();
		}

		/// <summary>
		/// Generate a help on parameter usage
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public static string CreateHelp(MethodInfo method)
		{
			var sb = new StringBuilder();
			var piList = method.GetParameters();

			// create the method signature:
			sb.Append("Usage: " + method.Name);
			foreach (var pi in piList)
			{
				sb.AppendFormat(" <{0}>", pi.Name);
			}

			// describe each parameter(s):
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine("Where:");

			foreach (var pi in piList)
			{
				sb.AppendFormat("    {0} is {1}", pi.Name, pi.ParameterType);
				sb.AppendLine();
			}

			sb.AppendLine();

			return sb.ToString();
		}

		/// <summary>
		/// Add a class type to the collection.  The class type specified must declare the MacroAttribute, and
		/// only methods with MacroMethodAttribute will be added to the collection.
		/// </summary>
		/// <param name="instance"></param>
		public void AddInstance(object instance)
		{
			var type = instance.GetType();
			var classAttr = GetAttribute<MacroServiceAttribute>(type);
			if (classAttr == null)
				throw new ArgumentException("Type does not declare the MacroAttribute attribute");

			var methods = type.GetMethods();

			foreach (var m in methods)
			{
				var methodAttr = GetAttribute<MacroMethodAttribute>(m);

				if (methodAttr == null)
					continue;

				if (!m.IsPublic)
				{
					var s = string.Format("The access modifier for method '{0}' must be public", m.Name);
					throw new ArgumentException(s);
				}

				this.Add(new MethodInfoEx(m, instance));
			}
		}
	}

	/// <summary>
	/// MethodInfo plus more
	/// </summary>
	public class MethodInfoEx
	{
		/// <summary>
		/// The method
		/// </summary>
		public MethodInfo MethodInfo
		{
			get;
			set;
		}

		/// <summary>
		/// The instance used to invoke the method
		/// </summary>
		public object Instance
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public MethodInfoEx()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <param name="instance"></param>
		public MethodInfoEx(MethodInfo methodInfo, object instance)
		{
			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");
			if (instance == null)
				throw new ArgumentNullException("instance");
			this.MethodInfo = methodInfo;
			this.Instance = instance;
		}
	}
}
