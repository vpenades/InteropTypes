using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    /// <summary>
    /// Code to flip an image horizontally, vertically, or both
    /// </summary>
    public struct BitmapMirror : SpanBitmap.IEffect
    {
        #region constructor

        public BitmapMirror(bool h, bool v, bool useMultiThreading = true)
        {
            Horizontal = h;
            Vertical = v;
            UseMultiThreading = useMultiThreading;
        }

        #endregion

        #region properties

        public bool Horizontal { get; set; }
        public bool Vertical { get; set; }
        public bool UseMultiThreading { get; set; }

        #endregion

        #region API

        public readonly bool TryApplyTo(SpanBitmap dst)
        {
            switch (dst.PixelFormat.ByteCount)
            {
                case 1: return TryApplyTo(dst.OfType<Byte>());
                case 2: return TryApplyTo(dst.OfType<UInt16>());
                case 3: return TryApplyTo(dst.OfType<Pixel.Undefined24>());
                case 4: return TryApplyTo(dst.OfType<UInt32>());
                case 8: return TryApplyTo(dst.OfType<UInt64>());
                case 12: return TryApplyTo(dst.OfType<System.Numerics.Vector3>());
                case 16: return TryApplyTo(dst.OfType<System.Numerics.Vector4>());
            }

            return false;
        }            

        public readonly bool TryApplyTo<TPixel>(SpanBitmap<TPixel> dst)
            where TPixel : unmanaged
        {
            // OpenCV implementation here:
            // https://github.com/opencv/opencv/blob/2155296a135ba6819b35a1eb2e48a11e71df7363/modules/core/src/copy.cpp#L598

            if (Vertical)
            {
                var rl = dst.Info.Width * dst.Info.PixelByteSize;

                Span<Byte> tmp = stackalloc byte[rl];

                var mid = dst.Height / 2;

                for (int i = 0; i < mid; ++i)
                {
                    var top = dst.UseScanlineBytes(i);
                    var down = dst.UseScanlineBytes(dst.Height - 1 - i);

                    // swap rows
                    top.CopyTo(tmp);
                    down.CopyTo(top);
                    tmp.CopyTo(down);
                }
            }

            if (Horizontal)
            {
                if (!UseMultiThreading) _HorizontalFlip(dst, 0, dst.Height);
                else dst.PinWritablePointer(ptr => _HorizontalFlip<TPixel>(ptr, true));
            }

            return true;
        }

        private static void _HorizontalFlip<TPixel>(SpanBitmap<TPixel> dst, int y0, int y1)
            where TPixel : unmanaged
        {
            for (int i = y0; i < y1; ++i)
            {
                // Reverse code is a plain loop
                // https://github.com/dotnet/corert/blob/c6af4cfc8b625851b91823d9be746c4f7abdc667/src/System.Private.CoreLib/shared/System/MemoryExtensions.cs#L1138

                dst.UseScanlinePixels(i).Reverse();
            }
        }

        private static void _HorizontalFlip<TPixel>(PointerBitmap ptrBmp, bool useMultiThreading = true)
            where TPixel : unmanaged
        {
            if (!useMultiThreading)
            {
                var dstBmp = ptrBmp.AsSpanBitmapOfType<TPixel>();
                _HorizontalFlip(dstBmp, 0, ptrBmp.Height);
                return;
            }

            const int threads = 4;

            void _hflip(int idx)
            {
                var bmp = ptrBmp.AsSpanBitmapOfType<TPixel>();
                int y0 = bmp.Height * (idx + 0) / threads;
                int y1 = bmp.Height * (idx + 1) / threads;
                _HorizontalFlip(bmp, y0, y1);
            }

            System.Threading.Tasks.Parallel.For(0, threads, _hflip);
        }

        #endregion
    }
    
}
