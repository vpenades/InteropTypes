using System;
using System.Collections.Generic;
using System.Text;

using POINT = System.Drawing.Point;
using POINTPROTO = System.Drawing.ProtoPoint;

using POINTF = System.Drawing.PointF;
using POINTFPROTO = System.Drawing.ProtoPointF;

using SIZE = System.Drawing.Size;
using SIZEPROTO = System.Drawing.ProtoSize;

using SIZEF = System.Drawing.SizeF;
using SIZEFPROTO = System.Drawing.ProtoSizeF;

using RECTANGLE = System.Drawing.Rectangle;
using RECTANGLEPROTO = System.Drawing.ProtoRectangle;

using RECTANGLEF = System.Drawing.RectangleF;
using RECTANGLEFPROTO = System.Drawing.ProtoRectangleF;


namespace System.Drawing
{
    partial class ProtoPoint
    {
        public static readonly POINTPROTO Empty = POINT.Empty;

        public static implicit operator POINTPROTO(POINT point)
        {
            return new POINTPROTO(point);
        }

        public ProtoPoint(POINT point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }
        public POINT ToPoint()
        {
            return new POINT(this.X, this.Y);
        }
    }

    partial class ProtoPointF
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

        public ProtoPointF(POINT point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public ProtoPointF(POINTF point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public POINTF ToPointF()
        {
            return new POINTF(this.X, this.Y);
        }
    }

    partial class ProtoRectangle
    {
        public static implicit operator RECTANGLEPROTO(RECTANGLE rect)
        {
            return new RECTANGLEPROTO(rect);
        }
        public ProtoRectangle(RECTANGLE rect)
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
