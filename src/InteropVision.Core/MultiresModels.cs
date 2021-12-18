using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropVision
{
    public class MultiresModels : IDisposable
    {

        #region lifecycle

        public MultiresModels() { }

        public MultiresModels(Func<string, IModelGraph> modelLoader)
        {
            _ModelLoader = modelLoader;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var mdl in _Models) mdl.Dispose();
                    _Models.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MultiresMoldels()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool disposedValue;

        #endregion

        #region data

        private readonly Func<string, IModelGraph> _ModelLoader;

        private readonly List<_ResModel> _Models = new List<_ResModel>();

        public IEnumerable<(int Width, int Height)> InputResolutions => _Models.Select(item => (item.Width, item.Height));

        #endregion

        #region API

        public void Register(int w, int h, string modelPath)
        {
            var m = new _ResModel(w, h, () => _ModelLoader(modelPath));
            _Models.Add(m);
        }

        public void Register(int w, int h, Func<IModelGraph> modelActivator)
        {
            var m = new _ResModel(w, h, modelActivator);
            _Models.Add(m);
        }

        public IModelGraph UseModel(int w, int h)
        {
            return _FindClosestResolution(w, h).UseModel();
        }

        public IModelSession UseSession(int w, int h)
        {
            return _FindClosestResolution(w, h).UseSession();
        }

        private _ResModel _FindClosestResolution(int w, int h)
        {
            // find exact
            foreach (var m in _Models)
            {
                if (m.Width == w && m.Height == h) return m;
            }

            // smallest of largers

            var larger = _Models
                .Where(item => item.Width >= w && item.Height >= h)
                .OrderBy(item => item.Area)
                .FirstOrDefault();

            if (larger != null) return larger;

            var smaller = _Models
                .OrderByDescending(item => item.Area)
                .FirstOrDefault();

            return smaller;
        }

        #endregion

        #region nested types

        sealed class _ResModel : IDisposable
        {
            #region lifecycle
            public _ResModel(int w, int h, Func<IModelGraph> modelActivator)
            {
                // var basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                // _ModelPath = System.IO.Path.Combine(basePath, p);

                _ModelActivator = modelActivator;

                Width = w;
                Height = h;
            }

            public void Dispose()
            {
                System.Threading.Interlocked.Exchange(ref _Session, null)?.Dispose();
                System.Threading.Interlocked.Exchange(ref _Model, null)?.Dispose();
            }

            #endregion

            #region data

            public int Width { get; }
            public int Height { get; }

            public int Area => Width * Height;            

            private readonly Func<IModelGraph> _ModelActivator;

            private IModelGraph _Model;
            private IModelSession _Session;

            #endregion

            #region API

            public IModelGraph UseModel()
            {                
                if (_Model == null) _Model = _ModelActivator.Invoke();
                return _Model;
            }

            public IModelSession UseSession()
            {
                if (_Session == null) _Session = UseModel().CreateSession();
                return _Session;
            }

            #endregion
        }

        #endregion
    }
}
