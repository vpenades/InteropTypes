using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Collections;


using VERTEX = Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture;


namespace InteropTypes.Graphics.Backends
{
    partial class MeshBuilder :
        ICoreScene3D,
        IBackendCanvas2D,
        IMeshCanvas2D        
    {
        #region lifecycle

        public MeshBuilder(bool faceFlip)
        {
            _FaceFlip = faceFlip;            
        }

        #endregion

        #region data

        private readonly bool _FaceFlip;
        private readonly float _DepthZ;

        private readonly TrianglesBuffer _Triangles = new TrianglesBuffer();
        private readonly LinesBuffer _Lines = new LinesBuffer();        

        private XY _SpriteCoordInvScale;

        private float _2DLineSize = 1f;        

        #endregion

        #region properties

        public float MinimumSolidLineDiameter { get; set; } = 0.001f;

        public int CylinderLOD { get; set; } = 6;

        public int SphereLOD { get; set; } = 1;

        public bool IsEmpty => _Lines.IsEmpty && _Triangles.IsEmpty;        

        #endregion

        #region API       
        public void SetSpriteTextureSize(int width, int height)
        {
            _SpriteCoordInvScale = XY.One / new XY(width, height);
        }

        #endregion

        #region API - ICanvas2D        

        public void SetThinLinesPixelSize(float pixelSize) { _2DLineSize = pixelSize; }

        /// <inheritdoc/>
        public float GetPixelsPerUnit() { return _2DLineSize; }        

        /// <inheritdoc/>
        public void DrawThinLines(ReadOnlySpan<Point2> points, float thickness, ColorStyle color)
        {
            if (color.IsEmpty) return;

            Span<Point3> vertices = stackalloc Point3[points.Length];
            Point3.Transform(vertices, points, _DepthZ);

            var c = color.ToXna();

            for (int i = 1; i < vertices.Length; ++i)
            {
                _DrawThinLine(vertices[i - 1], vertices[i], c);
            }
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            if (color.IsEmpty) return;

            Span<Point3> vertices = stackalloc Point3[points.Length];
            Point3.Transform(vertices, points, _DepthZ);

            switch (vertices.Length)
            {
                case 0: return;
                case 1: return;
                case 2: _DrawThinLine(vertices[0], vertices[1], color.ToXna()); return;
                default: _DrawConvexSurface(vertices, color.ToXna(), false); return;
            }
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<VERTEX>() != System.Runtime.CompilerServices.Unsafe.SizeOf<Vertex3>()) throw new InvalidOperationException("Vertex size mismatch");

            Span<Vertex3> vertices = stackalloc Vertex3[4];

            style.FillVertices(vertices, transform, true, _DepthZ);            

            var xnaVertices = System.Runtime.InteropServices.MemoryMarshal.Cast<Vertex3, VERTEX>(vertices);
            var v1 = _Triangles.UseVertex(xnaVertices[0]);
            var v2 = _Triangles.UseVertex(xnaVertices[1]);
            var v3 = _Triangles.UseVertex(xnaVertices[2]);
            var v4 = _Triangles.UseVertex(xnaVertices[3]);            

            _Triangles.AddTriangle(v1, v2, v3);
            _Triangles.AddTriangle(v1, v3, v4);
        }

        /// <inheritdoc/>
        public void DrawMeshPrimitive(ReadOnlySpan<Vertex2> vertices, ReadOnlySpan<int> indices, object texture)
        {
            Span<int> vvv = stackalloc int[indices.Length];

            for(int i=0; i <indices.Length; i++)
            {
                var v = vertices[indices[i]];
                vvv[i] = _Triangles.UseVertex(v.Position, _DepthZ, new XNACOLOR(v.Color), v.TextureCoord);
            }

            for(int i=0; i < vvv.Length; i+=3)
            {
                _Triangles.AddTriangle(vvv[i+0], vvv[i + 1], vvv[i + 2]);
            }
        }

        #endregion

        #region API - IScene3D        

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            var c = style.ToXna();

            _DrawConvexSurface(vertices, c, false);
        }

        #endregion

        #region API

        private void _DrawThinLine(Point3 a, Point3 b, XNACOLOR color)
        {
            // add vertices

            var v1 = new VERTEX
            {
                Position = a.ToXna(),
                Color = color
            };

            var v2 = new VERTEX
            {
                Position = b.ToXna(),
                Color = color
            };

            var aa = _Lines.UseVertex(v1);
            var bb = _Lines.UseVertex(v2);

            // add line

            _Lines.AddLine(aa, bb);
        }

        private void _DrawConvexSurface(ReadOnlySpan<Point3> vertices, XNACOLOR color, bool doubleSided)
        {
            // add vertices

            Span<int> indices = stackalloc int[vertices.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                var srcVrt = vertices[i];

                var v = new VERTEX
                {
                    Position = srcVrt.ToXna(),
                    Color = color
                };

                indices[i] = _Triangles.UseVertex(v);
            }

            // add triangles

            for (int i = 2; i < vertices.Length; ++i)
            {
                if (!_FaceFlip || doubleSided)
                {
                    _Triangles.AddTriangle(indices[0], indices[i - 0], indices[i - 1]);
                }

                if (_FaceFlip || doubleSided)
                {
                    _Triangles.AddTriangle(indices[0], indices[i - 1], indices[i - 0]);
                }
            }
        }

        public void Clear()
        {
            _Lines.Clear();
            _Triangles.Clear();
        }        

        #endregion
    }

    class VertexBuffer
    {
        #region data

        private readonly ValueListSet<VERTEX> _Vertices = new ValueListSet<VERTEX>();

        private readonly List<int> _Indices = new List<int>();

        private VERTEX[] _FlatVertices;
        private int[] _FlatIndices;

        #endregion

        #region API

        public bool IsEmpty => _Indices.Count == 0;

        public void Clear()
        {
            _Vertices.Clear();
            _Indices.Clear();
        }

        protected void AddIndex(int idx) { _Indices.Add(idx); }

        public int UseVertex(XY position, float z, XNACOLOR color, XY uv)
        {
            var v = new VERTEX
            {
                Position = new XNAV3(position.X, position.Y, z),
                Color = color,
                TextureCoordinate = uv.ToXna()
            };

            return _Vertices.Use(v);
        }

        public int UseVertex(in VERTEX v) { return _Vertices.Use(v); }

        public int UseVertex(XYZ position, COLOR color)
        {
            var v = new VERTEX
            {
                Position = position.ToXna(),
                Color = color.ToXna()
            };

            return _Vertices.Use(v);
        }

        public int UseVertex(VERTEX key)
        {
            return _Vertices.Use(key);
        }

        protected ArraySegment<VERTEX> GetVertices()
        {
            if (_FlatVertices == null || _FlatVertices.Length < _Vertices.Count) Array.Resize(ref _FlatVertices, _Vertices.Count);

            _Vertices.CopyTo(_FlatVertices, 0);

            return new ArraySegment<VERTEX>(_FlatVertices, 0, _Vertices.Count);
        }

        protected ArraySegment<int> GetIndices()
        {
            if (_FlatIndices == null || _FlatIndices.Length < _Indices.Count) Array.Resize(ref _FlatIndices, _Indices.Count);

            _Indices.CopyTo(_FlatIndices, 0);

            return new ArraySegment<int>(_FlatIndices, 0, _Indices.Count);
        }

        #endregion
    }

    partial class LinesBuffer : VertexBuffer
    {
        #region API        

        public void AddLine(int a, int b)
        {
            if (a == b) return;

            AddIndex(a);
            AddIndex(b);
        }

        #endregion
    }

    partial class TrianglesBuffer : VertexBuffer
    {
        #region API

        public void AddTriangle(int a, int b, int c)
        {
            if (a == b || b == c || c == a) return;

            AddIndex(a);
            AddIndex(b);
            AddIndex(c);
        }

        #endregion
    }
}
