using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

using InteropTypes.Tensors.Imaging;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor3<T>
    {
        public void FillFrom<TSrc>(ReadOnlySpanTensor3<TSrc> src, MultiplyAdd mad)
            where TSrc: unmanaged
        {
            if (typeof(T) == typeof(float))
            {
                var dstSingles = this.Cast<float>();

                if (typeof(TSrc) == typeof(float))
                {
                    _SpanTensor3Toolkit.Transfer(src.Cast<float>(), dstSingles, mad);
                    return;
                }

                if (typeof(TSrc) == typeof(Byte))
                {
                    _SpanTensor3Toolkit.Transfer(src.Cast<byte>(), dstSingles, mad);
                    return;
                }
            }

            throw new NotImplementedException();
        }

        
    }


    static class _SpanTensor3Toolkit
    {
        public static void Transfer(ReadOnlySpanTensor3<float> src, SpanTensor3<float> dst, MultiplyAdd mad)
        {
            if (src.Dimensions != dst.Dimensions) throw new ArgumentException("Dimensions mismatch", nameof(dst));

            switch (src.Dimensions.Dim2)
            {
                case 4:
                    {
                        var (mul, add) = mad.GetVector4();
                        src.UpCast<System.Numerics.Vector4>().Span.MultiplyAddTo(mul, add, dst.UpCast<System.Numerics.Vector4>().Span);
                        break;
                    }

                case 3:
                    {
                        var (mul, add) = mad.GetVector3();
                        src.UpCast<System.Numerics.Vector3>().Span.MultiplyAddTo(mul, add, dst.UpCast<System.Numerics.Vector3>().Span);
                        break;
                    }
                case 1:
                    {
                        var (mul, add) = mad.GetScalar();
                        src.UpCast<float>().Span.MultiplyAddTo(mul, add, dst.UpCast<float>().Span);
                        break;
                    }
                default: throw new ArgumentException("invalid dimensions", nameof(src));
            }
        }

        public static void Transfer(ReadOnlySpanTensor3<byte> src, SpanTensor3<float> dst, MultiplyAdd mad)
        {
            if (src.Dimensions != dst.Dimensions) throw new ArgumentException();

            var srcBytes = src.Cast<Byte>().Span;

            switch (src.Dimensions.Dim2)
            {
                case 4:
                    {
                        var (mul, add) = mad.GetVector4();
                        srcBytes.ScaledMultiplyAddTo(mul, add, dst.UpCast<System.Numerics.Vector4>().Span);
                        break;
                    }

                case 3:
                    {
                        var (mul, add) = mad.GetVector3();
                        srcBytes.ScaledMultiplyAddTo(mul, add, dst.UpCast<System.Numerics.Vector3>().Span);
                        break;
                    }
                case 1:
                    {
                        var (mul, add) = mad.GetScalar();
                        srcBytes.ScaledMultiplyAddTo(mul, add, dst.UpCast<float>().Span);
                        break;
                    }
                default: throw new ArgumentException("invalid dimensions", nameof(src));
            }
        }

        #region nested types

        public static ScaledInterleavedPixelsAccessor<T> CreateScaledInterleavedPixelsAccessor<T>(this SpanTensor3<T> tensor, ColorEncoding encoding, ColorRanges ranges)
            where T:unmanaged,IConvertible
        {
            return new ScaledInterleavedPixelsAccessor<T>(tensor, encoding, ranges);
        }

        /// <summary>
        /// Represents an accessor that allows reading and writing individual pixels in a standarized Vector4-as-RGBA style.
        /// </summary>
        /// <remarks>
        /// Use <see cref="TensorBitmap{T}.ScaledPixels"/> for access.
        /// </remarks>
        public readonly ref struct ScaledInterleavedPixelsAccessor<T>
            where T:unmanaged,IConvertible
        {
            #region lifecycle
            internal ScaledInterleavedPixelsAccessor(SpanTensor3<T> tensor, ColorEncoding encoding, ColorRanges ranges)
            {
                if (tensor.Dimensions.Dim2 != encoding.GetChannelCount()) throw new ArgumentException("invalid format", nameof(encoding));

                _Tensor = tensor.Span;
                Width = tensor.Dimensions.Dim1;
                Height = tensor.Dimensions.Dim0;
                _Stride = tensor.Dimensions.Dim0 * tensor.Dimensions.Dim2;
                _PixelSize = tensor.Dimensions.Dim2;
                _Encoding = encoding;
                _Forward = ranges.ToMultiplyAdd();
                _Inverse = _Forward.GetInverse();
            }

            #endregion

            #region data

            private readonly Span<T> _Tensor;
            private readonly int _Stride;
            private readonly int _PixelSize;
            private readonly ColorEncoding _Encoding;
            private readonly MultiplyAdd _Forward;
            private readonly MultiplyAdd _Inverse;

            #endregion

            #region properties
            public int Width { get; }
            public int Height { get; }

            #endregion

            #region API            

            public Vector4 GetPixel(int x, int y)
            {
                x = Math.Clamp(x, 0, Width - 1);
                y = Math.Clamp(y, 0, Height - 1);
                return GetPixelUnchecked(x, y);
            }
            public void SetPixel(int x, int y, Vector4 value)
            {
                if (x < 0) return;
                if (x >= Width) return;

                if (y < 0) return;
                if (y >= Height) return;

                SetPixelUnchecked(x, y, value);
            }

            public void GetRowPixels(int y, Span<Vector4> dst)
            {
                var srcRow = _Tensor.Slice(_Stride * y, _Stride);

                if (_Inverse.IsIdentity)
                {
                    for (int x = 0; x < this.Width; ++x)
                    {
                        var pixel = srcRow.Slice(x * _PixelSize, _PixelSize);
                        dst[x] = _Encoding.ToScaledPixel(pixel);
                    }
                }
                else
                {
                    for (int x = 0; x < this.Width; ++x)
                    {
                        var pixel = srcRow.Slice(x * _PixelSize, _PixelSize);
                        var value = _Encoding.ToScaledPixel(pixel);
                        dst[x] = _Inverse.Transform(value);
                    }
                }
            }
            public void SetRowPixels(int y, ReadOnlySpan<Vector4> src)
            {
                var dstRow = _Tensor.Slice(_Stride * y, _Stride);

                if (_Forward.IsIdentity)
                {
                    for (int x = 0; x < this.Width; ++x)
                    {
                        var pixel = dstRow.Slice(x * _PixelSize, _PixelSize);
                        src[x].CopyScaledPixelTo(pixel, _Encoding);
                    }
                }
                else
                {
                    for (int x = 0; x < this.Width; ++x)
                    {
                        var value = _Forward.Transform(src[x]);

                        var pixel = dstRow.Slice(x * _PixelSize, _PixelSize);
                        value.CopyScaledPixelTo(pixel, _Encoding);
                    }
                }
            }

            public Vector4 GetPixelUnchecked(int x, int y)
            {
                var pixelSpan = _Tensor.Slice(y * _Stride + x, _PixelSize);

                return _Inverse.Transform(_Encoding.ToScaledPixel(pixelSpan));
            }
            public void SetPixelUnchecked(int x, int y, Vector4 value)
            {
                var pixelSpan = _Tensor.Slice(y * _Stride + x, _PixelSize);

                _Forward.Transform(value).CopyScaledPixelTo(pixelSpan, _Encoding);
            }

            #endregion
        }

        #endregion
    }
}
