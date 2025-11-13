using System;
using System.IO;

using InteropTypes.Graphics.Bitmaps;

using STBREAD = StbImageSharp;
using STBWRITE = StbImageWriteSharp;
using STBDIRECTX = StbDxtSharp;

namespace InteropTypes.Codecs
{
    /// <summary>
    /// STB Sharp codec for Jpeg, Png and DDS
    /// </summary>
    /// <remarks>
    /// Images are read in <see cref="Pixel.RGBA32"/> format.
    /// </remarks>
    #if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public class STBCodec : IBitmapDecoder , IBitmapEncoder
    {
        #region lifecycle

        static STBCodec() { }        

        private static readonly STBCodec _Default = new STBCodec(80);

        public static STBCodec Default => _Default;

        public static STBCodec WithQuality(int quality)
        {
            if (quality < 1) quality = 1;
            if (quality > 100) quality = 100;
            return new STBCodec(quality);
        }

        private STBCodec() : this(80) { } // required for reflection

        private STBCodec(int jpegQuality) { _JpegQuality = jpegQuality; }

        #endregion

        #region data

        private readonly int _JpegQuality;

        #endregion

        #region API - Decoder

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            bitmap = default;            

            try
            {
                var img = STBREAD.ImageResult.FromStream(context.Stream, STBREAD.ColorComponents.RedGreenBlueAlpha);                

                bitmap = _Implementation.AsMemoryBitmap(img);

                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message == "unknown image type") return false;
                else throw;
            }
        }

        #endregion

        #region API - Encoder        

        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            if (format == CodecFormat.Png)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;                
                new STBWRITE.ImageWriter().WritePng(rdata, rinfo.Width, rinfo.Height, wfmt, stream.Value);
                return true;
            }

            if (format == CodecFormat.Jpeg)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;
                new STBWRITE.ImageWriter().WriteJpg(rdata, rinfo.Width, rinfo.Height, wfmt, stream.Value, _JpegQuality);
                return true;
            }

            if (format == CodecFormat.Bmp)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;
                new STBWRITE.ImageWriter().WriteBmp(rdata, rinfo.Width, rinfo.Height, wfmt, stream.Value);
                return true;
            }

            if (format == CodecFormat.Tga)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;
                new STBWRITE.ImageWriter().WriteTga(rdata, rinfo.Width, rinfo.Height, wfmt, stream.Value);
                return true;
            }

            if (format == CodecFormat.Hdr)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;
                new STBWRITE.ImageWriter().WriteHdr(rdata, rinfo.Width, rinfo.Height, wfmt, stream.Value);
                return true;
            }

            /*
            if (format == CodecFormat.Dds)
            {
                if (!_ExtractPixels(bmp, out var rinfo, out var rdata, out var wfmt)) return false;

                if (wfmt == STBWRITE.ColorComponents.RedGreenBlueAlpha)
                {
                    var compressedData = STBDIRECTX.StbDxt.CompressDxt5(rinfo.Width, rinfo.Height, rdata);
                    // write DDS header
                    // write compressedData
                }
            }*/

            return false;        
        }

        private static bool _ExtractPixels(SpanBitmap bmp, out BitmapInfo rinfo, out Byte[] rdata, out STBWRITE.ColorComponents rcmps)
        {
            rinfo = default;
            rdata = null;
            rcmps = default;

            rcmps = _Implementation.GetCompatibleFormat(bmp.PixelFormat);
            if (!_Implementation.TryGetPixelFormat(rcmps, out var rfmt)) return false;

            rinfo = new BitmapInfo(bmp.Width, bmp.Height, rfmt);
            rdata = new Byte[rinfo.BitmapByteSize];

            new SpanBitmap(rdata.AsSpan(), rinfo).SetPixels(0, 0, bmp);

            return true;
        }

        #endregion
    }
}
