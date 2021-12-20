using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace InteropDrawing.Backends
{
    partial class MeshBuilder
    {
        public void RenderTo(GraphicsDevice device)
        {
            _Triangles.RenderTrianglesTo(device);
            _Lines.RenderLinesTo(device);
        }
    }

    partial class LinesBuffer
    {
        public void RenderLinesTo(GraphicsDevice device)
        {
            var indices = GetIndices();
            if (indices.Count == 0) return;

            var vertices = GetVertices();

            device.DrawUserIndexedPrimitives
                (
                PrimitiveType.LineList,
                vertices.Array, vertices.Offset, vertices.Count,
                indices.Array, indices.Offset, indices.Count / 2
                );
        }
    }

    partial class TrianglesBuffer
    {
        public void RenderTrianglesTo(GraphicsDevice device)
        {
            var indices = GetIndices();
            if (indices.Count == 0) return;

            var vertices = GetVertices();

            device.DrawUserIndexedPrimitives
                (
                PrimitiveType.TriangleList,
                vertices.Array, vertices.Offset, vertices.Count,
                indices.Array, indices.Offset, indices.Count / 3
                );
        }
    }
}
