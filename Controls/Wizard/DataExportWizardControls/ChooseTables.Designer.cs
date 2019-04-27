namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	partial class ChooseTables
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseTables));
			this.lblDescription = new System.Windows.Forms.Label();
			this.chooseListBox1 = new crudwork.Controls.ChooseListBox();
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
			this.lblDescription.TabIndex = 2;
			this.lblDescription.Text = "[Description Goes Here...]";
			// 
			// chooseListBox1
			// 
			this.chooseListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chooseListBox1.Location = new System.Drawing.Point(3, 72);
			this.chooseListBox1.Name = "chooseListBox1";
			this.chooseListBox1.Options = ((System.Collections.Generic.Dictionary<string, bool>)(resources.GetObject("chooseListBox1.Options")));
			this.chooseListBox1.Size = new System.Drawing.Size(594, 325);
			this.chooseListBox1.TabIndex = 3;
			// 
			// ChooseTables
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chooseListBox1);
			this.Controls.Add(this.lblDescription);
			this.Name = "ChooseTables";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.ChooseTables_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblDescription;
		private ChooseListBox chooseListBox1;
	}
}
