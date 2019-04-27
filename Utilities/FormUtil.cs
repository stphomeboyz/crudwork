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
using System.Text;
//#if !SILVERLIGHT
using System.Windows.Forms;
//#else
//using System.Windows.Controls;
//using System.Windows.Input;
//#endif
using System.Collections;

namespace crudwork.Utilities
{
	/// <summary>
	/// Windows Forms Utility
	/// </summary>
	public static class FormUtil
	{
		/// <summary>
		/// custom ShowDialog
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public static DialogResult OpenDialogBox(Form form)
		{
			form.StartPosition = FormStartPosition.CenterParent;
			return form.ShowDialog();
		}

		/// <summary>
		/// Display an Exception in a dialog box
		/// </summary>
		/// <param name="ex"></param>
		public static void WinException(Exception ex)
		{
			WinException(ex, string.Empty);
		}

		/// <summary>
		/// Display an Exception in a dialog box
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="title"></param>
		public static void WinException(Exception ex, string title)
		{
			#region Sanity Checks
			if (ex == null)
				return;
			#endregion

			StringBuilder details = new StringBuilder();
			StringBuilder messages = new StringBuilder();
			Exception exfoo = ex;

			messages.AppendFormat("{1}{0}", Environment.NewLine, exfoo.Message);

			// show all error messages.
			while (exfoo != null)
			{
				details.AppendFormat("Message: {1}{0}", Environment.NewLine, exfoo.Message);
				details.AppendFormat("Detail : {0}{1}{0}", Environment.NewLine, exfoo.ToString());
				details.AppendFormat("Data   : {0}{1}{0}", Environment.NewLine, GetData(exfoo.Data));
				exfoo = exfoo.InnerException;
			}

			//MessageBox.Show(s.ToString());
			using (Dialogs.WinErrorDialog f = new Dialogs.WinErrorDialog())
			{
				string s = String.Format("----- Brief -----{0}{1}{0}{0}----- Details -----{0}{2}",
					Environment.NewLine,
					messages.ToString(),
					details.ToString()
					);

				if (!string.IsNullOrEmpty(title))
					f.WindowTitle = "Exception - " + title;
				f.Message = messages.ToString();
				f.Details = s;
				OpenDialogBox(f);
			}
		}

		private static string GetData(IDictionary iDictionary)
		{
			StringBuilder s = new StringBuilder();
			IDictionaryEnumerator ide = iDictionary.GetEnumerator();

			while (ide.MoveNext())
			{
				s.AppendFormat("{0} = {1}\r\n", ide.Key, ide.Value);
			}

			return s.ToString();
		}

		/// <summary>
		/// Set the busy state of the control to true or false.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="busyFlag"></param>
		public static void Busy(Control control, bool busyFlag)
		{
			if (control == null)
				return;

			control.Cursor = (busyFlag) ? Cursors.WaitCursor : Cursors.Default;

			if (busyFlag)
			{
				control.MouseDown += new MouseEventHandler(BusyControl_MouseDown);
				//control.Click += new EventHandler(BusyControl_Click);
				control.KeyDown += new KeyEventHandler(BusyControl_KeyDown);
			}
			else
			{
				control.MouseDown -= new MouseEventHandler(BusyControl_MouseDown);
				control.Click -= new EventHandler(BusyControl_Click);
				//control.KeyDown -= new KeyEventHandler(BusyControl_KeyDown);
			}
			Application.DoEvents();
		}

		/// <summary>
		/// Set the busy state of the main control and enable or disable a list of child controls.
		/// </summary>
		/// <param name="mainControl"></param>
		/// <param name="childControls"></param>
		/// <param name="busyFlag"></param>
		public static void Busy(Control mainControl, Control[] childControls, bool busyFlag)
		{
			Busy(mainControl, busyFlag);

			if (childControls == null || childControls.Length == 0)
				return;

			for (int i = 0; i < childControls.Length; i++)
			{
				childControls[i].Enabled = !busyFlag;
			}
		}

		internal static void BusyControl_MouseDown(object sender, MouseEventArgs e)
		{
			//e.
		}
		internal static void BusyControl_KeyDown(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
		}
		internal static void BusyControl_Click(object sender, EventArgs e)
		{
			// need to suppress the mouse click ...
		}
	}
}
