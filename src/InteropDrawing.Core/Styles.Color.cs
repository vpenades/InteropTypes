using System;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a style with a single Fill Color.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Color}")]    
    public readonly struct ColorStyle
    {
        #region constructors        

        public static implicit operator ColorStyle(COLOR fillColor) { return new ColorStyle(fillColor); }

        public ColorStyle(Int32 color) { _Color = color; }

        public ColorStyle(UInt32 color) { _Color = (int)color; }

        public ColorStyle(Byte red, Byte green, Byte blue, Byte alpha)
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

        private readonly Int32 _Color;

        #endregion

        #region properties

        public UInt32 PackedValue => (UInt32)_Color;

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
