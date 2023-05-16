using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO
{
    public class ArchiveFileProvider 
        : IFileProvider
        , IDisposable
        , IServiceProvider
    {
        #region password management        

        public static Func<ArchiveFileProvider, string> PasswordRetriever { get; set; }

        #endregion

        #region lifecycle

        public static ArchiveFileProvider Create(string archivePath)
        {
            var arch = SharpCompress.Archives.ArchiveFactory.Open(archivePath);
            return new ArchiveFileProvider(arch);
        }

        private ArchiveFileProvider(SharpCompress.Archives.IArchive archive)
        {
            _Archive = archive;
        }

        public void Dispose()
        {
            _Archive?.Dispose();
            _Archive = null;
        }

        #endregion

        #region data

        private SharpCompress.Archives.IArchive _Archive;

        #endregion

        #region API

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new ArchiveDirectoryInfo(_Archive, subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            foreach(var entry in _Archive.Entries)
            {
                if (entry.Key == subpath)
                {
                    if (!entry.IsDirectory) return new ArchiveFileInfo(entry);
                }
            }

            return new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotSupportedException();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(SharpCompress.Archives.IArchive)) return _Archive;            

            return null;
        }

        #endregion
    }
}
