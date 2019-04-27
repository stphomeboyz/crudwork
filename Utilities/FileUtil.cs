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
using System.IO;
using System.Text;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

namespace crudwork.Utilities
{
	/// <summary>
	/// File Utility
	/// </summary>
	public static class FileUtil
	{
		#region Enums
		/// <summary>
		/// File order type for sorting list of filenames
		/// </summary>
		public enum FileOrderType
		{
			/// <summary>
			/// No sort
			/// </summary>
			None = 0,

			/// <summary>
			/// Sort by filename
			/// </summary>
			Filename = 1,

			/// <summary>
			/// Sort by extension
			/// </summary>
			Extension = 2,

			/// <summary>
			/// Sort by the size
			/// </summary>
			Size = 3,

			/// <summary>
			/// Sort by last modified timestamp
			/// </summary>
			LastWriteTime = 4,

			/// <summary>
			/// Sort by file creation timestamp
			/// </summary>
			CreationDate = 5,
		}
		#endregion

		#region Fields
		private static TempFileCollection tempFiles = new TempFileCollection();
		private static int unique;
		#endregion

		#region ReadFile methods
		///// <summary>
		///// Read content of file and store into a string array.
		///// </summary>
		///// <param name="filename"></param>
		///// <returns></returns>
		//public static string[] ReadFile(string filename)
		//{
		//    try
		//    {
		//        List<String> results = new List<string>();

		//        using (StreamReader r = new StreamReader(filename))
		//        {
		//            while (!r.EndOfStream)
		//            {
		//                results.Add(r.ReadLine());
		//            }

		//            r.Close();
		//        }

		//        return results.ToArray();
		//    }
		//    catch (Exception ex)
		//    {
		//        DebuggerTool.AddData(ex, "filename", filename);
		//        throw;
		//    }
		//}

		/// <summary>
		/// Read the given filename and yield return a string
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static IEnumerable<string> ReadFile(string filename)
		{
			using (StreamReader r = new StreamReader(filename))
			{
				while (!r.EndOfStream)
				{
					string line = r.ReadLine();
					yield return line;
				}

				r.Close();
			}
			yield break;
		}

		///// <summary>
		///// Read the given filename and return a byte array
		///// </summary>
		///// <param name="filename"></param>
		///// <param name="bufSize"></param>
		///// <returns></returns>
		//public static byte[] ReadFile(string filename, int bufSize)
		//{
		//    StringBuilder s = new StringBuilder();
		//    List<byte> results = null;

		//    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, bufSize))
		//    using (BinaryReader r = new BinaryReader(fs))
		//    {
		//        results = new List<byte>((int)fs.Length);

		//        byte[] readChar = null;
		//        do
		//        {
		//            readChar = r.ReadBytes(bufSize);
		//            results.AddRange(readChar);
		//        } while ((readChar != null) && (readChar.Length > 0));

		//        r.Close();
		//        fs.Close();
		//    }

		//    return results.ToArray();
		//}

		/// <summary>
		/// Read the filename and yield return a byte array
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="bufSize"></param>
		/// <returns></returns>
		public static IEnumerable<byte[]> ReadFile(string filename, int bufSize)
		{
			return ReadFile(filename, bufSize, 0);
		}

		/// <summary>
		/// Read the filename, start a the specified position, and yield return a byte array
		/// </summary>
		/// <param name="filename">type input file</param>
		/// <param name="bufSize">this bufSize will be multiple by 10</param>
		/// <param name="startAtPosition">set the starting position</param>
		/// <returns></returns>
		public static IEnumerable<byte[]> ReadFile(string filename, int bufSize, int startAtPosition)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, bufSize * 10))
			using (BinaryReader r = new BinaryReader(fs))
			{
				fs.Position = startAtPosition;
				byte[] readChar = null;
				do
				{
					readChar = r.ReadBytes(bufSize);
					if (readChar != null)
						yield return readChar;
				} while ((readChar != null) && (readChar.Length > 0));

				r.Close();
				fs.Close();
			}

			yield break;
		}
		#endregion

		#region WriteFile methods
		/// <summary>
		/// Write a byte array to filename
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		public static void WriteFile(string filename, byte[] content)
		{
			int bufSize = 1024;
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read, bufSize))
				using (BinaryWriter w = new BinaryWriter(fs))
				{
					for (int i = 0; i < content.Length; i++)
					{
						w.Write(content[i]);
					}

					w.Flush();
					w.Close();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "filename", filename);
				DebuggerTool.AddData(ex, "content", content);
				throw;
			}
		}

		/// <summary>
		/// Write a string array to a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		public static void WriteFile(string filename, string[] content)
		{
			try
			{
				using (StreamWriter w = new StreamWriter(filename, false))
				{
					for (int i = 0; i < content.Length; i++)
					{
						w.WriteLine(content[i]);
					}

					w.Flush();
					w.Close();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "filename", filename);
				DebuggerTool.AddData(ex, "content", content);
				throw;
			}
		}

		/// <summary>
		/// Write a string to a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		public static void WriteFile(string filename, string content)
		{
			WriteFile(filename, new string[] { content });
		}
		#endregion

		#region AppendFile methods
		/// <summary>
		/// Append a string value to a filename
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="entry"></param>
		public static void AppendFile(string filename, string entry)
		{
			using (StreamWriter w = new StreamWriter(filename, true))
			{
				w.WriteLine(entry);
				w.Flush();
				w.Close();
			}
		}
		#endregion

		#region CreateTempFile
		/// <summary>
		/// Generate a unique temp filename with the extension specified.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static string GenerateUniqueTempFilename(string extension)
		{
			string fstem = Path.GetRandomFileName();
			return String.Format("{0}@AutoGen_{1}_{2}.{3}", Path.GetTempPath(), fstem, ++unique, extension);
		}

		/// <summary>
		/// Write a string List array to a unique temporarily filename with the specify extension.
		/// </summary>
		/// <param name="extension"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string CreateTempFile(string extension, params string[] list)
		{
			try
			{
				string filename = GenerateUniqueTempFilename(extension);
				tempFiles.AddFile(filename, false);

				WriteFile(filename, list);

				return filename;
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "extension", extension);
				DebuggerTool.AddData(ex, "list", DebuggerTool.Dump(list));
				throw;
			}
		}
		#endregion

		#region CleanNull
		/// <summary>
		/// Copy a file to a different filename, with cleaning null characters.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="outfile"></param>
		public static void CleanNull(string filename, string outfile)
		{
			try
			{
				if (File.Exists(outfile))
					File.Delete(outfile);

				using (StreamReader sr = new StreamReader(filename))
				using (BinaryReader br = new BinaryReader(sr.BaseStream))
				using (StreamWriter sw = new StreamWriter(outfile))
				using (BinaryWriter bw = new BinaryWriter(sw.BaseStream))
				{
					while (br.PeekChar() != -1)
					{
						byte b = br.ReadByte();

						// skip NULL character
						if (b == 0)
							continue;

						bw.Write(b);
					}

					sw.Flush();
					sw.Close();

					sr.Close();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "filename", filename);
				DebuggerTool.AddData(ex, "outfile", outfile);
				throw;
			}
		}
		#endregion

		#region SplitExtension method
		/// <summary>
		/// Split a string of file extensions (separated by comma or semicolons) and
		/// return an array.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		private static string[] SplitExtension(string extension)
		{
			return StringUtil.SplitDelimiter(extension, ',', ';');
		}
		#endregion

		#region FilterExtension and FilterDirectory methods and helpers (Obsolete)
		///// <summary>
		///// return true if the filename has one of the extension in the extensionList.
		///// </summary>
		///// <param name="ext"></param>
		///// <param name="extensions"></param>
		///// <returns></returns>
		//private static bool ContainsExtension(string filename, string[] extensionList)
		//{
		//    //return Array.BinarySearch(extensionList, filename) >= 0;

		//    for (int i = 0; i < extensionList.Length; i++)
		//    {
		//        string pattern = String.Format(@"\.{0}$", extensionList[i]);

		//        if (Regex.IsMatch(filename, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
		//            return true;
		//    }

		//    return false;
		//}

		//private static bool ContainsPath(string filename, string path)
		//{
		//    string path2 = String.Format(@"\{0}\", path.ToUpper());
		//    return filename.ToUpper().Contains(path2);
		//}

		///// <summary>
		///// Filter out the fileList for filenames having one of the extensions.
		///// Multiple extensions may be delimited by commas or semicolons.
		///// </summary>
		///// <param name="fileGroup"></param>
		///// <param name="extensions"></param>
		///// <returns></returns>
		//public static string[] FilterExtensions(string[] fileList, string extensions)
		//{
		//    return FilterExtensions(fileList, SplitExtension(extensions));
		//}

		///// <summary>
		///// Filter out the fileList for filenames having one of the extensionList.
		///// </summary>
		///// <param name="fileList"></param>
		///// <param name="extensionList"></param>
		///// <returns></returns>
		//public static string[] FilterExtensions(string[] fileList, string[] extensionList)
		//{
		//    List<String> results = new List<String>();

		//    for (int i = 0; i < fileList.Length; i++)
		//    {
		//        if (ContainsExtension(fileList[i], extensionList))
		//        {
		//            results.Add(fileList[i]);
		//        }
		//    }

		//    return results.ToArray();
		//}

		///// <summary>
		///// Filter out the fileList for filenames in the folder specified and having one of the extensions.
		///// </summary>
		///// <param name="vssAllFiles"></param>
		///// <param name="extensions"></param>
		///// <param name="folder"></param>
		///// <returns></returns>
		//public static string[] FilterExtensions(string[] vssAllFiles, string extensions, string folder)
		//{
		//    return FilterExtensions(vssAllFiles, SplitExtension(extensions), folder);
		//}

		///// <summary>
		///// Filter out the fileList for filenames in the folder specified and having one of the extensionList.
		///// </summary>
		///// <param name="fileList"></param>
		///// <param name="extensionList"></param>
		///// <param name="folder"></param>
		///// <returns></returns>
		//public static string[] FilterExtensions(string[] fileList, string[] extensionList, string folder)
		//{
		//    List<String> results = new List<string>();

		//    // filter out path first, then filter out extensionList.
		//    for (int i = 0; i < fileList.Length; i++)
		//    {
		//        if (ContainsPath(fileList[i], folder))
		//        {
		//            results.Add(fileList[i]);
		//        }
		//    }

		//    // if result is empty, return an empty list.
		//    if (results.Count == 0)
		//        return results.ToArray();

		//    return FilterExtensions(results.ToArray(), extensionList);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="vssAllFiles"></param>
		///// <param name="scriptParentFolder"></param>
		///// <returns></returns>
		//public static string[] FilterDirectory(string[] vssAllFiles, string scriptParentFolder)
		//{
		//    throw new Exception("The method or operation is not implemented.");
		//}
		#endregion

		#region GetFiles / GetFolders methods
		/// <summary>
		/// Shorten the folder name
		/// </summary>
		/// <param name="fileList"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string[] MakeRelativePath(string[] fileList, string path)
		{
			List<String> results = new List<string>();

			for (int i = 0; i < fileList.Length; i++)
			{
				string file = fileList[i];
				results.Add(file.Replace(path + "\\", ""));
			}

			return results.ToArray();
		}

		/// <summary>
		/// Return a list of filenames
		/// </summary>
		/// <param name="path"></param>
		/// <param name="patterns"></param>
		/// <param name="searchOption"></param>
		/// <param name="relativePath"></param>
		/// <param name="fileOrderType"></param>
		/// <returns></returns>
		public static string[] GetFiles(string path, string patterns, SearchOption searchOption, bool relativePath, FileOrderType fileOrderType)
		{
			List<String> results = new List<string>();
			string[] patternList = SplitExtension(patterns);

			// handle multiple patterns, such as "*.AAA,*.BBB,*.CCC"
			for (int j = 0; j < patternList.Length; j++)
			{
				results.AddRange(Directory.GetFiles(path, patternList[j], searchOption));
			}

			// apply order by ...
			string[] orderList = OrderFileBy(results.ToArray(), fileOrderType);

			// apply relative path ...
			if (!relativePath)
			{
				return orderList;
			}
			else
			{
				return MakeRelativePath(orderList, path);
			}
		}

		/// <summary>
		/// Return a list of folder names
		/// </summary>
		/// <param name="path"></param>
		/// <param name="pattern"></param>
		/// <param name="searchOption"></param>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public static string[] GetFolders(string path, string pattern, SearchOption searchOption, bool relativePath)
		{
			List<String> results = new List<string>();
			string[] patternList = SplitExtension(pattern);

			// handle multiple patterns, such as "*.AAA,*.BBB,*.CCC"
			for (int j = 0; j < patternList.Length; j++)
			{
				results.AddRange(Directory.GetDirectories(path, patternList[j], searchOption));
			}

			if (!relativePath)
			{
				return results.ToArray();
			}
			else
			{
				return MakeRelativePath(results.ToArray(), path);
			}
		}

		/// <summary>
		/// sort the file list based on the given order type
		/// </summary>
		/// <param name="fileList"></param>
		/// <param name="fileOrderType"></param>
		/// <returns></returns>
		public static string[] OrderFileBy(string[] fileList, FileOrderType fileOrderType)
		{
			string[] orderKey = new string[fileList.Length];
			string[] orderVal = new string[fileList.Length];

			//int maskLength = StringUtil.MaxLength(fileList);
			int maskLength = 100;
			string maskFormat = String.Format(@"{{0,{0}}}", maskLength);

			for (int i = 0; i < fileList.Length; i++)
			{
				string filename = fileList[i];
				string orderByKey;

				if (!File.Exists(filename))
					throw new FileNotFoundException(filename);

				FileInfo fi = new FileInfo(filename);

				switch (fileOrderType)
				{
					case FileOrderType.None:
						orderByKey = "";
						break;

					case FileOrderType.Filename:
						{
							orderByKey = String.Format(maskFormat, fi.Name);
						}
						break;

					case FileOrderType.LastWriteTime:
						{
							DateTime dt = fi.LastWriteTime;
							orderByKey = String.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}",
								dt.Year,
								dt.Month,
								dt.Day,
								dt.Hour,
								dt.Minute,
								dt.Second,
								dt.Millisecond
								);
						}
						break;

					default:
						throw new ArgumentOutOfRangeException("not supported: " + fileOrderType);
				}

				orderKey[i] = orderByKey;
				orderVal[i] = fileList[i];
			}

			if (fileOrderType != FileOrderType.None)
			{
				Array.Sort(orderKey, orderVal);
			}

			return orderVal;
		}
		#endregion

		#region GetFileInfo
		/// <summary>
		/// Return a FileInfo array
		/// </summary>
		/// <param name="path"></param>
		/// <param name="patterns"></param>
		/// <param name="searchOption"></param>
		/// <returns></returns>
		public static Dictionary<String, FileInfo> GetFileInfo(string path, string patterns, SearchOption searchOption)
		{
			List<String> fileList = new List<string>();
			string[] patternList = SplitExtension(patterns);

			// handle multiple patterns, such as "*.AAA,*.BBB,*.CCC"
			for (int i = 0; i < patternList.Length; i++)
			{
				fileList.AddRange(Directory.GetFiles(path, patternList[i], searchOption));
			}

			Dictionary<String, FileInfo> results = new Dictionary<string, FileInfo>();
			for (int i = 0; i < fileList.Count; i++)
			{
				FileInfo fi = new FileInfo(fileList[i]);
				results.Add(fileList[i], fi);
			}

			return results;
		}
		#endregion

		#region HasAttribute
		/// <summary>
		/// return true if the filename has the given attribute set
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="attr"></param>
		/// <returns></returns>
		public static bool HasAttribute(string filename, FileAttributes attr)
		{
			return (File.GetAttributes(filename) & attr) == attr;
		}
		#endregion

		#region Stream methods
		/// <summary>
		/// Read from stream
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string[] ReadStream(Stream s)
		{
			List<string> results = new List<string>();
			using (StreamReader r = new StreamReader(s))
			{
				while (!r.EndOfStream)
				{
					results.Add(r.ReadLine());
				}
				r.Close();
			}
			return results.ToArray();
		}

		/// <summary>
		/// Write to stream
		/// </summary>
		/// <param name="s"></param>
		/// <param name="value"></param>
		public static void WriteStream(Stream s, string value)
		{
			byte[] byteArray = ASCIIEncoding.UTF8.GetBytes(value);
			s.Write(byteArray, 0, byteArray.Length);
			s.Flush();
			s.Close();
		}
		#endregion

		/// <summary>
		/// Append a suffix (such as a date) to the name of the file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static string AppendSuffixToFilename(string filename, string suffix)
		{
			FileInfo fi = new FileInfo(filename);
			return string.Format(@"{0}{1}{2}{3}",
				fi.DirectoryName,
				Path.GetFileNameWithoutExtension(fi.FullName),
				suffix,
				fi.Extension
				);
		}
	}
}
