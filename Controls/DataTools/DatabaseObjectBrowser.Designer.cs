namespace crudwork.Controls
{
	partial class DatabaseObjectBrowser
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
			this.components = new System.ComponentModel.Container();
			this.dsv = new crudwork.Controls.SimpleDataSetViewer();
			this.SuspendLayout();
			// 
			// dsv
			// 
			this.dsv.ColumnName = "";
			this.dsv.DataSource = null;
			this.dsv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dsv.HideTablePanel = false;
			this.dsv.Location = new System.Drawing.Point(0, 0);
			this.dsv.MinimumSize = new System.Drawing.Size(250, 100);
			this.dsv.Name = "dsv";
			this.dsv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dsv.Size = new System.Drawing.Size(400, 200);
			this.dsv.TabIndex = 0;
			this.dsv.TableName = "";
			// 
			// DatabaseObjectBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dsv);
			this.Name = "DatabaseObjectBrowser";
			this.Size = new System.Drawing.Size(400, 200);
			this.Load += new System.EventHandler(this.DatabaseObjectsBrowser_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private SimpleDataSetViewer dsv;

	}
}
