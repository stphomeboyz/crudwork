namespace crudwork.Controls
{
	partial class DataTableAnalysisTool
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
			this.dgInput = new System.Windows.Forms.DataGridView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabDataTable = new System.Windows.Forms.TabPage();
			this.tabTextResult = new System.Windows.Forms.TabPage();
			this.txtResults = new System.Windows.Forms.TextBox();
			this.tabGridResult = new System.Windows.Forms.TabPage();
			this.dgResults = new System.Windows.Forms.DataGridView();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnGroupBy = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnAutoResizeColumns = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgInput)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabDataTable.SuspendLayout();
			this.tabTextResult.SuspendLayout();
			this.tabGridResult.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgInput
			// 
			this.dgInput.AllowUserToAddRows = false;
			this.dgInput.AllowUserToDeleteRows = false;
			this.dgInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgInput.Location = new System.Drawing.Point(3, 3);
			this.dgInput.Name = "dgInput";
			this.dgInput.ReadOnly = true;
			this.dgInput.Size = new System.Drawing.Size(554, 204);
			this.dgInput.StandardTab = true;
			this.dgInput.TabIndex = 0;
			this.dgInput.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(12, 12);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(568, 320);
			this.splitContainer1.SplitterDistance = 236;
			this.splitContainer1.TabIndex = 1;
			this.splitContainer1.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabDataTable);
			this.tabControl1.Controls.Add(this.tabTextResult);
			this.tabControl1.Controls.Add(this.tabGridResult);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(568, 236);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl1.TabIndex = 1;
			// 
			// tabDataTable
			// 
			this.tabDataTable.Controls.Add(this.dgInput);
			this.tabDataTable.Location = new System.Drawing.Point(4, 22);
			this.tabDataTable.Name = "tabDataTable";
			this.tabDataTable.Padding = new System.Windows.Forms.Padding(3);
			this.tabDataTable.Size = new System.Drawing.Size(560, 210);
			this.tabDataTable.TabIndex = 0;
			this.tabDataTable.Text = "Data Table";
			this.tabDataTable.UseVisualStyleBackColor = true;
			// 
			// tabTextResult
			// 
			this.tabTextResult.Controls.Add(this.txtResults);
			this.tabTextResult.Location = new System.Drawing.Point(4, 22);
			this.tabTextResult.Name = "tabTextResult";
			this.tabTextResult.Padding = new System.Windows.Forms.Padding(3);
			this.tabTextResult.Size = new System.Drawing.Size(560, 210);
			this.tabTextResult.TabIndex = 1;
			this.tabTextResult.Text = "Text Result";
			this.tabTextResult.UseVisualStyleBackColor = true;
			// 
			// txtResults
			// 
			this.txtResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResults.Location = new System.Drawing.Point(3, 3);
			this.txtResults.Multiline = true;
			this.txtResults.Name = "txtResults";
			this.txtResults.ReadOnly = true;
			this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtResults.Size = new System.Drawing.Size(554, 204);
			this.txtResults.TabIndex = 0;
			this.txtResults.WordWrap = false;
			// 
			// tabGridResult
			// 
			this.tabGridResult.Controls.Add(this.dgResults);
			this.tabGridResult.Location = new System.Drawing.Point(4, 22);
			this.tabGridResult.Name = "tabGridResult";
			this.tabGridResult.Padding = new System.Windows.Forms.Padding(3);
			this.tabGridResult.Size = new System.Drawing.Size(560, 210);
			this.tabGridResult.TabIndex = 2;
			this.tabGridResult.Text = "Grid Result";
			this.tabGridResult.UseVisualStyleBackColor = true;
			// 
			// dgResults
			// 
			this.dgResults.AllowUserToAddRows = false;
			this.dgResults.AllowUserToDeleteRows = false;
			this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgResults.Location = new System.Drawing.Point(3, 3);
			this.dgResults.Name = "dgResults";
			this.dgResults.ReadOnly = true;
			this.dgResults.Size = new System.Drawing.Size(554, 204);
			this.dgResults.StandardTab = true;
			this.dgResults.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnGroupBy);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(568, 80);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// btnGroupBy
			// 
			this.btnGroupBy.Location = new System.Drawing.Point(3, 3);
			this.btnGroupBy.Name = "btnGroupBy";
			this.btnGroupBy.Size = new System.Drawing.Size(75, 23);
			this.btnGroupBy.TabIndex = 0;
			this.btnGroupBy.Text = "Group By...";
			this.btnGroupBy.UseVisualStyleBackColor = true;
			this.btnGroupBy.Click += new System.EventHandler(this.btnGroupBy_Click);
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(504, 338);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 1;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnAutoResizeColumns
			// 
			this.btnAutoResizeColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAutoResizeColumns.Location = new System.Drawing.Point(12, 338);
			this.btnAutoResizeColumns.Name = "btnAutoResizeColumns";
			this.btnAutoResizeColumns.Size = new System.Drawing.Size(150, 23);
			this.btnAutoResizeColumns.TabIndex = 0;
			this.btnAutoResizeColumns.Text = "Auto Resize Column";
			this.btnAutoResizeColumns.UseVisualStyleBackColor = true;
			this.btnAutoResizeColumns.Click += new System.EventHandler(this.btnAutoResizeColumns_Click);
			// 
			// DataTableAnalysisTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.btnAutoResizeColumns);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.splitContainer1);
			this.KeyPreview = true;
			this.Name = "DataTableAnalysisTool";
			this.Text = "DataTable Analysis Tool";
			this.Load += new System.EventHandler(this.DataTableAnalysisTool_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataTableAnalysisTool_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.dgInput)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabDataTable.ResumeLayout(false);
			this.tabTextResult.ResumeLayout(false);
			this.tabTextResult.PerformLayout();
			this.tabGridResult.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgInput;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnGroupBy;
		private System.Windows.Forms.Button btnAutoResizeColumns;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDataTable;
		private System.Windows.Forms.TabPage tabTextResult;
		private System.Windows.Forms.TextBox txtResults;
		private System.Windows.Forms.TabPage tabGridResult;
		private System.Windows.Forms.DataGridView dgResults;
	}
}