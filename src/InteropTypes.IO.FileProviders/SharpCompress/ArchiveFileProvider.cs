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
    {
        #region password management

        public static Func<ArchiveFileProvider, string> PasswordRetriever { get; set; }

        #endregion

        #region lifecycle

        public ArchiveFileProvider(string archivePath)
        {
            _Archive = SharpCompress.Archives.ArchiveFactory.Open(archivePath);
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

        #endregion
    }
}
