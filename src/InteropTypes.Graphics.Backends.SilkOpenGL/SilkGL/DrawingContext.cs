using System;
using System.Collections.Generic;
using System.Text;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public struct DrawingContext : IDisposable
    {
        public DrawingContext(VertexLayout vertices, IndexBuffer indices)
        {
            _Context = vertices.Context;
            _Vertices = vertices;
            _Indices = indices;

            _Vertices.Bind();
            _IndicesBind = indices.Using();
        }

        private readonly OPENGL _Context;
        private readonly VertexLayout _Vertices;
        private readonly IndexBuffer _Indices;
        private readonly BufferInfo.BoundAPI _IndicesBind;

        public void Dispose()
        {
            _IndicesBind.Dispose();
            _Vertices.Unbind();
        }

        public void DrawTriangles()
        {
            DrawTriangles(0, _Indices.Count / 3);
        }

        public unsafe void DrawTriangles(int start, int count)
        {
            System.Diagnostics.Debug.Assert(_Indices.Mode == PrimitiveType.Triangles);

            // Context.DrawElements(Mode, (uint)Count, Encoding, null);

            start *= 3;
            count *= 3;

            // https://registry.khronos.org/OpenGL-Refpages/gl4/html/glDrawRangeElements.xhtml
            _Vertices.Context.ThrowOnError();
            _Vertices.Context.DrawRangeElements(_Indices.Mode, (uint)start, (uint)(start + count - 1), (uint)count, _Indices.Encoding, null);
            _Vertices.Context.ThrowOnError();

        }
    }
}
