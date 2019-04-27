namespace crudwork.Controls
{
	partial class RDBMSConnectionStringBuilder
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtService = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkIntegratedSecurity = new System.Windows.Forms.CheckBox();
			this.txtOthers = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.chkMaskPassword = new System.Windows.Forms.CheckBox();
			this.cboDatabase = new System.Windows.Forms.ComboBox();
			this.btnBrowserService = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Service Name:";
			// 
			// txtService
			// 
			this.txtService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtService.Location = new System.Drawing.Point(88, 3);
			this.txtService.Name = "txtService";
			this.txtService.Size = new System.Drawing.Size(228, 20);
			this.txtService.TabIndex = 1;
			this.txtService.TextChanged += new System.EventHandler(this.txtService_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 108);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Database:";
			// 
			// txtUserID
			// 
			this.txtUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUserID.Location = new System.Drawing.Point(88, 52);
			this.txtUserID.Name = "txtUserID";
			this.txtUserID.Size = new System.Drawing.Size(309, 20);
			this.txtUserID.TabIndex = 6;
			this.txtUserID.TextChanged += new System.EventHandler(this.txtUserID_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Username:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(88, 78);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(309, 20);
			this.txtPassword.TabIndex = 8;
			this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Password:";
			// 
			// chkIntegratedSecurity
			// 
			this.chkIntegratedSecurity.AutoSize = true;
			this.chkIntegratedSecurity.Location = new System.Drawing.Point(88, 29);
			this.chkIntegratedSecurity.Name = "chkIntegratedSecurity";
			this.chkIntegratedSecurity.Size = new System.Drawing.Size(137, 17);
			this.chkIntegratedSecurity.TabIndex = 3;
			this.chkIntegratedSecurity.Text = "Use Integrated Security";
			this.chkIntegratedSecurity.UseVisualStyleBackColor = true;
			this.chkIntegratedSecurity.Click += new System.EventHandler(this.chkIntegratedSecurity_Click);
			// 
			// txtOthers
			// 
			this.txtOthers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOthers.Location = new System.Drawing.Point(88, 131);
			this.txtOthers.Multiline = true;
			this.txtOthers.Name = "txtOthers";
			this.txtOthers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOthers.Size = new System.Drawing.Size(309, 66);
			this.txtOthers.TabIndex = 12;
			this.txtOthers.WordWrap = false;
			this.txtOthers.TextChanged += new System.EventHandler(this.txtOthers_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(3, 134);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Others:";
			// 
			// chkMaskPassword
			// 
			this.chkMaskPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMaskPassword.AutoSize = true;
			this.chkMaskPassword.Location = new System.Drawing.Point(296, 29);
			this.chkMaskPassword.Name = "chkMaskPassword";
			this.chkMaskPassword.Size = new System.Drawing.Size(101, 17);
			this.chkMaskPassword.TabIndex = 4;
			this.chkMaskPassword.Text = "Mask Password";
			this.chkMaskPassword.UseVisualStyleBackColor = true;
			this.chkMaskPassword.Click += new System.EventHandler(this.chkMaskPassword_Click);
			// 
			// cboDatabase
			// 
			this.cboDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabase.FormattingEnabled = true;
			this.cboDatabase.Location = new System.Drawing.Point(88, 104);
			this.cboDatabase.Name = "cboDatabase";
			this.cboDatabase.Size = new System.Drawing.Size(309, 21);
			this.cboDatabase.TabIndex = 10;
			this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
			this.cboDatabase.Enter += new System.EventHandler(this.cboDatabase_Enter);
			// 
			// btnBrowserService
			// 
			this.btnBrowserService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowserService.Location = new System.Drawing.Point(322, 1);
			this.btnBrowserService.Name = "btnBrowserService";
			this.btnBrowserService.Size = new System.Drawing.Size(75, 23);
			this.btnBrowserService.TabIndex = 2;
			this.btnBrowserService.Text = "Browse...";
			this.btnBrowserService.UseVisualStyleBackColor = true;
			this.btnBrowserService.Click += new System.EventHandler(this.btnBrowserService_Click);
			// 
			// RDBMSConnectionStringBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnBrowserService);
			this.Controls.Add(this.cboDatabase);
			this.Controls.Add(this.chkMaskPassword);
			this.Controls.Add(this.txtOthers);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkIntegratedSecurity);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtUserID);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtService);
			this.Controls.Add(this.label1);
			this.Name = "RDBMSConnectionStringBuilder";
			this.Size = new System.Drawing.Size(400, 200);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtService;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtUserID;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkIntegratedSecurity;
		private System.Windows.Forms.TextBox txtOthers;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkMaskPassword;
		private System.Windows.Forms.ComboBox cboDatabase;
		private System.Windows.Forms.Button btnBrowserService;
	}
}
