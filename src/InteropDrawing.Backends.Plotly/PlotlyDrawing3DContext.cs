using System;
using System.Numerics;


namespace InteropDrawing.Backends
{
    class _PlotlyDrawing3DContext : IDrawingContext3D
    {
        #region lifecycle
        public _PlotlyDrawing3DContext(PlotlyDocumentBuilder owner)
        {
            _Owner = owner;
            _Content = new _PlotlyMeshBuilder();
        }
        public void Dispose()
        {
            if (_Content != null)
            {
                var trace = _Content.ToTrace();

                _Owner.AppendTrace(trace);
            }

            _Content = null;
        }

        #endregion

        #region data

        private readonly PlotlyDocumentBuilder _Owner;
        private _PlotlyMeshBuilder _Content;

        #endregion

        #region API

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));

            if (asset is Model3D model3) model3.DrawTo(this.CreateTransformed3D(transform));
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));

            if (style.Style.HasFill)
            {
                style = style.With(style.Style.WithOutline(0));
            }

            _Content.DrawCylinderAsSurfaces(a, diameter, b, diameter, 6, style);
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));

            if (style.HasFill)
            {
                style = style.WithOutline(0);
            }

            _Content.DrawSphereAsSurfaces(center, diameter, 3, style);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            if (_Content == null) throw new ObjectDisposedException(nameof(_Content));
            _Content.DrawSurface(vertices, style);
        }        

        #endregion
    }


    
}
