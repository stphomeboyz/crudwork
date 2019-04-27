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
using System.IO;
using System.Diagnostics;
using crudwork.Utilities;

namespace crudwork.Executables
{
	/// <summary>
	/// Visual SourceSafe Utility
	/// </summary>
	public class SourceSafe
	{
		#region Enumerators
		#endregion

		#region Fields
		private string executeFile;
		private string vssCommandTemplate;
		private Dictionary<String, String> dosEnv;

		private List<String> log = new List<string>();

		private string username;
		private string password;
		#endregion

		#region Constructors
		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="executeFile"></param>
		/// <param name="vssServer"></param>
		/// <param name="commandTemplate"></param>
		public SourceSafe(string executeFile, string vssServer, string commandTemplate)
		{
			try
			{
				ExecuteFile = executeFile;

				/*
				 * Parameter hints:
				 * 
				 * -I-		Do not ask for input under any circumstance
				 * -R		recursive
				 * -W		make the retrieved local copy writable
				 * -GL		folder to check in
				 * -GCD		Date and time comparison is used to determine whether the local copy of the file is current.
				 * -GTM		Local copy is given the date and time that the file was last modified, not the current date and time.
				 * 
				 * For more info, check with VSS help doc.
				 * */
				//VssCommandTemplate = "\"{SS}\" get \"{ITEM}\" -I- -R -W -GL\"{FOLDER}\" -GCD -GTM"; // -Y{USERNAME},{PASSWORD}";
				vssCommandTemplate = commandTemplate;

				//set SSDIR=\\la1dev5\MSnetVSS
				dosEnv = new Dictionary<string, string>();
				dosEnv.Add("SSDIR", vssServer);
			}
			catch (Exception ex)
			{
				// absorb error
				Debug.Write(ex.ToString());
			}
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
		/// Retrieve items from VSS and return a list of filenames.
		/// </summary>
		/// <param name="outputFolder"></param>
		/// <param name="vssItem"></param>
		/// <returns></returns>
		public string[] GetFiles(string outputFolder, string vssItem)
		{
			try
			{
				#region Sanity Checks
				if (String.IsNullOrEmpty(outputFolder))
					throw new ArgumentNullException("outputFolder");

				if (File.Exists(outputFolder))
					throw new ArgumentException("Invalid Folder: file already existed");
				#endregion

				string cmd = VssCommand(outputFolder, vssItem);

				if (!Directory.Exists(outputFolder))
					Directory.CreateDirectory(outputFolder);

				ExeRunner r = new ExeRunner();

				log.Clear();

				try
				{
					r.Run(cmd, dosEnv);
				}
				finally
				{
					log.AddRange(r.Log);
				}

				return Directory.GetFiles(outputFolder, "*.*", SearchOption.AllDirectories);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Save the log file and return the filename.
		/// </summary>
		/// <returns></returns>
		public string SaveLog()
		{
			return FileUtil.CreateTempFile("log", log.ToArray());
		}
		#endregion

		#region Private Methods
		private string VssCommand(string outputFolder, string vssItem)
		{
			string s = VssCommandTemplate;

			s = s.Replace("{SS}", StringUtil.Quote(ExecuteFile));
			s = s.Replace("{ITEM}", StringUtil.Quote(vssItem));
			s = s.Replace("{FOLDER}", StringUtil.Quote(outputFolder));

			// special for User/Password switch (-Yadda,yadda)
			if (!String.IsNullOrEmpty(username))
			{
				s = String.Format("{0} -Y{1},{2}", s, username, password);
			}

			return s;
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the path to the VSS executable file
		/// </summary>
		[Description("Get or set the path to the VSS executable file"), Category("SourceSafe")]
		public string ExecuteFile
		{
			get
			{
				return executeFile;
			}
			set
			{
				if (!File.Exists(value))
					throw new FileNotFoundException(value);

				executeFile = value;
			}
		}

		/// <summary>
		/// Get the executable VSS command line
		/// </summary>
		[Description("Get the executable VSS command line"), Category("SourceSafe")]
		public string VssCommandTemplate
		{
			get
			{
				return vssCommandTemplate;
			}
			private set
			{
				vssCommandTemplate = value;
			}
		}

		/// <summary>
		/// Get or set the username
		/// </summary>
		[Description("Get or set the username"), Category("SourceSafe")]
		public string Username
		{
			get
			{
				return username;
			}
			set
			{
				username = value;
			}
		}

		/// <summary>
		/// Get or set the password
		/// </summary>
		[Description("Get or set the password"), Category("SourceSafe")]
		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}
		#endregion

		#region Others
		#endregion
	}
}
