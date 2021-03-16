using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace InteropDrawing
{
    public static class RightHanded
    {
        public static Matrix4x4 Concatenate(Matrix4x4 a, Matrix4x4 b)
        {
            return Matrix4x4.Multiply(a, b);
        }


        public static Matrix4x4 CreateMatrixFromRows(float[] data, int index)
        {
            return new Matrix4x4
                (
                data[index + 0], data[index + 1], data[index + 2], data[index + 3],
                data[index + 4], data[index + 5], data[index + 6], data[index + 7],
                data[index + 8], data[index + 9], data[index + 10], data[index + 11],
                data[index + 12], data[index + 13], data[index + 14], data[index + 15]
                );
        }


        public static Matrix4x4 CreateMatrixFromRows(Vector3 rowx, Vector3 rowy, Vector3 rowz, Vector3 roww)
        {
            return new Matrix4x4
                (
                rowx.X, rowx.Y, rowx.Z, 0,
                rowy.X, rowy.Y, rowy.Z, 0,
                rowz.X, rowz.Y, rowz.Z, 0,
                roww.X, roww.Y, roww.Z, 1
                );
        }

        public static Matrix4x4 CreateMatrixFromColumns(Vector4 colx, Vector4 coly, Vector4 colz)
        {
            return new Matrix4x4
                (
                colx.X, coly.X, colz.X, 0,
                colx.Y, coly.Y, colz.Y, 0,
                colx.Z, coly.Z, colz.Z, 0,
                colx.W, coly.W, colz.W, 1
                );
        }

        public static Matrix4x4 CreateMatrixFromRows(Vector4 rowx, Vector4 rowy, Vector4 rowz, Vector4 roww)
        {
            return new Matrix4x4
                (
                rowx.X, rowx.Y, rowx.Z, rowx.W,
                rowy.X, rowy.Y, rowy.Z, rowy.W,
                rowz.X, rowz.Y, rowz.Z, rowz.W,
                roww.X, roww.Y, roww.Z, roww.W
                );
        }

        public static Matrix4x4 CreateMatrixFromColumns(Vector4 colx, Vector4 coly, Vector4 colz, Vector4 colw)
        {
            return new Matrix4x4
                (
                colx.X, coly.X, colz.X, colw.X,
                colx.Y, coly.Y, colz.Y, colw.Y,
                colx.Z, coly.Z, colz.Z, colw.Z,
                colx.W, coly.W, colz.W, colw.W
                );
        }




        /// <summary>
        /// calculates a matrix transform which looks at a target
        /// </summary>
        /// <param name="origin">translation</param>
        /// <param name="target">position to look at</param>
        /// <param name="upVector">up vector of the reference frame</param>
        /// <returns>a transform matrix</returns>
        /// <remarks>
        /// System.Numerics.Vectors.Matrix4x4.CreateLookAt does essentially the same, but it assumes that everybody is going
        /// to use it as a view matrix, so they return the inverse matrix!!!
        /// https://github.com/dotnet/corefx/blob/master/src/System.Numerics.Vectors/src/System/Numerics/Matrix4x4.cs#L1051
        /// </remarks>
        public static Matrix4x4 CreateLookAtMatrix(Vector3 origin, Vector3 target, Vector3 upVector)
        {
            var zaxis = Vector3.Normalize(target - origin);
            var xaxis = Vector3.Normalize(Vector3.Cross(upVector, zaxis));
            var yaxis = Vector3.Cross(zaxis, xaxis);

            return new Matrix4x4
                (
                xaxis.X, xaxis.Y, xaxis.Z, 0,
                yaxis.X, yaxis.Y, yaxis.Z, 0,
                zaxis.X, zaxis.Y, zaxis.Z, 0,
                origin.X, origin.Y, origin.Z, 1
                );
        }

        public static Matrix4x4 CreateMatrixFromCrossPreserveFirst(Vector3 a, Vector3 b)
        {
            System.Diagnostics.Debug.Assert(a.IsReal() && a.ManhattanLength() > 0);
            System.Diagnostics.Debug.Assert(b.IsReal() && b.ManhattanLength() > 0);

            var c = Vector3.Cross(a, b).Normalized();
            a = a.Normalized();
            b = Vector3.Cross(c, a);

            return RightHanded.CreateMatrixFromRows(a, b, c, Vector3.Zero);
        }

        public static Matrix4x4 CreateMatrixFromCrossPreserveSecond(Vector3 a, Vector3 b)
        {
            System.Diagnostics.Debug.Assert(a.IsReal() && a.ManhattanLength() > 0);
            System.Diagnostics.Debug.Assert(b.IsReal() && b.ManhattanLength() > 0);

            var c = Vector3.Cross(a, b).Normalized();
            b = b.Normalized();
            a = Vector3.Cross(b, c);

            return RightHanded.CreateMatrixFromRows(a, b, c, Vector3.Zero);
        }




    }
}
