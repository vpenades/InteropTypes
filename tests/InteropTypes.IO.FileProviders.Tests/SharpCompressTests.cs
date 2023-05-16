using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class SharpCompressTests
    {
        [Test]
        public void AndEnumerate()
        {
            var path = ResourceInfo.From("test.cbz");

            using var provider = ArchiveFileProvider.Create(path);

            var contents = provider.GetDirectoryContents(string.Empty);
            contents._PrintContents();
        }
    }
}
