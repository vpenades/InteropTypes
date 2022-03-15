using System;

using GDICOLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a style with a single Fill Color.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("R:{R} G:{G} B:{B} A:{A}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly struct ColorStyle : IFormattable , IEquatable<ColorStyle>
    {
        #region constants

        private static readonly UInt32 _AlphaMask = BitConverter.IsLittleEndian ? 0xff000000 : 0xff;

        #endregion

        #region constructors        

        public static implicit operator ColorStyle(GDICOLOR fillColor) { return new ColorStyle(fillColor); }
        public static implicit operator ColorStyle(float opacity) { return FromOpacity(opacity); }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(GDICOLOR fillColor) : this() { Packed = (uint)fillColor.ToArgb(); }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(int color) : this() { Packed = (uint)color; }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(uint color) : this() { Packed = color; }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(int red, int green, int blue, int alpha = 255) : this()
        {
            B = (Byte)blue;
            G = (Byte)green;
            R = (Byte)red;
            A = (Byte)alpha;
        }

        #endregion

        #region data

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly UInt32 Packed;        

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly Byte B;

        [System.Runtime.InteropServices.FieldOffset(1)]
        public readonly Byte G;

        [System.Runtime.InteropServices.FieldOffset(2)]
        public readonly Byte R;

        [System.Runtime.InteropServices.FieldOffset(3)]
        public readonly Byte A;

        /// <inheritdoc/>
        public override int GetHashCode() { return A== 0 ? 0 : this.Packed.GetHashCode(); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is ColorStyle other && this.Equals(other); }

        /// <inheritdoc/>
        public bool Equals(ColorStyle other)
        {
            return (this.A | other.A) == 0 ? true : this.Packed == other.Packed;
        }

        /// <inheritdoc/>
        public static bool operator ==(ColorStyle a, ColorStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(ColorStyle a, ColorStyle b) => !a.Equals(b);

        #endregion

        #region properties

        /// <summary>
        /// Represents
        /// </summary>
        public ColorStyle Opaque => new ColorStyle(Packed | _AlphaMask);

        /// <summary>
        /// True if this color is transparent.
        /// </summary>
        public bool IsEmpty => A == 0;

        /// <summary>
        /// True if this color is not transparent.
        /// </summary>
        public bool IsVisible => A != 0;

        #endregion

        #region constants        

        public static readonly ColorStyle Gray = GDICOLOR.Gray;
        public static readonly ColorStyle Black = GDICOLOR.Black;
        public static readonly ColorStyle White = GDICOLOR.White;
        public static readonly ColorStyle Red = GDICOLOR.Red;
        public static readonly ColorStyle Blue = GDICOLOR.Blue;
        public static readonly ColorStyle Green = GDICOLOR.Green;        
        public static readonly ColorStyle Yellow = GDICOLOR.Yellow;
        public static readonly ColorStyle Transparent = GDICOLOR.Transparent;

        #endregion

        #region API

        public static bool AreEqual(uint colorA, uint colorB)
        {
            if (((colorA | colorB) & _AlphaMask) == 0) return true;
            return colorA == colorB;
        }

        /// <summary>
        /// Converts this color to <see cref="GDICOLOR"/>
        /// </summary>
        /// <returns>A <see cref="GDICOLOR"/> instance.</returns>
        public GDICOLOR ToGDI() => GDICOLOR.FromArgb((int)Packed);

        /// <inheritdoc/>
        public override string ToString() { return ToString(null, System.Globalization.CultureInfo.InvariantCulture); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // From System.Windows.Media.Color

            var stringBuilder = new System.Text.StringBuilder();

            if (format == null)
            {
                stringBuilder.AppendFormat(formatProvider, "#{0:X2}", A);
                stringBuilder.AppendFormat(formatProvider, "{0:X2}", R);
                stringBuilder.AppendFormat(formatProvider, "{0:X2}", G);
                stringBuilder.AppendFormat(formatProvider, "{0:X2}", B);
            }
            else
            {
                format = "sc#{1:" + format + "}{0} {2:" + format + "}{0} {3:" + format + "}{0} {4:" + format + "}";

                char separator = _GetNumericListSeparator(formatProvider);

                stringBuilder.AppendFormat(formatProvider, format, separator, A, R, G, B);
            }

            return stringBuilder.ToString();
        }

        private static char _GetNumericListSeparator(IFormatProvider provider)
        {
            char c = ',';

            var instance = System.Globalization.NumberFormatInfo.GetInstance(provider);
            if (instance.NumberDecimalSeparator.Length > 0 && c == instance.NumberDecimalSeparator[0]) { c = ';'; }

            return c;
        }

        #endregion

        #region API * With

        public static ColorStyle FromOpacity(float opacity)
        {
            var o = (int)(opacity * 255f);
            o = Math.Min(255, o);
            o = Math.Max(0, o);

            return new ColorStyle(255, 255, 255, (byte)o);
        }

        public ColorStyle WithOpacity(float opacity)
        {
            var a = (float)this.A * opacity;

            return new ColorStyle(R, G, B,(int)a);
        }

        #endregion

        #region nested types        

        public static ColorStyle TryGetDefaultFrom(Object source)
        {
            return GlobalStyle.TryGetGlobalProperty<ColorStyle>(source, GlobalStyle.COLOR, out var color)
                ? color
                : default;
        }

        public static ColorStyle GetDefaultFrom(Object source, ColorStyle defval)
        {
            return GlobalStyle.TryGetGlobalProperty<ColorStyle>(source, GlobalStyle.COLOR, out var color)
                ? color
                : defval;
        }

        public bool TrySetDefaultTo(Object target)
        {
            return GlobalStyle.TrySetGlobalProperty(target, GlobalStyle.COLOR, this);
        }

        #endregion
    }
}
