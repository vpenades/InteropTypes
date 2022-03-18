using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Graphics.Drawing
{
    internal class DummyBackend : ICanvas2D, IRenderTargetInfo
    {
        public int PixelsWidth { get; set; } = 100;

        public int PixelsHeight { get; set; } = 100;

        public float DotsPerInchX { get; set; } = 76;

        public float DotsPerInchY { get; set; } = 76;

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            
        }

        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle fillColor)
        {
            
        }

        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle style)
        {
            
        }

        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle style)
        {
            
        }
    }
}
