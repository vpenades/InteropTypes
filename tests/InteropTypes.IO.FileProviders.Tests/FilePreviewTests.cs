using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.IO
{
    [ResourcePathFormat("{ProjectDirectory}/../Resources")]
    internal class FilePreviewTests
    {
        [TestCase("shannon.jpg")]
        [TestCase("HelloWorld.txt")]
        public void TestRetrievingFilePreviews(string fileName)
        {
            var res = ResourceInfo.From(fileName);
            Assert.That(res.File.Exists);

            var bmp = FilePreviewFactory.GetPreviewOrDefault(res);

            Assert.That(bmp, Is.Not.Null);
            Assert.That(bmp.Width, Is.GreaterThan(0));
            Assert.That(bmp.Height, Is.GreaterThan(0));

            AttachmentInfo.From($"{fileName}-prv.bmp").WriteObject(bmp.Save);

            var cfg = new FilePreviewOptions();
            cfg.ExtensionAssociatedIcon = true;
            var ico = FilePreviewFactory.GetPreviewOrDefault(res, cfg);

            Assert.That(ico, Is.Not.Null);
            Assert.That(ico.Width, Is.GreaterThan(0));
            Assert.That(ico.Height, Is.GreaterThan(0));

            AttachmentInfo.From($"{fileName}-ico.bmp").WriteObject(ico.Save);
        }
    }
}
