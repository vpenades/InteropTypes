using System;
using System.Collections.Generic;
using System.Text;

using POINT = System.Drawing.Point;
using POINTPROTO = InteropTypes.Proto.Point32S;

using POINTF = System.Drawing.PointF;
using POINTFPROTO = InteropTypes.Proto.Point32F;

using SIZE = System.Drawing.Size;
using SIZEPROTO = InteropTypes.Proto.Size32S;

using SIZEF = System.Drawing.SizeF;
using SIZEFPROTO = InteropTypes.Proto.Size32F;

using RECTANGLE = System.Drawing.Rectangle;
using RECTANGLEPROTO = InteropTypes.Proto.Rectangle32S;

using RECTANGLEF = System.Drawing.RectangleF;
using RECTANGLEFPROTO = InteropTypes.Proto.Rectangle32F;


namespace InteropTypes.Proto
{
    partial class Point32S
    {
        public static readonly POINTPROTO Empty = POINT.Empty;

        public static implicit operator POINTPROTO(POINT point)
        {
            return new POINTPROTO(point);
        }

        public Point32S(POINT point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }
        public POINT ToPoint()
        {
            return new POINT(this.X, this.Y);
        }
    }

    partial class Point32F
    {
        public static readonly POINTFPROTO Empty = POINTF.Empty;

        public static implicit operator POINTFPROTO(POINT point)
        {
            return new POINTFPROTO(point);
        }

        public static implicit operator POINTFPROTO(POINTF point)
        {
            return new POINTFPROTO(point);
        }

        public Point32F(POINT point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public Point32F(POINTF point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public POINTF ToPointF()
        {
            return new POINTF(this.X, this.Y);
        }
    }

    partial class Rectangle32S
    {
        public static implicit operator RECTANGLEPROTO(RECTANGLE rect)
        {
            return new RECTANGLEPROTO(rect);
        }
        public Rectangle32S(RECTANGLE rect)
        {
            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;
        }

        public static bool IsNullOrEmpty(RECTANGLEPROTO rect)
        {
            return ToRectangle(rect).IsEmpty;
        }
        public static RECTANGLE ToRectangle(RECTANGLEPROTO rect)
        {
            return rect == null ? RECTANGLE.Empty : rect.ToRectangle();
        }
        public RECTANGLE ToRectangle()
        {
            return new RECTANGLE(this.X, this.Y, this.Width, this.Height);
        }
    }
}
