using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Server;

using NUnit.Framework;

namespace InteropBitmaps.Core
{
    [Category("Core")]
    public class HashCodeTests
    {
        [Test]
        public void TestHashCodes()
        {
            var gray = new MemoryBitmap<Byte>(256, 256, Pixel.Standard.Gray8);

            var rnd = new Random(117);
            for(int i=0; i < 256*256; ++i)
            {
                gray.SetPixel(i & 255, i / 256, (Byte)rnd.Next());
            }

            var span = gray.AsSpanBitmap();
            var less = span.AsTypeless();

            var hash = gray.GetHashCode();

            Assert.AreEqual(-929038500, hash);
            Assert.AreEqual(hash, gray.AsTypeless().GetHashCode());
            Assert.AreEqual(hash, span.GetHashCode());
            Assert.AreEqual(hash, less.GetHashCode());
        }
    }
}
