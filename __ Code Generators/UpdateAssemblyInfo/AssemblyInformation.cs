using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateAssemblyInfo
{
	[Serializable]
	public class AssemblyInformation
	{
		public string SolutionFolder
		{
			get;
			set;
		}
		public string Version
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public string Company
		{
			get;
			set;
		}
		public string Product
		{
			get;
			set;
		}
		public string Copyright
		{
			get;
			set;
		}

		public AssemblyInformation()
		{
		}
	}
}
