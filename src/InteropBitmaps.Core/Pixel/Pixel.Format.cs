using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace InteropBitmaps
{
    using PEF = Pixel.Format.ElementID;

    partial class Pixel
    {
        // TODO: Rename to PixelEncoding
        [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public readonly partial struct Format : IEquatable<Format>
        {
            #region diagnostics

            internal string _GetDebuggerDisplay()
            {
                var ptype = GetDefaultPixelType();
                if (ptype != null) return ptype.Name;

                var elements = Elements
                    .Where(item => item.Id != PEF.Empty)
                    .Select(item => item.Id);

                return string.Join("-", elements);
            }

            #endregion            

            #region constructors

            public static implicit operator UInt32(Format fmt) { return fmt.PackedFormat; }            

            /// <summary>
            /// Creates a new <see cref="Format"/> from a packed value.
            /// </summary>
            /// <param name="packedFormat">a raw packed value.</param>
            public Format(UInt32 packedFormat)
            {
                _Element0 = _Element1 = _Element2 = _Element3 = 0;
                PackedFormat = packedFormat;
            }

            /// <summary>
            /// Creates a new <see cref="Format"/> with one <see cref="PEF"/> channel.
            /// </summary>
            /// <param name="e0">The first channel format.</param>
            public Format(PEF e0)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = _Element2 = _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            /// <summary>
            /// Creates a new <see cref="Format"/> with two <see cref="PEF"/> channels.
            /// </summary>
            /// <param name="e0">The first channel format.</param>
            /// <param name="e1">The second channel format.</param>
            public Format(PEF e0, PEF e1)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            /// <summary>
            /// Creates a new <see cref="Format"/> with three <see cref="PEF"/> channels.
            /// </summary>
            /// <param name="e0">The first channel format.</param>
            /// <param name="e1">The second channel format.</param>
            /// <param name="e2">The third channel format.</param>
            public Format(PEF e0, PEF e1, PEF e2)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = (Byte)e2;
                _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            /// <summary>
            /// Creates a new <see cref="Format"/> with four <see cref="PEF"/> channels.
            /// </summary>
            /// <param name="e0">The first channel format.</param>
            /// <param name="e1">The second channel format.</param>
            /// <param name="e2">The third channel format.</param>
            /// <param name="e3">The fourth channel format.</param>
            public Format(PEF e0, PEF e1, PEF e2, PEF e3)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = (Byte)e2;
                _Element3 = (Byte)e3;

                _Validate();
            }

            private void _Validate()
            {
                int l = 0;
                l += Element0.BitCount;
                l += Element1.BitCount;
                l += Element2.BitCount;
                l += Element3.BitCount;

                if (l == 0) throw new InvalidOperationException("Format must not have a zero length");
                if ((l & 7) != 0) throw new InvalidOperationException("Format must have a length multiple of 8");
            }

            public static unsafe Format GetUndefined<TPixel>() where TPixel : unmanaged
            {
                var tp = typeof(TPixel);

                if (tp == typeof(float)) return new Format(PEF.Undefined32F);
                if (tp == typeof(Vector2)) return new Format(PEF.Undefined32F, PEF.Undefined32F);
                if (tp == typeof(Vector3)) return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                if (tp == typeof(Vector4)) return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);

                return GetUndefinedOfSize(sizeof(TPixel));
            }

            public static Format GetUndefinedOfSize(int byteCount)
            {
                switch (byteCount)
                {
                    case 1: return new Format(PEF.Undefined8);
                    case 2: return new Format(PEF.Undefined8, PEF.Undefined8);
                    case 3: return new Format(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                    case 4: return new Format(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                    case 8: return new Format(PEF.Undefined16, PEF.Undefined16, PEF.Undefined16, PEF.Undefined16);
                    case 12: return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                    case 16: return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                    default: throw new NotImplementedException();
                }
            }

            public static Format GetFromDepthAndChannels(Type depth, int channels)
            {
                if (depth == typeof(Byte))
                {
                    if (channels == 1) return Luminance8.Format;
                    if (channels == 3) return BGR24.Format;
                    if (channels == 4) return BGRA32.Format;
                }

                if (depth == typeof(UInt16))
                {
                    if (channels == 1) return Luminance16.Format;
                    if (channels == 3) return new Format(PEF.Blue16, PEF.Green16, PEF.Red16);
                    if (channels == 4) return new Format(PEF.Blue16, PEF.Green16, PEF.Red16, PEF.Alpha16);
                }

                if (depth == typeof(Single))
                {
                    if (channels == 1) return LuminanceScalar.Format;
                    if (channels == 3) return VectorBGR.Format;
                    if (channels == 4) return VectorBGRA.Format;
                }

                throw new NotImplementedException();
            }

            public static unsafe Format TryIdentifyPixel<TPixel>() where TPixel:unmanaged
            {
                int byteSize = sizeof(TPixel);

                switch(byteSize)
                {
                    case 1:
                        if (typeof(TPixel) == typeof(Alpha8)) return Alpha8.Format;
                        if (typeof(TPixel) == typeof(Luminance8)) return Luminance8.Format;
                        break;
                    case 2:
                        if (typeof(TPixel) == typeof(BGR565)) return BGR565.Format;
                        if (typeof(TPixel) == typeof(BGRA4444)) return BGRA4444.Format;
                        if (typeof(TPixel) == typeof(BGRA5551)) return BGRA5551.Format;
                        if (typeof(TPixel) == typeof(Luminance16)) return Luminance16.Format;
                        break;
                    case 3:
                        if (typeof(TPixel) == typeof(BGR24)) return BGR24.Format;
                        if (typeof(TPixel) == typeof(RGB24)) return RGB24.Format;
                        break;
                    case 4:
                        if (typeof(TPixel) == typeof(BGRA32)) return BGRA32.Format;
                        if (typeof(TPixel) == typeof(RGBA32)) return RGBA32.Format;
                        if (typeof(TPixel) == typeof(ARGB32)) return ARGB32.Format;
                        if (typeof(TPixel) == typeof(LuminanceScalar)) return LuminanceScalar.Format;
                        break;
                    case 24:
                        if (typeof(TPixel) == typeof(VectorBGR)) return VectorBGR.Format;
                        break;
                    case 32:
                        if (typeof(TPixel) == typeof(VectorBGRA)) return VectorBGRA.Format;
                        if (typeof(TPixel) == typeof(VectorRGBA)) return VectorRGBA.Format;
                        break;
                }

                return GetUndefined<TPixel>();
            }

            #endregion

            #region data

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            [System.Runtime.InteropServices.FieldOffset(0)]
            public readonly UInt32 PackedFormat;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            [System.Runtime.InteropServices.FieldOffset(0)]
            private readonly Byte _Element0;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            [System.Runtime.InteropServices.FieldOffset(1)]
            private readonly Byte _Element1;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            [System.Runtime.InteropServices.FieldOffset(2)]
            private readonly Byte _Element2;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            [System.Runtime.InteropServices.FieldOffset(3)]
            private readonly Byte _Element3;

            public override int GetHashCode() { return PackedFormat.GetHashCode(); }

            public override bool Equals(object obj) { return obj is Format other && this.Equals(other); }

            public bool Equals(Format other) { return this.PackedFormat == other.PackedFormat; }

            public static bool operator ==(Format a, Format b) { return a.PackedFormat == b.PackedFormat; }

            public static bool operator !=(Format a, Format b) { return a.PackedFormat != b.PackedFormat; }

            #endregion

            #region properties

            public Element Element0 => new Element(_Element0);

            public Element Element1 => new Element(_Element1);

            public Element Element2 => new Element(_Element2);

            public Element Element3 => new Element(_Element3);

            public int ByteCount => _GetByteLength();

            /// <summary>
            /// Gets the pixel format used by the Odd scanlines.
            /// </summary>
            /// <remarks>
            /// Some formats use a byte pattern where odd and even scanlines reverse the byte ordering.
            /// </remarks>
            public Format ScanlineOddFormat
            {
                get => this;
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            public IEnumerable<Element> Elements
            {
                get
                {
                    yield return Element0;
                    yield return Element1;
                    yield return Element2;
                    yield return Element3;
                }
            }

            /// <summary>
            /// Gets the number of bits used by the largest element in the format.
            /// </summary>
            public int MaxElementBitLength
            {
                get
                {
                    var l = Element0.BitCount;
                    l = Math.Max(l, Element1.BitCount);
                    l = Math.Max(l, Element2.BitCount);
                    l = Math.Max(l, Element3.BitCount);
                    return l;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this format is non empty
            /// and all defined channels are not of "undefined type"
            /// </summary>
            public bool IsDefined
            {
                get
                {
                    if (PackedFormat == 0) return false;
                    if (_Element0 > 0 && Element0.IsUndefined) return false;
                    if (_Element1 > 0 && Element1.IsUndefined) return false;
                    if (_Element2 > 0 && Element2.IsUndefined) return false;
                    if (_Element3 > 0 && Element3.IsUndefined) return false;
                    return true;
                }
            }

            public bool HasAlpha => Element0.IsAlpha | Element1.IsAlpha | Element2.IsAlpha | Element3.IsAlpha;

            #endregion

            #region API

            private int _GetByteLength()
            {
                int l = 0;
                l += Element0.BitCount;
                l += Element1.BitCount;
                l += Element2.BitCount;
                l += Element3.BitCount;
                return l / 8;
            }

            private int _FindIndex(PEF pef)
            {
                if (Element0.Id == pef) return 0;
                if (Element1.Id == pef) return 1;
                if (Element2.Id == pef) return 2;
                if (Element3.Id == pef) return 3;
                return -1;
            }

            private Element _GetComponentAt(int byteIndex)
            {
                switch (byteIndex)
                {
                    case 0: return Element0;
                    case 1: return Element1;
                    case 2: return Element2;
                    case 3: return Element3;
                    default: return PEF.Empty;
                }
            }

            public Type GetDepthType()
            {
                var e0Len = Element0.BitCount;
                var e1Len = Element1.BitCount; if (e1Len != 0 && e1Len != e0Len) return null;
                var e2Len = Element2.BitCount; if (e2Len != 0 && e2Len != e0Len) return null;
                var e3Len = Element3.BitCount; if (e3Len != 0 && e3Len != e0Len) return null;

                if (e0Len == 8) return typeof(Byte);
                if (e0Len == 16) return typeof(UInt16);
                if (e0Len == 32) return typeof(Single);

                return null;
            }

            public (Type Depth, int Channels) GetDepthTypeAndChannels()
            {
                int ch = 1;

                var len = Element0.BitCount;
                var next = Element1.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                next = Element2.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                next = Element3.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                if (len == 8) return (typeof(Byte), ch);
                if (len == 16) return (typeof(UInt16), ch);
                if (len == 32) return (typeof(Single), ch);

                return (null, 0);
            }

            public Type GetDefaultPixelType()
            {
                switch(PackedFormat)
                {
                    case Alpha8.Code: return typeof(Alpha8);

                    case Luminance8.Code: return typeof(Luminance8);
                    case Luminance16.Code: return typeof(Luminance16);
                    case LuminanceScalar.Code: return typeof(LuminanceScalar);

                    case BGR565.Code: return typeof(BGR565);
                    case BGRA5551.Code: return typeof(BGRA5551);
                    case BGRA4444.Code: return typeof(BGRA4444);

                    case BGR24.Code: return typeof(BGR24);
                    case RGB24.Code: return typeof(RGB24);

                    case BGRA32.Code: return typeof(BGRA32);
                    case RGBA32.Code: return typeof(RGBA32);
                    case ARGB32.Code: return typeof(ARGB32);

                    case VectorBGR.Code: return typeof(VectorBGR);
                    case VectorBGRA.Code: return typeof(VectorBGRA);
                    case VectorRGBA.Code: return typeof(VectorRGBA);
                }

                return null;
            }

            #endregion

            #region static        

            public static void Convert(SpanBitmap dst, SpanBitmap src)
            {
                Guard.AreEqual(nameof(src), dst.Width, src.Width);
                Guard.AreEqual(nameof(src), dst.Height, src.Height);

                var byteIndices = new int[dst.PixelByteSize];

                for (int i = 0; i < byteIndices.Length; ++i)
                {
                    var c = dst.PixelFormat._GetComponentAt(i);
                    var idx = src.PixelFormat._FindIndex(c);
                    if (idx < 0) throw new ArgumentException(nameof(src));
                    byteIndices[i] = idx;
                }

                for (int y = 0; y < dst.Height; ++y)
                {
                    var dstRow = dst.UseScanlineBytes(y);
                    var srcRow = src.GetScanlineBytes(y);

                    for (int x = 0; x < dst.Width; ++x)
                    {
                        for (int z = 0; z < byteIndices.Length; ++z)
                        {
                            var idx = byteIndices[z];
                            dstRow[z] = srcRow[idx];
                        }

                        dstRow = dstRow.Slice(dst.PixelByteSize);
                        srcRow = srcRow.Slice(src.PixelByteSize);

                        throw new NotImplementedException();
                    }
                }
            }

            #endregion
        }
    }

    
}
