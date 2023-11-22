using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class ListWrapperStreamTests
    {

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1024)]
        [TestCase(180000)]
        public async Task TestReadListAsStreamAsync(int buffSize)
        {
            var buffer = new Byte[buffSize];

            new Random().NextBytes(buffer);

            using (var srcStream = XStream.WrapList(buffer.ToList()))
            {
                Assert.IsTrue(srcStream.CanRead);
                Assert.AreEqual(0, srcStream.Position);
                Assert.AreEqual(buffSize, srcStream.Length);

                var dst = XStream.ReadAllBytes(srcStream);

                CollectionAssert.AreEqual(buffer, dst);
            }

            using (var srcStream = XStream.WrapList(buffer))
            {
                Assert.IsTrue(srcStream.CanRead);
                Assert.AreEqual(0, srcStream.Position);
                Assert.AreEqual(buffSize, srcStream.Length);

                var dst = XStream.ReadAllBytes(srcStream);

                CollectionAssert.AreEqual(buffer, dst);
            }

            using (var srcStream = XStream.WrapList(buffer.ToList()))
            {
                Assert.IsTrue(srcStream.CanRead);
                Assert.AreEqual(0, srcStream.Position);
                Assert.AreEqual(buffSize, srcStream.Length);

                var dst = await XStream
                    .ReadAllBytesAsync(srcStream, System.Threading.CancellationToken.None)
                    .ConfigureAwait(false);

                CollectionAssert.AreEqual(buffer, dst);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1024)]
        [TestCase(180000)]
        public async Task TestWriteListAsStreamAsync(int buffSize)
        {
            var buffer = new Byte[buffSize];
            new Random().NextBytes(buffer);

            var list = new List<byte>();

            list.Add(67);

            using (var dstStream = XStream.WrapList(list, FileMode.Create))
            {                
                Assert.IsTrue(dstStream.CanWrite);
                Assert.AreEqual(0, dstStream.Length);
                Assert.AreEqual(0, dstStream.Position);

                dstStream.Write(buffer,0, buffer.Length);

                Assert.AreEqual(buffer.Length, dstStream.Length);
                Assert.AreEqual(buffer.Length, dstStream.Position);
            }

            CollectionAssert.AreEqual(buffer, list);

            using (var dstStream = XStream.WrapList(list, FileMode.Create))
            {
                Assert.IsTrue(dstStream.CanWrite);
                Assert.AreEqual(0, dstStream.Length);
                Assert.AreEqual(0, dstStream.Position);

                await dstStream
                    .WriteAsync(buffer, System.Threading.CancellationToken.None)
                    .ConfigureAwait(false);

                Assert.AreEqual(buffer.Length, dstStream.Length);
                Assert.AreEqual(buffer.Length, dstStream.Position);
            }

            CollectionAssert.AreEqual(buffer, list);
        }

    }
}
