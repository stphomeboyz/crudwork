namespace crudwork.Controls.FormControls
{
	partial class ConnectionStringForm
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
			this.cboDatabaseProvider = new System.Windows.Forms.ComboBox();
			this.lblFieldname = new System.Windows.Forms.Label();
			this.cboSourceType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtOptions = new crudwork.Controls.FormControls.TextBoxForm();
			this.txtFilename = new crudwork.Controls.FormControls.TextBoxForm();
			this.txtConnectionString = new crudwork.Controls.FormControls.TextBoxForm();
			this.SuspendLayout();
			// 
			// cboDatabaseProvider
			// 
			this.cboDatabaseProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabaseProvider.FormattingEnabled = true;
			this.cboDatabaseProvider.Location = new System.Drawing.Point(15, 76);
			this.cboDatabaseProvider.Name = "cboDatabaseProvider";
			this.cboDatabaseProvider.Size = new System.Drawing.Size(99, 21);
			this.cboDatabaseProvider.TabIndex = 3;
			// 
			// lblFieldname
			// 
			this.lblFieldname.AutoSize = true;
			this.lblFieldname.Location = new System.Drawing.Point(12, 57);
			this.lblFieldname.Name = "lblFieldname";
			this.lblFieldname.Size = new System.Drawing.Size(94, 13);
			this.lblFieldname.TabIndex = 2;
			this.lblFieldname.Text = "Connection String:";
			// 
			// cboSourceType
			// 
			this.cboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSourceType.FormattingEnabled = true;
			this.cboSourceType.Location = new System.Drawing.Point(15, 20);
			this.cboSourceType.Name = "cboSourceType";
			this.cboSourceType.Size = new System.Drawing.Size(99, 21);
			this.cboSourceType.TabIndex = 1;
			this.cboSourceType.SelectedIndexChanged += new System.EventHandler(this.cboSourceType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source Type:";
			// 
			// txtOptions
			// 
			this.txtOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOptions.Label = "Options:";
			this.txtOptions.Location = new System.Drawing.Point(7, 163);
			this.txtOptions.Name = "txtOptions";
			this.txtOptions.ShowEllipseButton = true;
			this.txtOptions.Size = new System.Drawing.Size(390, 47);
			this.txtOptions.TabIndex = 6;
			this.txtOptions.ButtonClicked += new System.EventHandler(this.txtOptions_ButtonClicked);
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Label = "Filename:";
			this.txtFilename.Location = new System.Drawing.Point(7, 110);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.ShowEllipseButton = true;
			this.txtFilename.Size = new System.Drawing.Size(389, 47);
			this.txtFilename.TabIndex = 5;
			this.txtFilename.ButtonClicked += new System.EventHandler(this.txtFilename_ButtonClicked);
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.Label = "";
			this.txtConnectionString.Location = new System.Drawing.Point(120, 57);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.ShowEllipseButton = true;
			this.txtConnectionString.Size = new System.Drawing.Size(277, 47);
			this.txtConnectionString.TabIndex = 4;
			this.txtConnectionString.ButtonClicked += new System.EventHandler(this.txtConnectionString_ButtonClicked);
			// 
			// ConnectionStringForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblFieldname);
			this.Controls.Add(this.txtOptions);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.cboSourceType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboDatabaseProvider);
			this.Controls.Add(this.txtConnectionString);
			this.Name = "ConnectionStringForm";
			this.Size = new System.Drawing.Size(400, 225);
			this.Load += new System.EventHandler(this.ConnectionStringUC_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboDatabaseProvider;
		private System.Windows.Forms.Label lblFieldname;
		private System.Windows.Forms.ComboBox cboSourceType;
		private System.Windows.Forms.Label label1;
		private TextBoxForm txtOptions;
		private TextBoxForm txtFilename;
		private TextBoxForm txtConnectionString;
	}
}
