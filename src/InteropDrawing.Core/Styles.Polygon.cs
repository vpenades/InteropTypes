using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    public readonly struct PolygonStyle
    {
        #region constructors

        public static implicit operator PolygonStyle(ColorStyle style) { return new PolygonStyle(style.FillColor,style.OutlineColor, style.OutlineWidth); }

        public static implicit operator PolygonStyle(COLOR fillColor) { return new PolygonStyle(fillColor); }


        public static implicit operator PolygonStyle((COLOR, Single) style) { return new PolygonStyle(style.Item1, style.Item2); }

        // this operator can conflict with Color(r,g,b);
        public static implicit operator PolygonStyle((COLOR, COLOR, Single) style) { return new PolygonStyle(style.Item1, style.Item2, style.Item3); }

        public PolygonStyle(COLOR fillColor)
        {
            _FillColor = fillColor.ToArgb();
            _OutlineColor = COLOR.Transparent.ToArgb();
            OutlineWidth = 0;
        }

        public PolygonStyle(COLOR outColor, Single outWidth)
        {
            _FillColor = COLOR.Transparent.ToArgb();
            _OutlineColor = outColor.ToArgb();
            OutlineWidth = outWidth;
        }

        public PolygonStyle(COLOR fillColor, COLOR outColor, Single outWidth)
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

        private static readonly PolygonStyle _Default = new PolygonStyle(COLOR.Transparent);

        public static readonly PolygonStyle Gray = _Default.WithFill(COLOR.Gray);
        public static readonly PolygonStyle Black = _Default.WithFill(COLOR.Black);
        public static readonly PolygonStyle White = _Default.WithFill(COLOR.White);
        public static readonly PolygonStyle Red = _Default.WithFill(COLOR.Red);
        public static readonly PolygonStyle Green = _Default.WithFill(COLOR.Green);
        public static readonly PolygonStyle Blue = _Default.WithFill(COLOR.Blue);
        public static readonly PolygonStyle Yellow = _Default.WithFill(COLOR.Yellow);

        public PolygonStyle WithFill(COLOR fillColor)
        {
            return new PolygonStyle(fillColor, this.OutlineColor, this.OutlineWidth);
        }

        public PolygonStyle WithOutline(COLOR outlineColor, Single ow)
        {
            return new PolygonStyle(this.FillColor, outlineColor, ow);
        }

        public PolygonStyle WithOutline(COLOR outlineColor)
        {
            return new PolygonStyle(this.FillColor, outlineColor, this.OutlineWidth);
        }

        public PolygonStyle WithOutline(Single ow)
        {
            return new PolygonStyle(this.FillColor, this.OutlineColor, ow);
        }

        #endregion
    }
}
