using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using NUnit.Framework;

using POINT2 = InteropDrawing.Point2;
using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    [TestFixture]
    [Category("Schematic3D")]
    public class MixedTests
    {
        [SetUp]
        public void Setup()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test]
        public void TestModularScale()
        {
            Assert.AreEqual(1, Matrix4x4.Identity.DecomposeScale());
            Assert.AreEqual(0.5f, Matrix4x4.CreateScale(0.5f).DecomposeScale(), 0.0001f);
            Assert.AreEqual(3, Matrix4x4.CreateScale(3).DecomposeScale(), 0.0001f);
        }

        [Test]
        public void TestPolygonClipping()
        {
            var saw = new[] { Vector3.Zero, new Vector3(1, 5, 0), new Vector3(2, 1, 0), new Vector3(3, 5, 0), new Vector3(4, 0, 0) };

            Span<Vector3> sawOut1 = stackalloc Vector3[saw.Length * 2];

            var l = new Plane(Vector3.UnitY, 1).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(5, l);

            l = new Plane(Vector3.UnitY, -2).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(6, l);

            l = new Plane(-Vector3.UnitY, 2).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(7, l);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestClipScene3D(string sceneName)
        {
            TestContext.CurrentContext.AttachShowDirLink();

            var srcScene = SceneFactory.CreateScene3D(sceneName);

            var bounds = srcScene.BoundingMatrix;

            for (int i = -1; i <= 1; ++i)
            {
                var dstScene = new Model3D();

                srcScene.DrawTo(new Transforms.PlaneClip3D(dstScene, new Plane(Vector3.UnitX, i)));

                dstScene.AttachToCurrentTest($"{sceneName}_{i}.glb");
            }
        }


        [Test]
        public void FrustumProjectionTest()
        {
            var sceneSize = 100;
            var viewport = new Vector2(100, 100);

            var camera = Matrix4x4.CreateWorld(Vector3.UnitZ * sceneSize, -Vector3.UnitZ, Vector3.UnitY);

            // create world-to-view transform
            Matrix4x4.Invert(camera, out Matrix4x4 view);

            viewport *= 0.5f;
            var projMatrix = Matrix4x4.CreateOrthographicOffCenter(-viewport.X, +viewport.X, -viewport.Y, viewport.Y, -0.5f, -1000);

            var portMatrix = new Matrix4x4
                (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                viewport.X, viewport.Y, 0, 1
                );

            Vector3 projFunc(Vector3 p)
            {
                // classical transform pipeline
                p = Vector3.Transform(p, view);
                p = Vector3.Transform(p, projMatrix);
                p.X /= p.Z;
                p.Y /= p.Z;
                p = Vector3.Transform(p, portMatrix);
                return p;
            }

            var p1 = projFunc(Vector3.Zero);
            var p2 = projFunc(new Vector3(10, 0, 0));
            var p3 = projFunc(new Vector3(-10, 10, 0));

        }

        [Test]
        public void TestAsset2D()
        {
            var model = SceneFactory.CreateDefaultScene2D();

            var r = model.BoundingRect;
            // var c = model.CircleBounds;

            var scene = new Model2D();

            scene.DrawAsset(Matrix3x2.CreateRotation(1) * Matrix3x2.CreateTranslation(50, 50), model, COLOR.White);

            // scene.RectBounds.DrawTo(scene, (COLOR.Red, 0.1f));
            // scene.CircleBounds.DrawTo(scene, (COLOR.Red, 0.1f));

            scene.AttachToCurrentTest("document.svg");
            scene.AttachToCurrentTest("document.png");
            // scene.AttachToCurrentTestAsPlot("plot.pdf");
        }

        [Test]
        public void TestRenderBitmap()
        {
            var renderTarget = new Backends.WPFRenderTarget(256, 256);

            var charPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets\\PunkRun.png");
            var character = SpriteAsset.CreateGrid(charPath, (256, 256), (128, 128), 8, 8).ToArray();

            var tilesPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets\\Tiles.jpg");
            var tiles = SpriteAsset.CreateFromBitmap(tilesPath, (1024, 1024), (512, 512));

            using (var dc = renderTarget.OpenDrawingContext())
            {
                dc.DrawLine(new Vector2(2, 2), new Vector2(100, 100), 2, COLOR.SkyBlue);

                dc.DrawEllipse(new Vector2(30, 50), 20, 20, COLOR.Violet);

                dc.DrawSprite(Matrix3x2.Identity, tiles);
            }

            var path = TestContext.CurrentContext.GetFilePath("testrender1.png");

            renderTarget.SaveToPNG(path);
            TestContext.AddTestAttachment(path);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestRender3DSceneToBitmap(string sceneName)
        {
            var path = TestContext.CurrentContext.GetFilePath($"{sceneName}.png");

            var scene = SceneFactory.CreateScene3D(sceneName);

            Backends.WPFDrawingContext2D.SaveToBitmap(path, 1024, 1024, null, scene);

            TestContext.AddTestAttachment(path);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestPaintersAlgorythmPipeline(string sceneName)
        {
            var renderTarget = new Backends.WPFRenderTarget(1024, 1024);

            var scene = new Model3D();
            scene.DrawFloor(new Vector2(-50, -50), new Vector2(100, 100), 10, COLOR.Green, COLOR.DarkGreen);
            scene.DrawAsset(Matrix4x4.CreateTranslation(0, 5, -10), SceneFactory.CreateScene3D(sceneName));
            scene.DrawAsset(Matrix4x4.CreateTranslation(0, 5, 0), SceneFactory.CreateScene3D(sceneName));
            scene.AttachToCurrentTest("scene.glb");

            using (var dc = renderTarget.OpenDrawingContext())
            {
                var perspective = Transforms.PerspectiveTransform.CreateLookingAtCenter((dc, 1024, 1024), (10, 5, 30));
                perspective.DrawScene(scene);
            }

            var path = TestContext.CurrentContext.GetFilePath($"{sceneName}.png");

            renderTarget.SaveToPNG(path);
            TestContext.AddTestAttachment(path);
        }

        /*
        [Test]
        public void TestDrawDNA()
        {
            var mesh = new Backends.STLMeshBuilder();
            mesh.DrawAsset(Matrix4x4.Identity, SceneFactory.CreateScene3D("DNA"));

            TestContext.CurrentContext.Attach("DNA.stl", mesh);

            var renderTarget = new Backends.WPFRenderTarget(256, 256);
            using (var dc = renderTarget.OpenDrawingContext())
            {
                // dc.Project(mesh);
            }

            renderTarget.SaveToPNG("DNA.png");
            TestContext.AddTestAttachment("DNA.png");
        }*/


        [Test]
        public void TestSVG()
        {
            using (var svg = Backends.SVGSceneDrawing2D.CreateGraphic())
            {
                svg.DrawLine((0, 0), (100, 100), 2, (COLOR.SkyBlue, LineCapStyle.Round, LineCapStyle.Triangle));

                svg.DrawRectangle((10, 10), (80, 80), (COLOR.Blue, 4));

                svg.DrawEllipse(new Vector2(50, 50), 70, 70, (COLOR.Red, 2));

                var document = svg.ToSVGContent();

                var path = TestContext.CurrentContext.GetFilePath("document.svg");

                System.IO.File.WriteAllText(path, document);
                TestContext.AddTestAttachment(path);
            }
        }

        [Test]
        public void RenderSceneToSVG()
        {
            using (var svg = Backends.SVGSceneDrawing2D.CreateGraphic())
            {
                var scene = SceneFactory.CreateScene3D("Scene1");

                scene.DrawTo(svg, 1024, 1024, new Vector3(7, 5, 20));


                var document = svg.ToSVGContent();

                var path = TestContext.CurrentContext.GetFilePath("document.svg");

                System.IO.File.WriteAllText(path, document);
                TestContext.AddTestAttachment(path);
            }
        }


        [Test]
        public void TestRenderReferenceOutlinesInWPF()
        {
            var renderTarget = new Backends.WPFRenderTarget(512, 512);

            using (var dc = renderTarget.OpenDrawingContext())
            {
                DrawDirectVsPolygon(dc);
            }

            var path = TestContext.CurrentContext.GetFilePath("referenceWPF.png");
            renderTarget.SaveToPNG(path);
            TestContext.AddTestAttachment(path);
        }

        private static void DrawDirectVsPolygon(IDrawing2D dc)
        {
            var l1style = (COLOR.White, LineCapStyle.Flat, LineCapStyle.Round);
            var l2style = ((COLOR.White, COLOR.Red, 5), LineCapStyle.Flat, LineCapStyle.Round);

            var texts = 0.25f;

            var x = 50; dc.DrawFont((x, 30), texts, "Native", FontStyle.VFlip_Gray.With(COLOR.White));

            dc.DrawCircle((x, 50), 10, COLOR.White);
            dc.DrawCircle((x, 100), 10, (COLOR.White, COLOR.Red, 5));
            dc.DrawLine((x, 150), (50, 200), 10, l1style);
            dc.DrawLine((x, 250), (50, 300), 10, l2style);

            x = 100; dc.DrawFont((x, 30), texts, "Polygonized", FontStyle.VFlip_Gray.With(COLOR.White));

            dc.DrawEllipseAsPolygon((x, 50), 10, 10, COLOR.Yellow);
            dc.DrawEllipseAsPolygon((x, 100), 10, 10, (COLOR.Yellow, COLOR.Red, 5));
            dc.DrawLinesAsPolygons(new[] { new POINT2(x, 150), new POINT2(x, 200) }, 10, l1style);
            dc.DrawLinesAsPolygons(new[] { new POINT2(x, 250), new POINT2(x, 300) }, 10, l2style);

            var dc3d = Transforms.Drawing2DTransform.Create(dc, Matrix3x2.Identity);

            x = 150; dc.DrawFont((x, 30), texts, "3D", FontStyle.VFlip_Gray.With(COLOR.White));

            dc3d.DrawSphere((x, 50, 0), 10, COLOR.White);
            dc3d.DrawSphere((x, 100, 0), 10, (COLOR.White, COLOR.Red, 5));
            dc3d.DrawSegment((x, 150, 0), (x, 200, 0), 10, l1style);
            dc3d.DrawSegment((x, 250, 0), (x, 300, 0), 10, l2style);

            x = 200; dc.DrawFont((x, 30), texts, "3D Polygonized", FontStyle.VFlip_Gray.With(COLOR.White));

            dc3d.DrawSphereAsSurfaces(new Vector3(x, 50, 0), 10, 3, COLOR.Yellow);
            dc3d.DrawSphereAsSurfaces(new Vector3(x, 100, 0), 10, 3, (COLOR.Yellow, COLOR.Red, 5));
            dc3d.DrawCylinderAsSurfaces(new Vector3(x, 150, 0), 10, new Vector3(x, 200, 0), 10, 5, l1style);
            dc3d.DrawCylinderAsSurfaces(new Vector3(x, 250, 0), 10, new Vector3(x, 300, 0), 10, 5, l2style);
        }
    }
}
