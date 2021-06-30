using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropDrawing;
using Veldrid;

namespace InteropWith
{
    public class VeldridDrawingFactory : IDisposable
    {
        public VeldridDrawingFactory(Veldrid.GraphicsDevice device)
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

        private void _AddTriangle(int offset)
        {
            int idx = _UseBatchIndex(offset, -1);

            _BatchArray[idx].Count++;            
        }

        private void _AddSprite(int offset, int textureId)
        {
            int idx = _UseBatchIndex(offset, textureId);

            _BatchArray[idx].Count++;
        }

        private int _AddVertex(Point2 p, UInt32 c)
        {
            var o = _IndicesCount;
            var v = new Vertex(p, c);

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

        public void AddPolygon(System.Drawing.Color color, params Point2[] points)
        {
            AddPolygon(points, color);
        }

        public unsafe void AddPolygon(ReadOnlySpan<Point2> points, System.Drawing.Color color)
        {
            var c = (UInt32)color.ToArgb();

            Span<int> indices = stackalloc int[points.Length];

            for(int i=0; i < indices.Length; ++i)
            {
                indices[i] = _AddVertex(points[i], c);
            }

            for (int i = 2; i < points.Length; ++i)
            {
                _AddTriangle(_IndicesCount);

                _AddIndex(indices[0]);
                _AddIndex(indices[i - 1]);
                _AddIndex(indices[i + 0]);
            }
        }

        internal void CopyTo(_IndexedVertexBuffer dst)
        {
            var vertices = _VertexArray.AsSpan(0, _VertexCount);
            var indices = _IndicesArray.AsSpan(0, _IndicesCount);

            dst.SetData(vertices, indices);
        }

        internal void DrawTo(CommandList cmd, _IndexedVertexBuffer vb)
        {
            vb.Bind(cmd);
            for(int i=0; i < _BatchCount; ++i)
            {
                _BatchArray[i].DrawTo(cmd, vb);
            }
        }

        #endregion

        #region nested types
        struct _BatchRecorder
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
            if (style.HasFill) this.AddPolygon(points, style.FillColor);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
