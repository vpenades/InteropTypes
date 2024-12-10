using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

using SIXLABORS = SixLabors.ImageSharp;
using SIXLABORSPIXFMT = SixLabors.ImageSharp.PixelFormats;

using XYZ = System.Numerics.Vector3;
using XYZW = System.Numerics.Vector4;

using FINFO = System.IO.FileInfo;

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

        public static SIXLABORS.Image<SIXLABORSPIXFMT.Rgb24> ToImageSharpRgb24<T>(this TensorBitmap<T> srcBitmap)
            where T:unmanaged, IConvertible
        {
            var dstImage = new SIXLABORS.Image<SIXLABORSPIXFMT.Rgb24>(srcBitmap.Width, srcBitmap.Height);            

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

        public static void SaveToImageSharp<TPixel>(this ReadOnlySpanTensor2<TPixel> src, FINFO dst)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            using (var img = src.ToImageSharp())
            {
                img.Save(dst.FullName);
            }
        }

        public static void SaveToImageSharp(this ReadOnlySpanTensor2<XYZ> srcRGB, FINFO dst)
        {
            using (var img = srcRGB.ToImageSharp<SIXLABORSPIXFMT.Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        public static SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<TPixel> src)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new SIXLABORS.Image<TPixel>(src.BitmapSize.Width, src.BitmapSize.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<XYZ> srcRGB)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new SIXLABORS.Image<TPixel>(srcRGB.BitmapSize.Width, srcRGB.BitmapSize.Height);
            srcRGB.CopyTo(dst);
            return dst;
        }

        public static SIXLABORS.Image<TPixel> ToImageSharp<TPixel>(this ReadOnlySpanTensor2<XYZ> srcRGB, MultiplyAdd srcMAD)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            var dst = new SIXLABORS.Image<TPixel>(srcRGB.BitmapSize.Width, srcRGB.BitmapSize.Height);
            srcRGB.CopyTo(srcMAD, dst);
            return dst;
        }

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<TPixel> src, SIXLABORS.Image<TPixel> dst)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
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

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<XYZ> srcRGB, SIXLABORS.Image<TPixel> dst)
           where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            CopyTo(srcRGB, MultiplyAdd.Identity, dst);
        }

        public static void CopyTo<TPixel>(this ReadOnlySpanTensor2<XYZ> srcRGB, MultiplyAdd srcMAD, SIXLABORS.Image<TPixel> dst)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(srcRGB.BitmapSize.Width, dst.Width);
            var h = Math.Min(srcRGB.BitmapSize.Height, dst.Height);            

            var tmp = default(TPixel);

            for (int y = 0; y < h; y++)
            {
                var srcRow = srcRGB.GetRowSpan(y).Slice(0, w);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                XYZW v4;

                if (srcMAD.IsIdentity)
                {
                    for (int x = 0; x < w; x++)
                    {
                        v4 = new XYZW(srcRow[x], 1);
                        tmp.FromScaledVector4(v4);
                        dstRow[x] = tmp;
                    }
                }
                else
                {
                    for (int x = 0; x < w; x++)
                    {
                        v4 = new XYZW(srcRow[x], 1);
                        v4 = srcMAD.Transform(v4);
                        tmp.FromScaledVector4(v4);
                        dstRow[x] = tmp;
                    }
                }                
            }
        }

        #endregion

        #region ImageSharp to Tensor

        public static void FitSixLaborsImage<TPixel>(this SpanTensor2<TPixel> dstTensor, FINFO finfo)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            using(var srcImage = SIXLABORS.Image.Load<TPixel>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitSixLaborsImage(this SpanTensor2<XYZ> dstTensor, FINFO finfo)
        {
            using (var srcImage = SIXLABORS.Image.Load<SIXLABORSPIXFMT.Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }        

        public static void FitImage<TPixel>(this SpanTensor2<TPixel> dstTensor, SIXLABORS.Image<TPixel> srcImage)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
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

        public static void FitImage<TPixel>(this SpanTensor2<XYZ> dstTensor, SIXLABORS.Image<TPixel> srcImage)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
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

        private static SIXLABORS.Image<TPixel> _EnsureFitSize<TPixel>(SIXLABORS.Image<TPixel> srcImage, int dstW, int dstH, out SIXLABORS.Image<TPixel> resized)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
        {
            if (srcImage.Width != dstW || srcImage.Height != dstH)
            {
                resized = srcImage.Clone(dc => dc.Resize(dstW, dstH, KnownResamplers.Lanczos5, false));
                return resized;
            }

            resized = srcImage;
            return null; // nothing to dispose
        }

        public static void CopyRowToScaled<TPixel>(this SIXLABORS.Image<TPixel> srcImage, int y, Span<XYZW> dst)
            where TPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TPixel>
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
