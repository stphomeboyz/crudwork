using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.Network;

namespace EmailTest
{
	class Program
	{
		static void Main(string[] args)
		{
			EmailManager em = new EmailManager("la1ex1.mscorp.com");
			em.Send("nobody@marshallswift.com", "spham@marshallswift.com", "Testing", "Testing");
			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}
	}
}
