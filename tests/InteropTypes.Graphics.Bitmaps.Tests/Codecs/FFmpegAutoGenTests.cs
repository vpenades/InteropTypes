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


        [Test]
        public void ProcessMp4Frames()
        {
            TestContext.CurrentContext.AttachFolderBrowserShortcut();

            var input = ResourceInfo.From("count-video.mp4");
            var output = AttachmentInfo.From("result.h264");

            void _drawOver(MemoryBitmap bmp)
            {
                var bmpx = bmp.OfType<Pixel.BGR24>();

                var dc = InteropTypes.Graphics.Backends.InteropDrawing.CreateDrawingContext(bmpx);
                dc.DrawTextLine(System.Numerics.Matrix3x2.CreateTranslation(10, 10), "hello", 100, System.Drawing.Color.Black);

            }

            FFmpegAutoGen.ProcessFrames(input, output, _drawOver, 10);
        }
    }
}
