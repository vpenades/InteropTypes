using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

using OPENCV = OpenCvSharp.Cv2;
using CVMATRIX = OpenCvSharp.Mat;
using CVDEPTHTYPE = OpenCvSharp.MatType;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics
{
    public delegate void TransferCvAction(CVMATRIX src, CVMATRIX dst);
        

    static class _Implementation
    {
        // https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel                

        #region pixel format

        public static bool TryGetExactPixelFormat(CVDEPTHTYPE inFmt, out PixelFormat outFmt)
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
                if (inFmt == CVDEPTHTYPE.CV_32FC1) { outFmt = Pixel.Luminance32F.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_32FC3) { outFmt = Pixel.BGR96F.Format; return true; }
                if (inFmt == CVDEPTHTYPE.CV_32FC4) { outFmt = Pixel.BGRA128F.Format; return true; }                
            }

            outFmt = default;
            return false;
        }

        public static bool TryGetExactPixelFormat(PixelFormat src, out CVDEPTHTYPE dst)
        {
            switch(src.Code)
            {
                case Pixel.Alpha8.Code: { dst = CVDEPTHTYPE.CV_8UC1; return true; }

                case Pixel.Luminance8.Code: { dst = CVDEPTHTYPE.CV_8UC1; return true; }
                case Pixel.Luminance16.Code: { dst = CVDEPTHTYPE.CV_16UC1; return true; }
                case Pixel.Luminance32F.Code: { dst = CVDEPTHTYPE.CV_32FC1; return true; }

                case Pixel.BGR24.Code: { dst = CVDEPTHTYPE.CV_8UC3; return true; }
                case Pixel.BGRA32.Code: { dst = CVDEPTHTYPE.CV_8UC4; return true; }
                case Pixel.BGRP32.Code: { dst = CVDEPTHTYPE.CV_8UC4; return true; }

                case Pixel.BGR96F.Code: { dst = CVDEPTHTYPE.CV_32FC3; return true; }
                case Pixel.BGRA128F.Code: { dst = CVDEPTHTYPE.CV_32FC4; return true; }
            }

            dst = default;
            return false;
        }

        public static CVDEPTHTYPE GetCompatiblePixelFormat(PixelFormat src)
        {
            if (TryGetExactPixelFormat(src, out var cvFmt)) return cvFmt;

            switch (src.Code)
            {
                case Pixel.BGR565.Code:
                case Pixel.RGB24.Code:
                    return CVDEPTHTYPE.CV_8UC3;

                case Pixel.BGRA4444.Code:
                case Pixel.BGRA5551.Code:
                case Pixel.RGBA32.Code:
                case Pixel.RGBP32.Code:
                case Pixel.ARGB32.Code:
                    return CVDEPTHTYPE.CV_8UC4;

                case Pixel.RGBA128F.Code:
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

        /// <summary>
        /// Tries to wrap the input bitmap with a <see cref="CVMATRIX"/>.
        /// If it fails, it creates a <see cref="CVMATRIX"/> clone, using the closest compatible pixel format.
        /// </summary>
        /// <param name="src">The bitmap to wrap or clone</param>
        /// <returns>A <see cref="CVMATRIX"/></returns>
        public static CVMATRIX WrapOrCloneAsMat(PointerBitmap src)
        {
            #pragma warning disable CA2000 // Dispose objects before losing scope
            if (TryWrapAsMat(src, out var wrap)) return wrap;
            #pragma warning restore CA2000 // Dispose objects before losing scope
            return CloneAsMat(src.AsSpanBitmap());
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
            if (dst.Rows != 2) throw new ArgumentOutOfRangeException(nameof(dst), "Rows");
            if (dst.Cols != 3) throw new ArgumentOutOfRangeException(nameof(dst), "Cols");

            dst.Set(0, 0, src.M11);
            dst.Set(0, 1, src.M21);
            dst.Set(0, 2, src.M31);
            dst.Set(1, 0, src.M12);
            dst.Set(1, 1, src.M22);
            dst.Set(1, 2, src.M32);
        }

        public static void CopyTo(this in System.Numerics.Matrix3x2 src, ref CVMATRIX dst)
        {
            if (dst == null) dst = new CVMATRIX(2, 3, CVDEPTHTYPE.CV_32F, 1);
            else
            {
                if (dst.Type() != CVDEPTHTYPE.CV_32F) throw new ArgumentOutOfRangeException(nameof(dst), "Type");
                if (dst.Rows != 2) throw new ArgumentOutOfRangeException(nameof(dst), "Rows");
                if (dst.Cols != 3) throw new ArgumentOutOfRangeException(nameof(dst), "Cols");
            }            

            dst.Set(0, 0, src.M11);
            dst.Set(0, 1, src.M21);
            dst.Set(0, 2, src.M31);
            dst.Set(1, 0, src.M12);
            dst.Set(1, 1, src.M22);
            dst.Set(1, 2, src.M32);
        }

        public static Tensors.SpanTensor2<T> AsSpanTensor2<T>(this CVMATRIX src)
            where T : unmanaged
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (src.Empty()) throw new ArgumentNullException(nameof(src), "Empty");
            if (!src.IsContinuous()) throw new ArgumentNullException(nameof(src), "IsContinuous");
            // if (OPENCV.GetDepthType(src.Depth) != typeof(T)) throw new ArgumentNullException(nameof(T));            

            if (src.Dims == 1)
            {
                if (src.Channels() == 1) throw new ArgumentNullException(nameof(src), "Channels");
                return new Tensors.SpanTensor2<T>(src.Data, src.Size(0), src.Channels());
            }

            if (src.Dims == 2)
            {
                if (src.Channels() != 1) throw new ArgumentNullException(nameof(src), "Channels");
                return new Tensors.SpanTensor2<T>(src.Data, src.Size(0), src.Size(1));
            }

            throw new ArgumentNullException(nameof(src), "Dims");
        }

        public static Tensors.SpanTensor3<T> AsSpanTensor3<T>(this CVMATRIX src)
            where T : unmanaged
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (src.Empty()) throw new ArgumentNullException(nameof(src), "Empty");
            if (!src.IsContinuous()) throw new ArgumentNullException(nameof(src), "IsContinuous");
            // if (OPENCV.GetDepthType(src.Depth) != typeof(T)) throw new ArgumentNullException(nameof(T));



            if (src.Dims == 2)
            {
                if (src.Channels() == 1) throw new ArgumentNullException(nameof(src), "Channels");
                return new Tensors.SpanTensor3<T>(src.Data, src.Size(0), src.Size(1), src.Channels());
            }

            if (src.Dims == 3)
            {
                if (src.Channels() == 1) throw new ArgumentNullException(nameof(src), "Channels");
                return new Tensors.SpanTensor3<T>(src.Data, src.Size(0), src.Size(1), src.Size(2));
            }

            throw new ArgumentNullException(nameof(src.Dims));
        }

        #endregion

        #region extras

        public static POINT2 ToPoint2(this OpenCvSharp.Point2f p) { return new POINT2(p.X, p.Y); }        

        public static void TransferSpan(SpanBitmap src, SpanBitmap dst, SpanBitmap.Action2 action)
        {
            action(src, dst);
        }

        public static void TransferCv(SpanBitmap src, SpanBitmap dst, TransferCvAction action)
        {
            void _onPin(PointerBitmap ptrSrc, PointerBitmap ptrDst)
            {
                using var cvSrc = WrapAsMat(ptrSrc);
                using var cvDst = WrapAsMat(ptrDst);                
                action(cvSrc, cvDst);                
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
