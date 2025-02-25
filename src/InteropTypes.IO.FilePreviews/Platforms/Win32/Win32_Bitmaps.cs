using System;
using System.IO;
using System.Runtime.Versioning;

using InteropTypes.Graphics;

namespace System.Drawing.Imaging
{
    [SupportedOSPlatform("windows")]
    internal static class DrawingExtensions
    {
        public static ArraySegment<byte> SavePngBytes(this Bitmap bitmap)
        {
            if (bitmap == null) return default;

            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.TryGetBuffer(out var buff)
                    ? buff
                    : memoryStream.ToArray();
            }
        }

        public static unsafe WindowsBitmap GetBitmap(this Bitmap bitmap, PixelFormat format)
        {
            var fmt = format;
            if (fmt == PixelFormat.Undefined) fmt = bitmap.PixelFormat;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, fmt);

            var dstData = GetBitmap(bmpData);

            bitmap.UnlockBits(bmpData);

            return dstData;
        }

        private static unsafe WindowsBitmap GetBitmap(BitmapData bmpData)
        {
            var bpp = Image.GetPixelFormatSize(bmpData.PixelFormat);
            var dstBitmap = new WindowsBitmap(bmpData.Width, bmpData.Height, bpp / 8);

            for (int y = 0; y < bmpData.Height; y++)
            {
                var srcRow = GetRow(bmpData, y);
                var dstRow = dstBitmap.GetRow(y);
                var len = Math.Min(srcRow.Length, dstRow.Count);
                srcRow.Slice(0, len).CopyTo(dstRow);
            }

            return dstBitmap;
        }

        private static unsafe Span<Byte> GetRow(BitmapData bmpData, int rowIdx)
        {
            var srcPtr = bmpData.Scan0;
            srcPtr += bmpData.Stride * rowIdx;
            return new Span<byte>(srcPtr.ToPointer(), Math.Abs(bmpData.Stride));
        }

        /// <summary>
        /// Gets the span of the data
        /// </summary>
        /// <param name="bmpData"></param>
        /// <returns></returns>
        private static unsafe Span<Byte> GetSpan(BitmapData bmpData)
        {
            // var bpp = Image.GetPixelFormatSize(bmpData.PixelFormat);            

            if (bmpData.Stride > 0) // top-bottom
            {
                var length = bmpData.Stride * bmpData.Height;
                return new Span<byte>(bmpData.Scan0.ToPointer(), length);
            }
            else // bottom-top
            {
                var srcPtr = bmpData.Scan0;
                srcPtr += bmpData.Stride * (bmpData.Height - 1); // move ptr to the beginning
                var length = -bmpData.Stride * bmpData.Height;
                return new Span<byte>(srcPtr.ToPointer(), length);
            }
        }


    }
}