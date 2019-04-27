using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Utilities;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace crudwork.Controls.DataTools
{
	/// <summary>
	/// Object viewer / editor
	/// </summary>
	public partial class ObjectViewer : UserControl
	{
		#region Private Fields
		/// <summary>
		/// A value from the property grid has changed.  This indicator means the XML/textbox needs to be refreshed.
		/// </summary>
		private bool refreshXMLTextBox = false;
		private bool refreshXMLViewer = false;
		/// <summary>
		/// the XML/textbox has changed.  This indicator means the property grid needs to be refreshed.
		/// </summary>
		private bool refreshPropertyGrid = false;

		private ViewMode _viewMode;
		#endregion

		#region Event Handlers
		/// <summary>
		/// The viewer raises this event to allow custom serialization
		/// </summary>
		public event EventHandler<SerializationEventArgs> Serialize = null;
		/// <summary>
		/// The viewer raises this event to allow custom deserialization.
		/// </summary>
		public event EventHandler<SerializationEventArgs> Deserialize = null;
		/// <summary>
		/// Subscribe to this event to handle custom editor
		/// </summary>
		public event EventHandler EditXML = null;
		#endregion

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public ObjectViewer()
		{
			InitializeComponent();
			ViewMode = ViewMode.Split;
		}

		#region Properties
		/// <summary>
		/// Get or set the caption label
		/// </summary>
		public string Caption
		{
			get
			{
				return lblCaption.Text;
			}
			set
			{
				lblCaption.Text = value;
			}
		}
		/// <summary>
		/// Get or set the object to be viewed
		/// </summary>
		public object PropertyObject
		{
			get
			{
				return propertyGrid1.SelectedObject;
			}
			set
			{
				propertyGrid1.SelectedObject = value;
				if (value != null && propertyGrid1.Enabled)
				{
					refreshXMLTextBox = true;
					UpdateControls();
				}
			}
		}
		/// <summary>
		/// Show or hide the textbox control
		/// </summary>
		public bool TextBoxVisible
		{
			get
			{
				return richTextBox1.Visible;
			}
			set
			{
				richTextBox1.Visible = value;
				//richTextBox1.Enabled = value;
			}
		}
		/// <summary>
		/// Get or set the XML string
		/// </summary>
		public override string Text
		{
			get
			{
				return richTextBox1.Text;
			}
			set
			{
				richTextBox1.Text = DataUtil.ReformatXML(value);
				if (richTextBox1.Enabled)
				{
					refreshPropertyGrid = refreshXMLTextBox = false;

					// update the the XML Viewer first
					refreshPropertyGrid = true;
					UpdatePropertyGrid();

					// then once the property is up-to-date, update the XML viewer
					refreshXMLTextBox = true;
					UpdateTextBox();

					UpdateXMLViewer();
				}
			}
		}
		/// <summary>
		/// Get or set the view mode
		/// </summary>
		public ViewMode ViewMode
		{
			get
			{
				return _viewMode;
			}
			set
			{
				_viewMode = value;

				splitContainer1.Visible = value == ViewMode.Split;
				tabControl1.Visible = value == ViewMode.Tab;

				showSplitViewToolStripMenuItem.Checked = value == ViewMode.Split;
				showTabViewToolStripMenuItem.Checked = value == ViewMode.Tab;

				switch (value)
				{
					case ViewMode.Split:
						splitContainer1.Panel1.Controls.Add(webBrowser1);
						splitContainer1.Panel2.Controls.Add(propertyGrid1);
						break;
					case ViewMode.Tab:
						tabControl1.TabPages[0].Controls.Add(webBrowser1);
						tabControl1.TabPages[1].Controls.Add(propertyGrid1);
						break;
					default:
						break;
				}

				Debug.WriteLine(string.Format("Split={0} Tab={1}",
					splitContainer1.Visible, tabControl1.Visible));
			}
		}
		#endregion

		#region Helpers
		private void UpdateXMLViewer()
		{
			if (!refreshXMLViewer)
				return;

			try
			{
				var xml = DataUtil.ReformatXML(Text);

				if (string.IsNullOrEmpty(xml))
				{
					webBrowser1.Url = new Uri("about:blank");
				}
				else
				{
					var xmlFile = FileUtil.CreateTempFile("xml", xml);
					webBrowser1.Url = new Uri(xmlFile);
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				refreshXMLViewer = false;
			}
		}
		private void UpdateTextBox()
		{
			// this updates the XML/textbox if, and only if, the property has changed.
			if (!refreshXMLTextBox)
				return;

			try
			{
				var xml = SerializeInternal(PropertyObject);

				// use this approach to bypass the TextChanged event
				richTextBox1.Enabled = false;
				richTextBox1.Text = DataUtil.ReformatXML(xml);
				richTextBox1.Enabled = true;

				refreshXMLViewer = true;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				refreshXMLTextBox = false;
			}
		}
		private void UpdatePropertyGrid()
		{
			// this updates the property grid if, and only if, the xml has changed.
			if (!refreshPropertyGrid)
				return;

			try
			{
				var obj = DeserializeInternal(PropertyObject == null ? null : PropertyObject.GetType(), Text);

				propertyGrid1.Enabled = false;
				PropertyObject = obj;
				propertyGrid1.Enabled = true;

				propertyGrid1.Update();
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				refreshPropertyGrid = false;
			}
		}
		private void UpdateControls()
		{
			UpdateTextBox();
			UpdatePropertyGrid();
			UpdateXMLViewer();
		}
		#endregion

		private string SerializeInternal(object obj)
		{
			if (Serialize != null)
			{
				var e = new SerializationEventArgs(PropertyObject, null);
				Serialize(this, e);
				return e.XmlString;
			}

			StringBuilder s = new StringBuilder();
			using (StringWriter sw = new StringWriter(s))
			{
				XmlSerializer xs = new XmlSerializer(obj.GetType());
				xs.Serialize(sw, obj);
				sw.Close();
			}
			return s.ToString();
		}
		private object DeserializeInternal(Type t, string value)
		{
			if (Deserialize != null)
			{
				var e = new SerializationEventArgs(null, Text);
				Deserialize(this, e);
				return e.PropertyObject;
			}

			using (StringReader sr = new StringReader(value))
			{
				XmlSerializer xs = new XmlSerializer(t);
				return xs.Deserialize(sr);
			}
		}

		#region Application Events
		private void ObjectViewer_Load(object sender, EventArgs e)
		{
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			if (!richTextBox1.Enabled)
				return;
			// the user modifies the xml.  Don't update right away, pending more changes...
			refreshPropertyGrid = true;
			refreshXMLViewer = true;
		}
		private void richTextBox1_MouseHover(object sender, EventArgs e)
		{
			richTextBox1.Focus();
			richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
			richTextBox1.Height = 200;
			// update any pending changes (JUST IN CASE...)
			UpdateControls();
		}
		private void richTextBox1_MouseLeave(object sender, EventArgs e)
		{
			richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
			richTextBox1.Height = 20;
			// update any pending changes...
			UpdateControls();

			//webBrowser1.Focus();
		}
		private void richTextBox1_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (EditXML != null)
				{
					EditXML(sender, e);
					return;
				}

				var s = Text;
				if (ControlManager.ShowTextDialog(ref s, Caption + " XML Input") == DialogResult.OK)
					Text = s;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			// the user made changes to a property.  Update the textbox right away.
			refreshXMLTextBox = true;
			UpdateControls();
		}
		private void propertyGrid1_Leave(object sender, EventArgs e)
		{
			// the object _may_ be changed via a sub-component dialog box, and I don't see any event to raise;
			// therefore, we force an update here to make sure the textbox is up-to-date.
			refreshXMLTextBox = true;
			UpdateControls();
		}

		private void showSplitViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			showTabViewToolStripMenuItem.Checked = false;
			ViewMode = ViewMode.Split;
		}
		private void showTabViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			showSplitViewToolStripMenuItem.Checked = false;
			ViewMode = ViewMode.Tab;
		}
		#endregion
	}

	/// <summary>
	/// Display mode
	/// </summary>
	public enum ViewMode
	{
		/// <summary>
		/// Use split control
		/// </summary>
		Split,
		/// <summary>
		/// Use tab control
		/// </summary>
		Tab,
	}

	/// <summary>
	/// Serialize/Deserialize event argument
	/// </summary>
	public class SerializationEventArgs : EventArgs
	{
		/// <summary>
		/// The object
		/// </summary>
		public object PropertyObject
		{
			get;
			set;
		}
		/// <summary>
		/// The XML string
		/// </summary>
		public string XmlString
		{
			get;
			set;
		}

		/// <summary>
		/// create new instance with given attribute
		/// </summary>
		/// <param name="theObject"></param>
		/// <param name="xmlString"></param>
		public SerializationEventArgs(object theObject, string xmlString)
		{
			this.PropertyObject = theObject;
			this.XmlString = xmlString;
		}
	}
}
