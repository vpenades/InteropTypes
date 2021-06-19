using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

namespace InteropWith
{
    public class _VeldridTextureFactory : IDisposable
    {
        #region lifecycle        
        public _VeldridTextureFactory(GraphicsDevice gd)
        {
            if (gd == null)
                throw new ArgumentNullException(nameof(gd));

            _gd = gd;
            _setDataCommandList = _gd.ResourceFactory.CreateCommandList();

            _freeIds = new List<int>(InitialTextureCount);
            _freeIds.AddRange(Enumerable.Range(0, InitialTextureCount));
            _textures = new Texture[InitialTextureCount];
        }

        

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var t in _textures)
                        t?.Dispose();

                    _setDataCommandList.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose() => Dispose(true);

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("Cannot use this texture storage after it has been disposed.");
        }

        #endregion

        #region data

        private bool _disposed = false;

        public const int InitialTextureCount = 64;

        private GraphicsDevice _gd;

        private int _textureCount;        

        private CommandList _setDataCommandList;
        private Texture _staging;

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
            var bpp = tex.Format == PixelFormat.R8_UNorm ? 1 : 4;
            if (data.Length * Marshal.SizeOf<T>() != tex.Width * tex.Height * bpp)
                throw new ArgumentException($"Length of data (${data.Length * Marshal.SizeOf<T>()}) did not match width * height of the texture (${tex.Width * tex.Height * bpp}).", nameof(data));

            var byteSpan = MemoryMarshal.Cast<T, byte>(data);
            _CopyData(tex, byteSpan, (uint)subRect.X, (uint)subRect.Y, (uint)subRect.Width, (uint)subRect.Height);
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

        private unsafe void _CopyData(Texture tex, ReadOnlySpan<byte> data, uint tx, uint ty, uint width, uint height)
        {
            var staging = _GetStaging(width, height);

            _setDataCommandList.Begin();

            var bpp = tex.Format == PixelFormat.R8_UNorm ? 1 : 4;
            var rowBytes = (int)width * bpp;

            var map = _gd.Map(staging, MapMode.Write, 0);
            if (rowBytes == map.RowPitch)
            {
                var dst = new Span<byte>((byte*)map.Data.ToPointer() + rowBytes * ty, (int)(rowBytes * height));
                data.CopyTo(dst);
            }
            else
            {
                var stagingPtr = (byte*)map.Data.ToPointer();
                for (var y = 0; y < height; y++)
                {
                    var dst = new Span<byte>(stagingPtr + y * (int)map.RowPitch, rowBytes);
                    var src = data.Slice(y * rowBytes, rowBytes);
                    src.CopyTo(dst);
                }
            }

            _gd.Unmap(staging);

            _setDataCommandList.CopyTexture(staging, 0, 0, 0, 0, 0, tex, tx, ty, 0, 0, 0, width, height, 1, 1);
            _setDataCommandList.End();

            _gd.SubmitCommands(_setDataCommandList);
        }

        private Texture _GetStaging(uint width, uint height)
        {
            if (_staging == null)
            {
                var td = TextureDescription.Texture2D(width, height, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Staging);
                _staging = _gd.ResourceFactory.CreateTexture(ref td);
            }
            else if (_staging.Width < width || _staging.Height < height)
            {
                var newWidth = width > _staging.Width ? width : _staging.Width;
                var newHeight = height > _staging.Height ? height : _staging.Height;
                var td = TextureDescription.Texture2D(newWidth, newHeight, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Staging);
                _gd.DisposeWhenIdle(_staging);
                _staging = _gd.ResourceFactory.CreateTexture(ref td);
            }

            return _staging;
        }

        #endregion
    }
}
