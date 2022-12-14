using System;
using System.Collections.Generic;
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
            context.VertexAttribPointer(Index, Dimensions, Encoding, Normalized, ByteSize, null);
            context.EnableVertexAttribArray(Index);
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

        public interface ISource
        {
            public IEnumerable<VertexElement> GetElements();
        }

        #endregion
    }
}
