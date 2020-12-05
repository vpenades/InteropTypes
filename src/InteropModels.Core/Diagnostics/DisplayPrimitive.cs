using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

using InteropBitmaps;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using POINTI = System.Drawing.Point;
using POINTF = System.Drawing.PointF;
using SIZEI = System.Drawing.Size;
using SIZEF = System.Drawing.SizeF;

namespace InteropModels
{
	[DebuggerDisplay("{GetShapeType()}")]
	public struct DisplayPrimitive
	{
		#region lifecycle

		public static DisplayPrimitive Point(Color color, DisplayVector2 a, int size)
		{
			var num = 0.5f * (float)size;
			var value = new RectangleF(a.X - num, a.Y - num, num, num);
			return new DisplayPrimitive(color, Rectangle.Round(value));
		}

		public static DisplayPrimitive Line(Color color, DisplayVector2 a, DisplayVector2 b)
		{
			return new DisplayPrimitive(color, a, b)
			{
				IsLine = true
			};
		}

		public static DisplayPrimitive RectFromPoints(Color color, params DisplayVector2[] points)
		{
			var (min, max) = points
				.Select(item => item.ToVector2())
				.MinMax();

			var size = max - min;

			var rect = new RectangleF(min.X, min.Y, size.X, size.Y);
			return new DisplayPrimitive(color, rect);
		}

		public static DisplayPrimitive Rect(Color color, RectangleF rect)
		{
			return new DisplayPrimitive(color, rect);
		}

		public static DisplayPrimitive Rect(Color color, float x, float y, float w, float h)
		{
			return new DisplayPrimitive(color, new RectangleF(x, y, w, h));
		}

		public static DisplayPrimitive Rect(Color color, DisplayVector2 tl, DisplayVector2 wh)
		{
			var rect = new RectangleF(tl.ToPoint(), wh.ToSize());
			return new DisplayPrimitive(color, rect);
		}


		public static IEnumerable<DisplayPrimitive> Rect(Color color, DisplayVector2 tl, DisplayVector2 wh, Matrix3x2 xform)
		{
			if (xform.M12 == 0f && xform.M21 == 0f)
			{
				tl = DisplayVector2.Transform(tl, xform);
				wh = DisplayVector2.Transform(wh, xform);
				yield return Rect(color, tl, wh);
				yield break;
			}

			var displayVector = DisplayVector2.Transform(tl.X, tl.Y, xform);
			var displayVector2 = DisplayVector2.Transform(tl.X + wh.X, tl.Y, xform);
			var displayVector3 = DisplayVector2.Transform(tl.X + wh.X, tl.Y + wh.Y, xform);
			var displayVector4 = DisplayVector2.Transform(tl.X, tl.Y + wh.Y, xform);

			yield return Line(color, displayVector, displayVector2);
			yield return Line(color, displayVector2, displayVector3);
			yield return Line(color, displayVector3, displayVector4);
			yield return Line(color, displayVector4, displayVector);
			yield break;
		}

		public static DisplayPrimitive Write(Color color, DisplayVector2 p, string text, float size)
		{
			var result = Rect(color, p.X, p.Y, 1f, size);
			result.TextLine = text;
			return result;
		}

		private DisplayPrimitive(Color color, DisplayVector2 a, DisplayVector2 b)
		{
			IsLine = true;
			A = a;
			B = b;
			Color = color;
			TextLine = null;
		}

		private DisplayPrimitive(Color color, RectangleF rect)
		{
			if (rect.Width < 0f || rect.Height < 0f) { throw new ArgumentOutOfRangeException(); }

			IsLine = false;
			A = rect.Location;
			B = rect.Location + rect.Size;
			Color = color;
			TextLine = null;
		}

		#endregion		

        #region data

        private bool IsLine;

		public DisplayVector2 A;

		public DisplayVector2 B;

		public Color Color;

		public string TextLine;

        #endregion

        #region API		

		public Shape GetShapeType()
		{
			if (IsLine) return Shape.Line;
			if (!string.IsNullOrWhiteSpace(TextLine)) return Shape.Text;
			return Shape.Rectangle;
		}

		public RectangleF AsRectangle()
		{
			if (IsLine) throw new InvalidOperationException();
			return new RectangleF(A.ToPoint(), (B.ToVector2() - A.ToVector2()).ToSize());
		}

		public (POINTF A, POINTF B) AsLine()
		{
			if (!IsLine) throw new InvalidOperationException();
			return (A.ToPoint(), B.ToPoint());
		}

		#endregion

		#region nested types

		public enum Shape { Rectangle, Line, Point, Text }

		public interface ISource
		{
			IEnumerable<DisplayPrimitive> GetDisplayPrimitives();
		}

		public interface IBitmapSource
		{
			MemoryBitmap GetDisplayBitmap();
		}

		#endregion
	}
}
