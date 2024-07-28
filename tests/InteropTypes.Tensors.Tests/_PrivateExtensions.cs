using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Tensors.Imaging;

using NUnit.Framework;

using SixLabors.ImageSharp;

using MEMMARSHAL = System.Runtime.InteropServices.MemoryMarshal;


namespace InteropTypes.Tensors
{
    using V3 = System.Numerics.Vector3;
    using BGR24 = Graphics.Bitmaps.Pixel.BGR24;
    using BGR96F = Graphics.Bitmaps.Pixel.BGR96F;

    internal static class _PrivateExtensions
    {
        public static bool TryGetAsSpanTensor<TPixel>(this in SpanBitmap<TPixel> src, out SpanTensor2<TPixel> result)
            where TPixel : unmanaged
        {
            if (!src.Info.IsContinuous) { result = default; return false; }

            var data = MEMMARSHAL.Cast<Byte, TPixel>(src.WritableBytes);

            result = new SpanTensor2<TPixel>(data, src.Height, src.Width);
            return true;
        }

        
        public static void AttachToCurrentTest<T>(this TensorBitmap<T> bmp, string fileName)
            where T:unmanaged, IConvertible
        {
            using (var dstImg = bmp.ToImageSharpRgb24())
            {
                AttachmentInfo.From(fileName).WriteObject(f => dstImg.Save(f));
            }
        }


        public static void AttachToCurrentTest(this SpanTensor2<BGR24> tensor, string fileName)
        {
            tensor.ToSpanBitmap().Save(fileName, Codecs.STBCodec.Default);
            NUnit.Framework.TestContext.AddTestAttachment(fileName);
        }

        public static void AttachToCurrentTest(this SpanTensor2<V3> tensor, string fileName)
        {
            tensor.ToSpanBitmap().Save(fileName, Codecs.STBCodec.Default);
            NUnit.Framework.TestContext.AddTestAttachment(fileName);
        }

        public static SpanBitmap<BGR96F> ToSpanBitmap(this SpanTensor2<V3> tensor)
        {
            var data = MEMMARSHAL.Cast<V3, Byte>(tensor.Span);

            return new SpanBitmap<BGR96F>(data, tensor.BitmapSize.Width, tensor.BitmapSize.Height, BGR96F.Format);
        }

        public static SpanBitmap<BGR24> ToSpanBitmap(this SpanTensor2<BGR24> tensor)
        {
            var data = MEMMARSHAL.Cast<BGR24, Byte>(tensor.Span);

            return new SpanBitmap<BGR24>(data, tensor.BitmapSize.Width, tensor.BitmapSize.Height, BGR24.Format);
        }
    }
}
