using System;
using System.Collections.Generic;

using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Drawing;
using System.Numerics;
using InteropTypes.Graphics.Bitmaps;

namespace Tutorial
{
    class Program1
    {
        #region data

        private static IWindow window;
        private static GL Gl;

        private static BasicDynamicMesh<Vertex> Mesh;
        private static InteropTypes.Graphics.Backends.SilkGL.Texture Tex;
        private static Effect1 Shader;        

        private static readonly Random Rnd = new Random();

        #endregion


        public static void Run(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Render += OnRender;
            window.Update += OnUpdate;
            window.Closing += OnClose;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            using IInputContext input = window.CreateInput();

            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }            

            //Getting the opengl api for drawing to the screen.
            Gl = GL.GetApi(window);

            Shader = new Effect1(Gl);

            Mesh = new BasicDynamicMesh<Vertex>(Gl);


            // demo load texture

            Tex = new InteropTypes.Graphics.Backends.SilkGL.Texture(Gl);

            var data = InteropTypes.Graphics.Bitmaps.MemoryBitmap<Pixel.RGBA32>.Load("Assets\\qrhead.jpg");

            using var writer = Tex.Using();

            writer.SetPixels(data);
        }

        private static void OnUpdate(double obj)
        {
            Mesh.Clear();
            var a = new Vertex(0.5f + Rnd.NextSingle() * 0.1f, 0.5f + Rnd.NextSingle() * 0.1f, 0.0f).WithUV(0,0);
            var b = new Vertex(0.5f + Rnd.NextSingle() * 0.1f, -0.5f + Rnd.NextSingle() * 0.1f, 0.0f).WithUV(1, 0);
            var c = new Vertex(-0.5f + Rnd.NextSingle() * 0.1f, -0.5f + Rnd.NextSingle() * 0.1f, 0.0f).WithUV(1, 1);
            var d = new Vertex(-0.5f + Rnd.NextSingle() * 0.1f, 0.5f + Rnd.NextSingle() * 0.1f, 0.5f).WithUV(0, 1);

            Mesh.AddPolygon(a, b, c, d);
        }

        private static unsafe void OnRender(double obj) //Method needs to be unsafe due to draw elements.
        {
            //Clear the color channel.
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Shader.SolidTexture = Tex;

            using (var dcx = Shader.Using())
            {
                

                if (dcx.VertexUniforms is IUniformTransforms3D fxXforms)
                {
                    fxXforms.SetModelMatrix(Matrix4x4.CreateRotationZ(0.1f));
                    fxXforms.SetCameraMatrix(Matrix4x4.Identity);
                    fxXforms.SetProjMatrix(Matrix4x4.Identity);
                }

                Mesh.Draw(dcx);
            }
        }

        

        private static void OnClose()
        {
            //Remember to delete the buffers.

            Mesh.Dispose();
            Shader.Dispose();
            Tex.Dispose();
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                window.Close();
            }
        }
    }
}
