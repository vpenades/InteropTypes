using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using COLOR = System.Drawing.Color;

using VERTEX = Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture;

namespace InteropDrawing.Backends
{
    partial class MeshBuilder : IDrawing3D, IDrawing2D
    {
        #region lifecycle

        public MeshBuilder(bool faceFlip)
        {
            _FaceFlip = faceFlip;
        }

        #endregion

        #region data

        private readonly Boolean _FaceFlip = false;
        private readonly float _DepthZ = 0;

        private float _SpriteBleed = 0;
        private XY _SpriteUv0Bleed = XY.Zero;
        private XY _SpriteUv1Bleed = XY.Zero;
        private XY _SpriteUv2Bleed = XY.Zero;
        private XY _SpriteUv3Bleed = XY.Zero;
        private XY _SpriteCoordInvScale;

        private readonly LinesBuffer _Lines = new LinesBuffer();
        private readonly TrianglesBuffer _Triangles = new TrianglesBuffer();

        #endregion

        #region properties

        public float MinimumSolidLineDiameter { get; set; } = 0.001f;

        public int CylinderLOD { get; set; } = 6;

        public int SphereLOD { get; set; } = 1;

        public bool IsEmpty => _Lines.IsEmpty && _Triangles.IsEmpty;

        private Transforms.Decompose2D _Collapsed2D => new Transforms.Decompose2D(this);

        public float  SpriteCoordsBleed
        {
            get => _SpriteBleed;
            set
            {
                _SpriteBleed = value;
                _SpriteUv0Bleed = new XY(value, value);
                _SpriteUv1Bleed = new XY(-value, value);
                _SpriteUv2Bleed = new XY(-value, -value);
                _SpriteUv3Bleed = new XY(value, -value);
            }
        }

        #endregion

        #region Drawing2D

        public void SetSpriteTextureSize(int width, int height)
        {
            _SpriteCoordInvScale = XY.One / new XY(width, height);
        }              

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            _Collapsed2D.DrawAsset(transform, asset, brush);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            if (brush.Style.HasFill && diameter <= 1)
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    var a = new XYZ(points[i - 1].ToNumerics(), _DepthZ);
                    var b = new XYZ(points[i - 0].ToNumerics(), _DepthZ);

                    var aa = _Lines.UseVertex(a, brush.Style.FillColor);
                    var bb = _Lines.UseVertex(b, brush.Style.FillColor);
                    _Lines.AddLine(aa, bb);
                }

                brush = brush.With(COLOR.Transparent);

                if (!brush.Style.HasOutline) return;
            }

            if (diameter < 1) diameter = 1;

            _Collapsed2D.DrawLines(points, diameter, brush);
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle brush)
        {
            _Collapsed2D.DrawEllipse(center, width, height, brush);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            if (brush.HasOutline && brush.OutlineWidth > 1)
            {
                _Collapsed2D.DrawPolygon(points, brush);
                return;
            }

            if (brush.HasFill)
            {
                Span<Point3> vertices = stackalloc Point3[points.Length];

                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] = new Point3(points[i], _DepthZ);
                }

                DrawSurface(vertices, brush.FillColor);
            }

            if (brush.HasOutline)
            {
                System.Diagnostics.Debug.Assert(brush.OutlineWidth <= 1);

                this.DrawLines(points, brush.OutlineWidth, brush.OutlineColor);
                // close the loop
                this.DrawLine(points[points.Length - 1], points[0], brush.OutlineWidth, brush.OutlineColor);                
            }
        }        

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            var xform = style.Transform * transform;
            var color = style.Color.ToXna();

            var v1 = _Triangles.UseVertex(XY.Transform(XY.Zero, xform), _DepthZ, color, (style.Bitmap.UV0 + _SpriteUv0Bleed) * _SpriteCoordInvScale);
            var v2 = _Triangles.UseVertex(XY.Transform(XY.UnitX, xform), _DepthZ, color, (style.Bitmap.UV1 + _SpriteUv1Bleed) * _SpriteCoordInvScale);
            var v3 = _Triangles.UseVertex(XY.Transform(XY.One, xform), _DepthZ, color, (style.Bitmap.UV2 + _SpriteUv2Bleed) * _SpriteCoordInvScale);
            var v4 = _Triangles.UseVertex(XY.Transform(XY.UnitY, xform), _DepthZ, color, (style.Bitmap.UV3 + _SpriteUv3Bleed) * _SpriteCoordInvScale);

            _Triangles.AddTriangle(v1, v2, v3);
            _Triangles.AddTriangle(v1, v3, v4);
        }

        #endregion

        #region Drawing3D

        private Transforms.Decompose3D _Collapsed3D => new Transforms.Decompose3D(this, CylinderLOD, SphereLOD);

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            _Collapsed3D.DrawAsset(transform, asset, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, Single diameter, LineStyle brush)
        {
            if (brush.Style.HasFill && diameter <= MinimumSolidLineDiameter)
            {
                var aa = _Lines.UseVertex(a.ToNumerics(), brush.Style.FillColor);
                var bb = _Lines.UseVertex(b.ToNumerics(), brush.Style.FillColor);
                _Lines.AddLine(aa, bb);

                if (!brush.Style.HasOutline) return;

                brush = brush.With(COLOR.Transparent);
            }

            _Collapsed3D.DrawSegment(a, b, diameter, brush);
        }

        public void DrawSphere(Point3 center, Single diameter, ColorStyle brush)
        {
            _Collapsed3D.DrawSphere(center, diameter, brush);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            if (brush.Style.HasOutline)
            {
                _Collapsed3D.DrawSurface(vertices, brush);
                return;
            }
            
            var c = brush.Style.FillColor.ToXna();

            // add vertices

            Span<int> indices = stackalloc int[vertices.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                var srcVrt = vertices[i];

                var v = new VERTEX
                {
                    Position = srcVrt.ToXna(),
                    Color = c
                };

                indices[i] = _Triangles.UseVertex(v);
            }

            // add triangles

            for (int i = 2; i < vertices.Length; ++i)
            {
                if (!_FaceFlip || brush.DoubleSided)
                {
                    _Triangles.AddTriangle(indices[0], indices[i - 0], indices[i - 1]);
                }

                if (_FaceFlip || brush.DoubleSided)
                {
                    _Triangles.AddTriangle(indices[0], indices[i - 1], indices[i - 0]);
                }
            }
                        
        }

        #endregion

        #region API

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

        private readonly Collections.ValueListSet<VERTEX> _Vertices = new Collections.ValueListSet<VERTEX>();

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

        public int UseVertex(XY position, float z, Microsoft.Xna.Framework.Color color, XY uv)
        {
            var v = new VERTEX
            {
                Position = new Microsoft.Xna.Framework.Vector3(position.X, position.Y, z),
                Color = color,
                TextureCoordinate = uv.ToXna()
            };

            return _Vertices.Use(v);
        }

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
