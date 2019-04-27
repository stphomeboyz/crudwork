using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using crudwork.Utilities;

namespace ControlTests
{
	public partial class Form1 : Form
	{
		private enum ControlTypes
		{
			TextBox,
			ComboBox,
		}

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ControlUtil.PopulateControl(cboControlType, typeof(ControlTypes), "");

			flpMain.Controls.Clear();

			for (int i = 0; i < 3; i++)
			{
				btnAdd_Click(sender, e);
			}
		}

		private void AddControl(ControlTypes type)
		{
			UserControl ucx = null;

			switch (type)
			{
				case ControlTypes.ComboBox:
					{
						var uc = new crudwork.Controls.FormControls.ComboBoxForm();
						ucx = uc;

						uc.ShowEllipseButton = true;
						uc.Width = flpMain.ClientRectangle.Width;
						uc.Label = "My ComboBox";
						uc.Value = "abcdefg";
						uc.ButtonClicked += (object s2, EventArgs e2) => MessageBox.Show("Test");
					}
					break;
				case ControlTypes.TextBox:
					{
						var uc = new crudwork.Controls.FormControls.TextBoxForm();
						ucx = uc;

						uc.ShowEllipseButton = true;
						uc.Width = flpMain.ClientRectangle.Width;
						uc.Label = "My TextBox";
						uc.Value = "abcdefg";
						uc.ButtonClicked += (object s2, EventArgs e2) => MessageBox.Show("Test");
					}
					break;
			}

			flpMain.Controls.Add(ucx);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddControl((ControlTypes)cboControlType.SelectedItem);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			flpMain.Controls.Clear();
		}

		private void flpMain_Resize(object sender, EventArgs e)
		{
			foreach (Control item in flpMain.Controls)
			{
				item.Width = flpMain.ClientRectangle.Width;
			}
		}
	}
}
