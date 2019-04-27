namespace OpenSchemaGenerator
{
	partial class Form1
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
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.txtParseCSV = new System.Windows.Forms.Button();
			this.dataSetViewer1 = new crudwork.Controls.DataSetViewer();
			this.SuspendLayout();
			// 
			// txtFilename
			// 
			this.txtFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtFilename.Location = new System.Drawing.Point(13, 13);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(486, 20);
			this.txtFilename.TabIndex = 0;
			this.txtFilename.Text = ".\\DataFiles\\OpenSchemaColumns.csv";
			// 
			// txtParseCSV
			// 
			this.txtParseCSV.Location = new System.Drawing.Point(505, 12);
			this.txtParseCSV.Name = "txtParseCSV";
			this.txtParseCSV.Size = new System.Drawing.Size(75, 23);
			this.txtParseCSV.TabIndex = 1;
			this.txtParseCSV.Text = "Parse";
			this.txtParseCSV.UseVisualStyleBackColor = true;
			this.txtParseCSV.Click += new System.EventHandler(this.txtParseCSV_Click);
			// 
			// dataSetViewer1
			// 
			this.dataSetViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataSetViewer1.ColumnName = "";
			this.dataSetViewer1.Cursor = System.Windows.Forms.Cursors.Default;
			this.dataSetViewer1.DataSource = null;
			this.dataSetViewer1.HideTablePanel = false;
			this.dataSetViewer1.Location = new System.Drawing.Point(13, 40);
			this.dataSetViewer1.Name = "dataSetViewer1";
			this.dataSetViewer1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dataSetViewer1.Size = new System.Drawing.Size(567, 321);
			this.dataSetViewer1.TabIndex = 2;
			this.dataSetViewer1.TableName = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.dataSetViewer1);
			this.Controls.Add(this.txtParseCSV);
			this.Controls.Add(this.txtFilename);
			this.Name = "Form1";
			this.Text = "OpenSchema Generator";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button txtParseCSV;
		private crudwork.Controls.DataSetViewer dataSetViewer1;
	}
}

