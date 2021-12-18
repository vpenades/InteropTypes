using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTensors
{
    [System.Diagnostics.DebuggerDisplay("{XYZW}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    readonly struct VectorXYZW
    {
        public static implicit operator VectorXYZW(Vector4 v) { return new VectorXYZW(v); }

        public VectorXYZW(Vector4 value)
        {
            this = default;
            XYZW = value;
        }

        public VectorXYZW(Vector3 value, float w)
        {
            this = default;
            XYZW = new Vector4(value, w);
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly Vector4 XYZW;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly Vector3 XYZ;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly Vector2 XY;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly Single X;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public readonly Single Y;

        [System.Runtime.InteropServices.FieldOffset(8)]
        public readonly Single Z;

        [System.Runtime.InteropServices.FieldOffset(12)]
        public readonly Single W;

        public static Vector4 operator *(VectorXYZW a, float b) { return a.XYZW * b; }
        public static Vector4 operator *(VectorXYZW a, Vector4 b) { return a.XYZW * b; }
        public static Vector4 operator +(VectorXYZW a, Vector4 b) { return a.XYZW + b; }
    }

    // https://en.wikipedia.org/wiki/Multiply%E2%80%93accumulate_operation

    

    /// <summary>
    /// Represents a Multiply and Addition operation over a <see cref="Vector4"/>, <see cref="Vector3"/>, <see cref="Vector2"/> and <see cref="Single"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Mul {Multiply} Add {Addition}")]
    public readonly struct MultiplyAdd
    {
        #region constructor        

        public static MultiplyAdd CreateAdd(float x)
        {
            var add = new Vector4(x);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateAdd(float x, float y, float z = float.NaN, float w = float.NaN)
        {
            var add = new Vector4(x, y, z, w);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateMul(float x)
        {
            var mul = new Vector4(x);
            return new MultiplyAdd(mul, Vector4.Zero);
        }

        public static MultiplyAdd CreateMul(float x, float y, float z = float.NaN, float w = float.NaN)
        {
            var mul = new Vector4(x, y, z, w);
            return new MultiplyAdd(mul, Vector4.Zero);
        }

        public static MultiplyAdd CreateAdd(Vector4 add)
        {
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateMul(Vector4 mul)
        {
            return new MultiplyAdd(mul, Vector4.Zero);
        }        

        public MultiplyAdd(Vector4 mul, Vector4 add)
        {
            this.Multiply = mul;
            this.Addition = add;
        }

        public MultiplyAdd(float mul, float add)
        {
            this.Multiply = new Vector4(mul);
            this.Addition = new Vector4(add);
        }

        public MultiplyAdd((float X, float Y, float Z, float W) mul, (float X, float Y, float Z, float W) add)
        {
            this.Multiply = new Vector4(mul.X, mul.Y, mul.Z, mul.W);
            this.Addition = new Vector4(add.X, add.Y, add.Z, add.W);
        }

        #endregion

        #region data

        private readonly VectorXYZW Multiply;
        private readonly VectorXYZW Addition;

        public static readonly MultiplyAdd Identity = new MultiplyAdd(Vector4.One, Vector4.Zero);

        #endregion

        #region API

        public bool IsIdentity => Multiply.XYZW == Vector4.One && Addition.XYZW == Vector4.Zero;        

        public MultiplyAdd GetTransposedWZYX()
        {
            return new MultiplyAdd
                (
                (Multiply.W, Multiply.Z, Multiply.Y, Multiply.X),
                (Addition.W, Addition.Z, Addition.Y, Addition.X)
                );
        }

        public MultiplyAdd GetTransposedZYXW()
        {
            return new MultiplyAdd
                (
                (Multiply.Z, Multiply.Y, Multiply.X, Multiply.W),
                (Addition.Z, Addition.Y, Addition.X, Addition.W)
                );
        }

        public MultiplyAdd GetInverse()
        {
            var m = Vector4.One / Multiply.XYZW;

            return new MultiplyAdd(m, -Addition.XYZW * m);
        }

        public MultiplyAdd ConcatMul(float x)
        {
            var mul = new Vector4(x);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatMul(float x, float y, float z, float w)
        {
            var mul = new Vector4(x, y, z, w);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatAdd(float x)
        {
            var add = new Vector4(x);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        public MultiplyAdd ConcatAdd(float x, float y, float z, float w)
        {
            var add = new Vector4(x, y, z, w);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }        

        public MultiplyAdd ConcatMul(Vector4 mul)
        {
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatAdd(Vector4 add)
        {
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }        

        #endregion

        #region vector4

        public Vector4 Transform(Vector4 value)
        {
            value *= Multiply.XYZW;
            value += Addition.XYZW;
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

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Vector4> dst, MultiplyAdd xform)
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

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, Span<Single> dstW, MultiplyAdd xform)
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

        public static void Transform(ReadOnlySpan<Single> srcX, ReadOnlySpan<Single> srcY, ReadOnlySpan<Single> srcZ, ReadOnlySpan<Single> srcW, Span<Vector4> dst, MultiplyAdd xform)
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

        #region vector3

        public Vector3 Transform(Vector3 value)
        {
            value *= Multiply.XYZ;
            value += Addition.XYZ;
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

        public static void Transform(ReadOnlySpan<Vector3> src, Span<Vector3> dst, MultiplyAdd xform)
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

        public static void Transform(ReadOnlySpan<Vector3> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, MultiplyAdd xform)
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

        public static void Transform(ReadOnlySpan<Single> srcX, ReadOnlySpan<Single> srcY, ReadOnlySpan<Single> srcZ, Span<Vector3> dst, MultiplyAdd xform)
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

        #region vector2

        public Vector2 Transform(Vector2 value)
        {
            value *= Multiply.XY;
            value += Addition.XY;
            return value;
        }

        public void ApplyTransformTo(Span<Vector2> dst)
        {
            if (this.IsIdentity) return;

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = Transform(dst[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Vector2> src, Span<Vector2> dst, MultiplyAdd xform)
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

        public static void Transform(ReadOnlySpan<Vector2> src, Span<Single> dstX, Span<Single> dstY, MultiplyAdd xform)
        {
            var l = dstX.Length;

            if (xform.IsIdentity)
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;                    
                }
            }
            else
            {
                for (int i = 0; i < l; ++i)
                {
                    var v = xform.Transform(src[i]);
                    dstX[i] = v.X;
                    dstY[i] = v.Y;                    
                }
            }
        }

        public static void Transform(ReadOnlySpan<Single> srcX, ReadOnlySpan<Single> srcY, Span<Vector2> dst, MultiplyAdd xform)
        {
            if (xform.IsIdentity)
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    dst[x] = new Vector2(srcX[x], srcY[x]);
                }
            }
            else
            {
                for (int x = 0; x < dst.Length; ++x)
                {
                    var val = new Vector2(srcX[x], srcY[x]);
                    dst[x] = xform.Transform(val);
                }
            }
        }

        #endregion

        #region vector1

        public Single Transform(Single value)
        {
            value *= Multiply.X;
            value += Addition.X;
            return value;
        }

        public void ApplyTransformTo(Span<Single> dst)
        {
            if (this.IsIdentity) return;

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = Transform(dst[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Single> src, Span<Single> dst, MultiplyAdd xform)
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

        #endregion
    }
}
