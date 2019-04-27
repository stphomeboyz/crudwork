namespace crudwork.Controls.Wizard.PivotTableWizardControls
{
	partial class Introduction
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblIntroduction = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.Location = new System.Drawing.Point(3, 0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new System.Windows.Forms.Padding(10);
			this.lblDescription.Size = new System.Drawing.Size(594, 69);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "[Description Goes Here...]";
			// 
			// lblIntroduction
			// 
			this.lblIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntroduction.Location = new System.Drawing.Point(3, 69);
			this.lblIntroduction.Name = "lblIntroduction";
			this.lblIntroduction.Padding = new System.Windows.Forms.Padding(20);
			this.lblIntroduction.Size = new System.Drawing.Size(594, 331);
			this.lblIntroduction.TabIndex = 2;
			this.lblIntroduction.Text = "[Introduction Goes Here...]";
			// 
			// WizardIntro
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblIntroduction);
			this.Controls.Add(this.lblDescription);
			this.Name = "WizardIntro";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.WizardStart_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblIntroduction;
	}
}
