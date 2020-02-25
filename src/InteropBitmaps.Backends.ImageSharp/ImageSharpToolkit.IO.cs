using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    partial class ImageSharpToolkit
    {
        public static MemoryBitmap<Rgba32> LoadMemoryBitmapFromImageSharp(this System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return s.ReadMemoryBitmapFromImageSharp();
            }
        }

        public static MemoryBitmap<Rgba32> ReadMemoryBitmapFromImageSharp(this System.IO.Stream s)
        {
            using (var img = Image.Load<Rgba32>(s))
            {
                return img.CopyToMemoryBitmap();
            }
        }
    }
}
