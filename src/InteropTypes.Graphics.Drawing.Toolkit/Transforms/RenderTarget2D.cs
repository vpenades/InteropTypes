using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

using XYZ = System.Numerics.Vector3;
using PLANE = System.Numerics.Plane;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    [Obsolete("Use PerspectiveTransform instead.")]
    public delegate XYZ ProjectPointCallback(XYZ worldPoint);

    [Obsolete("Use PerspectiveTransform instead.")]
    class _RenderTarget2D :
        IScene3D,
        IServiceProvider
    {
        #region lifecycle

        /*
        public static void Project(IDrawing2D dc, CameraProjection3D camera, Model3D scene)
        {
            camera.GetProjectionInfo(out ProjectPointCallback projCallback, out PLANE plane, scene);

            var projector = new _RenderTarget2D(dc, projCallback, plane);

            projector.Draw(scene);
        }*/

        private _RenderTarget2D(ICanvas2D dc, ProjectPointCallback prjCallback, PLANE plane)
        {
            _RenderTarget = dc;

            _Proj3Func = prjCallback;

            _FrustumNearPlane = plane; // XYZ must be normalized
            _StrafeVector = plane.Normal.GetAnyPerpendicular().Normalized();
        }

        #endregion

        #region data

        /// <summary>
        /// 2D rendering context
        /// </summary>
        private ICanvas2D _RenderTarget;

        /// <summary>
        /// used to compute thickness
        /// </summary>
        private readonly XYZ _StrafeVector;

        /// <summary>
        /// used to clip the geometry
        /// </summary>
        private readonly PLANE _FrustumNearPlane;

        /// <summary>
        /// projection abstraction, converts points from world space to viewSpace (in pixels)
        /// </summary>
        private readonly ProjectPointCallback _Proj3Func;

        #endregion

        #region API

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType);
        }

        /*
        public void Draw(Model3D batch)
        {
            var drawingOrder = new List<_SortableItem>();

            drawingOrder.Clear();

            for (int i = 0; i < batch.Commands.Count; ++i)
            {
                var center = batch.Commands[i].GetCenter3D(batch.Vectors);

                var item = new _SortableItem
                {
                    Index = i,
                    Distance = _ProjectPoint(center).LengthSquared(),
                };

                drawingOrder.Add(item);
            }

            drawingOrder.Sort();

            foreach (var item in drawingOrder)
            {
                var cmd = batch.Commands[item.Index];

                cmd.DrawTo(this, batch, true); // should be false, and should let the underlaying system to handle the whole thing
            }
        }
        */

        #endregion

        #region core

        internal XYZ _ProjectPoint(XYZ p) { return _Proj3Func(p); }

        private float _ProjectRadius(XYZ p, float radius) { return (_Proj3Func(p) - _Proj3Func(p + _StrafeVector * radius)).Length(); }

        

        void IScene3D.DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle brush)
        {
            for(int i=1; i < vertices.Length; ++i)
            {
                _DrawSegment(vertices[i-1], vertices[i], diameter, brush);
            }
        }

        private void _DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            var aa = a.XYZ;
            var bb = b.XYZ;

            if (!_FrustumNearPlane.ClipLineToPlane(ref aa, ref bb)) return;

            var pa = _Proj3Func(aa);
            var pb = _Proj3Func(bb);
            var pt = _ProjectRadius(aa, diameter);

            _RenderTarget.DrawLine(pa.SelectXY(), pb.SelectXY(), pt, brush);
        }

        void IScene3D.DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            var c = center.XYZ;

            if (!_FrustumNearPlane.IsInPositiveSideOfPlane(c)) return;

            var pp = _ProjectPoint(c).SelectXY();
            var pr = _ProjectRadius(c, diameter);

            _RenderTarget.DrawEllipse(pp, pr, pr, brush);
        }

        void IScene3D.DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            Span<XYZ> clippedVertices = stackalloc XYZ[vertices.Length * 2];

            var cvertices = _FrustumNearPlane.ClipPolygonToPlane(clippedVertices, Point3.AsNumerics(vertices));
            if (cvertices < 3) return;

            clippedVertices = clippedVertices.Slice(0, cvertices);

            Span<Point2> points = stackalloc Point2[clippedVertices.Length];

            var center = XYZ.Zero;

            for (int i = 0; i < points.Length; ++i)
            {
                var v = clippedVertices[i];

                center += v;
                points[i] = _ProjectPoint(v).SelectXY();
            }

            center /= points.Length;

            brush = brush.WithOutline(_ProjectRadius(center, brush.Style.OutlineWidth));

            _RenderTarget.DrawPolygon(points, brush.Style);
        }

        void ICoreScene3D.DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            Span<XYZ> clippedVertices = stackalloc XYZ[vertices.Length * 2];

            var cvertices = _FrustumNearPlane.ClipPolygonToPlane(clippedVertices, Point3.AsNumerics(vertices));
            if (cvertices < 3) return;

            clippedVertices = clippedVertices.Slice(0, cvertices);

            Span<Point2> points = stackalloc Point2[clippedVertices.Length];

            for (int i = 0; i < points.Length; ++i)
            {
                var v = clippedVertices[i];

                points[i] = _ProjectPoint(v).SelectXY();
            }


            _RenderTarget.DrawConvexPolygon(points, style);
        }

        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            throw new NotImplementedException();
            // this.DrawAssetAsSurfaces(transform, asset, brush);
        }

        #endregion

        #region types

        [System.Diagnostics.DebuggerDisplay("{Index} at {Distance}")]
        private struct _SortableItem : IComparable<_SortableItem>
        {
            public float Distance;
            public int Index;

            public int CompareTo(_SortableItem other)
            {
                return -Distance.CompareTo(other.Distance);
            }
        }

        #endregion

    }
}
