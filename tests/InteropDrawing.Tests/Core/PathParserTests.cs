using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropDrawing.Backends;

using NUnit.Framework;

namespace InteropDrawing
{
    public class PathParserTests
    {
        // https://github.com/wwwMADwww/ManuPathLib/blob/master/ManuPathTest/svg/polygon%202.svg
        [TestCase("M 9.63491,6.7967016 32.405716,11.47376 39.601628,38.63927 4.4367316,37.033764")]
        [TestCase("M 60.135988,6.7967016 82.906794,11.47376 90.102706,38.63927 54.937809,37.033764")]
        [TestCase("M 9.7556821,57.177007 32.526488,61.854066 39.7224,89.019576 4.5575037,87.41407 Z")]
        [TestCase("m 60.256759,57.177007 22.770806,4.677059 7.195912,27.16551 -35.164896,-1.605506 z")]
        public void ParsePathAndFill(string path)
        {
            using (var svg = SVGSceneDrawing2D.CreateGraphic())
            {
                svg.DrawPath(System.Numerics.Matrix3x2.Identity, path, (System.Drawing.Color.Red, System.Drawing.Color.Blue, 2));

                var document = svg.ToSVGContent();
                var docPath = TestContext.CurrentContext.UseFilePath("document.svg");

                System.IO.File.WriteAllText(docPath, document);
                TestContext.AddTestAttachment(docPath);
            }
        }
    }
}
