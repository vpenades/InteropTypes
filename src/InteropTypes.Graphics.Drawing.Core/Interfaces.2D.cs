using System;
using System.Collections.Generic;
using System.Text;



namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents the most fundamental drawing canvas for drawing simple polygons and images. 
    /// </summary>
    /// <remarks>
    /// Inherited by <see cref="ICanvas2D"/><br/>
    /// Derived classes can optionally implement:<br/>
    /// - <see cref="GlobalStyle.ISource"/><br/>
    /// - <see cref="IRenderTargetInfo"/><br/>
    /// - <see cref="IMeshCanvas2D"/><br/>
    /// </remarks>
    public interface ICoreCanvas2D
    {
        /// <summary>
        /// Draws a convex polygon, with the given fill color
        /// </summary>
        /// <param name="points">
        /// <para>
        /// The vertices of the polygon.
        /// If 2 points are passed, it should draw a thin line.
        /// If 1 point is passed, it should draw a point.
        /// </para>
        /// <para>
        /// Helper builder: <c>Point2.Params( (2,2), (3,5) )</c>
        /// </para>
        /// </param>
        /// <param name="fillColor">The color of the polygon</param>
        /// <remarks>        
        /// The caller must ensure the points represent a non degenerated,convex polygon.
        /// </remarks>
        void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor);

        /// <summary>
        /// Draws an image at the given location.
        /// </summary>
        /// <param name="transform">The location (SRT) where to draw the image.</param>
        /// <param name="style">The image style, which also contains the image itself.</param>
        void DrawImage(in XFORM2 transform, ImageStyle style);
    }    

    /// <summary>
    /// Represents a drawing canvas where we can draw vector graphics.
    /// </summary>
    /// <remarks>
    /// Inherited by <see cref="IDisposableCanvas2D"/>
    /// </remarks>
    public interface ICanvas2D : ICoreCanvas2D
    {
        /// <summary>
        /// Draws an asset.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>        
        /// <remarks>
        /// Assets are dependant on the backend implementation.        
        /// </remarks>
        void DrawAsset(in XFORM2 transform, ASSET asset);

        /// <summary>
        /// Draws/Fills a closed polygon.
        /// </summary>
        /// <remarks>
        /// The capacity of drawing non convex polygons depends on the backend.
        /// </remarks>
        /// <param name="points">The vertices of the polygon.</param>
        /// <param name="style">The outline and fill style.</param>        
        void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle style);

        /// <summary>
        /// Draws an open polyline.
        /// </summary>
        /// <param name="points">The points of the line.</param>
        /// <param name="diameter">The thickness of the line.</param>
        /// <param name="style">The style of the line.</param>
        void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, LineStyle style);

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="style">The fill and outline of the ellipse.</param>
        void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, OutlineFillStyle style);

        /// <summary>
        /// Draws a text line using the given font
        /// </summary>        
        /// <param name="transform">the location of the top-left corner of the text line</param>
        /// <param name="text">the text to draw</param>
        /// <param name="size">the vertical size of the text</param>
        /// <param name="font">the font</param>
        void DrawTextLine(in XFORM2 transform, string text, float size, FontStyle font);
    }    

    /// <summary>
    /// Represents a disposable <see cref="ICanvas2D"/>.
    /// </summary>
    public interface IDisposableCanvas2D : ICanvas2D, IDisposable { }
}
