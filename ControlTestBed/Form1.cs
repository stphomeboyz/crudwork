using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using crudwork.Utilities;
using crudwork.Controls.DataTools;

namespace ControlTestBed
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			//objectViewer1.Serialize += new EventHandler<SerializationEventArgs>(objectViewer1_Serialize);
			//objectViewer1.Deserialize += new EventHandler<SerializationEventArgs>(objectViewer1_Deserialize);
			}

		private void Form1_Load(object sender, EventArgs e)
		{
			var foo = new Foo()
			{
				First = "John",
				Middle = "H",
				Last = "Doe",
				Address = "123 Main St",
				City = "New land",
				State = "CA",
				Zipcode = "90001",
				CurrentAddress = true,
				LastUpdated = DateTime.Parse("8/17/2008"),
				AddressType = AddressType.Home,
			};
			objectViewer1.PropertyObject = foo;
			objectViewer1.ViewMode = crudwork.Controls.DataTools.ViewMode.Split;
		}

		void objectViewer1_Deserialize(object sender, crudwork.Controls.DataTools.SerializationEventArgs e)
		{
			e.PropertyObject = Serializer.Deserialize<Foo>(e.XmlString, Serializer.SerializeMethods.Xml);
		}
		void objectViewer1_Serialize(object sender, crudwork.Controls.DataTools.SerializationEventArgs e)
		{
			e.XmlString = Serializer.Serialize(e.PropertyObject, Serializer.SerializeMethods.Xml);
		}

		public enum AddressType
		{
			Home,
			Business,
		}

		public class Foo
		{
			[Category("Contact Info"), Description("First Name")]
			public string First
			{
				get;
				set;
			}
			[Category("Contact Info"), Description("Middle Name")]
			public string Middle
			{
				get;
				set;
			}
			[Category("Contact Info"), Description("Last Name")]
			public string Last
			{
				get;
				set;
			}

			[Category("Address Info")]
			public string Address
			{
				get;
				set;
			}
			[Category("Address Info")]
			public string City
			{
				get;
				set;
			}
			[Category("Address Info")]
			public string State
			{
				get;
				set;
			}
			[Category("Address Info")]
			public string Zipcode
			{
				get;
				set;
			}

			[Category("Other Info")]
			public AddressType AddressType
			{
				get;
				set;
			}
			[Category("Other Info"), Description("Specify whether this is the current address")]
			public bool CurrentAddress
			{
				get;
				set;
			}
			[Category("Other Info"), Description("The date/time record was last updated")]
			public DateTime LastUpdated
			{
				get;
				set;
			}
		}
	}
}
