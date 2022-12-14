using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using Silk.NET.Maths;
using InteropTypes.Graphics.Backends.SilkGL;
using System.Collections.Generic;

namespace Tutorial
{
    class Program
    {
        #region data

        private static IWindow window;
        private static GL Gl;

        private static VertexBuffer Vao;
        private static IndexBuffer Ebo;        
        private static ShaderProgram Shader;

        //Vertex shaders are run on each vertex.
        private static readonly string VertexShaderSource = @"
        #version 330 core //Using version GLSL version 3.3
        layout (location = 0) in vec4 vPos;
        
        void main()
        {
            gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
        }
        ";

        //Fragment shaders are run on each fragment/pixel of the geometry.
        private static readonly string FragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;

        void main()
        {
            FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }
        ";

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


        private static void Main(string[] args)
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

            //Creating a vertex array.
            Vao = new VertexBuffer(Gl, BufferUsageARB.StaticDraw);
            Vao.SetData(Vertices.AsSpan());

            //Initializing a element buffer that holds the index data.
            Ebo = new IndexBuffer(Gl, BufferUsageARB.StaticDraw);
            Ebo.SetData(Indices.AsSpan(), PrimitiveType.Triangles);

            Shader = ShaderProgram.CreateFromVertexAndFragmentShaders(Gl, VertexShaderSource, FragmentShaderSource);
        }

        private static unsafe void OnRender(double obj) //Method needs to be unsafe due to draw elements.
        {
            //Clear the color channel.
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Shader.DrawTriangles(Vao, Ebo);
        }

        private static void OnUpdate(double obj)
        {

        }

        private static void OnClose()
        {
            //Remember to delete the buffers.

            Vao.Dispose();
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
    }
}
