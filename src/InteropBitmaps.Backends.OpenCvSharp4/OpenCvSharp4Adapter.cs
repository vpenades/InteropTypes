using System;
using System.Collections.Generic;
using System.Text;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace InteropBitmaps
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

        public Mat ToMat()
        {
            var mtype = MatType.CV_8UC(_Bitmap.PixelSize);

            var dst = new Mat(_Bitmap.Height, _Bitmap.Width, mtype);
            dst.AsSpanBitmap().SetPixels(0, 0, _Bitmap);

            return dst;
        }

        public unsafe void Mutate(Func<Mat, Mat> context)
        {
            _Bitmap.PinWritableMemory(bmp => _Mutate(bmp, context));
        }

        public unsafe MemoryBitmap CloneMutated(Func<Mat, Mat> context)
        {
            MemoryBitmap r = null;

            _Bitmap.PinWritableMemory(bmp => r = _CloneMutated(bmp, context));

            return r;
        }

        private static unsafe void _Mutate(PointerBitmap bmp, Func<Mat, Mat> operation)
        {
            using (var srcMat = bmp.AsOpenCvMat())
            {
                using (var dstMat = operation(srcMat))
                {
                    if (dstMat == srcMat) return;

                    bool isValid = true;

                    if (srcMat.Width != dstMat.Width) isValid = false;
                    if (srcMat.Height != dstMat.Height) isValid = false;
                    if (!isValid) throw new ArgumentException("Operation should not change image size.", nameof(operation));

                    
                    if (srcMat.Type() == dstMat.Type())
                    {
                        srcMat.AsSpanBitmap().SetPixels(0, 0, dstMat.AsSpanBitmap());
                        return;
                    }

                    throw new NotImplementedException();

                    /*
                    using (var tmp = new Mat(dstMat.Width, dstMat.Height, mtype))
                    {
                        // dstMat.AssignTo(tmp, mtype);
                        srcMat.AsSpanBitmap().SetPixels(0, 0, tmp.AsSpanBitmap());
                    }*/                   
                }
            }
        }

        private static unsafe MemoryBitmap _CloneMutated(PointerBitmap bmp, Func<Mat, Mat> operation)
        {
            using (var srcMat = bmp.AsOpenCvMat())
            {
                using (var dstMat = operation(srcMat))
                {
                    return dstMat.AsSpanBitmap().ToMemoryBitmap();
                }
            }
        }

        public void Save(string filePath)
        {
            Mutate(mat => { mat.SaveImage(filePath); return mat; } );
        }

        public void Blur((int w, int h) kernel)
        {
            Mutate(mat => mat.Blur(new Size(kernel.w,kernel.h)));
        }

        #endregion
    }
}
