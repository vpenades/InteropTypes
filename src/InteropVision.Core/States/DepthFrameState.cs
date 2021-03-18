using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

using DISPRIM = InteropVision.DisplayPrimitive;

namespace InteropVision
{
    public partial class DepthFrameState : DISPRIM.IBitmapSource
    {
        private MemoryBitmap<float> _Depth;

        public MemoryBitmap<float> Depth => _Depth;

        public void Update(SpanBitmap<float> newDepth)
        {
            newDepth.CopyTo(ref _Depth);
        }

        public MemoryBitmap<Byte> GetGrayBitmap()
        {
            var minmax = SpanBitmap.MinMax(_Depth);

            var gray = new MemoryBitmap<Byte>(_Depth.Width, _Depth.Height, Pixel.Luminance8.Format);

            SpanBitmap.CopyPixels(_Depth, gray, (0, 255), (0, 255));

            return gray;
        }

        public MemoryBitmap GetDisplayBitmap()
        {
            var (min, max) = SpanBitmap.MinMax(_Depth);

            var rgb = new MemoryBitmap<Byte>(_Depth.Width, _Depth.Height, Pixel.Luminance8.Format);

            SpanBitmap.CopyPixels(_Depth, rgb, (-min, 255.0f / (max - min)), (0, 255));

            return rgb;
        }
    }
}
