using System;
using System.Numerics;

using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace InteropWith
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the window and the graphics device
            VeldridInit(out var window, out var graphicsDevice);

            // Create a texture storage that manages textures.
            // Textures in OpenWheels are represented with integer values.
            // A platform-specific ITextureStorageImplementation handles texture creation,
            // destruction and modification.
            // var texStorage = new VeldridTextureStorage(graphicsDevice);

            // Create a renderer that implements the OpenWheels.Rendering.IRenderer interface
            // this guy actually draws everything to the backbuffer
            var renderer = new _VeldridGraphicsContext(graphicsDevice);
            var textures = new _VeldridTextureFactory(graphicsDevice);

            var texId = textures.CreateTexture(16, 16, PixelFormat.R8_G8_B8_A8_UNorm);
            var texData = new UInt32[16 * 16];
            texData.AsSpan().Fill(uint.MaxValue);
            textures.SetData<UInt32>(texId, new System.Drawing.Rectangle(0, 0, 16, 16), texData.AsSpan());

            var first = true;

            // We run the game loop here and do our drawing inside of it.
            VeldridRunLoop(window, graphicsDevice, () => Draw(renderer, textures));

            renderer.Dispose();
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
                WindowTitle = "OpenWheels Batcher Primitives"
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

        private static void Draw(_VeldridGraphicsContext dc, _VeldridTextureFactory tex)
        {
            dc.SetTarget();

            dc.UpdateWvp();

            dc.Clear(System.Drawing.Color.CornflowerBlue);
            dc.CurrentEffect.SetTexture(tex.GetTextureView(tex.GetTexture(0)));

            var tri = new Vertex[3];
            tri[0] = new Vertex
            {
                Position = new Vector3(0, 0, 0),
                Color = Vector4.One
            };

            tri[1] = new Vertex
            {
                Position = new Vector3(10, 10, 0),
                Color = Vector4.One
            };

            tri[2] = new Vertex
            {
                Position = new Vector3(0, 10, 0),
                Color = Vector4.One
            };

            dc.BeginRender(tri, new int[] { 0, 1, 2 }, 3, 3);

            dc.DrawBatch(0, 3);

            dc.EndRender();
        }
    }
}
