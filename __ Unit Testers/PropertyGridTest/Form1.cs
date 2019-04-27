using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.FileImporters;

namespace PropertyGridTest
{
	public partial class Form1 : Form
	{
		//private Foobar foobar = new Foobar();

		public Form1()
		{
			InitializeComponent();
			//propertyGrid1.SelectedObject = foobar;
			propertyGrid1.SelectedObject = new FixedLengthImportOptions();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}
	}

	[DefaultProperty("Foobar")]
	public class Foobar
	{
		[Category("Application"), Description("The name")]
		//[Browsable(true), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
		public string Name
		{
			get;
			set;
		}
		[Category("Application"), Description("Member is active")]
		//[Browsable(true), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
		public bool IsActive
		{
			get;
			set;
		}
		[Category("Application"), Description("Account open date")]
		//[Browsable(true), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
		public DateTime MemberSince
		{
			get;
			set;
		}
	}
}
