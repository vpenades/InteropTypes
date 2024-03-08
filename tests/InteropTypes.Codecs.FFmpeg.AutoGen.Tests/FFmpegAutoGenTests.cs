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
                TestContext.WriteLine(string.Join(" ", state.Times.Select(item => (item.Key, item.Value.den / (float)item.Value.num))));
                TestContext.WriteLine(string.Join(" ", state.State));

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
            var input = ResourceInfo.From("count-video.mp4");
            var output = AttachmentInfo.From("result.h264");

            var inputFrames = FFmpegAutoGen.DecodeFrames(input);

            var finfo = output.WriteObject(f => FFmpegAutoGen.EncodeFrames(f, 20, inputFrames.Select(item => item.bitmap)));            

            foreach (var (bitmap, state) in FFmpegAutoGen.DecodeFrames(finfo.FullName))
            {
                TestContext.WriteLine(string.Join(" ", state.Times.Select(item => (item.Key, item.Value.num / (float)item.Value.den))));
                TestContext.WriteLine(string.Join(" ", state.State));                
            }
        }


        [Test]
        public void ProcessMp4Frames()
        {
            var input = ResourceInfo.From("count-video.mp4");
            var output = AttachmentInfo.From("result.h264");

            void _drawOver(MemoryBitmap bmp)
            {
                var bmpx = bmp.OfType<Pixel.BGR24>();

                var dc = InteropTypes.Graphics.Backends.InteropDrawing.CreateDrawingContext(bmpx);
                dc.DrawTextLine(System.Numerics.Matrix3x2.CreateTranslation(10, 10), "hello", 100, System.Drawing.Color.Black);
            }

            FFmpegAutoGen.ProcessFrames(input, output, _drawOver, 3);
        }
    }
}
