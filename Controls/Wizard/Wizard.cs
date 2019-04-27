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
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using crudwork.Utilities;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using crudwork.Models;

namespace crudwork.Controls.Wizard
{
	/// <summary>
	/// The wizard main form
	/// </summary>
	[Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner)), ToolboxItemFilter("System.Windows.Forms.Control.TopLevel"), DesignerCategory("Form"), DefaultEvent("Load"), ToolboxItem(false), DesignTimeVisible(false), ComVisible(true), InitializationEvent("Load"), ClassInterface(ClassInterfaceType.AutoDispatch)]
	public partial class Wizard : Form
	{
		private string originalTitle = null;
		private List<UserControl> wizardControls;
		private int currentStep = -1;

		#region Custom Event Handlers
		/// <summary>
		/// Subscribe to this event to display help when the user clicks on the Help button
		/// </summary>
		/// <remarks>
		/// To utilize this feature, subscribe to this event handler and set the EnableHelp property to true.
		/// </remarks>
		public event WizardEventHandler OnHelpClick = null;

		/// <summary>
		/// Subscribe to this event to perform custom action when the user clicks on the [Custom] button
		/// </summary>
		/// <remarks>
		/// To utilize this feature, subscribe to this event handler, set the EnableCustom property to true,
		/// and set a text to the CustomButtonText property.
		/// </remarks>
		public event WizardEventHandler OnCustomClick = null;

		/// <summary>
		/// Subscribe to this event to start the procedure after the user clicks the 'Finish' button.
		/// </summary>
		public event WizardEventHandler OnExecute = null;
		#endregion

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public Wizard()
		{
			InitializeComponent();

			EnableCustom = false;
			EnableHelp = false;
		}

		#region Application Events
		private void Wizard_Load(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				RefreshWizard(0);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}

		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			try
			{
				var tmp = OnHelpClick;
				if (tmp == null)
					return;

				FormUtil.Busy(this, true);

				var exlist = new AggregatedException();

				foreach (WizardEventHandler eh in tmp.GetInvocationList())
				{
					try
					{
						eh(sender, new WizardEventArgs(currentStep));
					}
					catch (Exception ex)
					{
						exlist.Add(ex);
					}
				}

				if (exlist.Count > 0)
					throw exlist;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}

		}

		private void btnCustom_Click(object sender, EventArgs e)
		{
			try
			{
				var tmp = OnCustomClick;
				if (tmp == null)
					return;

				FormUtil.Busy(this, true);

				var exlist = new AggregatedException();

				foreach (WizardEventHandler eh in tmp.GetInvocationList())
				{
					try
					{
						eh(sender, new WizardEventArgs(currentStep));
					}
					catch (Exception ex)
					{
						exlist.Add(ex);
					}
				}

				if (exlist.Count > 0)
					throw exlist;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}

		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				RefreshWizard(-1);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			try
			{
				FormUtil.Busy(this, true);

				if (btnNext.Text == "Finish")
				{
					RaiseExecuteEventHandlers();
					this.DialogResult = DialogResult.OK;
					this.Close();
				}

				if (!ValidateOne(currentStep))
					return;

				RefreshWizard(+1);
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		#region Helpers
		private void RaiseExecuteEventHandlers()
		{
			try
			{
				var tmp = OnExecute;
				if (tmp == null)
					return;

				FormUtil.Busy(this, true);

				var exlist = new AggregatedException();

				foreach (WizardEventHandler eh in tmp.GetInvocationList())
				{
					try
					{
						eh(this, new WizardEventArgs(currentStep));
					}
					catch (Exception ex)
					{
						exlist.Add(ex);
					}
				}

				if (exlist.Count > 0)
					throw exlist;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
			finally
			{
				FormUtil.Busy(this, false);
			}
		}


		/// <summary>
		/// Show the current wizard control on the client area, enable/disable the Back 
		/// and Next buttons, etc...
		/// </summary>
		/// <param name="direction"></param>
		private void RefreshWizard(int direction)
		{
			try
			{
				if (wizardControls.Count == 0)
					return;

				int prevIndex = currentStep;

				#region Determine the next step
				switch (direction)
				{
					case 0:
						// first time initialization
						prevIndex = 0;
						currentStep = 0;
						break;

					case -1:
						// back button
						if (currentStep == 0)
							return; /*nothing to do*/
						currentStep--;
						break;

					case 1:
						// next button
						if (currentStep + 1 >= wizardControls.Count)
							return; /*nothing to do*/
						currentStep++;
						break;

					default:
						throw new ArgumentOutOfRangeException("direction=" + direction);
				}
				#endregion

				int nextIndex = currentStep;

				var prev = WizardControls[prevIndex];
				var next = WizardControls[nextIndex];

				#region Update Title
				Title = string.Format("{0} (Step {1} of {2})", originalTitle, nextIndex + 1, WizardControls.Count);
				#endregion

				#region Enable or disable the Back/Next buttons
				btnBack.Enabled = (nextIndex > 0);
				btnNext.Enabled = (nextIndex < WizardControls.Count);
				#endregion

				#region Sanity Checks - enforce main architecture design
				// double check: verify that both controls implement the IWizard interface
				GetWizard(prev);
				GetWizard(next);
				#endregion

				#region show the next wizard control on the client section
				next.Dock = DockStyle.Fill;
				GetWizard(next).RefreshContent();
				ClientPane.Controls.Clear();
				ClientPane.Controls.Add(next);
				#endregion

				#region determine the logical step for the NEXT button
				// if all wizard controls validated, this button behaves like a 'Finish' (a.k.a., OK)
				// button; otherwise, it behaves like a 'Cancel' button.
				var isValidated = ValidateAll();

				if (isValidated && nextIndex + 1 == wizardControls.Count)
				{
					btnNext.Text = "Finish";
				}
				else
				{
					btnNext.Text = "Next ->";
				}
				#endregion
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}

		/// <summary>
		/// Return the IWizardControl interface.  If the Control does not implement it, an exception will be thrown.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private IWizardControl GetWizard(Control c)
		{
			var iWiz = c as IWizardControl;

			#region Sanity Checks - enforce main architecture design
			if (iWiz == null)
				throw new ArgumentException("Control must implement the IWizardControl interface: " + c.Name);
			#endregion

			return iWiz;
		}

		/// <summary>
		/// Invoke the Validate() method for the wizard control at the index specified.
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		private bool ValidateOne(int idx)
		{
			try
			{
				var iWiz = GetWizard(WizardControls[idx]);
				iWiz.ValidateContent();

				// control validated successfully...
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Invoke the Validate() method for all wizard controls
		/// </summary>
		/// <returns></returns>
		private bool ValidateAll()
		{
			try
			{
				foreach (var c in WizardControls)
				{
					var iWiz = GetWizard(c);
					iWiz.ValidateContent();
				}

				// all control validated successfully...
				return true;
			}
			catch (Exception /*ex*/)
			{
				//MessageBox.Show(ex.Message);
				return false;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set the wizard controls
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the wizard controls"), DefaultValue(null)]
		public List<UserControl> WizardControls
		{
			get
			{
				if (wizardControls == null)
					wizardControls = new List<UserControl>();
				return wizardControls;
			}
			set
			{
				if (value == null || value.Count == 0)
					return;

				#region Ensure all controls implements the IWizardControl
				for (int i = 0; i < value.Count; i++)
				{
					GetWizard(value[i]);
				}
				#endregion

				wizardControls = value;
			}
		}

		/// <summary>
		/// Get or set the window title
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the window title"), DefaultValue(null)]
		public string Title
		{
			get
			{
				return this.Text;
			}
			set
			{
				if (this.originalTitle == null)
					this.originalTitle = value;
				this.Text = value;
			}
		}

		/// <summary>
		/// Show or hide the Custom button
		/// </summary>
		[Category("Custom Settings"), Description("Enable or disable the Custom button"), DefaultValue(false)]
		public bool EnableCustom
		{
			get
			{
				return btnCustom.Enabled;
			}
			set
			{
				btnCustom.Enabled = value;
				btnCustom.Visible = value;
			}
		}

		/// <summary>
		/// Get or set the text of the Custom button.
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the text of the Custom button"), DefaultValue(null)]
		public string CustomButtonText
		{
			get
			{
				return btnCustom.Text;
			}
			set
			{
				btnCustom.Text = value;
			}
		}

		/// <summary>
		/// Show or hide the Custom button
		/// </summary>
		[Category("Custom Settings"), Description("Enable or disable the Help button"), DefaultValue(false)]
		public bool EnableHelp
		{
			get
			{
				return btnHelp.Enabled;
			}
			set
			{
				btnHelp.Enabled = value;
				btnHelp.Visible = value;
			}
		}

		/// <summary>
		/// Get reference to the header pane.
		/// Use this property to fully utilize the panel's capability.
		/// </summary>
		[Category("Custom Settings"), Description("Get reference to the header pane")]
		public Panel HeaderPane
		{
			get
			{
				return pnlHeader;
			}
			private set
			{
				pnlHeader = value;
			}
		}

		/// <summary>
		/// Get reference to the left-hand-side pane.
		/// Use this property to fully utilize the panel's capability.
		/// </summary>
		[Category("Custom Settings"), Description("Get reference to the left-hand-side pane")]
		public Panel LHSPane
		{
			get
			{
				return pnlLHS;
			}
			private set
			{
				pnlLHS = value;
			}
		}

		/// <summary>
		/// Get or set the client pane.
		/// Use this property to fully utilize the panel's capability.
		/// </summary>
		[Category("Custom Settings"), Description("Get reference to the client pane")]
		private Panel ClientPane
		{
			get
			{
				return pnlClient;
			}
			set
			{
				pnlClient = value;
			}
		}

		/// <summary>
		/// Show or hide the header pane
		/// </summary>
		[Category("Custom Settings"), Description("Show or hide the header pane")]
		public bool ShowHeader
		{
			get
			{
				return !scVertical.Panel1Collapsed;
			}
			set
			{
				scVertical.Panel1Collapsed = !value;
			}
		}

		/// <summary>
		/// Show or hide the left-hand-side pane
		/// </summary>
		[Category("Custom Settings"), Description("Show or hide the left-hand-side pane")]
		public bool ShowLHS
		{
			get
			{
				return !scHorizontal.Panel1Collapsed;
			}
			set
			{
				scHorizontal.Panel1Collapsed = !value;
			}
		}
		#endregion
	}
}
