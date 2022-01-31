using System;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    public readonly struct OutlineFillStyle
    {
        #region constructors        

        public static implicit operator OutlineFillStyle(COLOR fillColor) { return new OutlineFillStyle(fillColor); }

        public static implicit operator OutlineFillStyle(ColorStyle fillColor) { return new OutlineFillStyle(fillColor.Color); }


        public static implicit operator OutlineFillStyle((COLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2); }

        // this operator can conflict with Color(r,g,b);
        public static implicit operator OutlineFillStyle((COLOR, COLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2, style.Item3); }

        public OutlineFillStyle(COLOR fillColor)
        {
            _FillColor = fillColor.ToArgb();
            _OutlineColor = COLOR.Transparent.ToArgb();
            OutlineWidth = 0;
        }

        public OutlineFillStyle(COLOR outColor, float outWidth)
        {
            _FillColor = COLOR.Transparent.ToArgb();
            _OutlineColor = outColor.ToArgb();
            OutlineWidth = outWidth;
        }

        public OutlineFillStyle(COLOR fillColor, COLOR outColor, float outWidth)
        {
            _FillColor = fillColor.ToArgb();
            _OutlineColor = outColor.ToArgb();
            OutlineWidth = outWidth;
        }

        #endregion

        #region data

        private readonly int _FillColor;
        private readonly int _OutlineColor;
        public readonly float OutlineWidth;

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

        private static readonly OutlineFillStyle _Default = new OutlineFillStyle(COLOR.Transparent);

        public static readonly OutlineFillStyle Gray = _Default.WithFill(COLOR.Gray);
        public static readonly OutlineFillStyle Black = _Default.WithFill(COLOR.Black);
        public static readonly OutlineFillStyle White = _Default.WithFill(COLOR.White);
        public static readonly OutlineFillStyle Red = _Default.WithFill(COLOR.Red);
        public static readonly OutlineFillStyle Green = _Default.WithFill(COLOR.Green);
        public static readonly OutlineFillStyle Blue = _Default.WithFill(COLOR.Blue);
        public static readonly OutlineFillStyle Yellow = _Default.WithFill(COLOR.Yellow);

        public OutlineFillStyle WithFill(COLOR fillColor)
        {
            return new OutlineFillStyle(fillColor, OutlineColor, OutlineWidth);
        }

        public OutlineFillStyle WithOutline(COLOR outlineColor, float ow)
        {
            return new OutlineFillStyle(FillColor, outlineColor, ow);
        }

        public OutlineFillStyle WithOutline(COLOR outlineColor)
        {
            return new OutlineFillStyle(FillColor, outlineColor, OutlineWidth);
        }

        public OutlineFillStyle WithOutline(float ow)
        {
            return new OutlineFillStyle(FillColor, OutlineColor, ow);
        }

        #endregion
    }
}
