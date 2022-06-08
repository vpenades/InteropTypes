using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Graphics.Drawing
{
    internal class RandomSpheres : IDrawingBrush<IScene3D>
    {
        public static RandomSpheres CreateRandom(Random rnd = null)
        {
            if (rnd == null) rnd = new Random();

            var l = new List<(BoundingSphere, OutlineFillStyle)>();

            for(int i=0; i < 10; ++i)
            {
                var s = new BoundingSphere(new Point3(10,10,10) * rnd, (float)rnd.NextDouble());
                var c = new ColorStyle(rnd, 255);
                l.Add((s, c));
            }

            return new RandomSpheres(l);
        }

        private RandomSpheres(IEnumerable<(BoundingSphere Sphere, OutlineFillStyle Style)> spheres)
        {
            _Spheres = spheres.ToArray();
        }

        private readonly (BoundingSphere Sphere, OutlineFillStyle Style)[] _Spheres;
        public void DrawTo(IScene3D context)
        {
            foreach(var (sphere, style) in _Spheres)
            {
                sphere.DrawTo(context, style);
            }
        }
    }
}
