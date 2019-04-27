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

#pragma warning disable 0649		// warning CS0649: Field 'Xxxx' is never assigned to, and will always have its default value 0

namespace crudwork.FileImporters.Specialized
{
	/// <summary>
	/// dBase file version III, IV, V; and xBase compatible.
	/// </summary>
	internal class dBaseX
	{
		internal unsafe struct Header
		{
			// --- 32-bytes chunk #0 ----------------------------------
			internal byte Signature;
			internal fixed byte UpdateDate[3];
			internal Int32 RecordCount;
			internal Int16 HeaderLength;
			internal Int16 OneRecordLength;
			internal fixed byte Reserved[2];
			internal byte IncompleteTransaction;
			internal byte EncryptionFlag;
			internal fixed byte FreeRecordThread[4];
			internal fixed byte MultipleUser[8];
			internal byte MDXFlag;
			internal byte LanguageDriver;
			internal fixed byte Reserved2[2];

			// --- 32-bytes chunk #1 ----------------------------------
			internal List<Field> Fields;
		}

		internal unsafe struct Field
		{
			internal fixed byte Name[10];
			internal byte Type;
			internal fixed byte DataAddress[4];
			internal byte Length;
			internal byte DecimalPrecision;
			internal fixed byte MultipleUser[2];
			internal byte WorkAreaId;
			internal fixed byte Reserved[2];
			internal byte SetFieldsFlag;
			internal fixed byte Reserved2[8];
			internal byte IndexFieldFlag;
		}

		private class DbfSignatureBitFlag
		{
			#region Properties
			public byte Flag
			{
				get;
				private set;
			}
			public int Version
			{
				get;
				private set;
			}
			public bool MemoFileIsPresence
			{
				get;
				private set;
			}
			public int SQLTableIsPresence
			{
				get;
				private set;
			}
			public bool DBTFlag
			{
				get;
				private set;
			}
			#endregion

			public DbfSignatureBitFlag(byte flag)
			{
				/*
				 * dBASE IV bit flags:
				 * -------------------------------------------
				 * Bit			Description 
				 * -------------------------------------------
				 * 0-2			Version no. i.e. 0-7 
				 * 3			Presence of memo file 
				 * 4-6			Presence of SQL table 
				 * 7			DBT flag 
				 * 
				 * $ base -f 2 -t 10 00000111		--> 7
				 * $ base -f 2 -t 10 00001000		--> 8
				 * $ base -f 2 -t 10 01110000		--> 112
				 * $ base -f 2 -t 10 10000000		--> 128
				 **/

				Version = flag & 7;
				MemoFileIsPresence = (flag & 8) == 1;
				SQLTableIsPresence = flag & 112;
				DBTFlag = (flag & 128) == 1;
			}
		}

		//public enum DbfSignature
		//{
		//    FoxBase = 0x02,
		//    FileWithoutDBT = 0x03,
		//    dBase4WithoutMemoFile,
		//    dbase5WithoutMemoFile,
		//    VisualFoxPro,
		//    VisualFoxProWithDBC,
		//    VisualFoxProWithAutoIncrement,
		//    DBVMemoVarsize,
		//    dBase4WithMemo,
		//    FileWithDBT,
		//    dBase3PlusWithMemoFile,
		//    dBase4WithMemo2,
		//    dBase4WithSQLTable,
		//    DBVandDBTMemo,
		//    Clipper6WithSMTMemoFile,
		//    Clipper6
		//    /*
		//    02h 00000010 FoxBase 
		//    03h 00000011 File without DBT 
		//    04h 00000100 dBASE IV w/o memo file 
		//    05h 00000101 dBASE V w/o memo file 
		//    30h 00110000 Visual FoxPro 
		//    30h  00110000 Visual FoxPro w. DBC 
		//    31h  00110001 Visual FoxPro w. AutoIncrement field 
		//    43h  01000011 .dbv memo var size (Flagship) 
		//    7Bh  01111011 dBASE IV with memo 
		//    83h 10000011 File with DBT 
		//    83h 10000011 dBASE III+ with memo file 
		//    8Bh  10001011 dBASE IV w. memo 
		//    8Eh  10001110 dBASE IV w. SQL table 
		//    B3h  10110011 .dbv and .dbt memo (Flagship) 
		//    E5h  11100101 Clipper SIX driver w. SMT memo file.
		//    Note! Clipper SIX driver sets lowest 3 bytes to 110 in descriptor of crypted databases. So, 3->6, 83h->86h, F5->F6, E5->E6 etc.
			 
		//    F5h  11110101 FoxPro w. memo file 
		//    FBh  11111011 FoxPro ??? 
		//    */
		//}
	}
}

#pragma warning restore 0649
