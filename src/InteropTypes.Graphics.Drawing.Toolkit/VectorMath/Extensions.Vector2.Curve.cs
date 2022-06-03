using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    static partial class _SystemNumericsExtensions
    {
        /// <summary>
        /// Interpolates over a cuadratic curve defined by 3 points
        /// </summary>
        public static Vector2 LerpCurve(this in (Vector2 P1, Vector2 P2, Vector2 P3) curve, float amount)
        {
            var p12 = Vector2.Lerp(curve.P1, curve.P2, amount);
            var p23 = Vector2.Lerp(curve.P2, curve.P3, amount);
            return Vector2.Lerp(p12, p23, amount);
        }

        /// <summary>
        /// Interpolates over a cubic curve defined by 4 points
        /// </summary>
        public static Vector2 LerpCurve(this in (Vector2 P1, Vector2 P2, Vector2 P3, Vector2 P4) curve, float amount)
        {
            var squared = amount * amount;
            var cubed = squared * amount;

            // calculate weights
            var w4 = (3.0f * squared) - (2.0f * cubed);
            var w1 = 1 - w4;
            var w3 = cubed - squared;
            var w2 = w3 - squared + amount;

            // convert p2 and p3 to tangent vectors:
            var t12 = curve.P2 - curve.P1; // forward tangent at P1
            var t34 = curve.P4 - curve.P3; // backward tangent at P4

            return curve.P1 * w1 + t12 * w2 + t34 * w3 + curve.P4 * w4;
        }
    }
}
