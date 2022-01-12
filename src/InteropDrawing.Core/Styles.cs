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

    /// <summary>
    /// Combines an <see cref="ColorStyle"/> with Line Cap styles.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IDrawing2D.DrawLines(ReadOnlySpan{Point2}, float, LineStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {StartCap} {EndCap}")]
    public readonly struct LineStyle
    {
        #region implicit

        public static implicit operator LineStyle(COLOR fillColor) { return new LineStyle(fillColor); }

        public static implicit operator LineStyle((COLOR, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }

        public static implicit operator LineStyle((COLOR, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((ColorStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }

        public static implicit operator LineStyle((ColorStyle, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((COLOR, Single) style) { return new LineStyle(style.Item1, style.Item2); }

        public static implicit operator LineStyle((COLOR, COLOR, Single) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((COLOR, COLOR, Single, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item4); }

        public static implicit operator LineStyle((COLOR, COLOR, Single, LineCapStyle, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item5); }

        #endregion

        #region constructors

        public LineStyle(COLOR fillColor)
        {
            Style = fillColor;
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(outColor, outWidth);
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR fillColor, COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(fillColor, outColor, outWidth);
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR fillColor, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = new ColorStyle(fillColor);
            StartCap = startCap;
            EndCap = endCap;
        }

        public LineStyle(ColorStyle color, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = color;
            StartCap = startCap;
            EndCap = endCap;
        }

        

        #endregion

        #region data

        public readonly ColorStyle Style;
        public readonly LineCapStyle StartCap;
        public readonly LineCapStyle EndCap;

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region With * API

        private static readonly LineStyle _Default = new LineStyle(COLOR.Transparent);

        public static readonly LineStyle Gray = _Default.With(COLOR.Gray);
        public static readonly LineStyle Black = _Default.With(COLOR.Black);
        public static readonly LineStyle White = _Default.With(COLOR.White);
        public static readonly LineStyle Red = _Default.With(COLOR.Red);
        public static readonly LineStyle Green = _Default.With(COLOR.Green);
        public static readonly LineStyle Blue = _Default.With(COLOR.Blue);
        public static readonly LineStyle Yellow = _Default.With(COLOR.Yellow);        

        public LineStyle With(LineCapStyle caps) { return new LineStyle(this.Style, caps, caps); }

        public LineStyle With(LineCapStyle startCap, LineCapStyle endCap) { return new LineStyle(this.Style, startCap, endCap); }

        public LineStyle With(ColorStyle style)
        {
            return new LineStyle(style, StartCap, EndCap);
        }

        public LineStyle WithOutline(Single ow)
        {
            return new LineStyle(Style.WithOutline(ow), StartCap, EndCap);
        }

        public LineStyle WithOutline(COLOR color, Single ow)
        {
            return new LineStyle(Style.WithOutline(color, ow), StartCap, EndCap);
        }

        #endregion
    }


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

        public static implicit operator SurfaceStyle(ColorStyle brush) { return new SurfaceStyle(brush, true); }

        public static implicit operator SurfaceStyle(COLOR fillColor) { return new SurfaceStyle(fillColor); }

        public static implicit operator SurfaceStyle((COLOR fillColor, Boolean doubleSided) style) { return new SurfaceStyle(style.fillColor, style.doubleSided); }

        public static implicit operator SurfaceStyle((COLOR fillColor, Single outWidth) style) { return new SurfaceStyle(style.fillColor, style.outWidth); }

        public static implicit operator SurfaceStyle((ColorStyle color, Boolean doubleSided) style) { return new SurfaceStyle(style.color, style.doubleSided); }

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

    /// <summary>
    /// Style used for font rendering WIP.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="Toolkit.DrawFont(IDrawing2D, System.Numerics.Matrix3x2, string, FontStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {Strength} {Alignment}")]
    public readonly struct FontStyle
    {
        #region implicit

        public static implicit operator FontStyle(COLOR fillColor) { return new FontStyle(fillColor); }

        public static implicit operator FontStyle((COLOR, Single) style) { return new FontStyle(style.Item1, style.Item2); }

        public static implicit operator FontStyle((COLOR, Single, FontAlignStyle) style) { return new FontStyle(style.Item1, style.Item2, style.Item3); }

        #endregion

        #region constructors

        public FontStyle(ColorStyle color, float strength = 0.1f)
        {
            Style = color;
            Strength = strength;
            Alignment = FontAlignStyle.None;
        }

        public FontStyle(ColorStyle color, float strength, FontAlignStyle align)
        {
            Style = color;
            Strength = strength;
            Alignment = align;
        }

        #endregion

        #region data

        public readonly ColorStyle Style;
        public readonly Single Strength;
        public readonly FontAlignStyle Alignment;

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region with* API

        private static readonly FontStyle _Default = new FontStyle(COLOR.Transparent, 0.1f);

        public static readonly FontStyle Gray = _Default.With(COLOR.Gray);
        public static readonly FontStyle Black = _Default.With(COLOR.Black);
        public static readonly FontStyle White = _Default.With(COLOR.White);
        public static readonly FontStyle Red = _Default.With(COLOR.Red);
        public static readonly FontStyle Green = _Default.With(COLOR.Green);
        public static readonly FontStyle Blue = _Default.With(COLOR.Blue);

        private static readonly FontStyle _VFlip = new FontStyle(COLOR.Transparent, 0.1f, FontAlignStyle.FlipVertical);

        public static readonly FontStyle VFlip_Gray = _VFlip.With(COLOR.Gray);
        public static readonly FontStyle VFlip_Black = _VFlip.With(COLOR.Black);
        public static readonly FontStyle VFlip_White = _VFlip.With(COLOR.White);
        public static readonly FontStyle VFlip_Red = _VFlip.With(COLOR.Red);
        public static readonly FontStyle VFlip_Green = _VFlip.With(COLOR.Green);
        public static readonly FontStyle VFlip_Blue = _VFlip.With(COLOR.Blue);

        public FontStyle With(ColorStyle style) { return new FontStyle(style, this.Strength, this.Alignment); }

        public FontStyle With(ColorStyle style, float strength) { return new FontStyle(style, strength, this.Alignment); }

        public FontStyle With(Single strength) { return new FontStyle(this.Style, strength, this.Alignment); }        

        public FontStyle With(FontAlignStyle align)
        {
            align |= this.Alignment;

            return new FontStyle(this.Style, this.Strength, align);
        }

        #endregion
    }    

    /// <summary>
    /// Style used for sprite rendering
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IDrawing2D.DrawSprite(in System.Numerics.Matrix3x2, in SpriteStyle)"/>.
    /// </remarks>
    public struct SpriteStyle
    {
        #region implicit

        public static implicit operator SpriteStyle(SpriteAsset asset) { return new SpriteStyle(asset, COLOR.White, false, false); }

        // public static implicit operator SpriteStyle((BitmapCell bitmap, float opacity) args) { return new SpriteStyle(args.bitmap, new COLOR((Byte)255, (Byte)255, (Byte)255, ((Byte)(args.opacity * 255)).Clamp(0, 255)), false, false); }

        public static implicit operator SpriteStyle((SpriteAsset asset, COLOR color) args) { return new SpriteStyle(args.asset, args.color, false, false); }

        public static implicit operator SpriteStyle((SpriteAsset, COLOR, bool, bool) tuple) { return new SpriteStyle(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4); }

        public static implicit operator SpriteStyle((SpriteAsset, bool, bool) tuple) { return new SpriteStyle(tuple.Item1, COLOR.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public SpriteStyle(SpriteAsset bitmap, COLOR color, bool flipHorizontal, bool flipVertical)
        {
            this.Bitmap = bitmap;
            this.Color = color;

            _Orientation = Orientation.None;
            _Orientation |= flipHorizontal ? Orientation.FlipHorizontal : Orientation.None;
            _Orientation |= flipVertical ? Orientation.FlipVertical : Orientation.None;
        }

        #endregion

        #region data

        public SpriteAsset Bitmap;

        public COLOR Color;

        internal Orientation _Orientation;        

        #endregion

        #region properties

        public bool IsVisible => Bitmap.IsVisible && Color.A > 0;

        public bool FlipHorizontal
        {
            get => _Orientation.HasFlag(Orientation.FlipHorizontal);
            set => _Orientation = (_Orientation & ~Orientation.FlipHorizontal) | (value ? Orientation.FlipHorizontal : Orientation.None);
        }

        public bool FlipVertical
        {
            get => _Orientation.HasFlag(Orientation.FlipVertical);
            set => _Orientation = (_Orientation & ~Orientation.FlipVertical) | (value ? Orientation.FlipVertical : Orientation.None);
        }

        #endregion

        #region API

        public System.Numerics.Matrix3x2 Transform => Bitmap.GetSpriteMatrix(FlipHorizontal, FlipVertical);

        public System.Numerics.Matrix3x2 GetTransform(bool hflip, bool vflip)
        {
            return Bitmap.GetSpriteMatrix(FlipHorizontal ^ hflip, FlipVertical ^ vflip);
        }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform, bool hflip, bool vflip)
        {
            Bitmap.PrependTransform(ref xform, FlipHorizontal ^ hflip, FlipVertical ^ vflip);
        }

        #endregion

        #region nested types

        [Flags]
        internal enum Orientation
        {
            None = 0,
            FlipHorizontal =1,
            FlipVertical = 2
        }

        #endregion
    }

    /// <summary>
    /// Line cap styles used by <see cref="LineStyle"/>.
    /// </summary>
    public enum LineCapStyle
    {
        /// <summary>
        /// A cap that does not extend past the last point of the line. If CanvasStrokeStyle.DashCap
        /// is set to Flat, dots will have zero size so only dashes are visible.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// Half of a square that has a length equal to the line thickness.
        /// </summary>
        Square = 1,

        /// <summary>
        /// A semicircle that has a diameter equal to the line thickness.
        /// </summary>
        Round = 2,

        /// <summary>
        /// An isosceles right triangle whose hypotenuse is equal in length to the thickness
        /// of the line.
        /// </summary>
        Triangle = 3
    }

    /// <summary>
    /// Font drawing modes used by <see cref="FontStyle"/>.
    /// </summary>
    [Flags]
    public enum FontAlignStyle
    {
        None,
        FlipHorizontal = 1,
        FlipVertical = 2,

        // to determine the text origin, calculate the points of the 4 corners, and
        // based on the values below, apply weights to the points.

        DockLeft = 4,
        DockRight = 8,
        DockTop = 16,
        DockBottom = 32,
        Center = DockLeft | DockRight | DockLeft | DockBottom
    }
}
