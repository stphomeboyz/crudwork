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
using System.Drawing.Drawing2D;

namespace crudwork.Controls
{
	/// <summary>
	/// Display a horizontal line
	/// </summary>
	public partial class UserControlEx : UserControl
	{
		private Bitmap bgImage = null;
		private string curSetting = string.Empty;

		/// <summary>
		/// create new instance with default attributes
		/// </summary>
		public UserControlEx()
		{
			InitializeComponent();
		}

		private void UserControlEx_Paint(object sender, PaintEventArgs e)
		{
			if (curSetting == this.ToString())
				return;

			curSetting = this.ToString();
			this.BackgroundImage = CreateBackgroundImage();
			this.BackgroundImageLayout = ImageLayout.Stretch;
		}



		private Rectangle ClientRect
		{
			get
			{
				return new Rectangle(new Point(0, 0), this.Size);
			}
		}
		private Bitmap CreateBackgroundImage()
		{
			if (bgImage != null)
			{
				bgImage.Dispose();
				bgImage = null;
			}

			bgImage = new Bitmap(this.Size.Width, this.Size.Height);

			using (var g = Graphics.FromImage(bgImage))
			using (var brush = CreateBrush())
			{
				g.FillRectangle(brush, ClientRect);
			}

			return bgImage;
		}
		private Brush CreateBrush()
		{
			Brush brush = null;
			switch (BrushType)
			{
				case BrushType.CustomBrush:
					brush = CustomBrush;
					break;

				case BrushType.BrushTypeSolidColor:
					brush = NewSolidBrush();
					break;
				case BrushType.BrushTypeHatchFill:
					brush = NewHatchBrush();
					break;
				case BrushType.BrushTypeTextureFill:
					brush = NewTextureBrush();
					break;
				case BrushType.BrushTypePathGradient:
					brush = NewPathGradientBrush();
					break;
				case BrushType.BrushTypeLinearGradient:
					brush = NewLinearGradientBrush();
					break;

				default:
					throw new ArgumentOutOfRangeException("BrushType=" + BrushType);
			}
			return brush;
		}

		#region Internal Brush creator methods
		private LinearGradientBrush NewLinearGradientBrush()
		{
			return new LinearGradientBrush(ClientRect, Color1, Color2, LinearGradientMode);
		}

		private PathGradientBrush NewPathGradientBrush()
		{
			throw new NotImplementedException();
		}

		private TextureBrush NewTextureBrush()
		{
			throw new NotImplementedException();
		}

		private HatchBrush NewHatchBrush()
		{
			return new HatchBrush(HatchStyle, Color1, Color2);
		}

		private SolidBrush NewSolidBrush()
		{
			return new SolidBrush(Color1);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get or set a custom-made brush
		/// </summary>
		[Category("Custom Settings"), Description("Get or set a customized brush"), DefaultValue(null)]
		public Brush CustomBrush
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the brush type
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the hatch style"), DefaultValue(HatchStyle.Plaid)]
		public HatchStyle HatchStyle
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the brush type
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the brush type"), DefaultValue(BrushType.BrushTypeLinearGradient)]
		public BrushType BrushType
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set linear gradient mode
		/// </summary>
		[Category("Custom Settings"), Description("Get or set linear gradient mode"), DefaultValue(LinearGradientMode.ForwardDiagonal)]
		public LinearGradientMode LinearGradientMode
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the color #1
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the color #1"), DefaultValue(BrushType.BrushTypeSolidColor)]
		public Color Color1
		{
			get;
			set;
		}

		/// <summary>
		/// Get or set the color #2
		/// </summary>
		[Category("Custom Settings"), Description("Get or set the color #2"), DefaultValue(BrushType.BrushTypeSolidColor)]
		public Color Color2
		{
			get;
			set;
		}

		#endregion

		/// <summary>
		/// return a string presentation of this object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("HatchStyle={0} BrushType={1} LinearGradientMode={2} Color1={3} Color2={4} Size={5}",
				HatchStyle, BrushType, LinearGradientMode, Color1, Color2, this.Size);
		}
	}

	/// <summary>
	/// The BrushType enumeration indicates the type of brush. There are five types of brushes.
	/// </summary>
	/// <remarks>enumerator taken from GDI+ (reference -- ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/gdicpp/GDIPlus/GDIPlusreference/enumerations/brushtype.htm)</remarks>
	public enum BrushType
	{
		/// <summary>Custom Brush</summary>
		CustomBrush = -1,
		/// <summary>Indicates a brush of type SolidBrush. A solid brush paints a single, constant color that can be opaque or transparent.</summary>
		BrushTypeSolidColor = 0,
		/// <summary>Indicates a brush of type HatchBrush. A hatch brush paints a background and paints, over that background, a pattern of lines, dots, dashes, squares, crosshatch, or some variation of these. The hatch brush consists of two colors: one for the background and one for the pattern over the background. The color of the background is called the background color, and the color of the pattern is called the foreground color.</summary>
		BrushTypeHatchFill = 1,
		/// <summary>Indicates a brush of type TextureBrush. A texture brush paints an image. The image or texture is either a portion of a specified image or a scaled version of a specified image. The type of image (metafile or nonmetafile) determines whether the texture is a portion of the image or a scaled version of the image.</summary>
		BrushTypeTextureFill = 2,
		/// <summary>Indicates a brush of type PathGradientBrush. A path gradient brush paints a color gradient in which the color changes from a center point outward to a boundary that is defined by a closed curve or path. The color gradient has one color at the center point and one or multiple colors at the boundary.</summary>
		BrushTypePathGradient = 3,
		/// <summary>Indicates a brush of type LinearGradientBrush. A linear gradient brush paints a color gradient in which the color changes evenly from the starting boundary line of the linear gradient brush to the ending boundary line of the linear gradient brush. The boundary lines of a linear gradient brush are two parallel straight lines. The color gradient is perpendicular to the boundary lines of the linear gradient brush, changing gradually across the stroke from the starting boundary line to the ending boundary line. The color gradient has one color at the starting boundary line and another color at the ending boundary line.</summary>
		BrushTypeLinearGradient = 4
	}

}
