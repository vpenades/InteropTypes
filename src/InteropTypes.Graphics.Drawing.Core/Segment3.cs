﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a segment delimited by two <see cref="System.Numerics.Vector3"/>.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{A}⊶{B}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct Segment3 : IEquatable<Segment3>
    {
        #region constructor

        public static Segment3 Create(in Point3 a, in Point3 b)
        {
            return new Segment3(a, b);
        }

        /// <summary>
        /// Creates a <see cref="System.Numerics.Vector3"/> segment, ensuring that the endpoints are ordered, in ascending ordinal X,Y,Z component wise.
        /// </summary>
        /// <param name="a">the first segment end point.</param>
        /// <param name="b">the other segment end point.</param>
        /// <returns>A segment pair</returns>
        /// <remarks>
        /// CreateOrdinal(a,b) == CreateOrdinal(b,a);
        /// </remarks>
        public static Segment3 CreateOrdinal(in Point3 a, in Point3 b)
        {
            switch (a.X.CompareTo(b.X))
            {
                case -1: return new Segment3(a, b);
                case 1: return new Segment3(b, a);
            }

            switch (a.Y.CompareTo(b.Y))
            {
                case -1: return new Segment3(a, b);
                case 1: return new Segment3(b, a);
            }

            switch (a.Z.CompareTo(b.Z))
            {
                case -1: return new Segment3(a, b);
                case 1: return new Segment3(b, a);
            }

            return new Segment3(a, b);
        }

        private Segment3(in Point3 a, in Point3 b)
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
        public override bool Equals(object obj) { return obj is Segment3 other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(Segment3 other) { return this.A == other.A && this.B == other.B; }

        /// <inheritdoc/>
        public static bool operator ==(Segment3 left, Segment3 right) { return left.Equals(right); }

        /// <inheritdoc/>
        public static bool operator !=(Segment3 left, Segment3 right) { return !left.Equals(right); }

        #endregion

        #region properties

        public bool IsFinite => Point3.IsFinite(A) && Point3.IsFinite(B);

        public int DominantAxis => Point3.GetDominantAxis(Direction);

        public System.Numerics.Vector3 Direction => B - A;

        public System.Numerics.Vector3 DirectionNormalized => System.Numerics.Vector3.Normalize(B - A);

        public float Length => Direction.Length();

        public Segment3 Ordinal => CreateOrdinal(A, B);

        #endregion

        #region API

        public bool HasEnd(in Point3 point) { return point == A || point == B; }

        /// <summary>
        /// Checks whether the two segments are connected by any of the two ends.
        /// </summary>
        /// <param name="a">The first segment.</param>
        /// <param name="b">The second segment.</param>
        /// <param name="connectingPoint">the connecting point.</param>
        /// <returns>true if they're connected</returns>
        public static bool TryGetConnectingPoint(in Segment3 a, in Segment3 b, out Point3 connectingPoint)
        {
            if (a.HasEnd(b.A)) { connectingPoint = b.A; return true; }
            if (a.HasEnd(b.B)) { connectingPoint = b.B; return true; }

            connectingPoint = default;
            return false;
        }

        public static IEqualityComparer<Segment3> GetEqualityComparer(bool ordinal)
        {
            return ordinal ? _OrderedComparer.Instance : _UnorderedComparer.Instance;
        }

        #endregion

        #region nested types

        sealed class _OrderedComparer : IEqualityComparer<Segment3>
        {
            public static readonly IEqualityComparer<Segment3> Instance = new _OrderedComparer();

            public bool Equals(Segment3 x, Segment3 y) { return x.A == y.A && x.B == y.B; }
            public int GetHashCode(Segment3 obj) { return obj.A.GetHashCode() ^ (obj.B.GetHashCode() * 17); }
        }

        sealed class _UnorderedComparer : IEqualityComparer<Segment3>
        {
            public static readonly IEqualityComparer<Segment3> Instance = new _UnorderedComparer();

            public bool Equals(Segment3 x, Segment3 y) { return (x.A == y.A && x.B == y.B) || (x.A == y.B && x.B == y.A); }

            public int GetHashCode(Segment3 obj) { return obj.A.GetHashCode() ^ obj.B.GetHashCode(); }
        }

        #endregion
    }
}