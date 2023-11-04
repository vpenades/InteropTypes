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

            Assert.AreEqual(0, s2.Position);
            Assert.AreEqual(rnds.Length, s2.Length);
            
            var seq = XStream.ReadAllBytesSequence(s2);
            Assert.AreEqual(rnds.Length, seq.Length);

            using var s3 = XStream.WrapWithCloseActions(XStream.CreateMemoryStream(false), len => TestContext.Progress.WriteLine($"{len}"));
            XStream.WriteAllBytes(s3, seq);
            s3.Position = 0;

            var list = XStream.ReadAllBytes(s3).ToList();

            using var s4 = XStream.WrapList(list);

            rnds.Position = 0;            

            Assert.IsTrue(StreamEqualityComparer.Default.AreStreamsContentEqual(rnds, s4));
        }
    }
}
