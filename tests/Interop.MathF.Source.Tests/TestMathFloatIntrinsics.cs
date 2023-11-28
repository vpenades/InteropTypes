using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Interop.MathFSource.Tests
{
    [Category("MathF")]
    public class SystemMathFloat
    {
        [Test]
        public void TestIntrinsics()
        {
            TestIntrinsics(Math.Round, MathF.Round, 0);
            TestIntrinsics(Math.Floor, MathF.Floor, 0);
            TestIntrinsics(Math.Ceiling, MathF.Ceiling, 0);
            TestIntrinsics(Math.Truncate, MathF.Truncate, 0);

            TestIntrinsics(x => Math.Sign(x), x => MathF.Sign(x), 0);

            TestIntrinsics(Math.Atan, MathF.Atan, 0.0000001f);
            TestIntrinsics(Math.Acos, MathF.Acos, 0.0000001f);
            TestIntrinsics(Math.Asin, MathF.Asin, 0.0000001f);

            TestIntrinsics(Math.Tan, MathF.Tan, 0.000001f);
            TestIntrinsics(Math.Cos, MathF.Cos, 0.0000001f);
            TestIntrinsics(Math.Sin, MathF.Sin, 0.0000001f);            
        }


        private static void TestIntrinsics(Func<double, double> doubleFunc, Func<float, float> singleFunc, float tolerance)
        {
            Assert.That(singleFunc(-1.5f), Is.EqualTo(doubleFunc(-1.5)).Within(tolerance));
            Assert.That(singleFunc(-0.6f), Is.EqualTo(doubleFunc(-0.6)).Within(tolerance));
            Assert.That(singleFunc(-0.5f), Is.EqualTo(doubleFunc(-0.5)).Within(tolerance));
            Assert.That(singleFunc(-0.4f), Is.EqualTo(doubleFunc(-0.4)).Within(tolerance));

            Assert.That(singleFunc(0), Is.EqualTo(doubleFunc(0)).Within(tolerance));

            Assert.That(singleFunc(0.4f), Is.EqualTo(doubleFunc(0.4)).Within(tolerance));
            Assert.That(singleFunc(0.5f), Is.EqualTo(doubleFunc(0.5)).Within(tolerance));
            Assert.That(singleFunc(0.6f), Is.EqualTo(doubleFunc(0.6)).Within(tolerance));
            Assert.That(singleFunc(1.5f), Is.EqualTo(doubleFunc(1.5)).Within(tolerance));            
        }

        [Test]
        public void TestIEEERemainder()
        {
            Assert.That(MathF.IEEERemainder(2, 1.6f), Is.EqualTo(Math.IEEERemainder(2, 1.6f)).Within(0.00001f));            
        }
    }
}
