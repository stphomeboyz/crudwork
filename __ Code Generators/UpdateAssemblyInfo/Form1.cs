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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using crudwork.Utilities;
using System.Diagnostics;
using crudwork.Controls;

namespace UpdateAssemblyInfo
{
	public partial class Form1 : Form
	{
		private Dictionary<string, string> _replacementTable = null;
		private bool runBatchMode = false;
		private bool closeUponCompletion = false;
		private StringBuilder log = new StringBuilder();
		private string[] args;
		private string settingFilename = null;

		#region Constructors
		public Form1(string[] args)
			: this()
		{
			this.args = args;
		}

		public Form1()
		{
			InitializeComponent();
		}
		#endregion

		#region Event handlers
		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				btnSave.Enabled = false;

				ParseCommandLineArguments(args);

				if (runBatchMode)
				{
					if (closeUponCompletion)
						this.Visible = false;

					btnApply_Click(sender, e);

					if (closeUponCompletion)
						Application.Exit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Application.ProductName);
				this.Close();
			}
		}

		private void btnIncreaseVersion_Click(object sender, EventArgs e)
		{
			try
			{
				var b = sender as Button;
				var action = b.Tag.ToString();

				Version = IncreaseVersion(Version, action);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			try
			{
				using (var d = new OpenFileDialog())
				{
					d.Title = "Open Settings ...";
					d.Filter = "XML Settings File|*.versionxml";
					if (d.ShowDialog() != DialogResult.OK)
						return;

					LoadSettings(d.FileName);
					btnSave.Enabled = true;
					//MessageBox.Show("Settings loaded successfully.");
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(settingFilename))
					return;

				SaveSettings(settingFilename);
				MessageBox.Show("Settings was saved successfully.");
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnSaveAs_Click(object sender, EventArgs e)
		{
			try
			{
				using (var d = new SaveFileDialog())
				{
					d.Title = "Save Settings As ...";
					d.Filter = "XML Settings File|*.xml";
					if (d.ShowDialog() != DialogResult.OK)
						return;

					SaveSettings(d.FileName);
					//MessageBox.Show("Settings was saved successfully.");
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			try
			{
				#region Sanity Checks
				if (string.IsNullOrEmpty(SolutionFolder))
					throw new ArgumentException("Solution Folder is not specified.");

				if (!Directory.Exists(SolutionFolder))
					throw new ArgumentException("Solution folder does not exist.");

				if (!Regex.IsMatch(Version, @"^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$", RegexOptions.Singleline))
					throw new ArgumentException("Version is not valid.");
				#endregion

				_replacementTable = null;
				log.Length = 0;
				int nfChanges = UpdateAllAssemblyInfo();

				if (!closeUponCompletion)
				{
					ControlManager.ShowTextDialog(log, "Updated " + nfChanges + " file(s)");
				}

				if (!string.IsNullOrEmpty(settingFilename))
					SaveSettings(settingFilename);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		#endregion

		#region Helper methods
		private int UpdateAllAssemblyInfo()
		{
			List<string> files = new List<string>();
			//files.AddRange(FileUtil.GetFiles(SolutionFolder, "AssemblyInfo.cs; AssemblyInfo.vb", SearchOption.AllDirectories, false, FileUtil.FileOrderType.None));
			files.AddRange(Directory.GetFiles(SolutionFolder, "AssemblyInfo.vb", SearchOption.AllDirectories));
			files.AddRange(Directory.GetFiles(SolutionFolder, "AssemblyInfo.cs", SearchOption.AllDirectories));

			int nfChanges = 0;
			foreach (var item in files)
			{
				if (UpdateAssemblyInfo(item))
					nfChanges++;
			}

			return nfChanges;
		}
		private bool UpdateAssemblyInfo(string filename)
		{
			try
			{
				Log("Scanning " + filename);

				#region Open / Save filename
				List<string> buffer = new List<string>();

				int nrChanges = 0;
				foreach (var inputLine in FileUtil.ReadFile(filename))
				{
					string outputLine = Replace(inputLine, ReplacementTable);
					buffer.Add(outputLine);

					if (inputLine != outputLine)
					{
						nrChanges++;
						Log(string.Format(" #{0:00} << {1}", nrChanges, inputLine));
						Log(string.Format(" #{0:00} >> {1}", nrChanges, outputLine));
					}
				}

				if (nrChanges > 0)
				{
					FileUtil.WriteFile(filename, buffer.ToArray());
					Log("  Changes were updated.");
				}
				else
				{
					Log("  No changes were made.");
				}

				Log(string.Empty);
				#endregion

				return nrChanges > 0;
			}
			catch (Exception ex)
			{
				Log("  Error: " + ex.Message);
				return false;
			}
		}
		private string Replace(string item, Dictionary<string, string> replaceTable)
		{
			StringBuilder sb = new StringBuilder(item);

			foreach (KeyValuePair<string, string> kv in replaceTable)
			{
				string regexStr = string.Format(@"^[ \t]*[\[\<][Aa]ssembly\: {0}\(""(?<Value>.*)""\)[\]\>][ \t]*$", kv.Key);
				var mc = Regex.Matches(item, regexStr);
				if (mc.Count == 0)
					continue;

				foreach (Match m in mc)
				{
					var g = m.Groups["Value"];

					if (string.IsNullOrEmpty(g.Value))
					{
						sb.Insert(g.Index, kv.Value);
					}
					else
					{
						sb.Replace(g.Value, kv.Value);
					}
				}
			}

			return sb.ToString();
		}

		private void LoadSettings(string filename)
		{
			settingFilename = filename;
			var buffer = File.ReadAllBytes(filename);
			AssemblyInfo = Serializer.Deserialize<AssemblyInformation>(buffer, Serializer.SerializeMethods.Xml);
		}
		private void SaveSettings(string filename)
		{
			var buffer = Serializer.Serialize(AssemblyInfo, Serializer.SerializeMethods.Xml);
			File.WriteAllLines(filename, new string[] { buffer });
		}

		private string IncreaseVersion(string curVersion, string action)
		{
			string[] ver = StringUtil.SplitDelimiter(curVersion, '.');
			int[] veri = new int[ver.Length];

			for (int i = 0; i < veri.Length; i++)
			{
				veri[i] = DataConvert.ToInt32(ver[i]);
			}

			switch (action.ToUpper())
			{
				case "A":
				case "MAJOR":
					veri[0] = veri[0] + 1;
					break;

				case "B":
				case "MINOR":
					veri[1] = veri[1] + 1;
					break;

				case "C":
				case "REVISION":
					veri[2] = veri[2] + 1;
					break;

				case "D":
				case "BUILD":
					veri[3] = veri[3] + 1;
					break;
				default:
					throw new ArgumentException("invalid action: " + action);
			}

			return string.Format("{0}.{1}.{2}.{3}", veri[0], veri[1], veri[2], veri[3]);

		}

		private void ParseCommandLineArguments(string[] args)
		{
			try
			{
				/*
				 * Usage: UpdateAssemblyInfo
				 * 
				 *		/S <Filename>		- specify the XML settings file
				 *		
				 * -- Batch options --
				 *		/B					- Update files when application starts up (in batch mode)
				 *		/E					- Close application upon successful completion (for used with batch mode)
				 *
				 * */

				for (int i = 0; i < args.Length; i++)
				{
					string p = args[i];

					if (p == "--")
						break;

					if (p.StartsWith("/"))
					{
						switch (p.ToUpper())
						{
							case "/S":
								if (i + 1 >= args.Length)
									throw new ArgumentException("expected parameter for /S missing");
								LoadSettings(args[++i]);
								break;
							
							case "/B":
								runBatchMode = true;
								break;
							
							case "/E":
								closeUponCompletion = true;
								break;

							case "/I":
								Version = IncreaseVersion(Version, args[++i]);
								break;
							
							case "/?":
								string msg = @"usage: UpdateAssemblyInfo [options]

/S filename - load the XML settings file
/B          - run in batch mode.  Run immediately when application starts up.
/E          - close application upon successful completion (used with /B)
/I [MODE]   - increase the version by the mode specified
                  A or MAJOR    - major number
                  B or MINOR    - minor number
                  C or REVISION - revision number
                  D or BUILD    - build number
";
								throw new ArgumentException("HELP " + msg);

							default:
								throw new ArgumentException("unknown option: " + p);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				throw;
			}
		}
		private void Log(string value)
		{
			log.AppendLine(value);
		}
		#endregion

		#region Properties
		private Dictionary<string, string> ReplacementTable
		{
			get
			{
				if (_replacementTable == null)
				{
					_replacementTable = new Dictionary<string, string>();

					_replacementTable.Add("AssemblyDescription", ExpandVar(Description));
					_replacementTable.Add("AssemblyCompany", ExpandVar(Company));
					_replacementTable.Add("AssemblyProduct", ExpandVar(Product));
					_replacementTable.Add("AssemblyCopyright", ExpandVar(Copyright));
					_replacementTable.Add("AssemblyVersion", ExpandVar(Version));
					_replacementTable.Add("AssemblyFileVersion", ExpandVar(Version));

					//_replacementTable.Add("AssemblyDescription", "crudwork is a collection of handy tools (for Microsoft .NET Framework CLR 2, CLR 3.5)");
					//_replacementTable.Add("AssemblyCompany", "Steve T Pham, crudwork.com");
					//_replacementTable.Add("AssemblyProduct", "Steve's Handy .NET Library");
					//_replacementTable.Add("AssemblyCopyright", string.Format(@"Copyright (C) 2004 - [YEAR] Steve T. Pham", DateTime.Now.Year));
					//_replacementTable.Add("AssemblyVersion", Version);
					//_replacementTable.Add("AssemblyFileVersion", Version);
				}

				return _replacementTable;
			}
		}

		private string ExpandVar(string value)
		{
			var sb = new StringBuilder(value);
			sb.Replace("[YEAR]", DateTime.Now.Year.ToString());
			return sb.ToString();
		}

		private AssemblyInformation AssemblyInfo
		{
			get
			{
				return new AssemblyInformation()
				{
					SolutionFolder = this.SolutionFolder,
					Version = this.Version,
					Description = this.Description,
					Company = this.Company,
					Product = this.Product,
					Copyright = this.Copyright,
				};
			}
			set
			{
				if (value == null)
					value = new AssemblyInformation();

				this.SolutionFolder = value.SolutionFolder;
				this.Version = value.Version;
				this.Description = value.Description;
				this.Company = value.Company;
				this.Product = value.Product;
				this.Copyright = value.Copyright;
			}
		}

		private string SolutionFolder
		{
			get
			{
				return txtSolutionFolder.Text;
			}
			set
			{
				txtSolutionFolder.Text = value;
			}
		}
		private string Version
		{
			get
			{
				return txtVersion.Text;
			}
			set
			{
				txtVersion.Text = value;
			}
		}
		private string Description
		{
			get
			{
				return txtDescription.Text;
			}
			set
			{
				txtDescription.Text = value;
			}
		}
		private string Company
		{
			get
			{
				return txtCompany.Text;
			}
			set
			{
				txtCompany.Text = value;
			}
		}
		private string Product
		{
			get
			{
				return txtProduct.Text;
			}
			set
			{
				txtProduct.Text = value;
			}
		}
		private string Copyright
		{
			get
			{
				return txtCopyright.Text;
			}
			set
			{
				txtCopyright.Text = value;
			}
		}
		#endregion
	}
}
