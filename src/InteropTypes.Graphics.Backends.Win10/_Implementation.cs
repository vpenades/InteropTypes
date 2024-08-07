﻿using System;

using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    // https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/imaging

    [ComImport]
    [Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    unsafe interface IMemoryBufferByteAccess
    {
        void GetBuffer(out byte* buffer, out uint capacity);
    }

    static class _Implementation
    {
        public static (BitmapPixelFormat, BitmapAlphaMode) ToWinSdkFormat<TPixel>()
            where TPixel:unmanaged
        {
            if (typeof(TPixel) == typeof(Bitmaps.Pixel.Luminance8)) return (BitmapPixelFormat.Gray8, BitmapAlphaMode.Ignore);
            if (typeof(TPixel) == typeof(Bitmaps.Pixel.Luminance16)) return (BitmapPixelFormat.Gray16, BitmapAlphaMode.Ignore);
            if (typeof(TPixel) == typeof(Bitmaps.Pixel.BGRA32)) return (BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight);
            if (typeof(TPixel) == typeof(Bitmaps.Pixel.ARGB32)) return (BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight);

            return (BitmapPixelFormat.Unknown, BitmapAlphaMode.Ignore);
        }

        public static SoftwareBitmap CreateSoftwareBitmap(this SpanBitmap src, BitmapPixelFormat colorFmt, BitmapAlphaMode alphaFmt)
        {
            if (src.IsEmpty || src.Width * src.Height == 0) return null;

            var dst = new SoftwareBitmap(colorFmt, src.Width, src.Height, alphaFmt);

            dst.SetPixels(src);

            return dst;
        }


        public static unsafe void SetPixels(this SoftwareBitmap dst, Bitmaps.SpanBitmap src)
        {
            using (var buffer = dst.LockBuffer(BitmapBufferAccessMode.Write))
            {
                using (var reference = buffer.CreateReference())
                {
                    var memoryBuffer = reference as IMemoryBufferByteAccess;
                    if (memoryBuffer == null) return;

                    byte* dataInBytes;
                    uint capacity;
                    memoryBuffer.GetBuffer(out dataInBytes, out capacity);

                    var bufferLayout = buffer.GetPlaneDescription(0);

                    throw new NotImplementedException();

                    if (bufferLayout.Stride == bufferLayout.Width * 2)
                    {
                        // src.CopyTo(dataInBytes, (int)capacity, bufferLayout.Width * bufferLayout.Height * 2);

                        return;
                    }

                    for (int y = 0; y < bufferLayout.Height; y++)
                    {
                        for (int x = 0; x < bufferLayout.Width; x++)
                        {
                            // var value = src[x, y];

                            // var didx = bufferLayout.StartIndex + bufferLayout.Stride * y + 2 * x;

                            // dataInBytes[didx + 0] = (Byte)(value & 255);
                            // dataInBytes[didx + 1] = (Byte)(value >> 8);
                        }
                    }
                }
            }
        }
    }
}