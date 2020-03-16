using System;
using System.Collections.Generic;
using System.Text;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct OpenCvSharp4Adapter
    {
        #region constructor

        public OpenCvSharp4Adapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        public Mat ToMat() { return _Implementation.CloneToMat(_Bitmap); }

        public unsafe void Apply(Func<Mat, Mat> context)
        {
            _Bitmap.PinWritableMemory(bmp => _Apply(bmp, context));
        }

        private static unsafe void _Apply(PointerBitmap bmp, Func<Mat, Mat> operation)
        {
            using (var srcMat = bmp.AsMat())
            {
                using (var dstMat = operation(srcMat))
                {
                    if (dstMat == srcMat) return;

                    bool isValid = true;

                    if (srcMat.Width != dstMat.Width) isValid = false;
                    if (srcMat.Height != dstMat.Height) isValid = false;
                    if (!isValid) throw new ArgumentException("Operation should not change image size.", nameof(operation));                    
                    
                    srcMat.AsSpanBitmap().SetPixels(0, 0, dstMat.AsSpanBitmap());                               
                }
            }
        }        

        public OpenCvSharp4Adapter ApplyBlur((int w, int h) kernel)
        {
            Apply(mat => mat.Blur(new Size(kernel.w,kernel.h)));
            return this;
        }

        public OpenCvSharp4Adapter ApplyCanny(double threshold1, double threshold2, int apertureSize = 3, bool L2gradient = false)
        {
            Apply(mat => mat.Canny(threshold1, threshold2, apertureSize, L2gradient));
            return this;
        }

        #endregion
    }
}
