using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace InteropTypes.Graphics.SilkOpenGL.Demo.App
{
    internal class BridgeMesh
    {

        private readonly List<System.Numerics.Vector3> _Vertices;

        private readonly Dictionary<Object, List<int>> _PrimitiveIndices;

        struct Corner
        {
            public int VertexIndex;
            public System.Drawing.Color Color;
            public System.Numerics.Vector2 UV;
        }
    }
}
