using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numerics;
using System.Drawing;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using POINTI = System.Drawing.Point;
using POINTF = System.Drawing.PointF;
using SIZEI = System.Drawing.Size;
using SIZEF = System.Drawing.SizeF;

namespace InteropModels
{
	[DebuggerDisplay("{X} {Y}")]
	public struct DisplayVector2
	{
		#region lifecycle

		public DisplayVector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		#endregion

		#region data

		public float X;

		public float Y;

		#endregion

		#region operators

		public static implicit operator DisplayVector2((int x, int y) point)
		{
			return new DisplayVector2((float)point.x, (float)point.y);
		}

		public static implicit operator DisplayVector2((float x, float y) point)
		{
			return new DisplayVector2(point.x, point.y);
		}

		public static implicit operator DisplayVector2(XY point)
		{
			return new DisplayVector2(point.X, point.Y);
		}

		public static implicit operator DisplayVector2(XYZ point)
		{
			return new DisplayVector2(point.X, point.Y);
		}

		public static implicit operator DisplayVector2(POINTI point)
		{
			return new DisplayVector2((float)point.X, (float)point.Y);
		}

		public static implicit operator DisplayVector2(POINTF point)
		{
			return new DisplayVector2(point.X, point.Y);
		}

		public static implicit operator DisplayVector2(SIZEI point)
		{
			return new DisplayVector2((float)point.Width, (float)point.Height);
		}

		public static implicit operator DisplayVector2(SIZEF point)
		{
			return new DisplayVector2(point.Width, point.Height);
		}

		public static implicit operator DisplayVector2(Rectangle rect)
		{
			return new DisplayVector2((float)rect.X + (float)rect.Width / 2f, (float)rect.Y + (float)rect.Height / 2f);
		}

		public static implicit operator DisplayVector2(RectangleF rect)
		{
			return new DisplayVector2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
		}

		#endregion

		#region API

		public static DisplayVector2 Lerp(DisplayVector2 a, DisplayVector2 b, float amount)
		{
			return XY.Lerp(a.ToVector2(), b.ToVector2(), amount);
		}

		public static DisplayVector2 Transform(DisplayVector2 v, Matrix3x2 xform)
		{
			return XY.Transform(v.ToVector2(), xform);
		}

		public static DisplayVector2 Transform(float x, float y, Matrix3x2 xform)
		{
			return XY.Transform(new XY(x, y), xform);
		}		

		public XY ToVector2() { return new XY(X, Y); }

		public POINTF ToPoint() { return new POINTF(X, Y); }

		public SIZEF ToSize() { return new SIZEF(X, Y); }

		public RectangleF ToCenteredRectangle(float size)
		{
			return new RectangleF(this.X - size / 2f, this.Y - size / 2f, size, size);
		}

		#endregion
	}
}
