using System;
using System.Collections.Generic;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Backends
{
    class _PlotlyMeshBuilder : ICoreScene3D
    {
        private readonly List<(VECTOR3 A, VECTOR3 B, VECTOR3 C, System.Drawing.Color Color)> _Triangles = new List<(VECTOR3 A, VECTOR3 B, VECTOR3 C, System.Drawing.Color Color)>();

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            var a = vertices[0].ToNumerics();

            for (int i = 2; i < vertices.Length; ++i)
            {
                var b = vertices[i - 1].ToNumerics();
                var c = vertices[i + 0].ToNumerics();

                var tri = (a, b, c, style.ToGDI());

                _Triangles.Add(tri);
            }
        }

        public IEnumerable<TRACES> ToTraces()
        {
            if (_Triangles.Count == 0) yield break;

            var xgroups = _Triangles
                .Select(tri => (new Plotly.PlotlyVertex(tri.A), new Plotly.PlotlyVertex(tri.B), new Plotly.PlotlyVertex(tri.C), tri.Color))
                .GroupBy(item => item.Color.A);

            foreach(var xtris in xgroups)
            {
                var opacity = (float)xtris.Key / 255f;

                yield return Plotly.TracesFactory.Mesh3D(xtris, Plotly._Extensions.ToPlotlyIntegerRGB, opacity);
            }            
        }        
    }
}
