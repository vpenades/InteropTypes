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
    public struct Vertex3 : IEquatable<Vertex3>
    {
        #region constructors            

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex3(Point3 p, ColorStyle c, Point2 t)
        {
            Position = p.XYZ;
            Color = c.Packed;
            TextureCoord = t.XY;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex3(Point3 p, ColorStyle c)
        {
            Position = p.XYZ;
            Color = c.Packed;
            TextureCoord = default;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex3(Point3 p, Point3 t)
        {
            Position = p.XYZ;
            Color = uint.MaxValue;
            TextureCoord = t.XY;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Vertex3(Point3 p)
        {
            Position = p.XYZ;
            Color = uint.MaxValue;
            TextureCoord = default;
        }

        #endregion

        #region data

        public System.Numerics.Vector3 Position;
        public UInt32 Color;
        public System.Numerics.Vector2 TextureCoord;

        /// <inheritdoc/>
        public override int GetHashCode() { return Position.GetHashCode(); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is Vertex3 other && AreEqual(this, other); }

        /// <inheritdoc/>
        public bool Equals(Vertex3 other) { return AreEqual(this, other); }

        /// <inheritdoc/>
        public static bool operator ==(in Vertex3 a, in Vertex3 b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in Vertex3 a, in Vertex3 b) { return !AreEqual(a, b); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(in Vertex3 a, in Vertex3 b)
        {
            return a.Position == b.Position && a.Color == b.Color && a.TextureCoord == b.TextureCoord;
        }

        #endregion

        #region properties

        public bool IsFinite => Point3.IsFinite(Position) && Point2.IsFinite(TextureCoord);

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Vertex3> vertices, in System.Numerics.Matrix4x4 transform)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = System.Numerics.Vector3.Transform(vertices[i].Position, transform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ReadOnlySpan<Vertex3> src, in System.Numerics.Matrix4x4 transform, Span<Vertex3> dst)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = src[i];
                dst[i].Position = System.Numerics.Vector3.Transform(dst[i].Position, transform);
            }
        }

        #endregion
    }
}
