using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace InteropTypes.IO.Providers
{
    public class PhysicalTests
    {
        [Test]
        public void CreateAndEnumerateReferenceMS()
        {
            var dinfo = new System.IO.DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            using var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(dinfo.FullName);

            _TestProvider(provider);
        }

        [Test]
        public void CreateAndEnumerateReferenceInterop()
        {
            var dinfo = new System.IO.DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            var provider = PhysicalFileProvider.Create(dinfo);

            _TestProvider(provider);
        }

        private static void _TestProvider(Microsoft.Extensions.FileProviders.IFileProvider provider)
        {
            Assert.That(provider.GetDirectoryContents(string.Empty).Exists);

            var contents = provider.GetDirectoryContents(string.Empty);
            contents._PrintContents();
        }

        [Test]
        public void CreateAndEnumerate()
        {
            var dinfo = new System.IO.DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            var provider = new PhysicalDirectoryInfo(dinfo);

            provider._PrintContents();
        }
    }
}