using System;
using System.Numerics;

using Plotly;

namespace InteropDrawing.Backends
{
    class _PlotlyDrawing3DContext : Transforms.Decompose3D.PassThrough, IDisposableScene3D
    {
        #region lifecycle

        public _PlotlyDrawing3DContext(PlotlyDocumentBuilder owner)
        {
            _Owner = owner;
            _Content = new _PlotlyMeshBuilder();

            SetPassThroughTarget(_Content);
        }

        public void Dispose()
        {
            SetPassThroughTarget(null);

            var content = System.Threading.Interlocked.Exchange(ref _Content, null);            

            if (content == null) return;

            // commit

            foreach (var trace in content.ToTraces())
            {
                _Owner.AppendTrace(trace);
            }
        }

        #endregion

        #region data

        private readonly PlotlyDocumentBuilder _Owner;        

        private _PlotlyMeshBuilder _Content;        

        #endregion        
    }


    
}
