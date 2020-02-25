using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = PixelElementFormat;

    public enum PixelElementFormat
    {
        // 0
        Empty,

        // 1
        Undefined1, Alpha1,

        // 4
        Undefined4, Red4, Green4, Blue4, Alpha4,

        // 5
        Undefined5, Red5, Green5, Blue5,

        // 6
        Undefined6, Green6,

        // 8
        Undefined8, Index8, Red8, Green8, Blue8, Alpha8, Gray8,

        // 16
        Undefined16, Index16, Gray16,

        // 32
        Undefined32, Red32F, Green32F, Blue32F, Alpha32F, Gray32F,
    }

    [System.Diagnostics.DebuggerDisplay("{Element0-Element1-Element2-Element3}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly struct PixelFormat : IEquatable<PixelFormat>
    {
        #region constants

        public static readonly PixelFormat Empty = new PixelFormat(PEF.Empty);

        public static readonly PixelFormat RGBA24 = new PixelFormat(PEF.Red8, PEF.Green8, PEF.Blue8, PEF.Alpha8);

        #endregion

        #region constructors

        public static implicit operator UInt32 (PixelFormat fmt) { return fmt.PackedFormat; }

        public static implicit operator PixelFormat(UInt32 fmt) { return new PixelFormat(fmt); }

        public PixelFormat(UInt32 packedFormat)
        {
            _Element0 = _Element1 = _Element2 = _Element3 = 0;
            PackedFormat = packedFormat;
        }

        public PixelFormat(PEF e0)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = _Element2 = _Element3 = (Byte)PixelElementFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = _Element3 = (Byte)PixelElementFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1, PixelElementFormat e2)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = (Byte)e2;
            _Element3 = (Byte)PixelElementFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1, PEF e2, PEF e3)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = (Byte)e2;
            _Element3 = (Byte)e3;
        }

        public static unsafe PixelFormat GetUndefined<TPixel>() where TPixel:unmanaged
        {
            return GetUndefinedOfSize(sizeof(TPixel));
        }

        public static PixelFormat GetUndefinedOfSize(int byteCount)
        {
            switch(byteCount)
            {
                case 1: return new PixelFormat(PEF.Undefined8);
                case 2: return new PixelFormat(PEF.Undefined8, PEF.Undefined8);
                case 3: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 4: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 8: return new PixelFormat(PEF.Undefined16, PEF.Undefined16, PEF.Undefined16, PEF.Undefined16);
                case 16: return new PixelFormat(PEF.Undefined32, PEF.Undefined32, PEF.Undefined32, PEF.Undefined32);
                default:throw new NotImplementedException();
            }
        }

        #endregion

        #region data

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly UInt32 PackedFormat;

        [System.Runtime.InteropServices.FieldOffset(0)]
        private readonly Byte _Element0;

        [System.Runtime.InteropServices.FieldOffset(1)]
        private readonly Byte _Element1;

        [System.Runtime.InteropServices.FieldOffset(2)]
        private readonly Byte _Element2;

        [System.Runtime.InteropServices.FieldOffset(3)]
        private readonly Byte _Element3;

        public override int GetHashCode() { return PackedFormat.GetHashCode(); }

        public override bool Equals(object obj) { return obj is PixelFormat other ? this.Equals(other) : false; }

        public bool Equals(PixelFormat other) { return this.PackedFormat == other.PackedFormat; }

        public static bool operator == (PixelFormat a, PixelFormat b) { return a.PackedFormat == b.PackedFormat; }

        public static bool operator != (PixelFormat a, PixelFormat b) { return a.PackedFormat != b.PackedFormat; }

        #endregion

        #region properties

        public PEF Element0 => (PEF)_Element0;

        public PEF Element1 => (PEF)_Element1;

        public PEF Element2 => (PEF)_Element2;

        public PEF Element3 => (PEF)_Element3;

        public int ByteCount => _GetByteLength();

        #endregion

        #region API

        private int _GetByteLength()
        {
            int c = 0;
            c += _GetBitLen(Element0);
            c += _GetBitLen(Element1);
            c += _GetBitLen(Element2);
            c += _GetBitLen(Element3);

            if (c == 0) throw new InvalidOperationException("Format must not have a zero length");
            if ((c & 7) != 0) throw new InvalidOperationException("Format must have a length multiple of 8");

            return c / 8;
        }

        private static int _GetBitLen(PEF pef)
        {
            switch(pef)
            {
                case PEF.Empty: return 0;

                case PEF.Alpha1:
                case PEF.Undefined1: return 1;

                case PEF.Red4:
                case PEF.Green4:
                case PEF.Blue4:
                case PEF.Undefined4: return 4;

                case PEF.Red5:
                case PEF.Green5:
                case PEF.Blue5:
                case PEF.Undefined5: return 5;

                case PEF.Green6:
                case PEF.Undefined6: return 6;

                case PEF.Index8:
                case PEF.Gray8:
                case PEF.Red8:
                case PEF.Green8:
                case PEF.Blue8:
                case PEF.Undefined8: return 8;

                case PEF.Index16:
                case PEF.Gray16:
                case PEF.Undefined16: return 16;

                case PEF.Gray32F:
                case PEF.Red32F:
                case PEF.Green32F:
                case PEF.Blue32F:
                case PEF.Undefined32: return 32;

                default: throw new NotImplementedException();
            }
        }

        #endregion
    }
}
