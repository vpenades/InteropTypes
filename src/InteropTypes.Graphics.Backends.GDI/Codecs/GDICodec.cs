using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using GDIIMAGEFORMAT = System.Drawing.Imaging.ImageFormat;

namespace InteropTypes.Codecs
{
    /// <remarks>
    /// Images are read in <see cref="Pixel.BGRA32"/> format.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("GDI Codec")]
    #if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public sealed class GDICodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle        

        public static GDICodec Default { get; } = new GDICodec();

        private GDICodec() { }
        
        public GDICodec(int quality)
        {
            _Quality = Math.Max(1, Math.Min(100, quality));
        }        

        #endregion

        #region data

        private int? _Quality;

        private ImageCodecInfo[] _CodecsCache;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                using (var img = System.Drawing.Image.FromStream(context.Stream))
                {
                    bitmap = _Implementation.CloneAsMemoryBitmap(img);
                }

                return true;
            }
            catch(System.ArgumentException)
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = GetFormatFromExtension(format);
            if (fmt == null) return false;            

            if (!_Implementation.TryGetExactPixelFormat(bmp.PixelFormat, out _))
            {
                using (var tmp = _Implementation.CloneAsGDIBitmap(bmp))
                {
                    _WriteBitmap(stream.Value, fmt, tmp);
                }
            }
            else
            {
                void _doSave(PointerBitmap ptr)
                {
                    using (var tmp = _Implementation.WrapOrCloneAsGDIBitmap(ptr))
                    {
                        _WriteBitmap(stream.Value, fmt, tmp);
                    }
                }

                bmp.PinReadablePointer(_doSave);
            }           

            return true;
        }

        private void _WriteBitmap(Stream stream, GDIIMAGEFORMAT fmt, System.Drawing.Bitmap tmp)
        {
            var customOptions = false;

            customOptions |= _Quality.HasValue;

            if (!customOptions)
            {
                tmp.Save(stream,fmt);
                return;
            }

            var encoder = GetEncoder(fmt);

            using (var ppp = new EncoderParameters(1))
            {
                ppp.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)_Quality.Value);
                tmp.Save(stream, encoder, ppp);
            }
        }

        private static GDIIMAGEFORMAT GetFormatFromExtension(CodecFormat format)
        {
            switch(format)
            {
                case CodecFormat.Png: return GDIIMAGEFORMAT.Png;
                case CodecFormat.Jpeg: return GDIIMAGEFORMAT.Jpeg;

                case CodecFormat.Tiff: return GDIIMAGEFORMAT.Tiff;
                case CodecFormat.Icon: return GDIIMAGEFORMAT.Icon;

                case CodecFormat.Gif: return GDIIMAGEFORMAT.Gif;
                case CodecFormat.Bmp: return GDIIMAGEFORMAT.Bmp;
                case CodecFormat.Emf: return GDIIMAGEFORMAT.Emf;
                case CodecFormat.Wmf: return GDIIMAGEFORMAT.Wmf;
                default: return null;
            }            
        }

        private ImageCodecInfo GetEncoder(GDIIMAGEFORMAT format)
        {
            if (_CodecsCache == null) _CodecsCache = ImageCodecInfo.GetImageEncoders();

            foreach (var codec in _CodecsCache)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        #endregion
    }
}
