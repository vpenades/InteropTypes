using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropWith
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {        
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TextureCoordinates;

        public static uint SizeInBytes => 4 * 9;
    }
}
