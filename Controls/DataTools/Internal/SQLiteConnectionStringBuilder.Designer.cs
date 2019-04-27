namespace crudwork.Controls.DataTools.Internal
{
	partial class SQLiteConnectionStringBuilder
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
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkReadOnly = new System.Windows.Forms.CheckBox();
			this.chkFileMustExist = new System.Windows.Forms.CheckBox();
			this.chkMaskPassword = new System.Windows.Forms.CheckBox();
			this.chkCompress = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFile.Location = new System.Drawing.Point(322, 3);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "Browse...";
			this.btnBrowseFile.UseVisualStyleBackColor = true;
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(88, 5);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(228, 20);
			this.txtFilename.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filename:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(88, 54);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(309, 20);
			this.txtPassword.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Password:";
			// 
			// chkReadOnly
			// 
			this.chkReadOnly.AutoSize = true;
			this.chkReadOnly.Location = new System.Drawing.Point(88, 80);
			this.chkReadOnly.Name = "chkReadOnly";
			this.chkReadOnly.Size = new System.Drawing.Size(76, 17);
			this.chkReadOnly.TabIndex = 6;
			this.chkReadOnly.Text = "Read Only";
			this.chkReadOnly.UseVisualStyleBackColor = true;
			// 
			// chkFileMustExist
			// 
			this.chkFileMustExist.AutoSize = true;
			this.chkFileMustExist.Location = new System.Drawing.Point(88, 103);
			this.chkFileMustExist.Name = "chkFileMustExist";
			this.chkFileMustExist.Size = new System.Drawing.Size(93, 17);
			this.chkFileMustExist.TabIndex = 7;
			this.chkFileMustExist.Text = "File Must Exist";
			this.chkFileMustExist.UseVisualStyleBackColor = true;
			// 
			// chkMaskPassword
			// 
			this.chkMaskPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMaskPassword.AutoSize = true;
			this.chkMaskPassword.Location = new System.Drawing.Point(296, 31);
			this.chkMaskPassword.Name = "chkMaskPassword";
			this.chkMaskPassword.Size = new System.Drawing.Size(101, 17);
			this.chkMaskPassword.TabIndex = 3;
			this.chkMaskPassword.Text = "Mask Password";
			this.chkMaskPassword.UseVisualStyleBackColor = true;
			this.chkMaskPassword.CheckedChanged += new System.EventHandler(this.chkMaskPassword_CheckedChanged);
			// 
			// chkCompress
			// 
			this.chkCompress.AutoSize = true;
			this.chkCompress.Location = new System.Drawing.Point(88, 126);
			this.chkCompress.Name = "chkCompress";
			this.chkCompress.Size = new System.Drawing.Size(72, 17);
			this.chkCompress.TabIndex = 8;
			this.chkCompress.Text = "Compress";
			this.chkCompress.UseVisualStyleBackColor = true;
			// 
			// SQLiteConnectionStringBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkCompress);
			this.Controls.Add(this.chkMaskPassword);
			this.Controls.Add(this.chkFileMustExist);
			this.Controls.Add(this.chkReadOnly);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnBrowseFile);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.label1);
			this.Name = "SQLiteConnectionStringBuilder";
			this.Size = new System.Drawing.Size(400, 200);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkReadOnly;
		private System.Windows.Forms.CheckBox chkFileMustExist;
		private System.Windows.Forms.CheckBox chkMaskPassword;
		private System.Windows.Forms.CheckBox chkCompress;
	}
}
