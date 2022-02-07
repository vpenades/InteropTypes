using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    static class SceneFactory
    {
        private static string AssetsDir => System.IO.Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Assets");
        

        public static Record3D CreateRecord3D(string name)
        {
            switch (name)
            {
                case "Scene1": return CreateDefaultScene1();
                case "Thunderbird1": return CreateThunderbirdsRocket();
                case "DNA": return CreateDNAScene();
                default: throw new ArgumentException();
            }
        }

        private static Record3D CreateDefaultScene1()
        {
            var context = new Record3D();

            context.DrawPivot(Matrix4x4.CreateTranslation(-10, 0, -10), 2);

            float diamenter = 1;

            context.DrawSegment(Vector3.Zero, Vector3.One * 3, diamenter, (COLOR.White, LineCapStyle.Triangle, LineCapStyle.Triangle));
            context.DrawSegment(Vector3.One * 3, Vector3.UnitX * 7, diamenter, COLOR.Blue);
            context.DrawSegment(Vector3.One * 3, Vector3.One * 3, diamenter, COLOR.Black);

            context.DrawSurface((COLOR.Red.WithAlpha(180), COLOR.Yellow, 0.25f), new Vector3(-1, 0, 10), new Vector3(1, 0, 10), new Vector3(0, 1, 10));

            context.DrawSegment(Vector3.Zero, Vector3.UnitX * 7, diamenter, COLOR.Violet);

            context.DrawSegment(new Vector3(9, 0, 0), new Vector3(9, 10, 0), diamenter, ((COLOR.Red, COLOR.Black, 0.1f), LineCapStyle.Round, LineCapStyle.Round));
            context.DrawSphere(new Vector3(-9, 0, 0), 2, (COLOR.Red, COLOR.Blue, 0.1f));

            // degenerated line falls back to sphere
            context.DrawSegment(new Vector3(0, 9, 0), new Vector3(0, 9, 0), 2, COLOR.Yellow);

            return context;
        }

        private static Record3D CreateThunderbirdsRocket()
        {
            var context = new Record3D();

            var style = new OutlineFillStyle(COLOR.Red, COLOR.Black, 0.1f);

            context.DrawSphere(Vector3.UnitY, 2.5f, style);

            context.DrawSegment((0, 4, 0), (0, 8, 0), 1.5f, (style, LineCapStyle.Round, LineCapStyle.Triangle));


            for (int i = 0; i < 3; ++i)
            {
                var angle = (i * 120) * (float)Math.PI / 180f;
                var h = new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));

                var a = new Vector3(0, 4, 0) + h * 0.5f;
                var b = new Vector3(0, 2, 0) + h * 2;
                var c = new Vector3(0, 0, 0) + h * 2;
                var d = new Vector3(0, -2, 0) + h * 2;
                var e = new Vector3(0, 0.15f, 0) + h * 0.5f;

                context.DrawSegments(Point3.Array(a, b ,c), 0.5f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));                
                context.DrawSegment(c, d, 1f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));

                // connection to central sphere
                context.DrawSegment((c + d) * 0.5f, e, 0.25f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));

                // exhaust plume
                context.DrawSegment(d - Vector3.UnitY, d - Vector3.UnitY * 3, 0.5f, ((COLOR.White, COLOR.Blue.WithAlpha(150), 0.5f), LineCapStyle.Round, LineCapStyle.Triangle));
            }

            return context;
        }

        private static Record3D CreateDNAScene()
        {
            var target = new Record3D();

            for (int i = 0; i < 20; ++i)
            {
                var angle = (float)i / 3;

                var x = (float)Math.Cos(angle) * 10;
                var z = (float)Math.Sin(angle) * 10;
                var y = 10 + i * 10;

                target.DrawSphere(new Vector3(x, y, z), 2, COLOR.SkyBlue);
                target.DrawSphere(new Vector3(-x, y, -z), 2, COLOR.Violet);

                x *= 0.5f;
                z *= 0.5f;

                target.DrawSegment(new Vector3(x, y, z), new Vector3(-x, y, -z), 0.5f, (COLOR.Blue, LineCapStyle.Triangle, LineCapStyle.Triangle));
            }

            return target;
        }

        public static Record2D CreateDefaultScene2D()
        {
            var scene = new Record2D();

            scene.DrawLine((2, 2), (25, 25), 1, COLOR.Green);
            scene.DrawCircle((10, 10), 5, COLOR.Blue);

            var charPath = System.IO.Path.Combine(AssetsDir, "Tiles.jpg");
            var cell = new ImageAsset(charPath, (0, 0), (64, 64), (32, 32));

            scene.DrawImage(Matrix3x2.CreateTranslation(2, 2), cell);            

            return scene;
        }
    }
}
