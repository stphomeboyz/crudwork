using System;
using System.Collections.Generic;
using System.Text;
using crudwork.Utilities;
using System.IO;

namespace ChecksumTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("{0:X}", MathUtil.ComputeChecksum(@"C:\twork\CopyMT\Source\foo1.tst"));


			//string[] files = Directory.GetFiles(@"C:\Sources\Visual Studio 2005", "*.*", SearchOption.AllDirectories);

			//for (int i = 0; i < files.Length; i++)
			//{
			//    string file = files[i];
			//    Console.WriteLine("checksum=[{0}] file=[{1}]", MathUtil.ComputeChecksum(file), file);
			//}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}
	}
}
