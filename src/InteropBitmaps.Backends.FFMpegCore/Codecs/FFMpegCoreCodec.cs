using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class FFMpegCoreCodec
    {
        public static void EncodeFrames(string url, IEnumerable<PointerBitmap> frames)
        {
            var wrappedFrames = _Implementation.WrapFrames(frames);
            _Implementation.Encode(url, wrappedFrames);
        }
    }
}
