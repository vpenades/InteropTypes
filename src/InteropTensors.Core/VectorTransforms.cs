using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTensors
{
    // https://en.wikipedia.org/wiki/Multiply%E2%80%93accumulate_operation

    /// <summary>
    /// Represents a Multiply and Addition operation over a <see cref="Vector3"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Mul {Multiply} Add {Addition}")]
    public readonly struct Mad3
    {
        #region constructor
        public static Mad3 CreateAdd(float x)
        {
            var add = new Vector3(x);
            return new Mad3(Vector3.One, add);
        }

        public static Mad3 CreateAdd(float x, float y, float z)
        {
            var add = new Vector3(x, y, z);
            return new Mad3(Vector3.One, add);
        }

        public static Mad3 CreateMul(float x)
        {
            var mul = new Vector3(x);
            return new Mad3(mul, Vector3.Zero);
        }

        public static Mad3 CreateMul(float x, float y, float z)
        {
            var mul = new Vector3(x, y, z);
            return new Mad3(mul, Vector3.Zero);
        }

        public static Mad3 CreateAdd(Vector3 add)
        {
            return new Mad3(Vector3.One, add);
        }

        public static Mad3 CreateMul(Vector3 mul)
        {
            return new Mad3(mul, Vector3.Zero);
        }        

        public Mad3(Vector3 mul, Vector3 add)
        {
            this.Multiply = mul;
            this.Addition = add;            
        }

        public Mad3(float mul, float add)
        {
            this.Multiply = new Vector3(mul);
            this.Addition = new Vector3(add);
        }

        public Mad3((float X, float Y, float Z) mul, (float X, float Y, float Z) add)
        {
            this.Multiply = new Vector3(mul.X, mul.Y, mul.Z);
            this.Addition = new Vector3(add.X, add.Y, add.Z);
        }

        #endregion

        #region data

        public readonly Vector3 Multiply;
        public readonly Vector3 Addition;

        public static readonly Mad3 Identity = new Mad3(Vector3.One, Vector3.Zero);

        #endregion

        #region API

        public bool IsIdentity => Multiply == Vector3.One && Addition == Vector3.Zero;

        public Mad3 GetTransposedZYX()
        {
            return new Mad3
                (
                (Multiply.Z, Multiply.Y, Multiply.X),
                (Addition.Z, Addition.Y, Addition.X)
                );
        }

        public Mad3 GetInverse()
        {
            var m = Vector3.One / Multiply;

            return new Mad3(m, -Addition * m);
        }
        public Mad3 ConcatMul(float x, float y, float z)
        {
            var mul = new Vector3(x, y, z);
            return new Mad3(Multiply * mul, Addition * mul);
        }

        public Mad3 ConcatAdd(float x, float y, float z)
        {
            var add = new Vector3(x, y, z);
            return new Mad3(Multiply, Addition + add);
        }

        public Mad3 ConcatMul(Vector3 mul)
        {
            return new Mad3(Multiply * mul, Addition * mul);
        }

        public Mad3 ConcatAdd(Vector3 add)
        {
            return new Mad3(Multiply, Addition + add);
        }

        public Vector3 Transform(Vector3 value)
        {
            value *= Multiply;
            value += Addition;
            return value;
        }

        public void ApplyTransformTo(Span<Vector3> dst)
        {
            if (this.IsIdentity) return;

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = Transform(dst[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Vector3> src, Span<Vector3> dst, Mad3 xform)
        {
            if (xform.IsIdentity)
            {
                src.Slice(0, dst.Length).CopyTo(dst);
            }
            else
            {
                var l = dst.Length;

                for (int i = 0; i < l; ++i)
                {
                    dst[i] = xform.Transform(src[i]);
                }
            }
        }

        public static void Transform(ReadOnlySpan<Vector3> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, Mad3 xform)
        {
            var l = dstX.Length;

            if (xform.IsIdentity)
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;
                }
            }
            else
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = xform.Transform(src[i]);
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;
                }
            }
        }

        public static void Transform(ReadOnlySpan<Single> srcX, ReadOnlySpan<Single> srcY, ReadOnlySpan<Single> srcZ, Span<Vector3> dst, Mad3 xform)
        {
            if (xform.IsIdentity)
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    dst[x] = new Vector3(srcX[x], srcY[x], srcZ[x]);
                }                
            }
            else
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    var val = new Vector3(srcX[x], srcY[x], srcZ[x]);
                    dst[x] = xform.Transform(val);
                }
            }
        }

        #endregion

        #region extras

        public static Mad3 FromMatrix(Matrix4x4 xform)
        {
            // todo: check if it has rotation            
            return new Mad3((xform.M11, xform.M22, xform.M33), (xform.M41, xform.M42, xform.M43));
        }

        public Matrix4x4 ToMatrix4x4()
        {
            var m = Matrix4x4.CreateScale(Multiply);
            m.Translation = Addition;
            return m;
        }

        #endregion
    }

    /// <summary>
    /// Represents a Multiply and Addition operation over a <see cref="Vector4"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Mul {Multiply} Add {Addition}")]
    public readonly struct Mad4
    {
        #region constructor

        public static Mad4 CreateAdd(float x)
        {
            var add = new Vector4(x);
            return new Mad4(Vector4.One, add);
        }

        public static Mad4 CreateAdd(float x, float y, float z, float w)
        {
            var add = new Vector4(x, y, z, w);
            return new Mad4(Vector4.One, add);
        }

        public static Mad4 CreateMul(float x)
        {
            var mul = new Vector4(x);
            return new Mad4(mul, Vector4.Zero);
        }

        public static Mad4 CreateMul(float x, float y, float z, float w)
        {
            var mul = new Vector4(x, y, z, w);
            return new Mad4(mul, Vector4.Zero);
        }

        public static Mad4 CreateAdd(Vector4 add)
        {
            return new Mad4(Vector4.One, add);
        }

        public static Mad4 CreateMul(Vector4 mul)
        {
            return new Mad4(mul, Vector4.Zero);
        }

        public Mad4(Mad3 other)
        {
            this.Multiply = new Vector4(other.Multiply,1);
            this.Addition = new Vector4(other.Addition,0);
        }

        public Mad4(Vector4 mul, Vector4 add)
        {
            this.Multiply = mul;
            this.Addition = add;
        }

        public Mad4(float mul, float add)
        {
            this.Multiply = new Vector4(mul);
            this.Addition = new Vector4(add);
        }

        public Mad4((float X, float Y, float Z, float W) mul, (float X, float Y, float Z, float W) add)
        {
            this.Multiply = new Vector4(mul.X, mul.Y, mul.Z, mul.W);
            this.Addition = new Vector4(add.X, add.Y, add.Z, add.W);
        }

        #endregion

        #region data

        public readonly Vector4 Multiply;
        public readonly Vector4 Addition;

        public static readonly Mad4 Identity = new Mad4(Vector4.One, Vector4.Zero);

        #endregion

        #region API

        public bool IsIdentity => Multiply == Vector4.One && Addition == Vector4.Zero;

        public Mad3 SelectXYZ()
        {
            return new Mad3
                (
                (Multiply.X, Multiply.Y, Multiply.Z),
                (Addition.X, Addition.Y, Addition.Z)
                );
        }

        public Mad4 GetTransposedWZYX()
        {
            return new Mad4
                (
                (Multiply.W, Multiply.Z, Multiply.Y, Multiply.X),
                (Addition.W, Addition.Z, Addition.Y, Addition.X)
                );
        }

        public Mad4 GetInverse()
        {
            var m = Vector4.One / Multiply;

            return new Mad4(m, -Addition * m);
        }

        public Mad4 ConcatMul(float x, float y, float z, float w)
        {
            var mul = new Vector4(x, y, z, w);
            return new Mad4(Multiply * mul, Addition * mul);
        }

        public Mad4 ConcatAdd(float x, float y, float z, float w)
        {
            var add = new Vector4(x, y, z, w);
            return new Mad4(Multiply, Addition + add);
        }

        public Mad4 ConcatMul(Vector4 mul)
        {
            return new Mad4(Multiply * mul, Addition * mul);
        }

        public Mad4 ConcatAdd(Vector4 add)
        {
            return new Mad4(Multiply, Addition + add);
        }

        public Vector4 Transform(Vector4 value)
        {
            value *= Multiply;
            value += Addition;
            return value;
        }

        public void ApplyTransformTo(Span<Vector4> dst)
        {
            if (this.IsIdentity) return;

            for(int i=0; i < dst.Length; ++i)
            {
                dst[i] = Transform(dst[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Vector4> dst, Mad4 xform)
        {
            if (xform.IsIdentity)
            {
                src.Slice(0, dst.Length).CopyTo(dst);                
            }
            else
            {
                var l = dst.Length;

                for (int i = 0; i < l; ++i)
                {
                    dst[i] = xform.Transform(src[i]);
                }
            }
        }

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, Span<Single> dstW, Mad4 xform)
        {
            var l = dstX.Length;

            if (xform.IsIdentity)
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;
                    dstW[i] = v.W;
                }
            }
            else
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = xform.Transform(src[i]);
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;
                    dstW[i] = v.W;
                }
            }
        }

        public static void Transform(ReadOnlySpan<Single> srcX, ReadOnlySpan<Single> srcY, ReadOnlySpan<Single> srcZ, ReadOnlySpan<Single> srcW, Span<Vector4> dst, Mad4 xform)
        {
            if (xform.IsIdentity)
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    dst[x] = new Vector4(srcX[x], srcY[x], srcZ[x], srcW[x]);
                }
            }
            else
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    var val = new Vector4(srcX[x], srcY[x], srcZ[x], srcW[x]);
                    dst[x] = xform.Transform(val);
                }
            }
        }

        #endregion
    }
}
