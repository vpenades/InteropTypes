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
    /// Preloads a snapshot of the entire tree in a single request. Refresh is done on the entire tree.
    /// </summary>
    sealed class _StaticRepoReader : IFileProvider
    {
        #region lifecycle

        public _StaticRepoReader(SvnClient client, SvnTarget repositoryUrl, SvnRevision revision)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(repositoryUrl);

            _Client = client;
            _Target = repositoryUrl;
            _Revision = revision;

            Refresh();
        }
        
        #endregion

        #region data

        private SvnClient _Client;
        private SvnTarget _Target;
        private SvnRevision _Revision;

        private Collections.ReadOnlyDirectoryContents _Tree;

        #endregion

        #region API

        public void Refresh()
        {
            _Tree = _ReadTree(_Client, _Target, _Revision);
        }

        private static Collections.ReadOnlyDirectoryContents _ReadTree(SvnClient client, SvnTarget repositoryUrl, SvnRevision revision)
        {
            var tree = client
                .FindFilesAndDirectories(repositoryUrl, System.IO.SearchOption.AllDirectories, revision)
                .ToList();

            var comparer = StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase);

            IFileInfo factory(SvnListEventArgs args)
            {
                var url = repositoryUrl.Concat(args.Path);

                return new _SVNClientFileInfo(client, url, args);
            }

            var entries = tree
                .Where(item => item.Entry.NodeKind == SvnNodeKind.File)
                .ToDictionary(entry => entry.Path, factory, comparer);

            return Collections.ReadOnlyDirectoryContents.MakeTree(entries, string.Empty, StringComparison.Ordinal);
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
