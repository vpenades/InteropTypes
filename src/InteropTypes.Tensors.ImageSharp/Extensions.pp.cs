using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;

#nullable disable

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

using __SIXLABORS = SixLabors.ImageSharp;
using __SIXLABORSPIXFMT = SixLabors.ImageSharp.PixelFormats;

using __XYZ = System.Numerics.Vector3;
using __XYZW = System.Numerics.Vector4;

using __FINFO = System.IO.FileInfo;

#if INTEROPTYPES_USEINTEROPNAMESPACE
namespace InteropTypes.Tensors
#elif INTEROPTYPES_TENSORS_USESIXLABORSNAMESPACE
namespace SixLabors.ImageSharp
#else
namespace $rootnamespace$
#endif
{
    internal static partial class InteropTensorsForImageSharp
    {
        #region Tensor to ImageSharp

        public static __SIXLABORS.Image<__SIXLABORSPIXFMT.Rgb24> ToImageSharpRgb24<T>(this TensorBitmap<T> srcBitmap)
            where T:unmanaged, IConvertible
        {
            var dstImage = new __SIXLABORS.Image<__SIXLABORSPIXFMT.Rgb24>(srcBitmap.Width, srcBitmap.Height);            

            for (int y = 0; y < dstImage.Height; y++)
            {
                var dstRow = dstImage.DangerousGetPixelRowMemory(y).Span;

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    var srcPixel = srcBitmap.ScaledPixels.GetPixelUnchecked(x, y);
                    dstRow[x].FromVector4(srcPixel);
                }                
            }

            return dstImage;
        }

        public static void SaveToImageSharp<TPixel>(this ReadOnlySpanTensor2<TPixel> src, __FINFO dst)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            using (var img = src.ToImageSharp())
            {
                img.Save(dst.FullName);
            }
        }

        public static void SaveToImageSharp(this ReadOnlySpanTensor2<__XYZ> srcRGB, __FINFO dst)
        {
            using (var img = srcRGB.ToImageSharp<__SIXLABORSPIXFMT.Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        public static __SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<TPixel> src)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new __SIXLABORS.Image<TPixel>(src.BitmapSize.Width, src.BitmapSize.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static __SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<__XYZ> srcRGB)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new __SIXLABORS.Image<TPixel>(srcRGB.BitmapSize.Width, srcRGB.BitmapSize.Height);
            srcRGB.CopyTo(dst);
            return dst;
        }

        public static __SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<__XYZ> srcRGB, MultiplyAdd srcMAD)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new __SIXLABORS.Image<TPixel>(srcRGB.BitmapSize.Width, srcRGB.BitmapSize.Height);
            srcRGB.CopyTo(srcMAD, dst);
            return dst;
        }

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<TPixel> src, __SIXLABORS.Image<TPixel> dst)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(src.BitmapSize.Width, dst.Width);
            var h = Math.Min(src.BitmapSize.Height, dst.Height);

            for (int y = 0; y < h; y++)
            {
                var srcRow = src.GetRowSpan(y).Slice(0, w);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                srcRow.CopyTo(dstRow);
            }
        }

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<__XYZ> srcRGB, __SIXLABORS.Image<TPixel> dst)
           where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            CopyTo(srcRGB, MultiplyAdd.Identity, dst);
        }

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<__XYZ> srcRGB, MultiplyAdd srcMAD, __SIXLABORS.Image<TPixel> dst)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(srcRGB.BitmapSize.Width, dst.Width);
            var h = Math.Min(srcRGB.BitmapSize.Height, dst.Height);            

            var tmp = default(TPixel);

            for (int y = 0; y < h; y++)
            {
                var srcRow = srcRGB.GetRowSpan(y).Slice(0, w);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                __XYZW v4;

                if (srcMAD.IsIdentity)
                {
                    for (int x = 0; x < w; x++)
                    {
                        v4 = new __XYZW(srcRow[x], 1);
                        tmp.FromScaledVector4(v4);
                        dstRow[x] = tmp;
                    }
                }
                else
                {
                    for (int x = 0; x < w; x++)
                    {
                        v4 = new __XYZW(srcRow[x], 1);
                        v4 = srcMAD.Transform(v4);
                        tmp.FromScaledVector4(v4);
                        dstRow[x] = tmp;
                    }
                }                
            }
        }

        #endregion

        #region ImageSharp to Tensor

        public static void FitSixLaborsImage<TPixel>(this SpanTensor2<TPixel> dstTensor, __FINFO finfo)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            using(var srcImage = __SIXLABORS.Image.Load<TPixel>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitSixLaborsImage(this SpanTensor2<__XYZ> dstTensor, __FINFO finfo)
        {
            using (var srcImage = __SIXLABORS.Image.Load<__SIXLABORSPIXFMT.Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }        

        public static void FitImage<TPixel>(this SpanTensor2<TPixel> dstTensor, __SIXLABORS.Image<TPixel> srcImage)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (srcImage == null) throw new ArgumentNullException(nameof(srcImage));

            using (var disposable = _EnsureFitSize(srcImage, dstTensor.BitmapSize.Width, dstTensor.BitmapSize.Height, out var srcResized))
            {
                for (int y = 0; y < srcResized.Height; y++)
                {
                    var srcRow = srcResized.DangerousGetPixelRowMemory(y).Span;
                    var dstRow = dstTensor[y].Span;
                    srcRow.CopyTo(dstRow);
                }
            }
        }

        public static void FitImage<TPixel>(this SpanTensor2<__XYZ> dstTensor, __SIXLABORS.Image<TPixel> srcImage)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (srcImage == null) throw new ArgumentNullException(nameof(srcImage));

            using (var disposable = _EnsureFitSize(srcImage, dstTensor.BitmapSize.Width, dstTensor.BitmapSize.Height, out var srcResized))
            {
                for (int y = 0; y < srcResized.Height; y++)
                {
                    var srcRow = srcResized.DangerousGetPixelRowMemory(y).Span;
                    var dstRow = dstTensor[y].Span;

                    for (int x = 0; x < dstTensor.Dimensions[1]; ++x)
                    {
                        var val = srcRow[x].ToVector4();
                        dstRow[x] = new System.Numerics.Vector3(val.X, val.Y, val.Z);
                    }
                }
            }
        }

        private static __SIXLABORS.Image<TPixel> _EnsureFitSize<TPixel>(__SIXLABORS.Image<TPixel> srcImage, int dstW, int dstH, out __SIXLABORS.Image<TPixel> resized)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (srcImage.Width != dstW || srcImage.Height != dstH)
            {
                resized = srcImage.Clone(dc => dc.Resize(dstW, dstH, KnownResamplers.Lanczos5, false));
                return resized;
            }

            resized = srcImage;
            return null; // nothing to dispose
        }

        public static void CopyRowToScaled<TPixel>(this __SIXLABORS.Image<TPixel> srcImage, int y, Span<__XYZW> dst)
            where TPixel : unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var srcRow = srcImage.DangerousGetPixelRowMemory(y).Span;

            var len = Math.Min(srcRow.Length, dst.Length);
            dst = dst.Slice(0, len);

            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = srcRow[i].ToScaledVector4();
            }
        }

        #endregion
    }
}
