using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Provides an API for drawing 2D textured polygons.
    /// </summary>
    /// <remarks>
    /// This interface can be retrieved by casting an <see cref="ICanvas2D"/> to a <see cref="IServiceProvider"/> to request it.
    /// </remarks>
    public interface IMeshCanvas2D
    {
        /// <summary>
        /// Draws a 2D triangle mesh using the associated texture.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="triangleIndices">the triangle indices.</param>
        /// <param name="texture">the texture source.</param>
        void DrawMeshPrimitive(ReadOnlySpan<Vertex2> vertices, ReadOnlySpan<int> triangleIndices, Object texture);
    }

    /// <summary>
    /// Provides an API for drawing 3D meshes and wireframes.
    /// </summary>
    /// <remarks>
    /// This interface can be retrieved by casting an <see cref="ICanvas2D"/> to a <see cref="IServiceProvider"/> to request it.
    /// </remarks>
    public interface IMeshScene3D
    {
        /// <summary>
        /// Draws a 3D triangle mesh using the associated texture.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="triangleIndices">the triangle indices. The amount must be multiple of 3</param>
        /// <param name="texture">the texture source.</param>
        void DrawMeshPrimitive(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> triangleIndices, Object texture);

        /// <summary>
        /// Draws a 3D wire frame.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="lineIndices">the triangle indices. The amount must be multiple of 2</param>        
        void DrawWireframePrimitive(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> lineIndices);
    }
}
