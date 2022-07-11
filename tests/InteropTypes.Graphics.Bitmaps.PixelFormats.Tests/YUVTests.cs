using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{

    [ResourcePathFormat("*/Resources")]
    [AttachmentPathFormat("?")]
    internal class YUVTests
    {
        [Test]
        public void LoadKinect2ColorFrame()
        {
            TestContext.CurrentContext.AttachFolderBrowserShortcut();

            var src = MemoryBitmap<ushort>.Load(ResourceInfo.From("Kinect2Color.InteropBmp"));

            var dst = new MemoryBitmap<Pixel.BGR24>(src.Width, src.Height);

            dst.AsSpanBitmap().SetPixelsFromYUY2(src);

            dst.Save(AttachmentInfo.From("kinect2color.jpg"));
        }

    }
}
