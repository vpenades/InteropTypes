using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class FFmpegAutoGenTests
    {
        [Test]
        public void DecodeMp4Frames()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\count-video.mp4");

            int idx = 0;

            foreach(var (bitmap, state) in Codecs.FFmpegAutoGenCodec.DecodeFrames(filePath))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);

                bitmap.AsSpanBitmap().AttachToCurrentTest($"frame{idx:D3}.jpg");
                ++idx;
            }
        }

        [Test]
        public void EncodeH264Frames()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\count-video.mp4");            

            var inputFrames = Codecs.FFmpegAutoGenCodec.DecodeFrames(filePath);

            filePath = inputFrames.Select(item => item.bitmap).AttachToCurrentTest("output.h264");

            foreach (var (bitmap, state) in Codecs.FFmpegAutoGenCodec.DecodeFrames(filePath))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);
            }
        }
    }
}
