using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Drawing;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends
{
    public class BasicDynamicMesh<TVertex> : ContextProvider
        where TVertex: unmanaged, VertexElement.ISource
    {
        #region lifecycle

        public BasicDynamicMesh(OPENGL gl) : base(gl) { }

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

        private readonly List<int> _Topology = new List<int>();
        private readonly List<TVertex> _Geometry = new List<TVertex>();

        private bool _GeometryDirty = false;
        private bool _TopologyDirty = false;        

        private VertexBuffer<TVertex> _VBuffer;
        private VertexBufferArray _VArray;
        private IndexBuffer _IBuffer;        

        #endregion

        #region API - Build

        public void Clear()
        {
            _Topology.Clear();
            _Geometry.Clear();

            _GeometryDirty = true;
            _TopologyDirty = true;
        }

        void AddVertex(TVertex v)
        {
            _Topology.Add(_Topology.Count);
            _Geometry.Add(v);

            _GeometryDirty = true;
            _TopologyDirty = true;
        }

        public void AddPolygon(params TVertex[] points)
        {
            AddPolygon(points.AsSpan());
        }

        public void AddPolygon(ReadOnlySpan<TVertex> points)
        {
            for (int i = 2; i < points.Length; i++)
            {
                var v0 = points[0];
                var v1 = points[i - 1];
                var v2 = points[i];

                AddVertex(v0);
                AddVertex(v1);
                AddVertex(v2);
            }
        }

        #endregion

        #region API - Rendering

        public void Flush()
        {
            if (_VBuffer == null)
            {
                _VBuffer = new VertexBuffer<TVertex>(this.Context, BufferUsageARB.DynamicDraw);

                _VArray = new VertexBufferArray(this.Context);
                _VArray.SetLayoutFrom<Vertex>(_VBuffer);

                _IBuffer = new IndexBuffer(this.Context, BufferUsageARB.DynamicDraw);
            }

            if (_GeometryDirty)
            {
                _GeometryDirty = false;
                _VBuffer.SetData(_Geometry);
            }

            if (_TopologyDirty)
            {
                _TopologyDirty = false;
                _IBuffer.SetData(_Topology, PrimitiveType.Triangles);
            }
        }
        
        public void Draw(Effect.DrawingAPI edc)
        {
            Flush();

            using (var dc = edc.UseDC(_VArray, _IBuffer))
            {
                dc.DrawTriangles();
            }            
        }

        #endregion
    }

    [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
    public struct Vertex : VertexElement.ISource
    {
        public Vertex(float x, float y, float z)
        {
            Position = new System.Numerics.Vector3(x,y,z);
            TexCoord = Vector2.Zero;
        }

        public Vertex WithUV(float u, float v)
        {
            return new Vertex(Position, new Point2(u, v));
        }

        public Vertex(Point3 point, Point2 uv)
        {
            Position = point.XYZ;
            TexCoord = uv.XY;
        }
        
        public System.Numerics.Vector3 Position;
        public System.Numerics.Vector2 TexCoord;

        public IEnumerable<VertexElement> GetElements()
        {
            yield return new VertexElement(3, false);
            yield return new VertexElement(2, false);
        }
    }
}
