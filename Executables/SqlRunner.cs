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
using System.Diagnostics;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using crudwork.DataAccess;
using crudwork.Utilities;
using crudwork.Models.DataAccess;

namespace crudwork.Executables
{
	/// <summary>
	/// Provides method to execute SQL queries in transaction mode.
	/// </summary>
	public class SqlRunner
	{
		#region Enumerators
		/// <summary>
		/// Execute type
		/// </summary>
		public enum ExecuteTypes
		{
			//Unspecified = 0,

			/// <summary>
			/// Execute Scalar
			/// </summary>
			ExecuteScalar = 1,

			/// <summary>
			/// Execute No Query
			/// </summary>
			ExecuteNonQuery = 2,
		}
		#endregion

		#region Fields
		private DataFactory dataFactory;
		private string connectionString;
		#endregion

		#region Constructors
		/// <summary>
		/// Create an empty object
		/// </summary>
		public SqlRunner()
		{
			dataFactory = new DataFactory(DatabaseProvider.SqlClient);
			connectionString = new ConfigManager().Get("connectionString");
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
		/// <summary>
		/// Execute a list of script buffers under transaction mode.
		/// </summary>
		/// <param name="buffers"></param>
		/// <param name="isolationLevel"></param>
		/// <param name="executeType"></param>
		public void ExecuteBufferTran(string[] buffers, IsolationLevel? isolationLevel, ExecuteTypes executeType)
		{
			#region Sanity Checks
			if ((buffers == null) || (buffers.Length == 0))
				throw new ArgumentNullException("buffers");
			#endregion

			DbConnection conn = null;
			DbTransaction tran = null;

			try
			{
				conn = dataFactory.GetConnection(ConnectionString);
				conn.Open();

				if (!isolationLevel.HasValue)
				{
					tran = conn.BeginTransaction();
				}
				else
				{
					tran = conn.BeginTransaction(isolationLevel.Value);
				}

				for (int i = 0; i < buffers.Length; i++)
				{
					string query = buffers[i];

					if (String.IsNullOrEmpty(query))
						continue;

					// split the script out on the 'GO' statement
					string[] subQueries = breakStatements(query);

					//string[] subQueries;
					//{
					//    string findToken = String.Format("{0}{1}{0}", Environment.NewLine, "GO");
					//    //string replToken = String.Format("{0}{1}{0}", Environment.NewLine, "-- GO");
					//    subQueries = query.Split(new string[] { findToken },  StringSplitOptions.RemoveEmptyEntries);
					//}

					try
					{
						for (int j = 0; j < subQueries.Length; j++)
						{
							string subQuery = subQueries[j];
							switch (executeType)
							{
								case ExecuteTypes.ExecuteNonQuery:
									dataFactory.ExecuteNonQuery(subQuery, conn, tran);
									break;
								case ExecuteTypes.ExecuteScalar:
									dataFactory.ExecuteScalar(subQuery, conn, tran);
									break;
								default:
									throw new ArgumentOutOfRangeException("executeType=" + executeType);
							}
						}
					}
					catch (Exception ex)
					{
						// wrap error with filename info.
						Exception ex2 = new ApplicationException("Buffer index " + i, ex);
						ex2.Data.Add("index", i);
						throw ex2;
					}
				}

				tran.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				if (tran != null)
					tran.Rollback();
				Debug.WriteLine(ex.Message);
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (conn != null)
					conn.Dispose();
			}
		}

		/// <summary>
		/// Execute a list of script files under transaction mode.
		/// </summary>
		/// <param name="filenames"></param>
		/// <param name="isolationLevel"></param>
		/// <param name="executeType"></param>
		public void ExecuteFileTran(string[] filenames, IsolationLevel? isolationLevel, ExecuteTypes executeType)
		{
			#region Sanity Checks
			if ((filenames == null) || (filenames.Length == 0))
				throw new ArgumentNullException("filenames");
			#endregion

			try
			{
				List<String> buffers = new List<string>();

				for (int i = 0; i < filenames.Length; i++)
				{
					buffers.Add(StringUtil.StringArrayToString(FileUtil.ReadFile(filenames[i])));
				}

				try
				{
					ExecuteBufferTran(buffers.ToArray(), isolationLevel, executeType);
				}
				catch (Exception ex)
				{
					// wrap more error
					int idx = int.Parse(ex.Data["index"].ToString());

					Exception ex2 = new ApplicationException("Filename index: " + filenames[idx], ex.InnerException);
					ex2.Data.Add("index", idx);
					throw ex2;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Execute a list of script files under transaction mode (using default
		/// isolation level.)
		/// </summary>
		/// <param name="filenames"></param>
		public void ExecuteFileTran(string[] filenames)
		{
			ExecuteFileTran(filenames, null, ExecuteTypes.ExecuteNonQuery);
		}

		/// <summary>
		/// This method attempts to order a list of script files for batch-execution under
		/// ACID transaction mode.
		/// <para>
		/// It performs this by running a list of scripts under transcation (ACID) mode.
		/// If one of the script fail to process successfully (due to missing requirements
		/// or other erroneous reason), it is moved to the end of the list.  And the test
		/// is re-run with the new order.
		/// </para>
		/// <para>
		/// The loop continues each time with a different order until the order is
		/// fully exhausted.  Upon completion, it returns the pivotalIndex int value.
		/// </para>
		/// <para>
		/// The pivotalIndex value marks the starting/ending spot to divide the list of
		/// scripts of usable/unusable scripts.
		/// </para>
		/// <para>
		/// If all scripts ran successfully, the pivotalIndex is equal to the total number
		/// of scripts passed (ie: the array's length).  If pivotalIndex is equals to 0,
		/// all of the scripts failed, apparently.
		/// </para>
		/// <para>
		/// For example, if you have 10 filename and the pivotalIndex is 6, it means the
		/// filenames[0 thru 5] were successful and the filenames[6 thru 9] were error.
		/// </para>
		/// </summary>
		/// <param name="filenames"></param>
		/// <param name="successList"></param>
		/// <param name="failList"></param>
		/// <param name="logfile"></param>
		/// <returns></returns>
		public int ExecuteFileTranSmart(
			string[] filenames,
			out string[] successList,
			out string[] failList,
			string logfile
			)
		{

			List<String> fileList = new List<string>();
			List<String> buffList = new List<string>();

			for (int i = 0; i < filenames.Length; i++)
			{
				fileList.Add(filenames[i]);
				buffList.Add(StringUtil.StringArrayToString(FileUtil.ReadFile(filenames[i])));
			}

			// delete log file
			File.Delete(logfile);

			int max = filenames.Length;
			int loopCounter = 0;
			int lastErrorIndex = -1;
			int pivotalIndex = 0;

			while (loopCounter < max)
			{
				try
				{
					ExecuteBufferTran(buffList.ToArray(), null, ExecuteTypes.ExecuteNonQuery);
					pivotalIndex = buffList.Count;
					break;
				}
				catch (Exception ex)
				{
					#region The Smart Algorithm
					try
					{
						if ((ex.Data.Contains("index")) &&
							(int.TryParse(ex.Data["index"].ToString(), out pivotalIndex)))
						{
							string tempFile = fileList[pivotalIndex];
							string tempBuff = buffList[pivotalIndex];

							fileList.RemoveAt(pivotalIndex);
							buffList.RemoveAt(pivotalIndex);

							fileList.Add(tempFile);
							buffList.Add(tempBuff);

							// check for infinite loop ...
							if (pivotalIndex == lastErrorIndex)
							{
								loopCounter++;
							}
							else
							{
								lastErrorIndex = pivotalIndex;
								loopCounter = pivotalIndex;
							}

							#region Logging to File
							try
							{
								StringBuilder sfoo = new StringBuilder();
								Exception exfoo = ex;

								sfoo.AppendFormat("moving to end of list: {1} | {2} | {3} | {4}{0}",
									Environment.NewLine,
									pivotalIndex, loopCounter, max, tempFile);

								while (exfoo != null)
								{
									sfoo.AppendFormat("Message: {1}{0}", Environment.NewLine, exfoo.Message);
									exfoo = exfoo.InnerException;
								}


								// move idx to the end of the list.
								FileUtil.AppendFile(logfile, sfoo.ToString());
							}
							catch (Exception ex3)
							{
								throw new ApplicationException("Error logging", ex3);
							}
							#endregion

							continue;
						}
					}
					catch (Exception ex2)
					{
						throw new ApplicationException("Error in the Smart Algorithm", ex2);
					}
					#endregion
				}
			}

			// create successList and failList
			if (pivotalIndex > 0)
				successList = StringUtil.CopyList(fileList, 0, pivotalIndex - 1);
			else
				successList = new string[0];

			failList = StringUtil.CopyList(fileList, pivotalIndex, fileList.Count - 1);

			return pivotalIndex;
		}

		/// <summary>
		/// Merge the fileList for OSQL.
		/// </summary>
		/// <param name="outFile"></param>
		/// <param name="fileList"></param>
		public void MergeScripts(string outFile, string[] fileList)
		{
			string uniqueTran = "T" + StringUtil.RandomKey();
			string separatorLine = StringUtil.FillChar('-', 79);
			try
			{
				using (StreamWriter w = new StreamWriter(outFile, false))
				{
					w.WriteLine("begin tran " + uniqueTran);


					for (int i = 0; i < fileList.Length; i++)
					{
						string inFile = fileList[i];

						// write header info
						w.WriteLine();
						w.WriteLine(separatorLine);

						w.WriteLine(String.Format("-- Filename #{1} of {2} : {0}",
							inFile, i + 1, fileList.Length));

						w.WriteLine(separatorLine);
						w.WriteLine();


						// read / write
						foreach (var item in FileUtil.ReadFile(inFile))
						{
							w.WriteLine(item);
						}
					}

					w.WriteLine("commit tran " + uniqueTran);

					w.Flush();
					w.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Private Methods
		private string[] breakStatements(string query)
		{
			string[] inputList = StringUtil.StringToStringArray(query);
			List<String> results = new List<string>();
			StringBuilder s = new StringBuilder();

			for (int i = 0; i < inputList.Length; i++)
			{
				string input = inputList[i];

				if (String.IsNullOrEmpty(input))
				{
					continue;
				}

				if (Regex.IsMatch(input, "^[ \t]*GO[ \t]*$", RegexOptions.IgnoreCase | RegexOptions.Singleline))
				{
					// break s here ...
					results.Add(s.ToString());
					s.Length = 0;
					continue;
				}

				// append to s ...
				if (s.Length > 0)
					s.Append(Environment.NewLine);
				s.AppendFormat("{1}{0}", Environment.NewLine, input);
			}

			return results.ToArray();
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the connection string
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return this.connectionString;
			}
			set
			{
				this.connectionString = value;
			}
		}
		#endregion

		#region Others
		#endregion

	}
}
