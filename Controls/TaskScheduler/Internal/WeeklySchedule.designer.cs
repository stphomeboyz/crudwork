namespace crudwork.Controls.TaskScheduler
{
	partial class WeeklySchedule
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
			this.label2 = new System.Windows.Forms.Label();
			this.numFrequencies = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.chkMonday = new System.Windows.Forms.CheckBox();
			this.chkTuesday = new System.Windows.Forms.CheckBox();
			this.chkWednesday = new System.Windows.Forms.CheckBox();
			this.chkThursday = new System.Windows.Forms.CheckBox();
			this.chkFriday = new System.Windows.Forms.CheckBox();
			this.chkSaturday = new System.Windows.Forms.CheckBox();
			this.chkSunday = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.wsfo1 = new crudwork.Controls.TaskScheduler.WeeklyScheduleFreqOccur();
			((System.ComponentModel.ISupportInitialize)(this.numFrequencies)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(114, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "week(s) on:";
			// 
			// numFrequencies
			// 
			this.numFrequencies.Location = new System.Drawing.Point(46, 16);
			this.numFrequencies.Maximum = new decimal(new int[] {
            52,
            0,
            0,
            0});
			this.numFrequencies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFrequencies.Name = "numFrequencies";
			this.numFrequencies.Size = new System.Drawing.Size(61, 20);
			this.numFrequencies.TabIndex = 1;
			this.numFrequencies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFrequencies.ValueChanged += new System.EventHandler(this.numFrequencies_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Every";
			// 
			// chkMonday
			// 
			this.chkMonday.AutoSize = true;
			this.chkMonday.Location = new System.Drawing.Point(9, 49);
			this.chkMonday.Name = "chkMonday";
			this.chkMonday.Size = new System.Drawing.Size(64, 17);
			this.chkMonday.TabIndex = 3;
			this.chkMonday.Text = "Monday";
			this.chkMonday.UseVisualStyleBackColor = true;
			// 
			// chkTuesday
			// 
			this.chkTuesday.AutoSize = true;
			this.chkTuesday.Location = new System.Drawing.Point(9, 72);
			this.chkTuesday.Name = "chkTuesday";
			this.chkTuesday.Size = new System.Drawing.Size(67, 17);
			this.chkTuesday.TabIndex = 4;
			this.chkTuesday.Text = "Tuesday";
			this.chkTuesday.UseVisualStyleBackColor = true;
			// 
			// chkWednesday
			// 
			this.chkWednesday.AutoSize = true;
			this.chkWednesday.Location = new System.Drawing.Point(9, 95);
			this.chkWednesday.Name = "chkWednesday";
			this.chkWednesday.Size = new System.Drawing.Size(83, 17);
			this.chkWednesday.TabIndex = 5;
			this.chkWednesday.Text = "Wednesday";
			this.chkWednesday.UseVisualStyleBackColor = true;
			// 
			// chkThursday
			// 
			this.chkThursday.AutoSize = true;
			this.chkThursday.Location = new System.Drawing.Point(9, 118);
			this.chkThursday.Name = "chkThursday";
			this.chkThursday.Size = new System.Drawing.Size(70, 17);
			this.chkThursday.TabIndex = 6;
			this.chkThursday.Text = "Thursday";
			this.chkThursday.UseVisualStyleBackColor = true;
			// 
			// chkFriday
			// 
			this.chkFriday.AutoSize = true;
			this.chkFriday.Location = new System.Drawing.Point(96, 49);
			this.chkFriday.Name = "chkFriday";
			this.chkFriday.Size = new System.Drawing.Size(54, 17);
			this.chkFriday.TabIndex = 7;
			this.chkFriday.Text = "Friday";
			this.chkFriday.UseVisualStyleBackColor = true;
			// 
			// chkSaturday
			// 
			this.chkSaturday.AutoSize = true;
			this.chkSaturday.Location = new System.Drawing.Point(96, 72);
			this.chkSaturday.Name = "chkSaturday";
			this.chkSaturday.Size = new System.Drawing.Size(68, 17);
			this.chkSaturday.TabIndex = 8;
			this.chkSaturday.Text = "Saturday";
			this.chkSaturday.UseVisualStyleBackColor = true;
			// 
			// chkSunday
			// 
			this.chkSunday.AutoSize = true;
			this.chkSunday.Location = new System.Drawing.Point(96, 95);
			this.chkSunday.Name = "chkSunday";
			this.chkSunday.Size = new System.Drawing.Size(62, 17);
			this.chkSunday.TabIndex = 9;
			this.chkSunday.Text = "Sunday";
			this.chkSunday.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.wsfo1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chkSunday);
			this.groupBox1.Controls.Add(this.numFrequencies);
			this.groupBox1.Controls.Add(this.chkSaturday);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.chkFriday);
			this.groupBox1.Controls.Add(this.chkMonday);
			this.groupBox1.Controls.Add(this.chkThursday);
			this.groupBox1.Controls.Add(this.chkTuesday);
			this.groupBox1.Controls.Add(this.chkWednesday);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(300, 300);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Weekly Schedule";
			// 
			// wsfo1
			// 
			this.wsfo1.AutoSize = true;
			this.wsfo1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.wsfo1.FromDate = "1/2006";
			this.wsfo1.Location = new System.Drawing.Point(6, 151);
			this.wsfo1.Name = "wsfo1";
			this.wsfo1.Size = new System.Drawing.Size(192, 54);
			this.wsfo1.TabIndex = 10;
			this.wsfo1.ToDate = "1/2006";
			// 
			// WeeklySchedule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.groupBox1);
			this.Name = "WeeklySchedule";
			this.Size = new System.Drawing.Size(300, 300);
			((System.ComponentModel.ISupportInitialize)(this.numFrequencies)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numFrequencies;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkMonday;
		private System.Windows.Forms.CheckBox chkTuesday;
		private System.Windows.Forms.CheckBox chkWednesday;
		private System.Windows.Forms.CheckBox chkThursday;
		private System.Windows.Forms.CheckBox chkFriday;
		private System.Windows.Forms.CheckBox chkSaturday;
		private System.Windows.Forms.CheckBox chkSunday;
		private System.Windows.Forms.GroupBox groupBox1;
		private WeeklyScheduleFreqOccur wsfo1;
	}
}
