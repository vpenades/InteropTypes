using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Codecs
{
    [Category("Backends")]
    public class FFmpegAutoGenTests
    {
        [Test]
        public void DecodeMp4Frames()
        {
            foreach (var (bitmap, state) in FFmpegAutoGen.DecodeFrames(ResourceInfo.From("count-video.mp4")))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);

                var idx = state.State["index"];

                bitmap
                    .AsSpanBitmap()
                    .ToMemoryBitmap()
                    .Save(AttachmentInfo.From($"frame{idx:D3}.jpg"));
            }
        }

        [Test]
        public void EncodeH264Frames()
        {
            var inputFrames = FFmpegAutoGen.DecodeFrames(ResourceInfo.From("count-video.mp4"));

            var finfo = AttachmentInfo
                .From("output.h264")
                .WriteVideo(inputFrames.Select(item => item.bitmap));

            foreach (var (bitmap, state) in FFmpegAutoGen.DecodeFrames(finfo.FullName))
            {
                var data = string.Join(" ", state.State);
                TestContext.WriteLine(data);
            }
        }
    }
}
