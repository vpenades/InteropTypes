using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point3
    {
        public struct Vertex
        {
            public Vertex(Point3 p, ColorStyle c, Point3 t)
            {
                Position = p.XYZ;
                Color = c.Packed;
                TextureCoord = t.XYZ;
            }

            public Vertex(Point3 p, Point3 t)
            {
                Position = p.XYZ;
                Color = uint.MaxValue;
                TextureCoord = t.XYZ;
            }

            public System.Numerics.Vector3 Position;
            public UInt32 Color;
            public System.Numerics.Vector3 TextureCoord;

            public static void Transform(Span<Vertex> vertices, in System.Numerics.Matrix4x4 transform)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Position = System.Numerics.Vector3.Transform(vertices[i].Position, transform);
                }
            }
        }
    }
}
