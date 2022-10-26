using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
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
                case "SideBySideSpheres": return CreateSideBySideSpheres();
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
            ThunderbirdRocket.DrawTo(Matrix4x4.Identity, context);
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
            var cell = new ImageSource(charPath, (0, 0), (64, 64), (32, 32));

            scene.DrawImage(Matrix3x2.CreateTranslation(2, 2), cell);            

            return scene;
        }


        public static Record3D CreateSideBySideSpheres()
        {
            var record = new Record3D();
            IScene3D dc = record;

            for(int i=0; i < 2; ++i)
            {
                int x = -5 + i * 10;

                dc.DrawSphere((x, 0, -5), 8, (COLOR.Red, COLOR.Blue, 1));
                dc.DrawSphere((x, 0, 5), 8, COLOR.Red);

                dc = new Transforms.Decompose3D(record, 5, 3);
            }

            return record;
        }
    }
}
