using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    public class VeldridDrawingFactory : IDisposable
    {
        #region lifecycle

        public VeldridDrawingFactory(GraphicsDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            GraphicsDevice = device;

            _SpriteEffect = _Disposables.Record(new SpriteEffect(GraphicsDevice));
            _SolidEffect = _Disposables.Record(new SolidEffect(GraphicsDevice));

            // We lazily create and cache texture views and resource sets when required.
            // When a texture is destroyed the matching cached values are destroyed as well.
            // _textureStorage.TextureDestroyed += (s, a) => RemoveTextureResourceSet(a.TextureId);
            _Textures = _Disposables.Record(new _VeldridTextureCollection(device));
        }        

        public void Dispose()
        {
            if (GraphicsDevice != null)
            {
                this.GraphicsDevice.WaitForIdle();                            

                _Disposables.Dispose();

                _Context2DPool = null;
                _Context3DPool = null;

                GraphicsDevice = null;
            }
        }

        #endregion

        #region data

        public GraphicsDevice GraphicsDevice { get; private set; }

        internal readonly _DisposablesRecorder _Disposables = new _DisposablesRecorder();

        private SpriteEffect _SpriteEffect;
        private SolidEffect _SolidEffect;
        private _VeldridTextureCollection _Textures;        

        private readonly Dictionary<Object, (int, Vector2)> _SpriteTextures = new Dictionary<object, (int, Vector2)>();

        private System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing2DContext> _Context2DPool = new System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing2DContext>();
        private System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing3DContext> _Context3DPool = new System.Collections.Concurrent.ConcurrentBag<_VeldridDrawing3DContext>();

        #endregion

        #region API

        internal SpriteEffect SpriteEffect => _SpriteEffect;
        internal SolidEffect SolidEffect => _SolidEffect;

        public IVeldridDrawingContext2D CreateDrawing2DContext(Framebuffer dstFrame)
        {
            if (!_Context2DPool.TryTake(out var dc))
            {
                // if there's no context available in the pool,
                // we create a new one (which will be stored in the pool layer)
                dc = new _VeldridDrawing2DContext(this);
            }

            dc.Initialize(dstFrame);
            return dc;
        }        

        public IVeldridDrawingContext3D CreateDrawing3DContext(Framebuffer dstFrame, Matrix4x4 view, Matrix4x4 projection)
        {
            if (!_Context3DPool.TryTake(out var dc)) dc = new _VeldridDrawing3DContext(this);

            dc.Initialize(dstFrame, view,  size => projection);
            return dc;
        }

        public IVeldridDrawingContext3D CreateDrawing3DContext(Framebuffer dstFrame, Matrix4x4 view, float fov, (float Near, float Far) depthPlanes)
        {
            if (!_Context3DPool.TryTake(out var dc)) dc = new _VeldridDrawing3DContext(this);

            Matrix4x4 projFunc(Vector2 size)
            {
                return Matrix4x4.CreatePerspectiveFieldOfView(fov, size.X / size.Y, depthPlanes.Near, depthPlanes.Far);
            };

            dc.Initialize(dstFrame, view, projFunc);

            return dc;
        }

        internal void _Return(_VeldridDrawing2DContext dc) { _Context2DPool.Add(dc); }

        internal void _Return(_VeldridDrawing3DContext dc) { _Context3DPool.Add(dc); }

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
                var tmp = MemoryBitmap.Load(filePath,Codecs.GDICodec.Default);

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
