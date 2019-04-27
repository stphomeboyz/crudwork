namespace crudwork.Controls
{
	partial class SimpleXMLViewer
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
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.chkMapAsAttribute = new System.Windows.Forms.CheckBox();
			this.chkIncludeNulls = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(3, 16);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(394, 148);
			this.webBrowser1.TabIndex = 3;
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveAs.Location = new System.Drawing.Point(322, 4);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
			this.btnSaveAs.TabIndex = 2;
			this.btnSaveAs.Text = "Save As";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// chkMapAsAttribute
			// 
			this.chkMapAsAttribute.AutoSize = true;
			this.chkMapAsAttribute.Location = new System.Drawing.Point(0, 8);
			this.chkMapAsAttribute.Name = "chkMapAsAttribute";
			this.chkMapAsAttribute.Size = new System.Drawing.Size(151, 17);
			this.chkMapAsAttribute.TabIndex = 0;
			this.chkMapAsAttribute.Text = "Map Columns as Attributes";
			this.chkMapAsAttribute.UseVisualStyleBackColor = true;
			this.chkMapAsAttribute.Click += new System.EventHandler(this.chkMapAsAttribute_Click);
			// 
			// chkIncludeNulls
			// 
			this.chkIncludeNulls.AutoSize = true;
			this.chkIncludeNulls.Enabled = false;
			this.chkIncludeNulls.Location = new System.Drawing.Point(157, 8);
			this.chkIncludeNulls.Name = "chkIncludeNulls";
			this.chkIncludeNulls.Size = new System.Drawing.Size(128, 17);
			this.chkIncludeNulls.TabIndex = 1;
			this.chkIncludeNulls.Text = "Show Empty Columns";
			this.chkIncludeNulls.UseVisualStyleBackColor = true;
			this.chkIncludeNulls.Click += new System.EventHandler(this.chkIncludeNulls_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.webBrowser1);
			this.groupBox1.Location = new System.Drawing.Point(0, 33);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 167);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// SimpleXMLViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chkIncludeNulls);
			this.Controls.Add(this.chkMapAsAttribute);
			this.Controls.Add(this.btnSaveAs);
			this.Name = "SimpleXMLViewer";
			this.Size = new System.Drawing.Size(400, 200);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.CheckBox chkMapAsAttribute;
		private System.Windows.Forms.CheckBox chkIncludeNulls;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
