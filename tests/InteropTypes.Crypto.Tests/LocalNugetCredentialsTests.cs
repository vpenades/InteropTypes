using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Windows.Networking.Connectivity;

namespace InteropTypes.Crypto
{
    internal class LocalNugetCredentialsTests
    {
        [Test]
        public void LoadNugetCredentials()
        {
            var d = new System.IO.DirectoryInfo(TestContext.CurrentContext.TestDirectory);            

            var icreds1 = CredentialsFactory.CreateFromNugetConfig("testCredentialsClear", d);

            var creds1 = icreds1.GetCredential(new Uri("https://www.google.com"), String.Empty);

            Assert.That(creds1, Is.Not.Null);
            Assert.That(creds1.UserName, Is.EqualTo("test1"));
            Assert.That(creds1.Password, Is.EqualTo("12345"));

            var password = NuGet.Configuration.EncryptionUtility.EncryptString("12345");


            // this no longer works because increased security when trying to access non clear password

            /*
             
            var icreds2 = CredentialsFactory.CreateFromNugetConfig("testCredentials", d);
            var creds2 = icreds2.GetCredential(new Uri("https://www.google.com"), String.Empty);           

            Assert.That(creds2, Is.Not.Null);
            Assert.That(creds2.UserName, Is.EqualTo("test1"));
            Assert.That(creds2.Password, Is.EqualTo("12345"));
            */
        }
    }
}
