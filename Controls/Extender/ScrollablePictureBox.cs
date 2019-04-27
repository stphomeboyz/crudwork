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

namespace crudwork.Controls
{
	/// <summary>
	/// PictureBox extended version - support for scrollable
	/// </summary>
	public partial class ScrollablePictureBox : UserControl
	{
		/// <summary>
		/// Create new instance with default attribute
		/// </summary>
		public ScrollablePictureBox()
		{
			InitializeComponent();
		}

		#region Events Methods
		private void ScrollablePictureBox_Load(object sender, System.EventArgs e)
		{
		}

		private void ScrollablePictureBox_Resize(Object sender, EventArgs e)
		{
			/* If the PictureBox has an image, see if it needs 
			   scrollbars and refresh the image. */
			if (pictureBox1.Image != null)
			{
				this.DisplayScrollBars();
				this.SetScrollBarValues();
				this.Refresh();
			}
		}
		
		private void pictureBox1_DoubleClick(Object sender, EventArgs e)
		{
			// Open the dialog box so the user can select a new image.
			if(openFileDialog1.ShowDialog() != DialogResult.Cancel)
			{
				// Display the image in the PictureBox.
				Image = Image.FromFile(openFileDialog1.FileName);
			}
		}
 
		private void btnAction_Click(object sender, System.EventArgs e)
		{
		}
		#endregion

		#region Private Methods
		private void HandleScroll(Object sender, ScrollEventArgs se)
		{
			/* Create a graphics object and draw a portion of the image in the PictureBox. */
			Graphics g = pictureBox1.CreateGraphics();

			Size hsb = HorizontalScrollBar();
			Size vsb = VerticalScrollBar();

			Rectangle rect1 = new Rectangle(0, 0, pictureBox1.Right - vsb.Width, pictureBox1.Bottom - hsb.Height);
			Rectangle rect2 = new Rectangle(hScrollBar1.Value, vScrollBar1.Value, pictureBox1.Right - vsb.Width, pictureBox1.Bottom - hsb.Height);

			g.DrawImage(pictureBox1.Image, rect1, rect2, GraphicsUnit.Pixel);

			pictureBox1.Update();
		}

		private Size HorizontalScrollBar()
		{
			Size sz = new Size();
			if (hScrollBar1.Visible)
			{
				sz.Width = hScrollBar1.Width;
				sz.Height = hScrollBar1.Height;
			}
			else
			{
				sz.Width = 0;
				sz.Height = 0;
			}
			return sz;
		}

		private Size VerticalScrollBar()
		{
			Size sz = new Size();
			if (vScrollBar1.Visible)
			{
				sz.Width = vScrollBar1.Width;
				sz.Height = vScrollBar1.Height;
			}
			else
			{
				sz.Width = 0;
				sz.Height = 0;
			}
			return sz;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Display the scrollbars
		/// </summary>
		public void DisplayScrollBars()
		{
			if (pictureBox1.Image == null)
			{
				return;
			}

			Size hsb = HorizontalScrollBar();
			Size vsb = VerticalScrollBar();

			// If the image is wider than the PictureBox, show the HScrollBar.
			hScrollBar1.Visible = !(pictureBox1.Width > pictureBox1.Image.Width - vsb.Width);

			// If the image is taller than the PictureBox, show the VScrollBar.
			vScrollBar1.Visible = !(pictureBox1.Height > pictureBox1.Image.Height - hsb.Height);
		}

		/// <summary>
		/// Set the scrollbar values
		/// </summary>
		public void SetScrollBarValues()
		{
			if (pictureBox1.Image == null)
			{
				return;
			}

			Size hsb = HorizontalScrollBar();
			Size vsb = VerticalScrollBar();

			// Set the Maximum, Minimum, LargeChange and SmallChange properties.
			this.vScrollBar1.Minimum = 0;
			this.hScrollBar1.Minimum = 0;
			// If the offset does not make the Maximum less than zero, set its value. 
			if ((this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width) > 0)
			{
				this.hScrollBar1.Maximum = this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width;
			}
			/* If the VScrollBar is visible, adjust the Maximum of the 
			   HSCrollBar to account for the width of the VScrollBar. */
			if (this.vScrollBar1.Visible)
			{
				this.hScrollBar1.Maximum += this.vScrollBar1.Width;
			}
			//this.hScrollBar1.Maximum += hsb.Width;

			this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 4;
			this.hScrollBar1.SmallChange = this.hScrollBar1.Maximum / 20;
			// Adjust the Maximum value to make the raw Maximum value attainable by user interaction.
			this.hScrollBar1.Maximum += this.hScrollBar1.LargeChange;

			// If the offset does not make the Maximum less than zero, set its value.    
			if ((this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height) > 0)
			{
				this.vScrollBar1.Maximum = this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height;
			}
			/* If the HScrollBar is visible, adjust the Maximum of the 
			   VSCrollBar to account for the width of the HScrollBar.*/
			if (this.hScrollBar1.Visible)
			{
				this.vScrollBar1.Maximum += this.hScrollBar1.Height;
			}
			//this.vScrollBar1.Maximum += hsb.Height;

			this.vScrollBar1.LargeChange = this.vScrollBar1.Maximum / 4;
			this.vScrollBar1.SmallChange = this.vScrollBar1.Maximum / 20;
			// Adjust the Maximum value to make the raw Maximum value attainable by user interaction.
			this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange;
		}
		#endregion

		#region Property Methods
		/// <summary>
		/// Get or set the image
		/// </summary>
		public Image Image
		{
			get
			{
				return pictureBox1.Image;
			}
			set
			{
				try
				{
					pictureBox1.Image = value;

					if (value != null)
					{
						this.DisplayScrollBars();
						this.SetScrollBarValues();
					}
				}
				catch
				{
					// absorb error
				}
			}
		}
		#endregion
	}
}
