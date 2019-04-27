namespace DelimiterFileImporter
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
			crudwork.Models.DataAccess.DataConnectionInfo dataConnectionInfo2 = new crudwork.Models.DataAccess.DataConnectionInfo();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnImport = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.csfSaveAs = new crudwork.Controls.FormControls.ConnectionStringForm();
			this.txtFilename = new crudwork.Controls.FormControls.TextBoxForm();
			this.groupBox1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.csfSaveAs);
			this.groupBox1.Location = new System.Drawing.Point(13, 67);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(567, 252);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Save As...";
			// 
			// btnImport
			// 
			this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImport.Location = new System.Drawing.Point(505, 325);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 3;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 351);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(592, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			// 
			// csfSaveAs
			// 
			dataConnectionInfo2.ConnectionString = "";
			dataConnectionInfo2.Filename = null;
			dataConnectionInfo2.InputSource = crudwork.Models.DataAccess.InputSource.Database;
			dataConnectionInfo2.Options = null;
			dataConnectionInfo2.Provider = crudwork.Models.DataAccess.DatabaseProvider.SqlClient;
			this.csfSaveAs.DataConnectionInfo = dataConnectionInfo2;
			this.csfSaveAs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.csfSaveAs.Fieldname = "Connection String:";
			this.csfSaveAs.Location = new System.Drawing.Point(3, 16);
			this.csfSaveAs.Name = "csfSaveAs";
			this.csfSaveAs.Size = new System.Drawing.Size(561, 233);
			this.csfSaveAs.TabIndex = 1;
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Label = "Filename:";
			this.txtFilename.Location = new System.Drawing.Point(12, 12);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(568, 48);
			this.txtFilename.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.statusStrip1);
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "Form1";
			this.Text = "Delimiter File Importer";
			this.groupBox1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private crudwork.Controls.FormControls.TextBoxForm txtFilename;
		private crudwork.Controls.FormControls.ConnectionStringForm csfSaveAs;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
	}
}

