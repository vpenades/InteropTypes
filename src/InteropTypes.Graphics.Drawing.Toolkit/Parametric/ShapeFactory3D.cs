using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Parametric
{
    static partial class ShapeFactory3D
    {
        #region constants

        #if NETSTANDARD2_1_OR_GREATER
        private const float PI = MathF.PI;
        #else
        private const float PI = (float)Math.PI;
        #endif

        #endregion
    }
}
