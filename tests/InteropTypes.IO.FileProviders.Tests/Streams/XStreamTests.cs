using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class XStreamTests
    {
        [Test]
        public void RoundTripTest1()
        {
            using var rnds = new RandomStream(100000, 4);

            using var s2 = XStream.WrapWithCloseActions(XStream.CreateMemoryStream(rnds, true), len=> TestContext.Progress.WriteLine($"{len}"));

            Assert.That(s2.Position, Is.EqualTo(0));
            Assert.That(s2.Length, Is.EqualTo(rnds.Length));
            
            var seq = XStream.ReadAllBytesSequence(s2);
            Assert.That(seq.Length, Is.EqualTo(rnds.Length));

            using var s3 = XStream.WrapWithCloseActions(XStream.CreateMemoryStream(false), len => TestContext.Progress.WriteLine($"{len}"));
            XStream.WriteAllBytes(s3, seq);
            s3.Position = 0;

            var list = XStream.ReadAllBytes(s3).ToList();

            using var s4 = XStream.WrapList(list);

            rnds.Position = 0;

            Assert.That(StreamEqualityComparer.Default.AreStreamsContentEqual(rnds, s4), Is.True);
        }
    }
}
