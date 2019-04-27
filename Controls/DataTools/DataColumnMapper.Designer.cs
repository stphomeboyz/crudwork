namespace crudwork.Controls
{
	partial class DataColumnMapper
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgRelationship = new System.Windows.Forms.DataGridView();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.scPreview = new System.Windows.Forms.SplitContainer();
			this.dsvParent = new crudwork.Controls.SimpleDataSetViewer();
			this.dsvChild = new crudwork.Controls.SimpleDataSetViewer();
			this.scDefinition = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnSet = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnLoadDefinition = new System.Windows.Forms.Button();
			this.btnSaveDefinition = new System.Windows.Forms.Button();
			this.scMain = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.dgRelationship)).BeginInit();
			this.scPreview.Panel1.SuspendLayout();
			this.scPreview.Panel2.SuspendLayout();
			this.scPreview.SuspendLayout();
			this.scDefinition.Panel1.SuspendLayout();
			this.scDefinition.Panel2.SuspendLayout();
			this.scDefinition.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.scMain.Panel1.SuspendLayout();
			this.scMain.Panel2.SuspendLayout();
			this.scMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgRelationship
			// 
			this.dgRelationship.AllowUserToAddRows = false;
			this.dgRelationship.AllowUserToDeleteRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgRelationship.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgRelationship.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgRelationship.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgRelationship.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgRelationship.Location = new System.Drawing.Point(0, 0);
			this.dgRelationship.MultiSelect = false;
			this.dgRelationship.Name = "dgRelationship";
			this.dgRelationship.ReadOnly = true;
			this.dgRelationship.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgRelationship.Size = new System.Drawing.Size(600, 166);
			this.dgRelationship.StandardTab = true;
			this.dgRelationship.TabIndex = 0;
			this.dgRelationship.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgRelationship_RowEnter);
			this.dgRelationship.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgRelationship_DataError);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(165, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "&Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(246, 3);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// scPreview
			// 
			this.scPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scPreview.Location = new System.Drawing.Point(0, 0);
			this.scPreview.Name = "scPreview";
			// 
			// scPreview.Panel1
			// 
			this.scPreview.Panel1.Controls.Add(this.dsvParent);
			// 
			// scPreview.Panel2
			// 
			this.scPreview.Panel2.Controls.Add(this.dsvChild);
			this.scPreview.Size = new System.Drawing.Size(600, 196);
			this.scPreview.SplitterDistance = 300;
			this.scPreview.TabIndex = 0;
			this.scPreview.TabStop = false;
			// 
			// dsvParent
			// 
			this.dsvParent.ColumnName = "";
			this.dsvParent.Cursor = System.Windows.Forms.Cursors.Default;
			this.dsvParent.DataSource = null;
			this.dsvParent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dsvParent.HideTablePanel = true;
			this.dsvParent.Location = new System.Drawing.Point(0, 0);
			this.dsvParent.MinimumSize = new System.Drawing.Size(250, 100);
			this.dsvParent.Name = "dsvParent";
			this.dsvParent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dsvParent.Size = new System.Drawing.Size(300, 196);
			this.dsvParent.TabIndex = 0;
			this.dsvParent.TableName = "";
			this.dsvParent.CellChanged += new crudwork.Controls.CellChangedEventHandler(this.dsvParent_CellChanged);
			this.dsvParent.DataMemberChanged += new System.EventHandler(this.dsvParent_DataMemberChanged);
			this.dsvParent.CellDoubleClick += new System.EventHandler(this.dsvParent_CellDoubleClick);
			// 
			// dsvChild
			// 
			this.dsvChild.ColumnName = "";
			this.dsvChild.Cursor = System.Windows.Forms.Cursors.Default;
			this.dsvChild.DataSource = null;
			this.dsvChild.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dsvChild.HideTablePanel = true;
			this.dsvChild.Location = new System.Drawing.Point(0, 0);
			this.dsvChild.MinimumSize = new System.Drawing.Size(250, 100);
			this.dsvChild.Name = "dsvChild";
			this.dsvChild.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
			this.dsvChild.Size = new System.Drawing.Size(296, 196);
			this.dsvChild.TabIndex = 0;
			this.dsvChild.TableName = "";
			this.dsvChild.CellChanged += new crudwork.Controls.CellChangedEventHandler(this.dsvChild_CellChanged);
			this.dsvChild.DataMemberChanged += new System.EventHandler(this.dsvChild_DataMemberChanged);
			this.dsvChild.CellDoubleClick += new System.EventHandler(this.dsvChild_CellDoubleClick);
			// 
			// scDefinition
			// 
			this.scDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scDefinition.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.scDefinition.IsSplitterFixed = true;
			this.scDefinition.Location = new System.Drawing.Point(0, 0);
			this.scDefinition.Name = "scDefinition";
			this.scDefinition.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scDefinition.Panel1
			// 
			this.scDefinition.Panel1.Controls.Add(this.flowLayoutPanel1);
			// 
			// scDefinition.Panel2
			// 
			this.scDefinition.Panel2.Controls.Add(this.dgRelationship);
			this.scDefinition.Size = new System.Drawing.Size(600, 200);
			this.scDefinition.SplitterDistance = 30;
			this.scDefinition.TabIndex = 2;
			this.scDefinition.TabStop = false;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel1.Controls.Add(this.btnSet);
			this.flowLayoutPanel1.Controls.Add(this.btnClear);
			this.flowLayoutPanel1.Controls.Add(this.btnAdd);
			this.flowLayoutPanel1.Controls.Add(this.btnDelete);
			this.flowLayoutPanel1.Controls.Add(this.btnLoadDefinition);
			this.flowLayoutPanel1.Controls.Add(this.btnSaveDefinition);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(600, 30);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// btnSet
			// 
			this.btnSet.Location = new System.Drawing.Point(3, 3);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(75, 23);
			this.btnSet.TabIndex = 0;
			this.btnSet.Text = "&Set";
			this.btnSet.UseVisualStyleBackColor = true;
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(84, 3);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "&Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnLoadDefinition
			// 
			this.btnLoadDefinition.Location = new System.Drawing.Point(327, 3);
			this.btnLoadDefinition.Name = "btnLoadDefinition";
			this.btnLoadDefinition.Size = new System.Drawing.Size(75, 23);
			this.btnLoadDefinition.TabIndex = 4;
			this.btnLoadDefinition.Text = "&Load...";
			this.btnLoadDefinition.UseVisualStyleBackColor = true;
			this.btnLoadDefinition.Click += new System.EventHandler(this.btnLoadDefinition_Click);
			// 
			// btnSaveDefinition
			// 
			this.btnSaveDefinition.Location = new System.Drawing.Point(408, 3);
			this.btnSaveDefinition.Name = "btnSaveDefinition";
			this.btnSaveDefinition.Size = new System.Drawing.Size(75, 23);
			this.btnSaveDefinition.TabIndex = 5;
			this.btnSaveDefinition.Text = "S&ave...";
			this.btnSaveDefinition.UseVisualStyleBackColor = true;
			this.btnSaveDefinition.Click += new System.EventHandler(this.btnSaveDefinition_Click);
			// 
			// scMain
			// 
			this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scMain.Location = new System.Drawing.Point(0, 0);
			this.scMain.Name = "scMain";
			this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scMain.Panel1
			// 
			this.scMain.Panel1.Controls.Add(this.scDefinition);
			// 
			// scMain.Panel2
			// 
			this.scMain.Panel2.Controls.Add(this.scPreview);
			this.scMain.Size = new System.Drawing.Size(600, 400);
			this.scMain.SplitterDistance = 200;
			this.scMain.TabIndex = 2;
			this.scMain.TabStop = false;
			// 
			// DataColumnMapper
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.scMain);
			this.Name = "DataColumnMapper";
			this.Size = new System.Drawing.Size(600, 400);
			this.Load += new System.EventHandler(this.DataColumnMapper_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgRelationship)).EndInit();
			this.scPreview.Panel1.ResumeLayout(false);
			this.scPreview.Panel2.ResumeLayout(false);
			this.scPreview.ResumeLayout(false);
			this.scDefinition.Panel1.ResumeLayout(false);
			this.scDefinition.Panel2.ResumeLayout(false);
			this.scDefinition.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.scMain.Panel1.ResumeLayout(false);
			this.scMain.Panel2.ResumeLayout(false);
			this.scMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgRelationship;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.SplitContainer scPreview;
		private System.Windows.Forms.SplitContainer scDefinition;
		private System.Windows.Forms.SplitContainer scMain;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.Button btnSaveDefinition;
		private System.Windows.Forms.Button btnLoadDefinition;
		private SimpleDataSetViewer dsvParent;
		private SimpleDataSetViewer dsvChild;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnClear;
	}
}
