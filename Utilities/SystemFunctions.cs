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
using System.Runtime.InteropServices;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif

namespace crudwork.Utilities
{
	/// <summary>
	/// System Functions: LogOff, Restart, Shutdown, Hibernate, Standby
	/// </summary>
	public static class SystemFunctions
	{
		[DllImport("user32.dll")]
		private static extern void LockWorkStation();

		[DllImport("user32.dll")]
		private static extern int ExitWindowsEx(int uFlags, int dwReason);

		private enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

		[DllImport("Shell32.dll",CharSet=CharSet.Unicode)]
        private static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

#if !SILVERLIGHT
		//[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
		//private static extern int MinimizeAll();

		/// <summary>
		/// Minimize all application (ie, the "Show Desktop" button)
		/// </summary>
		public static void MinimizeAll()
		{
			var s = new Shell32.ShellClass();
			s.MinimizeAll();
		}
#endif

		/// <summary>
		/// Empty the System Recycle Bin
		/// </summary>
		public static uint EmptyRecycleBin()
		{
			return SHEmptyRecycleBin(IntPtr.Zero, null, 0);
		}

		/// <summary>
		/// Lock the workstation
		/// </summary>
		public static void Lock()
		{
			LockWorkStation();
		}

		/// <summary>
		/// Log off the user
		/// </summary>
		/// <param name="force"></param>
		public static void LogOff(bool force)
		{
			ExitWindowsEx(force ? 4 : 0, 0);
		}

		/// <summary>
		/// Reboot the system
		/// </summary>
		public static void Reboot()
		{
			ExitWindowsEx(2, 0);
		}

		/// <summary>
		/// Shutdown the system
		/// </summary>
		public static void Shutdown()
		{
			ExitWindowsEx(1, 0);
		}

#if !SILVERLIGHT
		/// <summary>
		/// Put the system into hibernate mode
		/// </summary>
		public static void Hibernate()
		{
			Hibernate(true, true);
		}

		/// <summary>
		/// Put the system into hibernate mode
		/// </summary>
		/// <param name="force"></param>
		/// <param name="disableWakeupEvent"></param>
		public static void Hibernate(bool force, bool disableWakeupEvent)
		{
			Application.SetSuspendState(PowerState.Hibernate, force, disableWakeupEvent);
		}

		/// <summary>
		/// Put the system into standby mode
		/// </summary>
		public static void Standby()
		{
			Standby(true, true);
		}

		/// <summary>
		/// Put the system into standby mode
		/// </summary>
		/// <param name="force"></param>
		/// <param name="disableWakeupEvent"></param>
		public static void Standby(bool force, bool disableWakeupEvent)
		{
			Application.SetSuspendState(PowerState.Suspend, force, disableWakeupEvent);
		} 
#endif
	}
}
