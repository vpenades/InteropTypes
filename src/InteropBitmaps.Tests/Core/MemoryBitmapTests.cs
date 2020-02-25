using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    [Category("Core")]
    public class MemoryBitmapTests
    {
        [Test]
        public void CreateMemoryBitmap()
        {
            var m1 = new MemoryBitmap(16, 16, 4);

            var m2 = new MemoryBitmap(new Byte[16 * 16 * 4], 16, 16, 4);
        }
    }
}
