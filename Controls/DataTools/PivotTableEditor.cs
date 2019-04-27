using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using crudwork.DataWarehouse.Models;
using crudwork.DataWarehouse;
using crudwork.Utilities;

namespace crudwork.Controls.DataTools
{
	/// <summary>
	/// Pivot Table Editor
	/// </summary>
	public partial class PivotTableEditor : UserControl
	{
		private DataTable _dataSource = null;
		private DataSet _pivotDataSet = null;

		/// <summary>
		/// create new instance with default attirbutes
		/// </summary>
		public PivotTableEditor()
		{
			InitializeComponent();
		}

		private void PivotTableEditor_Load(object sender, EventArgs e)
		{
			userControlEx1.BrushType = BrushType.BrushTypeLinearGradient;
			userControlEx1.Color1 = Color.Black;
			userControlEx1.Color2 = Color.Green;

			cbPivotType.DataSource = new string[] { "One Field", "Two Fields" };
			cbPivotType.OnChanged += new crudwork.Controls.Base.ChangeEventHandler(cbPivotType_OnChanged);
			cbPivotType_OnChanged(sender, new crudwork.Controls.Base.ChangeEventArgs(this));

			cbGroupSignificant.DataSource = Enum.GetNames(typeof(Olap.GroupSignificant));

			cbPivotType.ShowEllipseButton = false;
			cbGroupSignificant.ShowEllipseButton = false;
			cbValue.ShowEllipseButton = false;
			cbKeyColumn.ShowEllipseButton = false;
			cbEntryColumn.ShowEllipseButton = false;
			cbBaseColumn.ShowEllipseButton = false;

			btnShowTable_Click(sender, e);
		}

		private void cbPivotType_OnChanged(object sender, crudwork.Controls.Base.ChangeEventArgs e)
		{
			switch (cbPivotType.Value)
			{
				case "One Field":
					cbGroupSignificant.Visible = true;
					cbKeyColumn.Visible = true;
					cbValue.Visible = true;

					cbEntryColumn.Visible = false;
					cbBaseColumn.Visible = false;
					break;
				case "Two Fields":
					cbGroupSignificant.Visible = false;
					cbKeyColumn.Visible = false;
					cbValue.Visible = true;

					cbEntryColumn.Visible = true;
					cbBaseColumn.Visible = true;
					break;
				default:
					throw new ArgumentOutOfRangeException("cbPivotType.Value=" + cbPivotType.Value);
			}
		}
		private void btnShowTable_Click(object sender, EventArgs e)
		{
			if (_pivotDataSet == null)
				return;
			_dataSource = Olap.SaveOutput(_pivotDataSet);

			dgResult.DataSource = _dataSource;
			dgResult.DataMember = "";
		}
		private void btnShowPivot_Click(object sender, EventArgs e)
		{
			try
			{
				switch (cbPivotType.Value)
				{
					case "One Field":
						_pivotDataSet = Olap.Pivot(_dataSource, ValueColumn, KeyColumn, GroupSignificant);
						break;
					case "Two Fields":
						_pivotDataSet = Olap.Pivot(_dataSource, ValueColumn, EntryColumn, BaseColumn);
						break;
					default:
						throw new ArgumentOutOfRangeException("cbPivotType.Value=" + cbPivotType.Value);
				}

				dgResult.DataSource = _pivotDataSet;
				dgResult.DataMember = Olap.PIVOT_TABLE_NAME;
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex);
			}
		}
		private void dgResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		/// <summary>
		/// Get or set the data table
		/// </summary>
		public DataTable DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
				if (value == null)
					return;

				var cols = new List<string>();
				foreach (DataColumn dc in _dataSource.Columns)
				{
					cols.Add(dc.ColumnName);
				}

				cbValue.DataSource = cols.ToArray();
				cbKeyColumn.DataSource = cols.ToArray();
				cbEntryColumn.DataSource = cols.ToArray();
				cbBaseColumn.DataSource = cols.ToArray();
			}
		}

		private Olap.GroupSignificant GroupSignificant
		{
			get
			{
				return (Olap.GroupSignificant)Enum.Parse(typeof(Olap.GroupSignificant), cbGroupSignificant.Value);
			}
			set
			{
				cbGroupSignificant.Value = value.ToString();
			}
		}
		private string ValueColumn
		{
			get
			{
				return cbValue.Value;
			}
			set
			{
				cbValue.Value = value;
			}
		}
		private string KeyColumn
		{
			get
			{
				return cbKeyColumn.Value;
			}
			set
			{
				cbKeyColumn.Value = value;
			}
		}
		private string EntryColumn
		{
			get
			{
				return cbEntryColumn.Value;
			}
			set
			{
				cbEntryColumn.Value = value;
			}
		}
		private string BaseColumn
		{
			get
			{
				return cbBaseColumn.Value;
			}
			set
			{
				cbBaseColumn.Value = value;
			}
		}
	}
}
