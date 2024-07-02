using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Crypto;
using InteropTypes.IO.VersionControl;

using Microsoft.Extensions.FileProviders;
using Microsoft.VisualBasic.Logging;

using NUnit.Framework;

using SharpSvn;

namespace InteropTypes.IO
{
    internal class SVNProviderTests
    {
        [Explicit]
        [Test]
        public void LogSVNTest()
        {
            using (var client = new SVNDisposableFileProvider("https://github.com/vpenades/InteropTypes.git/trunk/",15))
            {
                var log = client.GetLog(client.LastChangeRevision - 30);
            }
        }

        [Explicit]
        [TestCase(15, 1, "9C1AD3091FC8210C1A790F08660871ECC4F7CA46878D4FB89DBB4BC897E27870")]
        [TestCase(160, 156, "A7473F98566B6002CE75082C13470EB932159A713B68C2D1B58AA297EC220C82")]
        [TestCase(long.MaxValue, -1, "")]
        public void LoadSVNDirectoryTree(long revNumber, int readmeRev, string readmeSha256)
        {
            var rev = revNumber == long.MaxValue
                ? SvnRevision.Head
                : (SvnRevision)revNumber;

            using (var client = new SVNDisposableFileProvider("https://github.com/vpenades/InteropTypes.git/trunk/", rev))
            {
                TestContext.WriteLine($"LastR:{client.LastChangeRevision} LastT:{client.LastChangeTime}");

                var root = client.GetDirectoryContents(null);

                _DumpDirectory(root);

                if (readmeRev > 0)
                {
                    var finfoA = root.FirstOrDefault(item => !item.IsDirectory && item.Name == "README.md");
                    Assert.That(finfoA, Is.Not.Null);

                    var finfoB = client.GetFileInfo("README.md");
                    Assert.That(finfoB, Is.Not.Null);

                    Assert.That(SVNFileProvider.GetRevisionNumberFrom(finfoB), Is.EqualTo(readmeRev));

                    var sha256 = Hash256.Sha256FromFile(finfoB).ToHexString();

                    Assert.That(readmeSha256, Is.EqualTo(sha256));                    
                }
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
