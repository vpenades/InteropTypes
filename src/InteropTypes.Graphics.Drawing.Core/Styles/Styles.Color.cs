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

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(Random rnd, int? alpha) : this()
        {
            if (rnd == null) throw new ArgumentNullException(nameof(rnd));
            #pragma warning disable CA5394 // Do not use insecure randomness
            B = (Byte)rnd.Next(255);
            G = (Byte)rnd.Next(255);
            R = (Byte)rnd.Next(255);
            A = (Byte)(alpha ?? rnd.Next(255));
            #pragma warning restore CA5394 // Do not use insecure randomness
        }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(GDICOLOR fillColor) : this() { Packed = (uint)fillColor.ToArgb(); }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(int color) : this() { Packed = (uint)color; }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(uint color) : this() { Packed = color; }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(int red, int green, int blue) : this()
        {
            #if !NETSTANDARD2_0
            B = (Byte)Math.Clamp(blue,0,255);
            G = (Byte)Math.Clamp(green,0,255);
            R = (Byte)Math.Clamp(red,0,255);
            A = 255;
            #else
            B = (Byte)blue.Clamp(0,255);
            G = (Byte)green.Clamp(0,255);
            R = (Byte)red.Clamp(0,255);
            A = 255;
            #endif
        }

        [System.Diagnostics.DebuggerStepThrough]
        public ColorStyle(int red, int green, int blue, int alpha) : this()
        {
            #if !NETSTANDARD2_0
            B = (Byte)Math.Clamp(blue, 0, 255);
            G = (Byte)Math.Clamp(green, 0, 255);
            R = (Byte)Math.Clamp(red, 0, 255);
            A = (Byte)Math.Clamp(alpha, 0, 255);
            #else
            B = (Byte)blue.Clamp(0,255);
            G = (Byte)green.Clamp(0,255);
            R = (Byte)red.Clamp(0,255);
            A = (Byte)alpha.Clamp(0,255);
            #endif
        }

        [System.Diagnostics.DebuggerStepThrough]
        private ColorStyle(byte red, byte green, byte blue, byte alpha) : this()
        {            
            B = blue;
            G = green;
            R = red;
            A = alpha;
        }

        #endregion

        #region data

        /// <summary>
        /// Packed in B,G,R,A order
        /// </summary>
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
        public readonly override int GetHashCode() { return A == 0 ? 0 : this.Packed.GetHashCode(); }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is ColorStyle other && this.Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(ColorStyle other)
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
        public readonly ColorStyle Opaque => new ColorStyle(Packed | _AlphaMask);

        /// <summary>
        /// True if this color is transparent.
        /// </summary>
        public readonly bool IsEmpty => A == 0;

        /// <summary>
        /// True if this color is not transparent.
        /// </summary>
        public readonly bool IsVisible => A != 0;

        /// <summary>
        /// Packed in R,G,B,A order
        /// </summary>
        public UInt32 PackedRGBA
        {
            get
            {
                Span<byte> bytes = stackalloc byte[4];
                bytes[0] = R;
                bytes[1] = G;
                bytes[2] = B;
                bytes[3] = A;
                return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, uint>(bytes)[0];
            }
        }

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
        public readonly GDICOLOR ToGDI() => GDICOLOR.FromArgb((int)Packed);

        /// <inheritdoc/>
        public readonly override string ToString() { return ToString(null, System.Globalization.CultureInfo.InvariantCulture); }

        /// <inheritdoc/>
        public readonly string ToString(string format, IFormatProvider formatProvider)
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

        #region Fluent API

        public static ColorStyle FromOpacity(float opacity)
        {
            var o = (int)(opacity * 255f);
            o = Math.Min(255, o);
            o = Math.Max(0, o);

            return new ColorStyle((Byte)255, (Byte)255, (Byte)255, (byte)o);
        }

        public readonly ColorStyle WithOpacity(float opacity)
        {
            var o = (int)(((float)this.A) * opacity);
            o = Math.Min(255, o);
            o = Math.Max(0, o);

            return new ColorStyle((Byte)R, (Byte)G, (Byte)B, (Byte)o);
        }

        /// <summary>
        /// Gets the premultiplied representation of this color.
        /// </summary>
        /// <returns>This color, in premultiplied representation.</returns>        
        public readonly ColorStyle ToPremul()
        {
            uint fwdA = 257u * (uint)this.A;

            return new ColorStyle
                (
                (Byte)((this.R * fwdA + 255u) >> 16),
                (Byte)((this.G * fwdA + 255u) >> 16),
                (Byte)((this.B * fwdA + 255u) >> 16),
                this.A);
        }

        /// <summary>
        /// Gets the unpremultiplied representation of this color
        /// (assuming this color represents a premultiplied color)
        /// </summary>
        /// <returns>This color, in unpremultiplied representation.</returns>        
        public readonly ColorStyle ToUnpremul()
        {
            System.Diagnostics.Debug.Assert(this.R <= this.A, "not premultiplied");
            System.Diagnostics.Debug.Assert(this.G <= this.A, "not premultiplied");
            System.Diagnostics.Debug.Assert(this.B <= this.A, "not premultiplied");

            if (A == 0) return default;

            uint rcpA = (65536u * 255u) / (uint)this.A;

            return new ColorStyle
                (
                (Byte)((this.R * rcpA + 255u) >> 16),
                (Byte)((this.G * rcpA + 255u) >> 16),
                (Byte)((this.B * rcpA + 255u) >> 16),
                this.A);
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
