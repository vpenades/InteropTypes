using System;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a style with a single Fill Color.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Color}")]
    public readonly struct ColorStyle
    {
        #region constructors        

        public static implicit operator ColorStyle(COLOR fillColor) { return new ColorStyle(fillColor); }

        public ColorStyle(int color) { _Color = color; }

        public ColorStyle(uint color) { _Color = (int)color; }

        public ColorStyle(byte red, byte green, byte blue, byte alpha)
        {
            _Color = alpha;
            _Color <<= 8;
            _Color |= red;
            _Color <<= 8;
            _Color |= green;
            _Color <<= 8;
            _Color |= blue;
        }

        public ColorStyle(COLOR fillColor) { _Color = fillColor.ToArgb(); }

        #endregion

        #region data

        private readonly int _Color;

        #endregion

        #region properties

        public uint PackedValue => (uint)_Color;

        public COLOR Color => COLOR.FromArgb(_Color);

        public bool IsEmpty => !IsVisible;

        public bool IsVisible
        {
            get
            {
                var v = (uint)_Color;
                v >>= 24;
                return v != 0;
            }
        }

        #endregion

        #region defaults        

        public static readonly ColorStyle Gray = COLOR.Gray;
        public static readonly ColorStyle Black = COLOR.Black;
        public static readonly ColorStyle White = COLOR.White;
        public static readonly ColorStyle Red = COLOR.Red;
        public static readonly ColorStyle Green = COLOR.Green;
        public static readonly ColorStyle Blue = COLOR.Blue;
        public static readonly ColorStyle Yellow = COLOR.Yellow;

        public static readonly ColorStyle Transparent = COLOR.Transparent;

        #endregion
    }
}
