using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Drawing;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends
{
    public class StaticMesh : ContextProvider
    {
        #region lifecycle

        public StaticMesh(OPENGL gl) : base(gl) { }

        protected override void Dispose(OPENGL gl)
        {
            _VBuffer?.Dispose();
            _VBuffer = null;

            _VArray?.Dispose();
            _VArray = null;

            _IBuffer?.Dispose();
            _IBuffer = null;

            base.Dispose(gl);
        }

        #endregion

        #region data        

        private VertexBuffer _VBuffer;
        private VertexBufferArray _VArray;
        private IndexBuffer _IBuffer;

        #endregion        

        #region API

        public void SetVertexData<T>(List<T> vertexData)
             where T : unmanaged, VertexElement.ISource
        {
            var vb = new VertexBuffer<T>(this.Context, BufferUsageARB.StaticDraw);
            vb.SetData(vertexData);

            _VBuffer = vb;

            _VArray = new VertexBufferArray(this.Context);
            _VArray.SetLayoutFrom<Vertex>(_VBuffer);
        }

        public void SetIndexData<T>(List<T> indexData)
            where T : unmanaged
        {
            _IBuffer = new IndexBuffer(this.Context, BufferUsageARB.StaticDraw);
            _IBuffer.SetData<T>(indexData, PrimitiveType.Triangles);
        }

        public void Draw(Effect.DrawingAPI edc)
        {
            using (var dc = edc.UseDC(_VArray, _IBuffer))
            {
                dc.DrawTriangles();
            }
        }

        #endregion
    }
}
