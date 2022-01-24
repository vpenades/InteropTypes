using System;
using System.Collections.Generic;
using System.Text;

using InteropDrawing;

namespace InteropWith
{
    class _PrimitivesStack<TVertex>
        where TVertex: unmanaged
    {
        #region data

        private TVertex[] _VertexArray = Array.Empty<TVertex>();
        private int _VertexCount;

        private int[] _IndicesArray = Array.Empty<int>();
        private int _IndicesCount;

        private _BatchRecorder[] _BatchArray = Array.Empty<_BatchRecorder>();
        private int _BatchCount;

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

        private int _AddVertex(TVertex v)
        {
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

        public unsafe void AddConvexPolygon(ReadOnlySpan<TVertex> points, int texIdx)
        {
            Span<int> indices = stackalloc int[points.Length];

            for (int i = 0; i < indices.Length; ++i)
            {
                indices[i] = _AddVertex(points[i]);
            }

            for (int i = 2; i < points.Length; ++i)
            {
                _AddTriangle(_IndicesCount, texIdx);

                _AddIndex(indices[0]);
                _AddIndex(indices[i - 0]);
                _AddIndex(indices[i - 1]);
            }
        }

        public unsafe void AddQuad(in TVertex v0, in TVertex v1, in TVertex v2, in TVertex v3, int texIdx)
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

            public void DrawTo(Veldrid.CommandList cmd, _IndexedVertexBuffer vb)
            {
                vb.DrawIndexed(cmd, Offset, Count * 3);
            }
        }

        #endregion
    }
}
