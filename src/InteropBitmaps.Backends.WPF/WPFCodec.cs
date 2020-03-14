using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("WPF(WIC) Codec")]
    public sealed class WPFCodec : IBitmapDecoding, IBitmapEncoding
    {
        #region lifecycle

        static WPFCodec() { }

        private WPFCodec() { }

        private static readonly WPFCodec _Default = new WPFCodec();

        public static WPFCodec Default => _Default;

        #endregion

        public MemoryBitmap Read(Stream s)
        {
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(s, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.None);
             
            return _Implementation.ToMemoryBitmap(frame);
        }

        public void Write(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            var encoder = CreateEncoder(format);            

            var tmp = _Implementation.ToWritableBitmap(bmp);

            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(tmp);

            encoder.Frames.Add(frame);

            encoder.Save(s);
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
                default: throw new CodecException();
            }
        }
    }
}
