using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class STBCodec : IBitmapDecoding
    {
        #region lifecycle

        static STBCodec() { }

        private STBCodec() { }

        private static readonly STBCodec _Default = new STBCodec();

        public static STBCodec Default => _Default;

        #endregion
        public MemoryBitmap Read(System.IO.Stream s)
        {
            var img = StbImageLib.ImageResult.FromStream(s, StbImageLib.ColorComponents.RedGreenBlueAlpha);

            return _Implementation.AsSpanBitmap(img).ToMemoryBitmap();
        }        
    }
}
