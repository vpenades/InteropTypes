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

        public _DynamicRepoReader(SvnClient client, SvnTarget repositoryUrl, SvnRevision revision)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(repositoryUrl);

            Client = client;
            _Target = repositoryUrl;
            _Revision = revision;
        }

        #endregion

        #region data

        public SvnClient Client { get; }

        private SvnTarget _Target;
        private SvnRevision _Revision;

        #endregion

        #region API
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new _DirectoryContents(Client, _Target.Concat(subpath), _Revision);
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
}
