using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex2D
    {
        public static implicit operator Vertex2D(Vector2 p)
        {
            return new Vertex2D(p, UInt32.MaxValue);
        }           

        public Vertex2D(InteropTypes.Graphics.Drawing.Point2 pos, UInt32 color)
        {
            Position = new Vector3(pos.X, pos.Y, -1);
            Color = color;
            TextureCoordinates = Vector2.Zero;
        }

        public Vector3 Position;
        public UInt32 Color;
        public Vector2 TextureCoordinates;

        public static VertexLayoutDescription GetDescription()
        {
            return new VertexLayoutDescription
                (
                new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float3),
                new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Byte4_Norm),
                new VertexElementDescription("TextureCoordinate", VertexElementSemantic.TextureCoordinate,VertexElementFormat.Float2)
                );
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vertex3D
    {
        public Vector3 Position;
        public Vector3 Normal;
        public UInt32 Color;
        public static VertexLayoutDescription GetDescription()
        {
            return new VertexLayoutDescription
                (
                new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float3),
                new VertexElementDescription("Normal", VertexElementSemantic.Normal, VertexElementFormat.Float3),
                new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Byte4_Norm)
                );
        }
    }
}
