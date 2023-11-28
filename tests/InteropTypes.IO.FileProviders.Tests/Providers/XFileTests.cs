using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO.Providers
{
    internal class XFileTests
    {
        [Test]
        public void WriteTest()
        {
            var inText = "Hello World";

            var ainfo = AttachmentInfo.From("result.txt");

            void _writer(System.IO.FileInfo f)
            {
                var xinfo = new PhysicalFileInfo(f);
                XFile.WriteAllText(xinfo, inText);
            }

            var outPath = ainfo.WriteObjectEx(_writer);

            var xinfo = new PhysicalFileInfo(outPath);

            var outText = XFile.ReadAllText(xinfo);

            Assert.That(outText, Is.EqualTo(inText));

            // ------

            var baseDir = new PhysicalDirectoryInfo(ainfo.File.Directory);

            XFile.WriteAllText(baseDir, "someFile.txt", inText);

            var path = System.IO.Path.Combine(baseDir.PhysicalPath, "someFile.txt");

            outText = System.IO.File.ReadAllText(path);

            Assert.That(outText, Is.EqualTo(inText));
        }
    }
}
