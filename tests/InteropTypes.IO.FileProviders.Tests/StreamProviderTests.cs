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

            Assert.NotZero(StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(xinfo).Count);
            Assert.NotZero(StreamProvider<PhysicalFileInfo>.Default.ReadAllBytesFrom(xinfo).Count);
            Assert.NotZero(StreamProvider<IServiceProvider>.Default.ReadAllBytesFrom(xinfo).Count);            
        }

        [Test]
        public void ListWrapTest()
        {
            var provider = StreamProvider<List<Byte>>.Default;

            var list = new List<byte>();

            provider.WriteAllBytesTo(list, new byte[] { 1, 2, 3, 8 });

            provider.WriteAllBytesTo(list, new byte[] { 1, 2, 4 });

            var result = provider.ReadAllBytesFrom(list);

            CollectionAssert.AreEqual(result, new byte[] { 1, 2, 4 });
        }

        [Test]
        public void TestReaders()
        {
            var kk = ResourceInfo.From("test.cbz");

            var bytes = StreamProvider<FileInfo>.Default.ReadAllBytesFrom(kk);
            Assert.NotZero(bytes.Count);
        }
    }
}
