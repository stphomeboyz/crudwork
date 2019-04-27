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
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Diagnostics;

namespace crudwork.Utilities
{
	/// <summary>
	/// Windows Service Utility
	/// </summary>
	public static class WindowsServiceUtil
	{
		/// <summary>
		/// Change the service status
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="newStatus"></param>
		public static void RunServer(ServiceController sc, ServiceControllerStatus newStatus)
		{
			try
			{
				if (sc.Status == newStatus)
					return;

				// TODO: Need better waiting mechanism (ideally show a progress bar here...)
				// for now wait 30 seconds and confirm the new status afterward.
				var waitAmount = new TimeSpan(0, 0, 30);

				switch (newStatus)
				{
					case ServiceControllerStatus.Running:
						//Status("Starting server, please wait...");
						sc.Start();
						sc.WaitForStatus(newStatus, waitAmount);
						break;

					case ServiceControllerStatus.Stopped:
						//Status("Stopping server, please wait...");
						sc.Stop();
						sc.WaitForStatus(newStatus, waitAmount);
						break;

					default:
						throw new Exception("Unsupported action = " + newStatus.ToString());
				}

				if (sc.Status != newStatus)
					throw new ApplicationException("Service is not " + newStatus);

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

	}
}
