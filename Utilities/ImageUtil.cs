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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace crudwork.Utilities
{
	/// <summary>
	/// Image Utility
	/// </summary>
	public static class ImageUtil
	{
		#region Enumerators
		#endregion

		#region Fields
		private static int thumbWidth = 96;
		private static int thumbHeight = 96;
		#endregion

		#region Constructors
		#endregion

		#region Event Methods

		#region System Event Methods
		#endregion

		#region Application Event Methods
		#endregion

		#region Custom Event Methods
		#endregion

		#endregion

		#region Public Methods
		/// <summary>
		/// Create thumbnail from an image filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static Image GetThumbnail(string filename)
		{
			return GetThumbnail(Image.FromFile(filename), thumbWidth, thumbHeight);
		}

		/// <summary>
		/// Create thumbnail with the given size from an image filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Image GetThumbnail(string filename, int width, int height)
		{
			return GetThumbnail(Image.FromFile(filename), width, height);
		}

		/// <summary>
		/// Create thumbnail from an Image type.
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static Image GetThumbnail(Image image)
		{
			return GetThumbnail(image, thumbWidth, thumbHeight);
		}

		/// <summary>
		/// Create thumbnail with the given size for an Image type.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Image GetThumbnail(Image image, int width, int height)
		{
			if (image == null)
				throw new ArgumentNullException("image");

			return image.GetThumbnailImage(width, height, null, new IntPtr());
		}

		/// <summary>
		/// Read the binary content from a file into a byte array.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static byte[] FileToByteArray(string filename)
		{
			using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				byte[] imageData = new Byte[fs.Length];
				fs.Read(imageData, 0, imageData.Length);
				fs.Close();
				return imageData;
			}
		}

		/// <summary>
		/// Convert a byte array to Image type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Image ByteArrayToImage(byte[] value)
		{
			if ((value == null) || (value.Length == 0))
				throw new ArgumentNullException("value");

			return Image.FromStream(new MemoryStream(value));
		}

		/// <summary>
		/// Create a thumbnail image from an image filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static byte[] CreateThumbnail(string filename)
		{
			using (MemoryStream s = new MemoryStream())
			using (Image image = Image.FromFile(filename).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
			{
				image.Save(s, ImageFormat.Jpeg);
				return s.ToArray();
			}
		}

		/// <summary>
		/// Create a thumbnail image from a byte array, presumely from an image. 
		/// </summary>
		/// <param name="imageData"></param>
		/// <returns></returns>
		public static byte[] CreateThumbnail(byte[] imageData)
		{
			using (MemoryStream s = new MemoryStream())
			using (Image image = Image.FromStream(new MemoryStream(imageData)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
			{
				image.Save(s, ImageFormat.Jpeg);
				return s.ToArray();
			}
		}

		/// <summary>
		/// Convert an image to a byte array
		/// </summary>
		/// <param name="imageToConvert"></param>
		/// <param name="formatOfImage"></param>
		/// <returns></returns>
		public static byte[] ConvertImageToByteArray(Image imageToConvert, ImageFormat formatOfImage)
		{
			try
			{
				using (MemoryStream ms = new MemoryStream())
				{
					imageToConvert.Save(ms, formatOfImage);
					return ms.ToArray();
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "imageToConvert", DebuggerTool.Dump(imageToConvert));
				DebuggerTool.AddData(ex, "formatOfImage", DebuggerTool.Dump(formatOfImage));
				throw;
			}
		}

		/// <summary>
		/// Return the ImageFormat type based on the file's extension
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(string filename)
		{
			string ext = new FileInfo(filename).Extension.Substring(1);
			switch (ext.ToUpper())
			{
				case "BMP":
					return ImageFormat.Bmp;
				//case "":
				//	return ImageFormat.Emf;
				//case "":
				//	return ImageFormat.Exif;
				case "GIF":
					return ImageFormat.Gif;
				case "ICO":
					return ImageFormat.Icon;
				case "JPEG":
				case "JPG":
				case "JPE":
					return ImageFormat.Jpeg;
				//case "":
				//	return ImageFormat.MemoryBmp;
				case "PNG":
					return ImageFormat.Png;
				case "TIF":
				case "TIFF":
					return ImageFormat.Tiff;
				case "WMF":
					return ImageFormat.Wmf;
				default:
					return null;

			}
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Properties
		#endregion

		#region Others
		#endregion
	}
}
