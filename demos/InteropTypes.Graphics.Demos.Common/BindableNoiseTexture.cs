using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes
{
    public class BindableNoiseTexture
    {
        public BindableNoiseTexture()
        {
            _BindableBitmap = new BindableBitmap();
            _BindableSprite = new ImageSource(_BindableBitmap, (0, 0), (64, 64), (0, 0));

            void _UpdateBindableAsync()
            {
                var tmp = new MemoryBitmap<Pixel.RGB24>(64, 64);
                var rnd = new Random();
                var pix = default(Pixel.RGB24);

                while (true)
                {
                    foreach (var p in tmp.EnumeratePixels())
                    {
                        pix.SetRandom(rnd);
                        tmp.SetPixel(p.Location, pix);
                    }

                    _BindableBitmap.Enqueue(tmp);

                    Task.Delay(1000 / 30);
                }
            }

            Task.Run(_UpdateBindableAsync);
        }

        private BindableBitmap _BindableBitmap;
        private ImageSource _BindableSprite;

        public ImageSource Sprite => _BindableSprite;
    }
}
