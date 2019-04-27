namespace crudwork.Controls
{
	partial class FileImportBuilderDialogBox
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
			this.txtFilename = new crudwork.Controls.FormControls.TextBoxForm();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtOptions = new crudwork.Controls.FormControls.TextBoxForm();
			this.SuspendLayout();
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Label = "Filename:";
			this.txtFilename.Location = new System.Drawing.Point(12, 12);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.ShowEllipseButton = true;
			this.txtFilename.Size = new System.Drawing.Size(368, 47);
			this.txtFilename.TabIndex = 0;
			this.txtFilename.ButtonClicked += new System.EventHandler(this.txtFilename_ButtonClicked);
			this.txtFilename.OnChanged += new crudwork.Controls.Base.ChangeEventHandler(this.txtFilename_OnChanged);
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.Location = new System.Drawing.Point(224, 238);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 2;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(305, 238);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// txtOptions
			// 
			this.txtOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOptions.Label = "Options:";
			this.txtOptions.Location = new System.Drawing.Point(12, 65);
			this.txtOptions.Name = "txtOptions";
			this.txtOptions.ShowEllipseButton = true;
			this.txtOptions.Size = new System.Drawing.Size(368, 47);
			this.txtOptions.TabIndex = 1;
			this.txtOptions.ButtonClicked += new System.EventHandler(this.txtOptions_ButtonClicked);
			// 
			// FileImportBuilderDialogBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 273);
			this.Controls.Add(this.txtOptions);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.txtFilename);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "FileImportBuilderDialogBox";
			this.Text = "File Import Builder";
			this.Load += new System.EventHandler(this.FileImportBuilderDialogBox_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private crudwork.Controls.FormControls.TextBoxForm txtFilename;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private crudwork.Controls.FormControls.TextBoxForm txtOptions;

	}
}