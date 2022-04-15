using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Tensors
{
    /// <summary>
    /// Represents a Multiply and Addition operation over a <see cref="Vector4"/>, <see cref="Vector3"/>, <see cref="Vector2"/> and <see cref="Single"/>
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Multiply%E2%80%93accumulate_operation">Multiply-accumulate operation</see>    
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Mul {Multiply}, Add {Addition}")]
    public readonly partial struct MultiplyAdd
    {
        #region constructor        

        public static MultiplyAdd CreateAdd(in Vector3 add) { return new MultiplyAdd(Vector4.One, new Vector4(add, add.Z)); }

        public static MultiplyAdd CreateAdd(in Vector4 add) { return new MultiplyAdd(Vector4.One, add); }

        public static MultiplyAdd CreateAdd(float x)
        {
            var add = new Vector4(x);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateAdd(float x, float y)
        {
            var add = new Vector4(x, y, y, y);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateAdd(float x, float y, float z)
        {
            var add = new Vector4(x, y, z, z);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateAdd(float x, float y, float z, float w)
        {            
            var add = new Vector4(x, y, z, w);
            return new MultiplyAdd(Vector4.One, add);
        }

        public static MultiplyAdd CreateMul(in Vector3 mul) { return new MultiplyAdd(new Vector4(mul, mul.Z), Vector4.Zero); }

        public static MultiplyAdd CreateMul(in Vector4 mul) { return new MultiplyAdd(mul, Vector4.Zero); }
        
        public static MultiplyAdd CreateMul(float x)
        {
            var mul = new Vector4(x);
            return new MultiplyAdd(mul, Vector4.Zero);
        }

        public static MultiplyAdd CreateMul(float x, float y)
        {
            var mul = new Vector4(x, y, y, y);
            return new MultiplyAdd(mul, Vector4.Zero);
        }

        public static MultiplyAdd CreateMul(float x, float y, float z)
        {
            var mul = new Vector4(x, y, z, z);
            return new MultiplyAdd(mul, Vector4.Zero);
        }

        public static MultiplyAdd CreateMul(float x, float y, float z, float w)
        {            
            var mul = new Vector4(x, y, z, w);
            return new MultiplyAdd(mul, Vector4.Zero);
        }
        
        public MultiplyAdd(in Vector4 mul, in Vector4 add)
        {
            this.Multiply = mul;
            this.Addition = add;
        }

        public MultiplyAdd(float mul, float add)
        {
            this.Multiply = new Vector4(mul);
            this.Addition = new Vector4(add);
        }        

        #endregion

        #region data

        private readonly _VectorXYZW Multiply;
        private readonly _VectorXYZW Addition;

        public static readonly MultiplyAdd Identity = new MultiplyAdd(Vector4.One, Vector4.Zero);

        #endregion

        #region API

        public bool IsIdentity => Multiply.XYZW == Vector4.One && Addition.XYZW == Vector4.Zero;        

        public MultiplyAdd GetTransposedWZYX()
        {
            return new MultiplyAdd
                (
                new Vector4(Multiply.W, Multiply.Z, Multiply.Y, Multiply.X),
                new Vector4(Addition.W, Addition.Z, Addition.Y, Addition.X)
                );
        }

        public MultiplyAdd GetTransposedZYXW()
        {
            return new MultiplyAdd
                (
                new Vector4(Multiply.Z, Multiply.Y, Multiply.X, Multiply.W),
                new Vector4(Addition.Z, Addition.Y, Addition.X, Addition.W)
                );
        }

        public MultiplyAdd GetInverse()
        {
            var m = Vector4.One / Multiply.XYZW;

            return new MultiplyAdd(m, -Addition.XYZW * m);
        }

        public MultiplyAdd ConcatMul(in Vector3 mul)
        {
            var mul4 = new Vector4(mul, mul.Z);
            return new MultiplyAdd(Multiply * mul4, Addition * mul4);
        }

        public MultiplyAdd ConcatMul(in Vector4 mul)
        {
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatMul(float x)
        {
            var mul = new Vector4(x);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatMul(float x, float y)
        {            
            var mul = new Vector4(x, y, y, y);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatMul(float x, float y, float z)
        {
            var mul = new Vector4(x, y, z, z);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatMul(float x, float y, float z, float w)
        {
            var mul = new Vector4(x, y, z, w);
            return new MultiplyAdd(Multiply * mul, Addition * mul);
        }

        public MultiplyAdd ConcatAdd(in Vector3 add)
        {
            return new MultiplyAdd(Multiply.XYZW, Addition + new Vector4(add, add.Z));
        }

        public MultiplyAdd ConcatAdd(in Vector4 add)
        {
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        public MultiplyAdd ConcatAdd(float x)
        {
            var add = new Vector4(x);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        public MultiplyAdd ConcatAdd(float x, float y)
        {            
            var add = new Vector4(x, y, y, y);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        public MultiplyAdd ConcatAdd(float x, float y, float z)
        {
            var add = new Vector4(x, y, z, z);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        public MultiplyAdd ConcatAdd(float x, float y, float z, float w)
        {
            var add = new Vector4(x, y, z, w);
            return new MultiplyAdd(Multiply.XYZW, Addition + add);
        }

        #endregion

        #region vector4

        public (Vector4 Multiply, Vector4 Add) GetVector4() { return (Multiply.XYZW, Addition.XYZW); }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Vector4 Transform(Vector4 value)
        {
            value *= Multiply.XYZW;
            value += Addition.XYZW;
            return value;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Transform(ref Vector4 value)
        {
            value *= Multiply.XYZW;
            value += Addition.XYZW;            
        }

        public void ApplyTransformTo(Span<Vector4> dst)
        {
            if (this.IsIdentity) return;

            var m = this.Multiply.XYZW;
            var a = this.Addition.XYZW;

            for(int i=0; i < dst.Length; ++i)
            {
                dst[i] = dst[i] * m + a;
            }
        }

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Vector4> dst, MultiplyAdd xform)
        {
            _ArrayUtilities.VerifyOverlap(src, dst);

            if (xform.IsIdentity)
            {
                src.Slice(0, dst.Length).CopyTo(dst);
                return;
            }

            var m = xform.Multiply.XYZW;
            var a = xform.Addition.XYZW;

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = src[i] * m + a;
            }            
        }

        public static void Transform(ReadOnlySpan<Vector4> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, Span<Single> dstW, MultiplyAdd xform)
        {
            if (xform.IsIdentity)
            {
                for (int i = 0; i < dstX.Length; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;
                    dstW[i] = v.W;
                }

                return;
            }
            
            for (int i = 0; i < dstX.Length; ++i)
            {
                var v = xform.Transform(src[i]);
                dstX[i] = v.X;
                dstY[i] = v.Y;
                dstZ[i] = v.Z;
                dstW[i] = v.W;
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

                return;
            }
            
            for (int x = 0; x < dst.Length; ++x)
            {
                var val = new Vector4(srcX[x], srcY[x], srcZ[x], srcW[x]);
                dst[x] = xform.Transform(val);
            }            
        }

        #endregion

        #region vector3

        public (Vector3 Multiply, Vector3 Add) GetVector3() { return (Multiply.XYZ, Addition.XYZ); }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool _TryGetHomogeneous3(out MultiplyAdd mad)
        {
            mad = this;
            if (Multiply.X != Multiply.Y) return false;
            if (Multiply.X != Multiply.Z) return false;
            if (Addition.X != Addition.Y) return false;
            if (Addition.X != Addition.Z) return false;
            mad = new MultiplyAdd(Multiply.X, Addition.X);
            return true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void _Split(Span<Vector3> span3, out Span<Vector4> span4, out Span<float> span1)
        {
            var len = (span3.Length * 3) / 4;
            span4 = MEMMARSHALL.Cast<Vector3, Vector4>(span3.Slice(0, len));

            len *= 4;
            span1 = MEMMARSHALL.Cast<Vector3, float>(span3).Slice(len);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void _Split(ReadOnlySpan<Vector3> span3, out ReadOnlySpan<Vector4> span4, out ReadOnlySpan<float> span1)
        {
            var len = (span3.Length * 3) / 4;
            span4 = MEMMARSHALL.Cast<Vector3, Vector4>(span3.Slice(0, len));

            len *= 4;
            span1 = MEMMARSHALL.Cast<Vector3, float>(span3).Slice(len);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Vector3 Transform(Vector3 value)
        {
            value *= Multiply.XYZ;
            value += Addition.XYZ;
            return value;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Transform(ref Vector3 value)
        {
            value *= Multiply.XYZ;
            value += Addition.XYZ;            
        }

        public void ApplyTransformTo(Span<Vector3> dst3)
        {
            if (this.IsIdentity) return;

            if (_TryGetHomogeneous3(out var mad))
            {
                _Split(dst3, out var dst4, out var dst1);                
                mad.ApplyTransformTo(dst4);                
                mad.ApplyTransformTo(dst1);
                return;
            }

            for (int i = 0; i < dst3.Length; ++i)
            {
                dst3[i] = Transform(dst3[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Vector3> src3, Span<Vector3> dst3, MultiplyAdd xform)
        {
            _ArrayUtilities.VerifyOverlap(src3, dst3);

            if (xform.IsIdentity)
            {
                src3.Slice(0, dst3.Length).CopyTo(dst3);
                return;
            }

            if (xform._TryGetHomogeneous3(out var mad))
            {
                _Split(src3, out var src4, out var src1);
                _Split(dst3, out var dst4, out var dst1);                
                Transform(src4, dst4, mad);                
                Transform(src1, dst1, mad);
                return;
            }

            for (int i = 0; i < dst3.Length; ++i)
            {
                dst3[i] = xform.Transform(src3[i]);
            }
            
        }

        public static void Transform(ReadOnlySpan<Vector3> src, Span<Single> dstX, Span<Single> dstY, Span<Single> dstZ, MultiplyAdd xform)
        {
            if (xform.IsIdentity)
            {
                for (int i = 0; i < dstX.Length; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;
                    dstZ[i] = v.Z;                    
                }
            }
            else
            {
                for (int i = 0; i < dstX.Length; ++i)
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

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
            _ArrayUtilities.VerifyOverlap(src, dst);

            if (xform.IsIdentity)
            {
                src.Slice(0, dst.Length).CopyTo(dst);
            }
            else
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = xform.Transform(src[i]);
                }
            }
        }

        public static void Transform(ReadOnlySpan<Vector2> src, Span<Single> dstX, Span<Single> dstY, MultiplyAdd xform)
        {
            if (xform.IsIdentity)
            {
                for (int i = 0; i < dstX.Length; ++i)
                {
                    var v = src[i];
                    dstX[i] = v.X;
                    dstY[i] = v.Y;                    
                }
            }
            else
            {
                for (int i = 0; i < dstX.Length; ++i)
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

        #region scalar

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void _Split(Span<float> span, out Span<Vector4> span4, out Span<float> span1)
        {
            var len = span.Length / 4;
            span4 = MEMMARSHALL.Cast<float, Vector4>(span.Slice(0, len));

            len *= 4;
            span1 = MEMMARSHALL.Cast<float, float>(span).Slice(len);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void _Split(ReadOnlySpan<float> span, out ReadOnlySpan<Vector4> span4, out ReadOnlySpan<float> span1)
        {
            var len = span.Length / 4;
            span4 = MEMMARSHALL.Cast<float, Vector4>(span.Slice(0, len));

            len *= 4;
            span1 = MEMMARSHALL.Cast<float, float>(span).Slice(len);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Single Transform(Single value)
        {
            value *= Multiply.X;
            value += Addition.X;
            return value;
        }

        public void ApplyTransformTo(Span<Single> dst)
        {
            if (this.IsIdentity) return;
            
            _Split(dst, out var dst4, out var dst1);

            new MultiplyAdd(Multiply.X, Addition.X).ApplyTransformTo(dst4);

            for (int i = 0; i < dst1.Length; ++i)
            {
                dst1[i] = Transform(dst1[i]);
            }
        }

        public static void Transform(ReadOnlySpan<Single> src, Span<Single> dst, MultiplyAdd xform)
        {
            _ArrayUtilities.VerifyOverlap(src, dst);

            if (xform.IsIdentity)
            {
                src.Slice(0, dst.Length).CopyTo(dst);
                return;
            }

            _Split(src, out var src4, out var src1);
            _Split(dst, out var dst4, out var dst1);

            Transform(src4, dst4, new MultiplyAdd(xform.Multiply.X, xform.Addition.X));

            for (int i = 0; i < dst1.Length; ++i)
            {
                dst1[i] = xform.Transform(src1[i]);
            }            
        }

        #endregion
    }


    [System.Diagnostics.DebuggerDisplay("{XYZW}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    readonly struct _VectorXYZW
    {
        #region constructor

        public static implicit operator _VectorXYZW(Vector4 v) { return new _VectorXYZW(v); }

        public _VectorXYZW(Vector4 value)
        {
            this = default;
            XYZW = value;
        }

        public _VectorXYZW(Vector3 value, float w)
        {
            this = default;
            XYZW = new Vector4(value, w);
        }

        #endregion

        #region data

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

        #endregion

        #region API

        public static Vector4 operator *(_VectorXYZW a, float b) { return a.XYZW * b; }
        public static Vector4 operator *(_VectorXYZW a, Vector4 b) { return a.XYZW * b; }
        public static Vector4 operator +(_VectorXYZW a, Vector4 b) { return a.XYZW + b; }

        #endregion
    }
}
