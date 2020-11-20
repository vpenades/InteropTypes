using System;
using System.Numerics;

using InteropBitmaps;

using DISPRIM = InteropModels.DisplayPrimitive;

namespace InteropModels
{
    public partial class ColorFrameState : DISPRIM.IBitmapSource
    {
        private MemoryBitmap _Image;

        public MemoryBitmap Image => _Image;

        public void Update(SpanBitmap<Vector3> newImage)
        {
            // var minmax = SpanBitmap.MinMax(newImage);

            if (_Image.Width != newImage.Width || _Image.Height != newImage.Height)
            {
                _Image = new MemoryBitmap(newImage.Width, newImage.Height, Pixel.BGR24.Format);
            }

            SpanBitmap.CopyPixels(newImage, _Image, (0, 255), (0, 255));            
        }

        public MemoryBitmap GetDisplayBitmap()
        {
            return _Image; // it's already a BGR24 image!
        }
    }
}
