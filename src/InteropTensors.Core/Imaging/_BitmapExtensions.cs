using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using InteropBitmaps;

using TENSOR2FLOAT = InteropTensors.SpanTensor2<float>;
using TENSOR3FLOAT = InteropTensors.SpanTensor3<float>;
using TENSOR4FLOAT = InteropTensors.SpanTensor4<float>;

namespace InteropTensors
{
    public static partial class TensorExtensions
    {
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

            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<float, byte>(src.Span);
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
    }
}
