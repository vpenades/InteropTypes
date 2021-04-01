using System;
using System.Collections.Generic;
using System.Text;

using Plotly.Types;

using PROP_SCATTER = Plotly.Box<Plotly.Types.IScatterProperty>;
using PROP_BAR = Plotly.Box<Plotly.Types.IBarProperty>;

namespace Plotly
{
    public struct Point2<Tx,Ty>
        where Tx:IConvertible
        where Ty:IConvertible
    {
        public static implicit operator Point2<Tx,Ty>((Tx X, Ty Y) point)
        {
            return new Point2<Tx, Ty>(point.X, point.Y);
        }

        public Point2(Tx x, Ty y)
        {
            X = x;
            Y = y;
        }

        public Tx X;
        public Ty Y;

        public static (Tx[] x,Ty[] y) Split(IReadOnlyList<Point2<Tx, Ty>> points)
        {
            var xx = new Tx[points.Count];
            var yy = new Ty[points.Count];

            for (int i = 0; i < points.Count; ++i)
            {
                xx[i] = points[i].X;
                yy[i] = points[i].Y;
            }

            return (xx, yy);
        }

        public static (PROP_SCATTER x, PROP_SCATTER y) ToScatter(IReadOnlyList<Point2<Tx, Ty>> points)
        {
            var (x, y) = Split(points);

            var xx = x.CreateTypedProperty(Scatter.x, Scatter.x, Scatter.x, Scatter.x, Scatter.x);
            var yy = y.CreateTypedProperty(Scatter.y, Scatter.y, Scatter.y, Scatter.y, Scatter.y);

            return (xx, yy);
        }

        public static (PROP_BAR x, PROP_BAR y) ToBar(IReadOnlyList<Point2<Tx, Ty>> points)
        {
            var (x, y) = Split(points);

            var xx = x.CreateTypedProperty(Bar.x, Bar.x, Bar.x, Bar.x, Bar.x);
            var yy = y.CreateTypedProperty(Bar.y, Bar.y, Bar.y, Bar.y, Bar.y);

            return (xx, yy);
        }
    }

    
}
