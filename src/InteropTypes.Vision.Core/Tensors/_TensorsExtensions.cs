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
        public static void CopyTo<TSrc, TDst>(this Imaging.TensorBitmap<TSrc> src, ref MemoryBitmap<TDst> dst)
            where TSrc : unmanaged, IConvertible
            where TDst : unmanaged
        {
            if (dst.Width != src.Width || dst.Height != src.Height)
            {
                dst = new MemoryBitmap<TDst>(src.Width, src.Height);
            }

            for(int y=0; y < dst.Height; y++)
            {
                for(int x=0; x <dst.Width; x++)
                {
                    var pixel = src.ScaledPixels.GetPixelUnchecked(x, y);
                    var rgba = new Pixel.RGBA128F(pixel).To<TDst>();
                    dst.SetPixel(x, y, rgba);
                }
            }
        }

        public static bool TryGetSpanBitmap<T>(this Imaging.TensorBitmap<T> src, out SpanBitmap dst)
            where T:unmanaged, IConvertible
        {
            if (!src.TryGetImageInterleaved(out var t3))
            {
                dst = default;
                return false;
            }

            var fmt = GetFormat<T>(src.Encoding);
            var span = MEMMARSHAL.Cast<T, byte>(t3.Span);
            dst = new SpanBitmap(span, src.Width, src.Height, fmt);
            return true;
        }


        public static unsafe PixelFormat GetFormat<TPixel>(Imaging.ColorEncoding encoding)
            where TPixel:unmanaged
        {
            if (sizeof(TPixel) == 1)
            {
                if (encoding == Imaging.ColorEncoding.A) return Pixel.Alpha8.Format;                
                return Pixel.Luminance8.Format;
            }

            if (sizeof(TPixel) == 2)
            {                
                return Pixel.Luminance16.Format;
            }

            if (sizeof(TPixel) == 3)
            {
                if (encoding == Imaging.ColorEncoding.BGR) return Pixel.BGR24.Format;
                if (encoding == Imaging.ColorEncoding.BGRA) return Pixel.BGR24.Format;
                return Pixel.RGB24.Format;
            }

            if (sizeof(TPixel) == 4)
            {
                if (typeof(TPixel) == typeof(float))
                {
                    if (encoding == Imaging.ColorEncoding.L) return Pixel.Luminance32F.Format;
                }
                else
                {
                    if (encoding == Imaging.ColorEncoding.RGB) return Pixel.RGBA32.Format;
                    if (encoding == Imaging.ColorEncoding.BGR) return Pixel.BGRA32.Format;
                    if (encoding == Imaging.ColorEncoding.BGRA) return Pixel.BGRA32.Format;
                    if (encoding == Imaging.ColorEncoding.RGBA) return Pixel.RGBA32.Format;
                    if (encoding == Imaging.ColorEncoding.ARGB) return Pixel.ARGB32.Format;
                }                
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                if (encoding == Imaging.ColorEncoding.BGR) return Pixel.BGR96F.Format;
                if (encoding == Imaging.ColorEncoding.BGRA) return Pixel.BGR96F.Format;
                return Pixel.RGB96F.Format;
            }

            if (typeof(TPixel) == typeof(Vector4))
            {
                if (encoding == Imaging.ColorEncoding.BGR) return Pixel.BGRA128F.Format;
                if (encoding == Imaging.ColorEncoding.BGRA) return Pixel.BGRA128F.Format;                
                return Pixel.RGBA128F.Format;
            }

            throw new NotImplementedException();
        }

        public static Imaging.ColorEncoding GetColorEncoding<TPixel>()
        {
            if (typeof(TPixel) == typeof(Pixel.Alpha8)) return Imaging.ColorEncoding.A;

            if (typeof(TPixel) == typeof(Pixel.Luminance8)) return Imaging.ColorEncoding.L;
            if (typeof(TPixel) == typeof(Pixel.Luminance16)) return Imaging.ColorEncoding.L;
            if (typeof(TPixel) == typeof(Pixel.Luminance32F)) return Imaging.ColorEncoding.L;

            if (typeof(TPixel) == typeof(Pixel.BGR24)) return Imaging.ColorEncoding.BGR;
            if (typeof(TPixel) == typeof(Pixel.RGB24)) return Imaging.ColorEncoding.RGB;
            if (typeof(TPixel) == typeof(Pixel.RGBA32)) return Imaging.ColorEncoding.RGBA;
            if (typeof(TPixel) == typeof(Pixel.BGRA32)) return Imaging.ColorEncoding.BGRA;
            if (typeof(TPixel) == typeof(Pixel.ARGB32)) return Imaging.ColorEncoding.ARGB;            

            if (typeof(TPixel) == typeof(Pixel.BGR96F)) return Imaging.ColorEncoding.BGR;
            if (typeof(TPixel) == typeof(Pixel.BGRA128F)) return Imaging.ColorEncoding.BGRA;

            if (typeof(TPixel) == typeof(Pixel.RGB96F)) return Imaging.ColorEncoding.RGB;
            if (typeof(TPixel) == typeof(Pixel.RGBA128F)) return Imaging.ColorEncoding.RGBA;            

            return Imaging.ColorEncoding.Undefined;
        }

        public static Imaging.BitmapSampler<TPixel> AsBitmapSampler<TPixel>(this SpanBitmap<TPixel> src)
            where TPixel:unmanaged
        {
            var encoding = GetColorEncoding<TPixel>();

            return new Imaging.BitmapSampler<TPixel>(src.ReadableBytes, src.Info.StepByteSize, src.Width, src.Height, encoding);
        }

        public static unsafe void FillBitmap(this Imaging.TensorBitmap<float> dst, SpanBitmap src, in Imaging.BitmapTransform xform)            
        {
            switch(src.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: FillBitmap(dst, src.OfType<Pixel.Luminance8>(), xform); return;
                case Pixel.Luminance32F.Code: FillBitmap(dst, src.OfType<Pixel.Luminance32F>(), xform); return;
                case Pixel.BGR24.Code: FillBitmap(dst, src.OfType<Pixel.BGR24>(), xform); return;
                case Pixel.RGB24.Code: FillBitmap(dst, src.OfType<Pixel.RGB24>(), xform); return;
                case Pixel.BGRA32.Code: FillBitmap(dst, src.OfType<Pixel.BGRA32>(), xform); return;
                case Pixel.RGBA32.Code: FillBitmap(dst, src.OfType<Pixel.RGBA32>(), xform); return;
                case Pixel.ARGB32.Code: FillBitmap(dst, src.OfType<Pixel.ARGB32>(), xform); return;
                case Pixel.BGR96F.Code: FillBitmap(dst, src.OfType<Pixel.BGR96F>(), xform); return;
                case Pixel.RGB96F.Code: FillBitmap(dst, src.OfType<Pixel.RGB96F>(), xform); return;
                case Pixel.BGRA128F.Code: FillBitmap(dst, src.OfType<Pixel.BGRA128F>(), xform); return;
                case Pixel.RGBA128F.Code: FillBitmap(dst, src.OfType<Pixel.RGBA128F>(), xform); return;
            }

            throw new NotSupportedException($"{src.PixelFormat}");
        }

        public static unsafe void FillBitmap<TSrcPixel>(this Imaging.TensorBitmap<float> dst, SpanBitmap<TSrcPixel> src, in Imaging.BitmapTransform xform)
            where TSrcPixel:unmanaged
        {
            var sampler = src.AsBitmapSampler();
            dst.FillPixels(sampler, xform);
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

        /*
        public static System.Numerics.Tensors.DenseTensor<Byte> ToDenseTensor(this MemoryBitmap bitmap)
        {
            var dimensions = new int[] { 1, bitmap.Height, bitmap.Width, bitmap.PixelByteSize };

            return new System.Numerics.Tensors.DenseTensor<byte>(bitmap.Memory, dimensions);
        }*/

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
