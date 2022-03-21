using System;
using System.Collections.Generic;
using System.Text;

using Plotly.Types;

using PROP_SCATTER3D = Plotly.Box<Plotly.Types.IScatter3dProperty>;

namespace Plotly
{
    public struct Point3<Tx, Ty, Tz>
        where Tx : IConvertible
        where Ty : IConvertible
        where Tz : IConvertible
    {
        public static implicit operator Point3<Tx, Ty, Tz>((Tx X, Ty Y, Tz Z) point)
        {
            return new Point3<Tx, Ty, Tz>(point.X, point.Y, point.Z);
        }

        public Point3(Tx x, Ty y, Tz z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Tx X;
        public Ty Y;
        public Tz Z;

        public static (Tx[] x, Ty[] y, Tz[] z) Split(IReadOnlyList<Point3<Tx, Ty, Tz>> points)
        {
            var xx = new Tx[points.Count];
            var yy = new Ty[points.Count];
            var zz = new Tz[points.Count];

            for (int i = 0; i < points.Count; ++i)
            {
                xx[i] = points[i].X;
                yy[i] = points[i].Y;
                zz[i] = points[i].Z;
            }

            return (xx, yy, zz);
        }

        public static (PROP_SCATTER3D x, PROP_SCATTER3D y, PROP_SCATTER3D z) ToScatter3D(IReadOnlyList<Point3<Tx, Ty, Tz>> points)
        {
            var (x, y, z) = Split(points);

            var xx = x.CreateTypedProperty(Scatter3d.x, Scatter3d.x, Scatter3d.x, Scatter3d.x, Scatter3d.x);
            var yy = y.CreateTypedProperty(Scatter3d.y, Scatter3d.y, Scatter3d.y, Scatter3d.y, Scatter3d.y);
            var zz = z.CreateTypedProperty(Scatter3d.z, Scatter3d.z, Scatter3d.z, Scatter3d.z, Scatter3d.z);

            return (xx, yy, zz);
        }        
    }
}
