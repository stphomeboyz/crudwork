namespace crudwork.Controls
{
	partial class ChooseExportTable
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseExportTable));
			crudwork.Models.DataAccess.DataConnectionInfo dataConnectionInfo1 = new crudwork.Models.DataAccess.DataConnectionInfo();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.chooseListBox1 = new crudwork.Controls.ChooseListBox();
			this.connectionStringForm1 = new crudwork.Controls.FormControls.ConnectionStringForm();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTableStem = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(368, 27);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select the table(s) to export, and specify a destination connection string.";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(305, 442);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Location = new System.Drawing.Point(224, 442);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 5;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// chooseListBox1
			// 
			this.chooseListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chooseListBox1.Location = new System.Drawing.Point(12, 39);
			this.chooseListBox1.Name = "chooseListBox1";
			this.chooseListBox1.Options = ((System.Collections.Generic.Dictionary<string, bool>)(resources.GetObject("chooseListBox1.Options")));
			this.chooseListBox1.Size = new System.Drawing.Size(368, 200);
			this.chooseListBox1.TabIndex = 1;
			// 
			// connectionStringForm1
			// 
			this.connectionStringForm1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			dataConnectionInfo1.ConnectionString = "";
			dataConnectionInfo1.Filename = null;
			dataConnectionInfo1.InputSource = crudwork.Models.DataAccess.InputSource.Database;
			dataConnectionInfo1.Options = null;
			dataConnectionInfo1.Provider = crudwork.Models.DataAccess.DatabaseProvider.SqlClient;
			this.connectionStringForm1.DataConnectionInfo = dataConnectionInfo1;
			this.connectionStringForm1.Fieldname = "Destination Connection String:";
			this.connectionStringForm1.Location = new System.Drawing.Point(12, 245);
			this.connectionStringForm1.Name = "connectionStringForm1";
			this.connectionStringForm1.Size = new System.Drawing.Size(368, 165);
			this.connectionStringForm1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 419);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Table Stem:";
			// 
			// txtTableStem
			// 
			this.txtTableStem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTableStem.Location = new System.Drawing.Point(82, 416);
			this.txtTableStem.Name = "txtTableStem";
			this.txtTableStem.Size = new System.Drawing.Size(298, 20);
			this.txtTableStem.TabIndex = 4;
			// 
			// ChooseExportTable
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 473);
			this.Controls.Add(this.txtTableStem);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chooseListBox1);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.connectionStringForm1);
			this.MinimumSize = new System.Drawing.Size(400, 500);
			this.Name = "ChooseExportTable";
			this.Text = "Choose Data Tables to Export ...";
			this.Load += new System.EventHandler(this.SaveDataSetAs_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private crudwork.Controls.FormControls.ConnectionStringForm connectionStringForm1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnExport;
		private ChooseListBox chooseListBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTableStem;
	}
}
