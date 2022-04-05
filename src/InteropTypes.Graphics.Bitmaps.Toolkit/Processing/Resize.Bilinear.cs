using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    static class _ResizeBilinearImplementation
    {
        #region API

        public static void FitPixels(SpanBitmap src, SpanBitmap dst, (float offset, float scale) transform)
        {
            if (src.Width == dst.Width && src.Height == dst.Height)
            {
                dst.SetPixels(0, 0, src);
                return;
            }

            if (dst.PixelFormat == Pixel.BGR24.Format)
            {
                var dstX = dst.OfType<Pixel.BGR24>();

                switch (src.PixelFormat.Code)
                {
                    case Pixel.BGR24.Code: _FitPixels3(src.OfType<Pixel.BGR24>(), dstX, transform); return;
                    case Pixel.RGB24.Code: _FitPixels3(src.OfType<Pixel.RGB24>(), dstX, transform); return;
                    case Pixel.ARGB32.Code: _FitPixels3(src.OfType<Pixel.ARGB32>(), dstX, transform); return;
                    case Pixel.RGBA32.Code: _FitPixels3(src.OfType<Pixel.RGBA32>(), dstX, transform); return;
                    case Pixel.BGRA32.Code: _FitPixels3(src.OfType<Pixel.BGRA32>(), dstX, transform); return;
                    case Pixel.RGB96F.Code: _FitPixels3(src.OfType<Pixel.RGB96F>(), dstX, transform); return;
                    case Pixel.BGR96F.Code: _FitPixels3(src.OfType<Pixel.BGR96F>(), dstX, transform); return;
                    case Pixel.BGRA128F.Code: _FitPixels3(src.OfType<Pixel.BGRA128F>(), dstX, transform); return;
                    case Pixel.RGBA128F.Code: _FitPixels3(src.OfType<Pixel.RGBA128F>(), dstX, transform); return;
                }
            }

            if (dst.PixelFormat == Pixel.RGB24.Format)
            {
                var dstX = dst.OfType<Pixel.RGB24>();

                switch (src.PixelFormat.Code)
                {
                    case Pixel.BGR24.Code: _FitPixels3(src.OfType<Pixel.BGR24>(), dstX, transform); return;
                    case Pixel.RGB24.Code: _FitPixels3(src.OfType<Pixel.RGB24>(), dstX, transform); return;
                    case Pixel.ARGB32.Code: _FitPixels3(src.OfType<Pixel.ARGB32>(), dstX, transform); return;
                    case Pixel.RGBA32.Code: _FitPixels3(src.OfType<Pixel.RGBA32>(), dstX, transform); return;
                    case Pixel.BGRA32.Code: _FitPixels3(src.OfType<Pixel.BGRA32>(), dstX, transform); return;
                    case Pixel.RGB96F.Code: _FitPixels3(src.OfType<Pixel.RGB96F>(), dstX, transform); return;
                    case Pixel.BGR96F.Code: _FitPixels3(src.OfType<Pixel.BGR96F>(), dstX, transform); return;
                    case Pixel.BGRA128F.Code: _FitPixels3(src.OfType<Pixel.BGRA128F>(), dstX, transform); return;
                    case Pixel.RGBA128F.Code: _FitPixels3(src.OfType<Pixel.RGBA128F>(), dstX, transform); return;
                }
            }

            if (dst.PixelFormat == Pixel.BGR96F.Format)
            {
                var dstX = dst.OfType<Pixel.BGR96F>();

                switch (src.PixelFormat.Code)
                {
                    case Pixel.BGR24.Code: _FitPixels3(src.OfType<Pixel.BGR24>(), dstX, transform); return;
                    case Pixel.RGB24.Code: _FitPixels3(src.OfType<Pixel.RGB24>(), dstX, transform); return;
                    case Pixel.ARGB32.Code: _FitPixels3(src.OfType<Pixel.ARGB32>(), dstX, transform); return;
                    case Pixel.RGBA32.Code: _FitPixels3(src.OfType<Pixel.RGBA32>(), dstX, transform); return;
                    case Pixel.BGRA32.Code: _FitPixels3(src.OfType<Pixel.BGRA32>(), dstX, transform); return;
                    case Pixel.RGB96F.Code: _FitPixels3(src.OfType<Pixel.RGB96F>(), dstX, transform); return;
                    case Pixel.BGR96F.Code: _FitPixels3(src.OfType<Pixel.BGR96F>(), dstX, transform); return;
                    case Pixel.BGRA128F.Code: _FitPixels3(src.OfType<Pixel.BGRA128F>(), dstX, transform); return;
                    case Pixel.RGBA128F.Code: _FitPixels3(src.OfType<Pixel.RGBA128F>(), dstX, transform); return;
                }
            }

            if (dst.PixelFormat == Pixel.RGB96F.Format)
            {
                var dstX = dst.OfType<Pixel.RGB96F>();

                switch (src.PixelFormat.Code)
                {
                    case Pixel.BGR24.Code: _FitPixels3(src.OfType<Pixel.BGR24>(), dstX, transform); return;
                    case Pixel.RGB24.Code: _FitPixels3(src.OfType<Pixel.RGB24>(), dstX, transform); return;
                    case Pixel.ARGB32.Code: _FitPixels3(src.OfType<Pixel.ARGB32>(), dstX, transform); return;
                    case Pixel.RGBA32.Code: _FitPixels3(src.OfType<Pixel.RGBA32>(), dstX, transform); return;
                    case Pixel.BGRA32.Code: _FitPixels3(src.OfType<Pixel.BGRA32>(), dstX, transform); return;
                    case Pixel.RGB96F.Code: _FitPixels3(src.OfType<Pixel.RGB96F>(), dstX, transform); return;
                    case Pixel.BGR96F.Code: _FitPixels3(src.OfType<Pixel.BGR96F>(), dstX, transform); return;
                    case Pixel.BGRA128F.Code: _FitPixels3(src.OfType<Pixel.BGRA128F>(), dstX, transform); return;
                    case Pixel.RGBA128F.Code: _FitPixels3(src.OfType<Pixel.RGBA128F>(), dstX, transform); return;
                }
            }

            // TODO: for pixels with alpha support, alpha premultiplication should be required.

            throw new NotImplementedException($"{dst.PixelFormat} format not inplemented on {nameof(dst)}.");
        }

        private static void _FitPixels3<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, (float offset, float scale) transform)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IValueSetter<Pixel.RGB96F>
        {
            var (colPairs, rowPairs) = _BilinearSampleSource.Create(src.Info, dst.Info);

            var rowContext = new _RowBlendKernel3<TSrcPixel>(src, colPairs, transform);

            for (int dstY = 0; dstY < dst.Height; ++dstY)
            {
                var rowPair = rowPairs.AsSpan()[dstY];

                var r0 = rowContext.GetResizedRow(rowPair.IndexLeft);  // top row to blend
                var r1 = rowContext.GetResizedRow(rowPair.IndexRight); // bottom row to blend

                var dstRow = dst.UseScanlinePixels(dstY);

                Pixel.LerpArray(r0, r1, rowPair.Amount, dstRow);
            }
        }

        #endregion

        #region nested types

        /// <summary>
        /// Takes an input <see cref="SpanBitmap"/> and exposes its rows, resized according to a <see cref="_BilinearSampleSource"/> table.
        /// </summary>
        ref struct _RowBlendKernel3<TSrcPixel> where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            #region constructor

            [ThreadStatic]
            private static Vector3[] _Vector3Cache;

            private static (ArraySegment<Vector3> src, ArraySegment<Vector3> dst0, ArraySegment<Vector3> dst1) _CreateCache(int srcw, int dstw)
            {
                var len = srcw + dstw + dstw;
                if (_Vector3Cache == null || _Vector3Cache.Length < len) _Vector3Cache = new Vector3[len];

                var src = new ArraySegment<Vector3>(_Vector3Cache, 0, srcw);
                var dst0 = new ArraySegment<Vector3>(_Vector3Cache, srcw, dstw);
                var dst1 = new ArraySegment<Vector3>(_Vector3Cache, srcw + dstw, dstw);

                return (src, dst0, dst1);
            }

            public _RowBlendKernel3(SpanBitmap<TSrcPixel> srcBitmap, ReadOnlySpan<_BilinearSampleSource> pairs, (float offset, float scale) transform)
            {
                var (srcRow, dstRow0, dstRow1) = _CreateCache(srcBitmap.Width, pairs.Length);

                _Source = srcBitmap;
                _Pairs = pairs;
                _Temp = srcRow;
                _Row0 = dstRow0;
                _Row1 = dstRow1;
                _Index0 = -1;
                _Index1 = -1;

                _Transform = transform;
            }

            #endregion

            #region data

            private readonly SpanBitmap<TSrcPixel> _Source;
            private readonly ReadOnlySpan<_BilinearSampleSource> _Pairs;
            private readonly Span<Vector3> _Temp;
            private readonly Span<Vector3> _Row0;
            private readonly Span<Vector3> _Row1;

            private (float offset, float scale) _Transform;

            private int _Index0;
            private int _Index1;

            #endregion

            #region API

            private Span<Vector3> _GetSourceRow(int idx)
            {
                var srcRow = _Source.GetScanlinePixels(idx);
                var dstRow = _Temp;

                for (int i = 0; i < srcRow.Length; ++i)
                {
                    var pixel = srcRow[i].To<Pixel.RGB96F>().RGB;
                    dstRow[i] = new Vector3(pixel.X, pixel.Y, pixel.Z);
                }

                return _Temp;
            }

            public Span<Vector3> GetResizedRow(int idx)
            {
                // if the row already exists from a previous pass, use it
                if (idx == _Index0) return _Row0;
                if (idx == _Index1) return _Row1;

                var srcRow = _GetSourceRow(idx);

                if (_Index0 < 0)
                {
                    _BilinearSampleSource.ApplyTo(_Row0, srcRow, _Pairs);
                    _Index0 = idx;
                    _Index1 = -1; // invalidate the opposite row
                    return _Row0;
                }

                if (_Index1 < 0)
                {
                    _BilinearSampleSource.ApplyTo(_Row1, srcRow, _Pairs);
                    _Index1 = idx;
                    _Index0 = -1; // invalidate the opposite row
                    return _Row1;
                }

                throw new NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creates an equivalence table to map a source and destination set of pixels,
        /// so there's two weighted source pixels for every destination pixel.
        /// </summary>
        /// <example>
        /// pair = table[x];
        /// dstRow[x] = Lerp( srcRow[pair.<see cref="IndexLeft"/>], srcRow[pair.<see cref="IndexRight"/>], pair.<see cref="Amount"/>);
        /// </example>
        [System.Diagnostics.DebuggerDisplay("{IndexLeft} < {Amount} > {IndexRight}")]
        readonly struct _BilinearSampleSource
        {
            #region static methods

            [ThreadStatic]
            private static _BilinearSampleSource[] _BSPairs;

            public static (ArraySegment<_BilinearSampleSource> cols, ArraySegment<_BilinearSampleSource> rows) Create(BitmapInfo src, BitmapInfo dst)
            {
                var tables = Create(dst.Width, dst.Height);

                Initialize(src.Width, tables.cols);
                Initialize(src.Height, tables.rows);

                return tables;
            }

            public static (ArraySegment<_BilinearSampleSource> cols, ArraySegment<_BilinearSampleSource> rows) Create(int width, int height)
            {
                var len = width + height;
                if (_BSPairs == null || _BSPairs.Length < len) _BSPairs = new _BilinearSampleSource[len];

                var cols = new ArraySegment<_BilinearSampleSource>(_BSPairs, 0, width);
                var rows = new ArraySegment<_BilinearSampleSource>(_BSPairs, width, height);

                return (cols, rows);
            }

            public static void Initialize(int srcLen, Span<_BilinearSampleSource> dstPairs)
            {
                if (srcLen > dstPairs.Length) { InitializeShrink(srcLen, dstPairs); return; }
                if (srcLen < dstPairs.Length) { InitializeExpand(srcLen, dstPairs); return; }

                for (int i = 0; i < dstPairs.Length; ++i)
                {
                    dstPairs[i] = new _BilinearSampleSource(i, i, 0);
                }

            }

            private static void InitializeShrink(int srcLen, Span<_BilinearSampleSource> dstPairs)
            {
                var ratio = (float)srcLen / (float)dstPairs.Length;

                for (int i = 0; i < dstPairs.Length; ++i)
                {
                    var idx0 = i + 0;
                    var idx1 = i + 1;

                    float leftSide = (float)(idx0) * ratio;
                    float rightSide = (float)(idx1) * ratio;

                    idx0 = (int)leftSide; if (idx0 >= srcLen) idx0 = srcLen - 1;
                    idx1 = (int)rightSide; if (idx1 >= srcLen) idx1 = srcLen - 1;

                    var amount = 0f; // 1 pixel sampling

                    if (idx0 == idx1 - 1) // 2 pixels sampling
                    {
                        var lw = (float)idx1 - leftSide;
                        var rw = rightSide - (float)idx1;

                        amount = rw / (lw + rw);
                    }
                    else if (idx0 == idx1 - 2) // 3 pixels sampling
                    {
                        var lw = leftSide - (float)(idx0 + 1);
                        var rw = rightSide - (float)idx1;

                        // at least 1 pixel is fully covered, we need to pick the other pixels that has more coverage

                        if (lw > rw)
                        {
                            idx1 = idx0 + 1;
                            amount = 1 / (lw + 1);
                        }
                        else
                        {
                            idx0 = idx1 - 1;
                            amount = rw / (1 + rw);
                        }
                    }
                    else if (idx0 < idx1) // 4 or more pixels sampling
                    {
                        System.Diagnostics.Debug.Assert((idx1 - idx0) >= 3);

                        // at least 2 pixels are fully covered, since we can only sample 2 pixels, we'll pick these two

                        idx0 += 1;
                        idx1 -= 1;
                        amount = 0.5f;
                    }

                    System.Diagnostics.Debug.Assert(amount >= 0 && amount <= 1);

                    dstPairs[i] = new _BilinearSampleSource(idx0, idx1, amount);
                }
            }

            private static void InitializeExpand(int srcLen, Span<_BilinearSampleSource> dstPairs)
            {
                var ratio = (float)srcLen / (float)dstPairs.Length;

                for (int i = 0; i < dstPairs.Length; ++i)
                {
                    float samplePoint = ((float)i + 0.5f) * ratio;


                    var idx0 = (int)samplePoint;
                    var idx1 = idx0;

                    float amount = (samplePoint - idx0);

                    if (amount < 0.5f)
                    {
                        idx0 -= 1; if (idx0 < 0) idx0 = 0;
                        amount += 0.5f;
                    }
                    else
                    {
                        idx1 += 1; if (idx1 >= srcLen) idx1 = srcLen - 1;
                        amount -= 0.5f;
                    }


                    dstPairs[i] = new _BilinearSampleSource(idx0, idx1, amount);
                }
            }

            #endregion

            #region constructor

            private _BilinearSampleSource(int left, int right, float amount)
            {
                IndexLeft = left;
                IndexRight = right;
                Amount = amount;
            }

            #endregion

            #region data

            public readonly int IndexLeft; // index of the first pixel in source
            public readonly int IndexRight; // index of the second pixel in source
            public readonly float Amount;  // LERP amount between first and second source pixels.

            #endregion

            #region API

            public static void ApplyTo(Span<Single> dst, ReadOnlySpan<Single> src, ReadOnlySpan<_BilinearSampleSource> dstPairs)
            {
                System.Diagnostics.Debug.Assert(dst.Length == dstPairs.Length);

                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = dstPairs[i].GetSample(src);
                }
            }

            public static void ApplyTo(Span<Vector2> dst, ReadOnlySpan<Vector2> src, ReadOnlySpan<_BilinearSampleSource> dstPairs)
            {
                System.Diagnostics.Debug.Assert(dst.Length == dstPairs.Length);

                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = dstPairs[i].GetSample(src);
                }
            }

            public static void ApplyTo(Span<Vector3> dst, ReadOnlySpan<Vector3> src, ReadOnlySpan<_BilinearSampleSource> dstPairs)
            {
                System.Diagnostics.Debug.Assert(dst.Length == dstPairs.Length);

                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = dstPairs[i].GetSample(src);
                }
            }

            public static void ApplyTo(Span<Vector4> dst, ReadOnlySpan<Vector4> src, ReadOnlySpan<_BilinearSampleSource> dstPairs)
            {
                System.Diagnostics.Debug.Assert(dst.Length == dstPairs.Length);

                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = dstPairs[i].GetSample(src);
                }
            }

            public Single GetSample(ReadOnlySpan<Single> src)
            {
                return src[IndexLeft] * (1 - Amount) + src[IndexRight] * Amount;
            }

            public Vector2 GetSample(ReadOnlySpan<Vector2> src)
            {
                return Vector2.Lerp(src[IndexLeft], src[IndexRight], Amount);
            }

            public Vector3 GetSample(ReadOnlySpan<Vector3> src)
            {
                return Vector3.Lerp(src[IndexLeft], src[IndexRight], Amount);
            }

            public Vector4 GetSample(ReadOnlySpan<Vector4> src)
            {
                return Vector4.Lerp(src[IndexLeft], src[IndexRight], Amount);
            }

            #endregion
        }

        #endregion
    }
}
