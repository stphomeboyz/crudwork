using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;

namespace WhereIs
{
	class Program
	{
		private static string[] executableExtension;

		static Program()
		{
			string ext = ConfigurationManager.AppSettings["ExecutableExtensions"].ToString();

			if (String.IsNullOrEmpty(ext))
			{
				ext = "exe,com,bat";
			}

			executableExtension = ext.Split(',');
		}

		static int Main(string[] args)
		{
			string[] myPath = GetPath();
			List<String> executables;

			if (args.Length == 0)
			{
				ShowUsage();
				return 2;
			}

			if (args[0] == "--list")
			{
				ShowPath(myPath);
				return 2;
			}

			string executableFile = args[0];
			int totalFound = 0;

			//Console.WriteLine("Looking for: " + executableFile);
			for (int i = 0; i < myPath.Length; i++)
			{
				//Console.WriteLine(myPath[i]);
				executables = FindExecutables(myPath[i], executableFile);
				for (int y = 0; y < executables.Count; y++)
				{
					Console.WriteLine(executables[y]);
				}

				totalFound += executables.Count;
			}

			Console.WriteLine("Total found: {0}", totalFound);

			return totalFound > 0 ? 0 : 1;
		}

		public static List<String> FindExecutables(string folder, string filename)
		{
			List<String> allFilesFound = new List<String>();

			for (int i = 0; i < executableExtension.Length; i++)
			{
				string fullFilename;
				string[] filesFound;

				fullFilename = MergeExtension(filename, executableExtension[i]);

				try
				{
					filesFound = Directory.GetFiles(folder, fullFilename);
				}
				catch
				{
					filesFound = new string[0];
				}

				if (filesFound.Length == 0)
					continue;

				allFilesFound.AddRange(filesFound);
			}

			return allFilesFound;
		}

		public static string MergeExtension(string filestem, string extension)
		{
			if (filestem.IndexOf(".") > 0)
			{
				return filestem;
			}
			else
			{
				return filestem + "." + extension;
			}
		}

		public static string[] GetPath()
		{
			List<String> list = new List<string>();
			list.Add(".");
			list.AddRange(Environment.GetEnvironmentVariable("PATH").Split(';'));
			return list.ToArray();
			;
		}

		private static void ShowPath(string[] myPath)
		{
			string s;
			for (int i = 0; i < myPath.Length; i++)
			{
				s = String.Format("{0:00}: {1}", i + 1, myPath[i]);
				Console.WriteLine(s);
			}
		}

		public static void ShowUsage()
		{
			Console.WriteLine(
@"usage:
		whereis executable
		whereis --list

Specify the executable for searching.  If the extension is not specified,
program will attempt to look for a filename with any of the executable
extensions.

Use the '--list' option to show the executable folders in the path.
");
		}
	}
}

