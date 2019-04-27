namespace crudwork.Controls
{
	partial class SQLiteQueryAnalyzer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLiteQueryAnalyzer));
			this.label1 = new System.Windows.Forms.Label();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.btnBuildConnectString = new System.Windows.Forms.Button();
			this.txtQuery = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.dsvResult = new crudwork.Controls.DataSetViewer();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbRunQuery = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbExport = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbListTables = new System.Windows.Forms.ToolStripButton();
			this.tsbGetTableCounts = new System.Windows.Forms.ToolStripButton();
			this.tsbGetSamples = new System.Windows.Forms.ToolStripButton();
			this.tsbImport = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Connection String";
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.Location = new System.Drawing.Point(109, 14);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(386, 20);
			this.txtConnectionString.TabIndex = 1;
			// 
			// btnBuildConnectString
			// 
			this.btnBuildConnectString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBuildConnectString.Location = new System.Drawing.Point(501, 12);
			this.btnBuildConnectString.Name = "btnBuildConnectString";
			this.btnBuildConnectString.Size = new System.Drawing.Size(75, 23);
			this.btnBuildConnectString.TabIndex = 2;
			this.btnBuildConnectString.Text = "Build CS...";
			this.btnBuildConnectString.UseVisualStyleBackColor = true;
			this.btnBuildConnectString.Click += new System.EventHandler(this.btnBuildConnectString_Click);
			// 
			// txtQuery
			// 
			this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtQuery.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQuery.Location = new System.Drawing.Point(6, 40);
			this.txtQuery.Multiline = true;
			this.txtQuery.Name = "txtQuery";
			this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtQuery.Size = new System.Drawing.Size(570, 126);
			this.txtQuery.TabIndex = 3;
			this.txtQuery.Text = "select * from SQLite_master";
			this.txtQuery.WordWrap = false;
			this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtQuery);
			this.splitContainer1.Panel1.Controls.Add(this.btnBuildConnectString);
			this.splitContainer1.Panel1.Controls.Add(this.txtConnectionString);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.dsvResult);
			this.splitContainer1.Size = new System.Drawing.Size(592, 348);
			this.splitContainer1.SplitterDistance = 173;
			this.splitContainer1.TabIndex = 0;
			// 
			// dsvResult
			// 
			this.dsvResult.ColumnName = "";
			this.dsvResult.Cursor = System.Windows.Forms.Cursors.Default;
			this.dsvResult.DataSource = null;
			this.dsvResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dsvResult.HideTablePanel = true;
			this.dsvResult.Location = new System.Drawing.Point(0, 0);
			this.dsvResult.Name = "dsvResult";
			this.dsvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dsvResult.Size = new System.Drawing.Size(588, 167);
			this.dsvResult.TabIndex = 0;
			this.dsvResult.TableName = "";
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(592, 348);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(592, 373);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRunQuery,
            this.toolStripSeparator2,
            this.tsbImport,
            this.tsbExport,
            this.toolStripSeparator1,
            this.tsbListTables,
            this.tsbGetTableCounts,
            this.tsbGetSamples});
			this.toolStrip1.Location = new System.Drawing.Point(3, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(589, 25);
			this.toolStrip1.TabIndex = 0;
			// 
			// tsbRunQuery
			// 
			this.tsbRunQuery.Image = global::crudwork.Controls.Properties.Resources.PlayHS;
			this.tsbRunQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbRunQuery.Name = "tsbRunQuery";
			this.tsbRunQuery.Size = new System.Drawing.Size(79, 22);
			this.tsbRunQuery.Text = "Run Query";
			this.tsbRunQuery.Click += new System.EventHandler(this.tsbRunQuery_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbExport
			// 
			this.tsbExport.Image = global::crudwork.Controls.Properties.Resources.saveHS;
			this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbExport.Name = "tsbExport";
			this.tsbExport.Size = new System.Drawing.Size(74, 22);
			this.tsbExport.Text = "Export ...";
			this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbListTables
			// 
			this.tsbListTables.Image = ((System.Drawing.Image)(resources.GetObject("tsbListTables.Image")));
			this.tsbListTables.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbListTables.Name = "tsbListTables";
			this.tsbListTables.Size = new System.Drawing.Size(77, 22);
			this.tsbListTables.Text = "List Tables";
			this.tsbListTables.Click += new System.EventHandler(this.tsbListTables_Click);
			// 
			// tsbGetTableCounts
			// 
			this.tsbGetTableCounts.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetTableCounts.Image")));
			this.tsbGetTableCounts.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGetTableCounts.Name = "tsbGetTableCounts";
			this.tsbGetTableCounts.Size = new System.Drawing.Size(90, 22);
			this.tsbGetTableCounts.Text = "Table Counts";
			this.tsbGetTableCounts.Click += new System.EventHandler(this.tsbGetTableCounts_Click);
			// 
			// tsbGetSamples
			// 
			this.tsbGetSamples.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetSamples.Image")));
			this.tsbGetSamples.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGetSamples.Name = "tsbGetSamples";
			this.tsbGetSamples.Size = new System.Drawing.Size(144, 22);
			this.tsbGetSamples.Text = "Get Samples (100 Rows)";
			this.tsbGetSamples.Click += new System.EventHandler(this.tsbGetSamples_Click);
			// 
			// tsbImport
			// 
			this.tsbImport.Image = ((System.Drawing.Image)(resources.GetObject("tsbImport.Image")));
			this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbImport.Name = "tsbImport";
			this.tsbImport.Size = new System.Drawing.Size(74, 22);
			this.tsbImport.Text = "Import ...";
			this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
			// 
			// SQLiteQueryAnalyzer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.toolStripContainer1);
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "SQLiteQueryAnalyzer";
			this.Text = "Simple SQLite Query Analyzer";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Button btnBuildConnectString;
		private System.Windows.Forms.TextBox txtQuery;
		private crudwork.Controls.DataSetViewer dsvResult;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbRunQuery;
		private System.Windows.Forms.ToolStripButton tsbListTables;
		private System.Windows.Forms.ToolStripButton tsbGetTableCounts;
		private System.Windows.Forms.ToolStripButton tsbGetSamples;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsbExport;
		private System.Windows.Forms.ToolStripButton tsbImport;
	}
}

