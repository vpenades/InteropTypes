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
        public void Point3Tests()
        {
            Assert.AreEqual(12, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Point3)));

            Assert.AreEqual(0, new Point3(-3, 0, 0).DominantAxis);
            Assert.AreEqual(1, new Point3(0, -3, 0).DominantAxis);
            Assert.AreEqual(2, new Point3(0, 0, -3).DominantAxis);
        }
    }
}
