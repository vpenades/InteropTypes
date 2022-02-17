using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

using SharpGLTF.Geometry.VertexTypes;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using COLOR = System.Drawing.Color;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;

namespace InteropTypes.Graphics.Backends
{
    using MESHBUILDER = SharpGLTF.Geometry.MeshBuilder<VertexPosition, VertexEmpty, VertexEmpty>;
    using VERTEXBUILDER = SharpGLTF.Geometry.VertexBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    /// <summary>
    /// Wraps a <see cref="MESHBUILDER"/> with <see cref="IScene3D"/>.
    /// </summary>
    public class GltfMeshScene3D : Decompose3D.PassToSelf, IMeshScene3D
    {
        #region factory        

        public static SharpGLTF.Scenes.SceneBuilder Convert(Record3D srcModel, GLTFWriteSettings? settings = null)
        {
            var dst = new GltfMeshScene3D();

            srcModel.DrawTo(dst);

            return dst.ToSceneBuilder(settings);
        }

        #endregion 

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

        #endregion

        #region API

        public void Clear() { _Mesh = null; }

        /// <inheritdoc />
        public override void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
        {
            if (asset is GltfMeshScene3D other)
            {
                if (other._Mesh != null) _GetMesh().AddMesh(other._Mesh.Clone(), transform);
                return;
            }

            if (asset is MESHBUILDER otherMesh)
            {
                _GetMesh().AddMesh(otherMesh.Clone(), transform);
                return;
            }            

            base.DrawAsset(transform, asset, style);
        }

        /// <inheritdoc />
        public override void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle fillColor)
        {
            POINT3.DebugGuardIsFinite(vertices);

            switch (vertices.Length)
            {
                case 0: return;
                case 1: return; // _DrawPoint ??
                case 2: _DrawLine(vertices[0], vertices[1], fillColor.ToGDI()); return;
                default: _DrawSurface(vertices, fillColor.ToGDI(), false); return;
            }
        }        

        public SharpGLTF.Schema2.ModelRoot ToModel()
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();
            if (_Mesh != null) scene.AddRigidMesh(_Mesh, Matrix4x4.Identity);
            return scene.ToGltf2();
        }

        public SharpGLTF.Scenes.SceneBuilder ToSceneBuilder(GLTFWriteSettings? settings = null)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();
            if (_Mesh != null) scene.AddRigidMesh(_Mesh.Clone(), Matrix4x4.Identity);
            return scene;
        }

        /// <inheritdoc />
        public void DrawMeshPrimitive(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> triangleIndices, object texture)
        {
            for(int i=0; i < triangleIndices.Length; i+=3)
            {
                var a = vertices[triangleIndices[i + 0]];
                var b = vertices[triangleIndices[i + 1]];
                var c = vertices[triangleIndices[i + 2]];

                var style = new ColorStyle(a.Color); // should average colors from a, b, c

                _DrawSurface(POINT3.Array(a.Position, b.Position, c.Position), style.ToGDI(), false);
            }
        }

        /// <inheritdoc />
        public void DrawWireframePrimitive(ReadOnlySpan<Vertex3> vertices, ReadOnlySpan<int> lineIndices)
        {
            for (int i = 0; i < lineIndices.Length; i += 2)
            {
                var a = vertices[lineIndices[i + 0]];
                var b = vertices[lineIndices[i + 1]];

                var style = new ColorStyle(a.Color); // should average colors from a, b, c

                _DrawLine(a.Position, b.Position, style.ToGDI());
            }
        }

        #endregion

        #region core

        private MESHBUILDER _GetMesh()
        {
            if (_Mesh == null) _Mesh = new MESHBUILDER();
            return _Mesh;
        }

        private void _DrawSurface(ReadOnlySpan<POINT3> vertices, COLOR color, bool doubleSided)
        {
            _GetMesh();

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
            _GetMesh();

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
