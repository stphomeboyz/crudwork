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
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
//using crudwork.DataAccess.SqlClient;
using System.Diagnostics;
using crudwork.Utilities;
using crudwork.Models.DataAccess;
using crudwork.DataAccess.DbCommon;

namespace crudwork.DataAccess
{
	/// <summary>
	/// DataFactory - this implementation uses the DbProviderFactory to create provider-specific classes.
	/// </summary>
	public partial class DataFactory : IDataFactory<DbCommand, DbParameter>
	{
		#region Enumerators
		#endregion

		#region Fields
		//private const string UNASSIGNED_CONNECTION = "AUTO";

		private DatabaseProvider provider;
		private string connectionString;
		private DbProviderFactory providerFactory;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="connectionString"></param>
		public DataFactory(DatabaseProvider provider, string connectionString)
		{
			this.provider = provider;
			this.connectionString = connectionString;
			providerFactory = DbProviderFactories.GetFactory(ProviderName);
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="provider"></param>
		public DataFactory(DatabaseProvider provider)
			: this(provider, "data source=(local); integrated security=true")
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="connectionString"></param>
		public DataFactory(string connectionString)
			: this(DatabaseProvider.SqlClient, connectionString)
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="dbConnection"></param>
		public DataFactory(DataConnectionInfo dbConnection)
			: this(dbConnection.Provider, dbConnection.ConnectionString)
		{
		}

		/// <summary>
		/// Create a new instance with given attributes
		/// </summary>
		/// <param name="css"></param>
		public DataFactory(System.Configuration.ConnectionStringSettings css)
			: this(Providers.Converter(css.ProviderName), css.ConnectionString)
		{
		}
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods

		#region ---- GetService ----
		/// <summary>
		/// retrieve a list of service available
		/// </summary>
		/// <param name="databaseProvider"></param>
		/// <returns></returns>
		public static ServiceDefinitionList GetService(DatabaseProvider databaseProvider)
		{
			/*
			 * ServerName
			 * InstanceName
			 * IsClustered
			 * Version
			 * FactoryName
			 * 
			 */
			string providerString = Providers.ToProvider(databaseProvider);
			DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerString);
			DbDataSourceEnumerator enumerator = providerFactory.CreateDataSourceEnumerator();
			return enumerator.GetDataSources().ToServiceDefinitionList();
		}
		#endregion

		#region ---- UpdateTable/SychronizeTable methods ----
		/// <summary>
		/// Update changes (insert,update,delete) from DataSet to database provider, and
		/// retrieve any changes from the database provider to the DataSet.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="query"></param>
		public void SychronizeTable(DataTable dt, string query)
		{
			try
			{
				UpdateTable(dt, query);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				DebuggerTool.AddData(ex, "query", query);
				throw;
			}
		}

		/// <summary>
		/// Update changes (insert,update,delete) from DataSet to database provider, and
		/// retrieve any changes from the database provider to the DataSet.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="command"></param>
		public void UpdateTable(DataTable dt, DbCommand command)
		{
			using (DbDataAdapter da = GetDataAdapter(command))
			using (DbCommandBuilder cb = GetCommandBuilder(da))
			{
				da.AcceptChangesDuringFill = true;
				da.AcceptChangesDuringUpdate = true;
				da.UpdateBatchSize = 100;
				da.Update(dt);

				// refresh content from database.
				dt.Clear();
				da.Fill(dt);
			}
		}

		/// <summary>
		/// Update changes (insert,update,delete) from DataSet to database provider, and
		/// retrieve any changes from the database provider to the DataSet.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="query"></param>
		public void UpdateTable(DataTable dt, string query)
		{
			DbCommand command = GetCommand(query, this.connectionString);
			UpdateTable(dt, command);
		}

		/// <summary>
		/// Update changes (insert,update,delete) from DataSet to database provider, and
		/// retrieve any changes from the database provider to the DataSet.
		/// </summary>
		/// <param name="dt"></param>
		public void UpdateTable(DataTable dt)
		{
			try
			{
				UpdateTable(dt, "select * from " + dt.TableName);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				throw;
			}
		}

		/// <summary>
		/// Update changes (insert,update,delete) from DataSet to database provider, and
		/// retrieve any changes from the database provider to the DataSet.
		/// </summary>
		/// <param name="ds"></param>
		public void UpdateDataSet(DataSet ds)
		{
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dt = ds.Tables[i];

				UpdateTable(dt);
			}
		}
		#endregion

		#region ---- FillTable methods ----
		/// <summary>
		/// Open a DataAdapter with a query string, and fill the DataTable.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		public void FillTable(DataTable dt, string query, DbConnection conn)
		{
			// NOTE: the connection state is explicitly control by the calling method ...
			try
			{
				using (DbDataAdapter da = GetDataAdapter(query, conn))
				{
					dt.BeginLoadData();
					dt.Clear();
					da.Fill(dt);
					dt.EndLoadData();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "conn", conn);
				throw;
			}
		}

		/// <summary>
		/// Open a DataAdapter with a query string, and fill the DataTable.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="query"></param>
		/// <param name="connectionString"></param>
		/// <param name="parameters"></param>
		public void FillTable(DataTable dt, string query, string connectionString, params DbParameter[] parameters)
		{
			try
			{
				using (DbDataAdapter da = GetDataAdapter(query, connectionString, parameters))
				{
					dt.BeginLoadData();
					dt.Clear();
					da.Fill(dt);
					dt.EndLoadData();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "connectionString", connectionString);
				DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
				// TODO: save exception data.
				throw;
			}
		}

		/// <summary>
		/// Open a DataAdapter with a query string, and fill the DataTable.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		public void FillTable(DataTable dt, string query, params DbParameter[] parameters)
		{
			try
			{
				FillTable(dt, query, this.connectionString, parameters);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
				throw;
			}
		}

		/// <summary>
		/// Fill the DataTable with the command
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="command"></param>
		public void FillTable(DataTable dt, DbCommand command)
		{
			try
			{
				using (DbDataAdapter da = GetDataAdapter(command))
				{
					dt.BeginLoadData();
					dt.Clear();
					da.Fill(dt);
					dt.EndLoadData();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "dt", dt);
				DebuggerTool.AddData(ex, "command", DebuggerTool.Dump(command));
				throw;
			}
		}

		#region more FillTable wrappers
		/// <summary>
		/// Fill a table with the command
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public DataTable FillTable(DbCommand command)
		{
			DataTable dt = new DataTable("QueryResults");
			FillTable(dt, command);
			return dt;
		}

		/// <summary>
		/// Fill a table with the query string and optional parameters
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DataTable FillTable(string query, params DbParameter[] parameters)
		{
			DataTable dt = new DataTable("QueryResults");
			FillTable(dt, query, parameters);
			return dt;
		}
		#endregion

		#endregion

		#region ---- Fill DataSet ----
		/// <summary>
		/// Execute a SQL command and return a DataSet instance
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DataSet Fill(string query, params DbParameter[] parameters)
		{
			try
			{
				using (DbCommand cmd = GetCommand(query, parameters))
				using (DbDataAdapter da = GetDataAdapter(cmd))
				{
					DataSet ds = new DataSet();
					da.Fill(ds);
					return ds;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
				throw;
			}
		}

		/// <summary>
		/// Execute a SQL command and return a DataSet instance
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public DataSet Fill(DbCommand command)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region ---- ExecuteNonQuery ----
		/// <summary>
		/// Open a command based on the query string and connection,
		/// and run ExecuteNonQuery.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string query, DbConnection conn)
		{
			try
			{
				using (DbCommand cmd = GetCommand(query, conn))
				{
					AddReturnValueParameter(cmd);

					// NOTE: the connection state is explicitly control by the calling method ...
					cmd.ExecuteNonQuery();

					return (int)cmd.Parameters["@ReturnValue"].Value;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "conn", conn);
				throw;
			}
		}

		/// <summary>
		/// Open a command based on the query string, connection and transaction,
		/// and run ExecuteNonQuery.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		/// <param name="tran"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string query, DbConnection conn, DbTransaction tran)
		{
			try
			{
				using (DbCommand cmd = GetCommand(query, conn))
				{
					cmd.Transaction = tran;
					AddReturnValueParameter(cmd);

					// NOTE: the connection state is explicitly control by the calling method ...
					cmd.ExecuteNonQuery();

					//return (int)cmd.Parameters["@ReturnValue"].Value;

					if (cmd.Parameters.Contains("@ReturnValue"))
						return (int)cmd.Parameters["@ReturnValue"].Value;
					else
						return 0;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "conn", conn);
				DebuggerTool.AddData(ex, "tran", DebuggerTool.Dump(tran));
				throw;
			}
		}

		/// <summary>
		/// Open a command based on the query string with parameter,
		/// and run ExecuteNonQuery.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string query, params DbParameter[] parameters)
		{
			return ExecuteNonQuery(GetCommand(query, parameters));
		}

		/// <summary>
		/// Open a command baesd on the query string, connection and transaction,
		/// adn run ExecuteNonQuery
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(DbCommand cmd)
		{
			bool isOpen = false;

			try
			{
				AddReturnValueParameter(cmd);

				if (cmd.Connection == null)
					cmd.Connection = GetConnection();

				isOpen = cmd.Connection.State == ConnectionState.Open;
				if (!isOpen)
					cmd.Connection.Open();

				cmd.ExecuteNonQuery();

				if (!cmd.Parameters.Contains("@ReturnValue"))
					return 0;
				else
					return (int)cmd.Parameters["@ReturnValue"].Value;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "cmd", DebuggerTool.Dump(cmd));
				throw;
			}
			finally
			{
				if (!isOpen)
					cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Add a @ReturnValue parameter to the DbCommand to return the return value.
		/// </summary>
		/// <param name="cmd"></param>
		private void AddReturnValueParameter(DbCommand cmd)
		{
			if (cmd.CommandType != CommandType.StoredProcedure)
				return;

			if (cmd.Parameters.Contains("@ReturnValue"))
				return;

			cmd.Parameters.Add(GetParameterRV(DbType.Int32));
		}
		#endregion

		#region ---- ExecuteScalar ----
		/// <summary>
		/// Open a command based on the query string with parameter,
		/// and run ExecuteScalar.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object ExecuteScalar(string query, params DbParameter[] parameters)
		{
			try
			{
				using (DbCommand cmd = GetCommand(query, this.connectionString, parameters))
				{
					cmd.Connection.Open();
					object retValue = cmd.ExecuteScalar();
					cmd.Connection.Close();

					return retValue;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
				throw;
			}
		}

		/// <summary>
		/// Execute a scalar function and return the result
		/// </summary>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		/// <param name="tran"></param>
		/// <returns></returns>
		public object ExecuteScalar(string query, DbConnection conn, DbTransaction tran)
		{
			try
			{
				// if the connection was opened in this
				// scope, it must be closed afterward.
				//
				// if the connection was already opened, leave the state alone.
				bool isOpen = false;

				using (DbCommand cmd = GetCommand(query, conn))
				{
					isOpen = conn.State == ConnectionState.Open;
					if (!isOpen)
						cmd.Connection.Open();

					cmd.Transaction = tran;
					object retValue = cmd.ExecuteScalar();

					if (!isOpen)
						cmd.Connection.Close();

					return retValue;
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "query", query);
				DebuggerTool.AddData(ex, "conn", conn);
				DebuggerTool.AddData(ex, "tran", DebuggerTool.Dump(tran));
				throw;
			}
		}

		/// <summary>
		/// Execute a scalar function and return the result
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public object ExecuteScalar(DbCommand cmd)
		{
			try
			{
				if (cmd.Connection == null)
					cmd.Connection = GetConnection();

				bool isOpen = cmd.Connection.State == ConnectionState.Open;
				if (!isOpen)
					cmd.Connection.Open();

				object retValue = cmd.ExecuteScalar();

				if (!isOpen)
					cmd.Connection.Close();

				return retValue;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "cmd", DebuggerTool.Dump(cmd));
				throw;
			}
		}
		#endregion

		#region ---- ExecuteReader ----
		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public IEnumerable<DataRow> ExecuteReader(string query, params DbParameter[] parameters)
		{
			return ExecuteReader(GetCommand(query, parameters));
		}

		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public IEnumerable<DataRow> ExecuteReader(DbCommand command)
		{
			try
			{
				bool isOpen = false;
				using (DbCommand cmd = command)
				{
					if (cmd.Connection == null)
						cmd.Connection = GetConnection();

					isOpen = cmd.Connection.State == ConnectionState.Open;
					if (!isOpen)
						cmd.Connection.Open();

					using (DbDataReader reader = cmd.ExecuteReader())
					{
						if (!reader.HasRows)
							yield break;	// nothing to do... stop the iteration.

						int numFields = reader.FieldCount;
						//int rownum = -1;

						// we will use this DataTable to create columns and create a 
						// single DataRow for the entire iteration.  The DataTable is
						// not used for any other purposes.
						using (DataTable dt = new DataTable())
						{
							#region Create Columns
							for (int i = 0; i < numFields; i++)
							{
								dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
							}
							#endregion

							// create a single and reusable DataRow.
							DataRow dr = dt.NewRow();

							while (reader.Read())
							{
								//rownum++;

								#region Assign data to DataRow
								for (int i = 0; i < numFields; i++)
								{
									dr[reader.GetName(i)] = reader.GetValue(i);
								}
								#endregion

								//// commit to clean up the versions
								//dr.AcceptChanges();

								yield return dr;
							}
						}

						// clean up...
						reader.Close();
					}

					// clean up...
					if (!isOpen)
						cmd.Connection.Close();
				}

				yield break;	// iteration is completed.
			}

			/*********************************************************************************************/
			/***** error CS1626: Cannot yield a value in the body of a try block with a catch clause *****/
			/*********************************************************************************************/
			//catch (Exception ex)
			//{
			//    DebuggerTool.AddData(ex, "query", query);
			//    DebuggerTool.AddData(ex, "parameters", DebuggerTool.Dump(parameters));
			//    throw;
			//}

			finally
			{
			}
		}
		#endregion

		#region ---- Execute Stored Procedure ----
		/// <summary>
		/// Execute the stored procedure and return the @ReturnValue parameter
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public int ExecuteProcedure(string query, DbParameter[] parameters)
		{
			return ExecuteProcedure(GetCommand(query, parameters));
		}

		/// <summary>
		/// Execute the stored procedure and return the @ReturnValue parameter
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public int ExecuteProcedure(DbCommand cmd)
		{
			try
			{
				bool isOpen = false;

				AddReturnValueParameter(cmd);

				if (cmd.Connection == null)
					cmd.Connection = GetConnection();

				isOpen = cmd.Connection.State == ConnectionState.Open;
				if (!isOpen)
					cmd.Connection.Open();

				cmd.ExecuteNonQuery();

				if (!isOpen)
					cmd.Connection.Close();

				return (int)cmd.Parameters["@ReturnValue"].Value;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "cmd", DebuggerTool.Dump(cmd));
				throw;
			}
		}
		#endregion

		#region ---- TestConnection ----
		/// <summary>
		/// Test connectivity of the specified connection string.
		/// </summary>
		/// <param name="connectionString"></param>
		public void TestConnection(string connectionString)
		{
			try
			{
				if (String.IsNullOrEmpty(connectionString))
				{
					throw new ArgumentNullException("connectionString");
				}
				using (DbConnection conn = GetConnection(connectionString))
				{
					conn.Open();
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "connectionString", connectionString);
				throw;
			}
		}

		/// <summary>
		/// Test connectivity of the default connection string.
		/// </summary>
		public void TestConnection()
		{
			try
			{
				if (String.IsNullOrEmpty(this.connectionString))
				{
					throw new ArgumentNullException("connectionString");
				}

				TestConnection(this.connectionString);
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "connectionString", this.connectionString);
				throw;
			}
		}
		#endregion

		#region ---- Provider Independent methods ----

		#region ---- GetConnection methods ----
		/// <summary>
		/// Create a database connection using the specified connection string.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public DbConnection GetConnection(string connectionString)
		{
			#region Sanity check
			if (String.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("connectionString");
			}
			#endregion

			DbConnection conn = providerFactory.CreateConnection();

			conn.ConnectionString = connectionString;
			//conn.ConnectionTimeout = 90;

			return conn;
		}

		/// <summary>
		/// Create a database connection using the default connection string.
		/// </summary>
		/// <returns></returns>
		public DbConnection GetConnection()
		{
			return GetConnection(this.connectionString);
		}
		#endregion

		#region ---- GetCommand methods ----
		private DBTextCommandList commandList = new DBTextCommandList("select|insert|update|delete|if|create|drop");

		/// <summary>
		/// Get the DBTextCommandList
		/// </summary>
		public DBTextCommandList CommandList
		{
			get
			{
				return this.commandList;
			}
		}

		/// <summary>
		/// Create a database command using a specified query string,
		/// a DbConnection instance, and optionally an array of DbParameters.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DbCommand GetCommand(string query, DbConnection conn, params DbParameter[] parameters)
		{
			#region Sanity check
			if (String.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query");
			}

			if (conn == null)
			{
				throw new ArgumentNullException("conn");
			}
			#endregion

			DbCommand cmd = providerFactory.CreateCommand();

			cmd.CommandText = query;
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
			#endregion
			if (provider != DatabaseProvider.SQLite)
			{
				cmd.CommandType = commandList.GetCommandType(query);
			}
			cmd.CommandTimeout = 300;			// allow command to execute up to 5 min.
			cmd.Connection = conn;

			cmd.Parameters.Clear();

			if (parameters != null && parameters.Length > 0)
			{
				cmd.Parameters.AddRange(parameters);
			}

			return cmd;
		}

		/// <summary>
		/// Create a database command using a specified query string,
		/// a connection string, and optionally an array of DbParameters.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="connectionString"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DbCommand GetCommand(string query, string connectionString, params DbParameter[] parameters)
		{
			return GetCommand(query, GetConnection(connectionString), parameters);
		}

		/// <summary>
		/// Create a database command using a specified query string,
		/// and optionally an array of DbParameters.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DbCommand GetCommand(string query, params DbParameter[] parameters)
		{
			return GetCommand(query, this.connectionString, parameters);
		}
		#endregion

		#region ---- GetDataAdapter methods ----
		/// <summary>
		/// Create a data adapter with the specified query string.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DbDataAdapter GetDataAdapter(string query)
		{
			return GetDataAdapter(query, GetConnection(this.connectionString));
		}

		/// <summary>
		/// Create a data adapter with the specified query string,
		/// a DbConnection instance, and an array of DbParameters.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="conn"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DbDataAdapter GetDataAdapter(string query, DbConnection conn, params DbParameter[] parameters)
		{
			DbDataAdapter da = providerFactory.CreateDataAdapter();
			da.SelectCommand = GetCommand(query, conn, parameters);

			return da;
		}

		/// <summary>
		/// Create a data adapter with the specified query string,
		/// a DbConnection instance, and an array of DbParameters.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public DbDataAdapter GetDataAdapter(DbCommand command)
		{
			DbDataAdapter da = providerFactory.CreateDataAdapter();
			da.SelectCommand = command;

			return da;
		}

		/// <summary>
		/// Create a data adapter with the specified query string, a
		/// DbConnection instance, and an array of DbParameters.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="connectionString"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DbDataAdapter GetDataAdapter(string query, string connectionString, params DbParameter[] parameters)
		{
			return GetDataAdapter(query, GetConnection(connectionString), parameters);
		}
		#endregion

		#region ---- GetParameter methods ----
		/// <summary>
		/// Create a database parameter using the specified parameters.
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
		public DbParameter GetParameter(string name, DbType dbType, int size,
			ParameterDirection direction, bool isNullable, byte precision, byte scale,
			string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			DbParameter p = providerFactory.CreateParameter();
			p.ParameterName = name;
			p.DbType = dbType;
			p.Size = size;
			p.Direction = direction;

			// TODO: where is isNullable, precision, and scale?
			//p.IsNullable = isNullable;
			//p.Precision = precision;
			//p.Scale = scale;

			p.SourceColumn = sourceColumn;
			p.SourceVersion = sourceVersion;
			p.Value = value == null ? (object)DBNull.Value : value;

			return p;
		}

		/// <summary>
		/// Create a database parameter using the specified parameters.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public DbParameter GetParameter(string name, DbType dbType, int size,
			ParameterDirection direction, object value)
		{
			return GetParameter(name, dbType, size, direction, false, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// create a parameter with Input direction type.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public DbParameter GetParameterIn(string name, DbType dbType, int size, object value)
		{
			return GetParameter(name, dbType, size, ParameterDirection.Input, false, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// create a parameter with Output direction type.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public DbParameter GetParameterOut(string name, DbType dbType, int size, object value)
		{
			return GetParameter(name, dbType, size, ParameterDirection.Output, false, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// create a parameter with InputOutput direction type.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public DbParameter GetParameterIO(string name, DbType dbType, int size, object value)
		{
			return GetParameter(name, dbType, size, ParameterDirection.InputOutput, false, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// create a parameter with ReturnValue direction type.
		/// </summary>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public DbParameter GetParameterRV(DbType dbType)
		{
			DbParameter p = providerFactory.CreateParameter();
			p.ParameterName = "@ReturnValue";
			p.Direction = ParameterDirection.ReturnValue;
			p.DbType = dbType;
			return p;
		}
		#endregion

		#region ---- GetCommandBuilder methods ----
		/// <summary>
		/// Create a database command builder using the
		/// specified data adapter.
		/// </summary>
		/// <param name="da"></param>
		/// <returns></returns>
		public DbCommandBuilder GetCommandBuilder(DbDataAdapter da)
		{
			if (da == null)
				throw new ArgumentNullException("da");

			DbCommandBuilder commandBuilder = providerFactory.CreateCommandBuilder();

			commandBuilder.DataAdapter = da;

			return commandBuilder;
		}

		#endregion

		#region ---- GetTransaction methods ----
		//public DbTransaction BeginTransaction(DbConnection conn, IsolationLevel level)
		//{
		//}

		//public DbTransaction GetTransaction(DbConnection conn)
		//{
		//}
		#endregion

		#endregion

		#region ---- FilterTable Methods ----
		/// <summary>
		/// Filter the DataTable using row filter and sort
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="sort"></param>
		/// <param name="rowFilter"></param>
		/// <param name="distinct"></param>
		/// <param name="columnNames"></param>
		/// <returns></returns>
		public static DataTable FilterTable(DataTable dt, string sort, string rowFilter, bool distinct, params string[] columnNames)
		{
			using (DataView dv = new DataView(dt))
			{
				dv.RowFilter = rowFilter;
				dv.Sort = sort;
				// TODO: return X number of records ...

				if (columnNames == null)
				{
					return dv.ToTable(distinct);
				}
				else
				{
					return dv.ToTable(distinct, columnNames);
				}
			}
		}
		#endregion

		#region ---- Top method ----
		/// <summary>
		/// Return the top number of rows from data table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="numberOfRecords"></param>
		/// <returns></returns>
		public static DataRow[] Top(DataTable dt, int numberOfRecords)
		{
			int max = Math.Min(dt.Rows.Count, numberOfRecords);

			DataRow[] dr = new DataRow[max];

			for (int i = 0; i < max; i++)
			{
				dr[i] = dt.Rows[i];
			}

			return dr;
		}
		#endregion

		#region ---- Last method ----
		/// <summary>
		/// Return the last number of rows from data table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="numberOfRecords"></param>
		/// <returns></returns>
		public static DataRow[] Last(DataTable dt, int numberOfRecords)
		{
			int start = Math.Max(dt.Rows.Count - numberOfRecords, 0);
			int max = dt.Rows.Count - start;

			DataRow[] dr = new DataRow[max];

			for (int i = 0; i < max; i++)
			{
				dr[i] = dt.Rows[start + i];
			}

			return dr;
		}
		#endregion

		#region ---- ScriptRunner ----
		/// <summary>
		/// Execute a group of SQL commands
		/// </summary>
		/// <param name="queries"></param>
		public void ScriptRunner(string[] queries)
		{
			try
			{
				using (DbConnection conn = GetConnection())
				{
					conn.Open();

					using (DbTransaction tran = conn.BeginTransaction())
					{

						for (int lineNum = 0; lineNum < queries.Length; lineNum++)
						{
							//ScriptCommand sc = new ScriptCommand(queries[lineNum]);
							try
							{
								ExecuteNonQuery(queries[lineNum], conn, tran);
							}
							catch (Exception ex)
							{
								DebuggerTool.AddData(ex, "lineNum", lineNum);
								DebuggerTool.AddData(ex, "queries[lineNum]", queries[lineNum]);
								tran.Rollback();
								throw;
							}
						}

						tran.Commit();
					}

					conn.Close();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "queries", DebuggerTool.Dump(queries));
				throw;
			}
		}
		#endregion

		#region ---- CreateTable ----
		/// <summary>
		/// Create a new table
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		public void CreateTable(string tablename, DataTable dt)
		{
			MacroManager.CreateNewTable(tablename, dt, true);
		}

		/// <summary>
		/// Append data to an existing table
		/// </summary>
		/// <param name="tablename"></param>
		/// <param name="dt"></param>
		public void AppendTable(string tablename, DataTable dt)
		{
			MacroManager.CreateNewTable(tablename, dt, false);
		}
		#endregion

		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		private MacroManager macroManager = null;

		/// <summary>
		/// Get the ImportManager instance
		/// </summary>
		public MacroManager MacroManager
		{
			get
			{
				if (macroManager == null)
					MacroManager = new MacroManager(this);

				return macroManager;
			}
			private set
			{
				macroManager = value;
			}
		}

		private DatabaseManager databaseManager = null;

		/// <summary>
		/// Get the DatabaseManager instance
		/// </summary>
		public DatabaseManager Database
		{
			get
			{
				if (databaseManager == null)
					Database = new DatabaseManager(this);

				return databaseManager;
			}
			private set
			{
				databaseManager = value;
			}
		}

		private TableManager tableManager = null;

		/// <summary>
		/// Get the TableManager instance
		/// </summary>
		public TableManager Table
		{
			get
			{
				if (tableManager == null)
					Table = new TableManager(this.provider, true);

				return tableManager;
			}
			private set
			{
				tableManager = value;
			}
		}

		//private SqlSpecificBase sqlSpecific = null;
		//[Obsolete("sql stuffs", true)]
		//public SqlSpecificBase SqlSpecific
		//{
		//    get
		//    {
		//        if (sqlSpecific == null)
		//            SqlSpecific = new SqlSpecificBase(this);

		//        return sqlSpecific;
		//    }
		//    private set
		//    {
		//        sqlSpecific = value;
		//    }
		//}

		/// <summary>
		/// Get the DbProviderFactory instance
		/// </summary>
		public DbProviderFactory ProviderFactory
		{
			get
			{
				return this.providerFactory;
			}
		}

		private ConnectionStringManager connectionStringManager = null;
		/// <summary>
		/// Get the ConnectionStringManager instance
		/// </summary>
		[Description("Get the ConnectionStringManager instance"), Category("DataFactory")]
		public ConnectionStringManager ConnectionStringManager
		{
			get
			{
				if (connectionStringManager == null)
				{
					ConnectionStringManager = new ConnectionStringManager(this.connectionString);
				}
				return connectionStringManager;
			}
			set
			{
				connectionStringManager = value;
			}
		}

		/// <summary>
		/// Get the provider name
		/// </summary>
		[Description("Get the provider name"), Category("DataFactory")]
		private string ProviderName
		{
			get
			{
				return Providers.ToProvider(this.provider);
			}
		}

		/// <summary>
		/// Get the database provider
		/// </summary>
		[Description("Get the database provider"), Category("DataFactory")]
		public DatabaseProvider Provider
		{
			get
			{
				return this.provider;
			}
		}

		/// <summary>
		/// Get or set the database name
		/// </summary>
		[Description("Get or set the database"), Category("DataFactory")]
		public string DatabaseName
		{
			get
			{
				return ConnectionStringManager.Database;
			}
			set
			{
				ConnectionStringManager.Database = value;
				this.connectionString = ConnectionStringManager.ConnectionString;
			}
		}
		#endregion

		#region Others
		#endregion
	}
}
