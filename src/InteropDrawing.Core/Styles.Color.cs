using System;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a style with a single Fill Color.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
    public readonly struct ColorStyle : IFormattable
    {
        #region constructors        

        public static implicit operator ColorStyle(COLOR fillColor) { return new ColorStyle(fillColor); }
        public static implicit operator ColorStyle(float opacity) { return FromOpacity(opacity); }

        public static ColorStyle FromOpacity(float opacity)
        {
            var o = (int)(opacity * 255f);
            o = Math.Min(255, o);
            o = Math.Max(0, o);

            return new ColorStyle(255,255,255,(byte)o);
        }

        public ColorStyle(COLOR fillColor) { Packed = (uint)fillColor.ToArgb(); }

        public ColorStyle(int color) { Packed = (uint)color; }

        public ColorStyle(uint color) { Packed = color; }

        public ColorStyle(int red, int green, int blue, int alpha = 255)
        {
            Packed = Math.Min(255, (uint)alpha);
            Packed <<= 8;
            Packed |= Math.Min(255, (uint)red);
            Packed <<= 8;
            Packed |= Math.Min(255, (uint)green);
            Packed <<= 8;
            Packed |= Math.Min(255, (uint)blue);
        }        

        #endregion

        #region data

        public readonly uint Packed;

        #endregion

        #region properties        

        public int A => (int)(Packed >> 24);

        public int R => (int)(Packed >> 16) & 255;

        public int G => (int)(Packed >> 8) & 255;

        public int B => (int)(Packed & 255);

        public bool IsEmpty => Packed <= 0xffffff;

        public bool IsVisible => Packed > 0xffffff;

        #endregion

        #region constants        

        public static readonly ColorStyle Gray = COLOR.Gray;
        public static readonly ColorStyle Black = COLOR.Black;
        public static readonly ColorStyle White = COLOR.White;
        public static readonly ColorStyle Red = COLOR.Red;
        public static readonly ColorStyle Green = COLOR.Green;
        public static readonly ColorStyle Blue = COLOR.Blue;
        public static readonly ColorStyle Yellow = COLOR.Yellow;

        public static readonly ColorStyle Transparent = COLOR.Transparent;

        #endregion

        #region API

        /// <summary>
        /// Converts this color to <see cref="COLOR"/>
        /// </summary>
        /// <returns>A <see cref="COLOR"/> instance.</returns>
        public COLOR ToGDI() => COLOR.FromArgb((int)Packed);

        /// <inheritdoc/>        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null) format = "<R,G,B,A>";

            format = format.Replace("R", R.ToString(formatProvider));
            format = format.Replace("G", G.ToString(formatProvider));
            format = format.Replace("B", B.ToString(formatProvider));
            format = format.Replace("A", A.ToString(formatProvider));
            return format;
        }

        #endregion

        #region API * With

        public ColorStyle WithOpacity(float opacity)
        {
            var a = (float)this.A * opacity;

            return new ColorStyle(R, G, B,(int)a);
        }        

        #endregion
    }
}
