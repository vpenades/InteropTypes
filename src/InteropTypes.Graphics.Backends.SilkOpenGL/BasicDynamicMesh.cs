using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Drawing;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends
{
    public class BasicDynamicMesh : ContextProvider
    {
        #region lifecycle

        public BasicDynamicMesh(OPENGL gl) : base(gl)
        {
        }

        protected override void Dispose(OPENGL gl)
        {            
            _VBuffer?.Dispose();
            _VArray?.Dispose();
            _IBuffer?.Dispose();

            base.Dispose(gl);
        }

        #endregion

        #region data

        private readonly List<int> _Topology = new List<int>();
        private readonly List<Vertex> _Geometry = new List<Vertex>();

        private bool _GeometryDirty = false;
        private bool _TopologyDirty = false;        

        private VertexBuffer<Vertex> _VBuffer;
        private VertexBufferArray _VArray;
        private IndexBuffer _IBuffer;        

        #endregion

        #region API

        public void Clear()
        {
            _Topology.Clear();
            _Geometry.Clear();

            _GeometryDirty = true;
            _TopologyDirty= true;
        }

        void AddVertex(Vertex v)
        {
            _Topology.Add(_Topology.Count);
            _Geometry.Add(v);

            _GeometryDirty = true;
            _TopologyDirty = true;
        }

        public void AddPolygon(ReadOnlySpan<Point3> points, System.Drawing.Color color)
        {
            for (int i = 2; i < points.Length; i++)
            {
                var v0 = new Vertex(points[0], color);
                var v1 = new Vertex(points[i - 1], color);
                var v2 = new Vertex(points[i], color);

                AddVertex(v0);
                AddVertex(v1);
                AddVertex(v2);
            }
        }

        public void AddPolygon(System.Drawing.Color color, params Point3[] points)
        {
            AddPolygon(points, color);
        }
        
        public void Draw(Effect.DrawingAPI edc)
        {
            if (_VBuffer == null)
            {
                _VBuffer = new VertexBuffer<Vertex>(this.Context, BufferUsageARB.DynamicDraw);

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

            using (var dc = edc.UseDC(_VArray, _IBuffer))
            {
                dc.DrawTriangles();
            }            
        }

        #endregion
    }


    struct Vertex : VertexElement.ISource
    {
        public Vertex(Point3 point, System.Drawing.Color color)
        {
            Position = point.XYZ;
            // Normal = System.Numerics.Vector3.Normalize(Position);            
        }
        
        public System.Numerics.Vector3 Position;
        // public System.Numerics.Vector3 Normal;

        public IEnumerable<VertexElement> GetElements()
        {
            yield return new VertexElement(3, false);
            // yield return new VertexElement(3, true);
        }
    }
}
