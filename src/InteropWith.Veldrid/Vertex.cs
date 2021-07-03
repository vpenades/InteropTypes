using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

namespace InteropWith
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public static implicit operator Vertex(Vector2 p)
        {
            return new Vertex(p, UInt32.MaxValue);
        }
            

        public Vertex(InteropDrawing.Point2 pos, UInt32 color)
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
}
