namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	partial class ChooseInputSource
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
			this.lblFilename = new System.Windows.Forms.Label();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnBrowseFilename = new System.Windows.Forms.Button();
			this.cboDataSource = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBuildConnectionString = new System.Windows.Forms.Button();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.lblConnectionString = new System.Windows.Forms.Label();
			this.lblTablename = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.cboTablename = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// lblFilename
			// 
			this.lblFilename.AutoSize = true;
			this.lblFilename.Location = new System.Drawing.Point(3, 133);
			this.lblFilename.Name = "lblFilename";
			this.lblFilename.Size = new System.Drawing.Size(52, 13);
			this.lblFilename.TabIndex = 3;
			this.lblFilename.Tag = "Filename";
			this.lblFilename.Text = "Filename:";
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtFilename.Location = new System.Drawing.Point(3, 149);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(551, 20);
			this.txtFilename.TabIndex = 4;
			this.txtFilename.Tag = "Filename";
			// 
			// btnBrowseFilename
			// 
			this.btnBrowseFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFilename.Location = new System.Drawing.Point(560, 147);
			this.btnBrowseFilename.Name = "btnBrowseFilename";
			this.btnBrowseFilename.Size = new System.Drawing.Size(37, 23);
			this.btnBrowseFilename.TabIndex = 5;
			this.btnBrowseFilename.Tag = "Filename";
			this.btnBrowseFilename.Text = "...";
			this.btnBrowseFilename.UseVisualStyleBackColor = true;
			this.btnBrowseFilename.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// cboDataSource
			// 
			this.cboDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDataSource.FormattingEnabled = true;
			this.cboDataSource.Location = new System.Drawing.Point(3, 99);
			this.cboDataSource.Name = "cboDataSource";
			this.cboDataSource.Size = new System.Drawing.Size(156, 21);
			this.cboDataSource.TabIndex = 2;
			this.cboDataSource.Tag = "";
			this.cboDataSource.SelectedIndexChanged += new System.EventHandler(this.cboDataSource_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 1;
			this.label2.Tag = "";
			this.label2.Text = "Data Source:";
			// 
			// btnBuildConnectionString
			// 
			this.btnBuildConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBuildConnectionString.Location = new System.Drawing.Point(560, 198);
			this.btnBuildConnectionString.Name = "btnBuildConnectionString";
			this.btnBuildConnectionString.Size = new System.Drawing.Size(37, 23);
			this.btnBuildConnectionString.TabIndex = 8;
			this.btnBuildConnectionString.Tag = "ConnectionString";
			this.btnBuildConnectionString.Text = "...";
			this.btnBuildConnectionString.UseVisualStyleBackColor = true;
			this.btnBuildConnectionString.Click += new System.EventHandler(this.btnBuildConnectionString_Click);
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtConnectionString.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtConnectionString.Location = new System.Drawing.Point(3, 200);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(551, 20);
			this.txtConnectionString.TabIndex = 7;
			this.txtConnectionString.Tag = "ConnectionString";
			// 
			// lblConnectionString
			// 
			this.lblConnectionString.AutoSize = true;
			this.lblConnectionString.Location = new System.Drawing.Point(3, 184);
			this.lblConnectionString.Name = "lblConnectionString";
			this.lblConnectionString.Size = new System.Drawing.Size(94, 13);
			this.lblConnectionString.TabIndex = 6;
			this.lblConnectionString.Tag = "ConnectionString";
			this.lblConnectionString.Text = "Connection String:";
			// 
			// lblTablename
			// 
			this.lblTablename.AutoSize = true;
			this.lblTablename.Location = new System.Drawing.Point(3, 232);
			this.lblTablename.Name = "lblTablename";
			this.lblTablename.Size = new System.Drawing.Size(68, 13);
			this.lblTablename.TabIndex = 9;
			this.lblTablename.Tag = "Tablename";
			this.lblTablename.Text = "Table Name:";
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.Location = new System.Drawing.Point(3, 0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new System.Windows.Forms.Padding(10);
			this.lblDescription.Size = new System.Drawing.Size(594, 69);
			this.lblDescription.TabIndex = 0;
			this.lblDescription.Text = "[Description Goes Here...]";
			// 
			// cboTablename
			// 
			this.cboTablename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboTablename.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTablename.FormattingEnabled = true;
			this.cboTablename.Location = new System.Drawing.Point(3, 248);
			this.cboTablename.Name = "cboTablename";
			this.cboTablename.Size = new System.Drawing.Size(551, 21);
			this.cboTablename.TabIndex = 10;
			this.cboTablename.Tag = "Tablename";
			this.cboTablename.Enter += new System.EventHandler(this.cboTablename_Enter);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 284);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(591, 100);
			this.flowLayoutPanel1.TabIndex = 11;
			// 
			// ChooseInputSource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.cboTablename);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblTablename);
			this.Controls.Add(this.btnBuildConnectionString);
			this.Controls.Add(this.txtConnectionString);
			this.Controls.Add(this.lblConnectionString);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cboDataSource);
			this.Controls.Add(this.btnBrowseFilename);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.lblFilename);
			this.Name = "ChooseInputSource";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.WizardStep1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFilename;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnBrowseFilename;
		private System.Windows.Forms.ComboBox cboDataSource;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBuildConnectionString;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label lblConnectionString;
		private System.Windows.Forms.Label lblTablename;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.ComboBox cboTablename;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

	}
}
