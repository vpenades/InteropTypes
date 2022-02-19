﻿using System;
using System.Collections.Generic;
using System.Text;

using GDICOLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct PolygonStyle : IEquatable<PolygonStyle>
    {
        #region constructors

        public static implicit operator PolygonStyle(GDICOLOR fillColor) { return new PolygonStyle(fillColor); }
        public static implicit operator PolygonStyle((GDICOLOR, float) style) { return new PolygonStyle(style.Item1, style.Item2); }
        public static implicit operator PolygonStyle((GDICOLOR, GDICOLOR, float) style) { return new PolygonStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator PolygonStyle(ColorStyle fillColor) { return new PolygonStyle(fillColor); }
        public static implicit operator PolygonStyle((ColorStyle, float) style) { return new PolygonStyle(style.Item1, style.Item2); }        
        public static implicit operator PolygonStyle((ColorStyle, ColorStyle, float) style) { return new PolygonStyle(style.Item1, style.Item2, style.Item3); }
        public static implicit operator PolygonStyle(OutlineFillStyle style) { return new PolygonStyle(style.FillColor, style.OutlineColor, style.OutlineWidth); }

        public PolygonStyle(ColorStyle fillColor)
        {
            FillColor = fillColor;
            OutlineColor = ColorStyle.Transparent;
            OutlineWidth = 0;
        }

        public PolygonStyle(ColorStyle outColor, float outWidth)
        {
            FillColor = ColorStyle.Transparent;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        public PolygonStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
        {
            FillColor = fillColor;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        #endregion

        #region data

        public readonly ColorStyle FillColor;
        public readonly ColorStyle OutlineColor;
        public readonly float OutlineWidth;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var h = FillColor.GetHashCode();
            h ^= OutlineColor.GetHashCode();
            h ^= OutlineWidth.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is PolygonStyle other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(PolygonStyle other)
        {
            return
                this.FillColor == other.FillColor &&
                this.OutlineColor == other.OutlineColor &&
                this.OutlineWidth == other.OutlineWidth;
        }

        /// <inheritdoc/>
        public static bool operator ==(PolygonStyle a, PolygonStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(PolygonStyle a, PolygonStyle b) => !a.Equals(b);

        #endregion

        #region properties

        public bool IsVisible => HasFill || HasOutline;

        public bool HasFill => FillColor.IsVisible;

        public bool HasOutline => OutlineColor.IsVisible && OutlineWidth > 0;

        #endregion

        #region With * API        

        public PolygonStyle WithFill(ColorStyle fillColor)
        {
            return new PolygonStyle(fillColor, OutlineColor, OutlineWidth);
        }

        public PolygonStyle WithOutline(ColorStyle outlineColor, float ow)
        {
            return new PolygonStyle(FillColor, outlineColor, ow);
        }

        public PolygonStyle WithOutline(ColorStyle outlineColor)
        {
            return new PolygonStyle(FillColor, outlineColor, OutlineWidth);
        }

        public PolygonStyle WithOutline(float ow)
        {
            return new PolygonStyle(FillColor, OutlineColor, ow);
        }

        public bool IsSolid(ref float diameter, out ColorStyle solidColor)
        {
            if (OutlineColor.IsVisible && diameter < OutlineWidth)
            {
                diameter = OutlineWidth;
                solidColor = OutlineColor;
                return true;
            }

            if (!HasOutline) { solidColor = FillColor; return true; }

            solidColor = default;
            return false;
        }

        #endregion
    }
}