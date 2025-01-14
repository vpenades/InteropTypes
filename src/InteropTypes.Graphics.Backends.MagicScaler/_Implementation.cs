using System;

using InteropTypes.Graphics.Bitmaps;

using PhotoSauce.MagicScaler;
using PhotoSauce.MagicScaler.Transforms;

using XPIXFMT = InteropTypes.Graphics.Bitmaps.PixelFormat;


namespace InteropTypes.Graphics.Backends
{
    static class _Implementation
    {
        public static Guid ToMagicScalerFormat(XPIXFMT fmt)
        {
            var id = ToMagicScalerFormatOrDefault(fmt);

            return id != Guid.Empty ? id : throw new ArgumentException($"{fmt} not supported", nameof(fmt));
        }

        public static Guid ToMagicScalerFormatOrDefault(XPIXFMT fmt)
        {
            switch (fmt.Code)
            {
                case Pixel.Alpha8.Code: return PixelFormats.Grey8bpp;
                case Pixel.Luminance8.Code: return PixelFormats.Grey8bpp;
                case Pixel.BGR24.Code: return PixelFormats.Bgr24bpp;
                case Pixel.BGRA32.Code: return PixelFormats.Bgra32bpp;
                default: return Guid.Empty;
            }
        }

        public static MemoryBitmap Rescale(MemoryBitmap srcBitmap, int dstWidth, int dstHeight)
        {
            // https://github.com/saucecontrol/PhotoSauce/discussions/175

            var srcAdapter = new _BitmapAdapter(srcBitmap);

            var dstBitmap = new MemoryBitmap(dstWidth, dstHeight, ToPixelFormat(srcAdapter.Format));

            srcAdapter.ResizeTo(dstBitmap);

            return dstBitmap;
        }

        public static void RescaleTo(SpanBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            new _BitmapAdapter(srcBitmap).ResizeTo(dstBitmap);
        }

        public static void RescaleTo(MemoryBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            new _BitmapAdapter(srcBitmap).ResizeTo(dstBitmap);
        }

        public static MemoryBitmap ToMemoryBitmap(IPixelSource ps)
        {
            if (ps == null) throw new ArgumentNullException(nameof(ps));

            XPIXFMT fmt = ToPixelFormat(ps.Format);

            var m = new MemoryBitmap(ps.Width, ps.Height, fmt);            

            CopyTo(ps, m);

            return m;
        }

        public static void CopyTo(IPixelSource ps, SpanBitmap ms)
        {
            if (ps == null) throw new ArgumentNullException(nameof(ps));
            if (ToMagicScalerFormat(ms.PixelFormat) != ps.Format) throw new ArgumentException("pixel format mismatch", nameof(ms));

            var w = Math.Min(ms.Width, ps.Width);
            var h = Math.Min(ms.Height, ps.Height);
            var srcRect = new System.Drawing.Rectangle(0, 0, w, h);

            ps.CopyPixels(srcRect, ms.StepByteSize, ms.WritableBytes);
        }

        private static XPIXFMT ToPixelFormat(Guid magicScalerFormat)
        {
            XPIXFMT fmt = default;
            if (magicScalerFormat == PixelFormats.Grey8bpp) fmt = Pixel.Alpha8.Format;
            if (magicScalerFormat == PixelFormats.Bgr24bpp) fmt = Pixel.BGR24.Format;
            if (magicScalerFormat == PixelFormats.Bgra32bpp) fmt = Pixel.BGRA32.Format;
            return fmt;
        }
    }
}
