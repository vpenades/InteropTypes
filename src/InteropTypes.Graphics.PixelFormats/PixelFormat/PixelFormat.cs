﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace InteropTypes.Graphics.Bitmaps
{
    using PCID = PixelComponentID;

    /// <summary>
    /// Represents the pixel reflection type, which provides information about the pixel compontents layout and size.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This structure is designed so it must have a fixed sizeof of 4 bytes.
    /// </para>
    /// <para>
    /// ⚠️ This value is not serializable ⚠️<br/>
    /// <see cref="PixelFormat"/> depends on <see cref="PCID"/> values, which
    /// is in continuous evolution, so serialization roundtrips are dangerous.
    /// </para>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly partial struct PixelFormat : IEquatable<PixelFormat>
    {
        #region diagnostics

        private string GetDebuggerDisplay() { return ToString(); }

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
        /// Creates a new <see cref="PixelFormat"/> with one <see cref="PCID"/> channel.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        public PixelFormat(PCID e0) : this()
        {
            _Component0 = (Byte)e0;
            _Component1 = _Component2 = _Component3 = (Byte)PCID.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with two <see cref="PCID"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        public PixelFormat(PCID e0, PCID e1) : this()
        {
            _Component0 = (Byte)e0;
            _Component1 = (Byte)e1;
            _Component2 = _Component3 = (Byte)PCID.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with three <see cref="PCID"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        /// <param name="e2">The third channel format.</param>
        public PixelFormat(PCID e0, PCID e1, PCID e2) : this()
        {
            _Component0 = (Byte)e0;
            _Component1 = (Byte)e1;
            _Component2 = (Byte)e2;
            _Component3 = (Byte)PCID.Empty;

            _Validate();
        }

        /// <summary>
        /// Creates a new <see cref="PixelFormat"/> with four <see cref="PCID"/> channels.
        /// </summary>
        /// <param name="e0">The first channel format.</param>
        /// <param name="e1">The second channel format.</param>
        /// <param name="e2">The third channel format.</param>
        /// <param name="e3">The fourth channel format.</param>
        public PixelFormat(PCID e0, PCID e1, PCID e2, PCID e3) : this()
        {
            _Component0 = (Byte)e0;
            _Component1 = (Byte)e1;
            _Component2 = (Byte)e2;
            _Component3 = (Byte)e3;

            _Validate();
        }

        private void _Validate()
        {
            int l = 0;
            l += Component0.BitCount;
            l += Component1.BitCount;
            l += Component2.BitCount;
            l += Component3.BitCount;

            if (l == 0) throw new InvalidOperationException("Format must not have a zero length");
            if ((l & 7) != 0) throw new InvalidOperationException("Format must have a length multiple of 8");
        }

        public static unsafe PixelFormat CreateUndefined<TPixel>() where TPixel : unmanaged
        {
            var tp = typeof(TPixel);

            if (tp == typeof(float)) return new PixelFormat(PCID.Undefined32F);
            if (tp == typeof(Vector2)) return new PixelFormat(PCID.Undefined32F, PCID.Undefined32F);
            if (tp == typeof(Vector3)) return new PixelFormat(PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F);
            if (tp == typeof(Vector4)) return new PixelFormat(PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F);

            return CreateUndefinedOfSize(sizeof(TPixel));
        }

        public static PixelFormat CreateUndefinedOfSize(int byteCount)
        {
            switch (byteCount)
            {
                case 1: return new PixelFormat(PCID.Undefined8);
                case 2: return new PixelFormat(PCID.Undefined8, PCID.Undefined8);
                case 3: return new PixelFormat(PCID.Undefined8, PCID.Undefined8, PCID.Undefined8);
                case 4: return new PixelFormat(PCID.Undefined8, PCID.Undefined8, PCID.Undefined8, PCID.Undefined8);
                case 8: return new PixelFormat(PCID.Undefined16, PCID.Undefined16, PCID.Undefined16, PCID.Undefined16);
                case 12: return new PixelFormat(PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F);
                case 16: return new PixelFormat(PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F, PCID.Undefined32F);
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
                if (channels == 3) return new PixelFormat(PCID.Blue16, PCID.Green16, PCID.Red16);
                if (channels == 4) return new PixelFormat(PCID.Blue16, PCID.Green16, PCID.Red16, PCID.Alpha16);
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
        private readonly Byte _Component0;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Runtime.InteropServices.FieldOffset(1)]
        private readonly Byte _Component1;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Runtime.InteropServices.FieldOffset(2)]
        private readonly Byte _Component2;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Runtime.InteropServices.FieldOffset(3)]
        private readonly Byte _Component3;

        /// <inheritdoc />
        public override int GetHashCode() { return Code.GetHashCode(); }

        /// <inheritdoc />
        public override bool Equals(object obj) { return obj is PixelFormat other && this.Equals(other); }

        /// <inheritdoc />
        public bool Equals(PixelFormat other) { return this.Code == other.Code; }

        /// <inheritdoc />
        public static bool operator ==(PixelFormat a, PixelFormat b) { return a.Code == b.Code; }

        /// <inheritdoc />
        public static bool operator !=(PixelFormat a, PixelFormat b) { return a.Code != b.Code; }

        #endregion

        #region properties

        public PixelComponent this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Component0;
                    case 1: return Component1;
                    case 2: return Component2;
                    case 3: return Component3;
                    default: return PCID.Empty;
                }
            }
        }

        public PixelComponent Component0 => new PixelComponent(_Component0);

        public PixelComponent Component1 => new PixelComponent(_Component1);

        public PixelComponent Component2 => new PixelComponent(_Component2);

        public PixelComponent Component3 => new PixelComponent(_Component3);

        /// <summary>
        /// Gets the number of bytes required to store this pixel format
        /// </summary>
        public int ByteCount => _GetByteLength();        

        /// <summary>
        /// returns the sequence of non empty elements.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public IEnumerable<PixelComponent> Components
        {
            get
            {
                if (Component0.IsEmpty) yield break;
                yield return Component0;
                if (Component1.IsEmpty) yield break;
                yield return Component1;
                if (Component2.IsEmpty) yield break;
                yield return Component2;
                if (Component3.IsEmpty) yield break;
                yield return Component3;
            }
        }

        /// <summary>
        /// Gets the number of bits used by the largest element in the format.
        /// </summary>
        public int MaxComponentBitLength
        {
            get
            {
                var l = Component0.BitCount;
                l = Math.Max(l, Component1.BitCount);
                l = Math.Max(l, Component2.BitCount);
                l = Math.Max(l, Component3.BitCount);
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
                if (_Component0 > 0 && Component0.IsUndefined) return false;
                if (_Component1 > 0 && Component1.IsUndefined) return false;
                if (_Component2 > 0 && Component2.IsUndefined) return false;
                if (_Component3 > 0 && Component3.IsUndefined) return false;
                return true;
            }
        }

        /// <summary>
        /// True if one of the components is non premultiplied alpha.
        /// </summary>
        public bool HasAlpha => Component0.IsAlpha | Component1.IsAlpha | Component2.IsAlpha | Component3.IsAlpha;

        /// <summary>
        /// True if one of the components is premultiplied alpha.
        /// </summary>
        public bool HasPremul => Component0.IsPremul | Component1.IsPremul | Component2.IsPremul | Component3.IsPremul;

        /// <summary>
        /// True if one of the components is alpha (premultiplied or not).
        /// </summary>
        public bool HasAlphaOrPremul => HasAlpha | HasPremul;

        /// <summary>
        /// true if all the components are floating point values.
        /// </summary>
        public bool IsFloating => Component0.IsFloating && Component1.IsFloating && Component2.IsFloating && Component3.IsFloating;

        public bool IsQuantized => !IsFloating;

        public bool Is8BitOrLess => Component0.Is8BitOrLess && Component1.Is8BitOrLess && Component2.Is8BitOrLess && Component3.Is8BitOrLess;

        public static readonly IReadOnlyList<PixelFormat> AllFormats = new PixelFormat[]
        {
            // Millimeter16.Code, WIP

            Pixel.Alpha8.Format,

            Pixel.Luminance8.Format,
            Pixel.Luminance16.Format,
            Pixel.Luminance32F.Format,

            Pixel.BGR565.Format,
            Pixel.BGRA5551.Format,
            Pixel.BGRA4444.Format,

            Pixel.RGB24.Format,
            Pixel.BGR24.Format,

            Pixel.BGRA32.Format,
            Pixel.BGRP32.Format,

            Pixel.RGBA32.Format,
            Pixel.RGBP32.Format,

            Pixel.ARGB32.Format,
            Pixel.PRGB32.Format,

            Pixel.RGB96F.Format,
            Pixel.BGR96F.Format,

            Pixel.RGBA128F.Format,
            Pixel.RGBP128F.Format,

            Pixel.BGRA128F.Format,
            Pixel.BGRP128F.Format,
        };

        #endregion

        #region API

        private int _FindIndex(PCID pef)
        {
            if (Component0.Id == pef) return 0;
            if (Component1.Id == pef) return 1;
            if (Component2.Id == pef) return 2;
            if (Component3.Id == pef) return 3;
            return -1;
        }

        public bool All(PCID a) { return _FindIndex(a) >= 0; }
        public bool All(PCID a, PCID b) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0; }
        public bool All(PCID a, PCID b, PCID c) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0 && _FindIndex(c) >= 0; }
        public bool All(PCID a, PCID b, PCID c, PCID d) { return _FindIndex(a) >= 0 && _FindIndex(b) >= 0 && _FindIndex(c) >= 0 && _FindIndex(d) >= 0; }

        public bool Any(PCID a) { return _FindIndex(a) >= 0; }
        public bool Any(PCID a, PCID b) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0; }
        public bool Any(PCID a, PCID b, PCID c) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0 || _FindIndex(c) >= 0; }
        public bool Any(PCID a, PCID b, PCID c, PCID d) { return _FindIndex(a) >= 0 || _FindIndex(b) >= 0 || _FindIndex(c) >= 0 || _FindIndex(d) >= 0; }

        public int GetBitOffset(PCID pef)
        {
            if (Component0 == pef) return 0;

            int l = Component0.BitCount;
            if (Component1 == pef) return l;

            l += Component1.BitCount;
            if (Component2 == pef) return l;

            l += Component2.BitCount;
            if (Component3 == pef) return l;

            return -1;
        }

        public int GetByteOffset(PCID pef)
        {
            if (Component0 == pef) return 0;

            int l = Component0.ByteCount;
            if (Component1 == pef) return l;

            l += Component1.ByteCount;
            if (Component2 == pef) return l;

            l += Component2.ByteCount;
            if (Component3 == pef) return l;

            return -1;
        }

        private int _GetByteLength()
        {
            int l = 0;
            l += Component0.BitCount;
            l += Component1.BitCount;
            l += Component2.BitCount;
            l += Component3.BitCount;
            return l / 8;
        }        

        private int _GetEmptyCount()
        {
            int count = 0;
            if (Component0.Id == PCID.Empty) ++count;
            if (Component1.Id == PCID.Empty) ++count;
            if (Component2.Id == PCID.Empty) ++count;
            if (Component3.Id == PCID.Empty) ++count;
            return count;
        }        

        public Type GetDepthType()
        {
            var e0Len = Component0.BitCount;
            var e1Len = Component1.BitCount; if (e1Len != 0 && e1Len != e0Len) return null;
            var e2Len = Component2.BitCount; if (e2Len != 0 && e2Len != e0Len) return null;
            var e3Len = Component3.BitCount; if (e3Len != 0 && e3Len != e0Len) return null;

            if (e0Len == 8) return typeof(Byte);
            if (e0Len == 16) return typeof(UInt16);
            if (e0Len == 32) return typeof(Single);

            return null;
        }

        public (Type Depth, int Channels) GetDepthTypeAndChannels()
        {
            int ch = 1;

            var len = Component0.BitCount;
            var next = Component1.BitCount;
            if (next != 0) { if (next == len) ++ch; else return (null, 0); }

            next = Component2.BitCount;
            if (next != 0) { if (next == len) ++ch; else return (null, 0); }

            next = Component3.BitCount;
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
                case Pixel.BGRA128F.Code: return typeof(Pixel.BGRA128F);

                case Pixel.RGBP128F.Code: return typeof(Pixel.RGBP128F);                
                case Pixel.BGRP128F.Code: return typeof(Pixel.BGRP128F);
            }

            return null;
        }

        #endregion

        #region serialization

        /// <inheritdoc />        
        public override string ToString()
        {
            var ptype = GetDefaultPixelType();
            if (ptype != null) return ptype.Name;

            var components = Components.Select(item => item.Id);

            return string.Join("-", components);
        }

        #endregion
    }

}
