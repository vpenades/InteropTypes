using System;
using System.Collections.Generic;
using System.Numerics;

using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Bitmaps;

namespace Tutorial
{
    class Program2
    {
        #region data

        private static IWindow window;
        private static GL Gl;

        private static BasicDynamicMesh<Vertex> Mesh;
        private static InteropTypes.Graphics.Backends.SilkGL.Texture Tex;
        private static Effect1 Shader;        

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

            Mesh = new BasicDynamicMesh<Vertex>(Gl);

            var a = new Vertex(0.5f, 0.5f, 0.0f).WithUV(0,0);
            var b = new Vertex(0.5f, -0.5f, 0.0f).WithUV(0, 1);
            var c = new Vertex(-0.5f, -0.5f, 0.0f).WithUV(1, 1);
            var d = new Vertex(-0.5f, 0.5f, 0.5f).WithUV(1, 0);

            Mesh.AddPolygon(a, b, c, d);

            Shader = new Effect1(Gl);

            // demo load texture

            Tex = new InteropTypes.Graphics.Backends.SilkGL.Texture(Gl);

            var tdata = InteropTypes.Graphics.Bitmaps.MemoryBitmap<Pixel.RGBA32>.Load("Assets\\qrhead.jpg");

            using var writer = Tex.Using();

            writer.SetPixels(tdata);

            Shader.SolidTexture = Tex;
        }


        static float rot = 0;

        private static void OnUpdate(double obj)
        {
            rot += 0.1f;
        }

        private static unsafe void OnRender(double obj) //Method needs to be unsafe due to draw elements.
        {           

            //Clear the color channel.
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            var cameraPosition = new Vector3(0.0f, 0.0f, 30.0f);
            var cameraForward = new Vector3(0.0f, 0.0f, -1.0f);
            var cameraUp = Vector3.UnitY;

            var model = Matrix4x4.CreateRotationY(rot) * Matrix4x4.CreateRotationX(rot);
            var camera = Matrix4x4.CreateWorld(cameraPosition, cameraForward, cameraUp);
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(0.7f, 800f / 600f, 0.1f, 1000.0f);

            using (var edc = Shader.Using())
            {
                if (edc.VertexUniforms is IUniformTransforms3D fxXforms)
                {
                    fxXforms.SetProjMatrix(projection);
                    fxXforms.SetCameraMatrix(camera);
                    fxXforms.SetModelMatrix(model);                    
                }

                Mesh.Draw(edc);
            }
        }        

        private static void OnClose()
        {
            //Remember to delete the buffers.

            Mesh.Dispose();
            Shader.Dispose();
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
