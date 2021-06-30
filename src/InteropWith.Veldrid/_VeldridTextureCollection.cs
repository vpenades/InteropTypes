using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

namespace InteropWith
{
    public class _VeldridTextureCollection : IDisposable
    {
        #region lifecycle        
        public _VeldridTextureCollection(GraphicsDevice gd)
        {
            if (gd == null) throw new ArgumentNullException(nameof(gd));

            _gd = gd;
            _Factory = new _VeldridTextureFactory(gd);

            _freeIds = new List<int>(InitialTextureCount);
            _freeIds.AddRange(Enumerable.Range(0, InitialTextureCount));
            _textures = new Texture[InitialTextureCount];
        }        

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing)
            {
                for(int i=0; i < _textureCount; ++i)
                {
                    _textures[i].Dispose();
                }

                _textureCount = 0;
                _textures = null;                

                _Factory?.Dispose();
                _Factory = null;

                _gd = null;
            }            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void CheckDisposed()
        {
            if (_gd == null) throw new ObjectDisposedException("Cannot use this texture storage after it has been disposed.");
        }

        #endregion

        #region data        

        private _VeldridTextureFactory _Factory;

        public const int InitialTextureCount = 64;

        private GraphicsDevice _gd;

        private int _textureCount;        

        private readonly List<int> _freeIds;
        private Texture[] _textures;

        private Dictionary<Texture, TextureView> _TextureViews = new Dictionary<Texture, TextureView>();

        #endregion

        #region API

        public int TextureCount => _textureCount;        

        /// <inheritdoc/>
        /// <remarks>
        /// Created textures are 2D, non-multisampled, non-mipmapped textures with RGBA8 UNorm format and
        /// texture usage <see cref="TextureUsage.Sampled"/>.
        /// </remarks>
        public int CreateTexture(int width, int height, PixelFormat pFormat)
        {
            CheckDisposed();            

            var textureDescr = TextureDescription.Texture2D((uint)width, (uint)height, 1, 1, pFormat, TextureUsage.Sampled);
            var tex = _gd.ResourceFactory.CreateTexture(ref textureDescr);
            return _AddTexture(tex);
        }        

        public static PixelFormat From(InteropBitmaps.Pixel.Format fmt)
        {
            switch(fmt.PackedFormat)
            {
                case InteropBitmaps.Pixel.RGBA32.Code: return PixelFormat.R8_G8_B8_A8_UNorm;
                case InteropBitmaps.Pixel.BGRA32.Code: return PixelFormat.B8_G8_R8_A8_UNorm;                
                default: throw new NotImplementedException(nameof(fmt));
            }
        }

        public int CreateTexture(InteropBitmaps.SpanBitmap bitmap)
        {
            var dstFmt = From(bitmap.PixelFormat);

            var textureDescr = TextureDescription.Texture2D((uint)bitmap.Width, (uint)bitmap.Height, 1, 1, dstFmt, TextureUsage.Sampled);
            var tex = _gd.ResourceFactory.CreateTexture(ref textureDescr);
            return _AddTexture(tex);
        }

        
        public void DestroyTexture(int id)
        {
            CheckDisposed();

            if (_textures[id] == null)
                return;

            _textures[id] = null;
            _freeIds.Insert(~_freeIds.BinarySearch(id), id);
            _textureCount--;            
        }

        
        public bool HasTexture(int id)
        {
            CheckDisposed();
            return id < _textures.Length && _textures[id] != null;
        }

        
        public Texture GetTexture(int id)
        {
            CheckDisposed();
            return _textures[id];
        }

        public TextureView GetTextureView(Texture texture)
        {
            if (_TextureViews.TryGetValue(texture, out var view)) return view;
            view = _gd.ResourceFactory.CreateTextureView(texture);
            _TextureViews[texture] = view;
            return view;
        }

        
        public Size GetTextureSize(int id)
        {
            CheckDisposed();

            var tex = _textures[id];
            return new Size((int)tex.Width, (int)tex.Height);
        }

        
        public PixelFormat GetTextureFormat(int id)
        {
            CheckDisposed();
            var tex = _textures[id];
            return tex.Format;
        }

        
        public void SetData<T>(int id, in Rectangle subRect, ReadOnlySpan<T> data)
            where T:unmanaged
        {
            CheckDisposed();

            var tex = _textures[id];
            _Factory.SetData<T>(tex, subRect, data);
        }        

        private void _Grow()
        {
            _freeIds.AddRange(Enumerable.Range(_textures.Length, _textures.Length));
            Array.Resize(ref _textures, _textures.Length * 2);
        }

        private int _AddTexture(Texture texture)
        {
            if (_freeIds.Count == 0) _Grow();

            var id = _freeIds[0];
            _freeIds.RemoveAt(0);
            _textures[id] = texture;

            _textureCount++;            

            return id;
        }        

        #endregion
    }
}
