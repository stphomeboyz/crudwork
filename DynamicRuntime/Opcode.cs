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
using System.Reflection;
using System.Xml.Serialization;

namespace crudwork.DynamicRuntime
{
	/// <summary>
	/// CIL Opcode
	/// </summary>
	public class Opcode
	{
		#region Properties
		/// <summary>
		/// The CIL byte value in Hexadecimal
		/// </summary>
		[XmlAttribute("HexValue")]
		[XmlIgnore]
		public string HexValue
		{
			get;
			set;
		}

		/// <summary>
		/// The secondary CIL byte value in Hexadecimal
		/// </summary>
		[XmlAttribute("HexValue2")]
		[XmlIgnore]
		public string HexValue2
		{
			get;
			set;
		}

		/// <summary>
		/// The CIL byte value
		/// </summary>
		[XmlAttribute("Value")]
		[XmlIgnore]
		public byte Value
		{
			get;
			set;
		}

		/// <summary>
		/// The secondary CIL byte value
		/// </summary>
		[XmlIgnore]
		public byte? Value2
		{
			get;
			set;
		}

		/// <summary>
		/// The CIL opcode instruction
		/// </summary>
		[XmlAttribute("Name")]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Get the CIL bytes
		/// </summary>
		[XmlAttribute("OpValue")]
		public string OpValue
		{
			get
			{
				if (Value2.HasValue)
					return string.Format("{0},{1}", Value, Value2.Value);
				else
					return string.Format("{0}", Value);
			}
			set
			{
				throw new NotImplementedException("the setter for the purpose of serialization only!");
				// do nothing...
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public Opcode()
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="value"></param>
		/// <param name="value2"></param>
		/// <param name="instructionName"></param>
		public Opcode(byte value, byte? value2, string name)
		{
			this.Value = value;
			this.Value2 = value2;
			this.Name = name;
			this.HexValue = value.ToString("X");
			this.HexValue2 = value2.HasValue ? value2.Value.ToString("X") : string.Empty;
		}
		#endregion

		/// <summary>
		/// return string representation of this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Value1={0} Value2={1} Name={2}", Value, Value2, Name);
		}
	}

	/// <summary>
	/// List of Opcode
	/// </summary>
	[XmlTypeAttribute(TypeName = "Opcodes", Namespace = "http://metadata.crudwork.com/DynamicRuntime/Opcodes")]
	public class OpcodeList : List<Opcode>
	{
		/// <summary>
		/// add new item to list
		/// </summary>
		/// <param name="value"></param>
		/// <param name="value2"></param>
		/// <param name="name"></param>
		public void Add(byte value, byte? value2, string name)
		{
			this.Add(new Opcode(value, value2, name));
		}
	}
}
