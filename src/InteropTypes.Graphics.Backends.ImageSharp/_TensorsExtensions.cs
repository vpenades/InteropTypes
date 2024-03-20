using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Tensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using InteropTypes.Tensors.Imaging;

using FINFO = System.IO.FileInfo;

namespace InteropTypes.Graphics.Backends
{
    public static partial class ImageSharpToolkit
    {
        #region ImageSharp to Tensor

        public static void FitSixLaborsImage<TPixel>(this SpanTensor2<TPixel> dstTensor, FINFO finfo)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using(var srcImage = Image.Load<TPixel>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitSixLaborsImage(this SpanTensor2<System.Numerics.Vector3> dstTensor, FINFO finfo)            
        {
            using (var srcImage = Image.Load<Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitSixLaborsImage(this SpanPlanesBitmapRGB<float> dstTensor, FINFO finfo)
        {
            using (var srcImage = Image.Load<Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitSixLaborsImage(this SpanPlanesBitmapRGB<byte> dstTensor, FINFO finfo)
        {
            using (var srcImage = Image.Load<Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        public static void FitImage<TPixel>(this SpanTensor2<TPixel> dstTensor, Image<TPixel> srcImage)
            where TPixel : unmanaged, IPixel<TPixel>
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

        public static void FitImage<TPixel>(this SpanTensor2<System.Numerics.Vector3> dstTensor, Image<TPixel> srcImage)
            where TPixel : unmanaged, IPixel<TPixel>
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

        public static void FitImage<TPixel>(this SpanPlanesBitmapRGB<Single> dstTensor, Image<TPixel> srcImage)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (srcImage == null) throw new ArgumentNullException(nameof(srcImage));

            using (var disposable = _EnsureFitSize(srcImage, dstTensor.Width, dstTensor.Height, out var srcResized))
            {
                for (int y = 0; y < srcResized.Height; y++)
                {
                    var srcRow = srcResized.DangerousGetPixelRowMemory(y).Span;

                    for (int x = 0; x < dstTensor.Width; ++x)
                    {
                        var val = srcRow[x].ToVector4();
                        dstTensor.SetPixel(x, y, val.X, val.Y, val.Z);
                    }
                }
            }
        }

        public static void FitImage<TPixel>(this SpanPlanesBitmapRGB<Byte> dstTensor, Image<TPixel> srcImage)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (srcImage == null) throw new ArgumentNullException(nameof(srcImage));

            Rgba32 tmp = default;

            using (var disposable = _EnsureFitSize(srcImage, dstTensor.Width, dstTensor.Height, out var srcResized))
            {
                for (int y = 0; y < srcResized.Height; y++)
                {
                    var srcRow = srcResized.DangerousGetPixelRowMemory(y).Span;

                    for (int x = 0; x < dstTensor.Width; ++x)
                    {
                        srcRow[x].ToRgba32(ref tmp);
                        dstTensor.SetPixel(x, y, tmp.R, tmp.G, tmp.B);
                    }
                }
            }
        }

        private static Image<TPixel> _EnsureFitSize<TPixel>(Image<TPixel> srcImage, int dstW, int dstH, out Image<TPixel> resized)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (srcImage.Width != dstW || srcImage.Height != dstH)
            {
                resized = srcImage.Clone(dc => dc.Resize(dstW, dstH, KnownResamplers.Lanczos5, false));
                return resized;
            }

            resized = srcImage;
            return null; // nothing to dispose
        }

        #endregion

        #region Tensor to ImageSharp

        public static void SaveToImageSharp<TPixel>(this SpanTensor2<TPixel> src, FINFO dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var img = src.ToImageSharp())
            {
                img.Save(dst.FullName);
            }
        }

        public static void SaveToImageSharp(this SpanTensor2<System.Numerics.Vector3> src, FINFO dst)
        {
            using (var img = src.ToImageSharp<Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        public static void SaveToImageSharp(this SpanPlanesBitmapRGB<float> src, FINFO dst)
        {
            using (var img = src.ToImageSharp<Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        public static void SaveToImageSharp(this SpanPlanesBitmapRGB<byte> src, FINFO dst)
        {
            using (var img = src.ToImageSharp<Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        public static Image<TPixel> ToImageSharp<TPixel>(this SpanTensor2<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.BitmapSize.Width, src.BitmapSize.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static Image<TPixel> ToImageSharp<TPixel>(this SpanTensor2<System.Numerics.Vector3> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.BitmapSize.Width, src.BitmapSize.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static Image<TPixel> ToImageSharp<TPixel>(this SpanPlanesBitmapRGB<float> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static Image<TPixel> ToImageSharp<TPixel>(this SpanPlanesBitmapRGB<byte> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);
            src.CopyTo(dst);
            return dst;
        }

        public static void CopyTo<TPixel>(this SpanTensor2<TPixel> src, Image<TPixel> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(src.BitmapSize.Width, dst.Width);
            var h = Math.Min(src.BitmapSize.Height, dst.Height);

            for (int y = 0; y < h; y++)
            {
                var srcRow = src.GetRowSpan(y).Slice(0,w);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                srcRow.CopyTo(dstRow);
            }            
        }

        public static void CopyTo<TPixel>(this SpanTensor2<System.Numerics.Vector3> src, Image<TPixel> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(src.BitmapSize.Width, dst.Width);
            var h = Math.Min(src.BitmapSize.Height, dst.Height);

            var tmp = default(TPixel);

            for (int y = 0; y < h; y++)
            {
                var srcRow = src.GetRowSpan(y).Slice(0, w);
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                System.Numerics.Vector4 v4;

                for (int x = 0; x < w; x++)
                {
                    v4 = new System.Numerics.Vector4(srcRow[x], 1);
                    tmp.FromScaledVector4(v4);
                    dstRow[x] = tmp;
                }
            }
        }

        public static void CopyTo<TPixel>(this SpanPlanesBitmapRGB<float> src, Image<TPixel> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(src.Width, dst.Width);
            var h = Math.Min(src.Height, dst.Height);

            var tmp = default(TPixel);

            for (int y = 0; y < h; y++)
            {                
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                System.Numerics.Vector4 v4;

                for (int x=0; x < w; x++)
                {
                    var (r,g,b) = src.GetPixel(x, y);
                    v4 = new System.Numerics.Vector4(r,g,b,1);
                    tmp.FromScaledVector4(v4);
                    dstRow[x] = tmp;
                }                
            }
        }

        public static void CopyTo<TPixel>(this SpanPlanesBitmapRGB<byte> src, Image<TPixel> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));

            var w = Math.Min(src.Width, dst.Width);
            var h = Math.Min(src.Height, dst.Height);

            var tmp = default(TPixel);

            for (int y = 0; y < h; y++)
            {
                var dstRow = dst.DangerousGetPixelRowMemory(y).Span.Slice(0, w);

                Rgb24 rgb = default;

                for (int x = 0; x < w; x++)
                {
                    var (r, g, b) = src.GetPixel(x, y);
                    rgb.R = r;
                    rgb.G = g;
                    rgb.B = b;                    
                    tmp.FromRgb24(rgb);
                    dstRow[x] = tmp;
                }
            }
        }

        #endregion
    }
}
