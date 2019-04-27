namespace crudwork.Controls.DataTools
{
	partial class PivotTableEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PivotTableEditor));
			this.dgResult = new System.Windows.Forms.DataGridView();
			this.btnShowPivot = new System.Windows.Forms.Button();
			this.flpControls = new System.Windows.Forms.FlowLayoutPanel();
			this.cbPivotType = new crudwork.Controls.FormControls.ComboBoxForm();
			this.cbGroupSignificant = new crudwork.Controls.FormControls.ComboBoxForm();
			this.cbValue = new crudwork.Controls.FormControls.ComboBoxForm();
			this.cbKeyColumn = new crudwork.Controls.FormControls.ComboBoxForm();
			this.cbEntryColumn = new crudwork.Controls.FormControls.ComboBoxForm();
			this.cbBaseColumn = new crudwork.Controls.FormControls.ComboBoxForm();
			this.btnShowTable = new System.Windows.Forms.Button();
			this.userControlEx1 = new crudwork.Controls.UserControlEx();
			((System.ComponentModel.ISupportInitialize)(this.dgResult)).BeginInit();
			this.flpControls.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgResult
			// 
			this.dgResult.AllowUserToAddRows = false;
			this.dgResult.AllowUserToDeleteRows = false;
			this.dgResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgResult.Location = new System.Drawing.Point(0, 96);
			this.dgResult.Name = "dgResult";
			this.dgResult.Size = new System.Drawing.Size(400, 104);
			this.dgResult.TabIndex = 3;
			this.dgResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResult_CellContentClick);
			// 
			// btnShowPivot
			// 
			this.btnShowPivot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShowPivot.Location = new System.Drawing.Point(322, 67);
			this.btnShowPivot.Name = "btnShowPivot";
			this.btnShowPivot.Size = new System.Drawing.Size(75, 23);
			this.btnShowPivot.TabIndex = 2;
			this.btnShowPivot.Text = "Show Pivot";
			this.btnShowPivot.UseVisualStyleBackColor = true;
			this.btnShowPivot.Click += new System.EventHandler(this.btnShowPivot_Click);
			// 
			// flpControls
			// 
			this.flpControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flpControls.AutoScroll = true;
			this.flpControls.Controls.Add(this.cbPivotType);
			this.flpControls.Controls.Add(this.cbGroupSignificant);
			this.flpControls.Controls.Add(this.cbValue);
			this.flpControls.Controls.Add(this.cbKeyColumn);
			this.flpControls.Controls.Add(this.cbEntryColumn);
			this.flpControls.Controls.Add(this.cbBaseColumn);
			this.flpControls.Location = new System.Drawing.Point(0, 0);
			this.flpControls.Name = "flpControls";
			this.flpControls.Size = new System.Drawing.Size(400, 57);
			this.flpControls.TabIndex = 0;
			// 
			// cbPivotType
			// 
			this.cbPivotType.DataSource = null;
			this.cbPivotType.Label = "Pivot Type:";
			this.cbPivotType.Location = new System.Drawing.Point(3, 3);
			this.cbPivotType.Name = "cbPivotType";
			this.cbPivotType.ShowEllipseButton = true;
			this.cbPivotType.Size = new System.Drawing.Size(180, 47);
			this.cbPivotType.TabIndex = 0;
			// 
			// cbGroupSignificant
			// 
			this.cbGroupSignificant.DataSource = null;
			this.cbGroupSignificant.Label = "Group Significant:";
			this.cbGroupSignificant.Location = new System.Drawing.Point(189, 3);
			this.cbGroupSignificant.Name = "cbGroupSignificant";
			this.cbGroupSignificant.ShowEllipseButton = true;
			this.cbGroupSignificant.Size = new System.Drawing.Size(180, 47);
			this.cbGroupSignificant.TabIndex = 1;
			// 
			// cbValue
			// 
			this.cbValue.DataSource = null;
			this.cbValue.Label = "Value column:";
			this.cbValue.Location = new System.Drawing.Point(3, 56);
			this.cbValue.Name = "cbValue";
			this.cbValue.ShowEllipseButton = true;
			this.cbValue.Size = new System.Drawing.Size(180, 47);
			this.cbValue.TabIndex = 2;
			// 
			// cbKeyColumn
			// 
			this.cbKeyColumn.DataSource = null;
			this.cbKeyColumn.Label = "Key column:";
			this.cbKeyColumn.Location = new System.Drawing.Point(189, 56);
			this.cbKeyColumn.Name = "cbKeyColumn";
			this.cbKeyColumn.ShowEllipseButton = true;
			this.cbKeyColumn.Size = new System.Drawing.Size(180, 47);
			this.cbKeyColumn.TabIndex = 3;
			// 
			// cbEntryColumn
			// 
			this.cbEntryColumn.DataSource = null;
			this.cbEntryColumn.Label = "Entry column:";
			this.cbEntryColumn.Location = new System.Drawing.Point(3, 109);
			this.cbEntryColumn.Name = "cbEntryColumn";
			this.cbEntryColumn.ShowEllipseButton = true;
			this.cbEntryColumn.Size = new System.Drawing.Size(180, 47);
			this.cbEntryColumn.TabIndex = 4;
			// 
			// cbBaseColumn
			// 
			this.cbBaseColumn.DataSource = null;
			this.cbBaseColumn.Label = "Base column:";
			this.cbBaseColumn.Location = new System.Drawing.Point(189, 109);
			this.cbBaseColumn.Name = "cbBaseColumn";
			this.cbBaseColumn.ShowEllipseButton = true;
			this.cbBaseColumn.Size = new System.Drawing.Size(180, 47);
			this.cbBaseColumn.TabIndex = 5;
			// 
			// btnShowTable
			// 
			this.btnShowTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShowTable.Location = new System.Drawing.Point(241, 67);
			this.btnShowTable.Name = "btnShowTable";
			this.btnShowTable.Size = new System.Drawing.Size(75, 23);
			this.btnShowTable.TabIndex = 1;
			this.btnShowTable.Text = "Show Table";
			this.btnShowTable.UseVisualStyleBackColor = true;
			this.btnShowTable.Click += new System.EventHandler(this.btnShowTable_Click);
			// 
			// userControlEx1
			// 
			this.userControlEx1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.userControlEx1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("userControlEx1.BackgroundImage")));
			this.userControlEx1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.userControlEx1.BrushType = crudwork.Controls.BrushType.BrushTypeSolidColor;
			this.userControlEx1.Color1 = System.Drawing.Color.Black;
			this.userControlEx1.Color2 = System.Drawing.Color.Gainsboro;
			this.userControlEx1.HatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
			this.userControlEx1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
			this.userControlEx1.Location = new System.Drawing.Point(0, 56);
			this.userControlEx1.Name = "userControlEx1";
			this.userControlEx1.Size = new System.Drawing.Size(400, 5);
			this.userControlEx1.TabIndex = 1;
			// 
			// PivotTableEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnShowTable);
			this.Controls.Add(this.flpControls);
			this.Controls.Add(this.userControlEx1);
			this.Controls.Add(this.btnShowPivot);
			this.Controls.Add(this.dgResult);
			this.Name = "PivotTableEditor";
			this.Size = new System.Drawing.Size(400, 200);
			this.Load += new System.EventHandler(this.PivotTableEditor_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgResult)).EndInit();
			this.flpControls.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private crudwork.Controls.FormControls.ComboBoxForm cbPivotType;
		private crudwork.Controls.FormControls.ComboBoxForm cbEntryColumn;
		private crudwork.Controls.FormControls.ComboBoxForm cbBaseColumn;
		private System.Windows.Forms.DataGridView dgResult;
		private System.Windows.Forms.Button btnShowPivot;
		private crudwork.Controls.FormControls.ComboBoxForm cbValue;
		private crudwork.Controls.FormControls.ComboBoxForm cbGroupSignificant;
		private UserControlEx userControlEx1;
		private System.Windows.Forms.FlowLayoutPanel flpControls;
		private crudwork.Controls.FormControls.ComboBoxForm cbKeyColumn;
		private System.Windows.Forms.Button btnShowTable;

	}
}
