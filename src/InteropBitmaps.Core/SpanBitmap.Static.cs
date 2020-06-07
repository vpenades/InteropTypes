using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    partial struct SpanBitmap
    {
        public static (float min, float max) MinMax(SpanBitmap<float> src)
        {
            var min = float.PositiveInfinity;
            var max = float.NegativeInfinity;            

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                
                var (rMin, rMax) = _SpanFloatOps.MinMax(srcRow);

                if (min > rMin) min = rMin;
                if (max < rMax) max = rMax;
            }

            return (min, max);
        }

        public static void CopyPixels(SpanBitmap<Byte> src, SpanBitmap<Single> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            System.Diagnostics.Debug.Assert(dst.Width == src.Width);
            System.Diagnostics.Debug.Assert(dst.Height == src.Height);

            for(int y=0; y < dst.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                var dstRow = dst.UsePixelsScanline(y);
                _SpanFloatOps.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap<Single> src, SpanBitmap<Byte> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                var dstRow = dst.UsePixelsScanline(y);
                _SpanFloatOps.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap src, SpanBitmap<Vector3> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            if (src.PixelSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                for (int y = 0; y < dst.Height; ++y)
                {
                    var srcRow = src.GetBytesScanline(y);
                    var dstRow = dst.UsePixelsScanline(y);
                    
                    var dstFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(dstRow);

                    _SpanFloatOps.CopyPixels(srcRow, dstFFF, transform, range);
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void CopyPixels(SpanBitmap<Vector3> src, SpanBitmap dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            if (dst.PixelSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                for (int y = 0; y < dst.Height; ++y)
                {
                    var srcRow = src.GetPixelsScanline(y);
                    var dstRow = dst.UseBytesScanline(y);

                    var srcFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(srcRow);                    

                    _SpanFloatOps.CopyPixels(srcFFF, dstRow, transform, range);
                }

                return;
            }

            throw new NotImplementedException();
        }          

        public static bool ArePixelsEqual(SpanBitmap a, SpanBitmap b)
        {
            if (a.Info.Bounds != b.Info.Bounds) return false;
            if (a.Info.PixelFormat != b.Info.PixelFormat) return false;

            if (a.Info.PixelFormat.GetDepthType() == typeof(float))
            {
                Guard.AreEqual(nameof(a.Info.PixelFormat), a.PixelSize & 3, 0);

                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseBytesScanline(y);
                    var bRow = b.UseBytesScanline(y);
                    var aFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(aRow);
                    var bFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(bRow);
                    if (!_SpanFloatOps.SequenceEqual(aFlt, bFlt)) return false;
                }

                return true;
            }
            else
            {
                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseBytesScanline(y);
                    var bRow = b.UseBytesScanline(y);

                    if (!aRow.SequenceEqual(bRow)) return false;
                }

                return true;
            }


            throw new NotImplementedException();
        }

        public static void ApplyAddMultiply(SpanBitmap<float> target, float add, float multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                _SpanFloatOps.AddAndMultiply(row, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<float> target, float multiply, float add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                _SpanFloatOps.MultiplyAndAdd(row, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector3> target, float add, float multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                _SpanFloatOps.AddAndMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector3> target, float multiply, float add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                _SpanFloatOps.MultiplyAndAdd(fRow, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector4> target, float add, float multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, float>(row);
                _SpanFloatOps.AddAndMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector4> target, float multiply, float add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, float>(row);
                _SpanFloatOps.MultiplyAndAdd(fRow, multiply, add);
            }
        }
    }
}
