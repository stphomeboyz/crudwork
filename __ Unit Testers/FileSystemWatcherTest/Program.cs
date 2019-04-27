using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileSystemWatcherTest
{
	class Program
	{
		static void Main(string[] args)
		{
			ExamineFSW();
		}

		private static void ExamineFSW()
		{
			using (var watcher = new FileSystemWatcher(@"c:\twork\FileSystemWatcher", "*.*"))
			{
				watcher.Created += new FileSystemEventHandler(watcher_Created);
				watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
				watcher.Changed += new FileSystemEventHandler(watcher_Changed);
				watcher.Renamed += new RenamedEventHandler(watcher_Renamed);

				watcher.IncludeSubdirectories = false;
				watcher.InternalBufferSize = 16 * 1024;		// allocate biggerr buffer to store more events

				watcher.EnableRaisingEvents = true;

				Console.WriteLine("Press ENTER to continue...");
				Console.ReadLine();
			}
		}

		static void watcher_Created(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine("Created " + e.Name);
		}
		static void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine("Changed " + e.Name);
		}
		static void watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine("Deleted " + e.Name);
		}
		static void watcher_Renamed(object sender, RenamedEventArgs e)
		{
			Console.WriteLine("Renamed from {0} to {1}", e.OldName, e.Name);
		}
	}
}
