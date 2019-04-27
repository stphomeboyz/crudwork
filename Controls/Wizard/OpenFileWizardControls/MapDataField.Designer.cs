namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	partial class MapDataField
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
			this.dcm = new crudwork.Controls.DataColumnMapper();
			this.lblDescription = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// dcm
			// 
			this.dcm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dcm.Location = new System.Drawing.Point(6, 72);
			this.dcm.Name = "dcm";
			this.dcm.ShowChildGrid = true;
			this.dcm.ShowParentGrid = true;
			this.dcm.Size = new System.Drawing.Size(591, 325);
			this.dcm.TabIndex = 1;
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.Location = new System.Drawing.Point(3, 0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new System.Windows.Forms.Padding(10);
			this.lblDescription.Size = new System.Drawing.Size(594, 69);
			this.lblDescription.TabIndex = 12;
			this.lblDescription.Text = "[Description Goes Here...]";
			// 
			// MapDataField
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.dcm);
			this.Name = "MapDataField";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.WizardStep2_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private crudwork.Controls.DataColumnMapper dcm;
		private System.Windows.Forms.Label lblDescription;

	}
}
