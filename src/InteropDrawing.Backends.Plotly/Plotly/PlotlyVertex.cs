using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using POINT3 = InteropDrawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace Plotly
{
    [System.Diagnostics.DebuggerDisplay("{Position} {Normal}")]
    readonly struct PlotlyVertex : IEquatable<PlotlyVertex>
    {
        #region factory

        public static (float[] x, float[] y, float[] z) Split(IReadOnlyList<PlotlyVertex> vertices)
        {
            var mx = new float[vertices.Count];
            var my = new float[vertices.Count];
            var mz = new float[vertices.Count];

            for (int i = 0; i < vertices.Count; ++i)
            {
                mx[i] = vertices[i].Position.X;
                my[i] = vertices[i].Position.Y;
                mz[i] = vertices[i].Position.Z;
            }

            return (mx, my, mz);
        }

        public static (int[] i, int[] j, int[] k) Split(IReadOnlyList<(int A, int B, int C)> triangles)
        {
            var ti = new int[triangles.Count];
            var tj = new int[triangles.Count];
            var tk = new int[triangles.Count];

            for (int i = 0; i < triangles.Count; ++i)
            {
                ti[i] = triangles[i].A;
                tj[i] = triangles[i].B;
                tk[i] = triangles[i].C;
            }

            return (ti, tj, tk);
        }


        public static (PlotlyVertex A, PlotlyVertex B, PlotlyVertex C) GetTriangle(POINT3 a, POINT3 b, POINT3 c)
        {
            var ab = (b - a).ToNumerics();
            var ac = (c - a).ToNumerics();
            var n = VECTOR3.Normalize(VECTOR3.Cross(ab, ac));

            var aa = new PlotlyVertex(a, n);
            var bb = new PlotlyVertex(b, n);
            var cc = new PlotlyVertex(c, n);

            return (aa, bb, cc);
        }

        #endregion

        #region constructors

        public PlotlyVertex(POINT3 p)
        {
            Position = p.ToNumerics();
            Normal = Position == VECTOR3.Zero ? VECTOR3.Zero : VECTOR3.Normalize(Position);
        }

        public PlotlyVertex(POINT3 p, VECTOR3 n)
        {
            Position = p.ToNumerics();
            Normal = n;
        }

        public PlotlyVertex(VECTOR3 p, VECTOR3 n)
        {
            Position = p;
            Normal = n;
        }

        #endregion

        #region data

        /// <summary>
        /// Position of this vertex.
        /// </summary>
        public readonly VECTOR3 Position;

        /// <summary>
        /// Normal of this vertex.
        /// </summary>
        /// <remarks>
        /// Plotly doesn't actually use normals, but applies smoothing on shared vertices.<br/>
        /// So we use the normal to force a vertex split.
        /// </remarks>
        public readonly VECTOR3 Normal;

        public bool Equals(PlotlyVertex other)
        {
            return this.Position == other.Position && this.Normal == other.Normal;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        #endregion
    }
}
