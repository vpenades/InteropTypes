using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

namespace InteropTypes.Graphics.Codecs
{
    [System.Diagnostics.DebuggerDisplay("OpenCvSharp Codec")]
    public sealed class OpenCvCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static OpenCvCodec() { }

        private OpenCvCodec() { }

        private static readonly OpenCvCodec _Default = new OpenCvCodec();

        public static OpenCvCodec Default => _Default;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            bitmap = default;

            using(var mat = OpenCvSharp.Mat.FromStream(context.Stream, OpenCvSharp.ImreadModes.AnyColor))
            {
                if (mat.Cols == 0) return false;
                bitmap = mat.AsSpanBitmap().ToMemoryBitmap();                
            }

            return true;
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            using (var mat = bmp.WithOpenCv().ToMat())
            {
                mat.WriteToStream(stream.Value, $".{format.ToString().ToLower()}");
            }

            return true;
        }

        #endregion
    }
}
