using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using MEMORYMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes
{
    static partial class _Implementation
    {
        #region API - Dangerous             

        public static unsafe bool TryWrapAsSpanBitmap<TSrcPixel,TDstPixel>(Image<TSrcPixel> src, SpanBitmap<TDstPixel>.Action1 action)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
            where TDstPixel : unmanaged
        {
            if (src == null) throw new ArgumentNullException(nameof(src));

            if (TryGetExactPixelFormat<TSrcPixel>(out var pfmt))
            {
                if (src.DangerousTryGetSinglePixelMemory(out Memory<TSrcPixel> srcSpan))
                {                    
                    var span = MEMORYMARSHALL.Cast<TSrcPixel, Byte>(srcSpan.Span);

                    var bmp = new SpanBitmap<TDstPixel>(span, src.Width, src.Height, pfmt);

                    action(bmp);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region API - Pixel Format and Bitmap Info

        public static BitmapInfo GetBitmapInfo(Image image)
        {
            if (TryGetBitmapInfo(image, out var binfo)) return binfo;
            throw new Diagnostics.PixelFormatNotSupportedException(image.GetType().Name, nameof(image));
        }

        public static bool TryGetBitmapInfo(Image image, out BitmapInfo binfo)
        {
            if (TryGetExactPixelFormat(image, out var fmt))
            {
                binfo = new BitmapInfo(image.Width, image.Height, fmt);
                return true;
            }

            binfo = default;
            return false;
        }

        public static BitmapInfo GetBitmapInfo<TPixel>(Image<TPixel> image)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (TryGetBitmapInfo(image, out var binfo)) return binfo;
            throw new Diagnostics.PixelFormatNotSupportedException(typeof(TPixel).Name, nameof(image));
        }

        public static bool TryGetBitmapInfo<TPixel>(Image<TPixel> image, out BitmapInfo binfo)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (TryGetExactPixelFormat<TPixel>(out var fmt))
            {
                binfo = new BitmapInfo(image.Width, image.Height, fmt);
                return true;
            }

            binfo = default;
            return false;
        }

        #endregion

        #region Wrap & Clone

        public static Image<TPixel> WrapAsImageSharp<TPixel>(this MemoryBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (src.IsEmpty) throw new ArgumentNullException(nameof(src));

            // ImageSharp does not support non-continuous pixel rows.
            if (!src.Info.IsContinuous) throw new ArgumentException("source images with extra row stride are not sopported.", nameof(src));

            TryGetExactPixelType(src.PixelFormat, out var pixType);
            if (pixType != typeof(TPixel)) throw new Diagnostics.PixelFormatNotSupportedException(src.PixelFormat, nameof(src));

            var memMngr = new Graphics.Adapters.CastMemoryManager<Byte, TPixel>(src.Memory);

            return Image.WrapMemory(memMngr, src.Width, src.Height);
        }

        public static Image CloneToImageSharp(SpanBitmap src)
        {
            var dst = CreateImageSharp(src.PixelFormat, src.Width, src.Height);
            CopyPixels(src, dst);
            return dst;
        }

        public static Image<TPixel> CloneToImageSharp<TPixel>(SpanBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            TryGetExactPixelType(src.PixelFormat, out var pixType);
            System.Diagnostics.Debug.Assert(pixType == typeof(TPixel));

            return CloneToImageSharp(src.OfType<TPixel>());
        }

        public static Image<TPixel> CloneToImageSharp<TPixel>(SpanBitmap<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);
            CopyPixels(src, dst);
            return dst;
        }
        
        public static void Mutate(SpanBitmap src, Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImageSharp(src))
            {
                tmp.Mutate(operation);

                // if size has changed, throw error.
                if (tmp.Width != src.Width || tmp.Height != src.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(operation));
                
                CopyPixels(tmp, src);
            }
        }

        #endregion

        #region Read & Write bitmaps        

        public static TResult ReadAsSpanBitmap<TSelfPixel, TOtherPixel, TResult>(Image<TSelfPixel> self, SpanBitmap<TOtherPixel> other, SpanBitmap<TOtherPixel>.Function2<TResult> function)
            where TSelfPixel : unmanaged, IPixel<TSelfPixel>
            where TOtherPixel: unmanaged
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            if (!TryGetExactPixelFormat<TSelfPixel>(out var otherFmt)) throw new NotImplementedException($"{typeof(TSelfPixel)}");

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TSelfPixel> selfMem))
            {
                var selfBytes = MEMORYMARSHALL.Cast<TSelfPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap<TOtherPixel>(selfBytes, self.Width, self.Height, otherFmt);

                return function(selfBmp.AsReadOnly(), other);
            }
            else
            {
                var tempBmp = ImageSharpToolkit
                    .ToMemoryBitmap<TOtherPixel>(self)
                    .AsSpanBitmap()
                    .AsReadOnly();

                return function(tempBmp, other);
            }
        }        

        public static unsafe void WriteAsSpanBitmap<TSelfPixel, TOtherPixel>(Image<TSelfPixel> self, SpanBitmap<TOtherPixel> other, SpanBitmap<TOtherPixel>.Action2 action)
            where TSelfPixel : unmanaged, IPixel<TSelfPixel>
            where TOtherPixel : unmanaged
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            other = other.AsReadOnly();

            if (!TryGetExactPixelFormat<TSelfPixel>(out var otherFmt))
            {
                otherFmt = PixelFormat.TryIdentifyFormat<TOtherPixel>();
            }

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TSelfPixel> selfMem))
            {
                var selfBytes = MEMORYMARSHALL.Cast<TSelfPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap<TOtherPixel>(selfBytes, self.Width, self.Height, otherFmt);
                action(selfBmp, other);
            }
            else
            {
                var tempBmp = new MemoryBitmap<TOtherPixel>(self.Width, self.Height, otherFmt);

                CopyPixels(self, tempBmp);
                action(tempBmp, other);
                CopyPixels(tempBmp, self);
            }
        }

        public static void MutateAsImageSharp<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> self, Action<IImageProcessingContext> operation)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged, IPixel<TDstPixel>
        {
            _ImageSharpChangedMonitor.WriteAsImageSharp<TSrcPixel, TDstPixel>(self, img => ProcessingExtensions.Mutate(img, operation));
        }

        #endregion

        #region copy pixels

        public static unsafe void CopyPixels<TSrcPixel,TDstPixel>(Image<TSrcPixel> src, SpanBitmap<TDstPixel> dst)
            where TSrcPixel: unmanaged, IPixel<TSrcPixel>
            where TDstPixel : unmanaged
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (src.Width != dst.Width || src.Height != dst.Height) throw new ArgumentException("dimensions mismatch", nameof(dst));
            if (sizeof(TSrcPixel) != sizeof(TDstPixel)) throw new ArgumentException("Pixel size mismatch", typeof(TDstPixel).Name);

            for (int y = 0; y < dst.Height; y++)
            {
                var srcRow = src.DangerousGetPixelRowMemory(y).Span;
                var dstRow = dst.UseScanlinePixels(y);

                MEMORYMARSHALL
                    .Cast<TSrcPixel, TDstPixel>(srcRow)
                    .CopyTo(dstRow);
            }
        }

        public static unsafe void CopyPixels<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, Image<TDstPixel> dst)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged, IPixel<TDstPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            if (src.Width != dst.Width || src.Height != dst.Height) throw new ArgumentException("dimensions mismatch", nameof(dst));
            if (sizeof(TSrcPixel) != sizeof(TDstPixel)) throw new ArgumentException("Pixel size mismatch", typeof(TDstPixel).Name);

            src = src.AsReadOnly();

            for (int y = 0; y < dst.Height; y++)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span;

                MEMORYMARSHALL
                    .Cast<TSrcPixel, TDstPixel>(srcRow)
                    .CopyTo(dstRow);
            }
        }

        public static void CopyPixels<TSrcPixel>(Image<TSrcPixel> src, SpanBitmap dst)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>            
        {
            for (int y = 0; y < dst.Height; y++)
            {
                var srcRow = src.DangerousGetPixelRowMemory(y).Span;
                var dstRow = dst.UseScanlineBytes(y);

                MEMORYMARSHALL
                    .Cast<TSrcPixel, Byte>(srcRow)
                    .CopyTo(dstRow);
            }
        }

        public static void CopyPixels<TDstPixel>(SpanBitmap src, Image<TDstPixel> dst)
            where TDstPixel : unmanaged, IPixel<TDstPixel>
        {
            src = src.AsReadOnly();

            for (int y = 0; y < dst.Height; y++)
            {
                var srcRow = src.GetScanlineBytes(y);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span;

                MEMORYMARSHALL
                    .Cast<Byte, TDstPixel>(srcRow)
                    .CopyTo(dstRow);
            }
        }

        #endregion       
    }    
}
