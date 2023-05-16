using System.Linq;
using System.Reflection;

using Microsoft.Extensions.FileProviders;

using NUnit.Framework;

namespace InteropTypes.IO
{
    public class PhysicalTests
    {
        [Test]
        public void CreateAndEnumerate()
        {
            var provider = new PhysicalFileProvider(TestContext.CurrentContext.TestDirectory);

            var contents = provider.GetDirectoryContents(string.Empty);
            contents._PrintContents();
        }

        
    }
}