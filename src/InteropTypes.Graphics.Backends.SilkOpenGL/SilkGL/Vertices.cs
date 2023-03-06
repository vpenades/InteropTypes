using System;
using System.Collections;
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

        public VertexElement(VertexAttribPointerType encoding, int dims, bool nrm, uint index = 0, uint stride = 0, uint offset = 0)
        {
            if (dims < 1 || dims > 4) throw new ArgumentOutOfRangeException(nameof(dims));            

            Index = index;

            Encoding = encoding;
            Dimensions = dims;
            Normalized = nrm;

            Offset = offset;
            Stride = stride;
        }

        #endregion

        #region data

        public readonly uint Index;

        public readonly int Dimensions;
        public readonly VertexAttribPointerType Encoding;        
        public readonly bool Normalized;

        public readonly uint Offset;
        public readonly uint Stride;

        #endregion

        #region properties

        public uint ByteSize => (uint)(Dimensions * GetEncodingSize(Encoding));

        #endregion

        #region API

        public VertexElement WithIndex(uint @index)
        {
            return new VertexElement(Encoding, Dimensions, Normalized, @index, Stride);
        }

        public VertexElement WithStrideAndOffset(uint @stride, uint @offset)
        {
            return new VertexElement(Encoding, Dimensions, Normalized, Index, @stride, @offset);
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

        #region internal

        private unsafe void Set(OPENGL context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/API/WebGLRenderingContext/vertexAttribPointer
            // https://registry.khronos.org/OpenGL-Refpages/gl4/html/glVertexAttribPointer.xhtml

            context.ThrowOnError();            
            context.VertexAttribPointer(Index, Dimensions, Encoding, Normalized, Stride, new IntPtr(Offset).ToPointer());
            context.ThrowOnError();            
        }

        public void Enable(OPENGL context)
        {            
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

        // glBindAttribLocation to associate the vertex element with the shader name

        #endregion

        #region nested types

        public class Collection : IReadOnlyList<VertexElement>
        {
            #region lifecycle

            public unsafe void SetElements<T>()
                where T : unmanaged, ISource
            {
                if (_VertexType == typeof(T)) return;

                _VertexType = typeof(T);
                _Elements = default(T).GetElements().ToArray();

                uint offset = 0;

                for (int i = 0; i < _Elements.Length; ++i)
                {
                    _Elements[i] = _Elements[i].WithIndex((uint)i).WithStrideAndOffset((uint)sizeof(T), offset);

                    offset += _Elements[i].ByteSize;
                }
            }

            #endregion

            #region data

            private Type _VertexType;
            private VertexElement[] _Elements;

            #endregion

            #region API

            public int Count => _Elements.Length;

            public VertexElement this[int index] => _Elements[index];

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

            public IEnumerator<VertexElement> GetEnumerator()
            {
                return _Elements.AsEnumerable().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _Elements.GetEnumerator();
            }

            #endregion
        }

        /// <summary>
        /// Implented by vertex types so elements can be easily extracted
        /// </summary>
        public interface ISource
        {
            public IEnumerable<VertexElement> GetElements();
        }

        #endregion
    }
}
