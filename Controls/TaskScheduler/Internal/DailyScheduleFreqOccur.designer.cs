namespace crudwork.Controls.TaskScheduler
{
	partial class DailyScheduleFreqOccur
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
			this.label5 = new System.Windows.Forms.Label();
			this.numOccurrences = new System.Windows.Forms.NumericUpDown();
			this.rdoOccurrences = new System.Windows.Forms.RadioButton();
			this.rdoDateRange = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.dtpTo = new System.Windows.Forms.DateTimePicker();
			this.dtpFrom = new System.Windows.Forms.DateTimePicker();
			((System.ComponentModel.ISupportInitialize)(this.numOccurrences)).BeginInit();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(92, 5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(89, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "time(s) starting on";
			// 
			// numOccurrences
			// 
			this.numOccurrences.Location = new System.Drawing.Point(46, 1);
			this.numOccurrences.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numOccurrences.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOccurrences.Name = "numOccurrences";
			this.numOccurrences.Size = new System.Drawing.Size(40, 20);
			this.numOccurrences.TabIndex = 11;
			this.numOccurrences.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOccurrences.ValueChanged += new System.EventHandler(this.numOccurrences_ValueChanged);
			// 
			// rdoOccurrences
			// 
			this.rdoOccurrences.AutoSize = true;
			this.rdoOccurrences.Checked = true;
			this.rdoOccurrences.Location = new System.Drawing.Point(3, 3);
			this.rdoOccurrences.Name = "rdoOccurrences";
			this.rdoOccurrences.Size = new System.Drawing.Size(37, 17);
			this.rdoOccurrences.TabIndex = 10;
			this.rdoOccurrences.TabStop = true;
			this.rdoOccurrences.Text = "for";
			this.rdoOccurrences.UseVisualStyleBackColor = true;
			this.rdoOccurrences.Click += new System.EventHandler(this.rdoOccurrences_Click);
			// 
			// rdoDateRange
			// 
			this.rdoDateRange.AutoSize = true;
			this.rdoDateRange.Location = new System.Drawing.Point(3, 38);
			this.rdoDateRange.Name = "rdoDateRange";
			this.rdoDateRange.Size = new System.Drawing.Size(48, 17);
			this.rdoDateRange.TabIndex = 13;
			this.rdoDateRange.Text = "from:";
			this.rdoDateRange.UseVisualStyleBackColor = true;
			this.rdoDateRange.Click += new System.EventHandler(this.rdoDateRange_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(165, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "to";
			// 
			// dtpStart
			// 
			this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpStart.Location = new System.Drawing.Point(187, 1);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.Size = new System.Drawing.Size(87, 20);
			this.dtpStart.TabIndex = 16;
			this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
			// 
			// dtpTo
			// 
			this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpTo.Location = new System.Drawing.Point(187, 36);
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.Size = new System.Drawing.Size(87, 20);
			this.dtpTo.TabIndex = 17;
			this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
			// 
			// dtpFrom
			// 
			this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpFrom.Location = new System.Drawing.Point(57, 36);
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.Size = new System.Drawing.Size(87, 20);
			this.dtpFrom.TabIndex = 14;
			this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
			// 
			// DayScheduleFrequenciesOccurencies
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label5);
			this.Controls.Add(this.numOccurrences);
			this.Controls.Add(this.rdoOccurrences);
			this.Controls.Add(this.rdoDateRange);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.dtpStart);
			this.Controls.Add(this.dtpTo);
			this.Controls.Add(this.dtpFrom);
			this.Name = "DayScheduleFrequenciesOccurencies";
			this.Size = new System.Drawing.Size(300, 300);
			this.Load += new System.EventHandler(this.DayScheduleFrequenciesOccurencies_Load);
			((System.ComponentModel.ISupportInitialize)(this.numOccurrences)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numOccurrences;
		private System.Windows.Forms.RadioButton rdoOccurrences;
		private System.Windows.Forms.RadioButton rdoDateRange;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.DateTimePicker dtpFrom;
	}
}
