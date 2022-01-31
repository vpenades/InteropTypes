using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;

using XFORM3 = System.Numerics.Matrix4x4;
using POINT3 = InteropDrawing.Point3;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a render target context where we can draw 3D surfaces
    /// </summary>
    public interface IPrimitiveScene3D
    {
        /// <summary>
        /// Draws a convex polygon, with the given fill color
        /// </summary>
        /// <param name="vertices">
        /// The vertices of the polygon.
        /// If 2 points are passed, it should draw a thin line.
        /// If 1 point is passed, it should draw a point.
        /// </param>        
        /// <param name="style">The color of the polygon</param>
        /// <remarks>        
        /// The caller must ensure the points represent a convex polygon.
        /// </remarks>
        void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle style);
    }

    /// <summary>
    /// Represents a render target context where we can draw 3D primitives.
    /// </summary>
    public interface IScene3D : IPrimitiveScene3D
    {
        void DrawAsset(in XFORM3 transform, ASSET asset, ColorStyle style);

        void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style);

        void DrawSegment(POINT3 a, POINT3 b, SCALAR diameter, LineStyle style);

        void DrawSphere(POINT3 center, SCALAR diameter, OutlineFillStyle style);
    }

    /// <summary>
    /// Represents a disposable <see cref="IScene3D"/>.
    /// </summary>
    public interface IDisposableScene3D : IScene3D, IDisposable { }






    public interface ISceneBounds3D
    {
        (System.Numerics.Vector3 Center, Single Radius) GetBoundingSphere();
    }

    public interface ISceneViewport2D
    {
        (XFORM2 Camera, XFORM2 Projection) GetMatrices(float renderWidth, float renderHeight);
    }

    public interface ISceneViewport3D
    {
        // TODO: include near/far depth plane distance hints

        (XFORM3 Camera, XFORM3 Projection) GetMatrices(float renderWidth, float renderHeight);
    }    
}
