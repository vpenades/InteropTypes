using System;
using System.Numerics;

using InteropBitmaps;

using InteropTypes.Graphics.Drawing;

namespace InteropVision
{
    public partial class ColorFrameState : IDrawingBrush<ICanvas2D>
    {
        private MemoryBitmap _Image;
        private ImageSource _SpriteAsset;

        public MemoryBitmap Image => _Image;

        public void Update(SpanBitmap<Vector3> newImage)
        {
            // var minmax = SpanBitmap.MinMax(newImage);

            if (_Image.Width != newImage.Width || _Image.Height != newImage.Height)
            {
                _Image = new MemoryBitmap(newImage.Width, newImage.Height, Pixel.BGR24.Format);
            }

            SpanBitmap.CopyPixels(newImage, _Image, (0, 255), (0, 255));

            _SpriteAsset = null;
        }

        public MemoryBitmap GetDisplayBitmap()
        {
            return _Image; // it's already a BGR24 image!
        }

        public void DrawTo(ICanvas2D dc)
        {
            if (_SpriteAsset == null)
            {
                var bmp = GetDisplayBitmap();
                _SpriteAsset = ImageSource.CreateFromBitmap(bmp, (bmp.Width, bmp.Height), (0, 0));
            }

            dc.DrawImage(Matrix3x2.Identity, _SpriteAsset);
        }
    }
}
