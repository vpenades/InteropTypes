using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Codecs
{
    public sealed class WPFCodec : IBitmapDecoding, IBitmapEncoding
    {
        public MemoryBitmap Read(Stream s)
        {
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(s, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.None);
             
            return _Implementation.ToMemoryBitmap(frame);
        }

        public void Write(Stream s, string formatExtension, SpanBitmap bmp)
        {
            var encoder = CreateEncoder(formatExtension);            

            var tmp = _Implementation.ToWritableBitmap(bmp);

            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(tmp);

            encoder.Frames.Add(frame);

            encoder.Save(s);
        }

        private static System.Windows.Media.Imaging.BitmapEncoder CreateEncoder(string fileExt)
        {
            if (fileExt.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.BmpBitmapEncoder();
            if (fileExt.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.JpegBitmapEncoder();            
            if (fileExt.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.PngBitmapEncoder();
            if (fileExt.EndsWith("wmp", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.WmpBitmapEncoder();
            if (fileExt.EndsWith("tif", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.TiffBitmapEncoder();            
            if (fileExt.EndsWith("gif", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.GifBitmapEncoder();

            if (fileExt.EndsWith("tiff", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.TiffBitmapEncoder();
            if (fileExt.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.JpegBitmapEncoder();

            throw new NotImplementedException();
        }
    }
}
