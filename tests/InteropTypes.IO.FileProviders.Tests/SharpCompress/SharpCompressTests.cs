using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteropTypes.IO.Archives;
using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

namespace InteropTypes.IO.Archives
{
    internal class SharpCompressTests
    {
        [Test]
        public void StreamProvidersTest()
        {
            var path = ResourceInfo.From("test.cbz");

            using var archive = SharpCompress.Archives.ArchiveFactory.Open(path.File);            

            var firstEntry = archive.Entries.FirstOrDefault(item => !item.IsDirectory);

            var bytes = StreamProvider<SharpCompress.Archives.IArchiveEntry>.Default.ReadAllBytesFrom(firstEntry);

            Assert.AreEqual(firstEntry.Size, bytes.Count);
        }

        [Test]
        public void ArchiveProvidersTest()
        {
            var path = ResourceInfo.From("test.cbz");

            using var provider = SharpCompressFileProvider.Open(path);

            var kk = provider.GetDirectoryContents(null);
            kk._PrintContents();            

            var finfo = provider.GetFileInfo("c");

            var contents1 = provider.GetDirectoryContents(null);
            var contents2 = provider.GetDirectoryContents("c\\");

            Assert.AreEqual(4, contents1.Count());
            Assert.AreEqual(1, contents1.Where(item => item.IsDirectory).Count());
            contents1._PrintContents();            

            Assert.IsTrue(provider.GetDirectoryContents("c").Exists);
            Assert.IsTrue(contents2.Exists);
            Assert.IsTrue(contents2.Any(item => item.IsDirectory && item.Name=="c"));
            Assert.IsTrue(provider.GetFileInfo("a.txt").Exists);
            Assert.IsTrue(provider.GetFileInfo("c\\c\\b.txt").Exists);
            Assert.IsTrue(provider.GetFileInfo("c/c\\b.txt").Exists);

            var firstEntry = contents1.FirstOrDefault(item => !item.IsDirectory);

            var bytes = StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(firstEntry);

            Assert.AreEqual(firstEntry.Length, bytes.Count);
        }
    }
}
