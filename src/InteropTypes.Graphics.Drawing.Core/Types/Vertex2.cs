using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Vertex with a Position, Color and Texture Coordinate.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vertex2
        : IEquatable<Vertex2>
    {
        #region constructors            

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex2(Point2 p, ColorStyle c, Point2 t)
        {
            Position = p.XY;
            Color = c.Packed;
            TextureCoord = t.XY;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex2(Point2 p, ColorStyle c)
        {
            Position = p.XY;
            Color = c.Packed;
            TextureCoord = default;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex2(Point2 p, Point2 t)
        {
            Position = p.XY;
            Color = uint.MaxValue;
            TextureCoord = t.XY;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex2(Point2 p)
        {
            Position = p.XY;
            Color = uint.MaxValue;
            TextureCoord = default;
        }

        #endregion

        #region data

        public System.Numerics.Vector2 Position;
        public UInt32 Color;
        public System.Numerics.Vector2 TextureCoord;

        /// <inheritdoc/>
        public override int GetHashCode() { return Position.GetHashCode(); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is Vertex2 other && AreEqual(this, other); }

        /// <inheritdoc/>
        public bool Equals(Vertex2 other) { return AreEqual(this, other); }

        /// <inheritdoc/>
        public static bool operator ==(in Vertex2 a, in Vertex2 b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in Vertex2 a, in Vertex2 b) { return !AreEqual(a, b); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(in Vertex2 a, in Vertex2 b)
        {
            return a.Position == b.Position && a.Color == b.Color && a.TextureCoord == b.TextureCoord;
        }

        #endregion

        #region properties

        public bool IsFinite => Point2.IsFinite(Position) && Point2.IsFinite(TextureCoord);

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Vertex2> vertices, in System.Numerics.Matrix3x2 transform)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = System.Numerics.Vector2.Transform(vertices[i].Position, transform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ReadOnlySpan<Vertex2> src, in System.Numerics.Matrix3x2 transform, Span<Vertex2> dst)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = src[i];
                dst[i].Position = System.Numerics.Vector2.Transform(dst[i].Position, transform);
            }
        }

        #endregion
    }
}
