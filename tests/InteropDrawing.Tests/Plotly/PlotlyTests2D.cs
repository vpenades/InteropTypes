using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace InteropDrawing.Backends.Plotly
{
    public class PlotlyTests2D
    {
        [Test]
        public void DrawLines()
        {
            var doc = new PlotlyDocumentBuilder();
            using(var dc = doc.CreateTraces2DContext())
            {
                dc.DrawLines(15, (System.Drawing.Color.Red, System.Drawing.Color.Blue,13), (1, 1), (2, 2), (3, 8),(4,7));

                dc.DrawCircle((5, 4), 10, System.Drawing.Color.Red);
                dc.DrawCircle((7, 4), 50, System.Drawing.Color.Blue);
                dc.DrawCircle((8, 5), 20, System.Drawing.Color.Green);
                dc.DrawCircle((9, 6), 30, System.Drawing.Color.Black);

                dc.DrawPolygon((System.Drawing.Color.Red, System.Drawing.Color.Blue, 13), (1.1f, 1), (2, 1), (1.5f,3), (1.1f, 2));

                dc.DrawPolygon(System.Drawing.Color.Yellow, (1.1f, 10), (2, 10), (1.5f, 30), (1.1f, 20));
            }

            TestContext.CurrentContext.Attach("Drawing.html", doc, (key, val) => val.SaveHtml(key));
        }

        [Test]
        public void DrawShapes()
        {
            var doc = new PlotlyDocumentBuilder();
            using (var dc = doc.CreateShapes2DContext())
            {
                dc.DrawCircle((2, 2), 7, System.Drawing.Color.Red);
            }

            TestContext.CurrentContext.Attach("Drawing.html", doc, (key, val) => val.SaveHtml(key));
        }
    }
}
