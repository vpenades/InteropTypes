using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Codecs
{
    internal class EncodeTests
    {
        [Test]
        public void EncodeFrames()
        {
            AttachmentInfo.From("bait.txt").WriteAllText("bait");

            AttachmentInfo.From("out.mp4").WriteObject(f => new FFMpegCoreCodec().Encode(f, GetRandomFrames()));
        }



        public IEnumerable<InteropTypes.Graphics.Bitmaps.MemoryBitmap> GetRandomFrames()
        {
            var rnd = new Graphics.Bitmaps.Processing.RandomNoise();

            for (int i=0; i < 100; ++i)
            {
                var m = new InteropTypes.Graphics.Bitmaps.MemoryBitmap<Graphics.Bitmaps.Pixel.BGR24>(256, 256);

                m.AsSpanBitmap().AsTypeless().ApplyEffect(rnd);

                yield return m;
            }
        }
    }
}
