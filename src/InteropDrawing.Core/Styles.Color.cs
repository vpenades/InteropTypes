using System;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    public readonly struct ColorStyle
    {
        #region constructors        

        public static implicit operator ColorStyle(COLOR fillColor) { return new ColorStyle(fillColor); }        


        public static implicit operator ColorStyle((COLOR, Single) style) { return new ColorStyle(style.Item1, style.Item2); }

        // this operator can conflict with Color(r,g,b);
        public static implicit operator ColorStyle((COLOR, COLOR, Single) style) { return new ColorStyle(style.Item1, style.Item2, style.Item3); }

        public ColorStyle(COLOR fillColor)
        {
            _FillColor = fillColor.ToArgb();
            _OutlineColor = COLOR.Transparent.ToArgb();
            OutlineWidth = 0;
        }

        public ColorStyle(COLOR outColor, Single outWidth)
        {
            _FillColor = COLOR.Transparent.ToArgb();
            _OutlineColor = outColor.ToArgb();
            OutlineWidth = outWidth;
        }

        public ColorStyle(COLOR fillColor, COLOR outColor, Single outWidth)
        {
            _FillColor = fillColor.ToArgb();
            _OutlineColor = outColor.ToArgb();
            OutlineWidth = outWidth;
        }

        #endregion

        #region data

        private readonly Int32 _FillColor;
        private readonly Int32 _OutlineColor;
        public readonly Single OutlineWidth;

        public COLOR FillColor => COLOR.FromArgb(_FillColor);
        public COLOR OutlineColor => COLOR.FromArgb(_OutlineColor);

        #endregion

        #region properties

        public bool IsVisible => HasFill || HasOutline;

        public bool HasFill
        {
            get
            {
                var v = (uint)_FillColor;
                v >>= 24;

                return v != 0;
            }
        }

        public bool HasOutline
        {
            get
            {
                var v = (uint)_OutlineColor;
                v >>= 24;

                return v != 0 && OutlineWidth > 0;
            }
        }

        #endregion

        #region With * API

        private static readonly ColorStyle _Default = new ColorStyle(COLOR.Transparent);

        public static readonly ColorStyle Gray = _Default.WithFill(COLOR.Gray);
        public static readonly ColorStyle Black = _Default.WithFill(COLOR.Black);
        public static readonly ColorStyle White = _Default.WithFill(COLOR.White);
        public static readonly ColorStyle Red = _Default.WithFill(COLOR.Red);
        public static readonly ColorStyle Green = _Default.WithFill(COLOR.Green);
        public static readonly ColorStyle Blue = _Default.WithFill(COLOR.Blue);
        public static readonly ColorStyle Yellow = _Default.WithFill(COLOR.Yellow);

        public ColorStyle WithFill(COLOR fillColor)
        {
            return new ColorStyle(fillColor, this.OutlineColor, this.OutlineWidth);
        }

        public ColorStyle WithOutline(COLOR outlineColor, Single ow)
        {
            return new ColorStyle(this.FillColor, outlineColor, ow);
        }

        public ColorStyle WithOutline(COLOR outlineColor)
        {
            return new ColorStyle(this.FillColor, outlineColor, this.OutlineWidth);
        }

        public ColorStyle WithOutline(Single ow)
        {
            return new ColorStyle(this.FillColor, this.OutlineColor, ow);
        }

        #endregion
    }    
}
