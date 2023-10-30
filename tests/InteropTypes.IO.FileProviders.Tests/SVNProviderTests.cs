using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

using SharpSvn;

using ShellLink.Flags;

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

                var root = client.GetFileInfo(null);

                _DumpDirectory(root,string.Empty);
            }
        }

        private static void _DumpDirectory(IFileInfo entry, string indent)
        {
            if (entry.IsDirectory)
            {
                TestContext.WriteLine($"{indent}📁 {entry.Name}");

                var children = (entry as IDirectoryContents).ToList();

                foreach (var child in children.OrderBy(item => item.Name))
                {
                    _DumpDirectory(child, indent + "    ");                    
                }
            }
            else
            {
                var rev = SVNFileProvider.GetRevisionFrom(entry);

                TestContext.WriteLine($"{indent}🗎 {entry.Name} Len:{entry.Length} R:{rev.Revision} T:{entry.LastModified}");

                return;

                using var s = entry.CreateReadStream();

                using var t = new System.IO.StreamReader(s);
                var txt = t.ReadToEnd();
                TestContext.WriteLine(txt);
            }
        }
    }
}
