using System;
using System.Collections.Generic;
using System.Text;

using OPENCV = OpenCvSharp.Cv2;
using CVMATRIX = OpenCvSharp.Mat;
using CVDEPTHTYPE = OpenCvSharp.MatType;

namespace InteropBitmaps
{
    public delegate void TransferCvAction(CVMATRIX src, CVMATRIX dst);
    public delegate void TransferPtrAction(PointerBitmap src, PointerBitmap dst);

    static class _Implementation
    {
        // https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel                

        #region pixel format

        public static bool TryGetExactPixelFormat(CVDEPTHTYPE inFmt, out Pixel.Format outFmt)
        {
            if (inFmt.IsInteger)
            {
                if (inFmt == CVDEPTHTYPE.CV_8UC1) { outFmt = Pixel.Luminance8.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_16UC1) { outFmt = Pixel.Luminance16.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_8UC3) { outFmt = Pixel.BGR24.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_8UC4) { outFmt = Pixel.BGRA32.Format; return true; }                
            }
            else
            {
                if (inFmt == CVDEPTHTYPE.CV_32FC1) { outFmt = Pixel.LuminanceScalar.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_32FC3) { outFmt = Pixel.VectorBGR.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_32FC4) { outFmt = Pixel.VectorBGRA.Format; return true; }                
            }

            outFmt = default;
            return false;
        }

        public static bool TryGetExactPixelFormat(Pixel.Format src, out CVDEPTHTYPE dst)
        {
            switch(src.PackedFormat)
            {
                case Pixel.Luminance8.Code: { dst = CVDEPTHTYPE.CV_8UC1; return true; }
                case Pixel.Luminance16.Code: { dst = CVDEPTHTYPE.CV_16UC1; return true; }
                case Pixel.LuminanceScalar.Code: { dst = CVDEPTHTYPE.CV_32FC1; return true; }

                case Pixel.BGR24.Code: { dst = CVDEPTHTYPE.CV_8UC3; return true; }
                case Pixel.BGRA32.Code: { dst = CVDEPTHTYPE.CV_8UC4; return true; }

                case Pixel.VectorBGR.Code: { dst = CVDEPTHTYPE.CV_32FC3; return true; }
                case Pixel.VectorBGRA.Code: { dst = CVDEPTHTYPE.CV_32FC4; return true; }
            }

            dst = default;
            return false;
        }

        public static CVDEPTHTYPE GetCompatiblePixelFormat(Pixel.Format src)
        {
            if (TryGetExactPixelFormat(src, out var cvFmt)) return cvFmt;

            switch (src.PackedFormat)
            {
                case Pixel.BGR565.Code:
                case Pixel.RGB24.Code:
                    return CVDEPTHTYPE.CV_8UC3;

                case Pixel.BGRA4444.Code:
                case Pixel.BGRA5551.Code:
                case Pixel.RGBA32.Code:
                case Pixel.ARGB32.Code:
                    return CVDEPTHTYPE.CV_8UC4;

                case Pixel.VectorRGBA.Code:
                    return CVDEPTHTYPE.CV_32FC4;

                default: throw new NotImplementedException();
            }            
        }

        #endregion

        #region API        

        public static unsafe bool TryWrapAsPointer(CVMATRIX src, out PointerBitmap wrap)
        {
            wrap = default;

            if (src == null) return false;
            if (src.Width < 0) return false;

            // src.IsContinuous = true; ??
            // src.IsSubMatrix = false ??

            if (!TryGetExactPixelFormat(src.Type(), out var fmt)) return false;

            var info = new BitmapInfo(src.Width, src.Height, fmt, (int)src.Step());

            wrap = new PointerBitmap(src.Data, info);

            return true;
        }
        
        public static CVMATRIX WrapAsMat(PointerBitmap src)
        {
            return TryWrapAsMat(src, out var mat)
                ? mat
                : throw new ArgumentException("invalid format", nameof(src));
        }

        public static bool TryWrapAsMat(PointerBitmap src, out CVMATRIX wrap)
        {
            if (src.Info.IsEmpty) { wrap = null; return true; }
            if (!TryGetExactPixelFormat(src.Info.PixelFormat, out var fmt)) { wrap = null; return false; }

            wrap = new CVMATRIX(src.Info.Height, src.Info.Width, fmt, src.Pointer, src.Info.StepByteSize);

            return true;
        }

        public static CVMATRIX CloneAsMat(SpanBitmap src)
        {
            if (src.Info.IsEmpty) return null;

            // find a OpenCV compatible pixel format
            var cvFmt = GetCompatiblePixelFormat(src.Info.PixelFormat);

            // create a mat, and expose it as a Ptr
            var dst = new CVMATRIX(src.Height, src.Width, cvFmt);
            if (!TryWrapAsPointer(dst, out var dstPtr)) throw new InvalidOperationException();

            // copy/convert pixel data
            dstPtr.AsSpanBitmap().SetPixels(0, 0, src);

            return dst;
        }

        public static unsafe OpenCvSharp.Mat<TPixel> CloneAsMat<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel : unmanaged
        {
            var dst = new OpenCvSharp.Mat<TPixel>(srcSpan.Height, srcSpan.Width);
            var dstSpan = dst.AsSpanBitmap();
            dstSpan.SetPixels(0, 0, srcSpan);
            return dst;
        }

        public static CVMATRIX ToMat(in System.Numerics.Matrix3x2 src)
        {
            var dst = new CVMATRIX(2, 3, CVDEPTHTYPE.CV_32F, 1);
            src.CopyTo(dst);
            return dst;
        }

        public static void CopyTo(this in System.Numerics.Matrix3x2 src, CVMATRIX dst)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            if (dst.Rows != 2) throw new ArgumentOutOfRangeException("Rows", nameof(dst));
            if (dst.Cols != 3) throw new ArgumentOutOfRangeException("Cols", nameof(dst));

            dst.Set(0, 0, src.M11);
            dst.Set(0, 1, src.M21);
            dst.Set(0, 2, src.M31);
            dst.Set(1, 0, src.M12);
            dst.Set(1, 1, src.M22);
            dst.Set(1, 2, src.M32);
        }

        #endregion

        #region extras

        public static void TransferPtr(SpanBitmap src, SpanBitmap dst, TransferPtrAction action)
        {
            SpanBitmap.PinTransferPointers(src, dst, (s,d) => action(s,d));
        }

        public static void TransferCv(SpanBitmap src, SpanBitmap dst, TransferCvAction action)
        {
            void _onPin(PointerBitmap ptrSrc, PointerBitmap ptrDst)
            {
                using (var cvSrc = WrapAsMat(ptrSrc))
                using (var cvDst = WrapAsMat(ptrDst))
                {
                    action(cvSrc, cvDst);
                }
            }

            SpanBitmap.PinTransferPointers(src, dst, _onPin);
        }

        public static void TransferCv(PointerBitmap src, PointerBitmap dst, TransferCvAction action)
        {            
            using (var cvSrc = WrapAsMat(src))
            using (var cvDst = WrapAsMat(dst))
            {
                action(cvSrc, cvDst);
            }
        }
        
        public static void WarpAffine(CVMATRIX src, CVMATRIX dst, in System.Numerics.Matrix3x2 xform)
        {
            // https://docs.opencv.org/master/dd/d52/tutorial_js_geometric_transformations.html            

            using (var xformMat = ToMat(xform))
            {
                OPENCV.WarpAffine(src, dst, xformMat, new OpenCvSharp.Size(dst.Rows, dst.Cols));
            }
        }

        #endregion
    }
}
