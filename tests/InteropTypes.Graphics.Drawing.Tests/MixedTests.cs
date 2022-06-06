using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using NUnit.Framework;

using InteropTypes.Graphics.Drawing.Transforms;
using InteropTypes.Graphics.Backends;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
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

        

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestSaveScene3D(string sceneName)
        {
            TestContext.CurrentContext.AttachShowDirLink();

            var srcScene = SceneFactory.CreateRecord3D(sceneName);

            srcScene.AttachToCurrentTest($"{sceneName}.glb");
            srcScene.AttachToCurrentTest($"{sceneName}.html");            
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

            var scene = new Record2D();

            scene.DrawAsset(Matrix3x2.CreateRotation(1) * Matrix3x2.CreateTranslation(50, 50), model);

            // scene.RectBounds.DrawTo(scene, (COLOR.Red, 0.1f));
            // scene.CircleBounds.DrawTo(scene, (COLOR.Red, 0.1f));

            scene.AttachToCurrentTest("document.svg");
            scene.AttachToCurrentTest("document.png");
            // scene.AttachToCurrentTestAsPlot("plot.pdf");
        }

        [Test]
        public void TestRenderBitmap()
        {
            var renderTarget = new WPFRenderTarget(256, 256);

            var charPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets\\PunkRun.png");
            var character = ImageSource.CreateGrid(charPath, 8, 8, (256, 256), (128, 128)).ToArray();

            var tilesPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets\\Tiles.jpg");
            var tiles = ImageSource.CreateFromBitmap(tilesPath, (1024, 1024), (512, 512));

            renderTarget.Draw(dc =>
            {
                dc.DrawLine(new Vector2(2, 2), new Vector2(100, 100), 2, COLOR.SkyBlue);

                dc.DrawEllipse(new Vector2(30, 50), 20, 20, COLOR.Violet);

                dc.DrawImage(Matrix3x2.Identity, tiles);
            });

            var path = TestContext.CurrentContext.UseFilePath("testrender1.png");

            renderTarget.SaveToPNG(path);
            TestContext.AddTestAttachment(path);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestRender3DSceneToBitmap(string sceneName)
        {
            var scene = SceneFactory.CreateRecord3D(sceneName);

            scene.DrawCube(Matrix4x4.Identity, COLOR.Red, COLOR.Green, COLOR.Blue);

            var path = TestContext.CurrentContext.UseFilePath($"{sceneName}.png");

            Canvas2DFactory.SaveToBitmap(path, 1024, 1024, null, scene);

            TestContext.AddTestAttachment(path);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        [TestCase("SideBySideSpheres")]        
        public void TestPaintersAlgorythmPipeline(string sceneName)
        {
            var scene = new Record3D();
            scene.DrawFloorXZ((-50,0, -50), (100, 100), 10, COLOR.Green, COLOR.DarkGreen);
            scene.DrawAsset(Matrix4x4.CreateTranslation(0, 5, -10), SceneFactory.CreateRecord3D(sceneName));
            // scene.DrawAsset(Matrix4x4.CreateTranslation(0, 5, 0), SceneFactory.CreateRecord3D(sceneName));
            scene.AttachToCurrentTest("scene.glb");
            scene.AttachToCurrentTest("scene.html");

            // render scene with WPF

            var renderTarget = new WPFRenderTarget(1024, 1024);
            renderTarget.Draw(dc =>
            {
                PerspectiveTransform
                    .CreateLookingAtCenter((dc, 1024, 1024), (10, 5, 30))
                    .DrawScene(scene);
            });

            var path = TestContext.CurrentContext.UseFilePath($"WPF_{sceneName}.png");

            renderTarget.SaveToPNG(path);

            // render with MemoryBitmap

            var mem = new Bitmaps.MemoryBitmap(1024, 1024, Bitmaps.Pixel.RGBA32.Format);
            var mdc = Backends.InteropDrawing.CreateDrawingContext(mem);

            PerspectiveTransform
                    .CreateLookingAtCenter((mdc, 1024, 1024), (10, 5, 30))
                    .DrawScene(scene);

            mem.Save(new AttachmentInfo($"Span_{sceneName}.png"));


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
            using (var svg = SVGSceneDrawing2D.CreateGraphic())
            {
                svg.DrawLine((0, 0), (100, 100), 2, (COLOR.SkyBlue, LineCapStyle.Round, LineCapStyle.Triangle));

                svg.DrawRectangle((10, 10), (80, 80), (COLOR.Blue, 4));

                svg.DrawEllipse(new Vector2(50, 50), 70, 70, (COLOR.Red, 2));

                var document = svg.ToSVGContent();

                var path = TestContext.CurrentContext.UseFilePath("document.svg");

                System.IO.File.WriteAllText(path, document);
                TestContext.AddTestAttachment(path);
            }
        }

        [Test]
        public void RenderSceneToSVG()
        {
            using (var svg = SVGSceneDrawing2D.CreateGraphic())
            {
                var scene = SceneFactory.CreateRecord3D("Scene1");

                scene.DrawTo(svg, 1024, 1024, new Vector3(7, 5, 20));


                var document = svg.ToSVGContent();

                var path = TestContext.CurrentContext.UseFilePath("document.svg");

                System.IO.File.WriteAllText(path, document);
                TestContext.AddTestAttachment(path);
            }
        }


        [Test]
        public void TestRenderReferenceOutlinesInWPF()
        {
            var renderTarget = new WPFRenderTarget(512, 512);

            renderTarget.Draw(DrawDirectVsPolygon);

            var path = TestContext.CurrentContext.UseFilePath("referenceWPF.png");
            renderTarget.SaveToPNG(path);
            TestContext.AddTestAttachment(path);
        }

        private static void DrawDirectVsPolygon(ICanvas2D dc)
        {
            var l1style = (COLOR.White, LineCapStyle.Flat, LineCapStyle.Round);
            var l2style = ((COLOR.White, COLOR.Red, 5), LineCapStyle.Flat, LineCapStyle.Round);            

            var x = 50; dc.DrawTextLine((x, 30), "Native", 15, FontStyle.VFlip_Gray.With(COLOR.White));

            dc.DrawCircle((x, 50), 10, COLOR.White);
            dc.DrawCircle((x, 100), 10, (COLOR.White, COLOR.Red, 5));
            dc.DrawLine((x, 150), (50, 200), 10, l1style);
            dc.DrawLine((x, 250), (50, 300), 10, l2style);

            x = 100; dc.DrawTextLine((x, 30), "Polygonized", 15, FontStyle.VFlip_Gray.With(COLOR.White));

            var dc2x = new Decompose2D(dc);

            dc2x.DrawEllipse((x, 50), 10, 10, COLOR.Yellow);
            dc2x.DrawEllipse((x, 100), 10, 10, (COLOR.Yellow, COLOR.Red, 5));
            dc2x.DrawLines(new[] { new POINT2(x, 150), new POINT2(x, 200) }, 10, l1style);
            dc2x.DrawLines(new[] { new POINT2(x, 250), new POINT2(x, 300) }, 10, l2style);

            var dc3d = Canvas2DTransform.Create(dc, Matrix3x2.Identity);

            x = 150; dc.DrawTextLine((x, 30), "3D", 15, FontStyle.VFlip_Gray.With(COLOR.White));

            dc3d.DrawSphere((x, 50, 0), 10, COLOR.White);
            dc3d.DrawSphere((x, 100, 0), 10, (COLOR.White, COLOR.Red, 5));
            dc3d.DrawSegment((x, 150, 0), (x, 200, 0), 10, l1style);
            dc3d.DrawSegment((x, 250, 0), (x, 300, 0), 10, l2style);

            x = 200; dc.DrawTextLine((x, 30), "3D Polygonized", 15, FontStyle.VFlip_Gray.With(COLOR.White));

            var dc3x = new Decompose3D(dc3d, 5, 3);
            dc3x.DrawSphere(new Vector3(x, 50, 0), 10, COLOR.Yellow);
            dc3x.DrawSphere(new Vector3(x, 100, 0), 10, (COLOR.Yellow, COLOR.Red, 5));
            dc3x.DrawSegment(new Vector3(x, 150, 0), new Vector3(x, 200, 0), 10, l1style);
            dc3x.DrawSegment(new Vector3(x, 250, 0), new Vector3(x, 300, 0), 10, l2style);
        }

        [Test]
        public void  TestDrawMultiSegment()
        {
            var scene = new GltfSceneBuilder();

            using(var dc = scene.Create3DContext())
            {
                var style = new LineStyle(COLOR.Red, LineCapStyle.Round, LineCapStyle.Triangle).WithOutline(COLOR.Black,1);                

                Decompose3D.DrawSegment(dc, Point3.Array( (0,0,0), (17,0,0), (20,10,0), (20, 20, 0)), 5, style);

                Decompose3D.DrawSegment(dc, Point3.Array((0, 0, 20), (0, 15, 20), (30, 0, 20), (0, 0, 20)), 3, style);

                Decompose3D.DrawSegment(dc, Point3.Array((0, 0, 40), (0, 15, 40)), 3, style);
            }

            var path = TestContext.CurrentContext.UseFilePath("extrude1.glb");
            scene.Save(path);
            TestContext.AddTestAttachment(path);
        }
    }
}
