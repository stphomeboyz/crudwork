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
using crudwork.Utilities;
using crudwork.Models;
using System.Runtime.InteropServices;

namespace crudwork.FileImporters.Specialized
{
	/// <summary>
	/// Create a new instance of ADOX.CatalogClass unmanaged resource
	/// </summary>
	internal class ADOX_CatalogClass : DisposableObject
	{
		public ADOX.CatalogClass CatalogClass
		{
			get;
			private set;
		}

		public ADOX_CatalogClass()
		{
			CatalogClass = new ADOX.CatalogClass();
		}

		protected override void Dispose(bool disposeManagedResources)
		{
			if (disposeManagedResources)
			{
				// dispose any managed resources here...
			}

			// dispose any unmanaged resources here...
			Marshal.ReleaseComObject(CatalogClass);
			Marshal.FinalReleaseComObject(CatalogClass);
			CatalogClass = null;
		}
	}
}
