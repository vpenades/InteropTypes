using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("OpenCvSharp Codec")]
    public sealed class OpenCvCodec : IBitmapDecoding, IBitmapEncoding
    {
        #region lifecycle

        static OpenCvCodec() { }

        private OpenCvCodec() { }

        private static readonly OpenCvCodec _Default = new OpenCvCodec();

        public static OpenCvCodec Default => _Default;

        #endregion

        public MemoryBitmap Read(Stream s)
        {
            using(var mat = OpenCvSharp.Mat.FromStream(s, OpenCvSharp.ImreadModes.AnyColor))
            {
                return mat.AsSpanBitmap().ToMemoryBitmap();
            }
        }

        public void Write(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            using (var mat = bmp.AsOpenCVSharp().ToMat())
            {
                mat.WriteToStream(s, $".{format.ToString().ToLower()}");
            }                
        }
    }
}
