namespace crudwork.Controls
{
	partial class ConnectionStringBuilderDialogBox
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.cboDatabaseProvider = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.sqLiteConnectionStringBuilder1 = new crudwork.Controls.DataTools.Internal.SQLiteConnectionStringBuilder();
			this.rdbmsConnectionStringBuilder1 = new crudwork.Controls.RDBMSConnectionStringBuilder();
			this.cboDataProvider = new System.Windows.Forms.ComboBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(305, 238);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnTest
			// 
			this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTest.Location = new System.Drawing.Point(143, 238);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 3;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.Location = new System.Drawing.Point(224, 238);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 4;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
			// 
			// cboDatabaseProvider
			// 
			this.cboDatabaseProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboDatabaseProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabaseProvider.FormattingEnabled = true;
			this.cboDatabaseProvider.Location = new System.Drawing.Point(102, 10);
			this.cboDatabaseProvider.Name = "cboDatabaseProvider";
			this.cboDatabaseProvider.Size = new System.Drawing.Size(278, 21);
			this.cboDatabaseProvider.TabIndex = 1;
			this.cboDatabaseProvider.SelectedIndexChanged += new System.EventHandler(this.cboDataProvider_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Data Provider:";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.sqLiteConnectionStringBuilder1);
			this.panel1.Controls.Add(this.rdbmsConnectionStringBuilder1);
			this.panel1.Location = new System.Drawing.Point(16, 37);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(368, 195);
			this.panel1.TabIndex = 2;
			// 
			// sqLiteConnectionStringBuilder1
			// 
			this.sqLiteConnectionStringBuilder1.ConnectionString = "data source=\"\"; password=\"\"; read only=false; failifmissing=false; compress=false" +
				"";
			this.sqLiteConnectionStringBuilder1.Location = new System.Drawing.Point(49, 65);
			this.sqLiteConnectionStringBuilder1.MaskPassword = false;
			this.sqLiteConnectionStringBuilder1.Name = "sqLiteConnectionStringBuilder1";
			this.sqLiteConnectionStringBuilder1.Size = new System.Drawing.Size(199, 90);
			this.sqLiteConnectionStringBuilder1.TabIndex = 2;
			// 
			// rdbmsConnectionStringBuilder1
			// 
			this.rdbmsConnectionStringBuilder1.ConnectionString = "";
			this.rdbmsConnectionStringBuilder1.Location = new System.Drawing.Point(3, 3);
			this.rdbmsConnectionStringBuilder1.MaskPassword = true;
			this.rdbmsConnectionStringBuilder1.Name = "rdbmsConnectionStringBuilder1";
			this.rdbmsConnectionStringBuilder1.Size = new System.Drawing.Size(199, 98);
			this.rdbmsConnectionStringBuilder1.TabIndex = 1;
			// 
			// cboDataProvider
			// 
			this.cboDataProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDataProvider.FormattingEnabled = true;
			this.cboDataProvider.Location = new System.Drawing.Point(102, 10);
			this.cboDataProvider.Name = "cboDataProvider";
			this.cboDataProvider.Size = new System.Drawing.Size(278, 21);
			this.cboDataProvider.TabIndex = 1;
			this.cboDataProvider.SelectedIndexChanged += new System.EventHandler(this.cboDataProvider_SelectedIndexChanged);
			// 
			// ConnectionStringBuilderDialogBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 273);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboDatabaseProvider);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnOkay);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "ConnectionStringBuilderDialogBox";
			this.Text = "Connection String Builder";
			this.Load += new System.EventHandler(this.ConnectionStringBuilderDialogBox_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.ComboBox cboDatabaseProvider;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private RDBMSConnectionStringBuilder rdbmsConnectionStringBuilder1;
		private crudwork.Controls.DataTools.Internal.SQLiteConnectionStringBuilder sqLiteConnectionStringBuilder1;
		private System.Windows.Forms.ComboBox cboDataProvider;

	}
}