using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO
{
    public class FilePathInfoTests
    {
        [Test]
        public void TestOperators()
        {
            var p = new PathInfo("c:\\");

            p /= "hello";
            p /= "world";

            p = p / "1" / "2" / "file.txt";

            Assert.That(p.Path, Is.SamePath("c:\\hello\\world\\1\\2\\file.txt"));
        }
    }
}
