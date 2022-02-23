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
        public void ImageStyleTests()
        {
            var source = new ImageSource("hello world", (0, 0), (10, 10), (5, 5));

            var style = _GetStyle(source);
        }

        private static ImageStyle _GetStyle(ImageStyle style)
        {
            return style;
        }
    }
}
