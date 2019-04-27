namespace crudwork.Controls
{
	partial class SimpleDataSetViewerForm
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
			this.dataSetViewer1 = new crudwork.Controls.DataSetViewer();
			this.SuspendLayout();
			// 
			// dataSetViewer1
			// 
			this.dataSetViewer1.ColumnName = "";
			this.dataSetViewer1.DataSource = null;
			this.dataSetViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataSetViewer1.HideTablePanel = false;
			this.dataSetViewer1.Location = new System.Drawing.Point(0, 0);
			this.dataSetViewer1.Name = "dataSetViewer1";
			this.dataSetViewer1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dataSetViewer1.Size = new System.Drawing.Size(592, 373);
			this.dataSetViewer1.TabIndex = 0;
			this.dataSetViewer1.TableName = "";
			// 
			// SimpleDataSetViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.dataSetViewer1);
			this.Name = "SimpleDataSetViewerForm";
			this.Text = "SimpleDataSetViewer Form";
			this.ResumeLayout(false);

		}

		#endregion

		private DataSetViewer dataSetViewer1;
	}
}