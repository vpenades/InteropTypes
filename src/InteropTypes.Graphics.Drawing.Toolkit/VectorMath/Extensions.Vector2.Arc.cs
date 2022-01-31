using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
	static partial class _SystemNumericsExtensions
	{
		// Adapted from: https://github.com/MadLittleMods/svg-curve-lib/tree/master/src/c%2B%2B		

		public static Vector2 LerpArc(this (Vector2 p0, Vector2 p1, Vector2 rad, float xrotRadians, bool largeFlag, bool sweepFlag) arc, float t)
		{
			// rad.X and rad.Y must be positive
			var rad = Vector2.Abs(arc.rad);

			// If the endpoints are identical, then this is equivalent
			// to omitting the elliptical arc segment entirely.
			if (arc.p0 == arc.p1) return arc.p0;

			// If rad is zero then this arc is treated as a
			// straight line segment joining the endpoints.    
			if (rad == Vector2.Zero) return Vector2.Lerp(arc.p0, arc.p1, t);

			// Following "Conversion from endpoint to center parameterization"
			// http://www.w3.org/TR/SVG/implnote.html#ArcConversionEndpointToCenter
			
			var xrotSin = MathF.Sin(arc.xrotRadians);
			var xrotCos = MathF.Cos(arc.xrotRadians);

			// Step #1: Compute transformedPoint
			var d = (arc.p0 - arc.p1) / 2f;
			var xformPoint = new Vector2(xrotCos * d.X + xrotSin * d.Y, -xrotSin * d.X + xrotCos * d.Y);			

			// Ensure radii are large enough
			var radiiCheck = Vector2.Dot(xformPoint, xformPoint) / (rad.X * rad.X);
			if (radiiCheck > 1) rad *= MathF.Sqrt(radiiCheck);

			var rx2 = rad.X * rad.X;
			var ry2 = rad.Y * rad.Y;

			// Step #2: Compute transformedCenter			

			var cSquareNumerator
				= rx2 * ry2
				- rx2 * (xformPoint.Y * xformPoint.Y)
				- ry2 * (xformPoint.X * xformPoint.X);

			var cSquareRootDenom
				= rx2 * (xformPoint.Y * xformPoint.Y)
				+ ry2 * (xformPoint.X * xformPoint.X);

			var cRadicand = cSquareNumerator / cSquareRootDenom;

			// Make sure this never drops below zero because of precision
			cRadicand = cRadicand < 0 ? 0 : cRadicand;
			var cCoef = (arc.largeFlag != arc.sweepFlag ? 1 : -1) * MathF.Sqrt(cRadicand);			
			
			var xformCenter = new Vector2
                ( cCoef * (rad.X * xformPoint.Y / rad.Y)
				, -cCoef * (rad.Y * xformPoint.X / rad.X) );

			// Step #3: Compute center
			var center = (arc.p0 + arc.p1) / 2;
			center += new Vector2
                ( xrotCos * xformCenter.X - xrotSin * xformCenter.Y
				, xrotSin * xformCenter.X + xrotCos * xformCenter.Y);


			// Step #4: Compute start/sweep angles
			// Start angle of the elliptical arc prior to the stretch and rotate operations.
			// Difference between the start and end angles
			var startVector = (xformPoint - xformCenter) / rad;
			var startAngle = _AngleBetween(Vector2.UnitX, startVector);

			var endVector = (-xformPoint - xformCenter) / rad;
			var sweepAngle = _AngleBetween(startVector, endVector);

			if (!arc.sweepFlag && sweepAngle > 0) sweepAngle -= 2 * MathF.PI;
			else if (arc.sweepFlag && sweepAngle < 0) sweepAngle += 2 * MathF.PI;

			// We use % instead of `mod(..)` because we want it to be -360deg to 360deg(but actually in radians)
			sweepAngle %= 2 * MathF.PI;

			// From http://www.w3.org/TR/SVG/implnote.html#ArcParameterizationAlternatives
			var angle = startAngle + (sweepAngle * t);

			var ellipseComponent = rad * new Vector2(MathF.Cos(angle), MathF.Sin(angle));

			var point = center + new Vector2
				( xrotCos * ellipseComponent.X - xrotSin * ellipseComponent.Y
				, xrotSin * ellipseComponent.X + xrotCos * ellipseComponent.Y);

			return point;
		}

		private static float _AngleBetween(in Vector2 v0, in Vector2 v1)
		{
			var p = Vector2.Dot(v0, v1) / (v0.Length() * v1.Length());
			if (p < -1) p = -1;
			if (p > 1) p = 1;

			var angle = MathF.Acos(p);

			angle *= (v0.X * v1.Y - v0.Y * v1.X) < 0 ? -1 : 1; // sign			

			//var angle = Math.atan2(v0.y, v0.x) - Math.atan2(v1.y,  v1.x);

			return angle;

		}
	}
}
