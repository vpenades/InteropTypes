using System;
using System.Collections.Generic;
using System.Text;



namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents the most fundamental drawing scene to draw simple convex polygons.
    /// </summary>
    public interface ICoreScene3D
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
    public interface IScene3D : ICoreScene3D
    {
        /// <summary>
        /// Draws a backend dependant asset into to scene.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>        
        void DrawAsset(in XFORM3 transform, ASSET asset);

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
}
