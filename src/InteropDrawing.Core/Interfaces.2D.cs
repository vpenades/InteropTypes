using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;

/* Unmerged change from project 'InteropDrawing.Core (netstandard2.1)'
Before:
using POINT2 = InteropDrawing.Point2;
After:
using POINT2 = InteropDrawing.Point2;
using InteropDrawing;
using InteropTypes.Graphics.Drawing;
*/
using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a drawing canvas where we can draw 2D convex polygons and images
    /// </summary>
    public interface IPrimitiveCanvas2D
    {
        /// <summary>
        /// Draws a convex polygon, with the given fill color
        /// </summary>
        /// <param name="points">
        /// The vertices of the polygon.
        /// If 2 points are passed, it should draw a thin line.
        /// If 1 point is passed, it should draw a point.
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
        void DrawImage(in XFORM2 transform, in ImageStyle style);
    }

    /// <summary>
    /// Represents a drawing canvas where we can draw vector graphics.
    /// </summary>    
    public interface ICanvas2D : IPrimitiveCanvas2D
    {
        /// <summary>
        /// Draws an asset.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>
        /// <param name="style">The style to apply to the asset.</param>
        /// <remarks>
        /// Assets are dependant on the backend implementation.        
        /// </remarks>
        void DrawAsset(in XFORM2 transform, ASSET asset, ColorStyle style);

        /// <summary>
        /// Draws/Fills a closed polygon.
        /// </summary>
        /// <param name="points">The vertices of the polygon.</param>
        /// <param name="style">The outline and fill style.</param>        
        void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style);

        /// <summary>
        /// Draws an open polyline.
        /// </summary>
        /// <param name="points">The points of the line.</param>
        /// <param name="diameter">The thickness of the line.</param>
        /// <param name="style">The style of the line.</param>
        void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style);

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="style">The fill and outline of the ellipse.</param>
        void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, in OutlineFillStyle style);
    }

    /// <summary>
    /// Represents a disposable <see cref="ICanvas2D"/>.
    /// </summary>
    public interface IDisposableCanvas2D : ICanvas2D, IDisposable { }
}
