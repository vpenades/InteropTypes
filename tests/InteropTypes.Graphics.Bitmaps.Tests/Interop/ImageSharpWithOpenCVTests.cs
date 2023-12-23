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
        [TestCase("shannon.jpg")]
        [TestCase("white.png")]
        public void LoadImage(string filePath)
        {
            filePath = ResourceInfo.From(filePath);
            
            var membmp = MemoryBitmap.Load(filePath, Codecs.OpenCvCodec.Default);

            membmp.Save(AttachmentInfo.From("Result.png"));
        }


        [TestCase("shannon.jpg")]
        [TestCase("white.png")]
        public void DetectImageFeatures(string filePath)
        {
            filePath = ResourceInfo.From(filePath);

            var img = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(filePath);

            AttachmentInfo
                .From("original.png")
                .WriteObject(f => img.Save(f));

            OpenCvSharp.Mat _Process(OpenCvSharp.Mat src)
            {
                var sil_mask = OpenCVProcessing.ExtractSubjectSiluette(src);
                // return sil_mask;
                var sub_col_siluette = OpenCVProcessing.ExtractBlobTexture(src, sil_mask);
                var foot_seg_marker = OpenCVProcessing.ExtractSegmentalMarker(sub_col_siluette);

                return foot_seg_marker;
            }

            img.WriteAsSpanBitmap(self => self.WithOpenCv().Apply(_Process));

            AttachmentInfo
                .From("result.png")
                .WriteObject(f => img.Save(f));

            // var img2 = img.AsSpanBitmap().AsOpenCVSharp().CloneMutated(_Process);
            // AttachmentInfo.From("result2.png").WriteObject(f => img2.Save(f.FullName));
        }
    }
}
