using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public sealed class ImageSharpCodec : IBitmapDecoding, IBitmapEncoding
    {
        #region API

        public MemoryBitmap Read(Stream s)
        {
            using (var img = SixLabors.ImageSharp.Image.Load(s))
            {
                return img.AsSpanBitmap().ToMemoryBitmap();
            }
        }

        public void Write(Stream s, string formatExtension, SpanBitmap bmp)
        {
            var fmt = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindFormatByFileExtension(formatExtension);

            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(fmt);

            using (var img = _Implementation.ToImageSharp(bmp))
            {
                img.Save(s, encoder);
            }
        }

        #endregion
    }
}
