// QueryAnything: OleDbManager.cs
//
// Copyright 2007 Steve T. Pham
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// Linking this library statically or dynamically with other modules is
// making a combined work based on this library.  Thus, the terms and
// conditions of the GNU General Public License cover the whole
// combination.
// 
// As a special exception, the copyright holders of this library give you
// permission to link this library with independent modules to produce an
// executable, regardless of the license terms of these independent
// modules, and to copy and distribute the resulting executable under
// terms of your choice, provided that you also meet, for each linked
// independent module, the terms and conditions of the license of that
// module.  An independent module is a module which is not derived from
// or based on this library.  If you modify this library, you may extend
// this exception to your version of the library, but you are not
// obligated to do so.  If you do not wish to do so, delete this
// exception statement from your version.using System;

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using crudwork.Utilities;
//using crudwork.Utilities;

namespace crudwork.DataSetTools
{
	/// <summary>
	/// OleDb Manager
	/// </summary>
	public class OleDbManager
	{
		private string connectionString = null;

		/// <summary>
		/// Create new object with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public OleDbManager(string connectionString)
		{
			this.connectionString = connectionString;
		}

		/// <summary>
		/// Fill all Tables
		/// </summary>
		/// <returns></returns>
		public DataSet FillTables()
		{
			string[] tables = GetColumn(GetTables(), "TABLE_NAME");

			try
			{
				DataSet ds = new DataSet();

				for (int i = 0; i < tables.Length; i++)
				{
					try
					{
						string tableName = tables[i];
						DataTable dt = FillTable("select * from [" + tableName + "]", tableName);
						ds.Tables.Add(dt);
					}
					catch //(Exception ex)
					{
						// TODO: need to ignore error and continue loading...
						throw;
					}
				}

				return ds;
			}
			catch //(Exception ex)
			{
				//DebuggerTool.AddData(ex, "tables", StringUtil.StringArrayToString(tables, ","));
				throw;
			}
		}

		/// <summary>
		///  Fill a table
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataTable FillTable(string query)
		{
			return FillTable(query, "Query1");
		}

		/// <summary>
		/// Fill a table
		/// </summary>
		/// <param name="query"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable FillTable(string query, string tableName)
		{
			try
			{
				using (OleDbConnection conn = new OleDbConnection(this.connectionString))
				using (OleDbCommand cmd = new OleDbCommand(query, conn))
				using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
				{
					DataTable dt = new DataTable(tableName);
					da.Fill(dt);
					return dt;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "tableName", tableName);
				throw;
			}
		}

		private string[] GetColumn(DataTable dt, string columnName)
		{
			List<string> results = new List<string>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				results.Add(dt.Rows[i][columnName].ToString());
			}
			return results.ToArray();
		}

		#region Query Schema
		/// <summary>
		/// Get a list of tablenames
		/// </summary>
		/// <returns></returns>
		public DataTable GetTables()
		{
			return QuerySchema(this.connectionString, OleDbSchemaGuid.Tables, null, null, null, "TABLE");
		}

		#region Code that needs crudwork
		////public DataTable GetColumns(string tableName)
		////{
		////    crudwork.DataAccess.TableManager tm = new crudwork.DataAccess.TableManager(crudwork.DataAccess.DatabaseProvider.OleDb, tableName, false);
		////    return QuerySchema(this.connectionString, OleDbSchemaGuid.Columns, tm.Database, tm.Owner, tm.Tablename, null);
		////}

		////public DataTable GetPrimaryKeys(string tableName)
		////{
		////    crudwork.DataAccess.TableManager tm = new crudwork.DataAccess.TableManager(crudwork.DataAccess.DatabaseProvider.OleDb, tableName, false);
		////    return QuerySchema(this.connectionString, OleDbSchemaGuid.Primary_Keys, tm.Database, tm.Owner, tm.Tablename, null);
		////} 
		#endregion

		/// <summary>
		/// Query the Schema
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="schema"></param>
		/// <param name="restrictions"></param>
		/// <returns></returns>
		public static DataTable QuerySchema(string connectionString, Guid schema, params object[] restrictions)
		{
			if (restrictions.Length != 4)
				throw new ArgumentOutOfRangeException("Expected 4 restrictions; but found " + restrictions.Length);

			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				List<string> results = new List<string>();
				conn.Open();

				try
				{
					// TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE, 
					// TABLE_GUID, DESCRIPTION, TABLE_PROPID, DATE_CREATED, DATE_MODIFIED
					return conn.GetOleDbSchemaTable(schema, restrictions);
				}
				catch (Exception ex)
				{
					DebuggerTool.AddData(ex, "schema", schema.ToString());
					DebuggerTool.AddData(ex, "restrictions", restrictions.ToString());
					throw;
				}
				finally
				{
					if (conn.State != System.Data.ConnectionState.Closed)
						conn.Close();
				}
			}
		}
		#endregion
	}
}
