using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            Assert.NotZero(StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(xinfo).Length);
            Assert.NotZero(StreamProvider<PhysicalFileInfo>.Default.ReadAllBytesFrom(xinfo).Length);
            Assert.NotZero(StreamProvider<IServiceProvider>.Default.ReadAllBytesFrom(xinfo).Length);            
        }

        [Test]
        public void TestReaders()
        {
            var kk = ResourceInfo.From("test.cbz");

            var bytes = StreamProvider<FileInfo>.Default.ReadAllBytesFrom(kk);
            Assert.NotZero(bytes.Length);
        }
    }
}
