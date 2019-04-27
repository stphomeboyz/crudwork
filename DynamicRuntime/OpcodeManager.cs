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
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace crudwork.DynamicRuntime
{
	/// <summary>
	/// Opcode Manager
	/// </summary>
	public static class OpcodeManager
	{
		/// <summary>Opcode extender id (0xFE or 254) indicates a two-bytes instruction code</summary>
		private const byte OPCODE_EXTENDER = 0xFE;
		public static OpcodeList OpcodeList
		{
			get;
			private set;
		}

		#region Constructors
		static OpcodeManager()
		{
			OpcodeList = GetOpcodeList();
		}
		#endregion

		#region Static methods - for loading opcode from resource file
		public static OpcodeList GetOpcodeList()
		{
			var results = new OpcodeList();

			using (var ds = ReadOpcodeList("crudwork.DynamicRuntime.Resources.Opcodes.xml"))
			using (var dt = ds.Tables[0])
			{
				foreach (DataRow dr in dt.Rows)
				{
					string name = dr["Name"].ToString();
					string opValue = dr["OpValue"].ToString();
					byte[] bytes = ToByteArray(opValue.Split(','));

					#region Sanity Checks
					if (string.IsNullOrEmpty(name))
						throw new ArgumentNullException("name cannot be null", "name");
					if (string.IsNullOrEmpty(opValue))
						throw new ArgumentNullException("OpValue cannot be null", "OpValue");
					#endregion

					results.Add(new Opcode(bytes[0], bytes.Length == 2 ? bytes[1] : (byte?)null, name));
				}
			}

			Debug.Assert(results.Count == 219, "Expecting 218 opcodes total; but found " + results.Count);
			return results;
		}
		private static byte[] ToByteArray(string[] values)
		{
			byte[] results = new byte[values.Length];

			for (int i = 0; i < values.Length; i++)
			{
				results[i] = byte.Parse(values[i]);
			}
			return results;
		}
		public static OpcodeList GetOpcodeListXX()
		{
			var results = new OpcodeList();

			using (var ds = ReadOpcodeList("crudwork.DynamicRuntime.Resources.Opcodes.xml"))
			using (var dt = ds.Tables[0])
			{
				foreach (DataRow dr in dt.Rows)
				{
					byte? value1 = FromHex(dr["HexValue"]);
					byte? value2 = FromHex(dr["HexValue2"]);
					string name = dr["Name"].ToString();

					#region Sanity Checks
					if (!value1.HasValue)
						throw new ArgumentNullException("value1", "value1 cannot be null");
					if (string.IsNullOrEmpty(name))
						throw new ArgumentNullException("name", "name cannot be null");
					#endregion

					results.Add(new Opcode(value1.Value, value2, name));
				}
			}

			Debug.Assert(results.Count == 219, "Expecting 218 opcodes total; but found " + results.Count);
			return results;
		}
		private static byte? FromHex(object value)
		{
			if (value == null)
				return null;
			string sValue = value.ToString();
			if (string.IsNullOrEmpty(sValue))
				return null;

			if (sValue.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
				return byte.Parse(sValue.Substring(2), System.Globalization.NumberStyles.HexNumber);
			return byte.Parse(sValue, System.Globalization.NumberStyles.HexNumber);
		}
		private static DataSet ReadOpcodeList(string key)
		{
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(key))
			{
				var ds = new DataSet("Opcodes");
				ds.ReadXml(s);
				return ds;
			}
		}
		#endregion

		#region Public static methods
		/// <summary>
		/// Return the Opcode for the one-byte instruction code
		/// </summary>
		/// <param name="value1"></param>
		/// <returns></returns>
		public static Opcode GetOpcode(byte value1)
		{
			for (int i = 0; i < OpcodeList.Count; i++)
			{
				var item = OpcodeList[i];
				if (item.Value == value1 && !item.Value2.HasValue)
					return item;
			}

			throw new IndexOutOfRangeException(
				string.Format("index not found: value1={0}", value1));
		}

		/// <summary>
		/// Return the Opcode for the two-byte instruction code
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static Opcode GetOpcode(byte value1, byte value2)
		{
			for (int i = 0; i < OpcodeList.Count; i++)
			{
				var item = OpcodeList[i];
				if (item.Value == value1 && (item.Value2.HasValue && item.Value2.Value == value2))
					return item;
			}
			throw new IndexOutOfRangeException(
				string.Format("index not found: value1={0} value2={1}", value1, value2));
		}

		/// <summary>
		/// convert binary CIL instructions into human-readable instructions
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string[] ConvertToCIL(byte[] bytes)
		{
			var results = new List<string>();
			Opcode opcode = null;

			for (int i = 0; i < bytes.Length; i++)
			{
				byte v1 = bytes[i];

				try
				{
					if (v1 == OPCODE_EXTENDER)
					{
						v1 = bytes[++i];
						opcode = GetOpcode(OPCODE_EXTENDER, v1);
					}
					else
					{
						opcode = GetOpcode(v1);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					throw;
				}

				results.Add(opcode.Name);
			}

			return results.ToArray();
		}
		#endregion
	}
}
