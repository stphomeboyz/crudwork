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
using crudwork.Utilities;

namespace crudwork.Executables
{
	/// <summary>
	/// Visual Studio Solution Builder
	/// </summary>
	public class SolutionBuilder
	{
	    #region Enumerators
		/// <summary>
		/// Visual Studio version
		/// </summary>
		public enum VersionTypes
		{
			/// <summary>
			/// Visual Studio 2003
			/// </summary>
			VisualStudio2003 = 8,

			/// <summary>
			/// Visual Studio 2005
			/// </summary>
			VisualStudio2005 = 9,
		}

		/// <summary>
		/// Build Type
		/// </summary>
		public enum BuildTypes
		{
			/// <summary>
			/// Release mode
			/// </summary>
			Release = 0,
			/// <summary>
			/// Debug mode
			/// </summary>
			Debug = 1,
		}
        #endregion

        #region Fields
		private List<String> log = new List<string>();
		ConfigManager config = new ConfigManager();
        #endregion

        #region Constructors
		/// <summary>
		/// Create an empty object
		/// </summary>
		public SolutionBuilder()
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
		/// <summary>
		/// Start the Build with given filename, version, and build type.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="version"></param>
		/// <param name="build"></param>
		public void Build(string filename, VersionTypes version, BuildTypes build)
		{
			// build the command to call vsvars32.bat
			string initvar;
			switch (version)
			{
				case VersionTypes.VisualStudio2003:
					initvar = config.Get("VSVARS32_2003");
					break;
				case VersionTypes.VisualStudio2005:
					initvar = config.Get("VSVARS32_2005");
					break;
				default:
					throw new ArgumentOutOfRangeException("version=" + version);
			}
			initvar = String.Format("call \"{0}\"", initvar);


			// build the command to build the solution/project
			string buildcmd = new ConfigManager().Get("buildCommand");
			buildcmd = buildcmd.Replace("{FILENAME}", StringUtil.Quote(filename));
			buildcmd = buildcmd.Replace("{BUILDTYPE}", StringUtil.Quote(build.ToString()));

			string command = initvar + Environment.NewLine + buildcmd;

			//// setting up the dos environment
			//Dictionary<string, string> dosEnv = new Dictionary<string, string>();
			//dosEnv.Add("PATH", StringUtil.Merge(MicrosoftDotNetPath, Environment.GetEnvironmentVariable("PATH"), ";"));

			// run the commands.
			ExeRunner r = new ExeRunner();
			try
			{
				r.Run(command);
			}
			catch (Exception ex)
			{
				Debug.Write(ex.ToString());
				// WARNING: absorb error only!
			}
			finally
			{
				log.AddRange(r.Log);
			}
		}
		#endregion

		#region Private Methods
		#endregion

        #region Protected Methods
        #endregion

        #region Properties
		/// <summary>
		/// Get the log entries
		/// </summary>
		[Description("Get the log entries"), Category("SolutionBuilder")]
		public string[] Log
		{
			get
			{
				return this.log.ToArray();
			}
			private set
			{
				this.log.Clear();
				this.log.AddRange(value);
			}
		}

		private string MicrosoftDotNetPath
		{
			get
			{
				return config.Get("MicrosoftDotNetPath");
			}
		}
		#endregion

		#region Others
		#endregion
	}
}
