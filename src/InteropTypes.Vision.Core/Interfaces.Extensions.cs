using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Tensors;

using RECT = System.Drawing.Rectangle;

using CAPINFO = InteropTypes.Vision.IO.ICaptureDeviceInfo;
using CAPTIME = System.DateTime;
using InteropTypes.Graphics;

namespace InteropTypes.Vision
{
    public static partial class _Extensions
    {
        public static void Inference<TResult>(this IInferenceContext<PointerBitmap, TResult> context, TResult result, InferenceInput<MemoryBitmap> image, RECT? imageRect = null)
            where TResult : class
        {
            if (image.Content.PixelFormat == Pixel.BGR24.Format)
            {
                context.Inference(result, image.Content.OfType<Pixel.BGR24>(), imageRect);
                return;
            }

            // convert image to BGR24
            MemoryBitmap tmp = default;
            image.Content.AsSpanBitmap().CopyTo(ref tmp, Pixel.BGR24.Format);

            tmp.AsSpanBitmap().PinReadablePointer
                (
                ptrBmp => context.Inference(result, (image.CaptureDevice,image.CaptureTime, ptrBmp), imageRect)
                );
        }

        public static void Inference<TResult>(this IInferenceContext<PointerBitmap, TResult> context, TResult result, SpanBitmap image, RECT? imageRect = null)
            where TResult : class
        {
            if (image.PixelFormat == Pixel.BGR24.Format)
            {
                context.Inference(result, image.OfType<Pixel.BGR24>(), imageRect);
                return;
            }

            // convert image to BGR24
            MemoryBitmap tmp = default;
            image.CopyTo(ref tmp, Pixel.BGR24.Format);

            tmp.AsSpanBitmap().PinReadablePointer
                (
                ptrBmp => context.Inference(result, ptrBmp, imageRect)
                );
        }

        public static void Inference<TResult>(this IInferenceContext<PointerBitmap, TResult> context, TResult result, SpanBitmap<Pixel.BGR24> image, RECT? imageRect = null)
            where TResult:class
        {
            image.PinReadablePointer
                (
                ptrBmp => context.Inference(result, ptrBmp, imageRect)
                );
        }

        

        public static void FitInputImage(this IModelSession session, PointerBitmap bmp)
        {
            if (session.GetInputTensor(0) is IInputImageTensor inputImage)
            {
                inputImage.FitPixels(bmp);
            }
        }
    }
}
