using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes
{
    public class WhiteNoiseTexture : Graphics.Bitmaps.InterlockedBitmap
    {
        private WhiteNoiseTexture() { }

        // https://stackoverflow.com/questions/38739403/await-task-run-vs-await/

        public static WhiteNoiseTexture CreateInThread()
        {
            var instance = new WhiteNoiseTexture();

            Task.Run(instance._AsyncUpdateBitmap);

            return instance;
        }

        public static async Task<WhiteNoiseTexture> CreateAsync()
        {
            var instance = new WhiteNoiseTexture();

            await instance._AsyncUpdateBitmap().ConfigureAwait(false);

            return instance;
        }

        private async Task _AsyncUpdateBitmap()
        {
            var bmp = new Graphics.Bitmaps.MemoryBitmap(256, 256, Graphics.Bitmaps.Pixel.BGR24.Format);
            var rnd = new Random();

            while (true)
            {
                await Task.Delay(10).ConfigureAwait(false);

                _Update(bmp, rnd);
            }
        }

        private static void _Update(MemoryBitmap bmp, Random rnd)
        {
            if (bmp.TryGetBuffer(out var buffer))
            {
                var data = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(buffer.Array);

                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = rnd.Next();
                }
            }
        }

    }
}
