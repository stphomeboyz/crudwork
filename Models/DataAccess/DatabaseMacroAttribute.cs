using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crudwork.Models.DataAccess
{
	/// <summary>
	/// A Method for Database Macro
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class MacroMethodAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class MacroServiceAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public MacroServiceAttribute(string name)
		{
			this.Name = name;
		}
	}
}
