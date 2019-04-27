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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
//using System.ServiceModel;
using System.Xml;
using crudwork.Models;

namespace crudwork.Utilities
{
	/// <summary>
	/// Serialize Utility
	/// </summary>
	public static partial class Serializer
	{
		/// <summary>
		/// The serialize method
		/// </summary>
		public enum SerializeMethods
		{
			/// <summary>SoapFormatter (IFormatter)</summary>
			Soap = 1,

			/// <summary>BinaryFormatter (IFormatter)</summary>
			Binary = 2,

			/// <summary>XmlSerializer</summary>
			Xml = 3,

			/// <summary>NetDataContractSerializer</summary>
			NetDataContract = 4,

			/// <summary>XmlDataContractSerializer</summary>
			XmlDataContract = 5,

			/// <summary>JSON (WCF System.Runtime.Serialization.Json.DataContractJsonSerializer)</summary>
			Json = 6,
		}

		/// <summary>
		/// Serialize an object, using XmlSerializer.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private static string XmlSerialize(object obj)
		{
			//    StringBuilder s = new StringBuilder();
			//    using (StringWriter sw = new StringWriter(s))
			//    {
			//        XmlSerializer xs = new XmlSerializer(obj.GetType());
			//        xs.Serialize(sw, obj);
			//        sw.Close();
			//    }
			//    return s.ToString();

			StringBuilder s = new StringBuilder();

			// http://www.csharper.net/blog/serializing_without_the_namespace__xmlns__xmlns_xsd__xmlns_xsi_.aspx
			// http://devfuel.blogspot.com/2007/03/xmlserializer-now-with-less-xmlnsxsi.html

			// do not output these attributes:
			//		xmlns:xsd="http://www.w3.org/2001/XMLSchema
			//		xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			//ns.Add("xmlns", "http://www.blah.com");			// Prefix "xmlns" is reserved for use by XML

			// do not output this tag:
			//		<?xml version="1.0" encoding="utf-8"?>
			XmlWriterSettings xwSettings = new XmlWriterSettings();
			xwSettings.OmitXmlDeclaration = true;
			xwSettings.Indent = true;
			xwSettings.CloseOutput = true;
			XmlWriter xmlWriter = XmlWriter.Create(s, xwSettings);

			using (StringWriter sw = new StringWriter(s))
			{
				XmlSerializer xs = new XmlSerializer(obj.GetType());
				xs.Serialize(xmlWriter, obj, ns);
				sw.Close();
			}

			return s.ToString();
		}

		/// <summary>
		/// Deserialize an object, using XmlSerializer.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static T XmlDeserialize<T>(string value)
		{
			using (StringReader sr = new StringReader(value))
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				return (T)xs.Deserialize(sr);
			}
		}

		/// <summary>
		/// Deserialize an object, using XmlSerializer
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static T XmlDeserialize<T>(byte[] value)
		{
			return XmlDeserialize<T>(Encoding.ASCII.GetString(value));
		}

		/// <summary>
		/// Create a new instance of the IFormatter class.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		private static IFormatter CreateFormatter(SerializeMethods method)
		{
			switch (method)
			{
				case SerializeMethods.Binary:
					return new BinaryFormatter();
				case SerializeMethods.Soap:
					return new SoapFormatter();
				//// TODO: this stop working on clr 4.0
				//case SerializeMethods.NetDataContract:
				//	return new NetDataContractSerializer();
				case SerializeMethods.XmlDataContract:
					throw new NotImplementedException("XmlDataContract not available");
				//return new XmlDataContractSerializer();
				case SerializeMethods.Json:
					throw new NotImplementedException("json not available");
				//return new XmlObjectSerializer();
				//return new DataContractJsonSerializer();
				default:
					throw new ArgumentOutOfRangeException("method=" + method);
			}
		}

		/// <summary>
		/// Serialize an object, using SoapFormatter.
		/// </summary>
		/// <param name="theObject"></param>
		/// <returns></returns>
		public static string Serialize(object theObject)
		{
			return Serialize(theObject, SerializeMethods.Soap);
		}

		/// <summary>
		/// Serialize an object using the specified formatter.
		/// </summary>
		/// <param name="theObject"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static string Serialize(object theObject, SerializeMethods method)
		{
			if (theObject == null)
				return string.Empty;

			if (method == SerializeMethods.Xml)
				return XmlSerialize(theObject);

			using (MemoryStream stream = new MemoryStream())
			{
				IFormatter formatter = CreateFormatter(method);
				formatter.Serialize(stream, theObject);
				byte[] results = stream.ToArray();
				return Encoding.ASCII.GetString(results);
			}
		}

		/// <summary>
		/// Deserialize an object, using SoapFormatter.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Deserialize<T>(byte[] value)
		{
			return Deserialize<T>(value, SerializeMethods.Soap);
		}

		/// <summary>
		/// Deserialize the byte array value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static T Deserialize<T>(byte[] value, SerializeMethods method)
		{
			if (value == null || value.Length == 0)
				return default(T);

			if (method == SerializeMethods.Xml)
				return XmlDeserialize<T>(value);

			using (MemoryStream stream = new MemoryStream(value))
			{
				stream.Seek(0, SeekOrigin.Begin);
				IFormatter formatter = CreateFormatter(method);
				return (T)formatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Deserialize the string value, using SoapFormatter.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Deserialize<T>(string value)
		{
			return Deserialize<T>(value, SerializeMethods.Soap);
		}

		/// <summary>
		/// Deserialize the string value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static T Deserialize<T>(string value, SerializeMethods method)
		{
			if (string.IsNullOrEmpty(value))
				return default(T);

			if (method == SerializeMethods.Xml)
				return XmlDeserialize<T>(value);

			byte[] input = Encoding.ASCII.GetBytes(value);
			return Deserialize<T>(input, method);
		}
	}
}
