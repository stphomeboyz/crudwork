namespace crudwork.Controls.TaskScheduler
{
	partial class HolidaySchedule
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lstHolidays = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboType = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.lstHolidays);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cboType);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(300, 300);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Holiday Schedule";
			// 
			// lstHolidays
			// 
			this.lstHolidays.FormattingEnabled = true;
			this.lstHolidays.Location = new System.Drawing.Point(6, 50);
			this.lstHolidays.Name = "lstHolidays";
			this.lstHolidays.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstHolidays.Size = new System.Drawing.Size(288, 238);
			this.lstHolidays.TabIndex = 2;
			this.lstHolidays.SelectedIndexChanged += new System.EventHandler(this.lstHolidays_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Choose Holiday";
			// 
			// cboType
			// 
			this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboType.FormattingEnabled = true;
			this.cboType.Items.AddRange(new object[] {
            "Federal Holidays",
            "All Holidays"});
			this.cboType.Location = new System.Drawing.Point(90, 23);
			this.cboType.Name = "cboType";
			this.cboType.Size = new System.Drawing.Size(204, 21);
			this.cboType.TabIndex = 1;
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			// 
			// HolidaySchedule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "HolidaySchedule";
			this.Size = new System.Drawing.Size(300, 300);
			this.Load += new System.EventHandler(this.HolidaySchedule_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstHolidays;
	}
}
