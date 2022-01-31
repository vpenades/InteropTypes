using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropBitmaps;

using InteropTypes.Graphics.Drawing;

namespace InteropVision
{
    public partial class DepthFrameState : IDrawingBrush<ICanvas2D>
    {
        private MemoryBitmap<float> _Depth;
        private ImageAsset _SpriteAsset;

        public MemoryBitmap<float> Depth => _Depth;

        public void Update(SpanBitmap<float> newDepth)
        {
            newDepth.CopyTo(ref _Depth);
            _SpriteAsset = null;
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

        public void DrawTo(ICanvas2D dc)
        {
            if (_SpriteAsset == null)
            {
                var bmp = GetDisplayBitmap();
                _SpriteAsset = ImageAsset.CreateFromBitmap(bmp, (bmp.Width, bmp.Height), (0, 0));
            }

            dc.DrawImage(Matrix3x2.Identity, _SpriteAsset);
        }
    }
}
