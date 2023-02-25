using System;
using System.Collections.Generic;

using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using InteropTypes.Graphics.Backends.SilkGL;
using System.Numerics;

namespace Tutorial
{
    class Program0
    {
        #region data

        private static IWindow window;
        private static GL Gl;

        private static VertexBuffer<Vertex> Vbo;
        private static VertexBufferArray Vao;
        private static IndexBuffer Ebo;
        private static Effect0 Shader;

        [System.Diagnostics.DebuggerDisplay("{Position}")]
        struct Vertex : VertexElement.ISource
        {
            public Vertex(float x, float y, float z)
            {
                Position = new System.Numerics.Vector3(x, y, z);
            }

            public System.Numerics.Vector3 Position;

            public IEnumerable<VertexElement> GetElements()
            {
                yield return new VertexElement(3, false);
            }
        }

        //Vertex data, uploaded to the VBO.
        private static readonly Vertex[] Vertices =
        {            
            new Vertex(0.5f,  0.5f, 0.0f),
            new Vertex(0.5f, -0.5f, 0.0f),
            new Vertex(-0.5f, -0.5f, 0.0f),
            new Vertex(-0.5f,  0.5f, 0.5f)
        };

        //Index data, uploaded to the EBO.
        private static readonly uint[] Indices = { 0, 1, 3,   1, 2, 3 };

        #endregion

        #region API

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
            if (Gl.GetError() != GLEnum.NoError) throw new Exception(Gl.GetError().ToString());            

            //Creating a vertex array.
            Vbo = new VertexBuffer<Vertex>(Gl, BufferUsageARB.StaticDraw);

            Vao = new VertexBufferArray(Gl);
            Vao.SetLayoutFrom<Vertex>(Vbo);

            //Initializing a element buffer that holds the index data.
            Ebo = new IndexBuffer(Gl, BufferUsageARB.StaticDraw);            

            Vbo.SetData(Vertices);
            Ebo.SetData(Indices.AsSpan(), PrimitiveType.Triangles);            

            Shader = new Effect0(Gl);
        }

        private static unsafe void OnRender(double obj) //Method needs to be unsafe due to draw elements.
        {
            //Clear the color channel.
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            using (var effect = Shader.Using())
            {
                if (effect.Uniforms is IEffectTransforms3D fxXforms)
                {
                    fxXforms.SetModelMatrix(Matrix4x4.CreateRotationZ(0.1f));
                }                

                using (var dc = effect.UseDC(Vao, Ebo))
                {
                    dc.DrawTriangles();
                }
            }
        }

        private static void OnUpdate(double obj)
        {

        }

        private static void OnClose()
        {
            //Remember to delete the buffers.

            Vbo.Dispose();
            Ebo.Dispose();
            Shader.Dispose();
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                window.Close();
            }
        }

        #endregion
    }
}
