using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.Utilities;

namespace SchedulerTimerTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				TestSchedulerTimer();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}

		private static void TestSchedulerTimer()
		{
			/*
				Service Events - OnStart
				SchedulerTimer: Interval=[0d 1h 0m 0s] 
				start=2/6/2009 11:50:18 AM 
				 next=2/7/2009 12:00:00 AM 
				interval=3600000 
				RoundOff[h=Quad m=Zero s=Zero i=43782000]
			 * */
			SchedulerTimer t;
			DateTime dt = DateTime.Parse("2/6/2009 11:50:18 AM");

			/**/
			t = new SchedulerTimer(dt, 0, 1, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n1 Hour schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 2, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n2 Hour schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 3, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n3 Hour schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 4, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n4 Hour schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 6, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n6 Hour schedule: " + t.ToString());
			/**/

			/**/
			t = new SchedulerTimer(dt, 0, 12, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n12 Hour schedule: " + t.ToString());

			//t = new SchedulerTimer(dt, 0, 24, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			//Console.WriteLine("\n\n24 Hour schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 1, 0, 0, 0, RoundOffStyle.Zero, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\nDaily schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 0, 15, 0, RoundOffStyle.None, RoundOffStyle.Quad, RoundOffStyle.Zero);
			Console.WriteLine("\n\n15 Minute schedule: " + t.ToString());

			t = new SchedulerTimer(dt, 0, 1, 0, 0, RoundOffStyle.Quad, RoundOffStyle.Zero, RoundOffStyle.Zero);
			Console.WriteLine("\n\n15 Minute schedule: " + t.ToString());
			/**/

			t = new SchedulerTimer(dt, 0, 0, 10, 0, RoundOffStyle.None, RoundOffStyle.Quad, RoundOffStyle.Zero);
			Console.WriteLine("\n\n10 Minute schedule: " + t.ToString());
		}
	}
}


/*
1 Hour schedule: Interval=[0d 1h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T12:00:00
interval=3600000
RoundOff[h=Quad m=Zero s=Zero i=582000]


2 Hour schedule: Interval=[0d 2h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T14:00:00
interval=7200000
RoundOff[h=Quad m=Zero s=Zero i=7782000]


3 Hour schedule: Interval=[0d 3h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T14:00:00
interval=10800000
RoundOff[h=Quad m=Zero s=Zero i=7782000]


4 Hour schedule: Interval=[0d 4h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T15:00:00
interval=14400000
RoundOff[h=Quad m=Zero s=Zero i=11382000]


6 Hour schedule: Interval=[0d 6h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T18:00:00
interval=21600000
RoundOff[h=Quad m=Zero s=Zero i=22182000]


12 Hour schedule: Interval=[0d 12h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-07T00:00:00
interval=43200000
RoundOff[h=Quad m=Zero s=Zero i=43782000]


Daily schedule: Interval=[1d 0h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-07T00:00:00
interval=0
RoundOff[h=Zero m=Zero s=Zero i=43782000]


15 Minute schedule: Interval=[0d 0h 15m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T12:15:00
interval=900000
RoundOff[h=None m=Quad s=Zero i=1482000]


15 Minute schedule: Interval=[0d 1h 0m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T12:00:00
interval=3600000
RoundOff[h=Quad m=Zero s=Zero i=582000]


10 Minute schedule: Interval=[0d 0h 10m 0s]
start=2009-02-06T11:50:18
 next=2009-02-06T12:00:00
interval=600000
RoundOff[h=None m=Quad s=Zero i=582000]
Press ENTER to continue...
*/