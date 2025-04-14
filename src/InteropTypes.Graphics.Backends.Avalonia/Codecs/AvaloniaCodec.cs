using Avalonia.Media.Imaging;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Codecs
{
    /// <remarks>
    /// Images are read in <see cref="Pixel.BGR24"/> format.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Avalonia Codec")]
    #if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public sealed class AvaloniaCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static AvaloniaCodec() { }

        private AvaloniaCodec() { }

        private static readonly AvaloniaCodec _Default = new AvaloniaCodec();

        public static AvaloniaCodec Default => _Default;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                var frame = new Avalonia.Media.Imaging.Bitmap(context.Stream);

                bitmap = _Implementation.ToMemoryBitmap(frame);

                return true;
            }
            catch (System.NotSupportedException)
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            Bitmap avbmp = null;

            try
            {
                avbmp = _Implementation.CreateAvaloniaBitmap(bmp);
                if (avbmp == null) return false;
            }
            catch
            {
                return false;
            }

            avbmp.Save(stream.Value);
            return true;
        }        

        #endregion
    }
}
