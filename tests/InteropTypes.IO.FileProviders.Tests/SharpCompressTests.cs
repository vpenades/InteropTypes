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

            var contents = provider.GetDirectoryContents(string.Empty);
            contents._PrintContents();

            var firstEntry = contents.FirstOrDefault(item => !item.IsDirectory);

            var bytes = StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(firstEntry);

            Assert.AreEqual(firstEntry.Length, bytes.Count);
        }
    }
}
