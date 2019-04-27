namespace crudwork.Controls.Wizard
{
	partial class Wizard
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlClient = new System.Windows.Forms.Panel();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlLHS = new System.Windows.Forms.Panel();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCustom = new System.Windows.Forms.Button();
			this.scHorizontal = new System.Windows.Forms.SplitContainer();
			this.scVertical = new System.Windows.Forms.SplitContainer();
			this.scHorizontal.Panel1.SuspendLayout();
			this.scHorizontal.Panel2.SuspendLayout();
			this.scHorizontal.SuspendLayout();
			this.scVertical.Panel1.SuspendLayout();
			this.scVertical.Panel2.SuspendLayout();
			this.scVertical.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlClient
			// 
			this.pnlClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClient.Location = new System.Drawing.Point(0, 0);
			this.pnlClient.Name = "pnlClient";
			this.pnlClient.Size = new System.Drawing.Size(393, 283);
			this.pnlClient.TabIndex = 0;
			// 
			// btnBack
			// 
			this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBack.Location = new System.Drawing.Point(241, 338);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(75, 23);
			this.btnBack.TabIndex = 4;
			this.btnBack.Text = "<- &Back";
			this.btnBack.UseVisualStyleBackColor = true;
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNext.Location = new System.Drawing.Point(323, 338);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 5;
			this.btnNext.Text = "&Next ->";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(405, 338);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pnlLHS
			// 
			this.pnlLHS.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLHS.Location = new System.Drawing.Point(0, 0);
			this.pnlLHS.Name = "pnlLHS";
			this.pnlLHS.Size = new System.Drawing.Size(100, 283);
			this.pnlLHS.TabIndex = 0;
			// 
			// pnlHeader
			// 
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(494, 50);
			this.pnlHeader.TabIndex = 0;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnHelp.Location = new System.Drawing.Point(12, 338);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 2;
			this.btnHelp.Text = "&Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnCustom
			// 
			this.btnCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCustom.Location = new System.Drawing.Point(93, 338);
			this.btnCustom.Name = "btnCustom";
			this.btnCustom.Size = new System.Drawing.Size(75, 23);
			this.btnCustom.TabIndex = 3;
			this.btnCustom.Text = "[Custom]";
			this.btnCustom.UseVisualStyleBackColor = true;
			this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
			// 
			// scHorizontal
			// 
			this.scHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scHorizontal.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.scHorizontal.IsSplitterFixed = true;
			this.scHorizontal.Location = new System.Drawing.Point(0, 0);
			this.scHorizontal.Margin = new System.Windows.Forms.Padding(0);
			this.scHorizontal.Name = "scHorizontal";
			// 
			// scHorizontal.Panel1
			// 
			this.scHorizontal.Panel1.Controls.Add(this.pnlLHS);
			// 
			// scHorizontal.Panel2
			// 
			this.scHorizontal.Panel2.Controls.Add(this.pnlClient);
			this.scHorizontal.Size = new System.Drawing.Size(494, 283);
			this.scHorizontal.SplitterDistance = 100;
			this.scHorizontal.SplitterWidth = 1;
			this.scHorizontal.TabIndex = 1;
			this.scHorizontal.TabStop = false;
			// 
			// scVertical
			// 
			this.scVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.scVertical.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.scVertical.IsSplitterFixed = true;
			this.scVertical.Location = new System.Drawing.Point(0, 0);
			this.scVertical.Margin = new System.Windows.Forms.Padding(0);
			this.scVertical.Name = "scVertical";
			this.scVertical.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scVertical.Panel1
			// 
			this.scVertical.Panel1.Controls.Add(this.pnlHeader);
			// 
			// scVertical.Panel2
			// 
			this.scVertical.Panel2.Controls.Add(this.scHorizontal);
			this.scVertical.Size = new System.Drawing.Size(494, 334);
			this.scVertical.SplitterWidth = 1;
			this.scVertical.TabIndex = 0;
			this.scVertical.TabStop = false;
			// 
			// Wizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 373);
			this.Controls.Add(this.scVertical);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnCustom);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 400);
			this.Name = "Wizard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Wizard";
			this.Load += new System.EventHandler(this.Wizard_Load);
			this.scHorizontal.Panel1.ResumeLayout(false);
			this.scHorizontal.Panel2.ResumeLayout(false);
			this.scHorizontal.ResumeLayout(false);
			this.scVertical.Panel1.ResumeLayout(false);
			this.scVertical.Panel2.ResumeLayout(false);
			this.scVertical.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlClient;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlLHS;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Button btnCustom;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.SplitContainer scHorizontal;
		private System.Windows.Forms.SplitContainer scVertical;

	}
}