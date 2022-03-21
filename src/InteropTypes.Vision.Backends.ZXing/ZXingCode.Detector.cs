using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Vision.Backends
{
    partial class ZXingCode
    {
        public class Detector :
            PointerBitmapInput.IInference<ZXingCode>,
            IInferenceContext<PointerBitmap, ZXingCode>
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

            public void Dispose() { }

            #endregion

            #region data

            private ZXing.BarcodeReaderGeneric _Reader;

            private Byte[] _RecyclableBuffer = null;

            #endregion

            #region API

            public void Inference(ZXingCode result, PointerBitmapInput input, Rectangle? inputWindow = null)
            {
                input.PinInput(ptr => Inference(result, ptr, inputWindow));
            }

            public void Inference(ZXingCode result, InferenceInput<PointerBitmap> input, Rectangle? inputWindow = null)
            {
                var bmp = input.GetClippedPointerBitmap(ref inputWindow);

                var luminance = _Implementation.CreateLuminanceSource(bmp.AsSpanBitmap(), ref _RecyclableBuffer);
                
                 var r = _Reader.Decode(luminance);

                result._Results.Clear();
                result._Results.Add(r);
                result._ResultRect = inputWindow; // we need to store the crop to reinterpret the results as full image.

                result.CaptureTime = input.CaptureTime;
                result.CaptureDevice = input.CaptureDeviceName;
            }

            #endregion
        }
    }
}
