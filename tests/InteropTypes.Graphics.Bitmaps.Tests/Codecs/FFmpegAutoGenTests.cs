using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class FFmpegAutoGenTests
    {
        [Test]
        public void DecodeMp4Frames()
        {
            int idx = 0;

            foreach (var (bitmap, state) in Codecs.FFmpegAutoGen.DecodeFrames(ResourceInfo.From("count-video.mp4")))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);

                bitmap
                    .AsSpanBitmap()
                    .ToMemoryBitmap()
                    .Save(AttachmentInfo.From($"frame{idx:D3}.jpg"));

                ++idx;
            }
        }

        [Test]
        public void EncodeH264Frames()
        {
            var inputFrames = Codecs.FFmpegAutoGen.DecodeFrames(ResourceInfo.From("count-video.mp4"));

            var finfo = AttachmentInfo
                .From("output.h264")
                .WriteVideo(inputFrames.Select(item => item.bitmap));

            foreach (var (bitmap, state) in Codecs.FFmpegAutoGen.DecodeFrames(finfo.FullName))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);
            }
        }
    }
}
