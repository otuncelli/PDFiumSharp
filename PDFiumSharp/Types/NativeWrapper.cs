using System;

namespace PDFiumSharp.Types
{
    public abstract class NativeWrapper<T> : IDisposable where T : unmanaged, IHandle<T>
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private T handle;
#pragma warning restore IDE0044 // Add readonly modifier

        /// <summary>
        /// Handle which can be used with the native <see cref="PDFium"/> functions.
        /// </summary>
        public T Handle
		{
			get
			{
				if (handle.IsNull)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return handle;
			}
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="IDisposable.Dispose"/> was already
		/// called on this instance.
		/// </summary>
		public bool IsDisposed => handle.IsNull;

		protected NativeWrapper(T handle)
		{
			if (handle.IsNull)
			{
				throw new PDFiumException();
			}
			this.handle = handle;
		}

		/// <summary>
		/// Implementors should clean up here. This method is guaranteed to only be called once.
		/// </summary>
		protected virtual void Dispose(T handle) { }

		void IDisposable.Dispose()
		{
			var oldHandle = handle.SetToNull();
			if (!oldHandle.IsNull)
			{
				Dispose(oldHandle);
			}
		}
	}
}
