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
            Assert.That(Score.Zero, Is.EqualTo(new Score(0, false)));
            Assert.That(Score.Ok, Is.EqualTo(new Score(1, true)));

            Assert.That(Score.Zero, Is.LessThan(Score.Ok));
            Assert.That(Score.Ok > Score.Zero);
            Assert.That(Score.Zero < Score.Ok);

            Assert.That(new Score(1, false) > new Score(0, false));
            Assert.That(new Score(0, true) > new Score(1, false));

            var s1 = new Score(0.54234f, true);

            Assert.That(Score.Ok, Is.Not.EqualTo(s1));

            var sig0 = new Score(-17, Score.ResultType.Sigmoid);
            var sig1 = new Score(0, Score.ResultType.Sigmoid);
            var sig2 = new Score(0.1f, Score.ResultType.Sigmoid);
            var sig3 = new Score(0.5f, Score.ResultType.Sigmoid);
            var sig4 = new Score(1, Score.ResultType.Sigmoid);
            var sig5 = new Score(17, Score.ResultType.Sigmoid);
        }
    }
}
