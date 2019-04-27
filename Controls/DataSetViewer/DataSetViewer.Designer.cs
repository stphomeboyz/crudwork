namespace crudwork.Controls
{
	partial class DataSetViewer
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
			this.tabDataSet = new System.Windows.Forms.TabPage();
			this.tabMetadata = new System.Windows.Forms.TabPage();
			this.tabRelation = new System.Windows.Forms.TabPage();
			this.tabXML = new System.Windows.Forms.TabPage();
			this.tabQueryAnalyzer = new System.Windows.Forms.TabPage();
			this.simpleDataSetViewer1 = new crudwork.Controls.SimpleDataSetViewer();
			this.simpleMetaDataViewer1 = new crudwork.Controls.SimpleMetaDataViewer();
			this.simpleDataRelationViewer1 = new crudwork.Controls.SimpleDataRelationViewer();
			this.simpleXMLViewer1 = new crudwork.Controls.SimpleXMLViewer();
			this.queryAnalyzer1 = new crudwork.Controls.DatabaseUC.QueryAnalyzer();
			this.tabControl1.SuspendLayout();
			this.tabDataSet.SuspendLayout();
			this.tabMetadata.SuspendLayout();
			this.tabRelation.SuspendLayout();
			this.tabXML.SuspendLayout();
			this.tabQueryAnalyzer.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabDataSet);
			this.tabControl1.Controls.Add(this.tabMetadata);
			this.tabControl1.Controls.Add(this.tabRelation);
			this.tabControl1.Controls.Add(this.tabXML);
			this.tabControl1.Controls.Add(this.tabQueryAnalyzer);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 200);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.TabStop = false;
			this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
			this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabDataSet
			// 
			this.tabDataSet.Controls.Add(this.simpleDataSetViewer1);
			this.tabDataSet.Location = new System.Drawing.Point(4, 22);
			this.tabDataSet.Name = "tabDataSet";
			this.tabDataSet.Padding = new System.Windows.Forms.Padding(3);
			this.tabDataSet.Size = new System.Drawing.Size(392, 174);
			this.tabDataSet.TabIndex = 0;
			this.tabDataSet.Text = "DataSet";
			this.tabDataSet.UseVisualStyleBackColor = true;
			// 
			// tabMetadata
			// 
			this.tabMetadata.Controls.Add(this.simpleMetaDataViewer1);
			this.tabMetadata.Location = new System.Drawing.Point(4, 22);
			this.tabMetadata.Name = "tabMetadata";
			this.tabMetadata.Padding = new System.Windows.Forms.Padding(3);
			this.tabMetadata.Size = new System.Drawing.Size(392, 174);
			this.tabMetadata.TabIndex = 1;
			this.tabMetadata.Text = "Metadata";
			this.tabMetadata.UseVisualStyleBackColor = true;
			// 
			// tabRelation
			// 
			this.tabRelation.Controls.Add(this.simpleDataRelationViewer1);
			this.tabRelation.Location = new System.Drawing.Point(4, 22);
			this.tabRelation.Name = "tabRelation";
			this.tabRelation.Padding = new System.Windows.Forms.Padding(3);
			this.tabRelation.Size = new System.Drawing.Size(392, 174);
			this.tabRelation.TabIndex = 3;
			this.tabRelation.Text = "Relation";
			this.tabRelation.UseVisualStyleBackColor = true;
			// 
			// tabXML
			// 
			this.tabXML.Controls.Add(this.simpleXMLViewer1);
			this.tabXML.Location = new System.Drawing.Point(4, 22);
			this.tabXML.Name = "tabXML";
			this.tabXML.Size = new System.Drawing.Size(392, 174);
			this.tabXML.TabIndex = 2;
			this.tabXML.Text = "XML";
			this.tabXML.UseVisualStyleBackColor = true;
			// 
			// tabQueryAnalyzer
			// 
			this.tabQueryAnalyzer.Controls.Add(this.queryAnalyzer1);
			this.tabQueryAnalyzer.Location = new System.Drawing.Point(4, 22);
			this.tabQueryAnalyzer.Name = "tabQueryAnalyzer";
			this.tabQueryAnalyzer.Padding = new System.Windows.Forms.Padding(3);
			this.tabQueryAnalyzer.Size = new System.Drawing.Size(392, 174);
			this.tabQueryAnalyzer.TabIndex = 4;
			this.tabQueryAnalyzer.Text = "Query Analyzer";
			this.tabQueryAnalyzer.UseVisualStyleBackColor = true;
			// 
			// simpleDataSetViewer1
			// 
			this.simpleDataSetViewer1.ColumnName = "";
			this.simpleDataSetViewer1.DataSource = null;
			this.simpleDataSetViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleDataSetViewer1.HideTablePanel = false;
			this.simpleDataSetViewer1.Location = new System.Drawing.Point(3, 3);
			this.simpleDataSetViewer1.MinimumSize = new System.Drawing.Size(250, 110);
			this.simpleDataSetViewer1.Name = "simpleDataSetViewer1";
			this.simpleDataSetViewer1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.simpleDataSetViewer1.Size = new System.Drawing.Size(386, 168);
			this.simpleDataSetViewer1.TabIndex = 0;
			this.simpleDataSetViewer1.TableName = "";
			// 
			// simpleMetaDataViewer1
			// 
			this.simpleMetaDataViewer1.CurrentColumn = -1;
			this.simpleMetaDataViewer1.CurrentRow = -1;
			this.simpleMetaDataViewer1.DataSource = null;
			this.simpleMetaDataViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleMetaDataViewer1.Location = new System.Drawing.Point(3, 3);
			this.simpleMetaDataViewer1.Name = "simpleMetaDataViewer1";
			this.simpleMetaDataViewer1.Size = new System.Drawing.Size(386, 168);
			this.simpleMetaDataViewer1.TabIndex = 0;
			// 
			// simpleDataRelationViewer1
			// 
			this.simpleDataRelationViewer1.DataSource = null;
			this.simpleDataRelationViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleDataRelationViewer1.Location = new System.Drawing.Point(3, 3);
			this.simpleDataRelationViewer1.Name = "simpleDataRelationViewer1";
			this.simpleDataRelationViewer1.Size = new System.Drawing.Size(386, 168);
			this.simpleDataRelationViewer1.TabIndex = 0;
			// 
			// simpleXMLViewer1
			// 
			this.simpleXMLViewer1.DataSource = null;
			this.simpleXMLViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleXMLViewer1.Location = new System.Drawing.Point(0, 0);
			this.simpleXMLViewer1.Name = "simpleXMLViewer1";
			this.simpleXMLViewer1.Size = new System.Drawing.Size(392, 174);
			this.simpleXMLViewer1.TabIndex = 0;
			// 
			// queryAnalyzer1
			// 
			this.queryAnalyzer1.DataSource = null;
			this.queryAnalyzer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.queryAnalyzer1.Location = new System.Drawing.Point(3, 3);
			this.queryAnalyzer1.Name = "queryAnalyzer1";
			this.queryAnalyzer1.Query = "";
			this.queryAnalyzer1.Size = new System.Drawing.Size(386, 168);
			this.queryAnalyzer1.TabIndex = 0;
			// 
			// DataSetViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "DataSetViewer";
			this.Size = new System.Drawing.Size(400, 200);
			this.Load += new System.EventHandler(this.DataSetViewer_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabDataSet.ResumeLayout(false);
			this.tabMetadata.ResumeLayout(false);
			this.tabRelation.ResumeLayout(false);
			this.tabXML.ResumeLayout(false);
			this.tabQueryAnalyzer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDataSet;
		private SimpleDataSetViewer simpleDataSetViewer1;
		private System.Windows.Forms.TabPage tabMetadata;
		private SimpleMetaDataViewer simpleMetaDataViewer1;
		private System.Windows.Forms.TabPage tabRelation;
		private System.Windows.Forms.TabPage tabXML;
		private SimpleDataRelationViewer simpleDataRelationViewer1;
		private SimpleXMLViewer simpleXMLViewer1;
		private System.Windows.Forms.TabPage tabQueryAnalyzer;
		private crudwork.Controls.DatabaseUC.QueryAnalyzer queryAnalyzer1;
	}
}
