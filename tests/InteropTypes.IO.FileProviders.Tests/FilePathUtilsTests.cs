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

            Assert.AreEqual(".1", _GetExt(".1", 1));
            Assert.AreEqual(".1", _GetExt("file.1", 1));
            Assert.AreEqual(".2", _GetExt("file.1.2", 1));
            Assert.AreEqual(".1.2", _GetExt("file.1.2", 2));
            Assert.AreEqual(".1.2", _GetExt(".1.2", 2));
            Assert.AreEqual(".2.3", _GetExt(".1.2.3", 2));
            Assert.AreEqual(".1.2.3", _GetExt(".1.2.3", 3));
            Assert.AreEqual(".2ext.2", _GetExt(".2ext.2", 2));

            Assert.AreEqual("<ERROR>", _GetExt(".1", 2));
            Assert.AreEqual("<ERROR>", _GetExt("1", 2));
            Assert.AreEqual("<ERROR>", _GetExt(".1.2", 3));
        }
    }
}
