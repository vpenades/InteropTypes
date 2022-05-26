using System;
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

        [System.Diagnostics.DebuggerStepThrough]
        public PolygonStyle(ColorStyle fillColor)
        {
            FillColor = fillColor;
            OutlineColor = ColorStyle.Transparent;
            OutlineWidth = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public PolygonStyle(ColorStyle outColor, float outWidth)
        {
            FillColor = ColorStyle.Transparent;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        [System.Diagnostics.DebuggerStepThrough]
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
        public readonly override int GetHashCode()
        {
            var h = FillColor.GetHashCode();
            h ^= OutlineColor.GetHashCode();
            h ^= OutlineWidth.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is PolygonStyle other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(PolygonStyle other)
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

        public readonly bool IsVisible => HasFill || HasOutline;

        public readonly bool HasFill => FillColor.IsVisible;

        public readonly bool HasOutline => OutlineColor.IsVisible && OutlineWidth > 0;

        #endregion

        #region Fluent API       

        public readonly PolygonStyle WithFill(ColorStyle fillColor)
        {
            return new PolygonStyle(fillColor, OutlineColor, OutlineWidth);
        }

        public readonly PolygonStyle WithOutline(ColorStyle outlineColor, float ow)
        {
            return new PolygonStyle(FillColor, outlineColor, ow);
        }

        public readonly PolygonStyle WithOutline(ColorStyle outlineColor)
        {
            return new PolygonStyle(FillColor, outlineColor, OutlineWidth);
        }

        public readonly PolygonStyle WithOutline(float ow)
        {
            return new PolygonStyle(FillColor, OutlineColor, ow);
        }

        public readonly bool IsSolid(ref float diameter, out ColorStyle solidColor)
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
