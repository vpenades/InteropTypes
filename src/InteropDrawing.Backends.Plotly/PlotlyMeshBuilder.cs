using System;
using System.Collections.Generic;
using System.Linq;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropDrawing.Backends
{
    class _PlotlyMeshBuilder : ISurfaceDrawing3D
    {
        private readonly List<(VECTOR3 A, VECTOR3 B, VECTOR3 C, System.Drawing.Color Color)> _Triangles = new List<(VECTOR3 A, VECTOR3 B, VECTOR3 C, System.Drawing.Color Color)>();

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            var a = vertices[0].ToNumerics();

            for (int i = 2; i < vertices.Length; ++i)
            {
                var b = vertices[i - 1].ToNumerics();
                var c = vertices[i + 0].ToNumerics();

                var tri = (a, b, c, style.Style.FillColor);

                _Triangles.Add(tri);
            }
        }

        public TRACES ToTrace()
        {
            if (_Triangles.Count == 0) return null;

            var xtris = _Triangles.Select(tri => (new PlotlyVertex(tri.A), new PlotlyVertex(tri.B), new PlotlyVertex(tri.C), tri.Color));

            return PlotlyVertex.CreateTrace(xtris, GetPlotlyColor);
        }

        private static int GetPlotlyColor(System.Drawing.Color color)
        {
            var ccc = color.R * 65536 + color.G * 256 + color.B;

            return (int)ccc;
        }
    }
}
