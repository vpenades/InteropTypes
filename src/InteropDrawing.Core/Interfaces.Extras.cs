using System;
using System.Collections.Generic;
using System.Text;

using POINT3 = InteropTypes.Graphics.Drawing.Point3;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Provides an API for rendering 2D textured polygons.
    /// </summary>
    /// <remarks>
    /// This is an extension interface retrieved by casing an <see cref="ICanvas2D"/> to a <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IMeshCanvas2D
    {
        /// <summary>
        /// Draws a 2D triangle mesh using the associated texture.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="triangleIndices">the triangle indices.</param>
        /// <param name="texture">the texture source.</param>
        void DrawMesh(ReadOnlySpan<Vertex2> vertices, ReadOnlySpan<int> triangleIndices, Object texture);
    }

    /// <summary>
    /// Provides an API for rendering 3D meshes and wireframes.
    /// </summary>
    /// <remarks>
    /// This is an extension interface retrieved by casing an <see cref="ICanvas2D"/> to a <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IMeshScene3D
    {
        /// <summary>
        /// Draws a 3D triangle mesh using the associated texture.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="triangleIndices">the triangle indices. The amount must be multiple of 3</param>
        /// <param name="texture">the texture source.</param>
        void DrawMesh(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> triangleIndices, Object texture);

        /// <summary>
        /// Draws a 3D wire frame.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="lineIndices">the triangle indices. The amount must be multiple of 2</param>        
        void DrawWireframe(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> lineIndices);
    }
}
