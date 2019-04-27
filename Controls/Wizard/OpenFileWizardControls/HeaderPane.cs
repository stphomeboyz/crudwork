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
using crudwork.Controls;

namespace crudwork.Controls.Wizard.OpenFileWizardControls
{
	internal partial class HeaderPane : UserControlEx
	{
		public HeaderPane()
		{
			InitializeComponent();

			this.BrushType = BrushType.BrushTypeHatchFill;
			this.Color1 = Color.White;
			this.HatchStyle = System.Drawing.Drawing2D.HatchStyle.LightHorizontal;

			hrSep.BrushType = BrushType.BrushTypeLinearGradient;
			hrSep.Color1 = Color.RoyalBlue;
			hrSep.Color2 = Color.Transparent;
			this.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
		}
	}
}
