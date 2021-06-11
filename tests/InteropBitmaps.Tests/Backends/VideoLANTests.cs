using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Backends")]
    public class VideoLANTests
    {
        // [Test] // gets stucked somewhere
        public async Task DecodeMp4Frames()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\count-video.mp4");

            int idx = 0;

            var uri = new Uri(filePath, UriKind.Absolute);

            var codec = new Codecs.VideoLanCodecAsync(uri);


            await codec.DecodeAsync(bmp => TestContext.WriteLine("Frame received"));

            /*
            foreach (var bitmap in codec.Decode())
            {
                TestContext.WriteLine($"Frame {idx} received");

                // bitmap.AsSpanBitmap().AttachToCurrentTest($"frame{idx:D3}.jpg");
                ++idx;
            }*/
        }
    }
}
