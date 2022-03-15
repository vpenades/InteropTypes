using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes
{
    public class WhiteNoiseTexture : Graphics.Bitmaps.InterlockedBitmap
    {
        public WhiteNoiseTexture()
        {
            System.Threading.Tasks.Task.Run(_AsyncUpdateBitmap);
        }
        private void _AsyncUpdateBitmap()
        {
            var bmp = new Graphics.Bitmaps.MemoryBitmap(256, 256, Graphics.Bitmaps.Pixel.BGR24.Format);
            var rnd = new Random();

            while (true)
            {
                System.Threading.Thread.Sleep(1000 / 100);

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
}
