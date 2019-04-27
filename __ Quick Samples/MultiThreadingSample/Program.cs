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
using System.Threading;

using crudwork.MultiThreading;

namespace MultiThreadingSample
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcessMultiThread();
		}

		static void ProcessSingleThread()
		{
			// initialize the input
			List<int> mainInput = GenerateInput(25000);
			List<decimal> mainOutput = new List<decimal>();

			for (int i = 0; i < mainInput.Count; i++)
			{
				// your business logic goes here...
				int value = mainInput[i] + 1;
				mainOutput.Add(value);
			}
		}

		static void ProcessMultiThread()
		{
			/*
			 * PURPOSE: start a process in multi-threads.
			 * */

			// initialize the input
			List<int> mainInput = GenerateInput(25000);
			List<decimal> mainOutput = null;

			#region start process in Multi-thread mode
			// initialize the manager
			MultiThreadingManager<int, decimal> mtManager = new MultiThreadingManager<int, decimal>();

			// initialize the number of threads
			mtManager.InitializeThread(16, typeof(MTContainer));

			// set the input list
			// IMPORTANT: must pass in a List<T> object
			mtManager.MainInputList = mainInput;

			// distribute the input using the RoundRobin or Evenly distribution.
			mtManager.DistributeInput(DistributionTypes.Evenly);

			// start all threads...
			mtManager.StartAsync(true, null, false);

			// get the output list
			mainOutput = mtManager.MainOutputList;
			#endregion


			// do whatever is necessarily...
			for (int i = 0; i < mainInput.Count; i++)
			{
				int a = mainInput[i];
				decimal b = mainOutput[i];

				// verify output...
				if (a == b + 1)
					throw new ApplicationException("not expected");
				//Console.WriteLine("#{0}: {1} --> {2}", i, mainInput[i], mainOutput[i]);
			}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		static void ProcessMultiThreadNew()
		{
			/*
			 * PURPOSE: start a process in multi-threads.
			 * */

			// initialize the input
			List<int> mainInput = GenerateInput(25000);
			List<decimal> mainOutput = null;

			mainOutput = MT.Start<int, decimal>(mainInput, 16, typeof(MTContainer), null);

			// do whatever is necessarily...
			for (int i = 0; i < mainInput.Count; i++)
			{
				int a = mainInput[i];
				decimal b = mainOutput[i];

				// verify output...
				if (a == b + 1)
					throw new ApplicationException("not expected");
				//Console.WriteLine("#{0}: {1} --> {2}", i, mainInput[i], mainOutput[i]);
			}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static List<int> GenerateObservableInput(int maxItems)
		{
			List<int> results = new List<int>();
			for (int i = 0; i < maxItems; i++)
			{
				results.Add(i);
			}
			return results;
		}

		private static List<int> GenerateInput(int maxItems)
		{
			List<int> results = new List<int>();
			Random r = new Random();
			for (int i = 0; i < maxItems; i++)
			{
				results.Add(r.Next());
			}
			return results;
		}
	}

	/// <summary>
	/// The class containing the codes to run under multi-threads.
	/// </summary>
	class MTContainer : MultiThreadingBase<int, decimal>
	{
		public MTContainer()
			: base()
		{
		}

		protected override void ProcessInput(MultiThreadEventArgs<int, decimal> e)
		{
			/* Your business logic goes here...
			 * 
			 * Use e.Input to retrieve the input
			 * e.Result to store the output
			 * e.Index to get the relative index (use AbsoluteIndex(e.Index) to get the absolute index)
			 * 
			 * */
			//Thread.Sleep(1000);
			e.Result = e.Input + 1;
		}
	}
}
