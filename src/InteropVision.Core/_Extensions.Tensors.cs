using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

using InteropTensors;

namespace InteropVision
{
    public static partial class _Extensions
    {
        public static SpanTensor3<float> SetImage(this SpanTensor3<float> dst, SpanBitmap src, ITensorImageProcessor<float> options)
        {
            options.CopyImage(src, dst);
            return dst;
        }
    }
}
