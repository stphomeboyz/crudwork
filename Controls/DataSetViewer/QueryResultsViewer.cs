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
using System.Text;
using System.Windows.Forms;
using crudwork.DataAccess;
using crudwork.DataSetTools;

namespace crudwork.Controls.DatabaseUC
{
	/// <summary>
	/// Query Result viewer
	/// </summary>
	public partial class QueryResultsViewer : UserControl
	{
		private QueryResult results;

		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public QueryResultsViewer()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="result"></param>
		public QueryResultsViewer(QueryResult result)
			: this()
		{
			Results = result;
		}

		/// <summary>
		/// Get or set the result
		/// </summary>
		public QueryResult Results
		{
			get
			{
				return this.results;
			}
			set
			{
				this.results = value;
				AssignControls();
			}
		}

		private void AssignControls()
		{
			if (results == null)
			{
				dgDataResults.DataSource = null;
				txtTextResults.Text = string.Empty;
				return;
			}

			lblStatement.Text = results.Statement == null ? string.Empty : results.Statement.ToString();
			dgDataResults.DataSource = results.DataResult;

			txtTextResults.Text = string.Format("{0}\r\n{1}\r\n{2}",
				results.ErrorText,
				results.TextResult,
				results.RowAffected == -1 ? "" : "(" + results.RowAffected + " row(s) affected)");

			if (results.DataResult != null && string.IsNullOrEmpty(results.ErrorText))
				tabControl1.SelectedTab = tabDataResults;
			else
				tabControl1.SelectedTab = tabTextResults;
		}
	}
}
