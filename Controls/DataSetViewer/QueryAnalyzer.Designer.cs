namespace crudwork.Controls.DatabaseUC
{
	partial class QueryAnalyzer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryAnalyzer));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtQuery = new System.Windows.Forms.RichTextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabResults = new System.Windows.Forms.TabPage();
			this.flpResults = new crudwork.Controls.ExtendedFlowLayoutPanel();
			this.tabDataSet = new System.Windows.Forms.TabPage();
			this.simpleDataSetViewer1 = new crudwork.Controls.SimpleDataSetViewer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbNewFile = new System.Windows.Forms.ToolStripButton();
			this.tsbOpenFile = new System.Windows.Forms.ToolStripButton();
			this.tsbSaveFile = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbRun = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbToggleResults = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbHelp = new System.Windows.Forms.ToolStripButton();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabResults.SuspendLayout();
			this.tabDataSet.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtQuery);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Size = new System.Drawing.Size(600, 375);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.TabIndex = 0;
			this.splitContainer1.TabStop = false;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// txtQuery
			// 
			this.txtQuery.AcceptsTab = true;
			this.txtQuery.AutoWordSelection = true;
			this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtQuery.EnableAutoDragDrop = true;
			this.txtQuery.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQuery.HideSelection = false;
			this.txtQuery.Location = new System.Drawing.Point(0, 0);
			this.txtQuery.Name = "txtQuery";
			this.txtQuery.ShowSelectionMargin = true;
			this.txtQuery.Size = new System.Drawing.Size(600, 200);
			this.txtQuery.TabIndex = 1;
			this.txtQuery.Text = "";
			this.txtQuery.WordWrap = false;
			this.txtQuery.TextChanged += new System.EventHandler(this.txtQuery_TextChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabResults);
			this.tabControl1.Controls.Add(this.tabDataSet);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(600, 171);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.TabStop = false;
			// 
			// tabResults
			// 
			this.tabResults.Controls.Add(this.flpResults);
			this.tabResults.Location = new System.Drawing.Point(4, 22);
			this.tabResults.Name = "tabResults";
			this.tabResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabResults.Size = new System.Drawing.Size(592, 145);
			this.tabResults.TabIndex = 0;
			this.tabResults.Text = "Results";
			this.tabResults.UseVisualStyleBackColor = true;
			// 
			// flpResults
			// 
			this.flpResults.AutoScroll = true;
			this.flpResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flpResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpResults.Location = new System.Drawing.Point(3, 3);
			this.flpResults.Name = "flpResults";
			this.flpResults.Size = new System.Drawing.Size(586, 139);
			this.flpResults.TabIndex = 0;
			this.flpResults.SizeChanged += new System.EventHandler(this.flpResults_SizeChanged);
			// 
			// tabDataSet
			// 
			this.tabDataSet.Controls.Add(this.simpleDataSetViewer1);
			this.tabDataSet.Location = new System.Drawing.Point(4, 22);
			this.tabDataSet.Name = "tabDataSet";
			this.tabDataSet.Padding = new System.Windows.Forms.Padding(3);
			this.tabDataSet.Size = new System.Drawing.Size(592, 145);
			this.tabDataSet.TabIndex = 1;
			this.tabDataSet.Text = "DataSet";
			this.tabDataSet.UseVisualStyleBackColor = true;
			// 
			// simpleDataSetViewer1
			// 
			this.simpleDataSetViewer1.ColumnName = "";
			this.simpleDataSetViewer1.DataSource = null;
			this.simpleDataSetViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleDataSetViewer1.HideTablePanel = false;
			this.simpleDataSetViewer1.Location = new System.Drawing.Point(3, 3);
			this.simpleDataSetViewer1.MinimumSize = new System.Drawing.Size(250, 100);
			this.simpleDataSetViewer1.Name = "simpleDataSetViewer1";
			this.simpleDataSetViewer1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.simpleDataSetViewer1.Size = new System.Drawing.Size(586, 139);
			this.simpleDataSetViewer1.TabIndex = 0;
			this.simpleDataSetViewer1.TableName = "";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewFile,
            this.tsbOpenFile,
            this.tsbSaveFile,
            this.toolStripSeparator1,
            this.tsbRun,
            this.toolStripSeparator2,
            this.tsbToggleResults,
            this.toolStripSeparator4,
            this.tsbHelp});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(600, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbNewFile
			// 
			this.tsbNewFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewFile.Image")));
			this.tsbNewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbNewFile.Name = "tsbNewFile";
			this.tsbNewFile.Size = new System.Drawing.Size(48, 22);
			this.tsbNewFile.Text = "New";
			this.tsbNewFile.Click += new System.EventHandler(this.tsbNewFile_Click);
			// 
			// tsbOpenFile
			// 
			this.tsbOpenFile.Image = global::crudwork.Controls.Properties.Resources.openHS;
			this.tsbOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOpenFile.Name = "tsbOpenFile";
			this.tsbOpenFile.Size = new System.Drawing.Size(53, 22);
			this.tsbOpenFile.Text = "Open";
			this.tsbOpenFile.Click += new System.EventHandler(this.tsbOpenFile_Click);
			// 
			// tsbSaveFile
			// 
			this.tsbSaveFile.Image = global::crudwork.Controls.Properties.Resources.saveHS;
			this.tsbSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSaveFile.Name = "tsbSaveFile";
			this.tsbSaveFile.Size = new System.Drawing.Size(51, 22);
			this.tsbSaveFile.Text = "Save";
			this.tsbSaveFile.Click += new System.EventHandler(this.tsbSaveFile_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbRun
			// 
			this.tsbRun.Image = global::crudwork.Controls.Properties.Resources.PlayHS;
			this.tsbRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbRun.Name = "tsbRun";
			this.tsbRun.Size = new System.Drawing.Size(46, 22);
			this.tsbRun.Text = "Run";
			this.tsbRun.Click += new System.EventHandler(this.tsbRun_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbToggleResults
			// 
			this.tsbToggleResults.Image = global::crudwork.Controls.Properties.Resources.Expand_large;
			this.tsbToggleResults.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbToggleResults.Name = "tsbToggleResults";
			this.tsbToggleResults.Size = new System.Drawing.Size(122, 22);
			this.tsbToggleResults.Text = "Show / Hide Results";
			this.tsbToggleResults.Click += new System.EventHandler(this.tsbToggleResults_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbHelp
			// 
			this.tsbHelp.Image = global::crudwork.Controls.Properties.Resources.Help;
			this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbHelp.Name = "tsbHelp";
			this.tsbHelp.Size = new System.Drawing.Size(48, 22);
			this.tsbHelp.Text = "Help";
			this.tsbHelp.Click += new System.EventHandler(this.tsbHelp_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// QueryAnalyzer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "QueryAnalyzer";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.QueryAnalyzer_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabResults.ResumeLayout(false);
			this.tabDataSet.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private ExtendedFlowLayoutPanel flpResults;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbRun;
		private System.Windows.Forms.ToolStripButton tsbOpenFile;
		private System.Windows.Forms.ToolStripButton tsbSaveFile;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsbNewFile;
		private System.Windows.Forms.ToolStripButton tsbToggleResults;
		private System.Windows.Forms.ToolStripButton tsbHelp;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabResults;
		private System.Windows.Forms.TabPage tabDataSet;
		private SimpleDataSetViewer simpleDataSetViewer1;
		private System.Windows.Forms.RichTextBox txtQuery;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
	}
}
