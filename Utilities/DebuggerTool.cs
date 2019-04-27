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
using System.Diagnostics;
using System.IO;
using System.Collections;
#if !SILVERLIGHT
using System.Data;
using System.Data.SqlClient;
using System.Web;
#endif
//using System.ServiceModel;

namespace crudwork.Utilities
{
	/// <summary>
	/// Debugger Utility
	/// </summary>
	public class DebuggerTool
	{
		private const int MAXSTRINGLEN = 50;
		private string logFilename;
		private static object lockObj = new object();
		private const int PADBYLENGTH = 40;
		private const char PADWITHCHAR = '-';

		#region Constructors
		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="logFilename"></param>
		/// <param name="appendDate"></param>
		public DebuggerTool(string logFilename, bool appendDate)
		{
			if (appendDate)
			{
#if !SILVERLIGHT
				logFilename = FileUtil.AppendSuffixToFilename(logFilename, "-" + DateTime.Now.ToString("yyyy-MM-dd"));
#else
				throw new NotImplementedException();
#endif
			}
			this.logFilename = logFilename;
		}

		/// <summary>
		/// Create a new instance with given attribute
		/// </summary>
		/// <param name="logFilename"></param>
		public DebuggerTool(string logFilename)
			: this(logFilename, false)
		{
		}
		#endregion

		#region Dump methods
		/// <summary>
		/// Dump an object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Dump(object obj)
		{
			return obj.ToString();
		}

#if !SILVERLIGHT
		/// <summary>
		/// Dump a DataSet object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Dump(DataSet obj)
		{
			return obj.ToString();
		}

		/// <summary>
		/// Dump a DataTable object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Dump(DataTable obj)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < obj.Columns.Count; i++)
			{
				DataColumn dc = obj.Columns[i];

				if (i > 0)
					sb.AppendFormat(Environment.NewLine);

				sb.AppendFormat("public {1} {0} {{get;set;}} ", dc.ColumnName, dc.DataType);
			}

			return sb.ToString();
		}
#endif

		/// <summary>
		/// Dump a string array of object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Dump(string[] obj)
		{
			return StringUtil.StringArrayToString(obj);
		}

		/// <summary>
		/// Dump an IDictionary object
		/// </summary>
		/// <param name="iDictionary"></param>
		/// <returns></returns>
		public static string Dump(IDictionary iDictionary)
		{
			StringBuilder s = new StringBuilder();
			IDictionaryEnumerator ide = iDictionary.GetEnumerator();

			while (ide.MoveNext())
			{
				s.AppendFormat("{0} = {1}\r\n", ide.Key, ide.Value);
			}

			return s.ToString();
		}

		#region Sql object dumper
#if !SILVERLIGHT
		/// <summary>
		/// dump a SqlCommand object
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static string Dump(SqlCommand command)
		{
			if (command == null)
				return string.Empty;

			StringBuilder s = new StringBuilder();

			switch (command.CommandType)
			{
				case CommandType.StoredProcedure:
					s.AppendFormat("exec {0}", command.CommandText);
					break;

				case CommandType.TableDirect:
					s.AppendFormat("[TABLEDIRECT] {0}", command.CommandText);
					break;

				case CommandType.Text:
					s.AppendFormat("{0}", command.CommandText);
					break;
			}

			s.AppendFormat(" // ::::: USING PARAMETERS [ {0} ]", Dump(command.Parameters));

			return s.ToString();
		}

		/// <summary>
		/// Dump a SqlParameterCollection object
		/// </summary>
		/// <param name="parameterCollection"></param>
		/// <returns></returns>
		public static string Dump(SqlParameterCollection parameterCollection)
		{
			if (parameterCollection == null)
				return string.Empty;

			SqlParameter[] parameters = new SqlParameter[parameterCollection.Count];
			int idx = 0;

			foreach (SqlParameter p in parameterCollection)
			{
				parameters[idx++] = p;
			}

			return Dump(parameters);
		}

		/// <summary>
		/// dump a SqlParameter array of objects.
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string Dump(params SqlParameter[] parameters)
		{
			if (parameters == null)
				return string.Empty;

			StringBuilder s = new StringBuilder();

			for (int i = 0; i < parameters.Length; i++)
			{
				SqlParameter p = parameters[i];

				if (i > 0)
					s.Append(", ");

				string value = Dump(p);
				if (string.IsNullOrEmpty(value))
					value = "NULL";

				s.AppendFormat("{0} /*{1}{2}*/ = {3}\r\n",
					p.ParameterName,
					p.SqlDbType,
					(p.DbType == DbType.String) ? "(" + p.Size + ")" : "",
					value);
			}

			return s.ToString();
		}

		/// <summary>
		/// dump a Sql value (with qoutes for string type)
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static string Dump(SqlParameter p)
		{
			try
			{
				if (p == null)
					return string.Empty;

				bool useQuote = false;
				switch (p.DbType)
				{
					case DbType.AnsiString:
					case DbType.AnsiStringFixedLength:
					case DbType.DateTime:
					case DbType.Date:
					case DbType.String:
					case DbType.StringFixedLength:
					case DbType.Time:
					case DbType.Xml:
						useQuote = true;
						break;

					default:
						useQuote = false;
						break;
				}

				if (useQuote)
					return "'" + p.Value + "'";
				else
					return DataConvert.ToString(p.Value);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.ToString());
				return string.Empty;
			}
		}
#endif
		#endregion

		/// <summary>
		/// Dump an exception object
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string Dump(Exception ex)
		{
			StringBuilder s = new StringBuilder();
			int level = 0;

			while (ex != null)
			{
				level++;
				s.AppendLine("ExceptionLevel:\t" + level);
				s.AppendLine("Message:\t" + ex.Message);
				s.AppendLine("Data:\t" + Dump(ex.Data));
				s.AppendLine("Detail:\t" + ex.ToString());
				ex = ex.InnerException;
			}

			return s.ToString();
		}
		#endregion

		#region DumpDebugInfo and helpers methods
		private static string GetValue(string value)
		{
			if (value.Length > MAXSTRINGLEN)
			{
				return string.Format("{0}... [TRUNCATED.  SHOWING FIRST {1} OF {2} CHARS.]", value.Substring(0, MAXSTRINGLEN), MAXSTRINGLEN, value.Length);
			}
			else
			{
				return value;
			}
		}

#if !SILVERLIGHT
		private static string DumpWebPageInfo()
		{
			if (HttpContext.Current == null)
				return string.Empty;

			StringBuilder s = new StringBuilder();

			s.AppendLine("ASP.NET Page:\t" + HttpContext.Current.Request.Url);
			//s.AppendFormat("Page URL: {0}\r\n", HttpContext.Current.Request.Url);

			// dump session
			s.Append("ASP.NET Session:\t");
			GenerateSession(s);

			// dump cookies
			s.Append("ASP.NET Cookies:\t");
			GenerateCookies(s);

			// dump application
			s.Append("ASP.NET Application:\t");
			GenerateApplication(s);

			// dump querystring / form
			s.Append("ASP.NET QueryString:\t");
			GenerateRequest(s);

			return s.ToString();
		}

		private static void GenerateRequest(StringBuilder s)
		{
			if (HttpContext.Current == null)
				return;

			try
			{
				s.AppendLine("---- Request.Form Information ----");


				for (int i = 0; i < HttpContext.Current.Request.Form.AllKeys.Length; i++)
				{
					string name = HttpContext.Current.Request.Form.AllKeys[i];
					object value = HttpContext.Current.Request.Form[name];

					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}

				s.AppendLine();
			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}

			try
			{
				s.AppendLine("---- Request.QueryString Information ----");


				for (int i = 0; i < HttpContext.Current.Request.QueryString.AllKeys.Length; i++)
				{
					string name = HttpContext.Current.Request.QueryString.AllKeys[i];
					object value = HttpContext.Current.Request.QueryString[name];
					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}
				s.AppendLine();

			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}
		}

		private static void GenerateCache(StringBuilder s)
		{
			//try
			//{
			//    s.AppendLine("Application Information");

			//    for (int i = 0; i < Cache.Count; i++)
			//    {
			//        string name = Application.AllKeys[i];
			//        object value = Application[name];
			//        s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value));
			//    }
			//    s.AppendLine();
			//}
			//catch (Exception ex)
			//{
			//    s.AppendLine(ex.ToString());
			//}
		}

		private static void GenerateApplication(StringBuilder s)
		{
			if (HttpContext.Current == null)
				return;

			try
			{
				s.AppendLine("---- Application Information ----");

				for (int i = 0; i < HttpContext.Current.Application.AllKeys.Length; i++)
				{
					string name = HttpContext.Current.Application.AllKeys[i];
					object value = HttpContext.Current.Application[name];
					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}
				s.AppendLine();

			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}
		}

		private static void GenerateCookies(StringBuilder s)
		{
			if (HttpContext.Current == null)
				return;

			try
			{
				s.AppendLine("---- Request.Cookie Information ----");


				for (int i = 0; i < HttpContext.Current.Request.Cookies.AllKeys.Length; i++)
				{
					string name = HttpContext.Current.Request.Cookies.AllKeys[i];
					object value = HttpContext.Current.Request.Cookies[name].Value;
					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}
				s.AppendLine();

			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}

			try
			{
				s.AppendLine("---- Response.Cookie Information ----");


				for (int i = 0; i < HttpContext.Current.Response.Cookies.AllKeys.Length; i++)
				{
					string name = HttpContext.Current.Response.Cookies.AllKeys[i];
					object value = HttpContext.Current.Response.Cookies[name].Value;
					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}
				s.AppendLine();

			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}
		}

		private static void GenerateSession(StringBuilder s)
		{
			if (HttpContext.Current == null)
				return;

			try
			{
				s.AppendFormat("---- Session Information (SessionID:{0}) ----\r\n", HttpContext.Current.Session.SessionID);

				for (int i = 0; i < HttpContext.Current.Session.Keys.Count; i++)
				{
					string name = HttpContext.Current.Session.Keys[i];
					object value = HttpContext.Current.Session[name];
					s.AppendFormat("\t{0} - {1}\r\n", name, GetValue(value.ToString()));
				}
				s.AppendLine();
			}
			catch (Exception ex)
			{
				s.AppendLine(ex.ToString());
			}

		}
#endif
		#endregion

		#region Log methods
		private string PadLine
		{
			get
			{
				return "".PadLeft(PADBYLENGTH, PADWITHCHAR);
			}
		}

		/// <summary>
		/// Log exception to log file
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="methodName"></param>
		public void Log(Exception exception, string methodName)
		{
			lock (lockObj)
			{
				try
				{
					using (StreamWriter sw = new StreamWriter(this.logFilename, true))
					{
						sw.WriteLine(PadLine);
						sw.WriteLine("Date:\t" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
#if !SILVERLIGHT
						sw.WriteLine("MachineName:\t" + Environment.MachineName);
#endif
						sw.WriteLine("MethodName:\t" + methodName);

						sw.WriteLine(Dump(exception));

#if !SILVERLIGHT
						// dump ASP.NET information
						if (HttpContext.Current != null)
							sw.WriteLine(DumpWebPageInfo());
#endif

						sw.Flush();
						sw.Close();
					}
				}
				catch (Exception ex)
				{
#if !SILVERLIGHT
					// worse case scenario: show it in the trace!
					Trace.WriteLine(ex.ToString());
#endif
				}
			}
		}

#if !SILVERLIGHT
		class KeyValue
		{
			public readonly string Key;
			public readonly string Value;

			public KeyValue()
				: this(string.Empty, string.Empty)
			{
			}

			public KeyValue(string key, string value)
			{
				this.Key = key;
				this.Value = value;
			}
		}

		/// <summary>
		/// Convert a log file to a data table
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public DataTable Log2Table(string filename)
		{
			List<KeyValue> tokens = new List<KeyValue>();

			#region Key values
			tokens.Add(new KeyValue("Date", "Date"));
			tokens.Add(new KeyValue("MachineName", "MachineName"));
			tokens.Add(new KeyValue("MethodName", "MethodName"));
			tokens.Add(new KeyValue("ExceptionLevel", "ExceptionLevel"));
			tokens.Add(new KeyValue("Message", "Message"));
			tokens.Add(new KeyValue("Data", "Data"));
			tokens.Add(new KeyValue("Detail", "Detail"));
			tokens.Add(new KeyValue("ASP.NET Page", "ASPNET_Page"));
			tokens.Add(new KeyValue("ASP.NET Session", "ASPNET_Session"));
			tokens.Add(new KeyValue("ASP.NET Cookies", "ASPNET_Cookies"));
			tokens.Add(new KeyValue("ASP.NET Application", "ASPNET_Application"));
			tokens.Add(new KeyValue("ASP.NET QueryString", "ASPNET_QueryString"));
			#endregion

			DataTable dt = new DataTable("Log");
			dt.Columns.Add("LogId").AutoIncrement = true;

			foreach (KeyValue kv2 in tokens)
			{
				dt.Columns.Add(kv2.Value);
			}

			string padLine = PadLine;
			KeyValue kv = null;

			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (StreamReader r = new StreamReader(fs))
			{
				while (!r.EndOfStream)
				{
					string buffer = r.ReadLine();

					if (buffer == padLine)
					{
					}

					kv = StartsWithAnyToken(buffer, tokens);

					if (kv == null)
					{
					}
					else
					{
					}
				}
			}

			return dt;
		}

		/// <summary>
		/// Search for individual tokens and return one if found; otherwise, return null.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="tokens"></param>
		/// <returns></returns>
		private KeyValue StartsWithAnyToken(string buffer, List<KeyValue> tokens)
		{
			foreach (KeyValue kv in tokens)
			{
				if (buffer.StartsWith(kv.Key + ":\t"))
					return kv;
			}
			return null;
		}
#endif
		#endregion

		#region AddData method
		/// <summary>
		/// Add the key/value pair (or append if key exists) to the exception's Data instance.
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void AddData(Exception exception, string key, object value)
		{
			try
			{
				if (exception.Data.Contains(key))
				{
					exception.Data[key] = exception.Data[key].ToString() + "; " + value.ToString();
				}
				else
				{
					exception.Data.Add(key, value.ToString());
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString(), "DebuggerTool.AddData");
#if DEBUG
				//throw;
#endif
			}
		}
		#endregion
	}
}
