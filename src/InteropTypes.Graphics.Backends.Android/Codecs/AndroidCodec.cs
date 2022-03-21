using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    [System.Diagnostics.DebuggerDisplay("GDI Codec")]
    public sealed class AndroidBitmapCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static AndroidBitmapCodec() { }

        private AndroidBitmapCodec() { }

        private static readonly AndroidBitmapCodec _Default = new AndroidBitmapCodec();

        public static AndroidBitmapCodec Default => _Default;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                bitmap = default;

                var bmp = Android.Graphics.BitmapFactory.DecodeStream(context.Stream);                

                bmp.CopyTo(ref bitmap);

                return true;
            }
            catch (System.ArgumentException)
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = _GetFormatFromExtension(format);
            if (fmt == null) return false;
            
            using var dst = bmp.Info.ToAndroidFactory().CreateCompatibleBitmap();

            bmp.CopyTo(dst);

            dst.Compress(fmt, 100, stream.Value);

            return true;
        }

        private static Android.Graphics.Bitmap.CompressFormat _GetFormatFromExtension(CodecFormat format)
        {
            switch (format)
            {
                case CodecFormat.Png: return Android.Graphics.Bitmap.CompressFormat.Png;
                case CodecFormat.Jpeg: return Android.Graphics.Bitmap.CompressFormat.Jpeg;

                #pragma warning disable CS0618 // Type or member is obsolete
                case CodecFormat.Webp: return Android.Graphics.Bitmap.CompressFormat.Webp;
                #pragma warning restore CS0618 // Type or member is obsolete
                case CodecFormat.WebpLossy: return Android.Graphics.Bitmap.CompressFormat.WebpLossy;
                case CodecFormat.WebpLossless: return Android.Graphics.Bitmap.CompressFormat.WebpLossless;

                default: return null;
            }
        }

        #endregion
    }
}
