using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    sealed class _Scene3DBoundsBuilder : IScene3D
    {
        #region constructor

        public static _Scene3DBoundsBuilder CreateEmpty()
        {
            return new _Scene3DBoundsBuilder
            {
                Min = new Vector3(float.MaxValue),
                Max = new Vector3(float.MinValue),
                Sphere = BoundingSphere.Undefined
            };
        }

        #endregion

        #region data

        public Vector3 Min;
        public Vector3 Max;

        public BoundingSphere Sphere;

        #endregion

        #region API

        public void AddVertex(in Vector3 v, float radius)
        {
            // Bounding Box
            Min = Vector3.Min(Min, v - new Vector3(radius));
            Max = Vector3.Max(Max, v + new Vector3(radius));

            // Bounding Sphere
            if (Sphere.Radius < 0) { Sphere = (v, radius); return; }

            Sphere = BoundingSphere.Merge(Sphere, (v, radius));
        }

        public void AddVertex(in Vector3 v)
        {
            // Bounding Box
            Min = Vector3.Min(Min, v);
            Max = Vector3.Max(Max, v);

            // Bounding Sphere
            if (Sphere.Radius < 0) { Sphere = (v, 0); return; }

            Sphere = BoundingSphere.Merge(Sphere, v);
        }

        #endregion

        #region data

        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            if (asset is IDrawingBrush<IScene3D> drawable)
            {
                var s = BoundingSphere.From(drawable);
                s = BoundingSphere.Transform(s, transform);
                Sphere = BoundingSphere.Merge(Sphere, s);
                return;
            }

            if (asset is IDrawingBrush<ICoreScene3D> core)
            {
                var s = BoundingSphere.From(core);
                s = BoundingSphere.Transform(s, transform);
                Sphere = BoundingSphere.Merge(Sphere, s);
                return;
            }

            if (asset is IPseudoImmutable pseudo)
            {
                DrawAsset(transform, pseudo);
            }
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            foreach (var p in vertices) AddVertex(p.XYZ, 0);
        }

        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            diameter *= 0.5f;
            diameter += style.OutlineWidth * 0.5f;
            foreach (var p in vertices) AddVertex(p.XYZ, diameter);
        }

        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
        {
            diameter *= 0.5f;
            diameter += style.OutlineWidth * 0.5f;
            AddVertex(center.XYZ, diameter);
        }

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            foreach (var p in vertices) AddVertex(p.XYZ, 0);
        }

        #endregion
    }
}
