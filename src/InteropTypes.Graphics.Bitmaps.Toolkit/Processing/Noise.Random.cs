using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    public readonly struct RandomNoise : SpanBitmap.IEffect
    {
        private static Random _Rnd;

        public readonly bool TryApplyTo<TPixel>(SpanBitmap<TPixel> target)
            where TPixel : unmanaged
        {
            return false;
        }

        public readonly bool TryApplyTo(SpanBitmap target)
        {
            if (_Rnd == null) _Rnd = new Random();

            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UseScanlineBytes(y);

                var fourCount = row.Length & ~3;

                var rowInts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(row.Slice(0, fourCount));

                for (int x = 0; x < rowInts.Length; ++x)
                {
                    rowInts[x] = _Rnd.Next();
                }

                for (int x = rowInts.Length * 4; x < row.Length; ++x)
                {
                    row[x] = (Byte)_Rnd.Next();
                }
            }

            return true;
        }               
    }
}
