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

namespace crudwork.DataAccess
{
	/// <summary>
	/// Interface for the DataFactory classes
	/// </summary>
	/// <typeparam name="TCommand"></typeparam>
	/// <typeparam name="TParameter"></typeparam>
	public interface IDataFactory<TCommand, TParameter>
	{
		/// <summary>
		/// Test the connection
		/// </summary>
		void TestConnection();

		/// <summary>
		/// Execute a query and return a DataSet
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		DataSet Fill(string query, params TParameter[] parameters);

		/// <summary>
		/// Execute a query and return a DataSet
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		DataSet Fill(TCommand command);

		/// <summary>
		/// Execute a query and return a DataTable
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		DataTable FillTable(string query, params TParameter[] parameters);

		/// <summary>
		/// Execute a query and return a DataTable
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		DataTable FillTable(TCommand command);

		/// <summary>
		/// Execute a query w/o any results
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		int ExecuteNonQuery(string query, params TParameter[] parameters);

		/// <summary>
		/// Execute a query w/o any results
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		int ExecuteNonQuery(TCommand command);

		/// <summary>
		/// Execute a query and return an object value
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		object ExecuteScalar(string query, params TParameter[] parameters);

		/// <summary>
		/// Execute a query and return an object value
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		object ExecuteScalar(TCommand command);

		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		IEnumerable<DataRow> ExecuteReader(string query, params TParameter[] parameters);

		/// <summary>
		/// Execute the ExecuteReader and yield return the DataRow to the caller/iterator.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		IEnumerable<DataRow> ExecuteReader(TCommand command);

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
		TParameter GetParameter(string name, DbType dbType, int size,
			ParameterDirection direction, bool isNullable, byte precision, byte scale,
			string sourceColumn, DataRowVersion sourceVersion, object value);

		#region Extra GetParameterXXX methods
		///// <summary>
		///// Create a new instance of a database parameter for Input
		///// </summary>
		///// <param name="name"></param>
		///// <param name="value"></param>
		///// <param name="size"></param>
		///// <param name="isNullable"></param>
		///// <returns></returns>
		//TParameter GetParameterIn(string name, object value, int size, bool isNullable);

		///// <summary>
		///// Create a new instance of a database parameter for Output
		///// </summary>
		///// <param name="name"></param>
		///// <param name="value"></param>
		///// <param name="size"></param>
		///// <param name="isNullable"></param>
		///// <returns></returns>
		//TParameter GetParameterOut(string name, object value, int size, bool isNullable);

		///// <summary>
		///// Create a new instance of a database parameter for InputOutput
		///// </summary>
		///// <param name="name"></param>
		///// <param name="value"></param>
		///// <param name="size"></param>
		///// <param name="isNullable"></param>
		///// <returns></returns>
		//TParameter GetParameterIO(string name, object value, int size, bool isNullable);

		///// <summary>
		///// Create a new instance of a database parameter for ReturnValue
		///// </summary>
		///// <param name="name"></param>
		///// <param name="value"></param>
		///// <param name="size"></param>
		///// <param name="isNullable"></param>
		///// <returns></returns>
		//TParameter GetParameterRV(string name, object value, int size, bool isNullable);
		#endregion
	}
}
