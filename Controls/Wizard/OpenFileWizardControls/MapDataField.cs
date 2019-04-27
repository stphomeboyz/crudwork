// crudwork
// Copyright 2004 by Steve T. Pham (http://www.crudwork.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with This program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using crudwork.Utilities;
using crudwork.DataAccess;
using crudwork.Controls.Wizard;
using crudwork.FileImporters;
using crudwork.Controls;
using System.Diagnostics;
using crudwork.Models.OpenFileWizard;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	/// <summary>
	/// Wizard Step 2: Map Data Fields
	/// </summary>
	internal partial class MapDataField : UserControl, IWizardControl
	{
		private const int MAX_SAMPLE = 20;

		private string prevChildString = string.Empty;
		private DataSet prevChildDataSet = null;

		public event ValidateDataColumnMapperEventHandler ValidateDataColumnMapper = null;

		public ChooseInputSource Step1
		{
			get;
			private set;
		}

		public MapDataField(ChooseInputSource step1)
		{
			InitializeComponent();
			this.Step1 = step1;
		}

		private void WizardStep2_Load(object sender, EventArgs e)
		{
			lblDescription.Text = "Step 2: Map Data Fields";

			var dsParent = LoadParentDataSet();
			dcm.SetRelation(LoadRelationshipTable(dsParent.Tables[0]), "Field", "Column");
			dcm.SetParent(dsParent);

			dcm.ShowParentGrid = false;
			dcm.ShowChildGrid = true;

			dcm.ShowAddDeleteButtons(false);
			dcm.ShowLoadSaveButtons(true);

			//dcm.SetChild(LoadChildDataSet());
		}

		#region Helpers
		private DataSet LoadParentDataSet()
		{
			var ds = new DataSet("ParentDataSet");
			var dt = new DataTable("_");
			ds.Tables.Add(dt);

			dt.Columns.Add(NewDataColumn("Zipcode", true, "Zipcode"));
			dt.Columns.Add(NewDataColumn("State", true, "Abbreviated State Name"));
			dt.Columns.Add(NewDataColumn("County", false, "County Name"));
			dt.Columns.Add(NewDataColumn("CovA", true, "The Coverage Amount"));
			dt.Columns.Add(NewDataColumn("TLA", false, "The Total Living Area"));
			dt.Columns.Add(NewDataColumn("YearBuilt", false, "The Year Built"));
			dt.Columns.Add(NewDataColumn("CLSQ", false, "Client sequence number / primary number"));

			return ds;
		}

		private DataTable LoadRelationshipTable(DataTable dtParent)
		{
			var dt = new DataTable("Definition");
			dt.Columns.Add("Field", typeof(string));
			dt.Columns.Add("Required", typeof(bool));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("Column", typeof(string));

			string tablename = dtParent.TableName;

			foreach (DataColumn dc in dtParent.Columns)
			{
				dt.Rows.Add(tablename + "." + dc.ColumnName, !dc.AllowDBNull, dc.Caption, null);
			}

			return dt;
		}

		private DataSet LoadChildDataSet()
		{
			#region Check cache for previous settings
			if (prevChildString == Step1.ToString() && prevChildDataSet != null)
				return prevChildDataSet;

			prevChildString = Step1.ToString();
			#endregion

			DataSet ds = null;

			dcm.ClearRelations();

			if (Step1.InputDataSource == InputDataSourceType.File && !Step1.FileSupportsTables(Step1.Filename))
			{
				// load a sample from file
				// TODO: Need to implemen a way to import sample XXX records
				var options = new ConverterOptionList();
				options["ImportMaxRows"] = MAX_SAMPLE.ToString();

				// currently, the following file types support the ImportMaxRows feature:
				// 1) any delimiter derived class (csv, tab)
				// 2) dbase 4

#pragma warning disable 0618
				ds = prevChildDataSet = ImportManager.Import(Step1.Filename, options);
#pragma warning restore 0618
			}
			else
			{
				// load a sample from database
				ds = new DataSet("ChildDataSet");
				DataTable dt = null;
				int nr = 0;

				var df = new DataFactory(Step1.Provider, Step1.ConnectionString);
				string query = string.Format("select * from [{0}]", Step1.Tablename);

				#region Load a sample of XXX records
				foreach (DataRow dr in df.ExecuteReader(query))
				{
					if (nr++ == 0)
					{
						// create a data table
						dt = dr.Table.Copy();
						dt.TableName = Step1.Tablename;
						ds.Tables.Add(dt);
					}

					DataRow drNew = dt.NewRow();
					DataUtil.CopyRow(dr, drNew);
					dt.Rows.Add(drNew);

					if (nr == MAX_SAMPLE)
						break;
				}
				#endregion

				ds = prevChildDataSet = ds;
			}

			return ds;
		}

		private DataColumn NewDataColumn(string columnName, bool required, string caption)
		{
			DataColumn dc = new DataColumn(columnName, typeof(string));
			dc.AllowDBNull = !required;
			dc.Caption = caption;
			return dc;
		}

		private ParentChildRelationshipList ToParentChildRelationshipList(DataTable dataTable)
		{
			var results = new ParentChildRelationshipList();

			foreach (DataRow item in Editor.RelationshipTable.Rows)
			{
				string parent = item["Field"].ToString();
				string child = item["Column"].ToString();
				bool isRequired = DataConvert.ToBoolean(item["Required"].ToString(), false);
				results.Add(parent, child, isRequired);
			}

			return results;
		}
		#endregion

		public DataColumnMapper Editor
		{
			get
			{
				return dcm;
			}
		}

		private void DefaultOnValidateDataColumnMapper()
		{
			//			// if YearBuilt is defined, then TLA is also required.
			//			bool defineTLA = false;
			//			bool defineYearBuilt = false;

			var dt = dcm.RelationshipTable;
			var definedColumns = new List<string>();

			#region Check required fields
			foreach (DataRow dr in dt.Rows)
			{
				var column = new TableColumn(DataConvert.ToString(dr["Column"], string.Empty));
				var field = new TableColumn(DataConvert.ToString(dr["Field"], string.Empty));
				bool required = DataConvert.ToBoolean(dr["Required"], false);

				if (!string.IsNullOrEmpty(column.ColumnName))
				{
					if (definedColumns.Contains(column.ColumnName))
						throw new ArgumentException("A column cannot be mapped to multiple fields: " + column);
					definedColumns.Add(column.ColumnName);
				}

				if (required && string.IsNullOrEmpty(column.ColumnName))
					throw new ArgumentException("Field requirement not mapped: " + field.ColumnName);

				//				if (field.ColumnName == "TLA" && !string.IsNullOrEmpty(column.ColumnName))
				//					defineTLA = true;
				//				if (field.ColumnName == "YearBuilt" && !string.IsNullOrEmpty(column.ColumnName))
				//					defineYearBuilt = true;
			}
			#endregion

			//			if (defineYearBuilt && !defineTLA)
			//				throw new ArgumentException("If YearBuilt is mapped, then TLA is also required.");
		}

		#region IWizardControl Members

		void IWizardControl.ValidateContent()
		{
			var t = ValidateDataColumnMapper;

			if (t == null)
			{
				DefaultOnValidateDataColumnMapper();
			}
			else
			{
				// NOTE: Do not absorb the exception thrown by the subscriber!
				// If validation failed, allow the error to bubble up to the wizard
				var e = new ValidateDataColumnMapperEventArgs(
					Editor.ParentDataSet, 
					Editor.ChildDataSet, 
					ToParentChildRelationshipList(Editor.RelationshipTable));
				t(this, e);
			}
		}

		void IWizardControl.RefreshContent()
		{
			dcm.SetChild(LoadChildDataSet());
		}

		#endregion
	}
}
