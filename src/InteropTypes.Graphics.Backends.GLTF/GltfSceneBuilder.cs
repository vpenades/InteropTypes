using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

using SharpGLTF.Schema2;

using InteropTypes.Graphics.Drawing;

using MESHBUILDER = SharpGLTF.Geometry.IMeshBuilder<SharpGLTF.Materials.MaterialBuilder>;


namespace InteropTypes.Graphics.Backends
{
    public class GltfSceneBuilder : GlobalStyle.ISource
    {
        #region factory        

        public static SharpGLTF.Scenes.SceneBuilder Convert(Record3D srcModel, GLTFWriteSettings? settings = null)
        {
            var dst = new GltfSceneBuilder();

            using (var dc = dst.Create3DContext())
            {
                srcModel.DrawTo(dc);
            }

            return dst.ToSceneBuilder(settings);
        }

        #endregion        

        #region data

        private readonly List<(MESHBUILDER Mesh, Matrix4x4 Transform)> _Meshes = new List<(MESHBUILDER, Matrix4x4)>();

        private CameraTransform3D? _Camera;

        private GlobalStyle _GlobalStyle;

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

        #region API

        bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_GlobalStyle, name, out value);
        }

        bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, name, value);
        }

        #endregion

        #region Drawing API

        public GltfSceneBuilder Draw(params IDrawingBrush<IScene3D>[] drawables)
        {
            using (var dc = Create3DContext())
            {
                foreach (var d in drawables) d.DrawTo(dc);
            }

            return this;
        }

        public GltfSceneBuilder Draw(Matrix4x4 xform, params IDrawingBrush<IScene3D>[] drawables)
        {
            using (var dc = Create3DContext(xform))
            {
                foreach (var d in drawables) d.DrawTo(dc);
            }

            return this;
        }

        public IDisposableScene3D Create3DContext()
        {
            return new _GltfDrawing3DContext(this, Matrix4x4.Identity);
        }

        public IDisposableScene3D Create3DContext(Matrix4x4 transform)
        {
            return new _GltfDrawing3DContext(this, transform);
        }

        internal void _AddMesh(MESHBUILDER mesh, Matrix4x4 xform)
        {
            if (mesh == null) return;
            if (mesh.Primitives.Count == 0) return;
            _Meshes.Add((mesh, xform));
        }

        public void SetCamera(CameraTransform3D camera) { _Camera = camera; }

        #endregion

        #region serialization API

        public SharpGLTF.Scenes.SceneBuilder ToSceneBuilder(GLTFWriteSettings? settings = null)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            // add meshes

            foreach (var m in _Meshes) scene.AddRigidMesh(m.Mesh, m.Transform);

            // add camera

            GLTFWriteSettings._AddCameraTo(_Camera, settings, scene);

            return scene;
        }

        public ModelRoot ToModel() { return ToModel(default); }

        public ModelRoot ToModel(GLTFWriteSettings? settings = null)
        {
            return ToSceneBuilder(settings).ToGltf2();
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

    sealed class _GltfDrawing3DContext : GltfMeshScene3D, IDisposableScene3D, GlobalStyle.ISource
    {
        #region lifecycle

        public _GltfDrawing3DContext(GltfSceneBuilder owner, Matrix4x4 xform)
        {
            _Owner = owner;
            _Transform = xform;
        }

        public void Dispose()
        {
            if (_Owner != null)
            {
                if (!IsEmpty) _Owner._AddMesh(Mesh, _Transform);
                Clear();
            }

            _Owner = null;
        }

        #endregion

        #region data

        private GltfSceneBuilder _Owner;
        private Matrix4x4 _Transform;

        #endregion

        #region API

        /// <inheritdoc/>
        bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_Owner, name, out value);
        }

        /// <inheritdoc/>
        bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(_Owner, name, value);
        }

        #endregion
    }

    public struct GLTFWriteSettings
    {
        /// <summary>
        /// If defined it will add the current camera to the scene as a visible mesh.
        /// </summary>
        public float? CameraSize;

        internal static void _AddCameraTo(CameraTransform3D? _Camera, GLTFWriteSettings? settings, SharpGLTF.Scenes.SceneBuilder scene)
        {
            if (!_Camera.HasValue) return;            

            var vcam = _Camera.Value;

            var camNode = new SharpGLTF.Scenes.NodeBuilder("CameraNode");
            camNode.WorldMatrix = vcam.WorldMatrix;

            var cam = vcam;
            cam.WorldMatrix = Matrix4x4.Identity;
            vcam = cam;

            if (vcam.TryGetPerspectiveFieldOfView(out var vfov))
            {
                var persp = new SharpGLTF.Scenes.CameraBuilder.Perspective(null, vfov, 0.1f);

                scene.AddCamera(persp, camNode);
            }

            else if (vcam.TryGetOrthographicScale(out var oscale))
            {
                var ortho = new SharpGLTF.Scenes.CameraBuilder.Orthographic(oscale, oscale, 0.1f, 1000);

                scene.AddCamera(ortho, camNode);
            }

            if ((settings?.CameraSize ?? 0) > 0)
            {
                var camMesh = new GltfMeshScene3D();

                vcam.DrawCameraTo(camMesh, settings.Value.CameraSize.Value);
                vcam.DrawFustrumTo(camMesh, settings.Value.CameraSize.Value * 0.05f, System.Drawing.Color.Yellow);

                scene.AddRigidMesh(camMesh.Mesh, camNode);
            }

            if (Matrix4x4.Invert(_Camera.Value.AxisMatrix, out var invMatrix))
            {
                // scene.ApplyBasisTransform(invMatrix);
            }
        }
    }
}
