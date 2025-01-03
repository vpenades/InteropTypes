﻿using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public static class ZXingToolkit
    {
        [ThreadStatic]
        private static WeakReference<Byte[]> _TempBuffer;

        public static ZXing.Result ScanAndDecodeQRCode<TPixel>(this SpanBitmap<TPixel> src)
            where TPixel : unmanaged
        {
            var reader = new ZXing.BarcodeReaderGeneric();
            reader.Options.PureBarcode = false;
            reader.Options.Hints.Add(ZXing.DecodeHintType.TRY_HARDER, true);
            reader.Options.PossibleFormats = new ZXing.BarcodeFormat[] { ZXing.BarcodeFormat.QR_CODE };
            reader.Options.TryHarder = true;            

            Byte[] buffer = null;

            _TempBuffer?.TryGetTarget(out buffer);

            var luminance = _Implementation.CreateLuminanceSource(src, ref buffer);

            _TempBuffer = new WeakReference<byte[]>(buffer);

            return reader.Decode(luminance);
        }

        
    }
}
