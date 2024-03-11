using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Crypto
{
    internal class LocalNugetCredentialsTests
    {
        [Test]
        public void LoadNugetCredentials()
        {
            var d = new System.IO.DirectoryInfo(TestContext.CurrentContext.TestDirectory);            

            var creds1 = CredentialsFactory.CreateFromNugetConfig("testCredentialsClear", d);
            Assert.That(creds1, Is.Not.Null);
            Assert.That(creds1.UserName, Is.EqualTo("test1"));
            Assert.That(creds1.Password, Is.EqualTo("12345"));

            var password = NuGet.Configuration.EncryptionUtility.EncryptString("12345");

            var creds2 = CredentialsFactory.CreateFromNugetConfig("testCredentials", d);
            Assert.That(creds2, Is.Not.Null);
            Assert.That(creds2.UserName, Is.EqualTo("test1"));
            Assert.That(creds2.Password, Is.EqualTo("12345"));
        }
    }
}
