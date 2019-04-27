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

#undef IMPLEMENT_DRISH_CODE				// bad code from DRISH should never be included.  (How did these get here???)

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
#if !SILVERLIGHT
using System.Data;
using System.Resources;
#endif

namespace crudwork.Utilities
{
	/// <summary>
	/// Convert a base data type to another base data type, similar to Convert class, but with default value.
	/// </summary>
	public static class DataConvert
	{
		/// <summary>
		/// check for null-ness of given object
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNull(object value)
		{
			return (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString().Trim(' ', '\t')));
		}

		/// <summary>
		/// convert given object to a different object type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <param name="conversionType"></param>
		/// <returns></returns>
		public static object ChangeType(object value, object defaultValue, Type conversionType)
		{
			if (IsNull(value))
				return defaultValue;

			try
			{
				object result = null;

				// special case especially for DateTime....
				if (conversionType.ToString() == "System.DateTime")
				{
					try
					{
						result = DateTime.Parse(value.ToString());
						return result;
					}
					catch //(Exception ex)
					{
						// too bad, try next conversion...
						//DebuggerTool.AddData(ex,"value", value);
						//Log.LogError(ex, "ChangType, 1. DateTime.Parse()");
						//Log.LogError("DateTime.Parse('" + value + "') : " + ex.Message, "ChangeType");
					}
				}


				try
				{
#if !SILVERLIGHT
					result = Convert.ChangeType(value, conversionType);
#else
					result = Convert.ChangeType(value, conversionType, null);
#endif
					return result;
				}
				catch //(Exception ex)
				{
					// too bad, try next conversion...
					//DebuggerTool.AddData(ex,"value", value);
					//Log.LogError(ex, "ChangeType, 2. Convert.ChangeType()");
					//Log.LogError("Convert.ChangeType('" + value + "') : " + ex.Message, "ChangeType");
				}


				try
				{
					// invoke the Parse method: type.Parse(value, NumberStyles.Any);
					object[] arguments = new object[] { value.ToString(), NumberStyles.Any };

					result = conversionType.InvokeMember("Parse",
						System.Reflection.BindingFlags.InvokeMethod,
						null, result, arguments);
					return result;
				}
				catch //(Exception ex)
				{
					// too bad, try next conversion...
					//DebuggerTool.AddData(ex,"value", value);
					//Log.LogError(ex, "ChangeType, 3. Invoking Parse method");
					//Log.LogError("<Object>.Parse('" + value + "', NumberStyle.Any) : " + ex.Message, "ChangeType");
				}

				//Log.LogError("No more conversion handle, returning default value", "ChangeType",
				//    string.Format("value={0} default={1} type={2}", value, defaultValue, conversionType));

				return defaultValue;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "value", value);
				DebuggerTool.AddData(ex, "defaultValue", defaultValue);
				DebuggerTool.AddData(ex, "conversionType", conversionType);
				throw;
			}
		}

		#region Conversion with default values
		/// <summary>
		/// Convert the given object to Boolean type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Boolean ToBoolean(object value, System.Boolean defaultValue)
		{
			return (System.Boolean)ChangeType(value, defaultValue, typeof(System.Boolean));
		}
		/// <summary>
		/// Convert the given object to Char type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Char ToChar(object value, System.Char defaultValue)
		{
			return (System.Char)ChangeType(value, defaultValue, typeof(System.Char));
		}
		/// <summary>
		/// Convert the given object to SByte type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.SByte ToSByte(object value, System.SByte defaultValue)
		{
			return (System.SByte)ChangeType(value, defaultValue, typeof(System.SByte));
		}
		/// <summary>
		/// Convert the given object to Byte type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Byte ToByte(object value, System.Byte defaultValue)
		{
			return (System.Byte)ChangeType(value, defaultValue, typeof(System.Byte));
		}
		/// <summary>
		/// Convert the given object to Int16 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Int16 ToInt16(object value, System.Int16 defaultValue)
		{
			return (System.Int16)ChangeType(value, defaultValue, typeof(System.Int16));
		}
		/// <summary>
		/// Convert the given object to Int32 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Int32 ToInt32(object value, System.Int32 defaultValue)
		{
			return (System.Int32)ChangeType(value, defaultValue, typeof(System.Int32));
		}
		/// <summary>
		/// Convert the given object to Int64 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Int64 ToInt64(object value, System.Int64 defaultValue)
		{
			return (System.Int64)ChangeType(value, defaultValue, typeof(System.Int64));
		}
		/// <summary>
		/// Convert the given object to UInt16 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.UInt16 ToUInt16(object value, System.UInt16 defaultValue)
		{
			return (System.UInt16)ChangeType(value, defaultValue, typeof(System.UInt16));
		}
		/// <summary>
		/// Convert the given object to UInt32 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.UInt32 ToUInt32(object value, System.UInt32 defaultValue)
		{
			return (System.UInt32)ChangeType(value, defaultValue, typeof(System.UInt32));
		}
		/// <summary>
		/// Convert the given object to UInt64 type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.UInt64 ToUInt64(object value, System.UInt64 defaultValue)
		{
			return (System.UInt64)ChangeType(value, defaultValue, typeof(System.UInt64));
		}
		/// <summary>
		/// Convert the given object to Single type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Single ToSingle(object value, System.Single defaultValue)
		{
			return (System.Single)ChangeType(value, defaultValue, typeof(System.Single));
		}
		/// <summary>
		/// Convert the given object to Double type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Double ToDouble(object value, System.Double defaultValue)
		{
			return (System.Double)ChangeType(value, defaultValue, typeof(System.Double));
		}
		/// <summary>
		/// Convert the given object to Decimal type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Decimal ToDecimal(object value, System.Decimal defaultValue)
		{
			return (System.Decimal)ChangeType(value, defaultValue, typeof(System.Decimal));
		}
		/// <summary>
		/// Convert the given object to DateTime type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.DateTime ToDateTime(object value, System.DateTime defaultValue)
		{
			return (System.DateTime)ChangeType(value, defaultValue, typeof(System.DateTime));
		}
		/// <summary>
		/// Convert the given object to String type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.String ToString(object value, System.String defaultValue)
		{
			return (System.String)ChangeType(value, defaultValue, typeof(System.String));
		}
		/// <summary>
		/// Convert the given object to Guid type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static System.Guid ToGuid(object value, System.Guid defaultValue)
		{
			return (System.Guid)ChangeType(value, defaultValue, typeof(System.Guid));
		}
		#endregion

		#region Conversion without default values
		/// <summary>
		/// Convert the given object to Boolean type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Boolean ToBoolean(object value)
		{
			return (System.Boolean)ChangeType(value, false, typeof(System.Boolean));
		}
		/// <summary>
		/// Convert the given object to Char type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Char ToChar(object value)
		{
			return (System.Char)ChangeType(value, '\0', typeof(System.Char));
		}
		/// <summary>
		/// Convert the given object to SByte type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.SByte ToSByte(object value)
		{
			return (System.SByte)ChangeType(value, (System.SByte)0, typeof(System.SByte));
		}
		/// <summary>
		/// Convert the given object to Byte type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Byte ToByte(object value)
		{
			return (System.Byte)ChangeType(value, (System.Byte)0, typeof(System.Byte));
		}
		/// <summary>
		/// Convert the given object to Int16 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int16 ToInt16(object value)
		{
			return (System.Int16)ChangeType(value, (System.Int16)0, typeof(System.Int16));
		}
		/// <summary>
		/// Convert the given object to Int32 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int32 ToInt32(object value)
		{
			return (System.Int32)ChangeType(value, (System.Int32)0, typeof(System.Int32));
		}
		/// <summary>
		/// Convert the given object to Int64 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int64 ToInt64(object value)
		{
			return (System.Int64)ChangeType(value, (System.Int64)0, typeof(System.Int64));
		}
		/// <summary>
		/// Convert the given object to UInt16 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt16 ToUInt16(object value)
		{
			return (System.UInt16)ChangeType(value, (System.UInt16)0, typeof(System.UInt16));
		}
		/// <summary>
		/// Convert the given object to UInt32 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt32 ToUInt32(object value)
		{
			return (System.UInt32)ChangeType(value, (System.UInt32)0, typeof(System.UInt32));
		}
		/// <summary>
		/// Convert the given object to UInt64 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt64 ToUInt64(object value)
		{
			return (System.UInt64)ChangeType(value, (System.UInt64)0, typeof(System.UInt64));
		}
		/// <summary>
		/// Convert the given object to Single type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Single ToSingle(object value)
		{
			return (System.Single)ChangeType(value, (System.Single)0, typeof(System.Single));
		}
		/// <summary>
		/// Convert the given object to Double type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Double ToDouble(object value)
		{
			return (System.Double)ChangeType(value, (System.Double)0, typeof(System.Double));
		}
		/// <summary>
		/// Convert the given object to Decimal type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Decimal ToDecimal(object value)
		{
			return (System.Decimal)ChangeType(value, (System.Decimal)0, typeof(System.Decimal));
		}
		/// <summary>
		/// Convert the given object to String type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.String ToString(object value)
		{
			return (System.String)ChangeType(value, string.Empty, typeof(System.String));
		}
		/// <summary>
		/// Convert the given object to Guid type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Guid ToGuid(object value)
		{
			return (System.Guid)ChangeType(value, new Guid(), typeof(System.Guid));
		}
		#endregion

		#region Conversion for Nullable types
		/// <summary>
		/// return DbNull if value is null; otherwise return the value as is.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static object ToDbNull(object value)
		{
			return (value == null) ? DBNull.Value : value;
		}

		/// <summary>
		/// Convert the given object to Boolean type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Boolean? ToNullableBoolean(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Boolean)ChangeType(value, false, typeof(System.Boolean));
		}
		/// <summary>
		/// Convert the given object to Char type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Char? ToNullableChar(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Char)ChangeType(value, '\0', typeof(System.Char));
		}
		/// <summary>
		/// Convert the given object to SByte type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.SByte? ToNullableSByte(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.SByte)ChangeType(value, (System.SByte)0, typeof(System.SByte));
		}
		/// <summary>
		/// Convert the given object to Byte type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Byte? ToNullableByte(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Byte)ChangeType(value, (System.Byte)0, typeof(System.Byte));
		}
		/// <summary>
		/// Convert the given object to Int16 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int16? ToNullableInt16(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Int16)ChangeType(value, (System.Int16)0, typeof(System.Int16));
		}
		/// <summary>
		/// Convert the given object to Int32 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int32? ToNullableInt32(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Int32)ChangeType(value, (System.Int32)0, typeof(System.Int32));
		}
		/// <summary>
		/// Convert the given object to Int64 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Int64? ToNullableInt64(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Int64)ChangeType(value, (System.Int64)0, typeof(System.Int64));
		}
		/// <summary>
		/// Convert the given object to UInt16 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt16? ToNullableUInt16(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.UInt16)ChangeType(value, (System.UInt16)0, typeof(System.UInt16));
		}
		/// <summary>
		/// Convert the given object to UInt32 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt32? ToNullableUInt32(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.UInt32)ChangeType(value, (System.UInt32)0, typeof(System.UInt32));
		}
		/// <summary>
		/// Convert the given object to UInt64 type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.UInt64? ToNullableUInt64(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.UInt64)ChangeType(value, (System.UInt64)0, typeof(System.UInt64));
		}
		/// <summary>
		/// Convert the given object to Single type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Single? ToNullableSingle(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Single)ChangeType(value, (System.Single)0, typeof(System.Single));
		}
		/// <summary>
		/// Convert the given object to Double type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Double? ToNullableDouble(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Double)ChangeType(value, (System.Double)0, typeof(System.Double));
		}
		/// <summary>
		/// Convert the given object to Decimal type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Decimal? ToNullableDecimal(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Decimal)ChangeType(value, (System.Decimal)0, typeof(System.Decimal));
		}
		/// <summary>
		/// Convert the given object to String type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.String ToNullableString(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.String)ChangeType(value, string.Empty, typeof(System.String));
		}
		/// <summary>
		/// Convert DateTime to work with SQL data type: SmallDateTime and DateTime
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.DateTime? ToNullableDateTime(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.DateTime)ChangeType(value, DateTime.MinValue, typeof(System.DateTime));
		}
		/// <summary>
		/// Convert the given object to Guid type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.Guid? ToNullableGuid(object value)
		{
			if (IsNull(value))
				return null;
			else
				return (System.Guid)ChangeType(value, new Guid(), typeof(System.Guid));
		}
		#endregion

		#region Date Conversion with SQLDbType handles
		/// <summary>
		/// Convert DateTime to work with SQL data type: SmallDateTime and DateTime
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static System.DateTime ToDateTime(object value)
		{
			return (System.DateTime)ChangeType(value, DateTime.MinValue, typeof(System.DateTime));
		}

#if !SILVERLIGHT
		/// <summary>
		/// parse the value and return a DateTime object according to the specified SqlDbType
		/// </summary>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static System.DateTime ToDateTime(object value, SqlDbType dbType)
		{
			#region **** Info ****
			/*
			 * initialize the DateTime with EpochDate, on parse failure, because the primitive
			 * DateTime type does not have any limitations.
			 * 
			 *		DateTime					: 01/01/0001 - 12/31/9999
			 * 
			 * SQL types on the other hand, have tighter date ranges.  They are:
			 * 
			 *		SqlDbType.SmallDateTime		: 01/01/1900 - 06/06/2079
			 *		SqlDbType.DateTime			: 01/01/1753 - 12/31/9999
			 *		SqlDbType.VarChar			: 01/01/1000 - 12/31/2999		(customized for TaxSegPro)
			 * 
			 * Take action as follow:
			 * 
			 * 1) If the date value is equal to EpochDate, return GetMinDate(dbType)
			 * 
			 * 2) If date value is before the min date, return GetMinDate(dbType)
			 * 
			 * 3) If date value is after the max date, return GetMaxDate(dbType)
			 * 
			 * 4) If date value is within the acceptable range of the dbType, return the DateTime instance.
			 * 
			 * */
			#endregion

			DateTime dt = (System.DateTime)ChangeType(value, DateTime.MinValue, typeof(System.DateTime));
			DateTime minDate = DateTime.MinValue;
			DateTime maxDate = DateTime.MaxValue;

			if ((dt == DateTime.MinValue) || (dt < minDate))
			{
				return minDate;
			}
			else if (dt > maxDate)
			{
				return maxDate;
			}
			else
			{
				return dt;
			}
		}

		/// <summary>
		/// parse the value and return a DateTime object according to the specified SqlDbType.  If the
		/// date is outside the acceptable range, return DBNull.Value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static object ToDateTimeOrNull(object value, SqlDbType dbType)
		{
			DateTime dt = (System.DateTime)ChangeType(value, DateTime.MinValue, typeof(System.DateTime));
			DateTime minDate = DateTime.MinValue;
			DateTime maxDate = DateTime.MaxValue;

			if ((dt == DateTime.MinValue) || (dt < minDate))
			{
				return DBNull.Value;
			}
			else if (dt == minDate)		// special: we treat this as null for backward compatibility.
			{
				return DBNull.Value;
			}
			else if (dt > maxDate)
			{
				return DBNull.Value;
			}
			else
			{
				return dt;
			}
		}

		/// <summary>
		/// return a date string or null if date == Initializer.GetMinDate(dbType).
		/// </summary>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static string ToShortDateStringOrNull(object value, SqlDbType dbType)
		{
			object o = ToDateTimeOrNull(value, dbType);

			if (o is System.DBNull)
			{
				return string.Empty;
			}
			else
			{
				DateTime d = (DateTime)o;
				return d.ToString("MM/dd/yyyy");
				//return d.ToShortDateString();
			}
		}

		/// <summary>
		/// return a full date/time string or null if date == Initializer.GetMinDate(dbType).
		/// </summary>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static string ToFullDateStringOrNull(object value, SqlDbType dbType)
		{
			object o = ToDateTimeOrNull(value, dbType);

			if (o is System.DBNull)
			{
				return string.Empty;
			}
			else
			{
				DateTime d = (DateTime)o;
				return d.ToString();
			}
		}
#endif
		#endregion

		#region NumComma
		/// <summary>
		/// Convert a number to a human readible string
		/// </summary>
		/// <param name="value"></param>
		/// <param name="decimalPosition"></param>
		/// <returns></returns>
		public static string NumComma(decimal value, int decimalPosition)
		{
			string fmt = "{0:N" + decimalPosition + "}";
			return string.Format(fmt, value);
		}
		#endregion

		/// <summary>
		/// escape the apostophe char for SQL query.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Quoted(string value)
		{
			if (!value.Contains("'"))
				return value;

			var s = new StringBuilder(value);
			s.Replace("'", "''");
			return s.ToString();
		}

#if !SILVERLIGHT
		/// <summary>
		/// Converts DataSet to XmlFormat
		/// </summary>
		/// <param name="ds">DataSet to be converted to Xml</param>
		/// <returns>XmlFormatted Data</returns>
		[Obsolete("consider using DataUtil.DataSetToXml", true)]
		public static string DataSetToXml(DataSet ds)
		{
			using (StringWriter writer = new StringWriter())
			{
				ds.WriteXml(writer, XmlWriteMode.IgnoreSchema);
				return (writer.ToString());
			}
		}

		/// <summary>
		/// Converts a DataTable to XmlFormat
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		[Obsolete("consider using DataUtil.DataTableToXml", true)]
		public static string DataTableToXml(DataTable dt)
		{
			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(dt.Copy());
				return DataSetToXml(ds);
			}
		}
#endif

		#region Conversion with default values (treating Drish -99M as empty)
#if IMPLEMENT_DRISH_CODE
		/// <summary>
		/// Custom Conversion Handle
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <param name="drishDefault"></param>
		/// <returns></returns>
		[Obsolete("Do not use this", true)]
		public static System.Double ToDouble(object value, System.Double defaultValue, object drishDefault)
		{
			if (value.ToString() == drishDefault.ToString())
				return defaultValue;

			return (System.Double)ChangeType(value, defaultValue, typeof(System.Double));
		}

		/// <summary>
		/// Custom Conversion Handle
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <param name="drishDefault"></param>
		/// <returns></returns>
		[Obsolete("Do not use this", true)]
		public static System.Int16 ToInt16(object value, System.Int16 defaultValue, object drishDefault)
		{
			if (value.ToString() == drishDefault.ToString())
				return defaultValue;

			return (System.Int16)ChangeType(value, defaultValue, typeof(System.Int16));
		}
#endif
		#endregion

		#region Convert C# Types to .NET Types
#if !SILVERLIGHT
		/// <summary>
		/// Convert the data type (in string value) to its equivalent .NET Framework Type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Type ToType(string value)
		{
			var typename = ToNETType(value);
			return Type.GetType(typename);
		}

		private static DataSet _systemTypeMapping = null;

		/// <summary>
		/// Convert the given data type (C# or VB.NET) to its equivalent .NET Framework Type
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToNETType(string value)
		{
			if (_systemTypeMapping == null)
			{
				var ds = ResourceUtil.ToDataSet("crudwork.Utilities.Resources.SystemTypeMapping.xml");

				_systemTypeMapping = ds;
			}

			using (var dv = new DataView(_systemTypeMapping.Tables["SystemTypeMapping"]))
			{
				dv.RowFilter = string.Format("VBType='{0}' or CSType='{0}' or FrameworkType='{0}'", Quoted(value));
				if (dv.Count == 1)
					return dv[0]["FrameworkType"].ToString();

				throw new ArgumentException("unknown type: " + value);
			}
		}
#endif
		#endregion
	}
}
