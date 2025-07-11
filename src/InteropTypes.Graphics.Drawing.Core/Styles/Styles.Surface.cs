﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Combines an <see cref="OutlineFillStyle"/> with Double sided surface style.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IScene3D.DrawSurface(ReadOnlySpan{Point3}, SurfaceStyle)"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {DoubleSided}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct SurfaceStyle : IEquatable<SurfaceStyle>
    {
        #region implicit        

        public static implicit operator SurfaceStyle(GDICOLOR color) { return new SurfaceStyle(color); }
        public static implicit operator SurfaceStyle((GDICOLOR fillColor, bool doubleSided) style) { return new SurfaceStyle(style.fillColor, style.doubleSided); }
        public static implicit operator SurfaceStyle((GDICOLOR fillColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outWidth); }
        public static implicit operator SurfaceStyle((GDICOLOR fillColor, GDICOLOR outColor, float outWidth) style) { return new SurfaceStyle(style.fillColor, style.outColor, style.outWidth); }
        public static implicit operator SurfaceStyle((GDICOLOR fillColor, GDICOLOR outColor, float outWidth, bool doubleSided) style) { return new SurfaceStyle((style.fillColor, style.outColor, style.outWidth), style.doubleSided); }


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

        [System.Diagnostics.DebuggerStepThrough]
        public SurfaceStyle(ColorStyle fillColor)
        {
            Style = fillColor;
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public SurfaceStyle(ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public SurfaceStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(fillColor, outColor, outWidth);
            DoubleSided = true;
            SmoothingGroups = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public SurfaceStyle(ColorStyle fillColor, bool doubleSided)
        {
            Style = new OutlineFillStyle(fillColor);
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public SurfaceStyle(OutlineFillStyle color, bool doubleSided)
        {
            Style = color;
            DoubleSided = doubleSided;
            SmoothingGroups = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
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

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            var h = Style.GetHashCode();
            h ^= DoubleSided.GetHashCode();
            h ^= SmoothingGroups.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is SurfaceStyle other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(SurfaceStyle other)
        {
            return
                this.Style == other.Style &&
                this.DoubleSided == other.DoubleSided &&
                this.SmoothingGroups == other.SmoothingGroups;
        }

        /// <inheritdoc/>
        public static bool operator ==(SurfaceStyle a, SurfaceStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(SurfaceStyle a, SurfaceStyle b) => !a.Equals(b);

        #endregion

        #region properties

        public readonly bool IsVisible => Style.IsVisible;

        #endregion

        #region Fluent API

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

        public readonly SurfaceStyle With(OutlineFillStyle style) { return new SurfaceStyle(style, DoubleSided); }

        public readonly SurfaceStyle WithOutline(float outlineWidth)
        {
            return new SurfaceStyle(Style.WithOutline(outlineWidth), DoubleSided);
        }

        #endregion
    }
}
