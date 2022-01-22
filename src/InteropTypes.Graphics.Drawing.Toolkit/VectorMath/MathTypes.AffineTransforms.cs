using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using QROT = System.Numerics.Quaternion;

namespace InteropDrawing
{
    /// <summary>
    /// Represents an affine transform in 2D space, defined as a Scale, Rotation and Translation.
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Affine_transformation">Wikipedia Affine transformation</see>
    /// </remarks>
    public readonly struct TransformSRT2
    {
        public TransformSRT2(System.Numerics.Matrix3x2 matrix)
        {
            var sx = matrix.M12 == 0 ? Math.Abs(matrix.M11) : new XY(matrix.M11, matrix.M12).Length();
            var sy = matrix.M21 == 0 ? Math.Abs(matrix.M22) : new XY(matrix.M21, matrix.M22).Length();
            if (matrix.GetDeterminant() < 0) sy = -sy;

            Scale = new XY(sx, sy);
            Rotation = (float)Math.Atan2(matrix.M12, matrix.M11);
            Translation = matrix.Translation;
        }

        public TransformSRT2(XY s, float r, XY t)
        {
            Scale = s;
            Rotation = r;
            Translation = t;
        }

        public TransformSRT2(float r, XY t)
        {
            Scale = XY.One;
            Rotation = r;
            Translation = t;
        }

        public readonly XY Scale;
        public readonly float Rotation;
        public readonly XY Translation;
        // XY Shear ?

        public TransformSRT2 GetInverse()
        {
            return new TransformSRT2(XY.One / Scale, -Rotation, -Translation);
        }

        public System.Numerics.Matrix3x2 ToMatrix()
        {
            var m = System.Numerics.Matrix3x2.CreateRotation(Rotation);
            m.M11 *= Scale.X;
            m.M12 *= Scale.X;
            m.M21 *= Scale.Y;
            m.M22 *= Scale.Y;
            m.M31 = Translation.X;
            m.M32 = Translation.Y;
            return m;
        }
    }

    /// <summary>
    /// Represents an affine transform in 3D space, defined as a Scale, Rotation and Translation.
    /// </summary>
    public readonly struct TransformSRT3
    {
        public TransformSRT3(in System.Numerics.Matrix4x4 matrix)
        {
            if (!System.Numerics.Matrix4x4.Decompose(matrix, out Scale, out Rotation, out Translation))
            {
                throw new ArgumentException(nameof(matrix));
            }            
        }

        public TransformSRT3(XYZ s, QROT r, XYZ t)
        {
            Scale = s;
            Rotation = r;
            Translation = t;
        }

        public TransformSRT3(QROT r, XYZ t)
        {
            Scale = XYZ.One;
            Rotation = r;
            Translation = t;
        }

        public TransformSRT3(QROT r)
        {
            Scale = XYZ.One;
            Rotation = r;
            Translation = XYZ.Zero;
        }

        public readonly XYZ Scale;
        public readonly QROT Rotation;
        public readonly XYZ Translation;
        // XYZ Shear ?

        public TransformSRT3 GetInverse()
        {
            return new TransformSRT3(XYZ.One / Scale, -Rotation, -Translation);
        }

        public System.Numerics.Matrix4x4 ToMatrix()
        {            
            var m = Rotation == QROT.Identity 
                ? System.Numerics.Matrix4x4.Identity
                : System.Numerics.Matrix4x4.CreateFromQuaternion(Rotation);

            if (Scale != XYZ.One) m = System.Numerics.Matrix4x4.CreateScale(Scale) * m;

            m.Translation = this.Translation;

            return m;
        }
    }
}
