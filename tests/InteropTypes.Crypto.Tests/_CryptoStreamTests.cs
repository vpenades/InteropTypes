using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using NUnit.Framework;

namespace InteropTypes.Crypto
{
    internal class _CryptoStreamTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ReadWriteTest(bool useSalt)
        {
            var planData = SymmetricFactory.GetRandomBytes(1000);

            using var m = new System.IO.MemoryStream();

            using (var cw = _CryptoStream.CreateFromAES(m, System.Security.Cryptography.CryptoStreamMode.Write, "hello", useSalt))
            {
                cw.Write(planData);
            }

            Assert.That(m.CanRead, Is.True);

            m.Position = 0;

            using (var cr = _CryptoStream.CreateFromAES(m, System.Security.Cryptography.CryptoStreamMode.Read, "hello", useSalt))
            {
                using(var b = new System.IO.BinaryReader(cr))
                {
                    var result = b.ReadBytes(planData.Length);

                    Assert.That(result, Is.EqualTo(planData));
                }
            }
        }
    }
}
