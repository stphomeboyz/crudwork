using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace crudwork.Utilities
{
	/// <summary>
	/// Tools for Silverlight application
	/// </summary>
	public static class SilverlightTools
	{
		/// <summary>
		/// Return true if the Application Storage is enable; otherwise, return false.
		/// </summary>
		public static bool IsApplicationStorageEnabled
		{
			get
			{
				try
				{
					var asx = IsolatedStorageSettings.ApplicationSettings;
					return true;
				}
				catch
				{
					return false;
				}
			}
			/*set
			{
			}*/
		}
	}
}
