using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropBitmaps;

using InteropDrawing;
using Veldrid;

namespace InteropWith
{
    public class VeldridDrawingFactory : IDisposable
    {
        #region lifecycle

        public VeldridDrawingFactory(GraphicsDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            GraphicsDevice = device;

            CreateResources();
            
            // We lazily create and cache texture views and resource sets when required.
            // When a texture is destroyed the matching cached values are destroyed as well.
            // _textureStorage.TextureDestroyed += (s, a) => RemoveTextureResourceSet(a.TextureId);
            _Textures = new _VeldridTextureCollection(device);
        }

        private void CreateResources()
        {
            _CommandList = _Disposables.Record(GraphicsDevice.ResourceFactory.CreateCommandList());
            _SpriteEffect = _Disposables.Record(new SpriteEffect(GraphicsDevice));
            _IndexedVertexBuffer = _Disposables.Record(new _IndexedVertexBuffer(GraphicsDevice));
        }

        public void Dispose()
        {
            _Textures?.Dispose();
            _Textures = null;

            _Disposables.Dispose();
        }

        #endregion

        #region data

        public GraphicsDevice GraphicsDevice { get; }

        private readonly _DisposablesRecorder _Disposables = new _DisposablesRecorder();

        private SpriteEffect _SpriteEffect;
        private _VeldridTextureCollection _Textures;

        private CommandList _CommandList;        
        private _IndexedVertexBuffer _IndexedVertexBuffer;        

        private readonly Dictionary<Object, (int, Vector2)> _SpriteTextures = new Dictionary<object, (int, Vector2)>();

        private readonly System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing2DContext> _ContextPool = new System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing2DContext>();

        #endregion

        #region API

        internal SpriteEffect CurrentEffect => _SpriteEffect;        

        public IDrawingContext2D CreateDrawing2DContext(Framebuffer dstFrame, System.Drawing.Color? fillColor)
        {
            if (!_ContextPool.TryTake(out var dc)) dc = new _VeldridDrawing2DContext(this);

            dc.Initialize(dstFrame, fillColor);
            return dc;
        }

        internal void Draw(_VeldridDrawing2DContext primitives)
        {
            primitives.CopyTo(_IndexedVertexBuffer);

            _CommandList.Begin();

            primitives.DrawTo(_CommandList, _IndexedVertexBuffer);

            _CommandList.End();
            GraphicsDevice.SubmitCommands(_CommandList);
        }

        internal void _ReturnDrawing2DContext(_VeldridDrawing2DContext dc)
        {
            _ContextPool.Add(dc);
        }

        internal (int, Vector2) _GetTexture(Object textureSource)
        {
            if (textureSource == null) textureSource = System.Drawing.Color.White;

            if (_SpriteTextures.TryGetValue(textureSource, out var texInfo)) return texInfo;

            MemoryBitmap bmp = default;

            if (textureSource is System.Drawing.Color color)
            {
                bmp = new MemoryBitmap(16, 16, Pixel.RGBA32.Format);
                bmp.AsSpanBitmap().SetPixels(color);
            }

            if (textureSource is string filePath)
            {
                var tmp = MemoryBitmap.Load(filePath, InteropBitmaps.Codecs.GDICodec.Default);

                bmp = new MemoryBitmap(tmp.Width, tmp.Height, Pixel.RGBA32.Format);
                bmp.SetPixels(0, 0, tmp);
            }

            if (bmp.IsEmpty) throw new NotImplementedException();

            var idx = _Textures.CreateTexture(bmp);

            texInfo = (idx, new Vector2(bmp.Width, bmp.Height));

            _SpriteTextures[textureSource] = texInfo;
            return texInfo;
        }

        internal void _SetTexture(CommandList cmd, int idx)
        {
            if (idx < 0) idx = 0;

            var t = _Textures.GetTextureView(_Textures.GetTexture(idx));
            _SpriteEffect.SetTexture(cmd, t);
        }

        #endregion
    }    
}
