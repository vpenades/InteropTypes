using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using Veldrid;

namespace InteropWith
{
    public class SpriteEffect : IDisposable
    {
        #region lifecycle
        public SpriteEffect(GraphicsDevice gdev)
        {
            _Device = gdev;

            var rf = gdev.ResourceFactory;

            var vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float3),
                new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Byte4_Norm),
                new VertexElementDescription("TextureCoordinate", VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float2));

            var vertexShader = _Disposables.Record(VeldridHelper.LoadShader(rf, "SpriteShader", ShaderStages.Vertex, "VS"));
            var fragmentShader = _Disposables.Record(VeldridHelper.LoadShader(rf, "SpriteShader", ShaderStages.Fragment, "FS"));

            _ShaderSet = new ShaderSetDescription(
                new[] { vertexLayout },
                new[] { vertexShader, fragmentShader });

            // Wvp
            _WorldMatrix = _Disposables.Record(new _EffectUniform(gdev, "Wvp", ShaderStages.Vertex, 64));
            _PrimaryTexture = _Disposables.Record(new _EffectTexture(gdev, "Input", "Sampler"));            

            _ResourceLayouts = new[] { _WorldMatrix, _PrimaryTexture._OwnedTextureLayout, _PrimaryTexture._OwnedSamplerLayout };            
        }

        public void Dispose() { _Disposables.Dispose(); }

        #endregion

        #region data

        private GraphicsDevice _Device;

        private readonly DisposablesRecorder _Disposables = new DisposablesRecorder();

        private ShaderSetDescription _ShaderSet;

        private ResourceLayout[] _ResourceLayouts;

        private _EffectUniform _WorldMatrix;

        private _EffectTexture _PrimaryTexture;

        #endregion        

        #region API

        public GraphicsPipelineDescription GetPipelineDesc(in OutputDescription outputDescr)
        {
            var gpd = new GraphicsPipelineDescription();
            gpd.BlendState = BlendStateDescription.SingleAlphaBlend;
            gpd.DepthStencilState = DepthStencilStateDescription.Disabled;
            gpd.RasterizerState = new RasterizerStateDescription(FaceCullMode.None, PolygonFillMode.Solid, FrontFace.Clockwise, false, false);
            gpd.PrimitiveTopology = PrimitiveTopology.TriangleList;
            gpd.ShaderSet = _ShaderSet;
            gpd.ResourceLayouts = _ResourceLayouts;
            gpd.Outputs = outputDescr;

            return gpd;
        }

        public void SetWorldMatrix(Matrix4x4 wmatrix)
        {
            _WorldMatrix.Update(ref wmatrix);
        }

        public void SetTexture(TextureView texture)
        {
            _PrimaryTexture.SetTexture(texture);
            _PrimaryTexture.SetSampler(0, false);
        }

        public void Bind(CommandList cmdList)
        {
            _WorldMatrix.Bind(cmdList, 0);
            _PrimaryTexture.Bind(cmdList, 1, 2);
        }

        #endregion
    }
}
