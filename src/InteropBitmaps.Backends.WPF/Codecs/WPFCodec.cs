using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("WPF(WIC) Codec")]
    public sealed class WPFCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static WPFCodec() { }

        private WPFCodec() { }

        private static readonly WPFCodec _Default = new WPFCodec();

        public static WPFCodec Default => _Default;

        #endregion

        /// <inheritdoc/>
        public bool TryRead(Stream s, out MemoryBitmap bitmap)
        {
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(s, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.None);

            bitmap = _Implementation.ToMemoryBitmap(frame);

            return true;
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {            
            var encoder = CreateEncoder(format);
            if (encoder == null) return false;

            var writable = _Implementation.ToWritableBitmap(bmp);
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(writable);

            encoder.Frames.Add(frame);
            encoder.Save(stream.Value);

            return true;
        }

        private static System.Windows.Media.Imaging.BitmapEncoder CreateEncoder(CodecFormat format)
        {
            // TODO: System.Windows.Media.Imaging.WmpBitmapEncoder()

            switch (format)
            {
                case CodecFormat.Bmp: return new System.Windows.Media.Imaging.BmpBitmapEncoder();
                case CodecFormat.Jpeg: return new System.Windows.Media.Imaging.JpegBitmapEncoder();
                case CodecFormat.Png: return new System.Windows.Media.Imaging.PngBitmapEncoder();
                case CodecFormat.Gif: return new System.Windows.Media.Imaging.GifBitmapEncoder();
                case CodecFormat.Tiff: return new System.Windows.Media.Imaging.TiffBitmapEncoder();
                default: return null;
            }
        }
    }
}
