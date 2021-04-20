using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

// these types are defined in NeStandard2 by default, and available through System.Drawing.Primitives.
// why these types are also available in System.Drawing.Common is a mistery...

using POINTI = System.Drawing.Point;
using POINTF = System.Drawing.PointF;

using SIZEI = System.Drawing.Size;
using SIZEF = System.Drawing.SizeF;

using RECTI = System.Drawing.Rectangle;
using RECTF = System.Drawing.RectangleF;

using XY = System.Numerics.Vector2;

namespace InteropVision
{
    [Obsolete("Do not use",true)]
    public static class DrawingExtensions
    {
        public static XY ToVector2(this POINTI point) { return new XY(point.X, point.Y); }
        public static XY ToVector2(this POINTF point) { return new XY(point.X, point.Y); }
        public static XY ToVector2(this SIZEI point) { return new XY(point.Width, point.Height); }
        public static XY ToVector2(this SIZEF point) { return new XY(point.Width, point.Height); }

        public static POINTI ToPointT(this XY point) { return POINTI.Truncate(point.ToPoint()); }
        public static POINTI ToPointR(this XY point) { return POINTI.Round(point.ToPoint()); }
        public static POINTI ToPointC(this XY point) { return POINTI.Ceiling(point.ToPoint()); }        
        public static POINTI ToPoint(this (int X, int Y) point) { return new POINTI(point.X, point.Y); }
        public static POINTF ToPoint(this XY point) { return new POINTF(point.X, point.Y); }
        public static POINTF ToPoint(this (float X, float Y) point) { return new POINTF(point.X, point.Y); }
        public static POINTF Lerp(this (POINTF a,POINTF b) points, float amount)
        {
            return XY.Lerp(points.a.ToVector2(), points.b.ToVector2(), amount).ToPoint();
        }
        

        public static SIZEI ToSizeT(this XY size) { return SIZEI.Truncate(size.ToSize()); }
        public static SIZEI ToSizeR(this XY size) { return SIZEI.Round(size.ToSize()); }
        public static SIZEI ToSizeC(this XY size) { return SIZEI.Ceiling(size.ToSize()); }
        public static SIZEI ToSize(this (int X, int Y) size) { return new SIZEI(size.X, size.Y); }        
        public static SIZEF ToSize(this XY size) { return new SIZEF(size.X, size.Y); }
        public static SIZEF ToSize(this (float X, float Y) size) { return new SIZEF(size.X, size.Y); }
        public static SIZEF Lerp(this (SIZEF a, SIZEF b) points, float amount)
        {
            return XY.Lerp(points.a.ToVector2(), points.b.ToVector2(), amount).ToSize();
        }


        public static POINTF Center(this RECTF rect) { return rect.CenterVector2().ToPoint(); }
        public static XY CenterVector2(this RECTF rect)
        {
            var o = rect.Location.ToVector2();
            var s = rect.Size.ToVector2();
            return (o - s * 0.5f);
        }        

        public static RECTF EllipseToRect(this (XY center, int width, int height) ellipse)
        {
            ellipse.center -= new XY(ellipse.width, ellipse.height) * 0.5f;
            return new RECTF(ellipse.center.X, ellipse.center.Y, ellipse.width, ellipse.height);
        }

        public static RECTF CircleToRect(this (XY center, int radius) circle)
        {
            circle.center -= new XY(circle.radius) * 0.5f;
            return new RECTF(circle.center.X, circle.center.Y, circle.radius, circle.radius);
        }



        public static RECTI Lerp(this (RECTI a, RECTI b) rects, Single amount)
        {
            var r = Lerp(((RECTF)rects.a, (RECTF)rects.b), amount);
            return RECTI.Round(r);            
        }

        public static RECTF Lerp(this (RECTF a, RECTF b) rects, Single amount)
        {
            var p = (rects.a.Location, rects.b.Location).Lerp(amount);
            var s = (rects.a.Size, rects.b.Size).Lerp(amount);
            return new RECTF(p,s);
        }

        public static RECTF OuterSquare(this RECTF rect)
        {
            rect.Inflate(Math.Max(0, rect.Height - rect.Width)* 0.5f, Math.Max(0, rect.Width - rect.Height)*0.5f);
            System.Diagnostics.Debug.Assert(rect.Width == rect.Height);
            return rect;
        }

        public static RECTF InnerSquare(this RECTF rect)
        {
            rect.Inflate(Math.Min(0, rect.Width - rect.Height), Math.Min(0, rect.Height - rect.Width));
            System.Diagnostics.Debug.Assert(rect.Width == rect.Height);
            return rect;
        }

        public static RECTF Inflated(this RECTF rect, (Single x, Single y) amount)
        {
            rect.Inflate(amount.x, amount.y);
            return rect;
        }

        public static RECTF ScaledOffCenter(this RECTF rect, Single scaleX, Single scaleY)
        {
            var w = rect.Width * scaleX;
            var h = rect.Height * scaleY;

            scaleX = rect.X + (rect.Width - w) * 0.5f;
            scaleY = rect.Y + (rect.Height - h) * 0.5f;

            return new RECTF(scaleX, scaleY, w, h);
        }

        public static RECTF ScaledToResolution(this RECTF rect, XY currentResolution, XY newResolution)
        {
            return rect.Scaled(newResolution / currentResolution);
        }

        public static RECTF Scaled(this RECTF rect, XY scale)
        {
            rect.X *= scale.X;
            rect.Y *= scale.Y;
            rect.Width *= scale.X;
            rect.Height *= scale.Y;

            return rect;
        }


        public static (POINTF min, POINTF max) MinMax(this IEnumerable<POINTF> points)
        {
            var min = new XY(float.PositiveInfinity);
            var max = new XY(float.NegativeInfinity);

            foreach (var p in points)
            {
                var v = p.ToVector2();

                min = XY.Min(min, v);
                max = XY.Max(max, v);
            }

            return (min.ToPoint(), max.ToPoint());
        }

        public static RECTF BoundingRect(this IEnumerable<POINTF> points)
        {
            var min = new XY(float.PositiveInfinity);
            var max = new XY(float.NegativeInfinity);

            foreach (var p in points)
            {
                var v = p.ToVector2();

                min = XY.Min(min, v);
                max = XY.Max(max, v);
            }

            return new RECTF(min.ToPoint(), (max - min).ToSize());
        }        

        public static IEnumerable<IGrouping<T, T>> GroupByOverlapIslands<T>(this IEnumerable<T> collection, Func<T, RECTI> rectFunc)
        {
            T _findFirstOverlap(T item)
            {
                var itemRect = rectFunc(item);

                foreach (var other in collection)
                {
                    if (itemRect.IntersectsWith(rectFunc(other))) return other;
                }

                return item;
            }

            return collection.GroupBy(item => _findFirstOverlap(item));
        }        
    }
}
