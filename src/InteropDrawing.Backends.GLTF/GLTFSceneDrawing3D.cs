using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

using SharpGLTF.Schema2;

namespace InteropDrawing.Backends
{
    public class GLTFSceneDrawing3D : IDrawing3D
    {
        #region factory
        public static GLTFSceneDrawing3D Create() { return new GLTFSceneDrawing3D(); }        

        public static SharpGLTF.Scenes.SceneBuilder Convert(Model3D srcModel, GLTFWriteSettings? settings = null)
        {
            var dst = Create();
            srcModel.DrawTo(dst);
            return dst.ToSceneBuilder(settings);
        }

        #endregion

        #region lifecycle
        private GLTFSceneDrawing3D() { }

        #endregion

        #region data

        private readonly List<GLTFMeshDrawing3D> _Meshes = new List<GLTFMeshDrawing3D>();

        private GLTFMeshDrawing3D _CurrentMesh;        

        private readonly Model3D _Bounds = new Model3D();

        private CameraView3D? _Camera;

        #endregion

        #region properties        

        /// <summary>
        /// Gets or sets the quality of Cylinders
        /// </summary>
        public int CylinderLOD { get; set; } = 6;

        /// <summary>
        /// Gets or sets the quality of Spheres.
        /// </summary>
        public int SphereLOD { get; set; } = 2;

        #endregion

        #region Drawing API        

        private GLTFMeshDrawing3D _GetCurrent()
        {
            if (_CurrentMesh == null) _CurrentMesh = _CreateMesh();
            return _CurrentMesh;
        }

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            _GetCurrent().DrawAsset(transform, asset, brush);
            _Bounds.DrawAsset(transform, asset, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, Single diameter, LineStyle brush)
        {
            _GetCurrent().DrawSegment(a, b, diameter, brush);
            _Bounds.DrawSegment(a, b, diameter, brush);
        }        

        public void DrawSphere(Point3 center, Single diameter, ColorStyle brush)
        {
            _GetCurrent().DrawSphere(center, diameter, brush);
            _Bounds.DrawSphere(center, diameter, brush);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            _GetCurrent().DrawSurface(vertices, brush);
            _Bounds.DrawSurface(vertices, brush);
        }

        #endregion

        #region API

        private GLTFMeshDrawing3D _CreateMesh()
        {
            var mesh = new GLTFMeshDrawing3D();
            mesh.CylinderLOD = this.CylinderLOD;
            mesh.SphereLOD = this.SphereLOD;

            return mesh;
        }

        public void Clear()
        {
            _Meshes.Clear();
            _CurrentMesh = null;
        }

        public void Flush()
        {
            if (_CurrentMesh == null) return;

            if (!_CurrentMesh.IsEmpty) _Meshes.Add(_CurrentMesh);

            _CurrentMesh = null;
        }

        public void AddMesh(Matrix4x4 xform, Model3D model)
        {
            CreateMesh().DrawAsset(xform, model);
        }

        public GLTFMeshDrawing3D CreateMesh()
        {
            var mesh = _CreateMesh();
            _Meshes.Add(mesh);
            return mesh;
        }

        public void SetCamera(CameraView3D camera) { _Camera = camera; }

        #endregion

        #region serialization API

        public SharpGLTF.Scenes.SceneBuilder ToSceneBuilder(GLTFWriteSettings? settings = null)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            // add meshes

            foreach (var m in _Meshes.Where(item => !item.IsEmpty)) scene.AddRigidMesh(m.Mesh, Matrix4x4.Identity);

            if (!_CurrentMesh?.IsEmpty ?? false) scene.AddRigidMesh(_CurrentMesh.Mesh, Matrix4x4.Identity);

            // add camera

            if (_Camera.HasValue)
            {
                var vcam = _Camera.Value;

                var camNode = new SharpGLTF.Scenes.NodeBuilder("CameraNode");
                camNode.WorldMatrix = vcam.WorldMatrix;

                vcam.WorldMatrix = Matrix4x4.Identity;

                if (vcam.VerticalFieldOfView.HasValue)
                {
                    var yfov = vcam.VerticalFieldOfView.Value;

                    var persp = new SharpGLTF.Scenes.CameraBuilder.Perspective(null, yfov, 0.1f);

                    scene.AddCamera(persp, camNode);
                }
                else if (vcam.OrthographicScale.HasValue)
                {
                    var s = vcam.OrthographicScale.Value;

                    var ortho = new SharpGLTF.Scenes.CameraBuilder.Orthographic(s, s, 0.1f, 1000);

                    scene.AddCamera(ortho, camNode);
                }

                if ((settings?.CameraSize ?? 0) > 0)
                {
                    var camMesh = new GLTFMeshDrawing3D();
                    vcam.DrawTo(camMesh, settings.Value.CameraSize.Value);

                    scene.AddRigidMesh(camMesh.Mesh, camNode);
                }
            }

            return scene;
        }

        public ModelRoot ToModel() { return ToModel(default); }

        public ModelRoot ToModel(GLTFWriteSettings? settings = null)
        {
            var scene = ToSceneBuilder(settings);
            return scene.ToGltf2();
        }        

        public void Save(string filePath)
        {
            Save(filePath, default(GLTFWriteSettings));
        }

        public void Save(string filePath, GLTFWriteSettings? settings = null)
        {
            var model = ToModel(settings);

            if (filePath.ToLower().EndsWith(".obj"))
            {
                model.SaveAsWavefront(filePath);
                return;
            }

            model.Save(filePath);
        }

        #endregion
    }

    public struct GLTFWriteSettings
    {
        public float? CameraSize;
    }
}
