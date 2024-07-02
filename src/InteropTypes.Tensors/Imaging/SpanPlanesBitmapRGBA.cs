using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


using MMARSHAL = System.Runtime.InteropServices.MemoryMarshal;


namespace InteropTypes.Tensors.Imaging
{

    /// <summary>
    /// Represents a bitmap image defined as four R,G,B,A separated planes
    /// </summary>
    /// <typeparam name="T">The component type, typically <see cref="Byte"/> or <see cref="Single"/></typeparam>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    readonly ref struct SpanPlanesBitmapRGBA<T>
        where T : unmanaged, IConvertible
    {
        #region diagnostics

        private string _GetDebuggerDisplayString()
        {
            var text = $"{Width}x{Height}x{typeof(T).Name} ";

            /*
            var (minR, maxR) = _ChannelR.GetMinMax();
            var (minG, maxG) = _ChannelG.GetMinMax();
            var (minB, maxB) = _ChannelB.GetMinMax();

            text += $" R:[{minR} < {maxR}]";
            text += $" G:[{minG} < {maxG}]";
            text += $" B:[{minB} < {maxB}]";
            */

            return text;
        }

        #endregion

        #region lifecycle

        public SpanPlanesBitmapRGBA(int width, int height, ColorEncoding encoding)
        {
            _GuardTemplateParam();

            Width = width;
            Height = height;
            SpanTensor3<T> tensor = new SpanTensor3<T>(encoding.GetChannelCount(), width, height); 
            this = CreateFrom(tensor, encoding);
        }

        public static SpanPlanesBitmapRGBA<T> CreateFrom(SpanTensor3<T> tensor, ColorEncoding encoding)
        {
            _GuardTemplateParam();

            switch (encoding)
            {
                case ColorEncoding.A: return new SpanPlanesBitmapRGBA<T>(tensor[0]);
                case ColorEncoding.L: return new SpanPlanesBitmapRGBA<T>(tensor[0]);
                case ColorEncoding.RGB: return new SpanPlanesBitmapRGBA<T>(tensor[0], tensor[1], tensor[2]);
                case ColorEncoding.BGR: return new SpanPlanesBitmapRGBA<T>(tensor[2], tensor[1], tensor[0]);
                case ColorEncoding.RGBA: return new SpanPlanesBitmapRGBA<T>(tensor[0], tensor[1], tensor[2], tensor[3]);
                case ColorEncoding.BGRA: return new SpanPlanesBitmapRGBA<T>(tensor[2], tensor[1], tensor[0], tensor[3]);
                case ColorEncoding.ARGB: return new SpanPlanesBitmapRGBA<T>(tensor[3], tensor[0], tensor[1], tensor[2]);
                default: throw new NotImplementedException();
            }
        }

        public SpanPlanesBitmapRGBA(SpanTensor2<T> rPlane, SpanTensor2<T> gPlane, SpanTensor2<T> bPlane, SpanTensor2<T> aPlane)
        {
            _GuardTemplateParam();

            if (rPlane.Dimensions != gPlane.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(gPlane));
            if (rPlane.Dimensions != bPlane.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(bPlane));
            if (rPlane.Dimensions != aPlane.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(aPlane));

            Width = rPlane.Dimensions.Dim1;
            Height = rPlane.Dimensions.Dim0;

            _PlaneR = rPlane.Span;
            _PlaneG = gPlane.Span;
            _PlaneB = bPlane.Span;
            _PlaneA = aPlane.Span;
        }

        public SpanPlanesBitmapRGBA(SpanTensor2<T> rPlane, SpanTensor2<T> gPlane, SpanTensor2<T> bPlane)
        {
            _GuardTemplateParam();

            if (rPlane.Dimensions != gPlane.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(gPlane));
            if (rPlane.Dimensions != bPlane.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(bPlane));            

            Width = rPlane.Dimensions.Dim1;
            Height = rPlane.Dimensions.Dim0;

            _PlaneR = rPlane.Span;
            _PlaneG = gPlane.Span;
            _PlaneB = bPlane.Span;
            _PlaneA = default;
        }

        public SpanPlanesBitmapRGBA(SpanTensor2<T> aPlane)
        {
            _GuardTemplateParam();

            Width = aPlane.Dimensions.Dim1;
            Height = aPlane.Dimensions.Dim0;

            _PlaneR = default;
            _PlaneG = default;
            _PlaneB = default;
            _PlaneA = aPlane.Span;
        }

        private static void _GuardTemplateParam()
        {
            if (typeof(T) == typeof(Byte)) return;
            if (typeof(T) == typeof(float)) return;

            throw new InvalidOperationException("must by byte or float");
        }

        #endregion

        #region data

        private readonly Span<T> _PlaneR;
        private readonly Span<T> _PlaneG;
        private readonly Span<T> _PlaneB;
        private readonly Span<T> _PlaneA;        

        public int Width { get; }
        public int Height { get; }

        #endregion

        #region properties

        public TensorSize2 Dimensions => new TensorSize2(Height, Width);

        public SpanTensor2<T> PlaneRed => _PlaneR.IsEmpty ? default : new SpanTensor2<T>(_PlaneR, Height, Width);
        public SpanTensor2<T> PlaneGreen => _PlaneG.IsEmpty ? default : new SpanTensor2<T>(_PlaneG, Height, Width);
        public SpanTensor2<T> PlaneBlue => _PlaneB.IsEmpty ? default : new SpanTensor2<T>(_PlaneB, Height, Width);
        public SpanTensor2<T> PlaneAlpha => _PlaneA.IsEmpty ? default : new SpanTensor2<T>(_PlaneA, Height, Width);

        public ColorEncoding PixelEncoding
        {
            get
            {
                bool hasRGB = !(_PlaneR.IsEmpty || _PlaneG.IsEmpty || _PlaneB.IsEmpty);

                if (_PlaneA.IsEmpty)
                {
                    return hasRGB
                        ? ColorEncoding.RGB
                        : throw new NotImplementedException();
                }
                else
                {
                    return hasRGB
                        ? ColorEncoding.RGBA
                        : ColorEncoding.A;
                }
            }
        }

        #endregion

        #region API        

        public void SetPixels<TSrc>(SpanPlanesBitmapRGBA<TSrc> src, MultiplyAdd mad)
            where TSrc : unmanaged, IConvertible
        {
            TensorSize2.GuardEquals("this",nameof(src), this.Dimensions, src.Dimensions);

            // throw on pixel encoding issues

            var (mul, add) = mad.GetVector4();

            if (!this._PlaneR.IsEmpty && !src._PlaneR.IsEmpty) { _CopyChannel<TSrc>(src.PlaneRed, this.PlaneRed, mul.X, add.X); }
            if (!this._PlaneG.IsEmpty && !src._PlaneG.IsEmpty) { _CopyChannel<TSrc>(src.PlaneGreen, this.PlaneGreen, mul.Y, add.Y); }
            if (!this._PlaneB.IsEmpty && !src._PlaneB.IsEmpty) { _CopyChannel<TSrc>(src.PlaneBlue, this.PlaneBlue, mul.Z, add.Z); }
            if (!this._PlaneA.IsEmpty && !src._PlaneA.IsEmpty) { _CopyChannel<TSrc>(src.PlaneAlpha, this.PlaneAlpha, mul.W, add.W); }
        }

        private static void _CopyChannel<TSrc>(ReadOnlySpanTensor2<TSrc> src, SpanTensor2<T> dst, float mul, float add)
            where TSrc : unmanaged
        {
            if (typeof(TSrc) == typeof(float))
            {
                var dstSingles = dst.Cast<float>().Span;

                if (typeof(T) == typeof(Byte)) { src.Cast<Byte>().Span.ScaledMultiplyAddTo(mul, add, dstSingles); return; }
                if (typeof(T) == typeof(float)) { src.Cast<float>().Span.MultiplyAddTo(mul, add, dstSingles); return; }
            }

            throw new NotImplementedException();
        }

        internal unsafe Vector4 _GetScalarPixelUnchecked(int x, int y)
        {
            var idx = y * Width + x;

            if (typeof(T) == typeof(Byte))
            {
                var pr = _PlaneR.IsEmpty ? default : MMARSHAL.Cast<T, Byte>(_PlaneR)[idx];
                var pg = _PlaneG.IsEmpty ? default : MMARSHAL.Cast<T, Byte>(_PlaneG)[idx];
                var pb = _PlaneB.IsEmpty ? default : MMARSHAL.Cast<T, Byte>(_PlaneB)[idx];
                var pa = _PlaneA.IsEmpty ? default : MMARSHAL.Cast<T, Byte>(_PlaneA)[idx];
                return new Vector4(pr, pg, pb, pa) / 255f;
            }

            if (typeof(T) == typeof(float))
            {
                var pr = _PlaneR.IsEmpty ? default : MMARSHAL.Cast<T, float>(_PlaneR)[idx];
                var pg = _PlaneG.IsEmpty ? default : MMARSHAL.Cast<T, float>(_PlaneG)[idx];
                var pb = _PlaneB.IsEmpty ? default : MMARSHAL.Cast<T, float>(_PlaneB)[idx];
                var pa = _PlaneA.IsEmpty ? default : MMARSHAL.Cast<T, float>(_PlaneA)[idx];
                return new Vector4(pr, pg, pb, pa);
            }

            throw new NotImplementedException(typeof(T).Name);
        }

        internal unsafe void _SetScalarPixelUnchecked(int x, int y, Vector4 value)
        {
            var idx = y * Width + x;

            if (typeof(T) == typeof(Byte))
            {
                var pr = MMARSHAL.Cast<T, Byte>(_PlaneR);
                var pg = MMARSHAL.Cast<T, Byte>(_PlaneG);
                var pb = MMARSHAL.Cast<T, Byte>(_PlaneB);
                var pa = MMARSHAL.Cast<T, Byte>(_PlaneA);

                value *= 255;

                if (!pr.IsEmpty) pr[idx] = (byte)value.X;
                if (!pg.IsEmpty) pg[idx] = (byte)value.Y;
                if (!pb.IsEmpty) pb[idx] = (byte)value.Z;
                if (!pa.IsEmpty) pa[idx] = (byte)value.W;
                return;

            }

            if (typeof(T) == typeof(float))
            {
                var pr = MMARSHAL.Cast<T, float>(_PlaneR);
                var pg = MMARSHAL.Cast<T, float>(_PlaneG);
                var pb = MMARSHAL.Cast<T, float>(_PlaneB);
                var pa = MMARSHAL.Cast<T, float>(_PlaneA);                

                if (!pr.IsEmpty) pr[idx] = value.X;
                if (!pg.IsEmpty) pg[idx] = value.Y;
                if (!pb.IsEmpty) pb[idx] = value.Z;
                if (!pa.IsEmpty) pa[idx] = value.W;
                return;
            }

            throw new NotImplementedException(typeof(T).Name);
        }

        public void ApplyMultiplyAdd(MultiplyAdd mad)
        {
            if (typeof(T) != typeof(float)) throw new InvalidOperationException("invalid type");            

            var rrr = MMARSHAL.Cast<T, float>(_PlaneR);
            mad.X.ApplyTransformTo(rrr);

            var ggg = MMARSHAL.Cast<T, float>(_PlaneG);
            mad.Y.ApplyTransformTo(ggg);

            var bbb = MMARSHAL.Cast<T, float>(_PlaneB);
            mad.Z.ApplyTransformTo(bbb);

            var aaa = MMARSHAL.Cast<T, float>(_PlaneA);
            mad.W.ApplyTransformTo(aaa);
        }



        public unsafe void CopyToInterleavedRow(int y, Span<T> dstRow, ColorEncoding dstEncoding)
        {
            if (typeof(T) != typeof(Byte) || typeof(T) != typeof(float)) throw new InvalidOperationException();

            _Shuffle<T>(dstEncoding, out var srcXX, out var srcYY, out var srcZZ, out var srcWW);
            srcXX = srcXX.IsEmpty ? default : srcXX.Slice(y * Width, Width);
            srcYY = srcYY.IsEmpty ? default : srcYY.Slice(y * Width, Width);
            srcZZ = srcZZ.IsEmpty ? default : srcZZ.Slice(y * Width, Width);
            srcWW = srcWW.IsEmpty ? default : srcWW.Slice(y * Width, Width);

            switch(dstEncoding)
            {
                case ColorEncoding.A:
                case ColorEncoding.L:
                    if (dstRow.Length < Width) throw new ArgumentException("too small", nameof(dstRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstRow[x + 0] = srcWW[x];
                    }
                    break;
                case ColorEncoding.RGB:
                case ColorEncoding.BGR:
                    if (dstRow.Length < Width * 3) throw new ArgumentException("too small", nameof(dstRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstRow[x * 3 + 0] = srcXX[x];
                        dstRow[x * 3 + 1] = srcYY[x];
                        dstRow[x * 3 + 2] = srcZZ[x];
                    }
                    break;
                case ColorEncoding.RGBA:
                case ColorEncoding.BGRA:
                case ColorEncoding.ARGB:
                    if (dstRow.Length < Width * 4) throw new ArgumentException("too small", nameof(dstRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstRow[x * 4 + 0] = srcXX[x];
                        dstRow[x * 4 + 1] = srcYY[x];
                        dstRow[x * 4 + 2] = srcZZ[x];
                        dstRow[x * 4 + 3] = srcWW[x];
                    }
                    break;
            }            
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetInterleavedRow<TSrcPixel>(int y, ReadOnlySpanTensor2<TSrcPixel> srcRow, ColorEncoding srcEncoding)
            where TSrcPixel : unmanaged
        {
            if (typeof(TSrcPixel) == typeof(Vector3) || typeof(TSrcPixel) == typeof(Vector4)) { SetInterleavedRow(y, srcRow.Cast<float>(), srcEncoding); return; }
            if (sizeof(TSrcPixel) == 3) { SetInterleavedRow(y, srcRow.Cast<byte>(), srcEncoding); return; }

            if (typeof(T) == typeof(float))
            {
                if (typeof(TSrcPixel) == typeof(float)) // float to float
                {
                    var srcFFF = srcRow.Cast<float>();
                }

                else if (sizeof(TSrcPixel) <= 4) // byte to float
                {
                    var srcBytes = srcRow.Cast<Byte>().Span;
                    Span<T> rowSingles = stackalloc T[srcBytes.Length];

                    _ArrayUtilities.TryConvertSpan(srcBytes, rowSingles);

                    SetInterleavedRow(y, rowSingles, srcEncoding);
                    return;
                }                
            }

            if (typeof(T) == typeof(Byte))
            {
                if (typeof(TSrcPixel) == typeof(Vector3) || typeof(TSrcPixel) == typeof(Vector4))
                {
                    var srcSingles = srcRow.Cast<float>();
                    Span<T> rowSingles = stackalloc T[srcSingles.Span.Length];

                    // _ArrayUtilities.TryConvertSpan(srcSingles, rowSingles);
                    throw new NotImplementedException();

                    SetInterleavedRow(y, rowSingles, srcEncoding);
                    return;
                }
            }

            switch (srcEncoding)
            {
                case ColorEncoding.RGB:
                    if (sizeof(T) * 3 != sizeof(TSrcPixel)) throw new InvalidOperationException("TPixel size mismatch");
                    break;
                case ColorEncoding.BGR:
                    if (sizeof(T) * 3 != sizeof(TSrcPixel)) throw new InvalidOperationException("TPixel size mismatch");
                    break;
                default: throw new ArgumentException("invalid encoding", nameof(srcEncoding));
            }

            if (srcRow.Span.Length < Width) throw new ArgumentException("too small", nameof(srcRow));
            
            var srcRGB = MMARSHAL.Cast<TSrcPixel, T>(srcRow.Span.Slice(0, Width));

            SetInterleavedRow(y, srcRGB, srcEncoding);
        }
        
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetInterleavedRow(int y, ReadOnlySpan<T> srcRow, ColorEncoding srcEncoding)
        {
            if (typeof(T) != typeof(Byte) || typeof(T) != typeof(float)) throw new InvalidOperationException();

            _Shuffle<T>(srcEncoding, out var dstXX, out var dstYY, out var dstZZ, out var dstWW);
            dstXX = dstXX.IsEmpty ? default : dstXX.Slice(y * Width, Width);
            dstYY = dstYY.IsEmpty ? default : dstYY.Slice(y * Width, Width);
            dstZZ = dstZZ.IsEmpty ? default : dstZZ.Slice(y * Width, Width);
            dstWW = dstWW.IsEmpty ? default : dstWW.Slice(y * Width, Width);

            switch (srcEncoding)
            {
                case ColorEncoding.A:
                case ColorEncoding.L:
                    if (srcRow.Length < Width) throw new ArgumentException("too small", nameof(srcRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstWW[x + 0] = srcRow[x];
                    }
                    break;
                case ColorEncoding.RGB:
                case ColorEncoding.BGR:
                    if (srcRow.Length < Width * 3) throw new ArgumentException("too small", nameof(srcRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstXX[x] = srcRow[x * 3 + 0];
                        dstYY[x] = srcRow[x * 3 + 0];
                        dstZZ[x] = srcRow[x * 3 + 0];
                    }
                    break;
                case ColorEncoding.RGBA:
                case ColorEncoding.BGRA:
                case ColorEncoding.ARGB:
                    if (srcRow.Length < Width * 4) throw new ArgumentException("too small", nameof(srcRow));
                    for (int x = 0; x < Width; ++x)
                    {
                        dstXX[x] = srcRow[x * 4 + 0];
                        dstYY[x] = srcRow[x * 4 + 1];
                        dstZZ[x] = srcRow[x * 4 + 2];
                        dstWW[x] = srcRow[x * 4 + 3];
                    }
                    break;
            }

            /*
            if (srcRow.Length < Width * 3) throw new ArgumentException("too small", nameof(srcRow));

            srcRow = srcRow.Slice(0, Width);
            var srcRGB = srcRow;            

            ref var srcPtr = ref MMARSHAL.GetReference(srcRGB);

            var idx = y * Width;

            for (int i = 0; i < dstR.Length; i++)
            {
                dstR[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
                dstG[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
                dstB[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
            }*/
        }

        private unsafe void _Shuffle<TElement>(ColorEncoding encoding, out Span<TElement> xxx, out Span<TElement> yyy, out Span<TElement> zzz, out Span<TElement> www)
            where TElement : unmanaged
        {
            if (sizeof(T) != sizeof(TElement)) throw new InvalidOperationException("TElement must be of the same size as T");

            switch (encoding)
            {
                case ColorEncoding.A:
                case ColorEncoding.L:
                    xxx = default;
                    yyy = default;
                    zzz = default;
                    www = MMARSHAL.Cast<T, TElement>(_PlaneA);
                    break;
                case ColorEncoding.RGB:
                    xxx = MMARSHAL.Cast<T, TElement>(_PlaneR);
                    yyy = MMARSHAL.Cast<T, TElement>(_PlaneG);
                    zzz = MMARSHAL.Cast<T, TElement>(_PlaneB);
                    www = default;
                    break;
                case ColorEncoding.BGR:
                    xxx = MMARSHAL.Cast<T, TElement>(_PlaneB);
                    yyy = MMARSHAL.Cast<T, TElement>(_PlaneG);
                    zzz = MMARSHAL.Cast<T, TElement>(_PlaneR);
                    www = default;
                    break;
                case ColorEncoding.RGBA:
                    xxx = MMARSHAL.Cast<T, TElement>(_PlaneR);
                    yyy = MMARSHAL.Cast<T, TElement>(_PlaneG);
                    zzz = MMARSHAL.Cast<T, TElement>(_PlaneB);
                    www = MMARSHAL.Cast<T, TElement>(_PlaneA);
                    break;
                case ColorEncoding.BGRA:
                    xxx = MMARSHAL.Cast<T, TElement>(_PlaneB);
                    yyy = MMARSHAL.Cast<T, TElement>(_PlaneG);
                    zzz = MMARSHAL.Cast<T, TElement>(_PlaneR);
                    www = MMARSHAL.Cast<T, TElement>(_PlaneA);
                    break;
                case ColorEncoding.ARGB:
                    xxx = MMARSHAL.Cast<T, TElement>(_PlaneA);
                    yyy = MMARSHAL.Cast<T, TElement>(_PlaneR);
                    zzz = MMARSHAL.Cast<T, TElement>(_PlaneG);
                    www = MMARSHAL.Cast<T, TElement>(_PlaneB);
                    break;
                default: throw new ArgumentException("invalid encoding", nameof(encoding));
            }

        }


        public ColorRanges EvaluateGetContentColorRanges()
        {
            var ranges = new ColorRanges.Serializable(float.MaxValue, float.MinValue);

            var (min,max) = PlaneRed.Span.GetMinMax();
            ranges.RedMin = min.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
            ranges.RedMax = max.ToSingle(System.Globalization.CultureInfo.InvariantCulture);

            (min, max) = PlaneGreen.Span.GetMinMax();
            ranges.GreenMin = min.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
            ranges.GreenMax = max.ToSingle(System.Globalization.CultureInfo.InvariantCulture);

            (min, max) = PlaneBlue.Span.GetMinMax();
            ranges.BlueMin = min.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
            ranges.BlueMax = max.ToSingle(System.Globalization.CultureInfo.InvariantCulture);

            (min, max) = PlaneAlpha.Span.GetMinMax();
            ranges.AlphaMin = min.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
            ranges.AlphaMax = max.ToSingle(System.Globalization.CultureInfo.InvariantCulture);

            return ranges;
        }

        #endregion
    }
}
