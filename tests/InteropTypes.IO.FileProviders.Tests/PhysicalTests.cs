using System.Linq;
using System.Reflection;



using NUnit.Framework;

namespace InteropTypes.IO
{
    public class PhysicalTests
    {
        [Test]
        public void CreateAndEnumerateReference()
        {
            using var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(TestContext.CurrentContext.TestDirectory);

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