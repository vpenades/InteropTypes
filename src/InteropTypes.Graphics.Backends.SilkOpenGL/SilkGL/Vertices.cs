using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    [System.Diagnostics.DebuggerDisplay("{Encoding}{Dimensions}")]
    public readonly struct VertexElement
    {
        #region lifecycle

        public VertexElement(int dims, bool nrm)
            : this(VertexAttribPointerType.Float, dims, nrm)
        { }

        public VertexElement(VertexAttribPointerType encoding, int dims, bool nrm, uint index = 0)
        {
            Index = index;
            Encoding = encoding;
            Dimensions = dims;
            Normalized = nrm;
            ByteSize = (uint)(Dimensions * GetEncodingSize(encoding));
        }

        #endregion

        #region data

        public readonly uint Index;
        public readonly VertexAttribPointerType Encoding;
        public readonly int Dimensions;
        public readonly bool Normalized;
        public readonly uint ByteSize;

        #endregion

        #region API

        public VertexElement WithIndex(uint index)
        {
            return new VertexElement(Encoding, Dimensions, Normalized, index);
        }

        public unsafe void Set(OPENGL context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/API/WebGLRenderingContext/vertexAttribPointer
            context.ThrowOnError();
            context.VertexAttribPointer(Index, Dimensions, Encoding, Normalized, ByteSize, null);
            context.ThrowOnError();
        }

        public void Enable(OPENGL context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/API/WebGLRenderingContext/vertexAttribPointer            
            context.ThrowOnError();
            context.EnableVertexAttribArray(Index);
            context.ThrowOnError();
        }

        public unsafe void Disable(OPENGL context)
        {
            context.ThrowOnError();
            context.DisableVertexAttribArray(Index);
            context.ThrowOnError();
        }

        public static int GetEncodingSize(VertexAttribPointerType encoding)
        {
            switch (encoding)
            {
                case VertexAttribPointerType.Byte: return 1;
                case VertexAttribPointerType.UnsignedByte: return 1;
                case VertexAttribPointerType.Short: return 2;
                case VertexAttribPointerType.UnsignedShort: return 2;
                case VertexAttribPointerType.Int: return 4;
                case VertexAttribPointerType.UnsignedInt: return 4;
                case VertexAttribPointerType.Float: return 4;
                case VertexAttribPointerType.Double: return 8;
                case VertexAttribPointerType.HalfFloat: return 2;
                case VertexAttribPointerType.Fixed: throw new NotImplementedException();
                case VertexAttribPointerType.Int64Arb: return 8;
                case VertexAttribPointerType.UnsignedInt64Arb: return 8;
                case VertexAttribPointerType.UnsignedInt2101010Rev: return 4;
                case VertexAttribPointerType.UnsignedInt10f11f11fRev: return 4;
                case VertexAttribPointerType.Int2101010Rev: return 4;
                default: throw new NotImplementedException();
            }
        }

        #endregion

        #region nested types

        public class Collection
        {
            public void SetElements<T>()
                where T : unmanaged, ISource
            {
                if (_VertexType == typeof(T)) return;

                _VertexType = typeof(T);
                _Elements = default(T).GetElements().ToArray();

                uint offset = 0;

                for (int i = 0; i < _Elements.Length; ++i)
                {
                    _Elements[i] = _Elements[i].WithIndex(offset);
                    offset += _Elements[i].ByteSize;
                }
            }

            private Type _VertexType;
            private VertexElement[] _Elements;

            public int Count => _Elements.Length;

            public void Set(OPENGL context)
            {
                if (_Elements == null) return;
                foreach(var e in _Elements) e.Set(context);
            }

            public void Enable(OPENGL context)
            {
                if (_Elements == null) return;
                foreach (var e in _Elements) e.Enable(context);
            }

            public void Disable(OPENGL context)
            {
                if (_Elements == null) return;
                foreach (var e in _Elements) e.Disable(context);
            }
        }

        public interface ISource
        {
            public IEnumerable<VertexElement> GetElements();
        }

        #endregion
    }
}
