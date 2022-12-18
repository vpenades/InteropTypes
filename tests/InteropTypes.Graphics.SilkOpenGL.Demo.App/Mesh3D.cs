using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace Tutorial
{
    internal class DYnamicMesh3D : ContextProvider
    {
        public DYnamicMesh3D(OPENGL gl) : base(gl)
        {
            //Creating a vertex array.
            _VBuffer = new VertexBuffer(gl, BufferUsageARB.DynamicDraw);
            // Vbo.SetData(Vertices.AsSpan());

            //Initializing a element buffer that holds the index data.
            _IBuffer = new IndexBuffer(gl, BufferUsageARB.DynamicDraw);
            // Ebo.SetData(Indices.AsSpan(), PrimitiveType.Triangles);

            _VArray = new PrimitiveBuffer(_VBuffer, _IBuffer);
        }

        protected override void Dispose(OPENGL gl)
        {
            _VArray?.Dispose();
            _VBuffer?.Dispose();
            _IBuffer?.Dispose();            

            base.Dispose(gl);
        }

        private VertexBuffer _VBuffer;
        private IndexBuffer _IBuffer;
        private PrimitiveBuffer _VArray;

        
    }
}

