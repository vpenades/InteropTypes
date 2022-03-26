using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    partial struct SpanBitmap
    {        
        public static void PinTransferPointers(SpanBitmap src, SpanBitmap dst, PointerBitmap.Action2 onPinned)
        {
            _SpanBitmapImpl.PinTransferPointers(src, dst, onPinned);
        }

        public static (Single Min, Single Max) MinMax(SpanBitmap<float> src)
        {
            var min = float.PositiveInfinity;
            var max = float.NegativeInfinity;            

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);
                
                var (rMin, rMax) = Vector4Streaming.MinMax(srcRow);

                if (min > rMin) min = rMin;
                if (max < rMax) max = rMax;
            }

            return (min, max);
        }

        public static (Vector3 Min, Vector3 Max) MinMax(SpanBitmap<Vector3> src)
        {
            var min = new Vector3(float.PositiveInfinity);
            var max = new Vector3(float.NegativeInfinity);

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);

                for(int x=0; x < srcRow.Length; ++x)
                {
                    var p = srcRow[x];
                    min = Vector3.Min(min, p);
                    max = Vector3.Max(max, p);
                }
            }

            return (min, max);
        }

        public static (Vector4 Min, Vector4 Max) MinMax(SpanBitmap<Vector4> src)
        {
            var min = new Vector4(float.PositiveInfinity);
            var max = new Vector4(float.NegativeInfinity);

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);

                for (int x = 0; x < srcRow.Length; ++x)
                {
                    var p = srcRow[x];
                    min = Vector4.Min(min, p);
                    max = Vector4.Max(max, p);
                }
            }

            return (min, max);
        }

        public static void CopyPixels(SpanBitmap dst, SpanBitmap src) // Copy
        {
            Guard.AreEqual(nameof(src), dst.Width, src.Width);
            Guard.AreEqual(nameof(src), dst.Height, src.Height);

            src = src.AsReadOnly();

            var rowConverter = Pixel.GetByteCopyConverter(src.PixelFormat, dst.PixelFormat);

            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = dst.UseScanlineBytes(y);
                var srcRow = src.UseScanlineBytes(y);
                rowConverter(srcRow, dstRow);
            }
        }

        public static void CopyPixels(SpanBitmap<Byte> src, SpanBitmap<Single> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            src = src.AsReadOnly();

            System.Diagnostics.Debug.Assert(dst.Width == src.Width);
            System.Diagnostics.Debug.Assert(dst.Height == src.Height);            

            for(int y=0; y < dst.Height; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRow = dst.UseScanlinePixels(y);
                _SpanSingleExtensions.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap<Single> src, SpanBitmap<Byte> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            src = src.AsReadOnly();

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRow = dst.UseScanlinePixels(y);
                _SpanSingleExtensions.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap src, SpanBitmap<Vector3> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            src = src.AsReadOnly();

            if (src.PixelByteSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                for (int y = 0; y < dst.Height; ++y)
                {
                    var srcRow = src.GetScanlineBytes(y);
                    var dstRow = dst.UseScanlinePixels(y);
                    
                    var dstFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(dstRow);

                    _SpanSingleExtensions.CopyPixels(srcRow, dstFFF, transform, range);
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void CopyPixels(SpanBitmap<Vector3> src, SpanBitmap dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            src = src.AsReadOnly();

            if (dst.PixelByteSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                if (range.min == 0 && range.max == 255)
                {
                    for (int y = 0; y < dst.Height; ++y)
                    {
                        var srcRow = src.GetScanlinePixels(y);
                        var dstRow = dst.UseScanlineBytes(y);

                        var srcFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(srcRow);

                        _SpanSingleExtensions.CopyPixels(srcFFF, dstRow, transform);
                    }
                }
                else
                {
                    for (int y = 0; y < dst.Height; ++y)
                    {
                        var srcRow = src.GetScanlinePixels(y);
                        var dstRow = dst.UseScanlineBytes(y);

                        var srcFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(srcRow);

                        _SpanSingleExtensions.CopyPixels(srcFFF, dstRow, transform, range);
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void SplitPixels(SpanBitmap<Vector3> src, SpanBitmap<Single> dstB, SpanBitmap<Single> dstG, SpanBitmap<Single> dstR)
        {
            src = src.AsReadOnly();

            if (src.PixelFormat.Component0.IsBlue)
            {
                for (int y = 0; y < src.Height; ++y)
                {
                    var srcRow = src.GetScanlinePixels(y);
                    var dstRowX = dstB.UseScanlinePixels(y);
                    var dstRowY = dstG.UseScanlinePixels(y);
                    var dstRowZ = dstR.UseScanlinePixels(y);

                    for (int x = 0; x < srcRow.Length; ++x)
                    {
                        var bgr = srcRow[x];
                        dstRowX[x] = bgr.X;
                        dstRowY[x] = bgr.Y;
                        dstRowZ[x] = bgr.Z;
                    }
                }
            }

            if (src.PixelFormat.Component0.IsRed)
            {
                for (int y = 0; y < src.Height; ++y)
                {
                    var srcRow = src.GetScanlinePixels(y);
                    var dstRowX = dstR.UseScanlinePixels(y);
                    var dstRowY = dstG.UseScanlinePixels(y);
                    var dstRowZ = dstB.UseScanlinePixels(y);

                    for (int x = 0; x < srcRow.Length; ++x)
                    {
                        var bgr = srcRow[x];
                        dstRowX[x] = bgr.X;
                        dstRowY[x] = bgr.Y;
                        dstRowZ[x] = bgr.Z;
                    }
                }
            }
        }

        

        public static bool ArePixelsEqual(SpanBitmap a, SpanBitmap b)
        {
            if (a.Info.Bounds != b.Info.Bounds) return false;
            if (a.Info.PixelFormat != b.Info.PixelFormat) return false;

            if (a.Info.PixelFormat.GetDepthType() == typeof(float))
            {
                Guard.AreEqual(nameof(a.Info.PixelFormat), a.PixelByteSize & 3, 0);

                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseScanlineBytes(y);
                    var bRow = b.UseScanlineBytes(y);
                    var aFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(aRow);
                    var bFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(bRow);
                    if (!Vector4Streaming.SequenceEqual(aFlt, bFlt)) return false;
                }

                return true;
            }
            else
            {
                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseScanlineBytes(y);
                    var bRow = b.UseScanlineBytes(y);

                    if (!aRow.SequenceEqual(bRow)) return false;
                }

                return true;
            }


            throw new NotImplementedException();
        }

        public static void ApplyAddMultiply(SpanBitmap<Single> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                Vector4Streaming.AddMultiply(row, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Single> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                Vector4Streaming.MultiplyAdd(row, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector3> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                Vector4Streaming.AddMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector3> target, Vector3 add, Vector3 multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);                
                for(int i=0; i < row.Length; ++i)
                {
                    row[i] += add;
                    row[i] *= multiply;
                }
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector3> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                Vector4Streaming.MultiplyAdd(fRow, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector4> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, Single>(row);
                Vector4Streaming.AddMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector4> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlinePixels(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, float>(row);
                Vector4Streaming.MultiplyAdd(fRow, multiply, add);
            }
        }        
    }
}
