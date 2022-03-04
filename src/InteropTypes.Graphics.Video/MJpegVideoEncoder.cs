using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using SharpAvi;
using SharpAvi.Codecs;

namespace InteropTypes.Codecs
{    
    sealed class MJpegVideoEncoder : IVideoEncoder
    {
        #region lifecycle

        public MJpegVideoEncoder(int width, int height, IBitmapEncoder encoder)
        {
            // Argument.IsPositive(width, nameof(width));
            // Argument.IsPositive(height, nameof(height));
            // Argument.IsInRange(quality, 1, 100, nameof(quality));

            this.width = width;
            this.height = height;
            jpegEncoder = encoder;

            buffer = new MemoryStream(MaxEncodedSize);
        }

        #endregion

        #region data

        private readonly int width;
        private readonly int height;
        private readonly IBitmapEncoder jpegEncoder;

        private readonly MemoryStream buffer;

        #endregion

        #region properties

        /// <summary>Video codec.</summary>
        public FourCC Codec => CodecIds.MotionJpeg;

        /// <summary>
        /// Number of bits per pixel in encoded image.
        /// </summary>
        public BitsPerPixel BitsPerPixel => BitsPerPixel.Bpp24;

        /// <summary>
        /// Maximum size of encoded frmae.
        /// </summary>
        public int MaxEncodedSize => Math.Max(width * height * 3, 1024);

        #endregion

        #region API

        /// <summary>
        /// Encodes a frame.
        /// </summary>
        public int EncodeFrame(byte[] source, int srcOffset, byte[] destination, int destOffset, out bool isKeyFrame)
        {
            int length;
            using (var stream = new MemoryStream(destination))
            {
                stream.Position = destOffset;
                length = LoadAndEncodeImage(source.AsSpan(srcOffset), stream);
            }

            isKeyFrame = true;
            return length;
        }

        /// <summary>
        /// Encodes a frame.
        /// </summary>
        public int EncodeFrame(ReadOnlySpan<byte> source, Span<byte> destination, out bool isKeyFrame)
        {
            // Argument.ConditionIsMet(4 * width * height <= source.Length,"Source end offset exceeds the source length.");

            buffer.SetLength(0);
            var length = LoadAndEncodeImage(source, buffer);
            buffer.GetBuffer().AsSpan(0, length).CopyTo(destination);

            isKeyFrame = true;
            return length;
        }

        private int LoadAndEncodeImage(ReadOnlySpan<byte> source, Stream destination)
        {
            var startPosition = (int)destination.Position;

            var bmp = new SpanBitmap<Pixel.BGRA32>(source, width, height, Pixel.BGRA32.Format);

            var lazy = new Lazy<Stream>(()=>destination);
            if (lazy.Value != destination) throw new InvalidOperationException();

            jpegEncoder.TryWrite(lazy, CodecFormat.Jpeg, bmp);

            destination.Flush();
            return (int)(destination.Position - startPosition);
        }

        #endregion
    }
}
