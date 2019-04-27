namespace crudwork.Controls.TaskScheduler
{
	partial class WeeklyScheduleFreqOccur
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
			this.cboFromMonth = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboToMonth = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cboFromMonth
			// 
			this.cboFromMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFromMonth.FormattingEnabled = true;
			this.cboFromMonth.Location = new System.Drawing.Point(68, 3);
			this.cboFromMonth.Name = "cboFromMonth";
			this.cboFromMonth.Size = new System.Drawing.Size(121, 21);
			this.cboFromMonth.TabIndex = 0;
			this.cboFromMonth.SelectedIndexChanged += new System.EventHandler(this.cboFromMonth_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "From:";
			// 
			// cboToMonth
			// 
			this.cboToMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToMonth.FormattingEnabled = true;
			this.cboToMonth.Location = new System.Drawing.Point(68, 30);
			this.cboToMonth.Name = "cboToMonth";
			this.cboToMonth.Size = new System.Drawing.Size(121, 21);
			this.cboToMonth.TabIndex = 0;
			this.cboToMonth.SelectedIndexChanged += new System.EventHandler(this.cboToDate_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "To:";
			// 
			// WeeklyScheduleFreqOccur
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboToMonth);
			this.Controls.Add(this.cboFromMonth);
			this.Name = "WeeklyScheduleFreqOccur";
			this.Size = new System.Drawing.Size(300, 300);
			this.Load += new System.EventHandler(this.WeeklyScheduleFreqOccur_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboFromMonth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboToMonth;
		private System.Windows.Forms.Label label2;
	}
}
