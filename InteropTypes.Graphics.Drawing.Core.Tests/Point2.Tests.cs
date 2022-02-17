using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal partial class TypeTests
    {
        [Test]
        public void Point2Tests()
        {
            Assert.AreEqual(8, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Point2)));
        }
    }
}
