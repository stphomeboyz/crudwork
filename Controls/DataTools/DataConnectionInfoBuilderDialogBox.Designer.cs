namespace crudwork.Controls.DataTools
{
	partial class DataConnectionInfoBuilderDialogBox
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
			crudwork.Models.DataAccess.DataConnectionInfo dataConnectionInfo1 = new crudwork.Models.DataAccess.DataConnectionInfo();
			this.connectionStringForm1 = new crudwork.Controls.FormControls.ConnectionStringForm();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// connectionStringForm1
			// 
			this.connectionStringForm1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			dataConnectionInfo1.ConnectionString = "";
			dataConnectionInfo1.Filename = null;
			dataConnectionInfo1.InputSource = crudwork.Models.DataAccess.InputSource.Database;
			dataConnectionInfo1.Options = null;
			dataConnectionInfo1.Provider = crudwork.Models.DataAccess.DatabaseProvider.SqlClient;
			this.connectionStringForm1.DataConnectionInfo = dataConnectionInfo1;
			this.connectionStringForm1.Fieldname = "Connection String:";
			this.connectionStringForm1.Location = new System.Drawing.Point(12, 12);
			this.connectionStringForm1.Name = "connectionStringForm1";
			this.connectionStringForm1.Size = new System.Drawing.Size(368, 220);
			this.connectionStringForm1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(224, 238);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(305, 238);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// DataConnectionInfoBuilderDialogBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 273);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.connectionStringForm1);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "DataConnectionInfoBuilderDialogBox";
			this.Text = "Data Connection Info Builder";
			this.ResumeLayout(false);

		}

		#endregion

		private crudwork.Controls.FormControls.ConnectionStringForm connectionStringForm1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}