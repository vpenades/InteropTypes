using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Crypto
{
    internal class CryptoSerializerTests
    {
        [Test]
        public void ReadWriteTest()
        {
            var salt = Hash128.FromRandom();

            using(var context = CryptoSerializer.CreateAES("hello", salt, 10232))
            {
                for(int i=0; i < 10; ++i)
                {
                    var plainData = new Byte[1000];
                    new Random().NextBytes(plainData);

                    var encrypted = context.Encrypt(plainData);

                    var resulting = context.Decrypt(encrypted);

                    Assert.That(resulting, Is.EqualTo(plainData));
                }                
            }
        }
    }
}
