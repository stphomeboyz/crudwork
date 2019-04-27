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

//#define SUPPORT_BACKLINK_SECTION
//#define CREATE_RECORD_DELETED_COLUMN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using crudwork.Utilities;
using System.Data;
using crudwork.FileImporters.Tool;
using System.Diagnostics;

namespace crudwork.FileImporters.Specialized
{
	/// <summary>
	/// the dBase implementation based on format --> http://www.dbf2002.com/dbf-file-format.html
	/// </summary>
	internal class dBase4
	{
		#region Constants
		/// <summary>Header record terminator id (0x0D == '\r')</summary>
		private const byte HEADER_REC_TERM = 0x0D;
		/// <summary>End of File terminator id (0x1A == Ctrl-Z)</summary>
		private const byte EOF_TERM = 0x1A;
		/// <summary>Record Deleted id (0x2A == '*')</summary>
		private const byte RECORD_DELETED_FLAG = 0x2A;
#if CREATE_RECORD_DELETED_COLUMN
		/// <summary>a non-user field to store Row Deleted character</summary>
		/// <remarks>This name width cannot exceed 11 char!</remarks>
		private const string RECORD_DELETED_FIELDNAME = "__DBFRowDel";
#endif
		/// <summary>a byte in DBF has a maximum value of 254; and, not 255 like in CLR.  Need to distinguish this</summary>
		private const byte BYTE_MAX_VALUE = 254;

		/// <summary>The length of the header section</summary>
		private const byte HEADER_LENGTH = 32;
		/// <summary>The length of a subfield section</summary>
		private const byte SUBFIELD_LENGTH = 32;
		#endregion

		#region Fields / Properties
		private DBFImportOptions Options;

		/// <summary>
		/// The DBF header information
		/// </summary>
		private DbfHeader Header;

		/// <summary>
		/// the DataTable equivalent
		/// </summary>
		public DataTable DataTable
		{
			get;
			private set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// create a new instance with default attributes
		/// </summary>
		public dBase4()
			: this(new DBFImportOptions())
		{
		}

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="options"></param>
		public dBase4(DBFImportOptions options)
		{
			this.Options = options;
		}
		#endregion

		#region Structs
		/// <summary>
		/// DBF Header structure
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		[Serializable]
		private unsafe struct DbfHeader
		{
			private const int UPDATEDATE_MAX = 3;
			private const int RESERVED_MAX = 16;
			private const int RESERVED2_MAX = 2;

			#region Fields
			/// <summary>File Type</summary>
			internal DbfType _Type;
			/// <summary>Last update (YYMMDD)</summary>
			internal fixed byte _UpdateDate[UPDATEDATE_MAX];
			/// <summary>Number of records in file</summary>
			internal Int32 _RecordCount;
			/// <summary>Position of first data record</summary>
			internal Int16 _FirstRecordOffset;
			/// <summary>Length of one data record, including delete flag</summary>
			internal Int16 _OneRecordLength;
			/// <summary>Reserved</summary>
			internal fixed byte _Reserved[RESERVED_MAX];
			/// <summary>Table flags</summary>
			internal TableFlag _Flag;
			/// <summary>Code page mark</summary>
			internal byte _CodePageMark;
			/// <summary>Reserved, contains 0x00</summary>
			internal fixed byte _Reserved2[RESERVED2_MAX];
			/// <summary>Field subrecords</summary>
			internal List<DbfSubrecord> _SubRecords
			{
				get;
				set;
			}
			/// <summary>Header record terminator (0x0D)</summary>
			internal byte _HeaderRecordTerminator;
#if SUPPORT_BACKLINK_SECTION
			/// <summary>A 263-byte range that contains the backlink, which is the relative path of an associated database (.dbc) file, information. If the first byte is 0x00, the file is not associated with a database. Therefore, database files always contain 0x00.</summary>
			internal fixed byte _Backlink[263];
#endif
			#endregion

			#region Methods
			/// <summary>
			/// return the UpdateDate field as a DateTime instance
			/// </summary>
			/// <returns></returns>
			public unsafe DateTime UpdateDate
			{
				get
				{
					fixed (byte* ptr = _UpdateDate)
					{
						byte* p = ptr;

						byte yy = *p++;
						byte mm = *p++;
						byte dd = *p++;
						return new DateTime(Common.Y2K(yy), mm, dd);
					}
				}
				set
				{
					byte yy = (byte)(value.Year % 100);
					byte mm = (byte)(value.Month);
					byte dd = (byte)(value.Day);

					fixed (byte* ptr = _UpdateDate)
					{
						byte* p = ptr;

						*p++ = yy;
						*p++ = mm;
						*p++ = dd;
					}
				}
			}

			/// <summary>
			/// return a string presentation of this instance
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return string.Format("Type={0} UpdateDate={1} RecordCount={2} FirstRecordOffset={3} OneRecordLength={4}",
					_Type, UpdateDate, _RecordCount, _FirstRecordOffset, _OneRecordLength);
			}

			/// <summary>
			/// return a byte array presentation of this instance
			/// </summary>
			/// <returns></returns>
			public byte[] ToByteArray()
			{
				List<byte> results = new List<byte>();

				results.Add((byte)_Type);

				fixed (byte* ptr = _UpdateDate)
				{
					results.AddRange(Common.ToBytes(ptr, UPDATEDATE_MAX));
				}

				results.AddRange(Common.ToBytes(_RecordCount));
				results.AddRange(Common.ToBytes(_FirstRecordOffset));
				results.AddRange(Common.ToBytes(_OneRecordLength));

				fixed (byte* ptr = _Reserved)
				{
					results.AddRange(Common.ToBytes(ptr, RESERVED_MAX));
				}

				results.Add((byte)_Flag);
				results.Add((byte)_CodePageMark);

				fixed (byte* ptr = _Reserved2)
				{
					results.AddRange(Common.ToBytes(ptr, RESERVED2_MAX));
				}

				foreach (var item in _SubRecords)
				{
					results.AddRange(item.ToByteArray());
				}

				results.Add(_HeaderRecordTerminator);

				//if ((results.Count - 1) % 32 != 0)
				//	throw new ArgumentException("Expected size is NOT an even chunks of 32 bytes; [size=" + results.Count + "]");

				if (results.Count != this._FirstRecordOffset)
					throw new ArgumentException("header size not correct");

				return results.ToArray();
			}
			#endregion
		}

		/// <summary>
		/// Field Subrecords Structure
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		[Serializable]
		private unsafe struct DbfSubrecord
		{
			private const int FIELDNAME_MAX = 11;
			private const int DISPLACEMENT_MAX = 4;
			private const int RESERVED_MAX = 8;

			#region Fields
			/// <summary>Field name with a maximum of 10 characters. If less than 10, it is padded with null characters (0x00).</summary>
			internal fixed byte _Fieldname[FIELDNAME_MAX];
			/// <summary>Field type</summary>
			internal FieldType _Type;
			/// <summary>Displacement of field in record</summary>
			internal fixed byte _Displacement[DISPLACEMENT_MAX];
			/// <summary>Length of field</summary>
			internal byte _Length;
			/// <summary>Number of decimal places</summary>
			internal byte _DecimalPrecision;
			/// <summary>Field flags</summary>
			internal FieldFlag _Flags;
			/// <summary>Value of autoincrement Next value</summary>
			internal Int32 _Next;
			/// <summary>Value of autoincrement Step value</summary>
			internal byte _Step;
			/// <summary>Reserved</summary>
			internal fixed byte _Reserved[RESERVED_MAX];
			#endregion

			#region Methods
			/// <summary>
			/// return the fieldname field as a string-type
			/// </summary>
			public string Fieldname
			{
				get
				{
					fixed (byte* ptr = _Fieldname)
					{
						return Common.ByteArrayToString(ptr, 0, FIELDNAME_MAX);
					}
				}
				set
				{
					byte[] bytes = Common.StringToByteArray(value, 0, FIELDNAME_MAX);
					fixed (byte* ptr = _Fieldname)
					{
						Common.UnsafeCopy(bytes, 0, bytes.Length, ptr, FIELDNAME_MAX);
					}
				}
			}

			/// <summary>
			/// Get the data type
			/// </summary>
			/// <returns></returns>
			public Type GetDataType()
			{
				switch (_Type)
				{
					case FieldType.Character:
						//case FieldType.Character_Binary:
						return typeof(string);
					case FieldType.Currency:
						return typeof(decimal);
					case FieldType.Numeric:
						return typeof(int);
					case FieldType.Float:
						return typeof(float);
					case FieldType.Date:
						return typeof(DateTime);
					case FieldType.DateTime:
						return typeof(DateTime);
					case FieldType.Double:
						return typeof(double);
					case FieldType.Integer:
						return typeof(int);
					case FieldType.Logical:
						return typeof(bool);
					case FieldType.Memo:
						//case FieldType.Memo_Binary:
						return typeof(object);
					case FieldType.General:
						return typeof(string);
					case FieldType.Picture:
						return typeof(object);

					default:
						throw new ArgumentOutOfRangeException("Type=" + _Type);
				}
			}

			/// <summary>
			/// return a string presentation of this instance
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return string.Format("Name={0} Type={1} Length={2} Decimal={3} Next={4} Step={5}",
					Fieldname, _Type, _Length, _DecimalPrecision, _Next, _Step);
			}

			/// <summary>
			/// return a byte array presentation of this instance
			/// </summary>
			/// <returns></returns>
			public byte[] ToByteArray()
			{
				List<byte> results = new List<byte>();

				fixed (byte* ptr = _Fieldname)
					results.AddRange(Common.ToBytes(ptr, FIELDNAME_MAX));

				results.Add((byte)_Type);

				fixed (byte* ptr = _Displacement)
					results.AddRange(Common.ToBytes(ptr, DISPLACEMENT_MAX));

				results.Add(_Length);
				results.Add(_DecimalPrecision);
				results.Add((byte)_Flags);
				results.AddRange(Common.ToBytes(_Next));
				results.Add(_Step);

				fixed (byte* ptr = _Reserved)
					results.AddRange(Common.ToBytes(ptr, RESERVED_MAX));

				return results.ToArray();
			}
			#endregion
		}
		#endregion

		#region Enums
		/// <summary>
		/// DBF type/version
		/// </summary>
		private enum DbfType : byte
		{
			/// <summary>FoxBASE</summary>
			FoxBASE = 0x02,
			/// <summary>FoxBASE+/Dbase III plus, no memo</summary>
			FoxBasePlus = 0x03,
			/// <summary>Visual FoxPro</summary>
			VisualFoxPro = 0x30,
			/// <summary>Visual FoxPro, autoincrement enabled</summary>
			VisualFoxProAutoIncremental = 0x31,
			/// <summary>Visual FoxPro with field type Varchar or Varbinary</summary>
			VisualFoxProVarchar = 0x32,
			/// <summary>dBASE IV SQL table files, no memo</summary>
			dBaseIVSql = 0x43,
			/// <summary>dBASE IV SQL system files, no memo</summary>
			dBaseIVSqlSystem = 0x63,
			/// <summary>FoxBASE+/dBASE III PLUS, with memo</summary>
			FoxBaseMemo = 0x83,
			/// <summary>dBASE IV with memo</summary>
			dBaseIVMemo = 0x8B,
			/// <summary>dBASE IV SQL table files, with memo</summary>
			dBaseIVSqlMemo = 0xCB,
			/// <summary>FoxPro 2.x (or earlier) with memo</summary>
			FoxPro2 = 0xF5,
			/// <summary>HiPer-Six format with SMT memo file</summary>
			HiPerSix = 0xE5,
			/// <summary>FoxBASE</summary>
			FoxBase = 0xFB,
		}

		/// <summary>
		/// Table flags
		/// </summary>
		private enum TableFlag : byte
		{
			/// <summary>file has a structural .cdx</summary>
			HasStructural = 0x01,
			/// <summary>file has a Memo field</summary>
			HasMemo = 0x02,
			/// <summary>file is a database (.dbc)</summary>
			IsDatabase = 0x04,
		}

		/// <summary>
		/// Field type
		/// </summary>
		private enum FieldType : byte
		{
			/// <summary>Character - C</summary>
			Character = 0x43, //C
			/// <summary>Currency - Y</summary>
			Currency = 0x59, //Y
			/// <summary>Numeric - N</summary>
			Numeric = 0x4E, //N
			/// <summary>Float- F</summary>
			Float = 0x46, //F
			/// <summary>Date - D</summary>
			Date = 0x44, //D
			/// <summary>DateTime - T</summary>
			DateTime = 0x54, //T
			/// <summary>Double - B</summary>
			Double = 0x42, //B
			/// <summary>Integer - I</summary>
			Integer = 0x49, //I
			/// <summary>Logical - L</summary>
			Logical = 0x4C, //L
			/// <summary>Memo - M</summary>
			Memo = 0x4D, //M
			/// <summary>General - G</summary>
			General = 0x47, //G
			/// <summary>Character (Binary) - C</summary>
			Character_Binary = 0x43, //C
			/// <summary>Memo (Binary) - M</summary>
			Memo_Binary = 0x4D, //M
			/// <summary>Picture - P</summary>
			Picture = 0x50, //P
		}

		/// <summary>
		/// Field Flag
		/// </summary>
		private enum FieldFlag : byte
		{
			/// <summary>Normal column</summary>
			NormalColumn = 0x00,
			/// <summary>System Column (not visible to user)</summary>
			SystemColumn = 0x01,
			/// <summary>Column can store null values</summary>
			AllowNull = 0x02,
			/// <summary>Binary column (for CHAR and MEMO only)</summary>
			AllowBinary = 0x04,
			/// <summary>When a field is NULL and binary (Integer, Currency, and Character/Memo fields)</summary>
			AllowNullAndBinary = 0x06,
			/// <summary>Column is autoincrementing</summary>
			AutoIncrement = 0x0C,
		}
		#endregion

		#region Private helper methods
		private TableFlag ToTableFlag(byte value)
		{
			if (!Enum.IsDefined(typeof(TableFlag), value))
				throw new ArgumentException("Unrecognized value for TableFlag = " + value);
			return (TableFlag)value;
		}
		private DbfType ToDbfType(byte value)
		{
			if (!Enum.IsDefined(typeof(DbfType), value))
				throw new ArgumentException("Unrecognized value for DbfType = " + value);
			return (DbfType)value;
		}
		private FieldType ToFieldType(byte value)
		{
			if (!Enum.IsDefined(typeof(FieldType), value))
				throw new ArgumentException("Unrecognized value for FieldType = " + value);
			return (FieldType)value;
		}
		private FieldFlag ToFieldFlag(byte value)
		{
			if (!Enum.IsDefined(typeof(FieldFlag), value))
				throw new ArgumentException("Unrecognized value for FieldFlag = " + value);
			return (FieldFlag)value;
		}
		private FieldType ToFieldType(Type type)
		{
			FieldType result;
			switch (type.FullName)
			{
				case "System.String":
					result = FieldType.Character;
					break;
				//case "System.Decimal":
				//	result = FieldType.Currency;
				//	break;
				case "System.Int16":
				case "System.Int32":
				case "System.Int64":
					result = FieldType.Numeric;
					break;
				case "System.Decimal":
					result = FieldType.Float;
					break;
				//case "System.DateTime":
				//	result = FieldType.Date;
				//	break;
				case "System.DateTime":
					result = FieldType.DateTime;
					break;
				case "System.Double":
					result = FieldType.Double;
					break;
				//case "System.Int":
				//	result = FieldType.Integer;
				//	break;
				case "System.Boolean":
					result = FieldType.Logical;
					break;
				//case "System.Object":
				//	result = FieldType.Memo;
				//	break;
				case "System.Object":
					result = FieldType.General;
					break;
				//case "System.Object":
				//	result = FieldType.Character_Binary;
				//	break;
				//case "System.Object":
				//	result = FieldType.Memo_Binary;
				//	break;
				//case "System.Object":
				//	result = FieldType.Picture;
				//	break;

				default:
					throw new ArgumentOutOfRangeException("type=" + type.FullName);
			}

			return result;
		}

		private unsafe void ReadHeader(string filename)
		{
			int nr = 0;
			int bytesRead = 0;

#if SUPPORT_BACKLINK_SECTION
			const int BACKLINK_SIZE = 263;
			byte[] backlink = new byte[BACKLINK_SIZE];
			int backlinkpos = 0;
#endif

			#region Read Header
			foreach (var buffer in FileUtil.ReadFile(filename, HEADER_LENGTH))
			{
				if (nr++ == 0)
				{
					#region read header section
					#region Cannot get this to work...
					//IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(hdr));
					//Marshal.Copy(buffer, 0, p, buffer.Length);
					//hdr = (DbfHeader)Marshal.PtrToStructure(p, typeof(DbfHeader));
					#endregion

					Header._Type = ToDbfType(buffer[0]);
					fixed (byte* ptr = Header._UpdateDate)
					{
						Common.UnsafeCopy(buffer, 1, 3, ptr /*Header.UpdateDate*/);
					}
					Header._RecordCount = Common.ToInt32(buffer, 4/*, 7*/);
					Header._FirstRecordOffset = Common.ToInt16(buffer, 8/*, 9*/);
					Header._OneRecordLength = Common.ToInt16(buffer, 10/*, 11*/);
					Header._Flag = ToTableFlag(buffer[28]);
					Header._CodePageMark = buffer[29];
					Header._HeaderRecordTerminator = HEADER_REC_TERM;
					#endregion
				}
				else if (buffer[0] == HEADER_REC_TERM
#if SUPPORT_BACKLINK_SECTION
					|| backlinkpos > 0
#endif
)
				{
#if SUPPORT_BACKLINK_SECTION
					// First loop: buffer contains the first 32 bytes.  Save it to backlink array, less the HEADER_REC_TERM
					// Loop more: until array is 263 bytes is read
					int start = buffer[0] == HEADER_REC_TERM ? 1 : 0;

					for (int i = start; i < buffer.Length && backlinkpos < BACKLINK_SIZE; i++)
					{
						backlink[backlinkpos++] = buffer[i];
					}

					if (backlinkpos == BACKLINK_SIZE)
						break;
#else
					// we stop if we encounter the terminator char.
					break;
#endif
				}
				else
				{
					#region read subrecord entry
					// read subrecord header
					var entry = new DbfSubrecord();

					#region Cannot get this to work...
					//IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(record));
					//Marshal.Copy(buffer, 0, p, buffer.Length);
					//record = (DbfSubRecord)Marshal.PtrToStructure(p, typeof(DbfSubRecord));
					#endregion

					Common.UnsafeCopy(buffer, 0, 10, entry._Fieldname);
					entry._Type = ToFieldType(buffer[11]);
					Common.UnsafeCopy(buffer, 12, 15, entry._Displacement);
					entry._Length = buffer[16];
					entry._DecimalPrecision = buffer[17];
					entry._Flags = ToFieldFlag(buffer[18]);
					entry._Next = Common.ToInt32(buffer, 19/*, 22*/);
					entry._Step = buffer[23];

					Header._SubRecords.Add(entry);
					#endregion
				}

				bytesRead += buffer.Length;

				// We stop if the total bytes read trail the reported first record offset.  This would avoid reading 
				// another chunk of unnecessary data -- because the next buffer[0] would be the terminator char.
				if (bytesRead + 1 == Header._FirstRecordOffset)
					break;
			}
			#endregion

#if SUPPORT_BACKLINK_SECTION
			#region set the header Backlink
			if (backlinkpos > 0 && backlinkpos != BACKLINK_SIZE)
				throw new ArgumentException("incorrect size for backline " + backlinkpos);
			
			fixed (byte* ptr = Header._Backlink)
				Common.UnsafeCopy(backlink, 0, backlink.Length, ptr);
			#endregion
#endif
		}
		private void ReadData(string filename)
		{
			var dt = this.DataTable;
			int nr = 0;

			dt.TableName = Path.GetFileNameWithoutExtension(filename);

			#region Generate Data Columns
#if CREATE_RECORD_DELETED_COLUMN
			{
				DataColumn dc = dt.Columns.Add(RECORD_DELETED_FIELDNAME, typeof(string));
				dc.MaxLength = 1;
			}
#endif
			// convert Header info to DataTable columns
			foreach (var rec in this.Header._SubRecords)
			{
				DataColumn dc = new DataColumn();
				dc.ColumnName = rec.Fieldname;
				dc.DataType = rec.GetDataType();
				if (dc.DataType == typeof(string))
					dc.MaxLength = rec._Length;

				dc.AutoIncrement = rec._Step > 0;
				if (dc.AutoIncrement)
				{
					dc.AutoIncrementStep = rec._Next;
					dc.AutoIncrementSeed = rec._Step;
				}
				dt.Columns.Add(dc);
			}
			#endregion

			#region Populate Data Rows
			int maxRows = Options.ImportMaxRows;

			foreach (var buffer in FileUtil.ReadFile(filename, this.Header._OneRecordLength, this.Header._FirstRecordOffset))
			{
				if (buffer.Length == 1 && buffer[0] == EOF_TERM)
					break;

				nr++;

				// always start at position 1 (position 0 is the delete flag)
				int start = 1;
				DataRow dr = dt.NewRow();

#if CREATE_RECORD_DELETED_COLUMN
				dr[RECORD_DELETED_FIELDNAME] = buffer[0] == RECORD_DELETED_FLAG ? '*' : ' ';
#else
				if (buffer[0] == RECORD_DELETED_FLAG)
				{
					// mark this row as delete
					dr.Delete();
				}
#endif

				#region Convert data into column
				foreach (var rec in this.Header._SubRecords)
				{
					DataColumn dc = dt.Columns[rec.Fieldname];
					int stop = start + rec._Length - 1;
					object value = null;

					try
					{
						#region Convert to data column
						switch (rec._Type)
						{
							case FieldType.Character:	//case FieldType.Character_Binary:
								//return typeof(string);
								// TODO: need to check Flag to determin if Char/Memo is binary type
								value = Common.ToString(buffer, start, stop);
								break;
							case FieldType.Currency:
								//return typeof(decimal);
								break;
							case FieldType.Numeric:
								value = DataConvert.ToInt32(Common.ToString(buffer, start, stop));
								//return typeof(int);
								break;
							case FieldType.Float:
								value = DataConvert.ToSingle(Common.ToString(buffer, start, stop));
								//return typeof(float);
								break;
							case FieldType.Date:
								value = DataConvert.ToDateTime(Common.ToString(buffer, start, stop));
								//return typeof(DateTime);
								break;
							case FieldType.DateTime:
								value = DataConvert.ToDateTime(Common.ToString(buffer, start, stop));
								//return typeof(DateTime);
								break;
							case FieldType.Double:
								value = DataConvert.ToDouble(Common.ToString(buffer, start, stop));
								//return typeof(double);
								break;
							case FieldType.Integer:
								value = DataConvert.ToInt32(Common.ToString(buffer, start, stop));
								//return typeof(int);
								break;
							case FieldType.Logical:
								value = DataConvert.ToBoolean(Common.ToString(buffer, start, stop));
								//return typeof(bool);
								break;
							case FieldType.Memo:	//case FieldType.Memo_Binary:
								// TODO: Need to convert this the right way
								// TODO: need to check Flag to determin if Char/Memo is binary type
								value = Common.ToString(buffer, start, stop);
								//return typeof(object);
								break;
							case FieldType.General:
								// TODO: Need to convert this the right way
								value = Common.ToString(buffer, start, stop);
								//return typeof(string);
								break;
							case FieldType.Picture:
								// TODO: Need to convert this the right way
								value = Common.ToString(buffer, start, stop);
								//return typeof(object);
								break;

							default:
								throw new ArgumentOutOfRangeException("Type=" + rec._Type);
						}
						#endregion
					}
					catch (Exception ex)
					{
						Debug.Write(ex.ToString());
						throw;
					}

					dr[dc] = value;
					start += rec._Length;
				}
				#endregion

				dt.Rows.Add(dr);

				if (maxRows > 0 && nr > maxRows)
					break;
			}
			#endregion
		}
		#endregion

		#region Public methods
		/// <summary>
		/// open a dbf file
		/// </summary>
		/// <param name="filename"></param>
		public void OpenFile(string filename)
		{
			Header = new DbfHeader();
			Header._SubRecords = new List<DbfSubrecord>();
			DataTable = new DataTable();

			ReadHeader(filename);
			ReadData(filename);

			//// dump for debugging purposes.
			//DataSet ds = new DataSet();
			//ds.Tables.Add(dbf.DataTable);
			//ds.WriteXml(@"c:\foo.xml");
		}

		/// <summary>
		/// Save DBF to file
		/// </summary>
		/// <param name="filename"></param>
		public void SaveFile(string filename)
		{
			if (File.Exists(filename))
				File.Delete(filename);

			using (FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 1024))
			using (BinaryWriter w = new BinaryWriter(fs))
			{
				w.Write(Header.ToByteArray());

				foreach (DataRow dr in DataTable.Rows)
				{
					int recordLength = 0;

#if !CREATE_RECORD_DELETED_COLUMN
					w.Write(dr.RowState == DataRowState.Deleted ? '*' : ' ');
					recordLength++;
#endif
					foreach (DbfSubrecord rec in Header._SubRecords)
					{
						int len = rec._Length;
						string value = dr[rec.Fieldname].ToString();
						byte[] bytes = null;

#if CREATE_RECORD_DELETED_COLUMN
						if (rec.Fieldname == RECORD_DELETED_FIELDNAME)
						{
							w.Write(value[0]);
							recordLength++;
							continue;
						}
#endif

						#region Conversion to byte array
						switch (rec._Type)
						{
							case FieldType.Character:
								bytes = Common.ToBytes(value, len, 32 /*pad with spaces*/);
								break;

							//case FieldType.Character_Binary:
							//	bytes = Common.ToBytes(value, len, 0 /*pad with nulls*/);
							//	break;

							case FieldType.Currency:
							case FieldType.Double:
							case FieldType.Float:
							case FieldType.Integer:
							case FieldType.Numeric:
								// 1 --> '        1'
								bytes = Common.ToBytes(value, len, 32 /*pad with spaces*/);
								//throw new NotImplementedException("type=" + rec._Type);
								break;

							case FieldType.Date:
								bytes = Common.ToBytes(DateTime.Parse(value), false);
								break;
							case FieldType.DateTime:
								bytes = Common.ToBytes(DateTime.Parse(value), true);
								break;

							case FieldType.Logical:
								bytes = value == bool.TrueString ? new byte[] { 89 /*Y*/ } : new byte[] { 78 /*N*/ };
								break;

							case FieldType.General:
							case FieldType.Memo:
							//case FieldType.Memo_Binary:
							case FieldType.Picture:
								throw new NotImplementedException("type=" + rec._Type);
								//break;

							default:
								throw new ArgumentOutOfRangeException("type=" + rec._Type);
						}
						#endregion

						w.Write(bytes);
						recordLength += bytes.Length;
					}

					if (recordLength != Header._OneRecordLength)
						throw new ArgumentException("incorrect data length");

				}

				w.Flush();
				w.Close();
			}
		}

		/// <summary>
		/// Set the datatable instance, and create DBF header
		/// </summary>
		/// <param name="dt"></param>
		public void ImportDataTable(DataTable dt)
		{
			this.DataTable = dt;
			Header = new DbfHeader();
			Header._SubRecords = new List<DbfSubrecord>();
			Header._Type = DbfType.FoxBasePlus;

			short oneRecordLength = 0 + 1;		// plus a RECORD DELETE char
			short firstRecordOffset = HEADER_LENGTH + 1;	// 32-bytes header + EOF char

			#region Create columns
			foreach (DataColumn dc in dt.Columns)
			{
				var entry = new DbfSubrecord();

				entry.Fieldname = dc.ColumnName;
				entry._Type = ToFieldType(dc.DataType);

				switch (entry._Type)
				{
					case FieldType.Currency:
					case FieldType.Numeric:
					case FieldType.Float:
					case FieldType.Double:
					case FieldType.Integer:
						entry._Length = 20;
						break;

					default:
						entry._Length = dc.MaxLength > 0 && dc.MaxLength < BYTE_MAX_VALUE ? (byte)dc.MaxLength : BYTE_MAX_VALUE;
						break;
				}

				entry._DecimalPrecision = 0;

				oneRecordLength += entry._Length;

				if (dc.AutoIncrement)
				{
					entry._Next = dc.AutoIncrementSeed < int.MaxValue ? (int)dc.AutoIncrementSeed : int.MaxValue;
					entry._Step = dc.AutoIncrementStep < BYTE_MAX_VALUE ? (byte)dc.AutoIncrementStep : BYTE_MAX_VALUE;
				}

				Header._SubRecords.Add(entry);
				firstRecordOffset += SUBFIELD_LENGTH;
			}
			#endregion

			Header._RecordCount = dt.Rows.Count;
			Header._HeaderRecordTerminator = HEADER_REC_TERM;
			Header._OneRecordLength = oneRecordLength;
			Header._FirstRecordOffset = firstRecordOffset;
			Header.UpdateDate = DateTime.Now;
		}
		#endregion
	}
}
