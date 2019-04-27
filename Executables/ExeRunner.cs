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
using System.Diagnostics;
using System.Security;
using System.IO;
using System.Threading;
using crudwork.Utilities;

namespace crudwork.Executables
{
	/// <summary>
	/// Executable Runner
	/// </summary>
	public class ExeRunner
	{
		#region Enumerators
		#endregion

		#region Fields
		/// <summary>
		/// enable or disable standard input/output/error redirection.
		/// </summary>
		private bool enableStandardRedirection = true;
		/// <summary>
		/// containing logging information
		/// </summary>
		private List<String> log = new List<string>();
		/// <summary>
		/// wait this amount of time (in seconds) for a process execution.
		/// </summary>
		private int executionWaitAmount = 600;
		/// <summary>
		/// the process' exit code.
		/// </summary>
		private int exitCode;
		#endregion

		#region Constructors
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
		/// run a command under the specify user's account.  Specify user to 'null'
		/// to run under the current authentication.  Also support for setting the
		/// DOS environment variables.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dosEnv"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public void Run(string command, Dictionary<String, String> dosEnv, UserAccount user)
		{
			log = new List<string>();

			try
			{
				using (Process p = new Process())
				{
					// create a batch file ...
					string batchFile = FileUtil.CreateTempFile("bat", new string[] { command });


					// setup the command-lines ...
					p.StartInfo.FileName = Environment.GetEnvironmentVariable("COMSPEC");
					// use two doublequote to support spaces in filename/folder.
					p.StartInfo.Arguments = String.Format("/c \"\"{0}\"\"", batchFile);
					LogMessage("Filename/Arguments: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments, false);

					// runas user ...
					{
						string username;

						if (user == null)
						{
							// use current auth, nothing to do!
							username = String.Format("{1}\\{0}", Environment.UserName, Environment.UserDomainName);
						}
						else
						{
							p.StartInfo.UserName = user.Username;
							p.StartInfo.Password = user.Password;
							p.StartInfo.Domain = user.Domain;
							username = user.Fullname;
							//user.Password.Copy();
						}
						LogMessage("Run as : " + username, false);
					}

					// enable standard i/o/e redirection ...
					if (enableStandardRedirection)
					{
						LogMessage("Enabling standard output/error redirection.", false);
						p.StartInfo.UseShellExecute = false;
						p.StartInfo.RedirectStandardOutput = true;
						p.StartInfo.RedirectStandardError = true;
						p.StartInfo.RedirectStandardInput = true;

						// do not show dos prompt ...
						p.StartInfo.CreateNoWindow = true;
					}

					// set environment variables ...
					if ((dosEnv != null) && (dosEnv.Count>0))
					{
						System.Collections.Specialized.StringDictionary var = p.StartInfo.EnvironmentVariables;

						foreach (KeyValuePair<String, String> kv in dosEnv)
						{
							if (var.ContainsKey(kv.Key))
								var.Remove(kv.Key);

							var.Add(kv.Key, kv.Value);
						}
					}

					// start the process here ...
					LogMessage("Executing command: " + command, false);
					LogMessage("");
					p.Start();

					// handle standard i/o/e redirection ...
					if (enableStandardRedirection)
					{
						// force Ctrl-Z to standard input, just in case the application prompts
						// for input.  This will prevent an application from hanging.
						p.StandardInput.Close();

						ProcessRedirectStream(p.StandardOutput, "Standard Output");
						ProcessRedirectStream(p.StandardError, "Standard Error");
					}

					// wait for the specify amount of time ...
					if (ExecutionWaitAmount > 0)
					{
						// converts seconds --> milliseconds.
						p.WaitForExit(ExecutionWaitAmount * 1000);
					}

					// check exit status ...
					if (p.HasExited)
					{
						LogMessage("Application closed gracefully.");
					}
					else
					{
						#region Perform extra precautions
						if (!p.HasExited)
						{
							LogMessage("Sending the close.");
							p.CloseMainWindow();
							Thread.Sleep(500);
						}

						if (!p.HasExited)
						{
							LogMessage("NOTE: Application not responding.  Sending the kill.");
							p.Kill();
							Thread.Sleep(500);
						}

						if (!p.HasExited)
						{
							LogMessage("WARNING: Unable to kill application.  (Application must be closed manually!)");
						}
						#endregion
					}
					LogMessage("");

					ExitCode = p.ExitCode;

					LogProcessInfo(p);

					//return log.ToArray();
				}
			}
			catch (Exception ex)
			{
				LogMessage("Error: " + ex.ToString());
				throw;
			}
		}

		/// <summary>
		/// Run a command with the current authentication.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="dosEnv"></param>
		/// <returns></returns>
		public void Run(string command, Dictionary<String, String> dosEnv)
		{
			try
			{
				Run(command, dosEnv, null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Execute the command under the given user account
		/// </summary>
		/// <param name="command"></param>
		/// <param name="user"></param>
		public void Run(string command, UserAccount user)
		{
			try
			{
				Run(command, null, user);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Execute the command under the given user account
		/// </summary>
		/// <param name="command"></param>
		public void Run(string command)
		{
			try
			{
				Run(command, null, null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Log messages (with or without timestamp)
		/// </summary>
		/// <param name="message"></param>
		/// <param name="useStampDate"></param>
		private void LogMessage(string message, bool useStampDate)
		{
			string s;

			if (String.IsNullOrEmpty(message))
			{
				s = "";
			}
			else if (useStampDate)
			{
				s = String.Format("{0} : {1}", DateTime.Now.ToString(), message);
			}
			else
			{
				s = message;
			}
			log.Add(s);
		}

		/// <summary>
		/// Log messages timestamp.
		/// </summary>
		/// <param name="message"></param>
		private void LogMessage(string message)
		{
			LogMessage(message, true);
		}

		/// <summary>
		/// Log the redirected stream
		/// </summary>
		/// <param name="streamReader"></param>
		/// <param name="title"></param>
		private void ProcessRedirectStream(StreamReader streamReader, string title)
		{
			try
			{
				string crlf = Environment.NewLine;
				using (StreamReader r = streamReader)
				{
					StringBuilder s = new StringBuilder();
					s.Length = 0;

					// process faster if reading one line at a time.
					while (!r.EndOfStream)
					{
						s.AppendFormat("{1}{0}", crlf, StringUtil.RemoveNulls(r.ReadLine()));
					}

					if (s.Length == 0)
					{
						string na = "<<<< ---- N/A ---- >>>>";
						s.AppendFormat("{0}{1}{0}", crlf, na);
					}

					s.Insert(0, String.Format("[Begin of {1}]", crlf, title));
					s.AppendFormat("[End of {1}]{0}", crlf, title);
					LogMessage(s.ToString(), false);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
			finally
			{
				streamReader.Dispose();
			}
		}

		/// <summary>
		/// Log process information
		/// </summary>
		/// <param name="p"></param>
		private void LogProcessInfo(Process p)
		{
			String crlf = Environment.NewLine;
			StringBuilder s = new StringBuilder();
			TimeSpan elapsed = new TimeSpan(p.ExitTime.Ticks - p.StartTime.Ticks);

			s.AppendFormat("[Begin of Process Information]{0}", crlf);
			//s.AppendFormat("Priority = {1}{0}", crlf, p.BasePriority);
			s.AppendFormat("ExitCode = {1}{0}", crlf, p.ExitCode);

			s.AppendFormat("StartTime = {1}{0}", crlf, p.StartTime);
			s.AppendFormat("ExitTime  = {1}{0}", crlf, p.ExitTime);
			s.AppendFormat("ElapsedTime = {1}{0}", crlf, elapsed.ToString());

			s.AppendFormat("TotalProcessorTime = {1}{0}", crlf, p.TotalProcessorTime);
			s.AppendFormat("UserProcessorTime  = {1}{0}", crlf, p.UserProcessorTime);

			s.AppendFormat("MachineName = {1}{0}", crlf, p.MachineName);
			s.AppendFormat("[End of Process Information]{0}", crlf);

			LogMessage(s.ToString(), false);
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		/// <summary>
		/// Enable or disable the redirection of standard input/output/error.
		/// </summary>
		[Description("Enable or disable the redirection of standard input/output/error"), Category("ExeRunner")]
		public bool EnableStandardOutput
		{
			get
			{
				return this.enableStandardRedirection;
			}
			set
			{
				this.enableStandardRedirection = value;
			}
		}

		/// <summary>
		/// Get or set the execution wait amount (in seconds)
		/// </summary>
		[Description("Get or set the execution wait amount (in seconds)"), Category("ExeRunner")]
		public int ExecutionWaitAmount
		{
			get
			{
				return executionWaitAmount;
			}
			set
			{
				executionWaitAmount = value;
			}
		}

		/// <summary>
		/// Get or set the command result code (exitcode)
		/// </summary>
		[Description("Get or set the command result code (exitcode)"), Category("ExeRunner")]
		public int ExitCode
		{
			get
			{
				return exitCode;
			}
			set
			{
				exitCode = value;
			}
		}

		/// <summary>
		/// Get the log string array
		/// </summary>
		[Description("Get the log string array"), Category("ExeRunner")]
		public string[] Log
		{
			get
			{
				return this.log.ToArray();
			}
		}
		#endregion

		#region Others
		#endregion
	}
}
