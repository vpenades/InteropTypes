using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using XFORM2 = System.Numerics.Matrix3x2;
using XFORM3 = System.Numerics.Matrix4x4;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;

namespace InteropTypes.Graphics.Drawing
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
        /// The caller must ensure the points represent a non degenerated, convex polygon.
        /// </remarks>
        void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle style);
    }

    /// <summary>
    /// Represents a render target context where we can draw 3D primitives.
    /// </summary>
    public interface IScene3D : IPrimitiveScene3D
    {
        /// <summary>
        /// Draws a backend dependant asset into to scene.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>
        /// <param name="style">The tint color to apply to the asset, if supported.</param>
        void DrawAsset(in XFORM3 transform, ASSET asset, ColorStyle style);

        /// <summary>
        /// Draws a polygon surface into the scene.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <param name="style">The style of the surface.</param>
        void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style);        

        /// <summary>
        /// Draws an extruded cylinder along the line defined by the vertices.
        /// </summary>
        /// <param name="vertices">The vertices that define the line.</param>
        /// <param name="diameter">The diameter of the cylinder.</param>
        /// <param name="style">The style of the cylinder.</param>
        void DrawSegments(ReadOnlySpan<POINT3> vertices, SCALAR diameter, LineStyle style);

        /// <summary>
        /// Draws a sphere into the scene.
        /// </summary>
        /// <param name="center">The center of the sphere</param>
        /// <param name="diameter">The diameter of the sphere</param>
        /// <param name="style">The style of the sphere.</param>
        void DrawSphere(POINT3 center, SCALAR diameter, OutlineFillStyle style);
    }

    

    /// <summary>
    /// Represents a disposable <see cref="IScene3D"/>.
    /// </summary>
    public interface IDisposableScene3D : IScene3D, IDisposable { }






    public interface ISceneBounds3D
    {
        (System.Numerics.Vector3 Center, SCALAR Radius) GetBoundingSphere();
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
