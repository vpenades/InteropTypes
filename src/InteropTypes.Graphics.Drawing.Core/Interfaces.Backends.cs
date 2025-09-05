using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// provides additional information about the physical viewport used by the backend.
    /// </summary>
    /// <remarks>
    /// This interface must be implemented by the underlaying rendering backend, and
    /// queried through any exposed <see cref="ICanvas2D"/> casted to a
    /// <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IRenderTargetInfo
    {
        public static bool TryGetBackendViewportBounds<T>(T dc, out GDIRECTF bounds) where T : ICanvas2D
        {
            bounds = default;

            var rti = GetRenderTargetInfoOrDefault(dc);
            if (rti == null) return false;

            var w = rti.PixelsWidth;
            var h = rti.PixelsHeight;

            var xform = ITransformer2D.GetTransformer2DOrDefault(dc);
            if (xform == null)
            {
                bounds = new GDIRECTF(0, 0, w, h);
                return true;
            }

            Span<Point2> points = stackalloc Point2[4];
            points[0] = (0, 0);
            points[1] = (w, 0);
            points[2] = (0, h);
            points[3] = (w, h);
            xform.TransformInverse(points); // from screen space to dc space.

            bounds = GDIRECTF.Empty;
            bool first = true;

            foreach (var p in points)
            {
                var other = new GDIRECTF(p.X, p.Y, 0, 0);
                bounds = first ? other : GDIRECTF.Union(bounds, other);
                first = false;
            }

            return true;
        }

        public static IRenderTargetInfo GetRenderTargetInfoOrDefault<T>(T canvas) where T:ICanvas2D
        {
            switch (canvas)
            {
                case IRenderTargetInfo rti: return rti;
                case IServiceProvider srv: return srv.GetService(typeof(IRenderTargetInfo)) as IRenderTargetInfo;
                default: return null;
            }
        }

        /// <summary>
        /// The viewport width in pixels
        /// </summary>
        int PixelsWidth { get; }

        /// <summary>
        /// The viewport height in pixels
        /// </summary>
        int PixelsHeight { get; }

        float DotsPerInchX { get; }

        float DotsPerInchY { get; }        
    }
}
