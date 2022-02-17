using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    class MonoGameDrawing3D :
        Decompose3D.PassThrough,
        IMonoGameDrawing3D
    {
        #region lifecycle

        public MonoGameDrawing3D(GraphicsDevice device, bool flipFaces = false)
        {
            _Batch = new MeshBuilder(flipFaces);
            SetPassThroughTarget(_Batch);
        }

        public void Dispose()
        {
            SetPassThroughTarget(null);
            Interlocked.Exchange(ref _Effect3D, null)?.Dispose();
            _Device = null;
        }

        #endregion

        #region data

        private GraphicsDevice _Device;
        private BasicEffect _Effect3D;

        private CameraTransform3D? _Camera;

        private readonly MeshBuilder _Batch;

        private readonly List<(object, Matrix4x4)> _AssetInstances = new List<(object, Matrix4x4)>();

        #endregion

        #region service provider

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(GetType())) return this;
            return null;
        }

        #endregion

        #region API - Windows

        public int PixelsWidth => _Device.Viewport.Width;

        public int PixelsHeight => _Device.Viewport.Height;

        public float DotsPerInchX => 96;

        public float DotsPerInchY => 96;

        #endregion

        #region properties

        public int Width => _Device.Viewport.Width;
        public int Height => _Device.Viewport.Height;

        public CameraTransform3D? Camera => _Camera;

        public bool IsEmpty => _Batch.IsEmpty && _AssetInstances.Count == 0;

        #endregion

        #region rendering

        public void Clear()
        {
            _AssetInstances.Clear();
            _Batch.Clear();
            _Camera = null;
        }

        public void SetCamera(CameraTransform3D camera) { _Camera = camera; }

        public void Render() { Render(Matrix4x4.Identity); }

        public void Render(Matrix4x4 sceneWorldMatrix)
        {
            try
            {
                _DrawBatch(sceneWorldMatrix);
            }
            finally
            {
                Clear();
            }
        }



        private void _DrawBatch(Matrix4x4 sceneWorldMatrix)
        {
            if (_Batch.IsEmpty) return;

            var effect = GetShader3D();

            ApplyTransformsTo(effect, sceneWorldMatrix);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _Batch.RenderTo(effect.GraphicsDevice);
            }
        }

        private void _DrawAsset(Matrix4x4 sceneWorldMatrix)
        {
            var (proj, view) = GetMatrices();

            foreach (var instance in _AssetInstances)
            {
                var mr = instance.Item1;

                var xform = instance.Item2 * sceneWorldMatrix;

                // foreach (var e in template.Effects.OfType<IEffectLights>()) e.EnableDefaultLighting();
                // inst.Draw(proj.ToXNA(), view.ToXNA(), xform.ToXNA());
            }
        }

        public void ApplyTransformsTo(Effect effect, Matrix4x4 sceneWorldMatrix)
        {
            if (!(effect is IEffectMatrices effectMatrices)) return;

            var (proj, view) = GetMatrices();

            effectMatrices.Projection = proj.ToXNA();
            effectMatrices.View = view.ToXNA();
            effectMatrices.World = sceneWorldMatrix.ToXNA();
        }

        private (Matrix4x4 proj, Matrix4x4 view) GetMatrices()
        {
            var aspectRatio = Width / (float)Height;

            var cam = _Camera.Value;

            if (!Matrix4x4.Invert(cam.WorldMatrix, out Matrix4x4 view)) throw new InvalidOperationException("invalid camera matrix");

            var proj = cam.CreateProjectionMatrix(aspectRatio);

            return (proj, view);
        }

        protected BasicEffect GetShader3D()
        {
            if (_Effect3D == null || _Effect3D.IsDisposed)
            {
                _Effect3D = new BasicEffect(_Device);
                _Effect3D.LightingEnabled = false;
                _Effect3D.TextureEnabled = false;
                _Effect3D.FogEnabled = false;
                _Effect3D.VertexColorEnabled = true;
            }

            return _Effect3D;
        }        

        #endregion
    }
}
