using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Combines an <see cref="OutlineFillStyle"/> with Double sided surface style.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IScene3D.DrawSurface(ReadOnlySpan{Point3}, SurfaceStyle)"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {DoubleSided}")]
    public readonly struct SurfaceStyle
    {
        #region implicit        

        public static implicit operator SurfaceStyle(COLOR color) { return new SurfaceStyle(color); }
        public static implicit operator SurfaceStyle((COLOR fillColor, bool doubleSided) style) { return new SurfaceStyle(style.fillColor, style.doubleSided); }
        public static implicit operator SurfaceStyle((COLOR fillColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outWidth); }
        public static implicit operator SurfaceStyle((COLOR fillColor, COLOR outColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outColor, style.outWidth); }
        public static implicit operator SurfaceStyle((COLOR fillColor, COLOR outColor, float outWidth, bool doubleSided) style) { return new SurfaceStyle((style.fillColor, style.outColor, style.outWidth), style.doubleSided); }


        public static implicit operator SurfaceStyle(ColorStyle color) { return new SurfaceStyle(color); }
        public static implicit operator SurfaceStyle((ColorStyle fillColor, bool doubleSided) style) { return new SurfaceStyle(style.fillColor, style.doubleSided); }
        public static implicit operator SurfaceStyle((ColorStyle fillColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outWidth); }
        public static implicit operator SurfaceStyle((ColorStyle fillColor, ColorStyle outColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outColor, style.outWidth); }
        public static implicit operator SurfaceStyle((ColorStyle fillColor, ColorStyle outColor, float outWidth, bool doubleSided) style) { return new SurfaceStyle((style.fillColor, style.outColor, style.outWidth), style.doubleSided); }

        public static implicit operator SurfaceStyle(PolygonStyle brush) { return new SurfaceStyle(brush, true); }
        public static implicit operator SurfaceStyle((PolygonStyle color, bool doubleSided) style) { return new SurfaceStyle(style.color, style.doubleSided); }

        public static implicit operator SurfaceStyle(OutlineFillStyle brush) { return new SurfaceStyle(brush, true); }
        public static implicit operator SurfaceStyle((OutlineFillStyle color, bool doubleSided) style) { return new SurfaceStyle(style.color, style.doubleSided); }

        #endregion

        #region constructors

        public SurfaceStyle(ColorStyle fillColor)
        {
            Style = fillColor;
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(fillColor, outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(ColorStyle fillColor, bool doubleSided)
        {
            Style = new OutlineFillStyle(fillColor);
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(OutlineFillStyle color, bool doubleSided)
        {
            Style = color;
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        public SurfaceStyle(PolygonStyle color, bool doubleSided)
        {
            Style = new OutlineFillStyle(color.FillColor, color.OutlineColor, color.OutlineWidth);
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        #endregion

        #region data

        public readonly OutlineFillStyle Style;
        public readonly bool DoubleSided;
        public readonly uint SmoothingGroups;

        // another interesting value would be SmoothingGroups, which can be used at triangulation.

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region * with

        private static readonly SurfaceStyle _Default = new SurfaceStyle(ColorStyle.Transparent);

        public static readonly SurfaceStyle Gray = _Default.With(ColorStyle.Gray);
        public static readonly SurfaceStyle Black = _Default.With(ColorStyle.Black);
        public static readonly SurfaceStyle White = _Default.With(ColorStyle.White);
        public static readonly SurfaceStyle Red = _Default.With(ColorStyle.Red);
        public static readonly SurfaceStyle Green = _Default.With(ColorStyle.Green);
        public static readonly SurfaceStyle Blue = _Default.With(ColorStyle.Blue);

        private static readonly SurfaceStyle _TwoSides = new SurfaceStyle(ColorStyle.Transparent, true);

        public static readonly SurfaceStyle TwoSides_Gray = _TwoSides.With(ColorStyle.Gray);
        public static readonly SurfaceStyle TwoSides_Black = _TwoSides.With(ColorStyle.Black);
        public static readonly SurfaceStyle TwoSides_White = _TwoSides.With(ColorStyle.White);
        public static readonly SurfaceStyle TwoSides_Red = _TwoSides.With(ColorStyle.Red);
        public static readonly SurfaceStyle TwoSides_Green = _TwoSides.With(ColorStyle.Green);
        public static readonly SurfaceStyle TwoSides_Blue = _TwoSides.With(ColorStyle.Blue);

        public SurfaceStyle With(OutlineFillStyle style) { return new SurfaceStyle(style, DoubleSided); }

        public SurfaceStyle WithOutline(float outlineWidth)
        {
            return new SurfaceStyle(Style.WithOutline(outlineWidth), DoubleSided);
        }

        #endregion
    }
}
