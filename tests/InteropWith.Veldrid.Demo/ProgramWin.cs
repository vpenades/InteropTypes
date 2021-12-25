using System;
using System.Collections.Generic;
using System.Numerics;

using InteropBitmaps;

using InteropDrawing;

using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace InteropWith
{
    class ProgramWin
    {
        private static SpriteAsset _Sprite1 = new SpriteAsset("assets\\hieroglyph_sprites_by_asalga.png", (0, 0), (192, 192), (8, 8));

        public static void Run(string[] args)
        {
            // Create the window and the graphics device
            VeldridInit(out var window, out var graphicsDevice);

            var factory = new VeldridDrawingFactory(graphicsDevice);

            // We run the game loop here and do our drawing inside of it.
            VeldridRunLoop(window, graphicsDevice, () => Draw(factory));

            factory.Dispose();

            graphicsDevice.Dispose();
        }

        private static void VeldridInit(out Sdl2Window window, out GraphicsDevice graphicsDevice)
        {
            WindowCreateInfo windowCI = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "InteropDrawing Demo"
            };

            window = VeldridStartup.CreateWindow(ref windowCI);

            var options = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: Veldrid.PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved,
                preferDepthRangeZeroToOne: true,
                preferStandardClipSpaceYDirection: true);

#if DEBUG
            options.Debug = true;
#endif

            graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, options);
        }

        private static void VeldridRunLoop(Sdl2Window window, GraphicsDevice graphicsDevice, Action action)
        {
            while (window.Exists)
            {
                window.PumpEvents();

                if (window.Exists)
                {
                    action();

                    graphicsDevice.SwapBuffers();
                    graphicsDevice.WaitForIdle();
                }
            }
        }

        private static Vector3 _Camera = Vector3.Zero;

        private static _Sprites2D _AnimatedScene = new _Sprites2D();

        private static void Draw(VeldridDrawingFactory factory)
        {
            var buffer = factory.GraphicsDevice.SwapchainFramebuffer;

            var view = Matrix4x4.CreateTranslation(-_Camera);

            _Camera -= Vector3.UnitX * 0.1f;

            using (var dc3 = factory.CreateDrawing3DContext(buffer, view, 1.2f, (1, 1000)))
            {
                dc3.FillFrame(System.Drawing.Color.CornflowerBlue);

                dc3.DrawSphere((0, 0, -300), 100, System.Drawing.Color.Yellow);

                dc3.DrawSphere((100, 0, -300), 100, System.Drawing.Color.Yellow);
                dc3.DrawSphere((-100, 0, -300), 100, System.Drawing.Color.Yellow);

                dc3.DrawSphere((0, 100, -300), 100, System.Drawing.Color.Yellow);
                dc3.DrawSphere((0, -100, -300), 100, System.Drawing.Color.Yellow);

                dc3.DrawSegment((150, 150, -300), (100, 20, -100), 10, System.Drawing.Color.Red);
            }

            using (var dc2 = factory.CreateDrawing2DContext(buffer))
            {
                dc2.DrawEllipse((40, 40), 50, 50, System.Drawing.Color.Blue);
                dc2.DrawFont((150, 30), 2, "Hello World", (System.Drawing.Color.White, 2));
                dc2.DrawSprite(Matrix3x2.CreateTranslation(100, 20), _Sprite1);
                dc2.DrawRectangle((150, 350), (200, 100), (System.Drawing.Color.Yellow, 3), 20, 5);

                _AnimatedScene.DrawTo(dc2);
            }
        }
    }
}

