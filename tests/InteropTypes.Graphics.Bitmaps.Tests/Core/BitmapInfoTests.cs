using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Core")]
    public class BitmapInfoTests
    {
        [Test]
        public void CheckBitmapInfoEquality()
        {
            // Should these two be considered 'Equal' ?   The stride is essentially a "technicality" from the point of view of the content.
            var a = new BitmapInfo(10, 10, Pixel.Alpha8.Format);
            var b = new BitmapInfo(10, 10, Pixel.Alpha8.Format, 15);

            // having the same hash code does not mean they're equal.
            Assert.That(b.GetHashCode(), Is.EqualTo(a.GetHashCode()));

            // The layout comparison should be equal.
            Assert.That(b.Layout, Is.EqualTo(a.Layout));

            // structure wise comparisons is not equal.
            Assert.That(b, Is.Not.EqualTo(a));
        }

    }
}
