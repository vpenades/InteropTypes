using System.Linq;

using NUnit.Framework;

namespace InteropTypes
{
    public class LargeByteArrayTests
    {
        [Test]
        public void BasicTests()
        {
            var array = new Memory.LargeByteArray(256);

            array[2] = 255;
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(0));
            Assert.That(array[2], Is.EqualTo(255));
            Assert.That(array[3], Is.EqualTo(0));

            Assert.That(array.Count() == array.Length);
        }

        [Test]
        public void TestSmallMerges()
        {
            var a = new Memory.LargeByteArray(3);
            a.Slice(0).Fill(1);

            var b = new Memory.LargeByteArray(6);
            b.Slice(0).Fill(2);

            var merged = Memory.LargeByteArray.Merge(new[]  { a, b });
            Assert.That(merged.Length, Is.EqualTo(a.Length + b.Length));
            var mergedData = merged.Slice(0).ToArray();
        }
    }
}