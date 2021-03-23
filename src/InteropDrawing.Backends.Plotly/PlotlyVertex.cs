using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Plotly;
using Plotly.Types;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;

using VECTOR3 = System.Numerics.Vector3;

namespace InteropDrawing.Backends
{
    readonly struct PlotlyVertex : IEquatable<PlotlyVertex>
    {
        public static TRACES CreateTrace<TMaterial>(IEnumerable<(PlotlyVertex A, PlotlyVertex B, PlotlyVertex C, TMaterial Material)> triangles, Func<TMaterial, int> materialColorFunc)
        {
            var vrts = new List<PlotlyVertex>();              // vertex list
            var vrtm = new Dictionary<PlotlyVertex, int>();   // vertex sharing map

            var tris = new List<(int, int, int)>();                     // triangle indices
            var cols = new List<int>();                                 // face colors

            int _useSharedVertex(PlotlyVertex v)
            {
                if (vrtm.TryGetValue(v, out int idx)) return idx;
                idx = vrts.Count;
                vrts.Add(v);
                vrtm.Add(v, idx);
                return idx;
            }

            foreach (var (a, b, c, Material) in triangles)
            {
                var color = materialColorFunc(Material);

                var aidx = _useSharedVertex(a);
                var bidx = _useSharedVertex(b);
                var cidx = _useSharedVertex(c);

                tris.Add((aidx, bidx, cidx));
                cols.Add(color);
            }

            // create a Plotly Mesh3D from the previously filled lists.

            // Positions
            var mx = Mesh3d.x(vrts.Select(item => item.Position.X));
            var my = Mesh3d.y(vrts.Select(item => item.Position.Y));
            var mz = Mesh3d.z(vrts.Select(item => item.Position.Z));

            // triangle indices
            var mi = Mesh3d.i(tris.Select(item => item.Item1).ToArray());
            var mj = Mesh3d.j(tris.Select(item => item.Item2).ToArray());
            var mk = Mesh3d.k(tris.Select(item => item.Item3).ToArray());

            var mo = Mesh3d.opacity(1);
            var mc = Mesh3d.facecolor(cols.ToArray());

            return Traces.mesh3d(mx, my, mz, mi, mj, mk, mo, mc);
        }

        public static (PlotlyVertex A, PlotlyVertex B, PlotlyVertex C) GetTriangle(Point3 a, Point3 b, Point3 c)
        {
            var ab = (b - a).ToNumerics();
            var ac = (c - a).ToNumerics();
            var n = VECTOR3.Normalize(VECTOR3.Cross(ab, ac));

            var aa = new PlotlyVertex(a, n);
            var bb = new PlotlyVertex(b, n);
            var cc = new PlotlyVertex(c, n);

            return (aa, bb, cc);
        }

        public PlotlyVertex(Point3 p)
        {
            Position = p.ToNumerics();
            Normal = Position == VECTOR3.Zero ? VECTOR3.Zero : VECTOR3.Normalize(Position);
        }

        public PlotlyVertex(Point3 p, VECTOR3 n)
        {
            Position = p.ToNumerics();
            Normal = n;
        }

        public PlotlyVertex(VECTOR3 p, VECTOR3 n)
        {
            Position = p;
            Normal = n;
        }

        public readonly VECTOR3 Position;
        public readonly VECTOR3 Normal;

        public bool Equals(PlotlyVertex other)
        {
            return this.Position == other.Position && this.Normal == other.Normal;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
