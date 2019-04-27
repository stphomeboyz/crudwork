namespace crudwork.Controls.TaskScheduler
{
	partial class DailySchedule
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
			this.label1 = new System.Windows.Forms.Label();
			this.numFrequencies = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.dsfo1 = new crudwork.Controls.TaskScheduler.DailyScheduleFreqOccur();
			((System.ComponentModel.ISupportInitialize)(this.numFrequencies)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
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
			// numFrequencies
			// 
			this.numFrequencies.Location = new System.Drawing.Point(46, 12);
			this.numFrequencies.Maximum = new decimal(new int[] {
            365,
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(114, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "day(s)";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.dsfo1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numFrequencies);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(300, 300);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Daily Schedule";
			// 
			// dsfo1
			// 
			this.dsfo1.AutoSize = true;
			this.dsfo1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.dsfo1.Frequencies = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.dsfo1.Location = new System.Drawing.Point(6, 50);
			this.dsfo1.Name = "dsfo1";
			this.dsfo1.Size = new System.Drawing.Size(277, 59);
			this.dsfo1.TabIndex = 3;
			// 
			// DailySchedule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.groupBox1);
			this.Name = "DailySchedule";
			this.Size = new System.Drawing.Size(300, 300);
			this.Load += new System.EventHandler(this.DailySchedule_Load);
			((System.ComponentModel.ISupportInitialize)(this.numFrequencies)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numFrequencies;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private DailyScheduleFreqOccur dsfo1;
	}
}
