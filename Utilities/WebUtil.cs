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
using System.Net;
using System.Text;
using System.IO;

namespace crudwork.Utilities
{
	/// <summary>
	/// Web Utility
	/// </summary>
	public static class WebUtil
	{
		/// <summary>
		/// Retrieve a web page using GET method
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string[] GetURL(string url)
		{
			try
			{
				WebRequest request = HttpWebRequest.Create(new Uri(url));
				WebResponse response = request.GetResponse();
				return FileUtil.ReadStream(response.GetResponseStream());
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "url", url);
				throw;
			}
		}

		/// <summary>
		/// Retrieve a web page using POST method
		/// </summary>
		/// <param name="url"></param>
		/// <param name="contentType"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string PostURL(string url, string contentType, string parameters)
		{
			try
			{
				WebRequest request = HttpWebRequest.Create(new Uri(url));
				request.Method = "POST";
				request.ContentType = contentType;
				request.ContentLength = parameters.Length;
				FileUtil.WriteStream(request.GetRequestStream(), parameters);
				WebResponse response = request.GetResponse();
				return StringUtil.StringArrayToString(FileUtil.ReadStream(response.GetResponseStream()), Environment.NewLine);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "url", url);
				DebuggerTool.AddData(ex, "parameters", parameters);
				throw;
			}
		}

		/// <summary>
		/// Retrieve a web page using POST method
		/// </summary>
		/// <param name="url"></param>
		/// <param name="contentType"></param>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string PostURL(string url, string contentType, Stream stream)
		{
			return PostURL(url, contentType, new StreamReader(stream).ReadToEnd());
		}

		/// <summary>
		/// Retrieve a web page using POST method
		/// </summary>
		/// <param name="url"></param>
		/// <param name="contentType"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string PostURL(string url, string contentType, params string[] parameters)
		{
			return PostURL(url, contentType, StringUtil.StringArrayToString(parameters, "&"));
		}

		/// <summary>
		/// Retrieve a web page using POST method(without specifying any content type)
		/// </summary>
		/// <param name="url"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string PostURL(string url, string parameters)
		{
			return PostURL(url, string.Empty, parameters);
		}

		/// <summary>
		/// Retrieve a web page using POST method(without specifying any content type)
		/// </summary>
		/// <param name="url"></param>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string PostURL(string url, Stream stream)
		{
			return PostURL(url, string.Empty, stream);
		}

		/// <summary>
		/// Retrieve a web page using POST method (without specifying any content type)
		/// </summary>
		/// <param name="url"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string PostURL(string url, params string[] parameters)
		{
			return PostURL(url, string.Empty, parameters);
		}
	}
}
