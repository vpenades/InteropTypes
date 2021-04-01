using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;
using COLOR = System.Drawing.Color;

using POINT2 = InteropDrawing.Point2;
using POINT3 = InteropDrawing.Point3;

namespace Plotly
{
    partial class TracesContext
    {
        #region data

        private readonly List<TRACES> _Traces = new List<TRACES>();

        private void _Add(params Box<Types.IScatterProperty>[] properties)
        {
            var trace = Traces.scatter(properties);
            _Traces.Add(trace);
        }

        private void _Add(params Box<Types.IScatter3dProperty>[] properties)
        {
            var trace = Traces.scatter3d(properties);
            _Traces.Add(trace);
        }

        private void _Add(params Box<Types.IMesh3dProperty>[] properties)
        {
            var trace = Traces.mesh3d(properties);
            _Traces.Add(trace);
        }

        #endregion

        public void Markers(IReadOnlyList<(POINT2 Position, Single Size, COLOR Color)> markers)
        {
            var x = new float[markers.Count];
            var y = new float[markers.Count];
            var s = new float[markers.Count];
            var c = new string[markers.Count];

            for (int i = 0; i < markers.Count; ++i)
            {
                x[i] = markers[i].Position.X;
                y[i] = markers[i].Position.Y;
                s[i] = markers[i].Size;
                c[i] = markers[i].Color.ToPlotlyStringRGB();
            }

            _Add(
                Scatter.x(x),
                Scatter.y(y),
                Scatter.Mode.markers(),
                Scatter.marker(
                    s.ArrayOrValue(Marker.size, Marker.size),
                    c.ArrayOrValue(Marker.color, Marker.color)
                    )
                );
        }

        public void LineSeries<T>(IEnumerable<T> series, LineProperties style)
            where T : IConvertible
        {
            var y = series
                .ToArray()
                .CreateTypedProperty(Scatter.y, Scatter.y, Scatter.y, Scatter.y, Scatter.y);

            _Add
                (
                y,
                Scatter.Mode.lines(), Scatter.line(style)
                );
        }

        public void LineBars<T>(IEnumerable<T> series)
            where T : IConvertible
        {
            var y = series
                .ToArray()
                .CreateTypedProperty(Bar.y, Bar.y, Bar.y, Bar.y, Bar.y);

            throw new NotImplementedException();           
        }

        public void LineSeries<Tx, Ty>(IEnumerable<Point2<Tx, Ty>> points, LineProperties style)
            where Tx : IConvertible
            where Ty : IConvertible
        {
            var (x, y) = Point2<Tx, Ty>
                .ToScatter(points.OrderBy(item => item.X)
                .ToList());

            _Add
                (
                x, y,
                Scatter.Mode.lines(), Scatter.line(style)
                );
        }

        public void Lines(ReadOnlySpan<POINT2> points, LineProperties style)
        {
            var (x, y) = points.Split();

            _Add
                (
                Scatter.x(x), Scatter.y(y),
                Scatter.Mode.lines(), Scatter.line(style)
                );
        }

        public void Lines<Tx, Ty>(IReadOnlyList<Point2<Tx, Ty>> points, LineProperties style)
            where Tx : IConvertible
            where Ty : IConvertible
        {
            var (x, y) = Point2<Tx, Ty>.ToScatter(points);

            _Add
                (
                x, y,
                Scatter.Mode.lines(), Scatter.line(style)
                );
        }

        public void Polygon(ReadOnlySpan<POINT2> points, COLOR fill)
        {
            var (x, y) = points.Split();

            _Add
                    (
                    Scatter.x(x), Scatter.y(y),
                    Scatter.Fill.toself(), Scatter.fillcolor(fill.ToPlotlyStringRGBA()),
                    Scatter.Mode.none()
                    );
        }

        public void Polygon(ReadOnlySpan<POINT2> points, COLOR fill, LineProperties lineStyle)
        {
            var (x, y) = points.Split();

            _Add
                    (
                    Scatter.x(x), Scatter.y(y),
                    Scatter.Fill.toself(), Scatter.fillcolor(fill.ToPlotlyStringRGBA()),
                    Scatter.Mode.lines(), Scatter.line(lineStyle)
                    );
        }

        public void Lines(ReadOnlySpan<POINT3> points, LineProperties lineStyle)
        {
            var (mx, my, mz) = points.Split();

            // https://plotly.com/javascript/3d-line-plots/

            _Add
                (
                Scatter3d.x(mx), Scatter3d.y(my), Scatter3d.z(mz),
                Scatter3d.Mode.lines(), Scatter3d.line(lineStyle)
                );
        }

        public void Mesh3D<TMaterial>(IEnumerable<(PlotlyVertex A, PlotlyVertex B, PlotlyVertex C, TMaterial Material)> triangles, Func<TMaterial, int> materialColorFunc, float opacity = 1)
        {
            var vrts = new List<PlotlyVertex>();              // vertex list
            var vrtm = new Dictionary<PlotlyVertex, int>();   // vertex sharing map

            var tris = new List<(int, int, int)>();                     // triangle indices
            var cols = new List<int>();                         // face colors

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

            var (vx, vy, vz) = PlotlyVertex.Split(vrts);
            var (ti, tj, tk) = PlotlyVertex.Split(tris);

            var mc = cols.ToArray();

            _Add
                (
                Mesh3d.x(vx),
                Mesh3d.y(vy),
                Mesh3d.z(vz),
                Mesh3d.i(ti),
                Mesh3d.j(tj),
                Mesh3d.k(tk),
                mc.ArrayOrValue(Mesh3d.facecolor, Mesh3d.facecolor),
                Mesh3d.opacity(opacity)
                );
        }
    }
}
