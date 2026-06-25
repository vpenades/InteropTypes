using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XFILE = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO.Mvvm
{
    public class FileThumbnailServerCache<TImage> : IFileThumbnailServer<TImage>
    {
        #region lifecycle
        public FileThumbnailServerCache(IFileThumbnailServer<TImage> server)
        {
            _Server = server;
        }

        #endregion

        #region data        

        private readonly IFileThumbnailServer<TImage> _Server;

        private readonly ConcurrentDictionary<string, Task<TImage>> _PathsCache = new ConcurrentDictionary<string, Task<TImage>>();

        #endregion

        #region API      

        public void ReleaseCachedImages()
        {
            foreach (var key in _PathsCache.Keys)
            {
                if (_PathsCache.TryRemove(key, out var value))
                {
                    if (value is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public async Task<bool> UpdateClientAsync(IFileThumbnailClient<TImage> item)
        {
            ArgumentNullException.ThrowIfNull(item);

            var info = item.GetSource();
            if (info == null) return false;

            var image = await GetImageAsync(info);
            if (image == null) return false;

            item.SetImage(image);

            return true;
        }

        public async Task<TImage> GetImageAsync(XFILE key)
        {
            return await _GetOrAddImageAsync(key, _LoadImageAsync);
        }

        private async Task<TImage> _GetOrAddImageAsync(XFILE key, Func<XFILE, Task<TImage>> eval)
        {
            if (_TryGetPath(key, out var pkey))
            {
                return await _PathsCache.GetOrAdd(pkey, _ => eval(key));
            }

            return default;
        }

        private static bool _TryGetPath(XFILE file, out string path)
        {
            path = file.PhysicalPath;
            return !string.IsNullOrWhiteSpace(path);
        }

        private async Task<TImage> _LoadImageAsync(XFILE xinfo)
        {
            var cw = new _ClientWrap(xinfo);

            if (!await _Server.UpdateClientAsync(cw)) return default;            

            return cw.Image;
        }

        #endregion

        #region nested types
        sealed class _ClientWrap : IFileThumbnailClient<TImage>
        {
            public _ClientWrap(XFILE client) { _Client = client; }

            private readonly XFILE _Client;
            public TImage Image { get; private set; }
            public XFILE GetSource() { return _Client; }
            public void SetImage(TImage image) { Image = image; }
        }

        #endregion
    }
}
