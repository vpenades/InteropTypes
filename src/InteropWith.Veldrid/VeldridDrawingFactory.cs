using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropBitmaps;

using InteropDrawing;
using Veldrid;

namespace InteropWith
{
    public class VeldridDrawingFactory : IDisposable
    {
        public VeldridDrawingFactory(GraphicsDevice device)
        {
            _Graphics = new _VeldridGraphicsContext(device);
            _Textures = new _VeldridTextureCollection(device);
        }

        public void Dispose()
        {
            _Graphics?.Dispose();
            _Graphics = null;

            _Textures?.Dispose();
            _Textures = null;
        }

        private _VeldridGraphicsContext _Graphics;
        private _VeldridTextureCollection _Textures;

        public _VeldridGraphicsContext Context => _Graphics;

        private readonly Dictionary<Object, (int, Vector2)> _SpriteTextures = new Dictionary<object, (int, Vector2)>();

        public _Drawing2DContext CreateDrawingContext()
        {
            return new _Drawing2DContext(this);
        }

        internal (int, Vector2) _GetTexture(Object textureSource)
        {
            if (textureSource == null) textureSource = System.Drawing.Color.White;

            if (_SpriteTextures.TryGetValue(textureSource, out var texInfo)) return texInfo;

            MemoryBitmap bmp = default;

            if (textureSource is System.Drawing.Color color)
            {
                bmp = new MemoryBitmap(16, 16, Pixel.RGBA32.Format);
                bmp.AsSpanBitmap().SetPixels(color);
            }

            if (textureSource is string filePath)
            {
                var tmp = MemoryBitmap.Load(filePath, InteropBitmaps.Codecs.GDICodec.Default);

                bmp = new MemoryBitmap(tmp.Width, tmp.Height, Pixel.RGBA32.Format);
                bmp.SetPixels(0, 0, tmp);
            }

            if (bmp.IsEmpty) throw new NotImplementedException();

            var idx = _Textures.CreateTexture(bmp);

            texInfo = (idx, new Vector2(bmp.Width, bmp.Height));

            _SpriteTextures[textureSource] = texInfo;
            return texInfo;
        }

        internal void _SetTexture(CommandList cmd, int idx)
        {
            if (idx < 0) idx = 0;

            var t = _Textures.GetTextureView(_Textures.GetTexture(idx));
            _Graphics.CurrentEffect.SetTexture(cmd, t);
        }
    }


    public class _PrimitivesAccumulator
    {
        #region data

        private Vertex[] _VertexArray = Array.Empty<Vertex>();
        private int      _VertexCount;

        private int[] _IndicesArray = Array.Empty<int>();
        private int   _IndicesCount;

        private _BatchRecorder[] _BatchArray = Array.Empty<_BatchRecorder>();
        private int              _BatchCount;

        #endregion

        #region API

        public void Clear()
        {
            _VertexCount = 0;
            _IndicesCount = 0;
            _BatchCount = 0;
        }

        private int _UseBatchIndex(int offset, int textureId)
        {
            if (_BatchCount > 0)
            {
                var idx = _BatchCount - 1;
                if (_BatchArray[idx].TextureId == textureId) return idx;
            }

            if (_BatchArray.Length <= _BatchCount)
            {
                Array.Resize(ref _BatchArray, Math.Max(4, _BatchCount * 2));
            }

            _BatchArray[_BatchCount++] = new _BatchRecorder { Offset = offset, TextureId = textureId };

            return _BatchCount - 1;
        }        

        private void _AddTriangle(int offset, int textureId = -1)
        {
            int idx = _UseBatchIndex(offset, textureId);

            _BatchArray[idx].Count++;
        }

        private int _AddVertex(Point2 p, UInt32 c)
        {
            var v = new Vertex(p, c);
            return _AddVertex(v);
        }

        private int _AddVertex(Vertex v)
        {
            var o = _IndicesCount;

            var idx = _VertexCount; // we can look back up to 6 vertices to find a match

            if (_VertexArray.Length <= idx)
            {
                Array.Resize(ref _VertexArray, Math.Max(4, _VertexCount * 2));
            }

            _VertexArray[_VertexCount++] = v;

            return idx;
        }

        private void _AddIndex(int idx)
        {
            if (_IndicesArray.Length <= _IndicesCount)
            {
                Array.Resize(ref _IndicesArray, Math.Max(4, _IndicesCount * 2));
            }

            _IndicesArray[_IndicesCount++] = idx;
        }

        public void AddPolygon(System.Drawing.Color color, int textureId, params Point2[] points)
        {
            AddPolygon(points, color, textureId);
        }

        public unsafe void AddPolygon(ReadOnlySpan<Point2> points, System.Drawing.Color color, int textureId)
        {
            var c = (UInt32)color.ToArgb();

            Span<int> indices = stackalloc int[points.Length];

            for(int i=0; i < indices.Length; ++i)
            {
                indices[i] = _AddVertex(points[i], c);
            }

            for (int i = 2; i < points.Length; ++i)
            {
                _AddTriangle(_IndicesCount, textureId);

                _AddIndex(indices[0]);
                _AddIndex(indices[i - 1]);
                _AddIndex(indices[i + 0]);
            }
        }

        public unsafe void AddQuad(in Vertex v0, in Vertex v1, in Vertex v2, in Vertex v3, int texIdx)
        {
            Span<int> indices = stackalloc int[4];

            indices[0] = _AddVertex(v0);
            indices[1] = _AddVertex(v1);
            indices[2] = _AddVertex(v2);
            indices[3] = _AddVertex(v3);

            _AddTriangle(_IndicesCount, texIdx);
            _AddIndex(indices[0]);
            _AddIndex(indices[1]);
            _AddIndex(indices[2]);

            _AddTriangle(_IndicesCount, texIdx);
            _AddIndex(indices[0]);
            _AddIndex(indices[2]);
            _AddIndex(indices[3]);

        }

        internal void CopyTo(_IndexedVertexBuffer dst)
        {
            var vertices = _VertexArray.AsSpan(0, _VertexCount);
            var indices = _IndicesArray.AsSpan(0, _IndicesCount);

            dst.SetData(vertices, indices);
        }

        internal IEnumerable<_BatchRecorder> Batches
        {
            get
            {
                for (int i = 0; i < _BatchCount; ++i)
                {
                    yield return _BatchArray[i];
                }
            }
        }

        #endregion

        #region nested types
        internal struct _BatchRecorder
        {
            /// <summary>
            /// Offset into <see cref="_IndicesArray"/>
            /// </summary>
            public int Offset;

            /// <summary>
            /// number of triangles/sprites
            /// </summary>
            public int Count;

            /// <summary>
            /// -1 for triangles, or sprite ID
            /// </summary>
            public int TextureId;

            public void DrawTo(CommandList cmd, _IndexedVertexBuffer vb)
            {
                vb.DrawIndexed(cmd, Offset, Count * 3);
            }
        }

        #endregion
    }

    public class _Drawing2DContext : _PrimitivesAccumulator, IDrawing2D
    {
        internal _Drawing2DContext(VeldridDrawingFactory owner)
        {
            _TextureFactory = owner;
            _NullTextureId = _TextureFactory._GetTexture(null).Item1;
        }

        private readonly VeldridDrawingFactory _TextureFactory;

        private readonly int _NullTextureId;

        private InteropDrawing.Transforms.Decompose2D _Collapsed2D => new InteropDrawing.Transforms.Decompose2D(this);

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (asset is IDrawable2D drawable) { drawable.DrawTo(this); return; }
            _Collapsed2D.DrawAsset(transform, asset);
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawEllipse(center, width, height, style);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawLines(points, diameter, style);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (style.HasOutline) { _Collapsed2D.DrawPolygon(points, style); return; }
            if (style.HasFill) this.AddPolygon(points, style.FillColor, _NullTextureId);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            var final = Matrix3x2.CreateScale(style.Bitmap.Width, style.Bitmap.Height) * transform;

            Vertex p0 = final.Translation;
            Vertex p1 = Vector2.Transform(Vector2.UnitX, final);
            Vertex p2 = Vector2.Transform(Vector2.UnitX+ Vector2.UnitY, final);
            Vertex p3 = Vector2.Transform(Vector2.UnitY, final);

            var tex = _TextureFactory._GetTexture(style.Bitmap.Source);

            p0.TextureCoordinates = style.Bitmap.UV0 / tex.Item2;
            p1.TextureCoordinates = style.Bitmap.UV1 / tex.Item2;
            p2.TextureCoordinates = style.Bitmap.UV2 / tex.Item2;
            p3.TextureCoordinates = style.Bitmap.UV3 / tex.Item2;

            // tex = _TextureFactory.Invoke(null);

            AddQuad(p0, p1, p2, p3, tex.Item1);
        }


        internal void DrawTo(CommandList cmd, _IndexedVertexBuffer vb)
        {
            vb.Bind(cmd);
            foreach(var batch in this.Batches)
            {
                _TextureFactory._SetTexture(cmd, batch.TextureId);

                batch.DrawTo(cmd, vb);
            }
        }
    }
}
