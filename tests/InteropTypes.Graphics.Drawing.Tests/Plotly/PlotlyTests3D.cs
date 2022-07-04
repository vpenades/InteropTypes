using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    public class PlotlyTests3D
    {
        [Test]
        public void DrawVolumes()
        {
            var scene = new Record3D();
            scene.DrawPivot(System.Numerics.Matrix4x4.Identity, 2);
            scene.DrawCube(System.Numerics.Matrix4x4.CreateTranslation(5, 0, 0), Color.Red, Color.Green, Color.Blue);
            scene.DrawCube(System.Numerics.Matrix4x4.CreateTranslation(7, 0, 0), (Color.Black, 0.1f));

            var doc = new PlotlyDocumentBuilder().Draw(scene);

            AttachmentInfo.From("Drawing.html").WriteAllText(doc.ToHtml());
        }
    }
}
