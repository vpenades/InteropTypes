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

        public ColorStyle(COLOR fillColor) { _Color = (uint)fillColor.ToArgb(); }

        public ColorStyle(int color) { _Color = (uint)color; }

        public ColorStyle(uint color) { _Color = color; }

        public ColorStyle(byte red, byte green, byte blue, byte alpha = 255)
        {
            _Color = alpha;
            _Color <<= 8;
            _Color |= red;
            _Color <<= 8;
            _Color |= green;
            _Color <<= 8;
            _Color |= blue;
        }        

        #endregion

        #region data

        private readonly uint _Color;

        #endregion

        #region properties

        public uint Packed => _Color;

        public COLOR Color => COLOR.FromArgb((int)_Color);

        public int A => (int)(_Color >> 24);

        public int R => (int)(_Color >> 16) & 255;

        public int G => (int)(_Color >> 8) & 255;

        public int B => (int)(_Color & 255);

        public bool IsEmpty => !IsVisible;

        public bool IsVisible => 0 != (_Color & 0xff000000);

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
    }
}
