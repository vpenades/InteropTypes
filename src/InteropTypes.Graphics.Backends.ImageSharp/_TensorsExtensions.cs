using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

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
        #region source agent

        public static TensorBitmap<TDstElement>.IFactory CreateTensorBitmapFactory<TSrcPixel, TDstElement>(this Image<TSrcPixel> srcImage)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
            where TDstElement: unmanaged, IConvertible
        {
            if (srcImage == null) return null;

            var context = new _TensorBitmapFactory<TSrcPixel>(srcImage);

            return context as TensorBitmap<TDstElement>.IFactory;
        }

        internal static ColorEncoding _GetColorEncoding<TPixel>()
        {
            if (typeof(TPixel) == typeof(Rgb24)) return ColorEncoding.RGB;
            if (typeof(TPixel) == typeof(Bgr24)) return ColorEncoding.BGR;
            if (typeof(TPixel) == typeof(Argb32)) return ColorEncoding.ARGB;
            if (typeof(TPixel) == typeof(Bgra32)) return ColorEncoding.BGRA;
            if (typeof(TPixel) == typeof(Rgba32)) return ColorEncoding.RGBA;
            throw new NotImplementedException();
        }

        public static Image<Rgb24> ToImageSharpRgb24<T>(this TensorBitmap<T> srcBitmap)
            where T:unmanaged, IConvertible
        {
            var dstImage = new Image<Rgb24>(srcBitmap.Width, srcBitmap.Height);            

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

        #endregion

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

        #endregion        

        #region API

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

        public static void CopyRowToScaled<TPixel>(this Image<TPixel> srcImage, int y, Span<Vector4> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var srcRow = srcImage.DangerousGetPixelRowMemory(y).Span;

            var len = Math.Min(srcRow.Length, dst.Length);
            dst = dst.Slice(0, len);

            for(int i=0; i < dst.Length; i++)
            {
                dst[i] = srcRow[i].ToScaledVector4();
            }
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



        #endregion

        #region obsolete

        [Obsolete]
        public static Image<TPixel> ToImageSharp<TPixel>(this SpanPlanesBitmapRGB<float> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);
            src.CopyTo(dst);
            return dst;
        }

        [Obsolete]
        public static Image<TPixel> ToImageSharp<TPixel>(this SpanPlanesBitmapRGB<byte> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);
            src.CopyTo(dst);
            return dst;
        }

        [Obsolete]
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

                for (int x = 0; x < w; x++)
                {
                    var (r, g, b) = src.GetPixelUnchecked(x, y);
                    v4 = new System.Numerics.Vector4(r, g, b, 1);
                    tmp.FromScaledVector4(v4);
                    dstRow[x] = tmp;
                }
            }
        }

        [Obsolete]
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
                    var (r, g, b) = src.GetPixelUnchecked(x, y);
                    rgb.R = r;
                    rgb.G = g;
                    rgb.B = b;
                    tmp.FromRgb24(rgb);
                    dstRow[x] = tmp;
                }
            }
        }

        [Obsolete]
        public static void FitSixLaborsImage(this SpanPlanesBitmapRGB<float> dstTensor, FINFO finfo)
        {
            using (var srcImage = Image.Load<Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        [Obsolete]
        public static void FitSixLaborsImage(this SpanPlanesBitmapRGB<byte> dstTensor, FINFO finfo)
        {
            using (var srcImage = Image.Load<Rgb24>(finfo.FullName))
            {
                dstTensor.FitImage(srcImage);
            }
        }

        [Obsolete]
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
                        dstTensor.SetPixelUnchecked(x, y, val.X, val.Y, val.Z);
                    }
                }
            }
        }

        [Obsolete]
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
                        dstTensor.SetPixelUnchecked(x, y, tmp.R, tmp.G, tmp.B);
                    }
                }
            }
        }

        [Obsolete]
        public static void SaveToImageSharp(this SpanPlanesBitmapRGB<float> src, FINFO dst)
        {
            using (var img = src.ToImageSharp<Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        [Obsolete]
        public static void SaveToImageSharp(this SpanPlanesBitmapRGB<byte> src, FINFO dst)
        {
            using (var img = src.ToImageSharp<Rgb24>())
            {
                img.Save(dst.FullName);
            }
        }

        #endregion

        #region extras

        public static void DrawSpriteTo<TDstPixel, TSrcPixel>(Image<TDstPixel> target, Matrix3x2 spriteTransform, Image<TSrcPixel> sprite)
            where TDstPixel : unmanaged, IPixel<TDstPixel>
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
        {
            _Implementation.DrawSpriteTo(target, spriteTransform, sprite);
        }        

        #endregion
    }

    class _TensorBitmapFactory<TSrcPixel>
        : TensorBitmap<Byte>.IFactory
        , TensorBitmap<float>.IFactory
        where TSrcPixel : unmanaged, IPixel<TSrcPixel>
        
    {
        #region lifecycle
        public _TensorBitmapFactory(Image<TSrcPixel> source)
        {
            _Source = source;
            OriginalColorEncoding = ImageSharpToolkit._GetColorEncoding<TSrcPixel>();
        }

        #endregion

        #region data

        private readonly Image<TSrcPixel> _Source;

        private bool _CacheNext;

        private readonly Dictionary<_DerivedKey, _DerivedImage> _DerivedImages = new Dictionary<_DerivedKey, _DerivedImage>();

        private ColorEncoding OriginalColorEncoding { get; }

        public (int Width, int Height) OriginalSize => (_Source.Width, _Source.Height);

        #endregion

        #region properties

        public bool UseImageSharpResize { get; set; } = true;

        #endregion

        #region API

        public void CacheNext()
        {
            _CacheNext = true;
        }

        public bool TryFitPixelsToTensorBitmap(TensorBitmap<byte> dst, Matrix3x2 xform)
        {
            return _TryFitPixelsToTensorBitmap(dst, xform);
        }

        public bool TryFitPixelsToTensorBitmap(TensorBitmap<float> dst, Matrix3x2 xform)
        {
            return _TryFitPixelsToTensorBitmap(dst, xform);
        }

        private  bool _TryFitPixelsToTensorBitmap<TDstElement>(TensorBitmap<TDstElement> dst, Matrix3x2 xform)
            where TDstElement : unmanaged, IConvertible
        {
            var cacheNext = _CacheNext;
            _CacheNext = false;

            if (_Source == null) return false;

            var key = new _DerivedKey(xform, dst.Width, dst.Height);

            ISourceImage source = null;

            // TODO: if we have a derived image with the same scale factor, we could derive a cropped version of it.

            if (_DerivedImages.TryGetValue(key, out var derivedSource))
            {
                source = derivedSource;
                cacheNext = false;
            }

            source ??= _CreateDerivedImage(key);
            
            if (source == null) return false;

            if (cacheNext && source is _DerivedImage derived)
            {
                _DerivedImages[key] = derived;
            }

            source.TransferTo(dst.ScaledPixels);

            return true;
        }
        
        private ISourceImage _CreateDerivedImage(_DerivedKey key)
        {
            var xform = key.SourceTransform;

            if (xform.IsIdentity) return new _WrappedImage(_Source);            

            if (UseImageSharpResize && xform.M21 == 0 && xform.M12 == 0) // no rotation, just scale and translation
            {
                var dstW = this.OriginalSize.Width * xform.M11;
                var dstH = this.OriginalSize.Height * xform.M22;
                var dstS = new Size((int)dstW, (int)dstH);
                var dstP = new Point((int)xform.M31, (int)xform.M32);

                var options = new ResizeOptions();
                options.Mode = ResizeMode.Manual;
                options.Size = new Size(key.DstWidth, key.DstHeight);
                options.TargetRectangle = new Rectangle(dstP, dstS);

                using (var cloned = _Source.Clone(dc => dc.Resize(options)))
                {
                    return new _DerivedImage(cloned);
                }                
            }

            // full transform

            return new _DerivedImage(key.DstWidth, key.DstHeight, xform, _Source);
        }

        #endregion

        #region nested types

        readonly struct _DerivedKey
        {
            public _DerivedKey(Matrix3x2 x, int w, int h)
            {
                SourceTransform = x;
                DstWidth = w;
                DstHeight = h;
            }

            public readonly System.Numerics.Matrix3x2 SourceTransform;
            public readonly int DstWidth;
            public readonly int DstHeight;
        }

        interface ISourceImage
        {
            void TransferTo<TDstElement>(TensorBitmap<TDstElement>.ScaledPixelsAccessor dst) where TDstElement : unmanaged, IConvertible;
        }

        class _WrappedImage : ISourceImage
        {
            public _WrappedImage(Image<TSrcPixel> src) { _Source = src; }

            private readonly Image<TSrcPixel> _Source;

            public unsafe void TransferTo<TDstElement>(TensorBitmap<TDstElement>.ScaledPixelsAccessor dst) where TDstElement : unmanaged, IConvertible
            {
                if (sizeof(TSrcPixel) > 4) throw new InvalidOperationException(nameof(TSrcPixel));

                var minHeight = Math.Min(_Source.Height, dst.Height);
                var minWidth = Math.Min(_Source.Width, dst.Width);

                Span<Vector4> transferRow = stackalloc Vector4[minWidth];

                for (int y = 0; y < minHeight; y++)
                {
                    _Source.CopyRowToScaled(y, transferRow);
                    dst.SetRowPixels(y, transferRow);
                }
            }
        }

        class _DerivedImage : ISourceImage
        {
            public _DerivedImage(int w, int h)
            {
                Width = w;
                Height = h;
                _Data = new TSrcPixel[Width * Height];
            }

            public _DerivedImage(Image<TSrcPixel> src) : this(src.Width,src.Height)
            {
                for (int i=0; i<src.Height; i++)
                {
                    var srcRow = src.DangerousGetPixelRowMemory(i).Span;
                    var dstRow = _Data.AsSpan(i * Width, Width);
                    srcRow.CopyTo(dstRow);
                }
            }

            public _DerivedImage(int w, int h, Matrix3x2 srcXform, Image<TSrcPixel> src) : this(w,h)
            {
                Matrix3x2.Invert(srcXform, out var srcInverse);

                var srcStride = src.Width;
                var srcMaxX = src.Width - 1;
                var srcMaxY = src.Height - 1;

                if (src.DangerousTryGetSinglePixelMemory(out var singleBuffer))
                {
                    var srcSpan = singleBuffer.Span;

                    for (int y = 0; y < Height; y++)
                    {
                        var dstRow = _Data.AsSpan(y*Width, Width);

                        for(int x =0; x < Width; x++)
                        {
                            var v = new System.Numerics.Vector2(x,y);
                            v = Vector2.Transform(v, srcInverse);

                            var xx = Math.Clamp((int)v.X, 0, srcMaxX);
                            var yy = Math.Clamp((int)v.Y, 0, srcMaxY);

                            var srcRow = srcSpan.Slice(yy * srcStride, srcStride);

                            // TODO: bilinear sampling

                            dstRow[x] = srcRow[xx];
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var dstRow = _Data.AsSpan(y * Width, Width);

                        for (int x = 0; x < Width; x++)
                        {
                            var v = new System.Numerics.Vector2(x, y);
                            v = Vector2.Transform(v, srcInverse);

                            var xx = Math.Clamp((int)v.X, 0, srcMaxX);
                            var yy = Math.Clamp((int)v.Y, 0, srcMaxY);

                            var srcRow = src.DangerousGetPixelRowMemory(yy).Span;

                            // TODO: bilinear sampling

                            dstRow[x] = srcRow[xx];
                        }
                    }
                }                
            }

            public int Width { get; }
            public int Height { get; }

            private readonly TSrcPixel[] _Data;

            public unsafe void TransferTo<TDstElement>(TensorBitmap<TDstElement>.ScaledPixelsAccessor dst) where TDstElement : unmanaged, IConvertible
            {
                if (sizeof(TSrcPixel) > 4) throw new InvalidOperationException(nameof(TSrcPixel));

                var minHeight = Math.Min(this.Height, dst.Height);
                var minWidth = Math.Min(this.Width, dst.Width);

                Span<Vector4> transferRow = stackalloc Vector4[minWidth];

                for (int y = 0; y < minHeight; y++)
                {
                    var srcRow = _Data.AsSpan(y * Width, minWidth);

                    for(int x = 0; x < transferRow.Length; ++x)
                    {
                        transferRow[x] = srcRow[x].ToScaledVector4();
                    }                    
                    
                    dst.SetRowPixels(y, transferRow);
                }
            }
        }        

        #endregion
    }
}
