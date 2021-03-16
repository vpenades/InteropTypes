using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropDrawing.Transforms
{
    public class PlaneClip3D : IDrawing3D
    {
        #region lifecycle

        public PlaneClip3D(IDrawing3D target, Plane plane)
        {
            _Target = target;
            _Plane = plane;
        }

        #endregion

        #region data

        private readonly IDrawing3D _Target;
        private readonly Plane _Plane;

        #endregion

        #region interface

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            throw new NotImplementedException();

            /*
            var sphere = Toolkit.GetAssetBoundingSphere(asset);

            if (!sphere.HasValue)
            {
                this.DrawAssetAsSurfaces(transform, asset, brush);
                return;
            }

            var s = sphere.Value.Transform(transform);

            if (_Plane.IsInPositiveSideOfPlane(s.Center, s.Radius))
            {
                _Target.DrawAsset(transform, asset, brush);
                return;
            }

            this.DrawAssetAsSurfaces(transform, asset, brush);
            */
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle brush)
        {
            if (_Plane.IsInPositiveSideOfPlane(center, diameter*0.5f))
            {
                _Target.DrawSphere(center, diameter, brush);
                return;
            }

            this.DrawSphereAsSurfaces(center, diameter, 2, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            var aa = a.ToNumerics();
            var bb = b.ToNumerics();

            if (!_Plane.ClipLineToPlane(ref aa, ref bb)) return;

            _Target.DrawSegment(aa, bb, diameter, brush);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            if (_Plane.IsInPositiveSideOfPlane(vertices))
            {
                _Target.DrawSurface(vertices, brush);
                return;
            }

            Span<Point3> clippedVertices = stackalloc Point3[vertices.Length * 2];

            var cvertices = _Plane.ClipPolygonToPlane(clippedVertices, vertices);
            if (cvertices < 3) return;

            clippedVertices = clippedVertices.Slice(0, cvertices);

            _Target.DrawSurface(clippedVertices, brush);
        }

        #endregion
    }
}
