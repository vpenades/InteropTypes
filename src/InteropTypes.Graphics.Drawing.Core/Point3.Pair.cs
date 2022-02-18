using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point3
    {
        /// <summary>
        /// Represents a segment delimited by two <see cref="Point3"/>.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public readonly struct Pair : IEquatable<Pair>
        {
            #region constructor

            public static Pair Create(in Point3 a, in Point3 b)
            {
                return new Pair(a, b);
            }

            /// <summary>
            /// Creates a Point3 segment, ensuring that the endpoints are ordered, in ascending ordinal X,Y,Z component wise.
            /// </summary>
            /// <param name="a">the first segment end point.</param>
            /// <param name="b">the other segment end point.</param>
            /// <returns>A segment pair</returns>
            /// <remarks>
            /// CreateOrdered(a,b) == CreateOrdered(b,a);
            /// </remarks>
            public static Pair CreateOrdered(in Point3 a, in Point3 b)
            {
                switch(a.X.CompareTo(b.X))
                {
                    case -1: return new Pair(a, b);
                    case 1: return new Pair(b, a);
                }

                switch (a.Y.CompareTo(b.Y))
                {
                    case -1: return new Pair(a, b);
                    case 1: return new Pair(b, a);
                }

                switch (a.Z.CompareTo(b.Z))
                {
                    case -1: return new Pair(a, b);
                    case 1: return new Pair(b, a);
                }

                return new Pair(a, b);
            }            

            private Pair(in Point3 a, in Point3 b)
            {
                this.A = a.XYZ;
                this.B = b.XYZ;
            }

            #endregion

            #region data

            public readonly System.Numerics.Vector3 A;
            public readonly System.Numerics.Vector3 B;

            /// <inheritdoc/>            
            public override int GetHashCode() { return A.GetHashCode() ^ B.GetHashCode(); }

            /// <inheritdoc/>
            public override bool Equals(object obj) { return obj is Pair other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Pair other) { return this.A == other.A && this.B == other.B; }

            /// <inheritdoc/>
            public static bool operator ==(Pair left, Pair right) { return left.Equals(right); }

            /// <inheritdoc/>
            public static bool operator !=(Pair left, Pair right) { return !left.Equals(right); }

            #endregion

            #region properties

            public System.Numerics.Vector3 Direction => B - A;

            public System.Numerics.Vector3 DirectionNormalized => System.Numerics.Vector3.Normalize(B - A);

            public float Length => Direction.Length();

            public Pair Ordered => CreateOrdered(A, B);

            #endregion

            #region API

            public bool HasEnd(in Point3 point) { return point == A || point == B; }

            public static bool AreConnected(in Pair a, in Pair b) { return a.HasEnd(b.A) || a.HasEnd(b.B); }

            public static IEqualityComparer<Pair> GetEqualityComparer(bool ordered)
            {
                return ordered ? _OrderedComparer.Instance : _UnorderedComparer.Instance;
            }            

            #endregion

            #region nested types

            sealed class _OrderedComparer : IEqualityComparer<Pair>
            {
                public static readonly IEqualityComparer<Pair> Instance = new _OrderedComparer();

                public bool Equals(Pair x, Pair y) { return x.A == y.A && x.B == y.B; }
                public int GetHashCode(Pair obj) { return obj.A.GetHashCode() ^ (obj.B.GetHashCode() * 17); }
            }

            sealed class _UnorderedComparer : IEqualityComparer<Pair>
            {
                public static readonly IEqualityComparer<Pair> Instance = new _UnorderedComparer();

                public bool Equals(Pair x, Pair y) { return (x.A == y.A && x.B == y.B) || (x.A == y.B && x.B == y.A); }

                public int GetHashCode(Pair obj) { return obj.A.GetHashCode() ^ obj.B.GetHashCode(); }
            }

            #endregion
        }

    }
}
