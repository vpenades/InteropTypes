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
        public BindableNoiseTexture(int width, int height)
        {
            _Width= width;
            _Height= height;
            _BindableBitmap = new BindableBitmap();
            _BindableSprite = new ImageSource(_BindableBitmap, (0, 0), (width, height), (0, 0));
        }        

        public async Task RunAsync()
        {
            await _UpdateBindableAsync().ConfigureAwait(false);            
        }

        public void RunTask()
        {
            Task.Run(_UpdateBindableAsync);
        }

        private int _Width;
        private int _Height;

        private BindableBitmap _BindableBitmap;
        private ImageSource _BindableSprite;

        public ImageSource Sprite => _BindableSprite;

        async Task _UpdateBindableAsync()
        {
            var tmp = new MemoryBitmap<Pixel.RGB24>(_Width, _Height);
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

                await Task.Delay(1000 / 30).ConfigureAwait(false);
            }
        }
    }
}
