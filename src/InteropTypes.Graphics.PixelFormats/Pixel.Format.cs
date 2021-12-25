using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace InteropBitmaps
{
    using PEF = PixelFormat.ElementID;

    // TODO: Rename to PixelEncoding
    [System.Diagnostics.DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly partial struct PixelFormat : IEquatable<PixelFormat>
    {
        #region diagnostics

        public string GetDebuggerDisplay()
        {
            var ptype = GetDefaultPixelType();
            if (ptype != null) return ptype.Name;

            var elements = Elements.Select(item => item.Id);

            return string.Join("-", elements);
        }

        #endregion

        #region constructors

        public static implicit operator UInt32(PixelFormat fmt) { return fmt.Code; }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> from a packed value.
        /// </summary>
        /// <param name="packedFormat">a raw packed value.</param>
        public PixelFormat(UInt32 packedFormat) : this()
        {
            Code = packedFormat;
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with one <see cref="PEF"/> channel.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        public PixelFormat(PEF e0) : this()
        {
            _Element0 = (Byte)e0;
            _Element1 = _Element2 = _Element3 = (Byte)PEF.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with two <see cref="PEF"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        public PixelFormat(PEF e0, PEF e1) : this()
        {
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = _Element3 = (Byte)PEF.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with three <see cref="PEF"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        /// <param name="e2">The third channel format.</param>
        public PixelFormat(PEF e0, PEF e1, PEF e2) : this()
        {
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = (Byte)e2;
            _Element3 = (Byte)PEF.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with four <see cref="PEF"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        /// <param name="e2">The third channel format.</param>
        /// <param name="e3">The fourth channel format.</param>
        public PixelFormat(PEF e0, PEF e1, PEF e2, PEF e3) : this()
        {
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

        public static unsafe PixelFormat CreateUndefined<TPixel>() where TPixel : unmanaged
        {
            var tp = typeof(TPixel);

            if (tp == typeof(float)) return new PixelFormat(PEF.Undefined32F);
            if (tp == typeof(Vector2)) return new PixelFormat(PEF.Undefined32F, PEF.Undefined32F);
            if (tp == typeof(Vector3)) return new PixelFormat(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
            if (tp == typeof(Vector4)) return new PixelFormat(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);

            return CreateUndefinedOfSize(sizeof(TPixel));
        }

        public static PixelFormat CreateUndefinedOfSize(int byteCount)
        {
            switch (byteCount)
            {
                case 1: return new PixelFormat(PEF.Undefined8);
                case 2: return new PixelFormat(PEF.Undefined8, PEF.Undefined8);
                case 3: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 4: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 8: return new PixelFormat(PEF.Undefined16, PEF.Undefined16, PEF.Undefined16, PEF.Undefined16);
                case 12: return new PixelFormat(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                case 16: return new PixelFormat(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                default: throw new NotImplementedException();
            }
        }

        public static PixelFormat CreateFromDepthAndChannels(Type depth, int channels)
        {
            if (depth == typeof(Byte))
            {
                if (channels == 1) return Pixel.Luminance8.Format;
                if (channels == 3) return Pixel.BGR24.Format;
                if (channels == 4) return Pixel.BGRA32.Format;
            }

            if (depth == typeof(UInt16))
            {
                if (channels == 1) return Pixel.Luminance16.Format;
                if (channels == 3) return new PixelFormat(PEF.Blue16, PEF.Green16, PEF.Red16);
                if (channels == 4) return new PixelFormat(PEF.Blue16, PEF.Green16, PEF.Red16, PEF.Alpha16);
            }

            if (depth == typeof(Single))
            {
                if (channels == 1) return Pixel.Luminance32F.Format;
                if (channels == 3) return Pixel.BGR96F.Format;
                if (channels == 4) return Pixel.BGRA128F.Format;
            }

            throw new NotImplementedException();
        }

        public static unsafe PixelFormat TryIdentifyPixel<TPixel>() where TPixel : unmanaged
        {
            int byteSize = sizeof(TPixel);

            switch (byteSize)
            {
                case 1:
                    if (typeof(TPixel) == typeof(Pixel.Alpha8)) return Pixel.Alpha8.Format;
                    if (typeof(TPixel) == typeof(Pixel.Luminance8)) return Pixel.Luminance8.Format;
                    break;
                case 2:
                    if (typeof(TPixel) == typeof(Pixel.BGR565)) return Pixel.BGR565.Format;
                    if (typeof(TPixel) == typeof(Pixel.BGRA4444)) return Pixel.BGRA4444.Format;
                    if (typeof(TPixel) == typeof(Pixel.BGRA5551)) return Pixel.BGRA5551.Format;
                    if (typeof(TPixel) == typeof(Pixel.Luminance16)) return Pixel.Luminance16.Format;
                    break;
                case 3:
                    if (typeof(TPixel) == typeof(Pixel.BGR24)) return Pixel.BGR24.Format;
                    if (typeof(TPixel) == typeof(Pixel.RGB24)) return Pixel.RGB24.Format;
                    break;
                case 4:
                    if (typeof(TPixel) == typeof(Pixel.BGRA32)) return Pixel.BGRA32.Format;
                    if (typeof(TPixel) == typeof(Pixel.BGRP32)) return Pixel.BGRP32.Format;
                    if (typeof(TPixel) == typeof(Pixel.RGBA32)) return Pixel.RGBA32.Format;
                    if (typeof(TPixel) == typeof(Pixel.RGBP32)) return Pixel.RGBP32.Format;
                    if (typeof(TPixel) == typeof(Pixel.ARGB32)) return Pixel.ARGB32.Format;
                    if (typeof(TPixel) == typeof(Pixel.PRGB32)) return Pixel.PRGB32.Format;
                    if (typeof(TPixel) == typeof(Pixel.Luminance32F)) return Pixel.Luminance32F.Format;
                    break;
                case 12:
                    if (typeof(TPixel) == typeof(Pixel.RGB96F)) return Pixel.RGB96F.Format;
                    if (typeof(TPixel) == typeof(Pixel.BGR96F)) return Pixel.BGR96F.Format;
                    break;
                case 16:
                    if (typeof(TPixel) == typeof(Pixel.BGRA128F)) return Pixel.BGRA128F.Format;
                    if (typeof(TPixel) == typeof(Pixel.BGRP128F)) return Pixel.BGRP128F.Format;
                    if (typeof(TPixel) == typeof(Pixel.RGBA128F)) return Pixel.RGBA128F.Format;
                    if (typeof(TPixel) == typeof(Pixel.RGBP128F)) return Pixel.RGBP128F.Format;
                    break;
            }

            return CreateUndefined<TPixel>();
        }

        public static bool TryGetFormatAsRGBX(PixelFormat fmt, out PixelFormat newFmt)
        {
            newFmt = default;

            switch (fmt.Code)
            {
                case Pixel.RGB24.Code: newFmt = Pixel.RGB24.Format; break;
                case Pixel.BGR24.Code: newFmt = Pixel.RGB24.Format; break;

                case Pixel.RGBA32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.BGRA32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.ARGB32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.RGBP32.Code: newFmt = Pixel.RGBP32.Format; break;
                case Pixel.BGRP32.Code: newFmt = Pixel.RGBP32.Format; break;

                case Pixel.RGB96F.Code: newFmt = Pixel.RGB96F.Format; break;
                case Pixel.BGR96F.Code: newFmt = Pixel.RGB96F.Format; break;

                case Pixel.RGBA128F.Code: newFmt = Pixel.RGBA128F.Format; break;
                case Pixel.BGRA128F.Code: newFmt = Pixel.RGBA128F.Format; break;
                case Pixel.RGBP128F.Code: newFmt = Pixel.RGBP128F.Format; break;
                case Pixel.BGRP128F.Code: newFmt = Pixel.BGRP128F.Format; break;
            }

            return newFmt != default;
        }

        public static bool TryGetFormatAsBGRX(PixelFormat fmt, out PixelFormat newFmt)
        {
            newFmt = default;

            switch (fmt.Code)
            {
                case Pixel.RGB24.Code: newFmt = Pixel.BGR24.Format; break;
                case Pixel.BGR24.Code: newFmt = Pixel.BGR24.Format; break;

                case Pixel.RGBA32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.BGRA32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.ARGB32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.RGBP32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.BGRP32.Code: newFmt = Pixel.BGRA32.Format; break;

                case Pixel.RGB96F.Code: newFmt = Pixel.BGR96F.Format; break;
                case Pixel.BGR96F.Code: newFmt = Pixel.BGR96F.Format; break;

                case Pixel.RGBA128F.Code: newFmt = Pixel.BGRA128F.Format; break;
                case Pixel.BGRA128F.Code: newFmt = Pixel.BGRA128F.Format; break;
                case Pixel.RGBP128F.Code: newFmt = Pixel.RGBP128F.Format; break;
                case Pixel.BGRP128F.Code: newFmt = Pixel.BGRP128F.Format; break;
            }

            return newFmt != default;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly UInt32 Code;

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

        /// <inheritdoc />
        public override int GetHashCode() { return Code.GetHashCode(); }

        /// <inheritdoc />
        public override bool Equals(object obj) { return obj is PixelFormat other && this.Equals(other); }

        /// <inheritdoc />
        public bool Equals(PixelFormat other) { return this.Code == other.Code; }

        public static bool operator ==(PixelFormat a, PixelFormat b) { return a.Code == b.Code; }

        public static bool operator !=(PixelFormat a, PixelFormat b) { return a.Code != b.Code; }

        #endregion

        #region properties

        public Element Element0 => new Element(_Element0);

        public Element Element1 => new Element(_Element1);

        public Element Element2 => new Element(_Element2);

        public Element Element3 => new Element(_Element3);

        /// <summary>
        /// Gets the number of bytes required to store this pixel format
        /// </summary>
        public int ByteCount => _GetByteLength();

        /// <summary>
        /// Gets the pixel format used by the Odd scanlines.
        /// </summary>
        /// <remarks>
        /// Some formats use a byte pattern where odd and even scanlines reverse the byte ordering.
        /// </remarks>
        public PixelFormat ScanlineOddFormat
        {
            get => this;
        }

        /// <summary>
        /// returns the sequence of non empty elements.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public IEnumerable<Element> Elements
        {
            get
            {
                if (Element0.IsEmpty) yield break;
                yield return Element0;
                if (Element1.IsEmpty) yield break;
                yield return Element1;
                if (Element2.IsEmpty) yield break;
                yield return Element2;
                if (Element3.IsEmpty) yield break;
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
                if (Code == 0) return false;
                if (_Element0 > 0 && Element0.IsUndefined) return false;
                if (_Element1 > 0 && Element1.IsUndefined) return false;
                if (_Element2 > 0 && Element2.IsUndefined) return false;
                if (_Element3 > 0 && Element3.IsUndefined) return false;
                return true;
            }
        }

        /// <summary>
        /// True if one of the elements is non premultiplied alpha.
        /// </summary>
        public bool HasAlpha => Element0.IsAlpha | Element1.IsAlpha | Element2.IsAlpha | Element3.IsAlpha;

        /// <summary>
        /// True if one of the elements is premultiplied alpha.
        /// </summary>
        public bool HasPremul => Element0.IsPremul | Element1.IsPremul | Element2.IsPremul | Element3.IsPremul;

        /// <summary>
        /// True if one of the elements is alpha (premultiplied or not).
        /// </summary>
        public bool HasAlphaOrPremul => HasAlpha | HasPremul;

        /// <summary>
        /// true if all the elements are floating point values.
        /// </summary>
        public bool IsFloating => Element0.IsFloating && Element1.IsFloating && Element2.IsFloating && Element3.IsFloating;

        #endregion

        #region API

        public bool All(PEF a) { return _FindIndex(a) >= 0; }
        public bool All(PEF a, PEF b) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0; }
        public bool All(PEF a, PEF b, PEF c) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0 && _FindIndex(c) >= 0; }
        public bool All(PEF a, PEF b, PEF c, PEF d) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0 && _FindIndex(c) >= 0 && _FindIndex(d) >= 0; }

        public bool Any(PEF a) { return _FindIndex(a) >= 0; }
        public bool Any(PEF a, PEF b) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0; }
        public bool Any(PEF a, PEF b, PEF c) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0 || _FindIndex(c) >= 0; }
        public bool Any(PEF a, PEF b, PEF c, PEF d) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0 || _FindIndex(c) >= 0 || _FindIndex(d) >= 0; }

        public int GetBitOffset(PEF pef)
        {
            if (Element0 == pef) return 0;

            int l = Element0.BitCount;
            if (Element1 == pef) return l;

            l += Element1.BitCount;
            if (Element2 == pef) return l;

            l += Element2.BitCount;
            if (Element3 == pef) return l;

            return -1;
        }

        public int GetByteOffset(PEF pef)
        {
            if (Element0 == pef) return 0;

            int l = Element0.ByteCount;
            if (Element1 == pef) return l;

            l += Element1.ByteCount;
            if (Element2 == pef) return l;

            l += Element2.ByteCount;
            if (Element3 == pef) return l;

            return -1;
        }

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

        private int _GetEmptyCount()
        {
            int count = 0;
            if (Element0.Id == PEF.Empty) ++count;
            if (Element1.Id == PEF.Empty) ++count;
            if (Element2.Id == PEF.Empty) ++count;
            if (Element3.Id == PEF.Empty) ++count;
            return count;
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
            switch (Code)
            {
                case Pixel.Alpha8.Code: return typeof(Pixel.Alpha8);

                case Pixel.Luminance8.Code: return typeof(Pixel.Luminance8);
                case Pixel.Luminance16.Code: return typeof(Pixel.Luminance16);
                case Pixel.Luminance32F.Code: return typeof(Pixel.Luminance32F);

                case Pixel.BGR565.Code: return typeof(Pixel.BGR565);
                case Pixel.BGRA5551.Code: return typeof(Pixel.BGRA5551);
                case Pixel.BGRA4444.Code: return typeof(Pixel.BGRA4444);

                case Pixel.BGR24.Code: return typeof(Pixel.BGR24);
                case Pixel.RGB24.Code: return typeof(Pixel.RGB24);

                case Pixel.BGRA32.Code: return typeof(Pixel.BGRA32);
                case Pixel.RGBA32.Code: return typeof(Pixel.RGBA32);
                case Pixel.ARGB32.Code: return typeof(Pixel.ARGB32);
                case Pixel.PRGB32.Code: return typeof(Pixel.PRGB32);

                case Pixel.BGRP32.Code: return typeof(Pixel.BGRP32);
                case Pixel.RGBP32.Code: return typeof(Pixel.RGBP32);

                case Pixel.RGB96F.Code: return typeof(Pixel.RGB96F);
                case Pixel.BGR96F.Code: return typeof(Pixel.BGR96F);

                case Pixel.RGBA128F.Code: return typeof(Pixel.RGBA128F);
                case Pixel.RGBP128F.Code: return typeof(Pixel.RGBP128F);

                case Pixel.BGRA128F.Code: return typeof(Pixel.BGRA128F);
                case Pixel.BGRP128F.Code: return typeof(Pixel.BGRP128F);
            }

            return null;
        }

        #endregion
    }

}
