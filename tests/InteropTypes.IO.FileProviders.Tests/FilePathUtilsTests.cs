using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class FilePathUtilsTests
    {
        [Test]
        public void TestCompositeExtensions()
        {            
            string _GetExt(string name, int dots)
            {
                return FilePathUtils.TryGetCompositedExtension(name, dots, out var ext) ? ext : "<ERROR>";
            }

            Assert.That(_GetExt(".1", 1), Is.EqualTo(".1"));
            Assert.That(_GetExt("file.1", 1), Is.EqualTo(".1"));
            Assert.That(_GetExt("file.1.2", 1), Is.EqualTo(".2"));
            Assert.That(_GetExt("file.1.2", 2), Is.EqualTo(".1.2"));
            Assert.That(_GetExt(".1.2", 2), Is.EqualTo(".1.2"));
            Assert.That(_GetExt(".1.2.3", 2), Is.EqualTo(".2.3"));
            Assert.That(_GetExt(".1.2.3", 3), Is.EqualTo(".1.2.3"));
            Assert.That(_GetExt(".2ext.2", 2), Is.EqualTo(".2ext.2"));

            Assert.That(_GetExt(".1", 2), Is.EqualTo("<ERROR>"));
            Assert.That(_GetExt("1", 2), Is.EqualTo("<ERROR>"));
            Assert.That(_GetExt(".1.2", 3), Is.EqualTo("<ERROR>"));
        }
    }
}
