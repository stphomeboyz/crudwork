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
using System.Windows.Forms;
using crudwork.Utilities;

namespace crudwork.Controls
{
	/// <summary>
	/// FlowLayoutPanel extended version - supports scrolling
	/// </summary>
	public class ExtendedFlowLayoutPanel : FlowLayoutPanel
	{
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public ExtendedFlowLayoutPanel()
		{
			base.Dock = DockStyle.Fill;
			base.AutoScroll = true;
			base.Scroll += new ScrollEventHandler(ExtendedFlowLayoutPanel_Scroll);
		}

		private void ExtendedFlowLayoutPanel_Scroll(object sender, ScrollEventArgs e)
		{
			try
			{
				switch (e.ScrollOrientation)
				{
					case ScrollOrientation.HorizontalScroll:
						base.HorizontalScroll.Value = e.NewValue;
						break;
					case ScrollOrientation.VerticalScroll:
						base.VerticalScroll.Value = e.NewValue;
						break;
				}
			}
			catch (Exception ex)
			{
				FormUtil.WinException(ex, "FlowLayoutPanel Scroll");
			}
		}
	}
}
