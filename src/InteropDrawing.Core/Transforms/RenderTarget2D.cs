using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using PLANE = System.Numerics.Plane;
using COLOR = System.Drawing.Color;
using System.Numerics;

namespace InteropDrawing.Transforms
{
    [Obsolete("Use PerspectiveTransform instead.")]
    public delegate XYZ ProjectPointCallback(XYZ worldPoint);

    [Obsolete("Use PerspectiveTransform instead.")]
    class _RenderTarget2D : IDrawing3D
    {
        #region lifecycle

        /*
        public static void Project(IDrawing2D dc, CameraProjection3D camera, Model3D scene)
        {
            camera.GetProjectionInfo(out ProjectPointCallback projCallback, out PLANE plane, scene);

            var projector = new _RenderTarget2D(dc, projCallback, plane);

            projector.Draw(scene);
        }*/

        private _RenderTarget2D(IDrawing2D dc, ProjectPointCallback prjCallback, PLANE plane)
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
        private IDrawing2D _RenderTarget;

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

        void IDrawing3D.DrawSegment(Point3 a, Point3 b, Single diameter, LineStyle brush)
        {
            var aa = a.ToNumerics();
            var bb = b.ToNumerics();

            if (!_FrustumNearPlane.ClipLineToPlane(ref aa, ref bb)) return;

            var pa = _Proj3Func(aa);
            var pb = _Proj3Func(bb);
            var pt = _ProjectRadius(aa, diameter);

            _RenderTarget.DrawLine(pa.SelectXY(), pb.SelectXY(), pt, brush);
        }

        void IDrawing3D.DrawSphere(Point3 center, Single diameter, ColorStyle brush)
        {
            var c = center.ToNumerics();

            if (!_FrustumNearPlane.IsInPositiveSideOfPlane(c)) return;

            var pp = _ProjectPoint(c).SelectXY();
            var pr = _ProjectRadius(c, diameter);

            _RenderTarget.DrawEllipse(pp, pr, pr, brush);
        }

        void ISurfaceDrawing3D.DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            Span<XYZ> clippedVertices = stackalloc XYZ[vertices.Length * 2];

            var cvertices = _FrustumNearPlane.ClipPolygonToPlane(clippedVertices, Point3.AsNumerics(vertices));
            if (cvertices < 3) return;

            clippedVertices = clippedVertices.Slice(0, cvertices);            

            Span<Point2> points = stackalloc Point2[clippedVertices.Length];

            var center = XYZ.Zero;

            for(int i=0; i < points.Length; ++i)
            {
                var v = clippedVertices[i];

                center += v;
                points[i] = _ProjectPoint(v).SelectXY();
            }

            center /= points.Length;            

            brush = brush.WithOutline(_ProjectRadius(center, brush.Style.OutlineWidth));

            _RenderTarget.DrawPolygon(points, brush.Style);
        }

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
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
                return -this.Distance.CompareTo(other.Distance);
            }
        }

        #endregion

    }
}
