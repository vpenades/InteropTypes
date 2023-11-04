using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO.Archives.Primitives
{
    /// <summary>
    /// Represents an archive provider for a flattened list of entries.
    /// </summary>
    /// <typeparam name="TEntry"></typeparam>
    public abstract class ArchiveFileProviderBase<TEntry> : IFileProvider, IServiceProvider
    {
        #region lifecycle
        protected ArchiveFileProviderBase(IArchiveAccessor<TEntry> accessor)
        {
            var entries = accessor
                .GetAllEntries()
                .Where(item => !accessor.GetEntryIsDirectory(item))
                .ToDictionary(accessor.GetEntryKey, entry => ArchiveFileInfo<TEntry>.Create(accessor, entry), StringComparer.FromComparison(StringComparison.Ordinal));

            _Root = Collections.ReadOnlyDirectoryContents.MakeTree(entries, string.Empty, StringComparison.Ordinal);
        }

        #endregion

        #region data

        private readonly Collections.ReadOnlyDirectoryContents _Root;

        #endregion

        #region API

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _Root.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _Root.GetFileInfo(subpath);
        }        

        IChangeToken IFileProvider.Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        public virtual object GetService(Type serviceType) { return null; }

        #endregion
    }
}
