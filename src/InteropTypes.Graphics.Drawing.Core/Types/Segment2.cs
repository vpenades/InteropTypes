using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a segment delimited by two <see cref="Vector2"/>.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{A}⊶{B}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct Segment2
        : IEquatable<Segment2>
        , IDrawingBrush<ICoreCanvas2D>
        , IDrawingBrush<ICanvas2D>
    {
        #region constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Segment2 Create(in Point2 a, in Point2 b)
        {
            return new Segment2(a, b);
        }

        /// <summary>
        /// Creates a <see cref="Vector2"/> segment, ensuring that the endpoints are ordered, in ascending ordinal X,Y,Z component wise.
        /// </summary>
        /// <param name="a">the first segment end point.</param>
        /// <param name="b">the other segment end point.</param>
        /// <returns>A segment pair</returns>
        /// <remarks>
        /// CreateOrdinal(a,b) == CreateOrdinal(b,a);
        /// </remarks>        
        public static Segment2 CreateOrdinal(in Point2 a, in Point2 b)
        {
            switch (a.X.CompareTo(b.X))
            {
                case -1: return new Segment2(a, b);
                case 1: return new Segment2(b, a);
            }

            switch (a.Y.CompareTo(b.Y))
            {
                case -1: return new Segment2(a, b);
                case 1: return new Segment2(b, a);
            }

            return new Segment2(a, b);
        }

        [System.Diagnostics.DebuggerStepThrough]
        private Segment2(in Point2 a, in Point2 b)
        {
            this.A = a.XY;
            this.B = b.XY;
        }

        [System.Diagnostics.DebuggerStepThrough]
        private Segment2(in Vector2 a, in Vector2 b)
        {
            this.A = a;
            this.B = b;
        }

        #endregion

        #region data

        public readonly Vector2 A;
        public readonly Vector2 B;

        /// <inheritdoc/>
        public readonly override int GetHashCode() { return A.GetHashCode() ^ B.GetHashCode(); }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is Segment2 other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(Segment2 other) { return this.A == other.A && this.B == other.B; }

        /// <inheritdoc/>
        public static bool operator ==(Segment2 left, Segment2 right) { return left.Equals(right); }

        /// <inheritdoc/>
        public static bool operator !=(Segment2 left, Segment2 right) { return !left.Equals(right); }

        #endregion

        #region properties

        public readonly bool IsFinite => Point2.IsFinite(A) && Point2.IsFinite(B);

        public readonly int DominantAxis => Point2.GetDominantAxis(Direction);

        public readonly Vector2 Direction => B - A;        

        public readonly Vector2 DirectionNormalized => Vector2.Normalize(B - A);

        public readonly float Length => Direction.Length();

        public readonly Segment2 Ordinal => CreateOrdinal(A, B);

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Segment2 Lerp(Segment2 a, Segment2 b, float amount)
        {
            return new Segment2(Vector2.Lerp(a.A, b.A, amount), Vector2.Lerp(a.B, b.B, amount));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Segment2 LerpOrdinal(Segment2 a, Segment2 b, float amount)
        {
            return CreateOrdinal(Vector2.Lerp(a.A, b.A, amount), Vector2.Lerp(a.B, b.B, amount));
        }

        /// <summary>
        /// Checks whether one of the ends of the segment is <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>true if <paramref name="point"/> is one of the end of the segment.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HasEnd(in Point2 point) { return point == A || point == B; }

        /// <summary>
        /// Checks whether the two segments are connected by any of the two ends.
        /// </summary>
        /// <param name="a">The first segment.</param>
        /// <param name="b">The second segment.</param>
        /// <param name="connectingPoint">the connecting point.</param>
        /// <returns>true if they're connected</returns>
        public static bool TryGetConnectingPoint(in Segment2 a, in Segment2 b, out Point2 connectingPoint)
        {
            if (a.HasEnd(b.A)) { connectingPoint = b.A; return true; }
            if (a.HasEnd(b.B)) { connectingPoint = b.B; return true; }

            connectingPoint = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotProduct(in Segment2 segment, Point2 point)
        {
            var ab = segment.B - segment.A;
            var ac = point - segment.A;
            return Vector2.Dot(ab, ac) / ab.LengthSquared();
        }

        public static float Distance(in Segment2 segment, Point2 point)
        {
            var ab = segment.B - segment.A;
            var ac = point - segment.A;
            var u = Vector2.Dot(ab, ac) / ab.LengthSquared();

            u = Math.Max(0, u);
            u = Math.Min(1, u);
            return Vector2.Distance(point.XY, segment.A + ab * u);
        }

        public static IEqualityComparer<Segment2> GetEqualityComparer(bool ordinal)
        {
            return ordinal ? _OrderedComparer.Instance : _UnorderedComparer.Instance;
        }

        #endregion

        #region convert to

        /// <inheritdoc/>
        readonly void IDrawingBrush<ICanvas2D>.DrawTo(ICanvas2D context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var style = ColorStyle.GetDefaultFrom(context, ColorStyle.Red);
            DrawTo(context, style);
        }

        /// <inheritdoc/>
        public readonly void DrawTo(ICoreCanvas2D context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var style = ColorStyle.GetDefaultFrom(context, ColorStyle.Red);
            DrawTo(context, style);
        }

        public readonly void DrawTo(ICoreCanvas2D context, ColorStyle color)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Span<Point2> points = stackalloc Point2[2];
            points[0] = A;
            points[1] = B;

            context.DrawConvexPolygon(points, color);
        }

        public readonly void DrawTo(ICanvas2D context, float diameter, OutlineFillStyle style)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Span<Point2> points = stackalloc Point2[2];
            points[0] = A;
            points[1] = B;

            context.DrawLines(points, diameter, style);
        }

        #endregion

        #region nested types

        sealed class _OrderedComparer : IEqualityComparer<Segment2>
        {
            public static readonly IEqualityComparer<Segment2> Instance = new _OrderedComparer();

            public bool Equals(Segment2 x, Segment2 y) { return x.A == y.A && x.B == y.B; }
            public int GetHashCode(Segment2 obj) { return obj.A.GetHashCode() ^ (obj.B.GetHashCode() * 17); }
        }

        sealed class _UnorderedComparer : IEqualityComparer<Segment2>
        {
            public static readonly IEqualityComparer<Segment2> Instance = new _UnorderedComparer();

            public bool Equals(Segment2 x, Segment2 y) { return (x.A == y.A && x.B == y.B) || (x.A == y.B && x.B == y.A); }

            public int GetHashCode(Segment2 obj) { return obj.A.GetHashCode() ^ obj.B.GetHashCode(); }
        }

        #endregion
    }
}
