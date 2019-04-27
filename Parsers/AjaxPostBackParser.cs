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
using System.Text;
#if !SILVERLIGHT
using System.Data;
#endif
using System.Diagnostics;

namespace crudwork.Parsers
{
	/// <summary>
	/// Parser for the AJAX post back.
	/// </summary>
	public static class AjaxPostBackParser
	{
		/// <summary>
		/// Delimiter
		/// </summary>
		public const char Delimiter = '|';

		/// <summary>
		/// The Delta Type
		/// </summary>
		/// <remarks>description described on http://msdn.microsoft.com/en-us/magazine/cc163363.aspx</remarks>
		public enum DeltaType
		{
			/// <summary>Stores the updated markup for a partial rendering region. </summary>
			UpdatePanel,
			/// <summary>Stores the content of a hidden field.</summary>
			HiddenField,
			/// <summary>Stores an array declaration added to the response using the appropriate RegisterXXX method on the script manager.</summary>
			ArrayDeclaration,
			/// <summary>Stores a script block added to the response using the appropriate RegisterXXX method on the script manager. The specified script is executed or inserted in the page based on which RegisterXXX method you used.</summary>
			ScriptBlock,
			/// <summary>Stores an expando property added to the response using the appropriate RegisterXXX method on the script manager. The specified value is assigned to the specified property ID when the response is processed.</summary>
			Expando,
			/// <summary>Stores an onsubmit script block added to the response using the appropriate RegisterXXX method on the script manager.</summary>
			OnSubmit,
			/// <summary>Stores the ID of controls registered as asynchronous triggers for the update panels in the call.</summary>
			AsyncPostBackControlIDs,
			/// <summary>Stores the ID of controls registered as postback triggers for the update panels in the call.</summary>
			PostBackControlIDs,
			/// <summary>Stores the ID of the update panels involved with the call.</summary>
			UpdatePanelIDs,
			/// <summary>Stores the timeout of the request in seconds.</summary>
			AsyncPostBackTimeout,
			/// <summary>Stores the ID of any nested update panel refreshed in the call.</summary>
			ChildUpdatePanelIDs,
			/// <summary>Stores the ID of any update panels refreshed during the call, including panels refreshed programmatically.</summary>
			PanelsToRefreshIDs,
			/// <summary>Stores the URL of the action form.</summary>
			FormAction,
			/// <summary>Stores any extra information generated on the server to be consumed by client components.</summary>
			DataItem,
			/// <summary>Stores any extra JSON serialized information generated on the server to be consumed by client components.</summary>
			DataItemJson,
			/// <summary>Stores a dispose script added to the response using the RegisterDispose method on the script manager. The specified script is executed on the client for the specified DOM element.</summary>
			ScriptDispose,
			/// <summary>Stores the new URL to reach in case of a redirection.</summary>
			PageRedirect,
			/// <summary>Stores error information in case an exception is raised during the postback.</summary>
			Error,
			/// <summary>Stores the new title of the page.</summary>
			PageTitle,
			/// <summary>Stores the ID of the new control holding the input focus.</summary>
			Focus,
		}

		/// <summary>
		/// The Delta Node
		/// </summary>
		public class DeltaNode
		{
			/// <summary>
			/// The Delta type
			/// </summary>
			public readonly DeltaType DeltaType;

			/// <summary>
			/// the variable
			/// </summary>
			public readonly string Variable;

			/// <summary>
			/// The value
			/// </summary>
			public readonly string Value;

			/// <summary>
			/// create a new object with given attributes
			/// </summary>
			/// <param name="deltaType"></param>
			/// <param name="variable"></param>
			/// <param name="value"></param>
			public DeltaNode(string deltaType, string variable, string value)
				: this(DeltaNodeConverter(deltaType), variable, value)
			{
			}

			/// <summary>
			/// create a new object with given attributes
			/// </summary>
			/// <param name="deltaType"></param>
			/// <param name="variable"></param>
			/// <param name="value"></param>
			public DeltaNode(DeltaType deltaType, string variable, string value)
			{
				this.DeltaType = deltaType;
				this.Variable = variable;
				this.Value = value;
			}

			/// <summary>
			/// convert string to DeltaType
			/// </summary>
			/// <param name="deltaType"></param>
			/// <returns></returns>
			private static DeltaType DeltaNodeConverter(string deltaType)
			{
#if !SILVERLIGHT
				return (DeltaType)Enum.Parse(typeof(DeltaType), deltaType);
#else
				return (DeltaType)Enum.Parse(typeof(DeltaType), deltaType, true);
#endif
			}

			/// <summary>
			/// return string presentation of object
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return string.Format("{1}={2} (DeltaType={0})", DeltaType, Variable, Value);
			}
		}


		/// <summary>
		/// parse a string into DataBlock used in AJAX Postback.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DeltaNode[] Parse(string value)
		{
			List<DeltaNode> results = new List<DeltaNode>();
			int pos = 0;

			while (pos < value.Length)
			{
				int len = Convert.ToInt32(TestRoutine.GetNextToken(value, ref pos, Delimiter));
				// advance past the delimiter
				pos++;

				string deltaType = TestRoutine.GetNextToken(value, ref pos, Delimiter);
				pos++;

				string variable = TestRoutine.GetNextToken(value, ref pos, Delimiter);
				pos++;

				string val = value.Substring(pos, len);
				// advance past the delimiter
				pos += len + 1;

				#region Sanity Checks
				if (val.Length != len)
					throw new ArgumentOutOfRangeException("incorrect length");
				#endregion

				results.Add(new DeltaNode(deltaType, variable, val));
			}

			return results.ToArray();
		}

#if !SILVERLIGHT
		/// <summary>
		/// Convert DeltaNodes to DataTable
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		public static DataTable DeltaNodesToDataTable(DeltaNode[] nodes)
		{
			try
			{
				DataTable dt = new DataTable("DeltaNodes");
				dt.Columns.Add("NodeID", typeof(int)).AutoIncrement = true;
				dt.Columns.Add("DeltaType", typeof(string));
				dt.Columns.Add("Variable", typeof(string));
				dt.Columns.Add("Value", typeof(string));

				for (int i = 0; i < nodes.Length; i++)
				{
					DataRow dr = dt.NewRow();
					DeltaNode node = nodes[i];

					dr["DeltaType"] = node.DeltaType;
					dr["Variable"] = node.Variable;
					dr["Value"] = node.Value;

					dt.Rows.Add(dr);
				}

				return dt;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				//DebuggerTool.AddData(ex, "nodes", Debugger
				throw;
			}
		}
#endif
	}
}
