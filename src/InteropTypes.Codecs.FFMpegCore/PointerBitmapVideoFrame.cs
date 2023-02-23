﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

using FFMpegCore.Pipes;

using PIX = InteropTypes.Graphics.Bitmaps.Pixel;
using PIXFMT = InteropTypes.Graphics.Bitmaps.PixelFormat;

namespace InteropTypes.Graphics.Bitmaps
{
    class PointerBitmapVideoFrame : IVideoFrame, IDisposable
    {
        #region static API

        public static IEnumerable<PointerBitmapVideoFrame> WrapFrames(IEnumerable<PointerBitmap> frames)
        {
            return frames.Select(item => new PointerBitmapVideoFrame(item));
        }        

        #endregion

        #region lifecycle

        public PointerBitmapVideoFrame(PointerBitmap bitmap)
        {
            Source = bitmap;
            Format = ConvertStreamFormat(bitmap.PixelFormat);
        }

        public void Dispose() { }

        #endregion

        #region properties

        public PointerBitmap Source { get; private set; }

        public int Width => Source.Width;

        public int Height => Source.Height;

        public string Format { get; private set; }

        #endregion

        #region API

        public void Serialize(Stream stream)
        {
            for(int i=0; i < Source.Height; ++i)
            {
                var row = Source.GetScanlineBytes(i);

                #if NETSTANDARD2_0
                stream.Write(row.ToArray(), 0, row.Length);
                #else
                stream.Write(row);
                #endif
            }
        }

        public async Task SerializeAsync(Stream stream, CancellationToken token)
        {
            for (int i = 0; i < Source.Height; ++i)
            {
                var xrow = Source.GetScanlineBytes(i).ToArray();

                #if NETSTANDARD2_0                
                await stream.WriteAsync(xrow, 0, xrow.Length, token).ConfigureAwait(continueOnCapturedContext: false);
                #else
                await stream.WriteAsync(xrow, token).ConfigureAwait(continueOnCapturedContext: false);
                #endif         
            }
        }       

        private static string ConvertStreamFormat(PIXFMT fmt)
        {
            return fmt.Code switch
            {
                PIX.Luminance16.Code => "gray16le",
                // PixelFormat.Format16bppRgb555 => "bgr555le",
                PIX.BGR565.Code => "bgr565le",
                PIX.BGR24.Code => "bgr24",
                PIX.BGRA32.Code => "bgra",
                PIX.ARGB32.Code => "argb",
                PIX.RGBA32.Code => "rgba",
                // PixelFormat.Format48bppRgb => "rgb48le",
                _ => throw new NotSupportedException($"Not supported pixel format {fmt}"),
            };
        }

        #endregion
    }
}
