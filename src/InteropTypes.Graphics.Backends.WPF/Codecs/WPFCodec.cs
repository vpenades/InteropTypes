using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    /// <remarks>
    /// Images are read in <see cref="Pixel.BGR24"/> format.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("WPF(WIC) Codec")]
    #if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public sealed class WPFCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static WPFCodec() { }

        private WPFCodec() { }

        private static readonly WPFCodec _Default = new WPFCodec();

        public static WPFCodec Default => _Default;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                var frame = System.Windows.Media.Imaging.BitmapFrame.Create(context.Stream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.None);

                bitmap = _Implementation.ToMemoryBitmap(frame);

                return true;
            }
            catch(System.NotSupportedException)
            {
                bitmap = default;
                return false;
            }
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

        #endregion
    }
}
