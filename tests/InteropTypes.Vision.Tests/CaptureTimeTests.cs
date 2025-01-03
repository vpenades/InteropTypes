﻿using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace InteropTypes.Vision
{
    public class CaptureTimeTests
    {
        [Test]
        public void TestTimePrecission()
        {
            long t0 = 1000;
            long t1 = 1001;

            var d0 = new DateTime(t0, DateTimeKind.Utc);
            var d1 = new DateTime(t1, DateTimeKind.Utc);

            var tt = d1 - d0;

            var ttt = tt.Ticks;

            Assert.That(ttt, Is.EqualTo(1));
        }        
    }
}
