using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Combines an <see cref="ColorStyle"/> with Double sided surface style.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="ISurfaceDrawing3D.DrawSurface(ReadOnlySpan{Point3}, SurfaceStyle)"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {DoubleSided}")]
    public readonly struct SurfaceStyle
    {
        #region implicit

        public static implicit operator SurfaceStyle(PolygonStyle brush) { return new SurfaceStyle(brush, true); }
        public static implicit operator SurfaceStyle((PolygonStyle color, Boolean doubleSided) style) { return new SurfaceStyle(style.color, style.doubleSided); }

        public static implicit operator SurfaceStyle(ColorStyle brush) { return new SurfaceStyle(brush, true); }
        public static implicit operator SurfaceStyle((ColorStyle color, Boolean doubleSided) style) { return new SurfaceStyle(style.color, style.doubleSided); }

        public static implicit operator SurfaceStyle(COLOR fillColor) { return new SurfaceStyle(fillColor); }

        public static implicit operator SurfaceStyle((COLOR fillColor, Boolean doubleSided) style) { return new SurfaceStyle(style.fillColor, style.doubleSided); }

        public static implicit operator SurfaceStyle((COLOR fillColor, Single outWidth) style) { return new SurfaceStyle(style.fillColor, style.outWidth); }        

        public static implicit operator SurfaceStyle((COLOR fillColor, COLOR outColor, Single outWidth) style) { return new SurfaceStyle(style.fillColor, style.outColor, style.outWidth); }

        public static implicit operator SurfaceStyle((COLOR fillColor, COLOR outColor, Single outWidth, Boolean doubleSided) style) { return new SurfaceStyle((style.fillColor, style.outColor, style.outWidth), style.doubleSided); }

        #endregion

        #region constructors

        public SurfaceStyle(COLOR fillColor)
        {
            Style = fillColor;
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(COLOR fillColor, COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(fillColor, outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(COLOR fillColor, bool doubleSided)
        {
            Style = new ColorStyle(fillColor);
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(ColorStyle color, bool doubleSided)
        {
            Style = color;
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(PolygonStyle color, bool doubleSided)
        {
            Style = new ColorStyle(color.FillColor, color.OutlineColor, color.OutlineWidth);
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        #endregion

        #region data

        public readonly ColorStyle Style;
        public readonly Boolean DoubleSided;
        public readonly UInt32 SmoothingGroups;

        // another interesting value would be SmoothingGroups, which can be used at triangulation.

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region * with

        private static readonly SurfaceStyle _Default = new SurfaceStyle(COLOR.Transparent);

        public static readonly SurfaceStyle Gray = _Default.With(COLOR.Gray);
        public static readonly SurfaceStyle Black = _Default.With(COLOR.Black);
        public static readonly SurfaceStyle White = _Default.With(COLOR.White);
        public static readonly SurfaceStyle Red = _Default.With(COLOR.Red);
        public static readonly SurfaceStyle Green = _Default.With(COLOR.Green);
        public static readonly SurfaceStyle Blue = _Default.With(COLOR.Blue);

        private static readonly SurfaceStyle _TwoSides = new SurfaceStyle(COLOR.Transparent, true);

        public static readonly SurfaceStyle TwoSides_Gray = _TwoSides.With(COLOR.Gray);
        public static readonly SurfaceStyle TwoSides_Black = _TwoSides.With(COLOR.Black);
        public static readonly SurfaceStyle TwoSides_White = _TwoSides.With(COLOR.White);
        public static readonly SurfaceStyle TwoSides_Red = _TwoSides.With(COLOR.Red);
        public static readonly SurfaceStyle TwoSides_Green = _TwoSides.With(COLOR.Green);
        public static readonly SurfaceStyle TwoSides_Blue = _TwoSides.With(COLOR.Blue);

        public SurfaceStyle With(ColorStyle style) { return new SurfaceStyle(style, this.DoubleSided); }

        public SurfaceStyle WithOutline(Single outlineWidth)
        {
            return new SurfaceStyle(Style.WithOutline(outlineWidth), this.DoubleSided);
        }

        #endregion
    }
}
