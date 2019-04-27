namespace crudwork.Controls.Wizard.DataExportWizardControls
{
	partial class ChooseDestination
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
			crudwork.Models.DataAccess.DataConnectionInfo databaseConnection1 = new crudwork.Models.DataAccess.DataConnectionInfo();
			this.lblDescription = new System.Windows.Forms.Label();
			this.connectionStringForm1 = new crudwork.Controls.FormControls.ConnectionStringForm();
			this.SuspendLayout();
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.Location = new System.Drawing.Point(3, 0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new System.Windows.Forms.Padding(10);
			this.lblDescription.Size = new System.Drawing.Size(594, 69);
			this.lblDescription.TabIndex = 2;
			this.lblDescription.Text = "[Description Goes Here...]";
			// 
			// connectionStringForm1
			// 
			this.connectionStringForm1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			databaseConnection1.ConnectionString = "";
			databaseConnection1.Filename = null;
			databaseConnection1.Options = null;
			databaseConnection1.Provider = crudwork.Models.DataAccess.DatabaseProvider.SqlClient;
			databaseConnection1.InputSource = crudwork.Models.DataAccess.InputSource.Database;
			this.connectionStringForm1.DataConnectionInfo = databaseConnection1;
			this.connectionStringForm1.Fieldname = "Connection String:";
			this.connectionStringForm1.Location = new System.Drawing.Point(3, 72);
			this.connectionStringForm1.Name = "connectionStringForm1";
			this.connectionStringForm1.Size = new System.Drawing.Size(594, 229);
			this.connectionStringForm1.TabIndex = 4;
			// 
			// ChooseDestination
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.connectionStringForm1);
			this.Controls.Add(this.lblDescription);
			this.Name = "ChooseDestination";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.ChooseDestination_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblDescription;
		private crudwork.Controls.FormControls.ConnectionStringForm connectionStringForm1;
	}
}
