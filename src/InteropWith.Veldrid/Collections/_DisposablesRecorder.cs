using System;
using System.Collections.Generic;
using System.Text;

namespace InteropWith
{
    sealed class _DisposablesRecorder : IDisposable
    {
        public _DisposablesRecorder() { }

        public void Dispose()
        {
            var hhh = System.Threading.Interlocked.Exchange(ref _Disposables, null);
            if (hhh != null) { foreach (var item in hhh) item.Dispose(); }
        }

        private HashSet<IDisposable> _Disposables = new HashSet<IDisposable>();

        public T Record<T>(T disposable) where T:IDisposable
        {
            if (_Disposables == null) _Disposables = new HashSet<IDisposable>();
            _Disposables.Add(disposable);
            return disposable;
        }

        public void Record<T>(IEnumerable<T> disposables) where T : IDisposable
        {
            foreach (var d in disposables) Record<T>(d);
        }

        public void DisposeOf<T>(T disposable) where T:IDisposable
        {
            _Disposables?.Remove(disposable);

            disposable.Dispose();            
        }
    }
}
