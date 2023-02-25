﻿using System;
using System.Collections.Generic;

using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using InteropTypes.Graphics.Backends.SilkGL;
using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Drawing;

namespace Tutorial
{
    class Program1
    {
        #region data

        private static IWindow window;
        private static GL Gl;

        private static BasicDynamicMesh Mesh;
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

            Mesh = new BasicDynamicMesh(Gl);

            var a = new Point3(0.5f, 0.5f, 0.0f);
            var b = new Point3(0.5f, -0.5f, 0.0f);
            var c = new Point3(-0.5f, -0.5f, 0.0f);
            var d = new Point3(-0.5f, 0.5f, 0.5f);

            Mesh.AddPolygon(System.Drawing.Color.Red, a, b, c, d);

            Shader = new Effect1(Gl);
        }

        private static unsafe void OnRender(double obj) //Method needs to be unsafe due to draw elements.
        {
            //Clear the color channel.
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            /*
            Shader.Bind();

            Mesh.Draw();

            Shader.Unbind();
            */
        }

        private static void OnUpdate(double obj)
        {

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