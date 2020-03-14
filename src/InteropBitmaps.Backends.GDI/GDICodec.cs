using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("GDI Codec")]
    public sealed class GDICodec : IBitmapDecoding, IBitmapEncoding
    {
        #region lifecycle

        static GDICodec() { }

        private GDICodec() { }

        private static readonly GDICodec _Default = new GDICodec();

        public static GDICodec Default => _Default;

        #endregion

        public MemoryBitmap Read(Stream s)
        {
            using (var img = System.Drawing.Image.FromStream(s))
            {
                return img.ToMemoryBitmap();
            }
        }

        public void Write(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = GetFormatFromExtension(format);

            using (var tmp = _Implementation.CloneToGDI(bmp, true))
            {
                tmp.Save(s, fmt);
            }
        }

        private static System.Drawing.Imaging.ImageFormat GetFormatFromExtension(CodecFormat format)
        {
            switch(format)
            {
                case CodecFormat.Png: return System.Drawing.Imaging.ImageFormat.Png;
                case CodecFormat.Jpeg: return System.Drawing.Imaging.ImageFormat.Jpeg;

                case CodecFormat.Tiff: return System.Drawing.Imaging.ImageFormat.Tiff;
                case CodecFormat.Icon: return System.Drawing.Imaging.ImageFormat.Icon;

                case CodecFormat.Gif: return System.Drawing.Imaging.ImageFormat.Gif;
                case CodecFormat.Bmp: return System.Drawing.Imaging.ImageFormat.Bmp;
                case CodecFormat.Emf: return System.Drawing.Imaging.ImageFormat.Emf;
                case CodecFormat.Wmf: return System.Drawing.Imaging.ImageFormat.Wmf;
                default: throw new CodecException();
            }            
        }
    }
}
