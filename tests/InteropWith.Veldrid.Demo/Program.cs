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
    class Program
    {
        private static SpriteAsset _Sprite1 = new SpriteAsset("assets\\hieroglyph_sprites_by_asalga.png", (0, 0), (192, 192), (8, 8));        

        static void Main(string[] args)
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
            graphicsDevice = VeldridStartup.CreateGraphicsDevice(window);
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

        private static void Draw(VeldridDrawingFactory factory)
        {
            using var dc = factory.CreateDrawing2DContext(factory.GraphicsDevice.SwapchainFramebuffer, System.Drawing.Color.CornflowerBlue);

            dc.DrawEllipse((40, 40), 50, 50, System.Drawing.Color.Blue);
            dc.DrawFont((150, 30), 2, "Hello World", (System.Drawing.Color.White, 2));
            dc.DrawSprite(Matrix3x2.CreateTranslation(100, 20), _Sprite1);
            dc.DrawRectangle((150, 350), (200, 100), (System.Drawing.Color.Yellow, 3), 20, 5);
        }        
    }
}
