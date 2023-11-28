using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class StreamComparisonContextTests
    {
        [Test]
        public void TestCompareSmallStreams()
        {
            var a = new System.IO.MemoryStream(new byte[] { 1, 2, 3 });
            var b = new System.IO.MemoryStream(new byte[] { 1, 2, 4 });
            var c = new System.IO.MemoryStream(new byte[] { 1, 2, 3, 4 });
            var d = new System.IO.MemoryStream(new byte[] { 1, 2, 4 });

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(a, b), Is.False);
            a.Position= 0;
            b.Position= 0;

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(b, d), Is.True);
            b.Position = 0;
            d.Position = 0;

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(a, c), Is.False);
            a.Position = 0;
            c.Position = 0;
        }

        [Test]
        public void TestCompareLargeStreams()
        {
            var a = new RandomStream(int.MaxValue / 8, 2);
            var b = new RandomStream(int.MaxValue / 8, 3);
            var c = new RandomStream(int.MaxValue / 8, 2);

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(a, b), Is.False);
            a.Position = 0;
            b.Position = 0;

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(a, c), Is.True);
            a.Position = 0;
            c.Position = 0;
        }
    }


    
}
