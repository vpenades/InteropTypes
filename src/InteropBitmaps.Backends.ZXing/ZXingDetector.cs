using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Detectors
{
    public class ZXingDetector
    {
        #region lifecycle

        public ZXingDetector()
        {
            _Reader = new ZXing.BarcodeReader();
            _Reader.Options.PureBarcode = false;
            _Reader.Options.Hints.Add(ZXing.DecodeHintType.TRY_HARDER, true);
            _Reader.Options.PossibleFormats = new ZXing.BarcodeFormat[] { ZXing.BarcodeFormat.QR_CODE };
            _Reader.Options.TryHarder = true;
        }

        #endregion

        #region data

        private ZXing.BarcodeReader _Reader;

        private Byte[] _Buffer = null;

        #endregion

        #region API

        public ZXing.Result SearchForQRCodes(SpanBitmap src)
        {
            var luminance = _Implementation.CreateLuminanceSource(src, ref _Buffer);

            return _Reader.Decode(luminance);
        }

        #endregion
    }
}
