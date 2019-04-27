using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crudwork.DataWarehouse;
using System.Data;
using System.Diagnostics;
using crudwork.Controls;
using crudwork.FileImporters;
using crudwork.DataAccess;
using crudwork.Models.DataAccess;

namespace PivotTableTest
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				TestDBMacro();
				//ShowPTE();
				//PivotTest();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.ReadLine();
			}
		}

		private static void TestDBMacro()
		{
			ControlManager.ShowQueryAnalyzer();
		}
		private static void ShowPTE()
		{
			ControlManager.ShowPivotTableEditor(TestTable());
		}
		private static void PivotTest()
		{
			var dt = Olap.Pivot(TestTable(), "Value", "Field", "Type");
			if (dt == null)
				throw new ApplicationException("dt is empty");
		}
		private static DataTable TestTable()
		{
			var dt = new DataTable();
			dt.Columns.Add("RulesetRuleId", typeof(string));
			dt.Columns.Add("Value", typeof(string));
			dt.Columns.Add("Field", typeof(string));
			dt.Columns.Add("Type", typeof(string));

			#region Sample DataRow
			dt.Rows.Add("ITV_AssigneeUserIdExp_FLAG", "Y", "AssigneeUserId", "Exp_FLAG");
			dt.Rows.Add("ITV_AssigneeUserIdMode_STRING", "Both", "AssigneeUserId", "Mode_STRING");
			dt.Rows.Add("ITV_AssigneeUserIdPerm_STRING", "Write", "AssigneeUserId", "Perm_STRING");
			dt.Rows.Add("ITV_AssigneeUserIdPos_VALUE", "14", "AssigneeUserId", "Pos_VALUE");
			dt.Rows.Add("ITV_ConstructionTypeExp_FLAG", "Y", "ConstructionType", "Exp_FLAG");
			dt.Rows.Add("ITV_ConstructionTypeMode_STRING", "ITV", "ConstructionType", "Mode_STRING");
			dt.Rows.Add("ITV_ConstructionTypePerm_STRING", "Write", "ConstructionType", "Perm_STRING");
			dt.Rows.Add("ITV_ConstructionTypePos_VALUE", "15", "ConstructionType", "Pos_VALUE");
			dt.Rows.Add("ITV_CoverageAVarianceExp_FLAG", "Y", "CoverageAVariance", "Exp_FLAG");
			dt.Rows.Add("ITV_CoverageAVarianceMode_STRING", "Both", "CoverageAVariance", "Mode_STRING");
			dt.Rows.Add("ITV_CoverageAVariancePerm_STRING", "Read", "CoverageAVariance", "Perm_STRING");
			dt.Rows.Add("ITV_CoverageAVariancePos_VALUE", "9", "CoverageAVariance", "Pos_VALUE");
			dt.Rows.Add("ITV_DeliveryStatusExp_FLAG", "Y", "DeliveryStatus", "Exp_FLAG");
			dt.Rows.Add("ITV_DeliveryStatusMode_STRING", "ITV", "DeliveryStatus", "Mode_STRING");
			dt.Rows.Add("ITV_DeliveryStatusPerm_STRING", "Hidden", "DeliveryStatus", "Perm_STRING");
			dt.Rows.Add("ITV_DeliveryStatusPos_VALUE", "", "DeliveryStatus", "Pos_VALUE");
			dt.Rows.Add("ITV_DispositionCodeIdExp_FLAG", "Y", "DispositionCodeId", "Exp_FLAG");
			dt.Rows.Add("ITV_DispositionCodeIdMode_STRING", "Both", "DispositionCodeId", "Mode_STRING");
			dt.Rows.Add("ITV_DispositionCodeIdPerm_STRING", "Write", "DispositionCodeId", "Perm_STRING");
			dt.Rows.Add("ITV_DispositionCodeIdPos_VALUE", "11", "DispositionCodeId", "Pos_VALUE");
			dt.Rows.Add("ITV_DispositionCodeUpdateDateExp_FLAG", "Y", "DispositionCodeUpdateDate", "Exp_FLAG");
			dt.Rows.Add("ITV_DispositionCodeUpdateDateMode_STRING", "ITV", "DispositionCodeUpdateDate", "Mode_STRING");
			dt.Rows.Add("ITV_DispositionCodeUpdateDatePerm_STRING", "Read", "DispositionCodeUpdateDate", "Perm_STRING");
			dt.Rows.Add("ITV_DispositionCodeUpdateDatePos_VALUE", "12", "DispositionCodeUpdateDate", "Pos_VALUE");
			dt.Rows.Add("ITV_InsuredNameExp_FLAG", "Y", "InsuredName", "Exp_FLAG");
			dt.Rows.Add("ITV_InsuredNameMode_STRING", "Both", "InsuredName", "Mode_STRING");
			dt.Rows.Add("ITV_InsuredNamePerm_STRING", "Read", "InsuredName", "Perm_STRING");
			dt.Rows.Add("ITV_InsuredNamePos_VALUE", "5", "InsuredName", "Pos_VALUE");
			dt.Rows.Add("ITV_OriginalCoverageExp_FLAG", "Y", "OriginalCoverage", "Exp_FLAG");
			dt.Rows.Add("ITV_OriginalCoverageMode_STRING", "Both", "OriginalCoverage", "Mode_STRING");
			dt.Rows.Add("ITV_OriginalCoveragePerm_STRING", "Read", "OriginalCoverage", "Perm_STRING");
			dt.Rows.Add("ITV_OriginalCoveragePos_VALUE", "7", "OriginalCoverage", "Pos_VALUE");
			dt.Rows.Add("ITV_OriginalYearBuiltExp_FLAG", "Y", "OriginalYearBuilt", "Exp_FLAG");
			dt.Rows.Add("ITV_OriginalYearBuiltMode_STRING", "ITV", "OriginalYearBuilt", "Mode_STRING");
			dt.Rows.Add("ITV_OriginalYearBuiltPerm_STRING", "Read", "OriginalYearBuilt", "Perm_STRING");
			dt.Rows.Add("ITV_OriginalYearBuiltPos_VALUE", "", "OriginalYearBuilt", "Pos_VALUE");
			dt.Rows.Add("ITV_PeriDeductibleExp_FLAG", "N", "PeriDeductible", "Exp_FLAG");
			dt.Rows.Add("ITV_PeriDeductibleMode_STRING", "ITV", "PeriDeductible", "Mode_STRING");
			dt.Rows.Add("ITV_PeriDeductiblePerm_STRING", "Hidden", "PeriDeductible", "Perm_STRING");
			dt.Rows.Add("ITV_PeriDeductiblePos_VALUE", "", "PeriDeductible", "Pos_VALUE");
			dt.Rows.Add("ITV_PolicyNumExp_FLAG", "Y", "PolicyNum", "Exp_FLAG");
			dt.Rows.Add("ITV_PolicyNumMode_STRING", "Both", "PolicyNum", "Mode_STRING");
			dt.Rows.Add("ITV_PolicyNumPerm_STRING", "Read", "PolicyNum", "Perm_STRING");
			dt.Rows.Add("ITV_PolicyNumPos_VALUE", "4", "PolicyNum", "Pos_VALUE");
			dt.Rows.Add("ITV_RenewalDateExp_FLAG", "Y", "RenewalDate", "Exp_FLAG");
			dt.Rows.Add("ITV_RenewalDateMode_STRING", "ITV", "RenewalDate", "Mode_STRING");
			dt.Rows.Add("ITV_RenewalDatePerm_STRING", "Read", "RenewalDate", "Perm_STRING");
			dt.Rows.Add("ITV_RenewalDatePos_VALUE", "6", "RenewalDate", "Pos_VALUE");
			dt.Rows.Add("ITV_TesAlertIdExp_FLAG", "Y", "TesAlertId", "Exp_FLAG");
			dt.Rows.Add("ITV_TesAlertIdMode_STRING", "ITV", "TesAlertId", "Mode_STRING");
			dt.Rows.Add("ITV_TesAlertIdPerm_STRING", "Read", "TesAlertId", "Perm_STRING");
			dt.Rows.Add("ITV_TesAlertIdPos_VALUE", "13", "TesAlertId", "Pos_VALUE");
			dt.Rows.Add("ITV_TotalLivingAreaExp_FLAG", "Y", "TotalLivingArea", "Exp_FLAG");
			dt.Rows.Add("ITV_TotalLivingAreaMode_STRING", "ITV", "TotalLivingArea", "Mode_STRING");
			dt.Rows.Add("ITV_TotalLivingAreaPerm_STRING", "Read", "TotalLivingArea", "Perm_STRING");
			dt.Rows.Add("ITV_TotalLivingAreaPos_VALUE", "", "TotalLivingArea", "Pos_VALUE");
			dt.Rows.Add("ITV_UpdatedReplacementCostExp_FLAG", "Y", "UpdatedReplacementCost", "Exp_FLAG");
			dt.Rows.Add("ITV_UpdatedReplacementCostMode_STRING", "Both", "UpdatedReplacementCost", "Mode_STRING");
			dt.Rows.Add("ITV_UpdatedReplacementCostPerm_STRING", "Read", "UpdatedReplacementCost", "Perm_STRING");
			dt.Rows.Add("ITV_UpdatedReplacementCostPos_VALUE", "8", "UpdatedReplacementCost", "Pos_VALUE");
			dt.Rows.Add("ITV_UpdatedYearBuiltExp_FLAG", "Y", "UpdatedYearBuilt", "Exp_FLAG");
			dt.Rows.Add("ITV_UpdatedYearBuiltMode_STRING", "ITV", "UpdatedYearBuilt", "Mode_STRING");
			dt.Rows.Add("ITV_UpdatedYearBuiltPerm_STRING", "Read", "UpdatedYearBuilt", "Perm_STRING");
			dt.Rows.Add("ITV_UpdatedYearBuiltPos_VALUE", "", "UpdatedYearBuilt", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField10Exp_FLAG", "N", "UserDefinedField10", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField10Mode_STRING", "Both", "UserDefinedField10", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField10Perm_STRING", "Read", "UserDefinedField10", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField10Pos_VALUE", "16", "UserDefinedField10", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField11Exp_FLAG", "Y", "UserDefinedField11", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField11Mode_STRING", "Assign", "UserDefinedField11", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField11Perm_STRING", "Hidden", "UserDefinedField11", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField11Pos_VALUE", "", "UserDefinedField11", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField12Exp_FLAG", "Y", "UserDefinedField12", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField12Mode_STRING", "Assign", "UserDefinedField12", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField12Perm_STRING", "Hidden", "UserDefinedField12", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField12Pos_VALUE", "", "UserDefinedField12", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField13Exp_FLAG", "Y", "UserDefinedField13", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField13Mode_STRING", "Assign", "UserDefinedField13", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField13Perm_STRING", "Hidden", "UserDefinedField13", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField13Pos_VALUE", "", "UserDefinedField13", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField14Exp_FLAG", "Y", "UserDefinedField14", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField14Mode_STRING", "ITV", "UserDefinedField14", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField14Perm_STRING", "Read", "UserDefinedField14", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField14Pos_VALUE", "2", "UserDefinedField14", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField15Exp_FLAG", "Y", "UserDefinedField15", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField15Mode_STRING", "Assign", "UserDefinedField15", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField15Perm_STRING", "Hidden", "UserDefinedField15", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField15Pos_VALUE", "", "UserDefinedField15", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField1Exp_FLAG", "Y", "UserDefinedField1", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField1Mode_STRING", "Both", "UserDefinedField1", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField1Perm_STRING", "Read", "UserDefinedField1", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField1Pos_VALUE", "1", "UserDefinedField1", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField2Exp_FLAG", "Y", "UserDefinedField2", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField2Mode_STRING", "Both", "UserDefinedField2", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField2Perm_STRING", "Read", "UserDefinedField2", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField2Pos_VALUE", "3", "UserDefinedField2", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField3Exp_FLAG", "Y", "UserDefinedField3", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField3Mode_STRING", "Assign", "UserDefinedField3", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField3Perm_STRING", "Read", "UserDefinedField3", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField3Pos_VALUE", "5", "UserDefinedField3", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField4Exp_FLAG", "Y", "UserDefinedField4", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField4Mode_STRING", "Both", "UserDefinedField4", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField4Perm_STRING", "Read", "UserDefinedField4", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField4Pos_VALUE", "10", "UserDefinedField4", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField5Exp_FLAG", "Y", "UserDefinedField5", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField5Mode_STRING", "Both", "UserDefinedField5", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField5Perm_STRING", "Read", "UserDefinedField5", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField5Pos_VALUE", "", "UserDefinedField5", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField6Exp_FLAG", "Y", "UserDefinedField6", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField6Mode_STRING", "Assign", "UserDefinedField6", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField6Perm_STRING", "Hidden", "UserDefinedField6", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField6Pos_VALUE", "", "UserDefinedField6", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField7Exp_FLAG", "Y", "UserDefinedField7", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField7Mode_STRING", "Assign", "UserDefinedField7", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField7Perm_STRING", "Hidden", "UserDefinedField7", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField7Pos_VALUE", "", "UserDefinedField7", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField8Exp_FLAG", "Y", "UserDefinedField8", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField8Mode_STRING", "Assign", "UserDefinedField8", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField8Perm_STRING", "Hidden", "UserDefinedField8", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField8Pos_VALUE", "", "UserDefinedField8", "Pos_VALUE");
			dt.Rows.Add("ITV_UserDefinedField9Exp_FLAG", "Y", "UserDefinedField9", "Exp_FLAG");
			dt.Rows.Add("ITV_UserDefinedField9Mode_STRING", "Assign", "UserDefinedField9", "Mode_STRING");
			dt.Rows.Add("ITV_UserDefinedField9Perm_STRING", "Hidden", "UserDefinedField9", "Perm_STRING");
			dt.Rows.Add("ITV_UserDefinedField9Pos_VALUE", "", "UserDefinedField9", "Pos_VALUE");
			#endregion
			return dt;
		}
		private static DataTable SampleGSData()
		{
			var options = new ExcelImportOptions();
			var ds = ImportManager.Import(@"Data\PivotData.xlsx", options);
			using (var dv = new DataView(ds.Tables["SampleGS$"]))
			{
				return dv.ToTable();
			}
		}
		private static DataTable SampleXYData()
		{
			var options = new ExcelImportOptions();
			var ds = ImportManager.Import(@"Data\PivotData.xlsx", options);
			using (var dv = new DataView(ds.Tables["SampleXY$"]))
			{
				return dv.ToTable();
			}
		}
	}
}
