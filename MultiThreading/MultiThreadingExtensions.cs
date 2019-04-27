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

#if !SILVERLIGHT
using System.Windows.Forms;
#endif

namespace crudwork.MultiThreading
{
	/// <summary>
	/// Extensions to MultiThreading class
	/// </summary>
	public static class MultiThreadingExtensions
	{
	}

	/// <summary>
	/// Multi-threading shortcut.
	/// </summary>
	public static class MT
	{
		internal class ActionContainer<TInput, TOutput> : MultiThreadingBase<TInput, TOutput>
		{
			internal Action<MultiThreadEventArgs<TInput, TOutput>> callback = null;

			internal ActionContainer(Action<MultiThreadEventArgs<TInput, TOutput>> callback)
			{
				this.callback = callback;
			}

			protected override void ProcessInput(MultiThreadEventArgs<TInput, TOutput> e)
			{
				callback(e);
			}
		}

		private static MultiThreadingManager<TInput, TOutput> CreateManager<TInput, TOutput>(int numThreads, List<TInput> input,
			Type container, object[] args)
		{
			// initialize the manager
			MultiThreadingManager<TInput, TOutput> mtManager = new MultiThreadingManager<TInput, TOutput>();

			// initialize the number of threads
			mtManager.InitializeThread(numThreads, container, args);

			// set the input list
			// IMPORTANT: must pass in a List<T> object
			mtManager.MainInputList = input;

			// distribute the input using the RoundRobin or Evenly distribution.
			mtManager.DistributeInput(DistributionTypes.Evenly);

			return mtManager;
		}

		/// <summary>
		/// Setup multi-threading mode using the specified container
		/// </summary>
		/// <typeparam name="TInput"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="input"></param>
		/// <param name="numThreads"></param>
		/// <param name="container"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static List<TOutput> Start<TInput, TOutput>(List<TInput> input, int numThreads,
			Type container, object[] args)
		{
			var mtManager = CreateManager<TInput, TOutput>(numThreads, input, container, args);

			// start all threads...
			mtManager.StartAsync();

			// get the output list
			return mtManager.MainOutputList;
		}

		/// <summary>
		/// Setup multi-threading mode using the action callback
		/// </summary>
		/// <typeparam name="TInput"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="input"></param>
		/// <param name="numThreads"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static List<TOutput> Start<TInput, TOutput>(List<TInput> input, int numThreads,
			Action<MultiThreadEventArgs<TInput, TOutput>> callback)
		{
			Type container = typeof(ActionContainer<TInput, TOutput>);
			var mtManager = CreateManager<TInput, TOutput>(numThreads, input, container, new object[] { callback });

			// start all threads...
			mtManager.StartAsync();

			// get the output list
			return mtManager.MainOutputList;
		}

#if !SILVERLIGHT

		/// <summary>
		/// Setup multi-threading mode using the specified container
		/// </summary>
		/// <typeparam name="TInput"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="input"></param>
		/// <param name="numThreads"></param>
		/// <param name="container"></param>
		/// <param name="args"></param>
		/// <param name="owner"></param>
		/// <param name="closeOnCompletion"></param>
		/// <returns></returns>
		public static List<TOutput> StartDialog<TInput, TOutput>(List<TInput> input, int numThreads,
			Type container, object[] args,
			IWin32Window owner, bool closeOnCompletion)
		{
			var mtManager = CreateManager<TInput, TOutput>(numThreads, input, container, args);

			// start all threads...
			mtManager.StartAsync(true, owner, closeOnCompletion);

			// get the output list
			return mtManager.MainOutputList;
		}

		/// <summary>
		/// Setup multi-threading mode using the action callback
		/// </summary>
		/// <typeparam name="TInput"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="input"></param>
		/// <param name="numThreads"></param>
		/// <param name="callback"></param>
		/// <param name="owner"></param>
		/// <param name="closeOnCompletion"></param>
		/// <returns></returns>
		public static List<TOutput> StartDialog<TInput, TOutput>(List<TInput> input, int numThreads,
			Action<MultiThreadEventArgs<TInput, TOutput>> callback,
			IWin32Window owner, bool closeOnCompletion)
		{
			Type container = typeof(ActionContainer<TInput, TOutput>);
			var mtManager = CreateManager<TInput, TOutput>(numThreads, input, container, new object[] { callback });

			// start all threads...
			mtManager.StartAsync(true, owner, closeOnCompletion);

			// get the output list
			return mtManager.MainOutputList;
		}

#endif
	}
}