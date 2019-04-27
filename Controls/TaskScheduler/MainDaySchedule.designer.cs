namespace crudwork.Controls.TaskScheduler
{
	partial class MainDaySchedule
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
			this.cboScheduleType = new System.Windows.Forms.ComboBox();
			this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
			this.dailySchedule1 = new crudwork.Controls.TaskScheduler.DailySchedule();
			this.weeklySchedule1 = new crudwork.Controls.TaskScheduler.WeeklySchedule();
			this.monthlySchedule1 = new crudwork.Controls.TaskScheduler.MonthlySchedule();
			this.onceSchedule1 = new crudwork.Controls.TaskScheduler.OnceSchedule();
			this.holidaySchedule1 = new crudwork.Controls.TaskScheduler.HolidaySchedule();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.flpMain.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cboScheduleType
			// 
			this.cboScheduleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboScheduleType.FormattingEnabled = true;
			this.cboScheduleType.Items.AddRange(new object[] {
            "Daily",
            "Weekly",
            "Monthly",
            "Once",
            "Holidays",
            "Custom"});
			this.cboScheduleType.Location = new System.Drawing.Point(6, 19);
			this.cboScheduleType.Name = "cboScheduleType";
			this.cboScheduleType.Size = new System.Drawing.Size(121, 21);
			this.cboScheduleType.TabIndex = 1;
			this.cboScheduleType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// flpMain
			// 
			this.flpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flpMain.Controls.Add(this.dailySchedule1);
			this.flpMain.Controls.Add(this.weeklySchedule1);
			this.flpMain.Controls.Add(this.monthlySchedule1);
			this.flpMain.Controls.Add(this.onceSchedule1);
			this.flpMain.Controls.Add(this.holidaySchedule1);
			this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpMain.Location = new System.Drawing.Point(6, 46);
			this.flpMain.Name = "flpMain";
			this.flpMain.Size = new System.Drawing.Size(422, 309);
			this.flpMain.TabIndex = 2;
			// 
			// dailySchedule1
			// 
			this.dailySchedule1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.dailySchedule1.Frequencies = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.dailySchedule1.Location = new System.Drawing.Point(3, 3);
			this.dailySchedule1.Name = "dailySchedule1";
			this.dailySchedule1.Size = new System.Drawing.Size(100, 100);
			this.dailySchedule1.TabIndex = 0;
			// 
			// weeklySchedule1
			// 
			this.weeklySchedule1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.weeklySchedule1.Frequencies = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.weeklySchedule1.Location = new System.Drawing.Point(3, 109);
			this.weeklySchedule1.Name = "weeklySchedule1";
			this.weeklySchedule1.Size = new System.Drawing.Size(100, 100);
			this.weeklySchedule1.TabIndex = 1;
			// 
			// monthlySchedule1
			// 
			this.monthlySchedule1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.monthlySchedule1.Location = new System.Drawing.Point(109, 3);
			this.monthlySchedule1.Name = "monthlySchedule1";
			this.monthlySchedule1.Size = new System.Drawing.Size(100, 100);
			this.monthlySchedule1.TabIndex = 2;
			// 
			// onceSchedule1
			// 
			this.onceSchedule1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.onceSchedule1.Location = new System.Drawing.Point(109, 109);
			this.onceSchedule1.Name = "onceSchedule1";
			this.onceSchedule1.Size = new System.Drawing.Size(100, 100);
			this.onceSchedule1.TabIndex = 3;
			// 
			// holidaySchedule1
			// 
			this.holidaySchedule1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.holidaySchedule1.Location = new System.Drawing.Point(215, 3);
			this.holidaySchedule1.Name = "holidaySchedule1";
			this.holidaySchedule1.Size = new System.Drawing.Size(100, 100);
			this.holidaySchedule1.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cboScheduleType);
			this.groupBox1.Controls.Add(this.flpMain);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(436, 378);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Schedule Date";
			// 
			// MainDaySchedule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "MainDaySchedule";
			this.Size = new System.Drawing.Size(440, 382);
			this.Load += new System.EventHandler(this.MainDaySchedule_Load);
			this.flpMain.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cboScheduleType;
		private System.Windows.Forms.FlowLayoutPanel flpMain;
		private crudwork.Controls.TaskScheduler.DailySchedule dailySchedule1;
		private crudwork.Controls.TaskScheduler.OnceSchedule onceSchedule1;
		private crudwork.Controls.TaskScheduler.WeeklySchedule weeklySchedule1;
		private crudwork.Controls.TaskScheduler.MonthlySchedule monthlySchedule1;
		private crudwork.Controls.TaskScheduler.HolidaySchedule holidaySchedule1;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
