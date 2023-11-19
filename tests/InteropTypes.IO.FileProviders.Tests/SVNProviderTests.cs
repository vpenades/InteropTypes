using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.IO.VersionControl;

using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

namespace InteropTypes.IO
{
    internal class SVNProviderTests
    {
        [Test]
        public void LoadSVNDirectoryTree()
        {
            using (var client = new SVNDisposableFileProvider("https://github.com/vpenades/InteropTypes.git/trunk/"))
            {
                TestContext.WriteLine($"Rev:{client.LastChangeRevision} T:{client.LastChangeTime}");

                var root = client.GetDirectoryContents(null);

                _DumpDirectory(root);
            }
        }

        private static void _DumpDirectory(IDirectoryContents contents)
        {
            var files = LinkedFileInfo.Enumerate(contents, System.IO.SearchOption.AllDirectories).ToList();

            foreach(var entry in files)
            {
                var indent = string.Join("", Enumerable.Repeat("    ", entry.Depth));

                if (entry.IsDirectory)
                {
                    var rev = SVNFileProvider.GetRevisionFrom(entry); // null
                    TestContext.WriteLine($"{indent}📁 {entry.Name} T:{entry.LastModified}");
                }
                else
                {
                    var rev = SVNFileProvider.GetRevisionFrom(entry);
                    TestContext.WriteLine($"{indent}🗎 {entry.Name} Len:{entry.Length} R:{rev.Revision} T:{entry.LastModified}");

                    continue;

                    using var s = entry.CreateReadStream();

                    using var t = new System.IO.StreamReader(s);
                    var txt = t.ReadToEnd();
                    TestContext.WriteLine(txt);
                }
            }
        }
    }
}
