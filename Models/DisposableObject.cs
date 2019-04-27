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
//using System.Linq;
using System.Text;
using System.Diagnostics;

namespace crudwork.Models
{
	/// <summary>
	/// Generic disposable class - to clean up managed and unmanaged resources.
	/// Inspired by Dispose Pattern: http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx
	/// </summary>
	/// <remarks>
	/// Inherit from this class if your class contains managed/unmanaged resources that needs to be
	/// disposed properly.
	/// </remarks>
	public abstract class DisposableObject : IDisposable
	{
		private bool disposed = false;

		/// <summary>
		/// finalizer to clean up unmanaged resource(s)
		/// </summary>
		/// <remarks>
		/// The ~Finalize is raised to dispose unmanaged resourses if, and only if, the
		/// Dispose() method were not explicitly called; or, if the Dispose() method were
		/// not called implicitly by the "using" statement.
		/// 
		/// Having this ~Finalize does NOT create any overhead at all.  The Dispose() method,
		/// if called, contains a GC.SuppressFinalize() statement to suppress this event.
		/// Therefore, having this ~Finalize helps clean up the resources more thoroughly.
		/// </remarks>
		~DisposableObject()
		{
			// At this point, all managed resources should already have been disposed.
			// We will dispose only the unmanaged resource.
			RunDispose(false);
		}

		/// <summary>
		/// when overridden, this method will dispose both managed and unmanaged resources
		/// </summary>
		/// <example>
		/// 		protected virtual void Dispose(bool disposeManagedResources)
		/// 		{
		/// 			if (disposeManagedResources)
		/// 			{
		/// 				// dispose any managed resources here...
		/// 			}
		/// 			
		/// 			// dispose any unmanaged resources here...
		/// 		}
		/// </example>
		/// <param name="disposeManagedResources">true = dispose managed and unmanaged resources; false = dispose unmanaged resources only</param>
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (disposeManagedResources)
			{
				// dispose any managed resources here...
			}

			// dispose any unmanaged resources here...
		}

		/// <summary>
		/// Wrap the Dispose(bool) method to never throw exception.  Throwing exception(s)
		/// at this stage will prevent the system from cleaning up resources properly.
		/// </summary>
		/// <param name="disposeManagedResources"></param>
		private void RunDispose(bool disposeManagedResources)
		{
			if (disposed)
				return;  /* nothing left to do */

			try
			{
				Dispose(disposeManagedResources);
			}
			catch (Exception ex)
			{
#if !SILVERLIGHT
				Trace.WriteLine(ex.ToString());
#endif
				// never throw exception here...
			}
			finally
			{
				disposed = true;
			}
		}

		#region IDisposable Members
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// dispose both managed and unmanaged resources.  Then suppress the ~Finalize from being called.
			RunDispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose();
		}
		#endregion
	}

#if !SILVERLIGHT
	/// <summary>
	/// Generic disposable class - to clean up managed and unmanaged resources.
	/// Inspired by Dispose Pattern: http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx
	/// <para>
	/// This is the same as DisposableObject; but, for objects that needs to implement the MarshalByRefObject class.
	/// </para>
	/// <remarks>
	/// Inherit from this class if your class contains managed/unmanaged resources that needs to be
	/// disposed properly AND if your class is used for .NET remoting.
	/// </remarks>
	/// </summary>
	public abstract class DisposableMBRObject : MarshalByRefObject, IDisposable
	{
		private bool disposed = false;

		/// <summary>
		/// finalizer to clean up unmanaged resource(s)
		/// </summary>
		/// <remarks>
		/// The ~Finalize is raised to dispose unmanaged resourses if, and only if, the
		/// Dispose() method were not explicitly called; or, if the Dispose() method were
		/// not called implicitly by the "using" statement.
		/// 
		/// Having this ~Finalize does NOT create any overhead at all.  The Dispose() method,
		/// if called, contains a GC.SuppressFinalize() statement to suppress this event.
		/// Therefore, having this ~Finalize helps clean up the resources more thoroughly.
		/// </remarks>
		~DisposableMBRObject()
		{
			// At this point, all managed resources should already have been disposed.
			// We will dispose only the unmanaged resource.
			RunDispose(false);
		}

		/// <summary>
		/// when overridden, this method will dispose both managed and unmanaged resources
		/// </summary>
		/// <example>
		/// 		protected virtual void Dispose(bool disposeManagedResources)
		/// 		{
		/// 			if (disposeManagedResources)
		/// 			{
		/// 				// dispose any managed resources here...
		/// 			}
		/// 			
		/// 			// dispose any unmanaged resources here...
		/// 		}
		/// </example>
		/// <param name="disposeManagedResources">true = dispose managed and unmanaged resources; false = dispose unmanaged resources only</param>
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (disposeManagedResources)
			{
				// dispose any managed resources here...
			}

			// dispose any unmanaged resources here...
		}

		/// <summary>
		/// Wrap the Dispose(bool) method to never throw exception.  Throwing exception(s)
		/// at this stage will prevent the system from cleaning up resources properly.
		/// </summary>
		/// <param name="disposeManagedResources"></param>
		private void RunDispose(bool disposeManagedResources)
		{
			if (disposed)
				return;  /* nothing left to do */

			try
			{
				Dispose(disposeManagedResources);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.ToString());
				// never throw exception here...
			}
			finally
			{
				disposed = true;
			}
		}

	#region IDisposable Members
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// dispose both managed and unmanaged resources.  Then suppress the ~Finalize from being called.
			RunDispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose();
		}
		#endregion
	}
#endif
}
