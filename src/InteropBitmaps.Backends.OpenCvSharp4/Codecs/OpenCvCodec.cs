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

        public bool TryRead(Stream s, out MemoryBitmap bitmap)
        {
            bitmap = null;

            using(var mat = OpenCvSharp.Mat.FromStream(s, OpenCvSharp.ImreadModes.AnyColor))
            {
                if (mat.Cols == 0) return false;
                bitmap = mat.AsSpanBitmap().ToMemoryBitmap();                
            }

            return true;
        }

        public bool TryWrite(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            using (var mat = bmp.WithOpenCv().ToMat())
            {
                mat.WriteToStream(s, $".{format.ToString().ToLower()}");
            }

            return true;
        }
    }
}
