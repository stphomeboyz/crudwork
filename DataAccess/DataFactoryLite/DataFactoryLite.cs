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
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace crudwork.DataAccess
{
	/// <summary>
	/// Data Factory (lite version) - this implementation uses the strongly-typed classes by the provider.
	/// </summary>
	public abstract class DataFactoryLite<TConnection, TCommand, TDataAdapter, TParameter> : IDataFactory<TCommand, TParameter>
		where TConnection : DbConnection
		where TCommand : DbCommand
		where TDataAdapter : DbDataAdapter
		where TParameter : DbParameter
	{
		#region Fields
		/// <summary>
		/// the connection string used to create connection
		/// </summary>
		protected string connectionString;

		/// <summary>
		/// use this class to determine the query's CommandType
		/// </summary>
		protected static DBTextCommandList commandList = new DBTextCommandList("select|insert|update|delete|if|create|drop");
		#endregion

		#region Constructors
		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public DataFactoryLite(string connectionString)
		{
			this.connectionString = connectionString;
		}
		#endregion

		#region Helpers
		[Obsolete("not being used", true)]
		private TParameter[] ToParameter(DbParameterCollection parameters)
		{
			if (parameters == null || parameters.Count == 0)
				return null;

			var results = new List<TParameter>();

			for (int i = 0; i < parameters.Count; i++)
			{
				var p = parameters[i] as TParameter;
				if (p == null)
					throw new ArgumentException("parameters[" + i + "] is not of type TParameter");
				results.Add(p);
			}

			parameters.Clear();

			return results.ToArray();
		}
		#endregion

		#region TestConnection
		/// <summary>
		/// Test the connection
		/// </summary>
		public virtual void TestConnection()
		{
			using (var conn = GetConnection())
			{
				conn.Open();
				conn.Close();
			}
		}
		#endregion

		#region Fill
		/// <summary>
		/// Execute a query and return a DataSet
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual DataSet Fill(string query, params TParameter[] parameters)
		{
			return Fill(GetCommand(query, parameters));
		}

		/// <summary>
		/// Execute a query and return a DataSet
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual DataSet Fill(TCommand command)
		{
			using (var da = GetDataAdapter(command))
			{
				var result = new DataSet();
				da.Fill(result);
				return result;
			}
		}
		#endregion

		#region FillTable
		/// <summary>
		/// Execute a query and return a DataTable
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual DataTable FillTable(string query, params TParameter[] parameters)
		{
			return FillTable(GetCommand(query, parameters));
		}

		/// <summary>
		/// Execute a query and return a DataTable
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual DataTable FillTable(TCommand command)
		{
			using (var da = GetDataAdapter(command))
			{
				var result = new DataTable();
				da.Fill(result);
				return result;
			}
		}
		#endregion

		#region ExecuteNonQuery
		/// <summary>
		/// Execute a query w/o any results
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual int ExecuteNonQuery(string query, params TParameter[] parameters)
		{
			using (var cmd = GetCommand(query, parameters))
			{
				return ExecuteNonQuery(cmd);
			}
		}

		/// <summary>
		/// Execute a query w/o any results
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual int ExecuteNonQuery(TCommand command)
		{
			bool isOpened = command.Connection.State == ConnectionState.Open;
			try
			{
				if (!isOpened)
					command.Connection.Open();
				return command.ExecuteNonQuery();
			}
			finally
			{
				if (!isOpened && command.Connection.State == ConnectionState.Open)
					command.Connection.Close();

			}
		}
		#endregion

		#region ExecuteScalar
		/// <summary>
		/// Execute a query and return an object value
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual object ExecuteScalar(string query, params TParameter[] parameters)
		{
			using (var cmd = GetCommand(query, parameters))
			{
				return ExecuteScalar(cmd);
			}
		}

		/// <summary>
		/// Execute a query and return an object value
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual object ExecuteScalar(TCommand command)
		{
			bool isOpened = command.Connection.State == ConnectionState.Open;
			try
			{
				if (!isOpened)
					command.Connection.Open();
				return command.ExecuteScalar();
			}
			finally
			{
				if (!isOpened && command.Connection.State == ConnectionState.Open)
					command.Connection.Close();

			}
		}
		#endregion

		#region ExecuteReader
		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual IEnumerable<DataRow> ExecuteReader(string query, params TParameter[] parameters)
		{
			return ExecuteReader(GetCommand(query, parameters));
		}

		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual IEnumerable<DataRow> ExecuteReader(TCommand command)
		{
			bool isOpened = command.Connection.State == ConnectionState.Open;
			try
			{
				if (!isOpened)
					command.Connection.Open();

				using (var reader = command.ExecuteReader())
				{
					if (!reader.HasRows)
						yield break;	// nothing to do... stop the iteration.

					// we will use this DataTable to create columns and create a 
					// single DataRow for the entire iteration.  The DataTable is
					// not used for any other purposes.
					using (DataTable dt = new DataTable())
					{
						#region Create Columns
						int numFields = reader.FieldCount;
						for (int i = 0; i < numFields; i++)
						{
							dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
						}
						#endregion

						// create a single and reusable DataRow.
						DataRow dr = dt.NewRow();

						while (reader.Read())
						{
							#region Assign data to DataRow
							for (int i = 0; i < numFields; i++)
							{
								dr[reader.GetName(i)] = reader.GetValue(i);
							}
							#endregion
							yield return dr;
						}
					}

					// clean up...
					reader.Close();
				}

				yield break;	// iteration is completed.
			}
			#region WARNING: Cannot use the catch block!
			/*********************************************************************************************/
			/***** error CS1626: Cannot yield a value in the body of a try block with a catch clause *****/
			/*********************************************************************************************/
			//catch (Exception ex)
			//{
			//    DebuggerTool.AddData(ex, "query", query);
			//    DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
			//    throw;
			//}
			#endregion
			finally
			{
				if (!isOpened && command.Connection.State == ConnectionState.Open)
					command.Connection.Close();
			}
		}
		#endregion

		#region GetConnection, GetCommand, GetDataAdapter, GetParameterXXX
		/// <summary>
		/// Create a new instance of a database connection
		/// </summary>
		/// <returns></returns>
		public virtual TConnection GetConnection()
		{
			var conn = Activator.CreateInstance<TConnection>();
			conn.ConnectionString = this.connectionString;
			return conn;
		}

		/// <summary>
		/// Create a new instance of a database command
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual TCommand GetCommand(string query, TParameter[] parameters)
		{
			var cmd = Activator.CreateInstance<TCommand>();
			cmd.CommandText = query;
			cmd.Connection = GetConnection();

			if (parameters != null && parameters.Length > 0)
				cmd.Parameters.AddRange(parameters);

			#region stp: 07/28/2009: for some reason, setting cmd.CommandType = CommandType.Text throws a NotSupported exception...
			/*
				System.NotSupportedException: Specified method is not supported.
				   at System.Data.SQLite.SQLiteCommand.set_CommandType(CommandType value)
				   at crudwork.DataAccess.DataFactory.GetCommand(String query, DbConnection conn, DbParameter[] parameters)
				   at crudwork.DataAccess.DataFactory.GetCommand(String query, String connectionString, DbParameter[] parameters)
				   at crudwork.DataAccess.DataFactory.GetCommand(String query, DbParameter[] parameters)
				   at crudwork.DataAccess.DataFactory.ExecuteNonQuery(String query, DbParameter[] parameters)
				   at ReAlign.Utility.ReAlignWorkFile.Vacuum()
				   at ReAlign.Service.ComparisonEngine.Compare(String filename, String outFilestem)
			 */
			// 09/05/2009: We're taking a different approach.  In DataFactory, we skip this step if the provider is SQLite.
			// Here, we compare the current command type with the new command type, and change only if the new command type
			// is different.  This approach will work for SQLite, because the CommandType is Text by default.  (Funny,
			// setting it to Text again somehow throws the exception above.)
			#endregion
			var commandType = commandList.GetCommandType(query);
			if (cmd.CommandType != commandType)
				cmd.CommandType = commandType;

			return cmd;
		}

		/// <summary>
		/// Create a new instance of a database data adapter
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public virtual TDataAdapter GetDataAdapter(TCommand command)
		{
			TDataAdapter da = Activator.CreateInstance<TDataAdapter>();
			da.SelectCommand = command;
			return da;
		}

		/// <summary>
		/// Create a new instance of a database parameter
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="isNullable"></param>
		/// <param name="precision"></param>
		/// <param name="scale"></param>
		/// <param name="sourceColumn"></param>
		/// <param name="sourceVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual TParameter GetParameter(string name, DbType dbType, int size,
			ParameterDirection direction, bool isNullable, byte precision, byte scale,
			string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			var p = Activator.CreateInstance<TParameter>();
			p.ParameterName = name;
			p.DbType = dbType;
			p.Size = size;
			p.Direction = direction;
			p.SourceColumnNullMapping = isNullable;
			p.SourceColumn = sourceColumn;
			p.SourceVersion = sourceVersion;
			p.Value = value;
			return p;
		}
		#endregion

		#region IDataFactory<TCommand,TParameter> Members

		void IDataFactory<TCommand, TParameter>.TestConnection()
		{
			TestConnection();
		}

		DataSet IDataFactory<TCommand, TParameter>.Fill(string query, params TParameter[] parameters)
		{
			return Fill(query, parameters);
		}

		DataSet IDataFactory<TCommand, TParameter>.Fill(TCommand command)
		{
			return Fill(command);
		}

		DataTable IDataFactory<TCommand, TParameter>.FillTable(string query, params TParameter[] parameters)
		{
			return FillTable(query, parameters);
		}

		DataTable IDataFactory<TCommand, TParameter>.FillTable(TCommand command)
		{
			return FillTable(command);
		}

		int IDataFactory<TCommand, TParameter>.ExecuteNonQuery(string query, params TParameter[] parameters)
		{
			return ExecuteNonQuery(query, parameters);
		}

		int IDataFactory<TCommand, TParameter>.ExecuteNonQuery(TCommand command)
		{
			return ExecuteNonQuery(command);
		}

		object IDataFactory<TCommand, TParameter>.ExecuteScalar(string query, params TParameter[] parameters)
		{
			return ExecuteScalar(query, parameters);
		}

		object IDataFactory<TCommand, TParameter>.ExecuteScalar(TCommand command)
		{
			return ExecuteScalar(command);
		}

		IEnumerable<DataRow> IDataFactory<TCommand, TParameter>.ExecuteReader(string query, params TParameter[] parameters)
		{
			return ExecuteReader(query, parameters);
		}

		IEnumerable<DataRow> IDataFactory<TCommand, TParameter>.ExecuteReader(TCommand command)
		{
			return ExecuteReader(command);
		}

		#endregion
	}
}
