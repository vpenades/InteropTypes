using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using SharpSvn;

namespace InteropTypes.IO.VersionControl
{
    /// <summary>
    /// Loads data as navigation goes on. Refresh can be done on a per directory bases.
    /// </summary>
    sealed class _DynamicRepoReader : IFileProvider
    {
        #region lifecycle

        public _DynamicRepoReader(SvnClient client, SvnTarget target)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(target);

            Client = client;
            _Target = target;
        }

        #endregion

        #region data

        public SvnClient Client { get; protected set; }

        private SvnTarget _Target;

        #endregion

        #region API
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new _DirectoryContents(Client, _Target.Concat(subpath));
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var target = _Target.Concat(subpath);

            return SVNEntryInfo.Create(Client, target) as IFileInfo
                ?? new Microsoft.Extensions.FileProviders.NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion
    }

    /// <summary>
    /// Preloads a snapshot of the entire tree. Refresh is done on the entire tree.
    /// </summary>
    sealed class _StaticRepoReader : IFileProvider
    {
        #region lifecycle

        public _StaticRepoReader(SvnClient client, SvnTarget target)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(target);

            _Client = client;
            _Target = target;
            Refresh();
        }

        private static Collections.ReadOnlyDirectoryContents _ReadTree(SvnClient client, SvnTarget target)
        {
            var list = client.FindFilesAndDirectories(target, System.IO.SearchOption.AllDirectories).ToList();

            var entries = list
                .Where(item => item.Entry.NodeKind == SvnNodeKind.File)
                .ToDictionary(entry => entry.Path, entry => (IFileInfo)new _SVNClientFileInfo(client, entry.Path, entry), StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase));

            return Collections.ReadOnlyDirectoryContents.MakeTree(entries, string.Empty, StringComparison.Ordinal);
        }

        #endregion

        #region data

        private SvnClient _Client;
        private SvnTarget _Target;
        private Collections.ReadOnlyDirectoryContents _Tree;

        #endregion

        #region API

        public void Refresh()
        {
            _Tree = _ReadTree(_Client, _Target);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _Tree.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _Tree.GetFileInfo(subpath);
        }

        IChangeToken IFileProvider.Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion
    }
}
