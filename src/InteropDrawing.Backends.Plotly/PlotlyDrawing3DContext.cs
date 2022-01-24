using System;
using System.Numerics;

using Plotly;

namespace InteropDrawing.Backends
{
    class _PlotlyDrawing3DContext : IDisposableDrawing3D
    {
        #region lifecycle
        public _PlotlyDrawing3DContext(PlotlyDocumentBuilder owner)
        {
            _Owner = owner;
            _Content = new _PlotlyMeshBuilder();

            _Collapse =  new Transforms.Decompose3D(this);
        }
        public void Dispose()
        {
            if (_Content != null)
            {
                foreach(var trace in _Content.ToTraces())
                {
                    _Owner.AppendTrace(trace);
                }
            }

            _Content = null;
        }

        #endregion

        #region data

        private readonly PlotlyDocumentBuilder _Owner;
        private readonly Transforms.Decompose3D _Collapse;

        private _PlotlyMeshBuilder _Content;        

        #endregion

        #region API

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));

            _Collapse.DrawAsset(transform, asset, style);
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));

            if (style.Style.HasFill) { style = style.With(style.Style.WithOutline(0)); }

            _Collapse.DrawSegment(a, b, diameter, style);
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));            

            if (style.HasFill) { style = style.WithOutline(0); }

            _Collapse.DrawSphere(center, diameter, style);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));
            _Content.DrawSurface(vertices, style);
        }        

        #endregion
    }


    
}
