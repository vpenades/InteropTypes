using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace InteropTypes.Vision
{
    public class ScoreTests
    {
        [Test]
        public void TestEquality()
        {
            Assert.AreEqual(new Score(0, false), Score.Zero);
            Assert.AreEqual(new Score(1, true), Score.Ok);

            Assert.Less(Score.Zero, Score.Ok);
            Assert.IsTrue(Score.Ok > Score.Zero);
            Assert.IsTrue(Score.Zero < Score.Ok);

            Assert.IsTrue(new Score(1, false) > new Score(0, false));
            Assert.IsTrue(new Score(0, true) > new Score(1, false));

            var s1 = new Score(0.54234f, true);

            Assert.AreNotEqual(s1, Score.Ok);

            var sig0 = new Score(-17, Score.ResultType.Sigmoid);
            var sig1 = new Score(0, Score.ResultType.Sigmoid);
            var sig2 = new Score(0.1f, Score.ResultType.Sigmoid);
            var sig3 = new Score(0.5f, Score.ResultType.Sigmoid);
            var sig4 = new Score(1, Score.ResultType.Sigmoid);
            var sig5 = new Score(17, Score.ResultType.Sigmoid);
        }
    }
}
