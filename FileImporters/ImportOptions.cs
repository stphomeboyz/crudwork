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


/* TODO: The enhanced features are not really finish!
 * 
 *		Nice to have but is too complex...
 *		
 *		1) constant(C) where C can be string, int, datetime, etc...
 *		2) compute(rownum|date|time|datetime)
 * 
 * * */
#undef SUPPORT_ENHANCE_FEATURES

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using crudwork.DynamicRuntime;
using crudwork.DataAccess;
using System.IO;
using crudwork.Models.DataAccess;
using crudwork.FileImporters.Specialized;
using System.Xml.Serialization;
using crudwork.Utilities;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace crudwork.FileImporters
{
	#region ImportOptions - Abstract Class
	/// <summary>
	/// ImportOptions - implements strongly-typed class for import / export 
	/// </summary>
	public abstract class ImportOptions
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ImportOptions()
		{
		}

		/// <summary>
		/// Convert this instance to a ConvertOptionList object
		/// </summary>
		/// <returns></returns>
		internal virtual ConverterOptionList ToConverterOptionList()
		{
			return new ConverterOptionList(DynamicCode.GetProperty(this));
		}
	}
	#endregion

	#region OLEDB Import Engines
	/// <summary>
	/// Import Options for Engines using the OLEDB Driver
	/// </summary>
	public abstract class OledbImportOptions : ImportOptions
	{
		/// <summary>Specify the username (if any)</summary>
		public virtual string Username
		{
			get;
			set;
		}
		/// <summary>Specify the password (if any)</summary>
		public virtual string Password
		{
			get;
			set;
		}
		/// <summary>Specify the Tablename for importing or exporting</summary>
		public virtual string Tablename
		{
			get;
			set;
		}
		/// <summary>Specify the QueryFilter to filtering by table</summary>
		public virtual QueryFilter TableFilter
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public OledbImportOptions()
		{
		}
	}

	/// <summary>
	/// Import Options for Access
	/// </summary>
	public class AccessImportOptions : OledbImportOptions
	{
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public AccessImportOptions()
			: base()
		{
			Tablename = string.Empty;
			TableFilter = QueryFilter.None;

			Username = string.Empty;
			Password = string.Empty;
		}
	}

	/// <summary>
	/// Import Options for Excel file
	/// </summary>
	public class ExcelImportOptions : OledbImportOptions
	{
		/// <summary>Indicate whether or not to treat the first row as the header row</summary>
		public bool UseHeader
		{
			get;
			set;
		}
		/// <summary>The provider name</summary>
		public string Provider
		{
			get;
			set;
		}

		#region OpenTableBy features (not yet supported)
		/// <summary>Specify the technique use to open a worksheet (by name or by position)</summary>
		[Obsolete("This feature is not yet implemented", true)]
		internal string OpenTableBy
		{
			get;
			set;
		}

		/// <summary>Specify the table for importing or importing, by its position</summary>
		[Obsolete("This feature is not yet implemented", true)]
		internal int TablePosition
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ExcelImportOptions()
			: base()
		{
			Provider = string.Empty;		// Use Microsoft.ACE.OLEDB.12.0 to support Excel 2007

			#region OpenTableBy description
			// 06/11/2009: The GetOleDbSchemaTable() does not return the ORDINAL_POSITION for table listing
			// therefore this is disabled.  (A nice feature to have... only if had work.)

			/*
			 * 
			 * OpenTableBy can be set to "Name" (by default) or "Position"
			 * 
			 * OpenTableBy = "Name"
			 *		TableName		- the name (or portion) of the table
			 *		TableFilter		- the filter mode
			 * 
			 * OpenTableBy = "Position"
			 *		TablePosition	- the position of the table. This value must be >= 0.  (-1 or empty means disable)
			 *
			 * 
			 * */
			#endregion

			//OpenTableBy = "NAME";
			Tablename = string.Empty;
			TableFilter = QueryFilter.None;

			//TablePosition = -1;

			UseHeader = true;
			Username = string.Empty;
			Password = string.Empty;
		}
	}
	#endregion

	#region dBase 4 Engine
	/// <summary>
	/// The list of engines for importing / exporting DBF files
	/// </summary>
	public enum DBFEngine
	{
		/// <summary>DBF 4 Engine</summary>
		DBF4,
		/// <summary>OLEDB</summary>
		OLEDB,
	}

	/// <summary>
	/// Import Options for DBF File
	/// </summary>
	public class DBFImportOptions : ImportOptions
	{
		/// <summary>Specify the engine used for importing data</summary>
		public DBFEngine ImportEngine
		{
			get;
			set;
		}
		/// <summary>Specify the engine used for exporting data</summary>
		public DBFEngine ExportEngine
		{
			get;
			set;
		}
		/// <summary>Import a maximum row count</summary>
		public int ImportMaxRows
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public DBFImportOptions()
		{
			ImportEngine = DBFEngine.OLEDB;
			//ImportEngine = DBFEngine.DBF4;

			ExportEngine = DBFEngine.DBF4;

			ImportMaxRows = 0;					// implement 'select TOP 20 * from table' rows.
		}
	}
	#endregion

	#region Delimiter Engines (CSV/TAB)
	/// <summary>
	/// Import Options for delimited files
	/// </summary>
	public class DelimiterImportOptions : ImportOptions
	{
		/// <summary>Indicate whether or not to treat the first row as the header row</summary>
		public bool UseHeader
		{
			get;
			set;
		}
		/// <summary>Indicate whether or not to use the Text Qualifier Id</summary>
		public bool UseTextQualifier
		{
			get;
			set;
		}
		/// <summary>The character used to enclose data</summary>
		public string TextQualifierId
		{
			get;
			set;
		}
		/// <summary>The delimiter character to separate the fields</summary>
		public string Delimiter
		{
			get;
			internal set;
		}
		/// <summary>Specify a 'newline' characters (DOS/Windows use '\r\n'; Unix uses '\n'; Mac uses '\r')</summary>
		public string NewlineChar
		{
			get;
			set;
		}
		/// <summary>Import a maximum row count</summary>
		public int ImportMaxRows
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public DelimiterImportOptions()
			: this(null)
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="delimiter"></param>
		public DelimiterImportOptions(string delimiter)
		{
			UseHeader = true;
			UseTextQualifier = true;
			TextQualifierId = "\"";
			Delimiter = delimiter;
			NewlineChar = Environment.NewLine;		// to support unix o/s
			ImportMaxRows = 0;						// implement 'select TOP 20 * from table' rows.
		}
	}
	#endregion

	#region XML Engine
	/// <summary>
	/// Import Options for XML
	/// </summary>
	public class XmlImportOptions : ImportOptions
	{
		/// <summary>Specify the XmlWriteMode (for exporting data to XML file)</summary>
		public XmlWriteMode UseXmlWriteMode
		{
			get;
			set;
		}
		/// <summary>Specify the MappingType (for exporting data to XML file)</summary>
		public MappingType UseMappingType
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public XmlImportOptions()
		{
			UseXmlWriteMode = XmlWriteMode.WriteSchema;
			UseMappingType = MappingType.Element;
		}
	}
	#endregion

	#region Fixed-Length File Engine
	/// <summary>
	/// Field Spec - defines a field information (fieldname, type, start, length, etc...)
	/// </summary>
	public class FieldSpec
	{
		#region Properties
		/// <summary>Get or set the field name</summary>
		//[XmlAttribute(AttributeName="Name")]
		public string Name
		{
			get;
			set;
		}
		/// <summary>Get or set the field type</summary>
		[XmlIgnore]
		public Type Type
		{
			get;
			set;
		}

		/// <summary>Get or set the field type (via string value)</summary>
		//[XmlAttribute(AttributeName = "TypeName")]
		public string TypeName
		{
			get
			{
				return Type.FullName;
			}
			set
			{
				try
				{
					Type = DataConvert.ToType(value);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					throw;
				}
			}
		}

		/// <summary>Get or set the field's starting position</summary>
		//[XmlAttribute(AttributeName = "Start")]
		public int Start
		{
			get;
			set;
		}
		/// <summary>Get or set the field's length</summary>
		//[XmlAttribute(AttributeName = "Length")]
		public int Length
		{
			get;
			set;
		}

#if SUPPORT_ENHANCE_FEATURES
		/// <summary>Get or set the field's default value</summary>
		//[XmlAttribute(AttributeName = "Value")]
		public object Value
		{
			get;
			set;
		}

		/// <summary>Enable or disable the auto compute feature</summary>
		public bool Compute
		{
			get;
			set;
		}
#endif
		#endregion

		#region Constructors
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public FieldSpec()
		{
		}
#if SUPPORT_ENHANCE_FEATURES
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="value"></param>
		public FieldSpec(string name, Type type, int start, int length, object value)
		{
			this.Name = name;
			this.Type = type;
			this.Start = start;
			this.Length = length;
			this.Value = value;
		}
#else
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		public FieldSpec(string name, Type type, int start, int length)
		{
			this.Name = name;
			this.Type = type;
			this.Start = start;
			this.Length = length;
		}
#endif
		#endregion

		/// <summary>
		/// return a string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
#if SUPPORT_ENHANCE_FEATURES
			return string.Format("Name={0} Type={1} Start={2} Length={3} Value={4}", Name, Type, Start, Length, Value);
#else
			return string.Format("Name={0} Type={1} Start={2} Length={3}", Name, Type, Start, Length);
#endif
		}

		/// <summary>
		/// return a field spec
		/// </summary>
		/// <returns></returns>
		public string ToSpec()
		{
#if SUPPORT_ENHANCE_FEATURES
			if (Value != null)
			{
				string v = (Type == typeof(string)) ? "\"" + Value + "\"" : Value.ToString();
				return string.Format("= constant({0})", v);
			}
			else
			{
				return string.Format("= position({0},{1})", Start, Length);
			}
#else
			return string.Format("= position({0},{1})", Start, Length);
#endif
		}

		/// <summary>
		/// Parse a spec string and return a FieldSpec object
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static FieldSpec ToFieldSpec(string item)
		{
			//item = item.Trim(' ', '\t');
			var regOpt = RegexOptions.Singleline | RegexOptions.IgnoreCase;

			var m = Regex.Match(item, @"^(?<Type>\w+)\s+(?<Name>\w+)\s+=\s+(?<Spec>.+)$", regOpt);
			if (!m.Success)
				return null;

			var result = new FieldSpec();

			result.Name = m.Groups["Name"].Value;
			result.TypeName = m.Groups["Type"].Value;
#if SUPPORT_ENHANCE_FEATURES
			result.Compute = false;		// auto-compute is disabled, by default.
#endif
			string spec = m.Groups["Spec"].Value;

			if (spec.StartsWith("position", StringComparison.InvariantCultureIgnoreCase))
			{
				m = Regex.Match(spec, @"^position\((?<Start>\d+),(?<Length>\d+)\)$", regOpt);
				if (!m.Success)
					throw new ArgumentException("Unrecognized spec: " + spec);

				result.Start = DataConvert.ToInt32(m.Groups["Start"].Value);
				result.Length = DataConvert.ToInt32(m.Groups["Length"].Value);
			}
#if SUPPORT_ENHANCE_FEATURES
			else if (spec.StartsWith("constant", StringComparison.InvariantCultureIgnoreCase))
			{
				m = Regex.Match(spec, @"^constant\((?<Value>.+)\)$", regOpt);
				if (!m.Success)
					throw new ArgumentException("Unrecognized spec: " + spec);

				var v = StringUtil.Unquote(m.Groups["Value"].Value);
	
				result.Start = -1;
				result.Length = v.Length;
				result.Value = v;
			}
			else if (spec.StartsWith("compute", StringComparison.InvariantCultureIgnoreCase))
			{
				m = Regex.Match(spec, @"compute\((?<Action>.+)\)$", regOpt);
				if (!m.Success)
					throw new ArgumentException("Unrecognized spec: " + spec);

				result.Start = -1;
				result.Length = -1;
				result.Compute = true;
				result.Value = m.Groups["Action"].Value;
			}
#endif
			else
			{
				throw new ArgumentException("invalid field specifier: " + spec);
			}

			return result;
		}
	}

	/// <summary>
	/// List of Field Spec
	/// </summary>
	[XmlTypeAttribute(TypeName = "FieldSpecList")]
	public class FieldSpecList : List<FieldSpec>
	{
		private int? recordLength = null;

		#region Constructors
		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public FieldSpecList()
		{
		}
		#endregion

		/// <summary>
		/// add new entry to list
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		public void Add(string name, Type type, int start, int length)
		{
			if (start == 0)
				start = RecordLength + 1;

			base.Add(new FieldSpec()
			{
				Name = name,
				Type = type,
				Start = start,
				Length = length,
			});
		}

#if SUPPORT_ENHANCE_FEATURES
		/// <summary>
		/// add new entry to list
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="value"></param>
		public void Add(string name, Type type, int start, int length, object value)
		{
			if (start == 0)
				start = RecordLength + 1;

			base.Add(new FieldSpec()
			{
				Name = name,
				Type = type,
				Start = start,
				Length = length,
				Value = value,
			});
		}
#endif

		/// <summary>
		/// get the record length
		/// </summary>
		public int RecordLength
		{
			get
			{
				if (recordLength.HasValue)
					return recordLength.Value;

				// otherwise attempt to compute based on the list of defined field spec
				// NOTE: the filespec must be complete!
				int length = 0;
				for (int i = 0; i < this.Count; i++)
				{
					length += this[i].Length;
				}
				return length;
			}
			set
			{
				recordLength = value;
			}
		}

		/// <summary>
		/// Read the line and get the record length.  Return null if not applicable; otherwise return the given value
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static int? ReadRecordLength(string line)
		{
			var regex = @"^set\s+RecordLength\s+=\s+(?<RecordLength>[0-9]+)$";

			Match m = Regex.Match(line, regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			if (m == null || !m.Success)
				return null;

			return DataConvert.ToInt32(m.Groups["RecordLength"].Value);
		}

		/// <summary>
		/// return an SetRecordLength entry
		/// </summary>
		/// <returns></returns>
		public string ToRecordLength()
		{
			return "set RecordLength = " + RecordLength;
		}

		/// <summary>
		/// The string-based index of the Field spec
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public FieldSpec this[string index]
		{
			get
			{
				foreach (var item in this)
				{
					if (item.Name.Equals(index, StringComparison.InvariantCultureIgnoreCase))
						return item;
				}

				throw new ArgumentOutOfRangeException("index=" + index);
			}
		}
	}

	/// <summary>
	/// Definition Format Type
	/// </summary>
	public enum DefinitionFormat
	{
		/// <summary>Text Format</summary>
		Text,
		/// <summary>XML Format</summary>
		XML,
	}

	/// <summary>
	/// Import Options for Fixed-Length File
	/// </summary>
	public class FixedLengthImportOptions : ImportOptions
	{
		/// <summary>Get or set the field spec list</summary>
		public FieldSpecList Definition
		{
			get;
			set;
		}

		/// <summary>Get the record length</summary>
		public int RecordLength
		{
			get
			{
				return Definition == null ? -1 : Definition.RecordLength;
			}
			set
			{
			}
		}

		/// <summary>Specify a 'newline' characters (DOS/Windows use '\r\n'; Unix uses '\n'; Mac uses '\r')</summary>
		public string NewlineChar
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public FixedLengthImportOptions()
		{
			Definition = new FieldSpecList();
		}

		/// <summary>
		/// Read the Definition from file
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public void ReadDefinition(string filename, DefinitionFormat format)
		{
			Definition = FixedLengthFileReader.ReadSchema(filename, format);
		}

		/// <summary>
		/// Write the Definition to file
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public void WriteDefinition(string filename, DefinitionFormat format)
		{
			FixedLengthFileReader.WriteSchema(Definition, filename, format);
		}
	}
	#endregion
}
