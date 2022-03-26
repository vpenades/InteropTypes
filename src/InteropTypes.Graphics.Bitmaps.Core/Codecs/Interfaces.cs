using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    public class CodecException : System.IO.IOException { }

    public struct BitmapDecoderContext
    {
        public System.IO.Stream Stream;
        public int? BytesToRead;
    }

    /// <summary>
    /// Interface implemented by bitmap reader codecs.
    /// </summary>
    public interface IBitmapDecoder
    {
        /// <summary>
        /// Tries to read an image from the stream.
        /// </summary>
        /// <param name="readContext">The input stream to read from.</param>
        /// <param name="bitmap">The output bitmap.</param>        
        /// <returns>True if succeeded reading the image, false otherwise.</returns>
        bool TryRead(BitmapDecoderContext readContext, out MemoryBitmap bitmap);
    }

    /// <summary>
    /// Interface implemented by bitmap writer codecs.
    /// </summary>
    public interface IBitmapEncoder
    {
        /// <summary>
        /// Tries to write a bitmap to a stream.
        /// </summary>
        /// <param name="stream">A late initialized stream.</param>
        /// <param name="format">The format in which the image must be written.</param>
        /// <param name="bmp">The bitmap to write.</param>
        /// <returns>True if succeeded writing the image, false otherwise.</returns>
        /// <remarks>
        /// If the object implementing this interface cannot write the given <see cref="CodecFormat"/><br/>
        /// it must return false without attempting to use the stream.
        /// </remarks>
        bool TryWrite(Lazy<System.IO.Stream> stream, CodecFormat format, SpanBitmap bmp);
    }
    
    public enum CodecFormat
    {
        Undefined,
        Bmp,
        Gif,
        Icon,
        Jpeg,
        Png,
        
        Pkm,
        Ktx,
        Astc,
        Dng,
        Heif,
        Tiff,
        Tga,

        Hdr,
        Dds,

        Emf,
        Wmf,
        Wbmp,

        [Obsolete("Use WebpLossy and WebpLossless")]
        Webp,
        WebpLossy,
        WebpLossless,        

        Psd,
    }    
}
