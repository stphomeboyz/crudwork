namespace crudwork.MultiThreading
{
	partial class MultiThreadingStatusDialog
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
			this.btnShowDetail = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.pbOverall = new System.Windows.Forms.ProgressBar();
			this.txtDetails = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabLogDetail = new System.Windows.Forms.TabPage();
			this.tabError = new System.Windows.Forms.TabPage();
			this.txtErrors = new System.Windows.Forms.TextBox();
			this.chkCloseOnCompletion = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabLogDetail.SuspendLayout();
			this.tabError.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnShowDetail
			// 
			this.btnShowDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnShowDetail.Location = new System.Drawing.Point(12, 238);
			this.btnShowDetail.Name = "btnShowDetail";
			this.btnShowDetail.Size = new System.Drawing.Size(75, 23);
			this.btnShowDetail.TabIndex = 3;
			this.btnShowDetail.Text = "Less...";
			this.btnShowDetail.UseVisualStyleBackColor = true;
			this.btnShowDetail.Click += new System.EventHandler(this.btnShowDetail_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(12, 9);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(568, 68);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Please wait while performing task...";
			// 
			// pbOverall
			// 
			this.pbOverall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbOverall.Location = new System.Drawing.Point(12, 80);
			this.pbOverall.Name = "pbOverall";
			this.pbOverall.Size = new System.Drawing.Size(568, 23);
			this.pbOverall.Step = 1;
			this.pbOverall.TabIndex = 1;
			// 
			// txtDetails
			// 
			this.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDetails.Location = new System.Drawing.Point(3, 3);
			this.txtDetails.Multiline = true;
			this.txtDetails.Name = "txtDetails";
			this.txtDetails.ReadOnly = true;
			this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDetails.Size = new System.Drawing.Size(554, 91);
			this.txtDetails.TabIndex = 0;
			this.txtDetails.WordWrap = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(424, 238);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(505, 238);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 6;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabLogDetail);
			this.tabControl1.Controls.Add(this.tabError);
			this.tabControl1.Location = new System.Drawing.Point(12, 109);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(568, 123);
			this.tabControl1.TabIndex = 2;
			// 
			// tabLogDetail
			// 
			this.tabLogDetail.Controls.Add(this.txtDetails);
			this.tabLogDetail.Location = new System.Drawing.Point(4, 22);
			this.tabLogDetail.Name = "tabLogDetail";
			this.tabLogDetail.Padding = new System.Windows.Forms.Padding(3);
			this.tabLogDetail.Size = new System.Drawing.Size(560, 97);
			this.tabLogDetail.TabIndex = 0;
			this.tabLogDetail.Text = "Log Detail";
			this.tabLogDetail.UseVisualStyleBackColor = true;
			// 
			// tabError
			// 
			this.tabError.Controls.Add(this.txtErrors);
			this.tabError.Location = new System.Drawing.Point(4, 22);
			this.tabError.Name = "tabError";
			this.tabError.Padding = new System.Windows.Forms.Padding(3);
			this.tabError.Size = new System.Drawing.Size(560, 97);
			this.tabError.TabIndex = 1;
			this.tabError.Text = "Error";
			this.tabError.UseVisualStyleBackColor = true;
			// 
			// txtErrors
			// 
			this.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtErrors.Location = new System.Drawing.Point(3, 3);
			this.txtErrors.Multiline = true;
			this.txtErrors.Name = "txtErrors";
			this.txtErrors.ReadOnly = true;
			this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtErrors.Size = new System.Drawing.Size(554, 91);
			this.txtErrors.TabIndex = 3;
			this.txtErrors.WordWrap = false;
			// 
			// chkCloseOnCompletion
			// 
			this.chkCloseOnCompletion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkCloseOnCompletion.AutoSize = true;
			this.chkCloseOnCompletion.Location = new System.Drawing.Point(93, 242);
			this.chkCloseOnCompletion.Name = "chkCloseOnCompletion";
			this.chkCloseOnCompletion.Size = new System.Drawing.Size(174, 17);
			this.chkCloseOnCompletion.TabIndex = 4;
			this.chkCloseOnCompletion.Text = "Close on successful completion";
			this.chkCloseOnCompletion.UseVisualStyleBackColor = true;
			this.chkCloseOnCompletion.Click += new System.EventHandler(this.chkCloseOnCompletion_Click);
			// 
			// MultiThreadingStatusDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 273);
			this.Controls.Add(this.chkCloseOnCompletion);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.pbOverall);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnShowDetail);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(450, 170);
			this.Name = "MultiThreadingStatusDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Please Wait ...";
			this.Load += new System.EventHandler(this.MultiThreadingStatusDialog_Load);
			this.Resize += new System.EventHandler(this.MultiThreadingStatusDialog_Resize);
			this.tabControl1.ResumeLayout(false);
			this.tabLogDetail.ResumeLayout(false);
			this.tabLogDetail.PerformLayout();
			this.tabError.ResumeLayout(false);
			this.tabError.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnShowDetail;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.ProgressBar pbOverall;
		private System.Windows.Forms.TextBox txtDetails;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabLogDetail;
		private System.Windows.Forms.TabPage tabError;
		private System.Windows.Forms.TextBox txtErrors;
		private System.Windows.Forms.CheckBox chkCloseOnCompletion;
	}
}