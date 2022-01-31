using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropDrawing.Transforms
{
    public class PlaneClip3D :
        IScene3D,
        IServiceProvider

    {
        #region lifecycle

        public PlaneClip3D(IScene3D target, Plane plane)
        {
            _Target = target;
            _DecomposedTarget = new Decompose3D(this, 6, 2);
            _Plane = plane;
        }

        #endregion

        #region data

        private readonly IScene3D _Target;
        private readonly IScene3D _DecomposedTarget;
        
        private readonly Plane _Plane;

        #endregion

        #region interface

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Decompose3D)) return _DecomposedTarget;
            return this.TryGetService(serviceType, _Target);
        }

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

        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            if (_Plane.IsInPositiveSideOfPlane(center, diameter*0.5f))
            {
                _Target.DrawSphere(center, diameter, brush);
            }
            else
            {
                _DecomposedTarget.DrawSphere(center, diameter, brush);
            }
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

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle brush)
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

            _Target.DrawConvexSurface(clippedVertices, brush);
        }

        #endregion
    }
}
