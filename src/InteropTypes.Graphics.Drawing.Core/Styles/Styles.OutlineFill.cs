using System;

using GDICOLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct OutlineFillStyle : IEquatable<OutlineFillStyle>
    {
        #region constructors

        public static implicit operator OutlineFillStyle(GDICOLOR fillColor) { return new OutlineFillStyle(fillColor); }
        public static implicit operator OutlineFillStyle((GDICOLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2); }
        public static implicit operator OutlineFillStyle((GDICOLOR, GDICOLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator OutlineFillStyle(ColorStyle fillColor) { return new OutlineFillStyle(fillColor); }
        public static implicit operator OutlineFillStyle((ColorStyle, float) style) { return new OutlineFillStyle(style.Item1, style.Item2); }        
        public static implicit operator OutlineFillStyle((ColorStyle, ColorStyle, float) style) { return new OutlineFillStyle(style.Item1, style.Item2, style.Item3); }

        [System.Diagnostics.DebuggerStepThrough]
        public OutlineFillStyle(ColorStyle fillColor)
        {
            FillColor = fillColor;
            OutlineColor = ColorStyle.Transparent;
            OutlineWidth = 0;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public OutlineFillStyle(ColorStyle outColor, float outWidth)
        {
            FillColor = ColorStyle.Transparent;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public OutlineFillStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
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
        public readonly override bool Equals(object obj) { return obj is OutlineFillStyle other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(OutlineFillStyle other)
        {
            return
                this.FillColor == other.FillColor &&
                this.OutlineColor == other.OutlineColor &&
                this.OutlineWidth == other.OutlineWidth;
        }

        /// <inheritdoc/>
        public static bool operator ==(OutlineFillStyle a, OutlineFillStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(OutlineFillStyle a, OutlineFillStyle b) => !a.Equals(b);

        #endregion

        #region properties

        public readonly bool IsEmpty => !IsVisible;

        public readonly bool IsVisible => HasFill || HasOutline;

        public readonly bool HasFill => FillColor.IsVisible;

        public readonly bool HasOutline => OutlineColor.IsVisible && OutlineWidth > 0;

        #endregion

        #region Fluent API

        public readonly OutlineFillStyle WithFill(ColorStyle fillColor)
        {
            return new OutlineFillStyle(fillColor, OutlineColor, OutlineWidth);
        }

        public readonly OutlineFillStyle WithOutline(ColorStyle outlineColor, float ow)
        {
            return new OutlineFillStyle(FillColor, outlineColor, ow);
        }

        public readonly OutlineFillStyle WithOutline(ColorStyle outlineColor)
        {
            return new OutlineFillStyle(FillColor, outlineColor, OutlineWidth);
        }

        public readonly OutlineFillStyle WithOutline(float ow)
        {
            return new OutlineFillStyle(FillColor, OutlineColor, ow);
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
