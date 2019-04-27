namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	partial class HeaderPane
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeaderPane));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.hrSep = new crudwork.Controls.UserControlEx();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::crudwork.Controls.Properties.Resources.ExportIcon_50x50_px;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(50, 50);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Visible = false;
			// 
			// hrSep
			// 
			this.hrSep.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hrSep.BackgroundImage")));
			this.hrSep.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.hrSep.Color1 = System.Drawing.Color.RoyalBlue;
			this.hrSep.Color2 = System.Drawing.Color.Transparent;
			this.hrSep.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hrSep.HatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
			this.hrSep.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
			this.hrSep.Location = new System.Drawing.Point(0, 45);
			this.hrSep.Name = "hrSep";
			this.hrSep.Size = new System.Drawing.Size(600, 5);
			this.hrSep.TabIndex = 2;
			// 
			// WizardHeaderPane
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.hrSep);
			this.Controls.Add(this.pictureBox1);
			this.DoubleBuffered = true;
			this.Name = "WizardHeaderPane";
			this.Size = new System.Drawing.Size(600, 50);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private crudwork.Controls.UserControlEx hrSep;
	}
}
