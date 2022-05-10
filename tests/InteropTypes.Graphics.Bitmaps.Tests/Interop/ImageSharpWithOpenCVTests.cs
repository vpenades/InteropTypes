using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

using SixLabors.ImageSharp;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Backends")]
    public class ImageSharpWithOpenCVTests
    {
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);
            
            var membmp = MemoryBitmap.Load(filePath, Codecs.OpenCvCodec.Default);

            membmp.Save(AttachmentInfo.From("Result.png"));


            // var imgsharp = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(filePath);
            // imgsharp.AttachToCurrentTest("original.png");
            // imgsharp.AsSpanBitmap().AsOpenCVSharp().Blur((5,1));
            // imgsharp.AttachToCurrentTest("result.png");
        }


        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\white.png")]
        public void DetectImageFeatures(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var img = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(filePath);

            img.AttachToCurrentTest("original.png");

            OpenCvSharp.Mat _Process(OpenCvSharp.Mat src)
            {
                var sil_mask = OpenCVProcessing.ExtractSubjectSiluette(src);
                // return sil_mask;
                var sub_col_siluette = OpenCVProcessing.ExtractBlobTexture(src, sil_mask);
                var foot_seg_marker = OpenCVProcessing.ExtractSegmentalMarker(sub_col_siluette);

                return foot_seg_marker;
            }

            img.WriteAsSpanBitmap(self => self.WithOpenCv().Apply(_Process));

            img.AttachToCurrentTest("result.png");

            // var img2 = img.AsSpanBitmap().AsOpenCVSharp().CloneMutated(_Process);
            // img2.AttachToCurrentTest("result.png");
        }
    }
}
