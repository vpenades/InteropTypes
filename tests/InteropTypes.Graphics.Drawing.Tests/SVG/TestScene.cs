﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    [Category("InteropDrawing SVG")]
    public class TestScene
    {
        [Test]
        public void TestSVG()
        {
            using (var svg = SVGSceneDrawing2D.CreateGraphic())
            {
                svg.DrawLine((0, 0), (100, 100), 2, (COLOR.SkyBlue, LineCapStyle.Round, LineCapStyle.Triangle));
                svg.DrawRectangle((10, 10), (80, 80), (COLOR.Blue, 4));
                svg.DrawEllipse(new Vector2(50, 50), 70, 70, (COLOR.Red, 2));

                AttachmentInfo.From("document.svg").WriteAllText(svg.ToSVGContent());
            }
        }

        [Test]
        public void RenderSceneToSVG()
        {
            using var svg = SVGSceneDrawing2D.CreateGraphic();

            var scene = SceneFactory.CreateRecord3D("Scene1");

            scene.DrawTo(svg, 1024, 1024, new Vector3(7, 5, 20));

            AttachmentInfo.From("document.svg").WriteAllText(svg.ToSVGContent());
        }
    }
}
