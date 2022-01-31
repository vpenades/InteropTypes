using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

using SharpGLTF.Geometry.VertexTypes;

using COLOR = System.Drawing.Color;
using POINT3 = InteropDrawing.Point3;

namespace InteropDrawing.Backends
{
    using MESHBUILDER = SharpGLTF.Geometry.MeshBuilder<VertexPosition, VertexEmpty, VertexEmpty>;
    using VERTEXBUILDER = SharpGLTF.Geometry.VertexBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    /// <summary>
    /// Wraps a <see cref="MESHBUILDER"/> with <see cref="IScene3D"/>.
    /// </summary>
    public class GltfMeshDrawing3D : Transforms.Decompose3D.PassToSelf, IScene3D
    {
        #region data

        private MESHBUILDER _Mesh;        

        private readonly Dictionary<GltfSolidMaterial, SharpGLTF.Materials.MaterialBuilder> _Materials = new Dictionary<GltfSolidMaterial, SharpGLTF.Materials.MaterialBuilder>();

        #endregion

        #region properties

        /// <summary>
        /// Gets the underlaying <see cref="MESHBUILDER"/>.
        /// </summary>
        public MESHBUILDER Mesh => _Mesh;

        /// <summary>
        /// true if the mesh is empty.
        /// </summary>
        public bool IsEmpty => _Mesh == null || !_Mesh.Primitives.Any(item => item.Vertices.Count > 0);

        /// <summary>
        /// Gets or sets the quality of Cylinders
        /// </summary>
        public int CylinderLOD { get; set; } = 6;

        /// <summary>
        /// Gets or sets the quality of Spheres.
        /// </summary>
        public int SphereLOD { get; set; } = 2;

        #endregion

        #region API

        public void Clear() { _Mesh = null; }
        
        /// <inheritdoc />
        public override void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle fillColor)
        {
            switch(vertices.Length)
            {
                case 0: return;
                case 1: return;
                case 2: _DrawLine(vertices[0], vertices[1], fillColor.Color); return;
                default: _DrawSurface(vertices, fillColor.Color, false); return;
            }            
        }

        #endregion

        #region core

        private void _DrawSurface(ReadOnlySpan<POINT3> vertices, COLOR color, Boolean doubleSided)
        {
            if (_Mesh == null) _Mesh = new MESHBUILDER();

            var material = _UseMaterial(color, doubleSided);
            var prim = _Mesh.UsePrimitive(material);

            var a = new VERTEXBUILDER(vertices[0].ToNumerics());
            var b = new VERTEXBUILDER(vertices[1].ToNumerics());

            if (vertices.Length == 4)
            {
                var c = new VERTEXBUILDER(vertices[2].ToNumerics());
                var d = new VERTEXBUILDER(vertices[3].ToNumerics());
                prim.AddQuadrangle(a, b, c, d);
            }
            else
            {
                for (int i = 2; i < vertices.Length; ++i)
                {
                    var c = new VERTEXBUILDER(vertices[i].ToNumerics());
                    prim.AddTriangle(a, b, c);
                    b = c;
                }
            }
        }

        private void _DrawLine(POINT3 a, POINT3 b, COLOR color)
        {
            if (_Mesh == null) _Mesh = new MESHBUILDER();

            var material = _UseMaterial(color, false);

            var prim = _Mesh.UsePrimitive(material, 2);

            var aa = new VERTEXBUILDER(a.ToNumerics());
            var bb = new VERTEXBUILDER(b.ToNumerics());

            prim.AddLine(aa, bb);
        }

        private SharpGLTF.Materials.MaterialBuilder _UseMaterial(COLOR color, bool doubleSided)
        {
            var mkey = new GltfSolidMaterial(color, doubleSided);

            if (!_Materials.TryGetValue(mkey, out SharpGLTF.Materials.MaterialBuilder material))
            {
                _Materials[mkey] = material = mkey.CreateMaterial();
            }

            return material;
        }

        #endregion
    }
}
