using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

using SIXLABORSPIXFMT = SixLabors.ImageSharp.PixelFormats;

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
        #region API

        public static TensorBitmap<TDstElement>.IFactory CreateTensorBitmapFactory<TSrcPixel, TDstElement>(this Image<TSrcPixel> srcImage)
            where TSrcPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TSrcPixel>
            where TDstElement : unmanaged, IConvertible
        {
            if (srcImage == null) return null;

            var context = new _TensorBitmapFactory<TSrcPixel>(srcImage);

            return context as TensorBitmap<TDstElement>.IFactory;
        }

        #endregion

        #region nested types

        sealed class _TensorBitmapFactory<TSrcPixel>
            : TensorBitmap<Byte>.IFactory
            , TensorBitmap<float>.IFactory
            where TSrcPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TSrcPixel>
        {
            #region lifecycle
            public _TensorBitmapFactory(Image<TSrcPixel> source)
            {
                _Source = source;
                OriginalColorEncoding = _GetColorEncoding<TSrcPixel>();
            }

            internal static ColorEncoding _GetColorEncoding<TPixel>()
            {
                if (typeof(TPixel) == typeof(SIXLABORSPIXFMT.Rgb24)) return ColorEncoding.RGB;
                if (typeof(TPixel) == typeof(SIXLABORSPIXFMT.Bgr24)) return ColorEncoding.BGR;
                if (typeof(TPixel) == typeof(SIXLABORSPIXFMT.Argb32)) return ColorEncoding.ARGB;
                if (typeof(TPixel) == typeof(SIXLABORSPIXFMT.Bgra32)) return ColorEncoding.BGRA;
                if (typeof(TPixel) == typeof(SIXLABORSPIXFMT.Rgba32)) return ColorEncoding.RGBA;
                throw new NotImplementedException($"{typeof(TPixel).Name} not supported");
            }

            #endregion

            #region data

            private readonly Image<TSrcPixel> _Source;

            private bool _CacheNext;

            private readonly Dictionary<_DerivedKey, _DerivedImage<TSrcPixel>> _DerivedImages = new Dictionary<_DerivedKey, _DerivedImage<TSrcPixel>>();

            private ColorEncoding OriginalColorEncoding { get; }

            public (int Width, int Height) OriginalSize => (_Source.Width, _Source.Height);

            #endregion

            #region properties

            public bool UseImageSharpResize { get; set; } = true;

            public bool UseSixLaborsRotation { get; set; } = true;

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

            private bool _TryFitPixelsToTensorBitmap<TDstElement>(TensorBitmap<TDstElement> dst, Matrix3x2 xform)
                where TDstElement : unmanaged, IConvertible
            {
                var cacheNext = _CacheNext;
                _CacheNext = false;

                if (_Source == null) return false;

                var key = new _DerivedKey(xform, dst.Width, dst.Height);

                _ISourceImage source = null;

                // TODO: if we have a derived image with the same scale factor, we could derive a cropped version of it.

                if (_DerivedImages.TryGetValue(key, out var derivedSource))
                {
                    source = derivedSource;
                    cacheNext = false;
                }

                source ??= _CreateDerivedImage(key);

                if (source == null) return false;

                if (cacheNext && source is _DerivedImage<TSrcPixel> derived)
                {
                    _DerivedImages[key] = derived;
                }

                source.TransferTo(dst.ScaledPixels);

                return true;
            }

            private _ISourceImage _CreateDerivedImage(_DerivedKey key)
            {
                var xform = key.SourceTransform;

                if (xform.IsIdentity) return new _WrappedImage<TSrcPixel>(_Source);

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
                        return new _DerivedImage<TSrcPixel>(cloned);
                    }
                }

                if (UseSixLaborsRotation)
                {
                    // full transform with Sixlabors            
                    using (var cloned = _Source.CloneTransformed(new System.Drawing.Size(key.DstWidth, key.DstHeight), xform))
                    {
                        return new _DerivedImage<TSrcPixel>(cloned);
                    }
                }
                else
                {
                    // full transform
                    return new _DerivedImage<TSrcPixel>(key.DstWidth, key.DstHeight, xform, _Source);
                }
            }

            #endregion            
        }

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

        interface _ISourceImage
        {
            void TransferTo<TDstElement>(TensorBitmap<TDstElement>.ScaledPixelsAccessor dst) where TDstElement : unmanaged, IConvertible;
        }

        sealed class _WrappedImage<TSrcPixel> : _ISourceImage
            where TSrcPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TSrcPixel>
        {
            #region lifecycle
            public _WrappedImage(Image<TSrcPixel> src) { _Source = src; }
            #endregion

            #region data

            private readonly Image<TSrcPixel> _Source;

            #endregion

            #region API
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

            #endregion
        }

        sealed class _DerivedImage<TSrcPixel> : _ISourceImage
            where TSrcPixel : unmanaged, SIXLABORSPIXFMT.IPixel<TSrcPixel>
        {
            #region lifecycle
            public _DerivedImage(int w, int h)
            {
                Width = w;
                Height = h;
                _Data = new TSrcPixel[Width * Height];
            }

            public _DerivedImage(Image<TSrcPixel> src) : this(src.Width, src.Height)
            {
                for (int i = 0; i < src.Height; i++)
                {
                    var srcRow = src.DangerousGetPixelRowMemory(i).Span;
                    var dstRow = _Data.AsSpan(i * Width, Width);
                    srcRow.CopyTo(dstRow);
                }
            }

            public _DerivedImage(int w, int h, Matrix3x2 srcXform, Image<TSrcPixel> src) : this(w, h)
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
                        var dstRow = _Data.AsSpan(y * Width, Width);

                        for (int x = 0; x < Width; x++)
                        {
                            var v = new System.Numerics.Vector2(x, y);
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
            #endregion

            #region data

            public int Width { get; }
            public int Height { get; }

            private readonly TSrcPixel[] _Data;

            #endregion

            #region API

            public unsafe void TransferTo<TDstElement>(TensorBitmap<TDstElement>.ScaledPixelsAccessor dst) where TDstElement : unmanaged, IConvertible
            {
                if (sizeof(TSrcPixel) > 4) throw new InvalidOperationException(nameof(TSrcPixel));

                var minHeight = Math.Min(this.Height, dst.Height);
                var minWidth = Math.Min(this.Width, dst.Width);

                Span<Vector4> transferRow = stackalloc Vector4[minWidth];

                for (int y = 0; y < minHeight; y++)
                {
                    var srcRow = _Data.AsSpan(y * Width, minWidth);

                    for (int x = 0; x < transferRow.Length; ++x)
                    {
                        transferRow[x] = srcRow[x].ToScaledVector4();
                    }

                    dst.SetRowPixels(y, transferRow);
                }
            }

            #endregion
        }

        #endregion
    }


}
