using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// provides additional information about the rendering backend
    /// </summary>
    /// <remarks>
    /// This interface must be implemented by the underlaying rendering backend, and
    /// queried through any exposed <see cref="ICanvas2D"/> casted to a
    /// <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IBackendViewportInfo
    {
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
