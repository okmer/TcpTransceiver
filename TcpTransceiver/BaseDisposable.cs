using System;

namespace Com.Okmer.Communication
{
    public abstract class BaseDisposable : IDisposable
    {
        bool disposed = false;

        ~BaseDisposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DisposeManaged();
            }

            DisposeUnmanaged();

            disposed = true;
        }

        protected abstract void DisposeManaged();

        protected virtual void DisposeUnmanaged() {}
    }
}