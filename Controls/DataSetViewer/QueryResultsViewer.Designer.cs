namespace crudwork.Controls.DatabaseUC
{
	partial class QueryResultsViewer
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabDataResults = new System.Windows.Forms.TabPage();
			this.dgDataResults = new crudwork.Controls.ExtendedDataGridView();
			this.tabTextResults = new System.Windows.Forms.TabPage();
			this.txtTextResults = new System.Windows.Forms.TextBox();
			this.lblStatement = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabDataResults.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgDataResults)).BeginInit();
			this.tabTextResults.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabDataResults);
			this.tabControl1.Controls.Add(this.tabTextResults);
			this.tabControl1.Location = new System.Drawing.Point(0, 30);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 170);
			this.tabControl1.TabIndex = 0;
			// 
			// tabDataResults
			// 
			this.tabDataResults.Controls.Add(this.dgDataResults);
			this.tabDataResults.Location = new System.Drawing.Point(4, 22);
			this.tabDataResults.Name = "tabDataResults";
			this.tabDataResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabDataResults.Size = new System.Drawing.Size(392, 144);
			this.tabDataResults.TabIndex = 0;
			this.tabDataResults.Text = "Data Grid";
			this.tabDataResults.UseVisualStyleBackColor = true;
			// 
			// dgDataResults
			// 
			this.dgDataResults.AllowUserToAddRows = false;
			this.dgDataResults.AllowUserToDeleteRows = false;
			this.dgDataResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgDataResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgDataResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dgDataResults.Location = new System.Drawing.Point(3, 3);
			this.dgDataResults.Name = "dgDataResults";
			this.dgDataResults.ReadOnly = true;
			this.dgDataResults.Size = new System.Drawing.Size(386, 138);
			this.dgDataResults.StandardTab = true;
			this.dgDataResults.TabIndex = 0;
			// 
			// tabTextResults
			// 
			this.tabTextResults.Controls.Add(this.txtTextResults);
			this.tabTextResults.Location = new System.Drawing.Point(4, 22);
			this.tabTextResults.Name = "tabTextResults";
			this.tabTextResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabTextResults.Size = new System.Drawing.Size(392, 144);
			this.tabTextResults.TabIndex = 1;
			this.tabTextResults.Text = "Text";
			this.tabTextResults.UseVisualStyleBackColor = true;
			// 
			// txtTextResults
			// 
			this.txtTextResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTextResults.Location = new System.Drawing.Point(3, 3);
			this.txtTextResults.Multiline = true;
			this.txtTextResults.Name = "txtTextResults";
			this.txtTextResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtTextResults.Size = new System.Drawing.Size(386, 138);
			this.txtTextResults.TabIndex = 0;
			this.txtTextResults.WordWrap = false;
			// 
			// lblStatement
			// 
			this.lblStatement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatement.Location = new System.Drawing.Point(4, 0);
			this.lblStatement.Name = "lblStatement";
			this.lblStatement.Size = new System.Drawing.Size(392, 27);
			this.lblStatement.TabIndex = 1;
			this.lblStatement.Text = "SQL Statement...";
			// 
			// QueryResultsViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblStatement);
			this.Controls.Add(this.tabControl1);
			this.Name = "QueryResultsViewer";
			this.Size = new System.Drawing.Size(400, 200);
			this.tabControl1.ResumeLayout(false);
			this.tabDataResults.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgDataResults)).EndInit();
			this.tabTextResults.ResumeLayout(false);
			this.tabTextResults.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDataResults;
		private System.Windows.Forms.TabPage tabTextResults;
		private System.Windows.Forms.Label lblStatement;
		private ExtendedDataGridView dgDataResults;
		private System.Windows.Forms.TextBox txtTextResults;
	}
}
