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

        #region API
        public bool TryRead(System.IO.Stream s, out MemoryBitmap bitmap)
        {
            bitmap = default;

            try
            {
                var img = StbImageLib.ImageResult.FromStream(s, StbImageLib.ColorComponents.RedGreenBlueAlpha);

                var info = _Implementation.GetBitmapInfo(img);

                bitmap = new MemoryBitmap(img.Data, info);

                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message == "unknown image type") return false;
                else throw ex;
            }
        }

        #endregion
    }
}
