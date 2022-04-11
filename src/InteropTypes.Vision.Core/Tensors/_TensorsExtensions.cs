using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using MEMMARSHAL = System.Runtime.InteropServices.MemoryMarshal;

using TENSOR2V3 = InteropTypes.Tensors.SpanTensor2<System.Numerics.Vector3>;
using TENSOR4FLOAT = InteropTypes.Tensors.SpanTensor4<float>;

namespace InteropTypes.Tensors
{
    public static class _TensorsExtensions
    {
        public static bool TryGetAsSpanTensor<TPixel>(this SpanBitmap<TPixel> src, out SpanTensor2<TPixel> result)
            where TPixel : unmanaged
        {
            if (!src.Info.IsContinuous) { result = default; return false; }

            var data = MEMMARSHAL.Cast<Byte, TPixel>(src.WritableBytes);

            result = new SpanTensor2<TPixel>(data, src.Height, src.Width);
            return true;
        }

        public static unsafe void FillBitmap<TSrcPixel>(this TENSOR2V3 dst, SpanBitmap<TSrcPixel> src, in Matrix3x2 srcXform, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel:unmanaged
        {
            var data = src.ReadableBytes;
            var w = src.Width;
            var h = src.Height;
            var stride = src.Info.StepByteSize;

            // TODO: support RGB component swapping.

            if (typeof(TSrcPixel) == typeof(Vector3)) { dst.FillPixelsXYZ96F(data, stride, w, h, srcXform, mad, useBilinear); return; }

            if (typeof(TSrcPixel) == typeof(Pixel.BGR96F)) { dst.FillPixelsXYZ96F(data, stride, w, h, srcXform, mad, useBilinear); return; }
            if (typeof(TSrcPixel) == typeof(Pixel.RGB96F)) { dst.FillPixelsXYZ96F(data, stride, w, h, srcXform, mad, useBilinear); return; }

            if (typeof(TSrcPixel) == typeof(Pixel.BGR24)) { dst.FillPixelsXYZ24(data, stride, w, h, srcXform, mad, useBilinear); return; }
            if (typeof(TSrcPixel) == typeof(Pixel.RGB24)) { dst.FillPixelsXYZ24(data, stride, w, h, srcXform, mad, useBilinear); return; }

            if (sizeof(TSrcPixel) == 3) { dst.FillPixelsXYZ24(data, stride, w, h, srcXform, mad, useBilinear); return; }
        }

        public static void FitBitmap(this TENSOR2V3 dst, SpanBitmap src)
        {
            var dstData = MEMMARSHAL.Cast<Vector3, byte>(dst.Span);
            var dstBmp = new SpanBitmap(dstData, dst.Dimensions[1], dst.Dimensions[0], Pixel.BGR96F.Format);

            if (src.PixelFormat.IsFloating)
            {
                dstBmp.FitPixels(src);
            }
            else
            {
                BitmapsToolkit.FitPixels(src, dstBmp, (0, 1f / 255f));
            }
        }

        public static bool TryGetBitmapGray(this IDenseTensor<float> src, out SpanBitmap<float> bmp)
        {
            bmp = default;

            var dims = src.Dimensions;

            if (dims.Length < 3 || dims.Length > 4) return false;
            if (dims.Length == 4)
            {
                if (dims[0] != 1) return false;
                dims = dims.Slice(1);
            }

            if (dims[2] != 1) return false;

            var data = MEMMARSHAL.Cast<float, byte>(src.Span);
            bmp = new SpanBitmap<float>(data, dims[1], dims[0], Pixel.Luminance32F.Format);
            return true;
        }

        public static bool TryGetBitmapBGR(this IDenseTensor<float> src, out SpanBitmap<Vector3> bmp)
        {
            return _Implementation.TryGetBitmapBGR(src, out bmp);
        }


        public static MemoryBitmap<Byte> CreateGrayBitmapQuantized(this TENSOR4FLOAT tensor, Func<Single[], Single> pixelFunc)
        {
            var src = tensor.CreateGrayBitmap(pixelFunc);
            var (min, max) = SpanBitmap.MinMax(src);

            var dst = new MemoryBitmap<Byte>(src.Width, src.Height, Pixel.Luminance8.Format);

            SpanBitmap.CopyPixels(src, dst, (-min, 255.0f / (max - min)), (0, 255));

            return dst;
        }

        public static MemoryBitmap<Single> CreateGrayBitmap(this TENSOR4FLOAT tensor4, Func<Single[], Single> pixelFunc)
        {
            var tensor = tensor4[0];

            var memory = new MemoryBitmap<Single>(tensor.Dimensions[1], tensor.Dimensions[0]);

            var pix = new Single[tensor.Dimensions[2]];

            for (int y = 0; y < tensor.Dimensions[0]; ++y)
            {
                var row = tensor.GetSubTensor(y);

                for (int x = 0; x < row.Dimensions[0]; ++x)
                {
                    var pixel = row.GetSubTensor(x);

                    pixel.Span.CopyTo(pix);

                    var col = pixelFunc(pix);

                    memory.SetPixel(x, y, col);
                }
            }

            return memory;
        }


        public static MemoryBitmap<Vector3> CreateBGRBitmap(this TENSOR4FLOAT tensor, Func<Single[], Vector3> pixelFunc)
        {
            return _Implementation.CreateBGRBitmap(tensor, pixelFunc);
        }


        public static MemoryBitmap<Byte> CreateGrayBitmap(this TENSOR4FLOAT tensor)
        {
            var channel = tensor.CreateGrayBitmap(pixel => pixel.Sum());
            var (min, max) = SpanBitmap.MinMax(channel);

            var gray = new MemoryBitmap<Byte>(channel.Width, channel.Height, Pixel.Luminance8.Format);
            SpanBitmap.CopyPixels(channel, gray, (-min, 255.0f / (max - min)), (0, 255));

            return gray;
        }


        public static MemoryBitmap CreateBGRBitmap(this TENSOR4FLOAT tensor)
        {
            var channel = tensor.CreateBGRBitmap(pixel => new Vector3(pixel[0], pixel[1], pixel[2]));

            var bgr = new MemoryBitmap(channel.Width, channel.Height, Pixel.BGR24.Format);
            SpanBitmap.CopyPixels(channel, bgr, (0, 255), (0, 255));

            return bgr;
        }

        public static System.Numerics.Tensors.DenseTensor<Byte> ToDenseTensor(this MemoryBitmap bitmap)
        {
            var dimensions = new int[] { 1, bitmap.Height, bitmap.Width, bitmap.PixelByteSize };

            return new System.Numerics.Tensors.DenseTensor<byte>(bitmap.Memory, dimensions);
        }

        /// <summary>
        /// Copies the ROI of an image to the target tensor.
        /// </summary>
        /// <param name="dstTensor">The tensor target.</param>
        /// <param name="dstTensorSizeIdx">
        /// <para>Index in the tensor dimensions representing the bitmap size.</para>
        /// <para>In tensor semantics, the size is represented as Height, followed by Width.</para>
        /// </param>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageROI">The region of interest of the source image.</param>
        /// <param name="dstToSrcXform">A transform to transform from <paramref name="dstTensor"/> coordinates to <paramref name="srcImage"/> coordinates. </param>
        public static void SetPixels(this IInputImageTensor dstTensor, int dstTensorSizeIdx, PointerBitmap srcImage, Matrix3x2 srcImageROI, out Matrix3x2 dstToSrcXform)
        {
            if (dstTensor == null) throw new ArgumentNullException(nameof(dstTensor));

            for (int i = 0; i <= dstTensorSizeIdx + 1; ++i)
            {
                if (dstTensor.Dimensions[i] <= 0) throw new ArgumentException($"Dimension[{i}] value is invalid.", nameof(dstTensor));
            }

            Matrix3x2.Invert(srcImageROI, out Matrix3x2 iform);
            var ih = dstTensor.Dimensions[dstTensorSizeIdx + 0];
            var iw = dstTensor.Dimensions[dstTensorSizeIdx + 1];
            iform *= Matrix3x2.CreateScale(iw, ih);
            Matrix3x2.Invert(iform, out dstToSrcXform);

            dstTensor.SetPixels(srcImage, iform);
        }

        /// <summary>
        /// Copies the ROI of an image to the target tensor.
        /// </summary>
        /// <param name="dstTensor">The tensor target.</param>
        /// <param name="dstTensorSizeIdx">
        /// <para>Index in the tensor dimensions representing the bitmap size.</para>
        /// <para>In tensor semantics, the size is represented as Height, followed by Width.</para>
        /// </param>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageROI">The region of interest of the source image.</param>
        /// <param name="dstToSrcXform">A transform to transform from <paramref name="dstTensor"/> coordinates to <paramref name="srcImage"/> coordinates. </param>
        public static void SetPixels(this ITensor dstTensor, int dstTensorSizeIdx, PointerBitmap srcImage, Matrix3x2 srcImageROI, out Matrix3x2 dstToSrcXform)
        {
            if (!(dstTensor is IInputImageTensor dstImage)) throw new ArgumentException($"Does not implement {nameof(IInputImageTensor)}.", nameof(dstTensor));

            dstImage.SetPixels(dstTensorSizeIdx, srcImage, srcImageROI, out dstToSrcXform);
        }

        public static unsafe SpanBitmap<T> AsSpanBitmap<T>(this SpanTensor2<T> tensor)
            where T : unmanaged
        {
            var l = sizeof(T);

            if (typeof(Pixel.IReflection).IsAssignableFrom(typeof(T))) return tensor.AsSpanBitmap<T, T>();

            if (typeof(T) == typeof(Single)) return tensor.AsSpanBitmap<T>(PixelFormat.CreateFromDepthAndChannels(typeof(float), 1));
            if (typeof(T) == typeof(Vector2)) return tensor.AsSpanBitmap<T>(PixelFormat.CreateFromDepthAndChannels(typeof(float), 2));
            if (typeof(T) == typeof(Vector3)) return tensor.AsSpanBitmap<T>(PixelFormat.CreateFromDepthAndChannels(typeof(float), 3));
            if (typeof(T) == typeof(Vector4)) return tensor.AsSpanBitmap<T>(PixelFormat.CreateFromDepthAndChannels(typeof(float), 4));

            if (l == 1) return tensor.AsSpanBitmap<T>(Pixel.Luminance8.Format);
            if (l == 3) return tensor.AsSpanBitmap<T>(Pixel.BGR24.Format);

            throw new NotImplementedException();
        }

        public static SpanBitmap<T> AsSpanBitmap<T>(this SpanTensor2<T> tensor, PixelFormat fmt)
            where T : unmanaged
        {
            var buff = MEMMARSHAL.Cast<T, Byte>(tensor.Span);

            return new SpanBitmap<T>(buff, tensor.BitmapSize.Width, tensor.BitmapSize.Height, fmt);
        }

        public static unsafe SpanBitmap<TPixel> AsSpanBitmap<TTensor, TPixel>(this SpanTensor2<TTensor> tensor)
            where TTensor : unmanaged
            where TPixel : unmanaged
        {
            if (sizeof(TTensor) != sizeof(TPixel)) throw new ArgumentException(nameof(TPixel));

            var data = MEMMARSHAL.Cast<TTensor, Byte>(tensor.Span);
            var pfmt = PixelFormat.TryIdentifyFormat<TPixel>();

            return new SpanBitmap<TPixel>(data, tensor.BitmapSize.Width, tensor.BitmapSize.Height, pfmt);
        }
    }
}
