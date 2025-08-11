using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    using PRIMITIVE2D = ICoreCanvas2D;    

    partial class DrawingToolkit
    {
        public static GDIRECTF GetBackendViewportBoundsOrDefault(this PRIMITIVE2D dc)
        {
            if (TryGetBackendViewportBounds(dc, out var viewport)) return viewport;

            throw new ArgumentException($"Backend must implement interface {nameof(IRenderTargetInfo)}", nameof(dc));
        }

        public static bool TryGetBackendViewportBounds(this PRIMITIVE2D dc, out GDIRECTF bounds)
        {
            bounds = GDIRECTF.Empty;

            if (!(dc is IServiceProvider srv)) return false;
            if (!(srv.GetService(typeof(IRenderTargetInfo)) is IRenderTargetInfo vinfo)) return false;
            if (!(dc is ITransformer2D xform)) return false;

            var w = vinfo.PixelsWidth;
            var h = vinfo.PixelsHeight;

            Span<POINT2> points = stackalloc POINT2[4];
            points[0] = (0, 0);
            points[1] = (w, 0);
            points[2] = (0, h);
            points[3] = (w, h);
            xform.TransformInverse(points); // from screen space to dc space.            
            
            bool first = true;

            foreach (var p in points)
            {
                var other = new GDIRECTF(p.X, p.Y, 0, 0);
                bounds = first ? other : GDIRECTF.Union(bounds, other);
                first = false;
            }

            return true;
        }
    }
}
