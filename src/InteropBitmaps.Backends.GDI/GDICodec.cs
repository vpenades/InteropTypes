using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public sealed class GDICodec : IBitmapDecoding, IBitmapEncoding
    {
        public MemoryBitmap Read(Stream s)
        {
            using (var img = System.Drawing.Image.FromStream(s))
            {
                return img.ToMemoryBitmap();
            }
        }

        public void Write(Stream s, string formatExtension, SpanBitmap bmp)
        {
            var fmt = GetFormatFromExtension(formatExtension);

            using (var tmp = _Implementation.CloneToGDI(bmp, true))
            {
                tmp.Save(s, fmt);
            }
        }

        private static System.Drawing.Imaging.ImageFormat GetFormatFromExtension(string extension)
        {
            if (extension.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Png;
            if (extension.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Jpeg;
            
            if (extension.EndsWith("tif", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Tiff;
            
            if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Gif;
            if (extension.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Bmp;
            if (extension.EndsWith("emf", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Emf;
            if (extension.EndsWith("ico", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Icon;
            if (extension.EndsWith("wmf", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Wmf;

            if (extension.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Jpeg;
            if (extension.EndsWith("tiff", StringComparison.OrdinalIgnoreCase)) return System.Drawing.Imaging.ImageFormat.Tiff;

            throw new NotSupportedException();
        }
    }
}
