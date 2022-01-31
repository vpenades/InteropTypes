using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;
using ASSET = System.Object;
using SCALAR = System.Single;
using POINT3 = InteropDrawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropDrawing.Transforms
{
    public readonly partial struct Decompose3D :
        IScene3D,
        IServiceProvider
    {
        #region lifecycle
        public Decompose3D(IPrimitiveScene3D renderTarget)
        {
            _RenderTarget = renderTarget;
            _DecomposeSurfaceOutlines = true;
            _CylinderLod = 6;
            _SphereLod = 3;            
        }

        public Decompose3D(IPrimitiveScene3D renderTarget, int cylinderLOD, int sphereLOD)
        {
            _RenderTarget = renderTarget;
            _DecomposeSurfaceOutlines = true;
            _CylinderLod = cylinderLOD;
            _SphereLod = sphereLOD;
        }

        #endregion

        #region data

        private readonly IPrimitiveScene3D _RenderTarget;
        private readonly bool _DecomposeSurfaceOutlines;
        private readonly int _CylinderLod;
        private readonly int _SphereLod;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {            
            return this.TryGetService(serviceType, _RenderTarget);
        }

        #endregion

        #region API - IDrawing3D

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, ASSET asset, ColorStyle style)
        {
            if (asset is Asset3D a3d) { a3d._DrawAsSurfaces(this); return; }

            if (asset is IDrawingBrush<IScene3D> drawable) { drawable.DrawTo(this); return; }            
        }

        /// <inheritdoc/>
        public void DrawSegment(POINT3 a, POINT3 b, SCALAR diameter, LineStyle style)
        {
            DrawSegment(_RenderTarget, a, b, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawSphere(POINT3 center, SCALAR diameter, OutlineFillStyle style)
        {
            DrawSphere(_RenderTarget, center, diameter, _SphereLod, style);
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style)
        {
            if (vertices.Length < 3) return;

            if (_DecomposeSurfaceOutlines)
            {
                if (style.Style.HasFill) _RenderTarget.DrawConvexSurface(vertices, style.Style.FillColor);
                if (style.Style.HasOutline) DrawOutlineAsSegments(this, vertices, style.Style.OutlineWidth, style.Style.OutlineColor);
            }
            else if (style.IsVisible)
            {
                _RenderTarget.DrawConvexSurface(vertices, style.Style.FillColor);
            }
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle style)
        {
            _RenderTarget.DrawConvexSurface(vertices, style);
        }

        #endregion
    }
}
