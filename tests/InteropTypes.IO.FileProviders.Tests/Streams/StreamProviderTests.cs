using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class StreamProviderTests
    {
        [Test]
        public void UnsafeCastTest()
        {
            var kk = ResourceInfo.From("test.cbz");
            var xinfo = new PhysicalFileInfo(kk.File);

            Assert.That(StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(xinfo).Count, Is.Not.Zero);
            Assert.That(StreamProvider<PhysicalFileInfo>.Default.ReadAllBytesFrom(xinfo).Count, Is.Not.Zero);
            Assert.That(StreamProvider<IServiceProvider>.Default.ReadAllBytesFrom(xinfo).Count, Is.Not.Zero);            
        }

        [Test]
        public void ListWrapTest()
        {
            var provider = StreamProvider<List<Byte>>.Default;

            var list = new List<byte>();

            provider.WriteAllBytesTo(list, new byte[] { 1, 2, 3, 8 });

            provider.WriteAllBytesTo(list, new byte[] { 1, 2, 4 });

            var result = provider.ReadAllBytesFrom(list);

            Assert.That(new byte[] { 1, 2, 4 }, Is.EqualTo(result).AsCollection);
        }

        [Test]
        public void TestReaders()
        {
            var kk = ResourceInfo.From("test.cbz");

            var bytes = StreamProvider<FileInfo>.Default.ReadAllBytesFrom(kk);
            Assert.That(bytes.Count, Is.Not.Zero);
        }
    }
}
