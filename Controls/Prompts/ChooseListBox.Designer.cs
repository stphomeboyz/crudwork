namespace crudwork.Controls
{
	partial class ChooseListBox
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
			this.lstAvailable = new System.Windows.Forms.ListBox();
			this.lstSelected = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnDeselect = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.pnlCenter = new System.Windows.Forms.Panel();
			this.btnMoveDown = new System.Windows.Forms.Button();
			this.btnMoveUp = new System.Windows.Forms.Button();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1.SuspendLayout();
			this.pnlCenter.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstAvailable
			// 
			this.lstAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstAvailable.FormattingEnabled = true;
			this.lstAvailable.IntegralHeight = false;
			this.lstAvailable.Location = new System.Drawing.Point(0, 29);
			this.lstAvailable.Name = "lstAvailable";
			this.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstAvailable.Size = new System.Drawing.Size(140, 196);
			this.lstAvailable.TabIndex = 1;
			this.lstAvailable.DoubleClick += new System.EventHandler(this.lstAvailable_DoubleClick);
			// 
			// lstSelected
			// 
			this.lstSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstSelected.FormattingEnabled = true;
			this.lstSelected.IntegralHeight = false;
			this.lstSelected.Location = new System.Drawing.Point(0, 29);
			this.lstSelected.Name = "lstSelected";
			this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstSelected.Size = new System.Drawing.Size(140, 196);
			this.lstSelected.TabIndex = 1;
			this.lstSelected.DoubleClick += new System.EventHandler(this.lstSelected_DoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Available";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Selected";
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(9, 13);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(75, 23);
			this.btnSelect.TabIndex = 0;
			this.btnSelect.Text = "-->";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnDeselect
			// 
			this.btnDeselect.Location = new System.Drawing.Point(9, 42);
			this.btnDeselect.Name = "btnDeselect";
			this.btnDeselect.Size = new System.Drawing.Size(75, 23);
			this.btnDeselect.TabIndex = 1;
			this.btnDeselect.Text = "<--";
			this.btnDeselect.UseVisualStyleBackColor = true;
			this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.pnlCenter, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.pnlLeft, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.pnlRight, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 231);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// pnlCenter
			// 
			this.pnlCenter.Controls.Add(this.btnMoveDown);
			this.pnlCenter.Controls.Add(this.btnMoveUp);
			this.pnlCenter.Controls.Add(this.btnSelect);
			this.pnlCenter.Controls.Add(this.btnDeselect);
			this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCenter.Location = new System.Drawing.Point(149, 3);
			this.pnlCenter.Name = "pnlCenter";
			this.pnlCenter.Size = new System.Drawing.Size(94, 225);
			this.pnlCenter.TabIndex = 1;
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Location = new System.Drawing.Point(9, 100);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
			this.btnMoveDown.TabIndex = 3;
			this.btnMoveDown.Text = "Move Down";
			this.btnMoveDown.UseVisualStyleBackColor = true;
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Location = new System.Drawing.Point(9, 71);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
			this.btnMoveUp.TabIndex = 2;
			this.btnMoveUp.Text = "Move Up";
			this.btnMoveUp.UseVisualStyleBackColor = true;
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.label1);
			this.pnlLeft.Controls.Add(this.lstAvailable);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeft.Location = new System.Drawing.Point(3, 3);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(140, 225);
			this.pnlLeft.TabIndex = 0;
			// 
			// pnlRight
			// 
			this.pnlRight.Controls.Add(this.label2);
			this.pnlRight.Controls.Add(this.lstSelected);
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRight.Location = new System.Drawing.Point(249, 3);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(140, 225);
			this.pnlRight.TabIndex = 2;
			// 
			// ChooseListBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ChooseListBox";
			this.Size = new System.Drawing.Size(392, 231);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.pnlCenter.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlLeft.PerformLayout();
			this.pnlRight.ResumeLayout(false);
			this.pnlRight.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstAvailable;
		private System.Windows.Forms.ListBox lstSelected;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnDeselect;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel pnlCenter;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Panel pnlRight;
		private System.Windows.Forms.Button btnMoveDown;
		private System.Windows.Forms.Button btnMoveUp;
	}
}