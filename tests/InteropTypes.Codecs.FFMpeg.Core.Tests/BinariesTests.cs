using System;

using NUnit.Framework;

namespace InteropTypes.Codecs
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var path = InteropTypes.Graphics.Backends._Binaries.UseFFMpegDirectory();

            var exe = System.IO.Path.Combine(path.FullName, "ffmpeg.exe");

            Assert.IsTrue(System.IO.File.Exists(exe)); 
        }
    }
}