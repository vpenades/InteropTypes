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
            foreach (var item in _Disposables) item.Dispose();
            _Disposables.Clear();
        }

        private readonly HashSet<IDisposable> _Disposables = new HashSet<IDisposable>();

        public T Record<T>(T disposable) where T:IDisposable
        {
            _Disposables.Add(disposable);
            return disposable;
        }

        public void Record<T>(IEnumerable<T> disposables) where T : IDisposable
        {
            foreach (var d in disposables) Record<T>(d);
        }

        public void DisposeOf<T>(T disposable) where T:IDisposable
        {
            _Disposables.Remove(disposable);

            disposable.Dispose();            
        }
    }
}
