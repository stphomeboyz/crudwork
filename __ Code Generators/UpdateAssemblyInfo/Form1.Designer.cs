namespace UpdateAssemblyInfo
{
	partial class Form1
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
			this.txtSolutionFolder = new System.Windows.Forms.TextBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtCopyright = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtProduct = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtCompany = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnIncreaseBuild = new System.Windows.Forms.Button();
			this.btnIncreaseRevision = new System.Windows.Forms.Button();
			this.btnIncreaseMinor = new System.Windows.Forms.Button();
			this.btnIncreaseMajor = new System.Windows.Forms.Button();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtSolutionFolder
			// 
			this.txtSolutionFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSolutionFolder.Location = new System.Drawing.Point(118, 13);
			this.txtSolutionFolder.Name = "txtSolutionFolder";
			this.txtSolutionFolder.Size = new System.Drawing.Size(262, 20);
			this.txtSolutionFolder.TabIndex = 1;
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(305, 202);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 18;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// txtVersion
			// 
			this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtVersion.Location = new System.Drawing.Point(118, 40);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.Size = new System.Drawing.Size(262, 20);
			this.txtVersion.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Solution Folder";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Version";
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLoad.Location = new System.Drawing.Point(12, 202);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 16;
			this.btnLoad.Text = "Load...";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Location = new System.Drawing.Point(93, 202);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 17;
			this.btnSave.Text = "Save...";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtCopyright
			// 
			this.txtCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCopyright.Location = new System.Drawing.Point(118, 171);
			this.txtCopyright.Name = "txtCopyright";
			this.txtCopyright.Size = new System.Drawing.Size(262, 20);
			this.txtCopyright.TabIndex = 15;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 174);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 23);
			this.label4.TabIndex = 14;
			this.label4.Text = "Copyright";
			// 
			// txtProduct
			// 
			this.txtProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProduct.Location = new System.Drawing.Point(118, 145);
			this.txtProduct.Name = "txtProduct";
			this.txtProduct.Size = new System.Drawing.Size(262, 20);
			this.txtProduct.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 148);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 23);
			this.label5.TabIndex = 12;
			this.label5.Text = "Product";
			// 
			// txtCompany
			// 
			this.txtCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCompany.Location = new System.Drawing.Point(118, 119);
			this.txtCompany.Name = "txtCompany";
			this.txtCompany.Size = new System.Drawing.Size(262, 20);
			this.txtCompany.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 122);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 23);
			this.label6.TabIndex = 10;
			this.label6.Text = "Company";
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(118, 93);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(262, 20);
			this.txtDescription.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(12, 96);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 23);
			this.label7.TabIndex = 8;
			this.label7.Text = "Description";
			// 
			// btnIncreaseBuild
			// 
			this.btnIncreaseBuild.Location = new System.Drawing.Point(321, 64);
			this.btnIncreaseBuild.Name = "btnIncreaseBuild";
			this.btnIncreaseBuild.Size = new System.Drawing.Size(59, 23);
			this.btnIncreaseBuild.TabIndex = 7;
			this.btnIncreaseBuild.Tag = "Build";
			this.btnIncreaseBuild.Text = "+Build";
			this.btnIncreaseBuild.UseVisualStyleBackColor = true;
			this.btnIncreaseBuild.Click += new System.EventHandler(this.btnIncreaseVersion_Click);
			// 
			// btnIncreaseRevision
			// 
			this.btnIncreaseRevision.Location = new System.Drawing.Point(253, 64);
			this.btnIncreaseRevision.Name = "btnIncreaseRevision";
			this.btnIncreaseRevision.Size = new System.Drawing.Size(62, 23);
			this.btnIncreaseRevision.TabIndex = 6;
			this.btnIncreaseRevision.Tag = "Revision";
			this.btnIncreaseRevision.Text = "+Revision";
			this.btnIncreaseRevision.UseVisualStyleBackColor = true;
			this.btnIncreaseRevision.Click += new System.EventHandler(this.btnIncreaseVersion_Click);
			// 
			// btnIncreaseMinor
			// 
			this.btnIncreaseMinor.Location = new System.Drawing.Point(188, 64);
			this.btnIncreaseMinor.Name = "btnIncreaseMinor";
			this.btnIncreaseMinor.Size = new System.Drawing.Size(59, 23);
			this.btnIncreaseMinor.TabIndex = 5;
			this.btnIncreaseMinor.Tag = "Minor";
			this.btnIncreaseMinor.Text = "+Minor";
			this.btnIncreaseMinor.UseVisualStyleBackColor = true;
			this.btnIncreaseMinor.Click += new System.EventHandler(this.btnIncreaseVersion_Click);
			// 
			// btnIncreaseMajor
			// 
			this.btnIncreaseMajor.Location = new System.Drawing.Point(123, 64);
			this.btnIncreaseMajor.Name = "btnIncreaseMajor";
			this.btnIncreaseMajor.Size = new System.Drawing.Size(59, 23);
			this.btnIncreaseMajor.TabIndex = 4;
			this.btnIncreaseMajor.Tag = "Major";
			this.btnIncreaseMajor.Text = "+Major";
			this.btnIncreaseMajor.UseVisualStyleBackColor = true;
			this.btnIncreaseMajor.Click += new System.EventHandler(this.btnIncreaseVersion_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSaveAs.Location = new System.Drawing.Point(174, 202);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
			this.btnSaveAs.TabIndex = 19;
			this.btnSaveAs.Text = "Save As...";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 237);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.btnIncreaseMajor);
			this.Controls.Add(this.btnIncreaseMinor);
			this.Controls.Add(this.btnIncreaseRevision);
			this.Controls.Add(this.btnIncreaseBuild);
			this.Controls.Add(this.txtCopyright);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtProduct);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtCompany);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtVersion);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.txtSolutionFolder);
			this.MinimumSize = new System.Drawing.Size(400, 264);
			this.Name = "Form1";
			this.Text = "Update AssemblyInfo.cs File";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSolutionFolder;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.TextBox txtVersion;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtCopyright;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtProduct;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtCompany;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnIncreaseBuild;
		private System.Windows.Forms.Button btnIncreaseRevision;
		private System.Windows.Forms.Button btnIncreaseMinor;
		private System.Windows.Forms.Button btnIncreaseMajor;
		private System.Windows.Forms.Button btnSaveAs;
	}
}

