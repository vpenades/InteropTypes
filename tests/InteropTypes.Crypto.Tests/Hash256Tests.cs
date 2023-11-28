using System;
using System.Linq;

using NUnit.Framework;

namespace InteropTypes.Crypto
{
    public class Hash256Tests
    {
        [Test]
        public void TestCreateSha256()
        {
            var bytes = new byte[200505];
            new Random().NextBytes(bytes);
            
            var h = System.Security.Cryptography.SHA256.HashData(bytes);
            var href = new Hash256(h);

            var h1 = Hash256.Sha256FromList(bytes);
            Assert.That(h1, Is.EqualTo(href));

            var h2 = Hash256.Sha256FromList(bytes.ToList());
            Assert.That(h2, Is.EqualTo(href));

            var h3 = Hash256.Sha256FromList(bytes.Select(v=>v));
            Assert.That(h3, Is.EqualTo(href));
        }
    }
}