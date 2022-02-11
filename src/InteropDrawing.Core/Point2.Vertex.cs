using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point2
    {
        public struct Vertex
        {
            public Vertex(Point2 p, ColorStyle c, Point2 t)
            {
                Position = p.XY;
                Color = c.Packed;
                TextureCoord = t.XY;
            }

            public Vertex(Point2 p, Point2 t)
            {
                Position = p.XY;
                Color = uint.MaxValue;
                TextureCoord = t.XY;
            }

            public System.Numerics.Vector2 Position;
            public UInt32 Color;
            public System.Numerics.Vector2 TextureCoord;

            public static void Transform(Span<Vertex> vertices, in System.Numerics.Matrix3x2 transform)
            {
                for(int i=0; i<vertices.Length; i++)
                {
                    vertices[i].Position = System.Numerics.Vector2.Transform(vertices[i].Position,transform);
                }
            }
        }
    }
}
