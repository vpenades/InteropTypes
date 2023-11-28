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
                Assert.That(srcStream.CanRead, Is.True);
                Assert.That(srcStream.Position, Is.EqualTo(0));
                Assert.That(srcStream.Length, Is.EqualTo(buffSize));

                var dst = XStream.ReadAllBytes(srcStream);

                Assert.That(dst, Is.EqualTo(buffer).AsCollection);
            }

            using (var srcStream = XStream.WrapList(buffer))
            {
                Assert.That(srcStream.CanRead, Is.True);
                Assert.That(srcStream.Position, Is.EqualTo(0));
                Assert.That(srcStream.Length, Is.EqualTo(buffSize));

                var dst = XStream.ReadAllBytes(srcStream);

                Assert.That(dst, Is.EqualTo(buffer).AsCollection);
            }

            using (var srcStream = XStream.WrapList(buffer.ToList()))
            {
                Assert.That(srcStream.CanRead);
                Assert.That(srcStream.Position, Is.EqualTo(0));
                Assert.That(srcStream.Length, Is.EqualTo(buffSize));

                var dst = await XStream
                    .ReadAllBytesAsync(srcStream, System.Threading.CancellationToken.None)
                    .ConfigureAwait(false);

                Assert.That(dst, Is.EqualTo(buffer).AsCollection);
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
                Assert.That(dstStream.CanWrite);
                Assert.That(dstStream.Length, Is.EqualTo(0));
                Assert.That(dstStream.Position, Is.EqualTo(0));

                dstStream.Write(buffer,0, buffer.Length);

                Assert.That(dstStream.Length, Is.EqualTo(buffer.Length));
                Assert.That(dstStream.Position, Is.EqualTo(buffer.Length));
            }

            Assert.That(list, Is.EqualTo(buffer).AsCollection);

            using (var dstStream = XStream.WrapList(list, FileMode.Create))
            {
                Assert.That(dstStream.CanWrite);
                Assert.That(dstStream.Length, Is.EqualTo(0));
                Assert.That(dstStream.Position, Is.EqualTo(0));

                await dstStream
                    .WriteAsync(buffer, System.Threading.CancellationToken.None)
                    .ConfigureAwait(false);

                Assert.That(dstStream.Length, Is.EqualTo(buffer.Length));
                Assert.That(dstStream.Position, Is.EqualTo(buffer.Length));
            }

            Assert.That(list, Is.EqualTo(buffer).AsCollection);
        }

    }
}
