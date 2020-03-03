using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class STBCodec : IBitmapDecoding
    {
        public MemoryBitmap Read(System.IO.Stream s)
        {
            var img = StbImageLib.ImageResult.FromStream(s, StbImageLib.ColorComponents.RedGreenBlueAlpha);

            return _Implementation.AsSpanBitmap(img).ToMemoryBitmap();
        }        
    }
}
