using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using Veldrid;

namespace InteropWith
{
    public interface IEffectInput
    {
        string Name { get; }
        ResourceLayout GetResourceLayout();
        void Bind(CommandList cmdList, uint resourceIndex);
    }

    public abstract class BaseEffect : IDisposable
    {
        #region lifecycle

        public BaseEffect(GraphicsDevice gdev)
        {
            _Device = gdev;
        }

        public virtual void Dispose()
        {
            _Disposables.Dispose();

            foreach (var p in _Pipelines.Values) p.Dispose();
            _Pipelines.Clear();

            _Device = null;            
        }

        #endregion

        #region data

        private readonly Dictionary<OutputDescription, Pipeline> _Pipelines = new Dictionary<OutputDescription, Pipeline>();

        internal readonly _DisposablesRecorder _Disposables = new _DisposablesRecorder();

        private GraphicsDevice _Device;

        #endregion

        #region API

        public Pipeline UsePipeline(in OutputDescription outputDescr)
        {
            if (_Pipelines.TryGetValue(outputDescr, out var pipeline)) return pipeline;

            var gpd = GetPipelineDesc(outputDescr);

            pipeline = _Device.ResourceFactory.CreateGraphicsPipeline(ref gpd);
            
            _Pipelines[outputDescr] = pipeline;
            return pipeline;
        }

        protected abstract GraphicsPipelineDescription GetPipelineDesc(in OutputDescription outputDescr);

        #endregion
    }

    public class SpriteEffect : BaseEffect
    {
        #region lifecycle
        public SpriteEffect(GraphicsDevice gdev) : base(gdev)
        {
            // uniforms

            _WorldMatrix = _Disposables.Record(new _EffectUniform<Matrix4x4>(gdev, "WorldViewProjection", ShaderStages.Vertex));
            _PrimaryTexture = _Disposables.Record(new _EffectTexture(gdev, "InputTexture"));
            _PrimarySampler = _Disposables.Record(new _EffectSampler(gdev, "InputSampler"));

            // shaders
            
            _SolidShader = _Disposables.Record(new _EffectShader(VeldridShaders.SpriteShader.CreateShaderSolid(gdev.ResourceFactory), _WorldMatrix));
            _TexturedShader = _Disposables.Record(new _EffectShader(VeldridShaders.SpriteShader.CreateShaderTextured(gdev.ResourceFactory), _WorldMatrix, _PrimaryTexture, _PrimarySampler));
        }        

        #endregion

        #region data        

        private _EffectShader _SolidShader;
        private _EffectShader _TexturedShader;        

        private _EffectUniform<Matrix4x4> _WorldMatrix;
        private _EffectTexture _PrimaryTexture;
        private _EffectSampler _PrimarySampler;

        #endregion        

        #region API

        protected override GraphicsPipelineDescription GetPipelineDesc(in OutputDescription outputDescr)
        {
            var rasterizer = new RasterizerStateDescription(FaceCullMode.None, PolygonFillMode.Solid, FrontFace.Clockwise, false, false);
            
            var gpd = new GraphicsPipelineDescription();
            gpd.BlendState = BlendStateDescription.SingleAlphaBlend;            
            gpd.DepthStencilState = DepthStencilStateDescription.Disabled;
            gpd.RasterizerState = rasterizer;
            gpd.PrimitiveTopology = PrimitiveTopology.TriangleList;            
            gpd.Outputs = outputDescr;

            _TexturedShader.FillPipelineDesc(ref gpd);

            return gpd;
        }

        public void SetWorldMatrix(Matrix4x4 wmatrix)
        {
            _WorldMatrix.Update(ref wmatrix);
        }

        public void SetTexture(TextureView texture)
        {
            _PrimaryTexture.SetTexture(texture);
            _PrimarySampler.SetSampler(0, false);
        }        

        public void Bind(CommandList cmdList, in OutputDescription outDesc)
        {
            var pipeline = UsePipeline(outDesc);

            cmdList.SetPipeline(pipeline);

            _TexturedShader.BindInputs(cmdList);
        }

        public void SetTexture(CommandList cmdList, TextureView texture)
        {
            SetTexture(texture);

            _PrimaryTexture.Bind(cmdList, 1);
        }

        #endregion
    }

    public class SolidEffect : BaseEffect
    {
        #region lifecycle
        public SolidEffect(GraphicsDevice gdev) : base(gdev)
        {
            // uniforms

            _ViewMatrix = _Disposables.Record(new _EffectUniform<Matrix4x4>(gdev, "XformView", ShaderStages.Vertex));
            _ProjMatrix = _Disposables.Record(new _EffectUniform<Matrix4x4>(gdev, "XformProj", ShaderStages.Vertex));

            // shader

            _Shader = _Disposables.Record(new _EffectShader(VeldridShaders.SolidShader.CreateShader(gdev.ResourceFactory), _ViewMatrix, _ProjMatrix));            
        }

        #endregion

        #region data        

        private _EffectShader _Shader;        

        private _EffectUniform<Matrix4x4> _ViewMatrix;
        private _EffectUniform<Matrix4x4> _ProjMatrix;

        #endregion        

        #region API

        protected override GraphicsPipelineDescription GetPipelineDesc(in OutputDescription outputDescr)
        {
            var rasterizer = RasterizerStateDescription.Default;

            var gpd = new GraphicsPipelineDescription();
            gpd.BlendState = BlendStateDescription.SingleOverrideBlend;
            gpd.DepthStencilState = DepthStencilStateDescription.DepthOnlyLessEqual;
            gpd.RasterizerState = rasterizer;
            gpd.PrimitiveTopology = PrimitiveTopology.TriangleList;
            gpd.Outputs = outputDescr;

            _Shader.FillPipelineDesc(ref gpd);

            return gpd;
        }

        public void SetViewMatrix(Matrix4x4 matrix) { _ViewMatrix.Update(ref matrix); }

        public void SetProjMatrix(Matrix4x4 matrix) { _ProjMatrix.Update(ref matrix); }

        public void Bind(CommandList cmdList, in OutputDescription outDesc)
        {
            var pipeline = UsePipeline(outDesc);

            cmdList.SetPipeline(pipeline);

            _Shader.BindInputs(cmdList);
        }        

        #endregion
    }
}
