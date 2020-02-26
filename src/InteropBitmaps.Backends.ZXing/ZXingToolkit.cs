using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public static class ZXingToolkit
    {
        internal static ZXing.RGBLuminanceSource.BitmapFormat _ToZXingFormat(this PixelFormat enc)
        {
            switch (enc.PackedFormat)
            {
                case PixelFormat.Packed.GRAY8: return ZXing.RGBLuminanceSource.BitmapFormat.Gray8;
                case PixelFormat.Packed.GRAY16: return ZXing.RGBLuminanceSource.BitmapFormat.Gray16;

                case PixelFormat.Packed.BGR565: return ZXing.RGBLuminanceSource.BitmapFormat.RGB565; // notice that colors here are inverted                

                case PixelFormat.Packed.RGB24: return ZXing.RGBLuminanceSource.BitmapFormat.RGB24;
                case PixelFormat.Packed.BGR24: return ZXing.RGBLuminanceSource.BitmapFormat.BGR24;

                case PixelFormat.Packed.RGBA32: return ZXing.RGBLuminanceSource.BitmapFormat.RGBA32;                
                case PixelFormat.Packed.BGRA32: return ZXing.RGBLuminanceSource.BitmapFormat.BGRA32;
                case PixelFormat.Packed.ARGB32: return ZXing.RGBLuminanceSource.BitmapFormat.ARGB32;

                default: return ZXing.RGBLuminanceSource.BitmapFormat.Unknown;
            }
        }        

        [ThreadStatic]
        private static WeakReference<Byte[]> _TempBuffer;

        public static ZXing.Result ScanAndDecodeQRCode(this SpanBitmap src)
        {
            var reader = new ZXing.BarcodeReader();
            reader.Options.PureBarcode = false;
            reader.Options.Hints.Add(ZXing.DecodeHintType.TRY_HARDER, true);
            reader.Options.PossibleFormats = new ZXing.BarcodeFormat[] { ZXing.BarcodeFormat.QR_CODE };
            reader.Options.TryHarder = true;            

            Byte[] buffer = null;

            _TempBuffer?.TryGetTarget(out buffer);

            var luminance = src.CreateLuminanceSource(ref buffer);

            _TempBuffer = new WeakReference<byte[]>(buffer);

            return reader.Decode(luminance);
        }

        private static ZXing.LuminanceSource CreateLuminanceSource(this SpanBitmap src, ref Byte[] persistentBuffer)
        {
            var fmt = src.PixelFormat._ToZXingFormat();
            if (fmt == ZXing.RGBLuminanceSource.BitmapFormat.Unknown) return null;

            var len = src.Width * src.Height * src.PixelSize;

            if (persistentBuffer == null || persistentBuffer.Length != len) persistentBuffer = new byte[len];

            var tmp = new SpanBitmap(persistentBuffer, src.Width, src.Height, src.PixelFormat);
            tmp.SetPixels(0, 0, src);

            // ZXing.PlanarYUVLuminanceSource

            return new ZXing.RGBLuminanceSource(persistentBuffer, tmp.Width, tmp.Height, fmt);
        }
    }
}
