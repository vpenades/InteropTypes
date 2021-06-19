using System;
using System.Collections.Generic;
using System.Numerics;

using Veldrid;

namespace InteropWith
{
    // based on https://github.com/Jjagg/OpenWheels/blob/master/src/OpenWheels.Veldrid/VeldridRenderer.cs

    public class _VeldridGraphicsContext : IDisposable
    {
        #region lifecycle

        public _VeldridGraphicsContext(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException(nameof(graphicsDevice));

            GraphicsDevice = graphicsDevice;
            _currentTarget = GraphicsDevice.SwapchainFramebuffer;

            CreateResources();

            _pipelines = new Dictionary<OutputDescription, Pipeline>();

            // _textureStorage = textureStorage;
            // _textureViews = new TextureView[32];
            // _textureResourceSets = new ResourceSet[32];
            // We lazily create and cache texture views and resource sets when required.
            // When a texture is destroyed the matching cached values are destroyed as well.
            // _textureStorage.TextureDestroyed += (s, a) => RemoveTextureResourceSet(a.TextureId);
        }

        private void CreateResources()
        {
            _commandList = _Disposables.Record(GraphicsDevice.ResourceFactory.CreateCommandList());
            _spriteEffect = _Disposables.Record(new SpriteEffect(GraphicsDevice));
            _ivBuffer = _Disposables.Record(new IndexedVertexBuffer(GraphicsDevice));

            UpdateWvp();            
        }        

        public void Dispose()
        {
            
        }

        #endregion

        #region data

        public GraphicsDevice GraphicsDevice { get; }

        private readonly DisposablesRecorder _Disposables = new DisposablesRecorder();

        private readonly Dictionary<OutputDescription, Pipeline> _pipelines;        

        private Framebuffer _currentTarget;

        private CommandList _commandList;        

        private SpriteEffect _spriteEffect;
        private IndexedVertexBuffer _ivBuffer;

        #endregion

        #region API
        
        public void SetTarget()
        {
            SetTarget(GraphicsDevice.SwapchainFramebuffer);
        }

        /// <summary>
        /// Set the render target.
        /// </summary>
        /// <param name="target">The framebuffer to render to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="target"/> is <c>null</c>.</exception>
        public void SetTarget(Framebuffer target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            _currentTarget = target;
        }

        public void UpdateWvp() { UpdateWvp((int)_currentTarget.Width, (int)_currentTarget.Height); }

        private void UpdateWvp(int width, int height)
        {
            var wvp = Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
            _spriteEffect.SetWorldMatrix(wvp);
        }

        public SpriteEffect CurrentEffect => _spriteEffect;

        public void Clear(System.Drawing.Color color)
        {
            _commandList.Begin();

            _commandList.SetFramebuffer(_currentTarget);
            var c = new RgbaFloat(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            _commandList.ClearColorTarget(0, c);
            _commandList.End();

            GraphicsDevice.SubmitCommands(_commandList);
        }

        public void BeginRender(Span<Vertex> vertexBuffer, Span<int> indexBuffer, int vertexCount, int indexCount)
        {
            _ivBuffer.SetData(vertexBuffer, indexBuffer, vertexCount, indexCount);

            // Begin() must be called before commands can be issued.
            _commandList.Begin();

            _commandList.SetFramebuffer(_currentTarget);            
        }       

        
        public void DrawBatch(int startIndex, int indexCount)
        {
            var outputDescr = _currentTarget.OutputDescription;
            var pipeline = UsePipeline(outputDescr);
            _commandList.SetPipeline(pipeline);

            _spriteEffect.Bind(_commandList);

            _ivBuffer.Bind(_commandList);
            _ivBuffer.DrawIndexed(_commandList, startIndex, indexCount);
        }

        public void EndRender()
        {
            // End() must be called before commands can be submitted for execution.
            _commandList.End();
            GraphicsDevice.SubmitCommands(_commandList);
        }

        private Pipeline UsePipeline(in OutputDescription outputDescr)
        {
            if (_pipelines.TryGetValue(outputDescr, out var pipeline)) return pipeline;

            var gpd = _spriteEffect.GetPipelineDesc(outputDescr);

            pipeline = _Disposables.Record(GraphicsDevice.ResourceFactory.CreateGraphicsPipeline(gpd));
            _pipelines[outputDescr] = pipeline;
            return pipeline;
        }


        #endregion

    }
}
