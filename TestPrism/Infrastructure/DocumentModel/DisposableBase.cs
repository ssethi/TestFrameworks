using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DocumentModel
{
	/// <summary>
	/// Convenience base for objects which need to be Disposable
	/// and may have hierarchies of objects derive from them
	/// which also need to correctly implement the Disposable
	/// pattern.
	/// </summary>
	public abstract class DisposableBase : IDisposable
	{
		bool disposed = false;

		public bool IsDisposed { get { return disposed; } }

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		~DisposableBase()
		{
			this.Dispose(false);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
				return;

			if (disposing)
			{
				try
				{
					DisposeManaged();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(String.Format("Error {0}: {1}", this.GetType().Name + ".DisposeManaged", ex));
				}
			}
			try
			{
				DisposeUnmanaged();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(String.Format("Error {0}: {1}", this.GetType().Name + ".DisposeUnmanaged", ex));
			}
		}

		protected void CheckDisposed()
		{
			if (this.disposed)
				throw new ObjectDisposedException(
					this.GetType().Name);
		}

		/// <summary>
		/// When overridden in a derived class, call Dispose 
		/// on IDisposable members, null arrays and collections.
		/// Must call base.DisposeManaged() before exit.
		/// </summary>
		protected virtual void DisposeManaged()
		{
		}

		/// <summary>
		/// When overridden in a derived class, frees allocated
		/// pointers, releases handles and frees COM objects.
		/// Must call base.DisposeUnmanaged() before exit.
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
		}
	}
}
