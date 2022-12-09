using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    internal class BindableBitmapTests
    {
        [Test]
        public void EmptyTests()
        {
            var bindable = new BindableBitmap();
            bindable.UpdateFromQueue();

            bindable.Enqueue(default);
            bindable.UpdateFromQueue();

            bindable.Enqueue(new MemoryBitmap<Pixel.RGB24>(16,16));
            bindable.UpdateFromQueue();

            bindable.Enqueue(default);
            bindable.UpdateFromQueue();

        }

    }
}
