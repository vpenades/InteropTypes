using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace InteropVision.With
{
    public class ZXingCode
    {
        #region data

        public DateTime Time { get; internal set; }
        public ZXing.Result Result { get; internal set; }

        #endregion

        #region nested

        public class Detector : IImageInference<ZXingCode>
        {
            #region lifecycle        

            public Detector()
            {
                _Reader = new ZXing.BarcodeReaderGeneric();
                _Reader.Options.PureBarcode = false;
                _Reader.Options.Hints.Add(ZXing.DecodeHintType.TRY_HARDER, true);
                _Reader.Options.PossibleFormats = new ZXing.BarcodeFormat[] { ZXing.BarcodeFormat.QR_CODE };
                _Reader.Options.TryHarder = true;
            }

            public void Dispose()
            {

            }

            #endregion

            #region data

            private ZXing.BarcodeReaderGeneric _Reader;

            private Byte[] _Buffer = null;

            #endregion

            #region API        

            public void Inference(ZXingCode result, PointerBitmapInput input, Rectangle? inputWindow = null)
            {
                var luminance = _Implementation.CreateLuminanceSource(input.Content.AsSpanBitmap(), ref _Buffer);

                result.Time = input.Time;
                result.Result = _Reader.Decode(luminance);
            }

            #endregion
        }

        #endregion
    }


}
