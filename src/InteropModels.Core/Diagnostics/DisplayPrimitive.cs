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

using POINT = InteropDrawing.Point2;

namespace InteropModels
{
	[DebuggerDisplay("{GetShapeType()}")]
	public struct DisplayPrimitive
	{
		#region lifecycle

		public static DisplayPrimitive Point(Color color, POINT a, int size)
		{
			var num = 0.5f * (float)size;
			var value = new RectangleF(a.X - num, a.Y - num, num, num);
			return new DisplayPrimitive(color, Rectangle.Round(value));
		}

		public static DisplayPrimitive Line(Color color, POINT a, POINT b)
		{
			return new DisplayPrimitive(color, a, b)
			{
				IsLine = true
			};
		}

		public static DisplayPrimitive RectFromPoints(Color color, params POINT[] points)
		{
			var (min, max) = points
				.Select(item => item.ToNumerics())
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

		public static DisplayPrimitive Rect(Color color, POINT tl, POINT wh)
		{
			var rect = new RectangleF(tl.ToGDIPoint(), wh.ToGDISize());
			return new DisplayPrimitive(color, rect);
		}


		public static IEnumerable<DisplayPrimitive> Rect(Color color, POINT tl, POINT wh, Matrix3x2 xform)
		{
			if (xform.M12 == 0f && xform.M21 == 0f)
			{
				tl = POINT.Transform(tl, xform);
				wh = POINT.Transform(wh, xform);
				yield return Rect(color, tl, wh);
				yield break;
			}

			var displayVector = POINT.Transform(tl.X, tl.Y, xform);
			var displayVector2 = POINT.Transform(tl.X + wh.X, tl.Y, xform);
			var displayVector3 = POINT.Transform(tl.X + wh.X, tl.Y + wh.Y, xform);
			var displayVector4 = POINT.Transform(tl.X, tl.Y + wh.Y, xform);

			yield return Line(color, displayVector, displayVector2);
			yield return Line(color, displayVector2, displayVector3);
			yield return Line(color, displayVector3, displayVector4);
			yield return Line(color, displayVector4, displayVector);
			yield break;
		}

		public static DisplayPrimitive Write(Color color, POINT p, string text, float size)
		{
			var result = Rect(color, p.X, p.Y, 1f, size);
			result.TextLine = text;
			return result;
		}

		private DisplayPrimitive(Color color, POINT a, POINT b)
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

		public POINT A;

		public POINT B;

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
			return new RectangleF(A.ToGDIPoint(), (B.ToNumerics() - A.ToNumerics()).ToSize());
		}

		public (POINTF A, POINTF B) AsLine()
		{
			if (!IsLine) throw new InvalidOperationException();
			return (A.ToGDIPoint(), B.ToGDIPoint());
		}

		public static void Draw(InteropDrawing.IDrawing2D dc, IEnumerable<DisplayPrimitive> primitives, float fontsize = 1, bool fontflip = false)
        {
			foreach(var primitive in primitives)
            {
				switch(primitive.GetShapeType())
                {
					case Shape.Point:
                        {
							InteropDrawing.Toolkit.DrawCircle(dc, primitive.A, 1, primitive.Color);
							break;
                        }

					case Shape.Line:
                        {
							var l = primitive.AsLine();
							InteropDrawing.Toolkit.DrawLine(dc, l.A, l.B, 1, primitive.Color);
							break;
                        }
					case Shape.Rectangle:
                        {
							var r = primitive.AsRectangle();
							InteropDrawing.Toolkit.DrawRectangle(dc, r.Location, r.Size, primitive.Color);
							break;
                        }
					case Shape.Text:
                        {
							var style = new InteropDrawing.FontStyle(primitive.Color);
							if (fontflip) style = style.With(InteropDrawing.FontAlignStyle.FlipVertical);

							InteropDrawing.Toolkit.DrawFont(dc, primitive.A, fontsize, primitive.TextLine, style);
							break;
                        }
                }
            }
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
