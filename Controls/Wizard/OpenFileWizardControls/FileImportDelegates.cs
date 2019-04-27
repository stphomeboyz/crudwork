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
//using System.Linq;
using System.Text;
using System.Data;
using crudwork.Models.OpenFileWizard;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	internal delegate void FilenameChangedEventHandler(object sender, FilenameChangedEventArgs e);
	internal class FilenameChangedEventArgs : EventArgs
	{
		public string Filename
		{
			get;
			private set;
		}
		public FilenameChangedEventArgs(string filename)
		{
			this.Filename = filename;
		}
	}

	/// <summary>
	/// subscribe to this event handler to perform custom data column validation
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ValidateDataColumnMapperEventHandler(object sender, ValidateDataColumnMapperEventArgs e);

	/// <summary>
	/// the event args to the ValidateDataColumnMapperEventHandler delegate
	/// </summary>
	public class ValidateDataColumnMapperEventArgs : EventArgs
	{
		/// <summary>
		/// get the parent data source
		/// </summary>
		public DataSet Parent
		{
			get;
			private set;
		}

		/// <summary>
		/// get the child data source
		/// </summary>
		public DataSet Child
		{
			get;
			private set;
		}

		/// <summary>
		/// get the data column mapping relational map
		/// </summary>
		public ParentChildRelationshipList Relationship
		{
			get;
			set;
		}

		/// <summary>
		/// create a new instance with given attributes
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		/// <param name="relationship"></param>
		public ValidateDataColumnMapperEventArgs(DataSet parent, DataSet child, ParentChildRelationshipList relationship)
		{
			this.Parent = parent;
			this.Child = child;
			this.Relationship = relationship;
		}
	}
}
