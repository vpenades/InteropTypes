using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using OpenCvSharp;

namespace InteropTypes.Graphics.Adapters
{
    public readonly ref struct OpenCvSharp4SpanAdapter
    {
        #region constructor

        public OpenCvSharp4SpanAdapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        public Mat ToMat() { return _Implementation.CloneAsMat(_Bitmap); }

        public unsafe void Apply(Func<Mat, Mat> context)
        {
            _Bitmap.PinWritablePointer(bmp => _Apply(bmp, context));
        }        

        private static unsafe void _Apply(PointerBitmap bmp, Func<Mat, Mat> operation)
        {
            using (var srcMat = _Implementation.WrapOrCloneAsMat(bmp))
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

        public OpenCvSharp4SpanAdapter ApplyBlur((int w, int h) kernel)
        {
            Apply(mat => mat.Blur(new Size(kernel.w,kernel.h)));
            return this;
        }

        public OpenCvSharp4SpanAdapter ApplyCanny(double threshold1, double threshold2, int apertureSize = 3, bool L2gradient = false)
        {
            Apply(mat => mat.Canny(threshold1, threshold2, apertureSize, L2gradient));
            return this;
        }

        public MemoryBitmap ToResizedMemoryBitmap(int width, int height, InterpolationFlags flags = InterpolationFlags.Linear)
        {
            return _Bitmap.PinReadablePointer(ptr => _Resize(ptr, width, height, flags));
        }

        private static MemoryBitmap _Resize(PointerBitmap src, int width, int height, InterpolationFlags flags)
        {
            using (var srcMat = _Implementation.WrapAsMat(src))
            {
                using (var dstMat = srcMat.Resize(new Size(width, height), 0, 0, flags))
                {
                    return dstMat.AsSpanBitmap().ToMemoryBitmap();
                }
            }
        }

        #endregion
    }
}
