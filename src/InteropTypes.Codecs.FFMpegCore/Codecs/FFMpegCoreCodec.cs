using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
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
